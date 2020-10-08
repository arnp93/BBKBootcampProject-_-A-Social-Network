using Microsoft.EntityFrameworkCore.Migrations;

namespace BBKBootcampSocial.DataLayer.Migrations
{
    public partial class FriendsRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FriendId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_FriendId",
                table: "Users",
                column: "FriendId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Users_FriendId",
                table: "Users",
                column: "FriendId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Users_FriendId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_FriendId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FriendId",
                table: "Users");
        }
    }
}
