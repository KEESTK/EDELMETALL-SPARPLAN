using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparplan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sparplaene_Depots_DepotId",
                table: "Sparplaene");

            migrationBuilder.DropForeignKey(
                name: "FK_Sparplaene_Depots_DepotId1",
                table: "Sparplaene");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepotId",
                table: "Sparplaene",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sparplaene_Depots_DepotId",
                table: "Sparplaene",
                column: "DepotId",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sparplaene_Depots_DepotId1",
                table: "Sparplaene",
                column: "DepotId1",
                principalTable: "Depots",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sparplaene_Depots_DepotId",
                table: "Sparplaene");

            migrationBuilder.DropForeignKey(
                name: "FK_Sparplaene_Depots_DepotId1",
                table: "Sparplaene");

            migrationBuilder.AlterColumn<Guid>(
                name: "DepotId",
                table: "Sparplaene",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_Sparplaene_Depots_DepotId",
                table: "Sparplaene",
                column: "DepotId",
                principalTable: "Depots",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sparplaene_Depots_DepotId1",
                table: "Sparplaene",
                column: "DepotId1",
                principalTable: "Depots",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
