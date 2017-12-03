using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace IdentityExample001.Migrations
{
    public partial class MakeNullableAuditUpdateFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LastUpdatedBy",
                table: "Products",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "Products",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));

            migrationBuilder.AlterColumn<Guid>(
                name: "LastUpdatedBy",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "Organizations",
                nullable: true,
                oldClrType: typeof(DateTimeOffset));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "LastUpdatedBy",
                table: "Products",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "Products",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LastUpdatedBy",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "LastUpdatedAt",
                table: "Organizations",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldNullable: true);
        }
    }
}
