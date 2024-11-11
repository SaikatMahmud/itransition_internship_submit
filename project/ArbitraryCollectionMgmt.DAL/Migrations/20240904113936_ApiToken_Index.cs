using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArbitraryCollectionMgmt.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ApiToken_Index : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TokenKey",
                table: "ApiTokens",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_ApiTokens_TokenKey",
                table: "ApiTokens",
                column: "TokenKey",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ApiTokens_TokenKey",
                table: "ApiTokens");

            migrationBuilder.AlterColumn<string>(
                name: "TokenKey",
                table: "ApiTokens",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
