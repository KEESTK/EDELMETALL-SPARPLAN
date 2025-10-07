using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparplan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixDepotRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sparplaene_Depots_DepotId1",
                table: "Sparplaene");

            migrationBuilder.DropIndex(
                name: "IX_Sparplaene_DepotId1",
                table: "Sparplaene");

            migrationBuilder.DropColumn(
                name: "DepotId1",
                table: "Sparplaene");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DepotId1",
                table: "Sparplaene",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sparplaene_DepotId1",
                table: "Sparplaene",
                column: "DepotId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Sparplaene_Depots_DepotId1",
                table: "Sparplaene",
                column: "DepotId1",
                principalTable: "Depots",
                principalColumn: "Id");
        }
    }
}
