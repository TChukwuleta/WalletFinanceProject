using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Dot.Infrastructure.Migrations
{
    public partial class SavingsFunctionalities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Savings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    SavingStatus = table.Column<int>(type: "int", nullable: false),
                    SavingStatusDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FundingSource = table.Column<int>(type: "int", nullable: false),
                    FundingSourceDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SavingFrequency = table.Column<int>(type: "int", nullable: false),
                    SavingFrequencyDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SavingsType = table.Column<int>(type: "int", nullable: false),
                    SavingsTypeDesc = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Purpose = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Savings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestBuddies",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SavingsName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuddyEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BuddyStudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StudentId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SavingId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestBuddies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RequestBuddies_Savings_SavingId",
                        column: x => x.SavingId,
                        principalTable: "Savings",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_RequestBuddies_SavingId",
                table: "RequestBuddies",
                column: "SavingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RequestBuddies");

            migrationBuilder.DropTable(
                name: "Savings");
        }
    }
}
