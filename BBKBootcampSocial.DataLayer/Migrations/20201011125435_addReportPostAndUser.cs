using Microsoft.EntityFrameworkCore.Migrations;

namespace BBKBootcampSocial.DataLayer.Migrations
{
    public partial class addReportPostAndUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TimesOfReports",
                table: "Users",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ReportDeleteReason",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimesOfReports",
                table: "Posts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimesOfReports",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ReportDeleteReason",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "TimesOfReports",
                table: "Posts");
        }
    }
}
