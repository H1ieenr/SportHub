using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sporthub.repository.context.migrations
{
    /// <inheritdoc />
    public partial class AddInventoryRowVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_inventory_items_product_variant_id",
                table: "inventory_items");

            migrationBuilder.AddColumn<byte[]>(
                name: "row_version",
                table: "inventory_items",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_product_variant_id",
                table: "inventory_items",
                column: "product_variant_id",
                unique: true,
                filter: "[deleted_date] IS NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_inventory_items_on_hand",
                table: "inventory_items",
                sql: "[quantity_on_hand] >= 0");

            migrationBuilder.AddCheckConstraint(
                name: "CK_inventory_items_reserved",
                table: "inventory_items",
                sql: "[reserved_quantity] >= 0 AND [reserved_quantity] <= [quantity_on_hand]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_inventory_items_product_variant_id",
                table: "inventory_items");

            migrationBuilder.DropCheckConstraint(
                name: "CK_inventory_items_on_hand",
                table: "inventory_items");

            migrationBuilder.DropCheckConstraint(
                name: "CK_inventory_items_reserved",
                table: "inventory_items");

            migrationBuilder.DropColumn(
                name: "row_version",
                table: "inventory_items");

            migrationBuilder.CreateIndex(
                name: "IX_inventory_items_product_variant_id",
                table: "inventory_items",
                column: "product_variant_id",
                unique: true);
        }
    }
}
