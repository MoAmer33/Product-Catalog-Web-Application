using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Product_Catalog_Web_Application.Migrations
{
    /// <inheritdoc />
    public partial class Newinit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "duration",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "duration",
                table: "Products");
        }
    }
}
