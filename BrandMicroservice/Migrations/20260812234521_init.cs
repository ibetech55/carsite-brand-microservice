using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandMicroservice.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "forms",
                columns: table => new
                {
                    _id = table.Column<Guid>(type: "uuid", nullable: false),
                    form_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    form_type = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    form_description = table.Column<string>(type: "VARCHAR(1000)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_forms", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "makes",
                columns: table => new
                {
                    _id = table.Column<Guid>(type: "uuid", nullable: false),
                    make_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    origin = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    make_logo = table.Column<string>(type: "TEXT", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    year_founded = table.Column<int>(type: "integer", nullable: false),
                    company = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    make_code = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    make_abbreviation = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    date_created = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    date_updated = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_makes", x => x._id);
                });

            migrationBuilder.CreateTable(
                name: "models",
                columns: table => new
                {
                    _id = table.Column<Guid>(type: "uuid", nullable: false),
                    model_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    make_id = table.Column<Guid>(type: "uuid", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    body_type = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    year_founded = table.Column<int>(type: "integer", nullable: false),
                    model_code = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    date_created = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    date_updated = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_models", x => x._id);
                    table.ForeignKey(
                        name: "FK_models_makes_make_id",
                        column: x => x.make_id,
                        principalTable: "makes",
                        principalColumn: "_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_models_make_id",
                table: "models",
                column: "make_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "forms");

            migrationBuilder.DropTable(
                name: "models");

            migrationBuilder.DropTable(
                name: "makes");
        }
    }
}
