using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparplan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMetalPriceHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MetalPrices",
                table: "MetalPrices");

            migrationBuilder.RenameTable(
                name: "MetalPrices",
                newName: "MetalPriceHistories");

            migrationBuilder.RenameIndex(
                name: "IX_MetalPrices_Metal_Date",
                table: "MetalPriceHistories",
                newName: "IX_MetalPriceHistories_Metal_Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "MetalPriceHistories",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MetalPriceHistories",
                table: "MetalPriceHistories",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_MetalPriceHistories",
                table: "MetalPriceHistories");

            migrationBuilder.RenameTable(
                name: "MetalPriceHistories",
                newName: "MetalPrices");

            migrationBuilder.RenameIndex(
                name: "IX_MetalPriceHistories_Metal_Date",
                table: "MetalPrices",
                newName: "IX_MetalPrices_Metal_Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "MetalPrices",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date");

            migrationBuilder.AddPrimaryKey(
                name: "PK_MetalPrices",
                table: "MetalPrices",
                column: "Id");
        }
    }
}
