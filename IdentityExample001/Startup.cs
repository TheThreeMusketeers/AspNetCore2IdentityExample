using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using IdentityExample001.Persistence;
using Microsoft.EntityFrameworkCore;
using IdentityExample001.Core.Models;
using Microsoft.AspNetCore.Identity;
using System.Threading;
using AspNet.Security.OpenIdConnect.Primitives;
using OpenIddict.Core;
using OpenIddict.Models;
using AspNet.Security.OAuth.Validation;
using IdentityExample001.Services;

namespace IdentityExample001
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //Add Db Context
            services.AddDbContext<AppDbContext>(options => 
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict<Guid>();
            });

            services.AddIdentity<UserEntity, UserRoleEntity>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            // Register the validation handler, that is used to decrypt the tokens.
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = OAuthValidationDefaults.AuthenticationScheme;
            })
            .AddOAuthValidation();

            //map some of the default claims names to OpenId claim names
            services.Configure<IdentityOptions>(opt=> 
            {
                opt.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                opt.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                opt.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;

            });



            // Register the OpenIddict services.
            services.AddOpenIddict(options =>
            {
                // Register the Entity Framework stores.
                options.AddEntityFrameworkCoreStores<AppDbContext>();

                // Register the ASP.NET Core MVC binder used by OpenIddict.
                // Note: if you don't call this method, you won't be able to
                // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                options.AddMvcBinders();

                // Enable the token endpoint.
                options.EnableTokenEndpoint("/connect/token");

                // Enable the password flow.
                options.AllowPasswordFlow();

                // During development, you can disable the HTTPS requirement.
                options.DisableHttpsRequirement();

                // Note: to use JWT access tokens instead of the default
                // encrypted format, the following lines are required:
                //
                // options.UseJsonWebTokens();
                // options.AddEphemeralSigningKey();
            });

            services.AddAuthorization(opt=> 
            {
                opt.AddPolicy(Common.Policies.Policies.CreateProductPolicy,
                    p => p.RequireAuthenticatedUser().RequireRole("Admin"));
            });

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IProductService, ProductService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            

            app.UseMvcWithDefaultRoute();
            
            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }

            // Seed the database with the sample application.
            // Note: in a real world application, this step should be part of a setup script.
           InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();

            

            app.UseMvc();
        }

       
        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await context.Database.EnsureCreatedAsync();

                var roleManager = scope.ServiceProvider.GetService<RoleManager<UserRoleEntity>>();
                var userManager = scope.ServiceProvider.GetService<UserManager<UserEntity>>();

                var dbContext = scope.ServiceProvider.GetService<AppDbContext>();

                AddTestusers(roleManager, userManager,dbContext).Wait();
                
            }
        }

        private static async Task AddTestusers(RoleManager<UserRoleEntity> roleManager, UserManager<UserEntity> userManager, AppDbContext dbContext)
        {
            Guid orgId = Guid.NewGuid();

            var user = new UserEntity
            {
                Email = "ersinsivaz@hotmail.com",
                UserName = "ersinsivaz@hotmail.com",
                FirstName = "ersin",
                LastName = "sivaz",
                CreatedAt = DateTimeOffset.UtcNow,
                SecurityStamp = "UserSecurityStamp",
                Id = Guid.NewGuid(),
                OrganizationId = orgId
            };

            var user2 = new UserEntity
            {
                Email = "ersinsivaz@gmail.com",
                UserName = "ersinsivaz@gmail.com",
                FirstName = "ersin",
                LastName = "sivaz",
                CreatedAt = DateTimeOffset.UtcNow,
                SecurityStamp = "UserSecurityStamp",
                Id = Guid.NewGuid(),
                OrganizationId = orgId
            };


            OrganizationEntity organizationEntity = new OrganizationEntity
            {
                Id = orgId,
                Name = "Default Organization",
                Description = "Default organization created seed data",
                CreatedAt = DateTimeOffset.UtcNow,
                CreatedBy = user.Email
            };

            var org = await dbContext.Organizations.SingleOrDefaultAsync(o => o.Name == organizationEntity.Name);

            if(org==null)
            {
                await dbContext.Organizations.AddAsync(organizationEntity);

                await dbContext.SaveChangesAsync();
            }
            else
            {
                user.OrganizationId = org.Id;
                user2.OrganizationId = org.Id;
            }

           
            //Add test role
            await roleManager.CreateAsync(new UserRoleEntity("Admin"));
            await roleManager.CreateAsync(new UserRoleEntity("User"));


            await userManager.CreateAsync(user, "ersin123!!AA");
            await userManager.AddToRoleAsync(user, "Admin");
            await userManager.UpdateAsync(user);

            await userManager.CreateAsync(user2, "ersin123!!AA");
            await userManager.AddToRoleAsync(user2, "User");
            await userManager.UpdateAsync(user2);

        }

    }//cs
}//ns
