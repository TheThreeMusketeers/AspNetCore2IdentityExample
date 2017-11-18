﻿using System;
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
                options.UseOpenIddict();
            });

            //map some of the default claims names to OpenId claim names
            services.Configure<IdentityOptions>(opt=> 
            {
                opt.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                opt.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                opt.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;

            });

            services.AddOpenIddict<Guid>(opt=> 
            {
                opt.AddEntityFrameworkCoreStores<AppDbContext>();
                opt.AddMvcBinders();
                opt.EnableTokenEndpoint("api/token");
                opt.AllowPasswordFlow();
                opt.AllowClientCredentialsFlow();
                opt.DisableHttpsRequirement();
            });
        

            services.AddIdentity<UserEntity, UserRoleEntity>()
                    .AddEntityFrameworkStores<AppDbContext>()
                    .AddDefaultTokenProviders();

            services.AddScoped<RoleManager<UserRoleEntity>>();



            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();

            if (env.IsDevelopment())
            {

                app.UseDeveloperExceptionPage();
            }

            // Seed the database with the sample application.
            // Note: in a real world application, this step should be part of a setup script.
            InitializeAsync(app.ApplicationServices, CancellationToken.None).GetAwaiter().GetResult();

            app.UseOAuthValidation();
           

            app.UseMvc();
        }

        private static async Task AddTestusers(RoleManager<UserRoleEntity> roleManager, UserManager<UserEntity> userManager)
        {
            //Add test role
            await roleManager.CreateAsync(new UserRoleEntity("Admin"));

            //Add test user
            var user = new UserEntity
            {
                Email = "ersinsivaz@hotmail.com",
                UserName = "ersinsivaz@hotmail.com",
                FirstName = "ersin",
                LastName = "sivaz",
                CreatedAt = DateTimeOffset.UtcNow,
                SecurityStamp = "UserSecurityStamp",
                Id = Guid.NewGuid()
            };

            await userManager.CreateAsync(user, "ersin123!!AA");
            await userManager.AddToRoleAsync(user, "Admin");
            await userManager.UpdateAsync(user);
        }

        private async Task InitializeAsync(IServiceProvider services, CancellationToken cancellationToken)
        {
            // Create a new service scope to ensure the database context is correctly disposed when this methods returns.
            using (var scope = services.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                await context.Database.EnsureCreatedAsync();

                var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

                if (await manager.FindByClientIdAsync("client", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "client",
                        ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                        DisplayName = "My client application"
                    };

                    await manager.CreateAsync(descriptor, cancellationToken);
                }

                if (await manager.FindByClientIdAsync("postman", cancellationToken) == null)
                {
                    var descriptor = new OpenIddictApplicationDescriptor
                    {
                        ClientId = "postman",
                        DisplayName = "Postman",
                        RedirectUris = { new Uri("https://www.getpostman.com/oauth2/callback") }
                    };

                    await manager.CreateAsync(descriptor, cancellationToken);
                }

                var roleManager = scope.ServiceProvider.GetService<RoleManager<UserRoleEntity>>();
                var userManager = scope.ServiceProvider.GetService<UserManager<UserEntity>>();

                AddTestusers(roleManager, userManager).Wait();
                
            }
        }
    }
}