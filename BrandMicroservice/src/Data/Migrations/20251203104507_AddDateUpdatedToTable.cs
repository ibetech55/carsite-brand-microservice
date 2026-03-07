using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BrandMicroservice.src.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDateUpdatedToTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
           name: "date_updated",
           table: "makes",
           type: "timestamp with time zone",
           nullable: true
       );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
        name: "date_updated",
        table: "makes"
    );
        }
    }
}
