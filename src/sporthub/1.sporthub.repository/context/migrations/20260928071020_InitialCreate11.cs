using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sporthub.repository.context.migrations
{
    /// <inheritdoc />
    public partial class InitialCreate11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoryOptionMaps_categories_category_id",
                table: "CategoryOptionMaps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryOptionMaps",
                table: "CategoryOptionMaps");

            migrationBuilder.RenameTable(
                name: "CategoryOptionMaps",
                newName: "category_option_maps");

            migrationBuilder.RenameIndex(
                name: "IX_CategoryOptionMaps_category_id_option_column",
                table: "category_option_maps",
                newName: "IX_category_option_maps_category_id_option_column");

            migrationBuilder.AddPrimaryKey(
                name: "PK_category_option_maps",
                table: "category_option_maps",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_category_option_maps_categories_category_id",
                table: "category_option_maps",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_category_option_maps_categories_category_id",
                table: "category_option_maps");

            migrationBuilder.DropPrimaryKey(
                name: "PK_category_option_maps",
                table: "category_option_maps");

            migrationBuilder.RenameTable(
                name: "category_option_maps",
                newName: "CategoryOptionMaps");

            migrationBuilder.RenameIndex(
                name: "IX_category_option_maps_category_id_option_column",
                table: "CategoryOptionMaps",
                newName: "IX_CategoryOptionMaps_category_id_option_column");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryOptionMaps",
                table: "CategoryOptionMaps",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_CategoryOptionMaps_categories_category_id",
                table: "CategoryOptionMaps",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
