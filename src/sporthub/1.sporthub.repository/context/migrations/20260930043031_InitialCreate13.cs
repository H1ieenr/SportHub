using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sporthub.repository.context.migrations
{
    /// <inheritdoc />
    public partial class InitialCreate13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_option_maps");

            migrationBuilder.DropColumn(
                name: "option1",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "option2",
                table: "product_variants");

            migrationBuilder.DropColumn(
                name: "option3",
                table: "product_variants");

            migrationBuilder.CreateTable(
                name: "option_definition",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    code = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<long>(type: "bigint", nullable: false),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_option_definition", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "category_option",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    option_definition_id = table.Column<long>(type: "bigint", nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: true),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<long>(type: "bigint", nullable: false),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_option", x => x.id);
                    table.ForeignKey(
                        name: "FK_category_option_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_category_option_option_definition_option_definition_id",
                        column: x => x.option_definition_id,
                        principalTable: "option_definition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "option_value",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    option_definition_id = table.Column<long>(type: "bigint", nullable: false),
                    value = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    display_order = table.Column<int>(type: "int", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<long>(type: "bigint", nullable: false),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_option_value", x => x.id);
                    table.ForeignKey(
                        name: "FK_option_value_option_definition_option_definition_id",
                        column: x => x.option_definition_id,
                        principalTable: "option_definition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_variant_option_value",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    product_variant_id = table.Column<long>(type: "bigint", nullable: false),
                    option_definition_id = table.Column<long>(type: "bigint", nullable: false),
                    option_value_id = table.Column<long>(type: "bigint", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    updated_by = table.Column<long>(type: "bigint", nullable: false),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    deleted_by = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_variant_option_value", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_variant_option_value_option_definition_option_definition_id",
                        column: x => x.option_definition_id,
                        principalTable: "option_definition",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_variant_option_value_option_value_option_value_id",
                        column: x => x.option_value_id,
                        principalTable: "option_value",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_variant_option_value_product_variants_product_variant_id",
                        column: x => x.product_variant_id,
                        principalTable: "product_variants",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_option_category_id_option_definition_id",
                table: "category_option",
                columns: new[] { "category_id", "option_definition_id" },
                unique: true,
                filter: "[deleted_date] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_category_option_option_definition_id",
                table: "category_option",
                column: "option_definition_id");

            migrationBuilder.CreateIndex(
                name: "IX_option_definition_code",
                table: "option_definition",
                column: "code",
                unique: true,
                filter: "[deleted_date] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_option_value_option_definition_id_value",
                table: "option_value",
                columns: new[] { "option_definition_id", "value" },
                unique: true,
                filter: "[deleted_date] IS NULL");

            migrationBuilder.CreateIndex(
                name: "IX_product_variant_option_value_option_definition_id",
                table: "product_variant_option_value",
                column: "option_definition_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variant_option_value_option_value_id",
                table: "product_variant_option_value",
                column: "option_value_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_variant_option_value_product_variant_id_option_definition_id",
                table: "product_variant_option_value",
                columns: new[] { "product_variant_id", "option_definition_id" },
                unique: true,
                filter: "[deleted_date] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "category_option");

            migrationBuilder.DropTable(
                name: "product_variant_option_value");

            migrationBuilder.DropTable(
                name: "option_value");

            migrationBuilder.DropTable(
                name: "option_definition");

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

            migrationBuilder.CreateTable(
                name: "category_option_maps",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    created_by = table.Column<long>(type: "bigint", nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    deleted_by = table.Column<long>(type: "bigint", nullable: false),
                    deleted_date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    display_order = table.Column<int>(type: "int", nullable: true),
                    label = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
                    option_column = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    updated_by = table.Column<long>(type: "bigint", nullable: false),
                    updated_date = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_category_option_maps", x => x.id);
                    table.ForeignKey(
                        name: "FK_category_option_maps_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_category_option_maps_category_id_option_column",
                table: "category_option_maps",
                columns: new[] { "category_id", "option_column" },
                unique: true,
                filter: "[deleted_date] IS NULL");
        }
    }
}
