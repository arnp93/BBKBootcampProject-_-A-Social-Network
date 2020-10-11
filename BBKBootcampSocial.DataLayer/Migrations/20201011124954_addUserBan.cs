using Microsoft.EntityFrameworkCore.Migrations;

namespace BBKBootcampSocial.DataLayer.Migrations
{
    public partial class addUserBan : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BanReason",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBan",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BanReason",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsBan",
                table: "Users");
        }
    }
}
