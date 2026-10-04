using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sporthub.repository.context.migrations
{
    /// <inheritdoc />
    public partial class InitialCreate12 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_category_option_maps_category_id_option_column",
                table: "category_option_maps");

            migrationBuilder.CreateIndex(
                name: "IX_category_option_maps_category_id_option_column",
                table: "category_option_maps",
                columns: new[] { "category_id", "option_column" },
                unique: true,
                filter: "[deleted_date] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_category_option_maps_category_id_option_column",
                table: "category_option_maps");

            migrationBuilder.CreateIndex(
                name: "IX_category_option_maps_category_id_option_column",
                table: "category_option_maps",
                columns: new[] { "category_id", "option_column" },
                unique: true,
                filter: "[is_deleted] = 0");
        }
    }
}
