using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandMicroservice.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class CreateModelTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "make_logo",
                table: "makes",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "makes",
                type: "TIMESTAMPTZ",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMPTZ",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.CreateTable(
                name: "models",
                columns: table => new
                {
                    _id = table.Column<Guid>(type: "uuid", nullable: false),
                    model_name = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    make_id = table.Column<Guid>(type: "uuid", nullable: false),
                    active = table.Column<bool>(type: "boolean", nullable: false),
                    body_type = table.Column<string>(type: "VARCHAR(100)", nullable: false),
                    year_founded = table.Column<string>(type: "integer", nullable: false),
                    model_code = table.Column<string>(type: "VARCHAR(20)", nullable: false),
                    date_created = table.Column<DateTime>(type: "TIMESTAMPTZ", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
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
                name: "models");

            migrationBuilder.AlterColumn<string>(
                name: "make_logo",
                table: "makes",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateTime>(
                name: "date_created",
                table: "makes",
                type: "TIMESTAMPTZ",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "TIMESTAMPTZ",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");
        }
    }
}
