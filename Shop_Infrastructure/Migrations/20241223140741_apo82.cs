using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class apo82 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Factor",
                table: "InvItemStores");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Factor",
                table: "InvItemStores",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
