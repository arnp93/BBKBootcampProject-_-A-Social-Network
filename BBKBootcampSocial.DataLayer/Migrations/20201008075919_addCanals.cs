using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BBKBootcampSocial.DataLayer.Migrations
{
    public partial class addCanals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CanalId",
                table: "Posts",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Canals",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    ImageName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canals", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CanalUsers",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    UpdateDate = table.Column<DateTime>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    isAdmin = table.Column<bool>(nullable: false),
                    Userid = table.Column<long>(nullable: false),
                    CanalId = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CanalUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CanalUsers_Canals_CanalId",
                        column: x => x.CanalId,
                        principalTable: "Canals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CanalUsers_Users_Userid",
                        column: x => x.Userid,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_CanalId",
                table: "Posts",
                column: "CanalId");

            migrationBuilder.CreateIndex(
                name: "IX_CanalUsers_CanalId",
                table: "CanalUsers",
                column: "CanalId");

            migrationBuilder.CreateIndex(
                name: "IX_CanalUsers_Userid",
                table: "CanalUsers",
                column: "Userid");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Canals_CanalId",
                table: "Posts",
                column: "CanalId",
                principalTable: "Canals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Canals_CanalId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "CanalUsers");

            migrationBuilder.DropTable(
                name: "Canals");

            migrationBuilder.DropIndex(
                name: "IX_Posts_CanalId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "CanalId",
                table: "Posts");
        }
    }
}
