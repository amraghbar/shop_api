using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class apo8 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CitiesId",
                table: "Stores",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CitiesId",
                table: "Stores",
                column: "CitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Cities_CitiesId",
                table: "Stores",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Cities_CitiesId",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_CitiesId",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "CitiesId",
                table: "Stores");
        }
    }
}
