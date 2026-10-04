using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sporthub.repository.context.migrations
{
    /// <inheritdoc />
    public partial class InitialCreate10 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryOptionMaps",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    category_id = table.Column<long>(type: "bigint", nullable: false),
                    option_column = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    label = table.Column<string>(type: "nvarchar(220)", maxLength: 220, nullable: false),
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
                    table.PrimaryKey("PK_CategoryOptionMaps", x => x.id);
                    table.ForeignKey(
                        name: "FK_CategoryOptionMaps_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryOptionMaps_category_id_option_column",
                table: "CategoryOptionMaps",
                columns: new[] { "category_id", "option_column" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryOptionMaps");
        }
    }
}
