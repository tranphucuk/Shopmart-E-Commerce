using Microsoft.EntityFrameworkCore.Migrations;

namespace NetcoreOnlineShop.Data.EF.Migrations
{
    public partial class UpdateAnnouncementTable2nd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Announcements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Announcements");
        }
    }
}
