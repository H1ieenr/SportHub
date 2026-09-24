using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sporthub.repository.context.migrations
{
    /// <inheritdoc />
    public partial class InitialCreate9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "option1",
                table: "product_variants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "option2",
                table: "product_variants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "option3",
                table: "product_variants",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "option1",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "option2",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "option3",
                table: "product_variants");
        }
    }
}
