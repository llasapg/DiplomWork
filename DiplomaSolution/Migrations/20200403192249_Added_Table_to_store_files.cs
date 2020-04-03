using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DiplomaSolution.Migrations
{
    public partial class Added_Table_to_store_files : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerFiles");

            migrationBuilder.CreateTable(
                name: "AccountLevelFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true),
                    FileData = table.Column<byte[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLevelFiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerImageFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CustomerId = table.Column<string>(nullable: true),
                    FullName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerImageFiles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLevelFiles");

            migrationBuilder.DropTable(
                name: "CustomerImageFiles");

            migrationBuilder.CreateTable(
                name: "CustomerFiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    CustomerId = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    FileData = table.Column<byte[]>(type: "longblob", nullable: true),
                    FullName = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerFiles", x => x.Id);
                });
        }
    }
}
