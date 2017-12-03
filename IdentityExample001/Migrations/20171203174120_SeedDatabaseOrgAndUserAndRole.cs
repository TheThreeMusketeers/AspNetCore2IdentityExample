using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IdentityExample001.Migrations
{
    public partial class SeedDatabaseOrgAndUserAndRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            Guid orgId = Guid.NewGuid();
            Guid userId = Guid.NewGuid();

            string insertOrganizationCmd = string.Format("insert into Organizations (Id,CreatedAt,CreatedBy,Description,Name) values ()");

           // migrationBuilder.Sql();
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
