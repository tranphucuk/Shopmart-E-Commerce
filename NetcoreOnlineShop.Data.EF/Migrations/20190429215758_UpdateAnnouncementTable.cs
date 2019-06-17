using Microsoft.EntityFrameworkCore.Migrations;

namespace NetcoreOnlineShop.Data.EF.Migrations
{
    public partial class UpdateAnnouncementTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Announcements",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 500);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Announcements",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string));
        }
    }
}
