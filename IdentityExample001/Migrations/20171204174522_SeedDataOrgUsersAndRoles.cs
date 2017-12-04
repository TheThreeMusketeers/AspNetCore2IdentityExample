using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IdentityExample001.Migrations
{
    public partial class SeedDataOrgUsersAndRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sqlCommand = @"
                INSERT [dbo].[AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName]) VALUES (N'5a2524a5-cbf3-49be-48da-08d53b3de7c1', N'801a805c-5ef7-444b-84c2-5f86612205fb', N'Admin', N'ADMIN')
                GO
                INSERT [dbo].[AspNetRoles] ([Id], [ConcurrencyStamp], [Name], [NormalizedName]) VALUES (N'ae65e422-15b5-4cde-48db-08d53b3de7c1', N'a14f2a66-9145-4ddd-b2dc-a15ff3ef6f80', N'User', N'USER')
                GO
                INSERT [dbo].[Organizations] ([Id], [CreatedAt], [CreatedBy], [Description], [LastUpdatedAt], [LastUpdatedBy], [Name]) VALUES (N'4d47600b-f875-40b7-b9b8-c4140f1f2682', CAST(N'2017-12-04T17:39:00.2675351+00:00' AS DateTimeOffset), N'1e9da00e-6424-4195-8cb0-d669a1c6b2ae', N'Default organization created seed data', NULL, NULL, N'Default Organization')
                GO
                INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [CreatedAt], [Email], [EmailConfirmed], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [OrganizationId], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName]) VALUES (N'8d05b8bc-ccfb-4fcc-af09-d4c1ebdee70e', 0, N'88539db2-3c94-48cb-9b26-0d118b00298c', CAST(N'2017-12-04T17:39:00.2672304+00:00' AS DateTimeOffset), N'ersinsivaz@gmail.com', 0, N'ersin', N'sivaz', 1, NULL, N'ERSINSIVAZ@GMAIL.COM', N'ERSINSIVAZ@GMAIL.COM', N'4d47600b-f875-40b7-b9b8-c4140f1f2682', N'AQAAAAEAACcQAAAAEH56olT19pwyBow9yhHffwYtyGjLcJJSykK87Q+3RhN6W4Y9hMgK7plvebj1AtZU1Q==', NULL, 0, N'9b8fa8a4-b89d-4f67-886d-561bb2ed218d', 0, N'ersinsivaz@gmail.com')
                GO
                INSERT [dbo].[AspNetUsers] ([Id], [AccessFailedCount], [ConcurrencyStamp], [CreatedAt], [Email], [EmailConfirmed], [FirstName], [LastName], [LockoutEnabled], [LockoutEnd], [NormalizedEmail], [NormalizedUserName], [OrganizationId], [PasswordHash], [PhoneNumber], [PhoneNumberConfirmed], [SecurityStamp], [TwoFactorEnabled], [UserName]) VALUES (N'1e9da00e-6424-4195-8cb0-d669a1c6b2ae', 0, N'd02d4ea9-9ee4-447e-9d90-fe61ea2b4eb9', CAST(N'2017-12-04T17:39:00.2670191+00:00' AS DateTimeOffset), N'ersinsivaz@hotmail.com', 0, N'ersin', N'sivaz', 1, NULL, N'ERSINSIVAZ@HOTMAIL.COM', N'ERSINSIVAZ@HOTMAIL.COM', N'4d47600b-f875-40b7-b9b8-c4140f1f2682', N'AQAAAAEAACcQAAAAEFjjpW5p0SNMnGCohPecCAqcQ/FgYYLdCudYsqf9F0SMzYaPHhJcP8h+bEHn/C4/3g==', NULL, 0, N'84169fd1-cfe8-4dbb-888b-10fe195d289b', 0, N'ersinsivaz@hotmail.com')
                GO
                INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'1e9da00e-6424-4195-8cb0-d669a1c6b2ae', N'5a2524a5-cbf3-49be-48da-08d53b3de7c1')
                GO
                INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'8d05b8bc-ccfb-4fcc-af09-d4c1ebdee70e', N'ae65e422-15b5-4cde-48db-08d53b3de7c1')
                GO
            ";

            migrationBuilder.Sql(sqlCommand);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
