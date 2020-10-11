using Microsoft.EntityFrameworkCore.Migrations;

namespace BBKBootcampSocial.DataLayer.Migrations
{
    public partial class addUserIdToComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Comments",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Comments");
        }
    }
}
