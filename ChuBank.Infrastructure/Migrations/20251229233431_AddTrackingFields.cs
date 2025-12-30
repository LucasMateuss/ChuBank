using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChuBank.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTrackingFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FromAccountId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ToAccountId",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FromAccountId",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "ToAccountId",
                table: "Transactions");
        }
    }
}
