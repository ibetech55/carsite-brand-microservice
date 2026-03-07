using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandMicroservice.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateMakeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "makes",
                columns: table => new
                {
                    _id = table.Column<Guid>(type: "uuid", nullable: false),
                    make_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    origin = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    make_logo = table.Column<string>(type: "TEXT", nullable: true),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    year_founded = table.Column<int>(type: "integer", nullable: false),
                    company = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    make_code = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    make_abbreviation = table.Column<string>(type: "VARCHAR(10)", nullable: false),
                    date_created = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_makes", x => x._id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "makes");
        }
    }
}
