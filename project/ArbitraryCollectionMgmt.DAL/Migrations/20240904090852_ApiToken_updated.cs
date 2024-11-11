using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArbitraryCollectionMgmt.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ApiToken_updated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiTokens",
                table: "ApiTokens");

            migrationBuilder.AlterColumn<string>(
                name: "TokenKey",
                table: "ApiTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ApiTokens",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "ApiTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiTokens",
                table: "ApiTokens",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ApiTokens",
                table: "ApiTokens");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ApiTokens");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "ApiTokens");

            migrationBuilder.AlterColumn<Guid>(
                name: "TokenKey",
                table: "ApiTokens",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApiTokens",
                table: "ApiTokens",
                column: "TokenKey");
        }
    }
}
