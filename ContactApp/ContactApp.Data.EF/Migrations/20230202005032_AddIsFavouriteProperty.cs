using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactApp.Data.EF.Migrations
{
    public partial class AddIsFavouriteProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFavourite",
                table: "Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFavourite",
                table: "Contacts");
        }
    }
}
