using Microsoft.EntityFrameworkCore.Migrations;

namespace TimeSheetsCoreApp.Migrations
{
    public partial class InitializeUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO Users (UserId, UserName, Password, FirstName, LastName) VALUES (1, \"ptletski@cox.net\", \"test\", \"Paul\", \"Tletski\")");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
