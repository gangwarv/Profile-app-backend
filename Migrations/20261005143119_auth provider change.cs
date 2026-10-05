using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Profile_app_backend.Migrations
{
    /// <inheritdoc />
    public partial class authproviderchange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AzureB2COid",
                table: "UserProfiles",
                newName: "AzureOid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AzureOid",
                table: "UserProfiles",
                newName: "AzureB2COid");
        }
    }
}
