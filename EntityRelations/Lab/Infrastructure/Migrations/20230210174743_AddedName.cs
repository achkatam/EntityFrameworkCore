using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class AddedName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "People",
                comment: "This sis person");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "People",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Person's name",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterTable(
                name: "People",
                oldComment: "This sis person");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "People",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Person's name");
        }
    }
}
