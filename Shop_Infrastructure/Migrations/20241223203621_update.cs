using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Cities_CitiesId",
                table: "AspNetUsers");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_Stores_Cities_CitiesId",
            //    table: "Stores");

            //migrationBuilder.DropIndex(
            //    name: "IX_Stores_CitiesId",
            //    table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CitiesId",
                table: "AspNetUsers");

            //migrationBuilder.DropColumn(
            //    name: "CitiesId",
            //    table: "Stores");

            migrationBuilder.DropColumn(
                name: "CitiesId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CitiesId",
                table: "Stores",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CitiesId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stores_CitiesId",
                table: "Stores",
                column: "CitiesId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CitiesId",
                table: "AspNetUsers",
                column: "CitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Cities_CitiesId",
                table: "AspNetUsers",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Cities_CitiesId",
                table: "Stores",
                column: "CitiesId",
                principalTable: "Cities",
                principalColumn: "Id");
        }
    }
}
