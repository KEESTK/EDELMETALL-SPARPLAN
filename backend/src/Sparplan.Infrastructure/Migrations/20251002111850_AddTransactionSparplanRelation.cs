using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Sparplan.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTransactionSparplanRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SparplanClassId",
                table: "Transactions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SparplanClassId",
                table: "Transactions",
                column: "SparplanClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_Sparplaene_SparplanClassId",
                table: "Transactions",
                column: "SparplanClassId",
                principalTable: "Sparplaene",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_Sparplaene_SparplanClassId",
                table: "Transactions");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SparplanClassId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SparplanClassId",
                table: "Transactions");
        }
    }
}
