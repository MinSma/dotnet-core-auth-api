using Microsoft.EntityFrameworkCore.Migrations;

namespace dotnet_core_auth_api.Migrations
{
    public partial class AddRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($"SET IDENTITY_INSERT [AspNetRoles] ON;");
            migrationBuilder.Sql("INSERT INTO [AspNetRoles] (Id, Name, NormalizedName) VALUES " +
                                 "((SELECT COALESCE(MAX(Id), 0) FROM [AspNetRoles]) + 1, 'ADMIN', 'Admin');");
            migrationBuilder.Sql("INSERT INTO [AspNetRoles] (Id, Name, NormalizedName) VALUES " +
                                 "((SELECT COALESCE(MAX(Id), 0) FROM [AspNetRoles]) + 1, 'CUSTOMER', 'Customer');");
            migrationBuilder.Sql($"SET IDENTITY_INSERT [AspNetRoles] OFF;");
        }
    }
}