using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BBKBootcampSocial.DataLayer.Migrations
{
    public partial class UpdateUserInfo2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Facebook",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Instagram",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LinkdIn",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "WhatsApp",
                table: "Users");

            migrationBuilder.AddColumn<DateTime>(
                name: "BirthDay",
                table: "Users",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "SocialNetwork",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthDay",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "SocialNetwork",
                table: "Users");

            migrationBuilder.AddColumn<string>(
                name: "Facebook",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Instagram",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LinkdIn",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsApp",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
