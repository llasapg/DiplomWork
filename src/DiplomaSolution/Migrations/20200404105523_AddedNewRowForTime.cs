using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomaSolution.Migrations
{
    public partial class AddedNewRowForTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "UploadTime",
                table: "CustomerImageFiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UploadTime",
                table: "CustomerImageFiles");
        }
    }
}
