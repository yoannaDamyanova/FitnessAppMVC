using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class StatusModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Classes");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "13da7a5c-c97a-45d4-a8b6-714c3c6906f4", "AQAAAAIAAYagAAAAECd6Wx6x0A3exFQvpdiFjbjcuAY2H9myPsL8YMFMH31hItnlE+6fHHRYW6r3c1hZ8w==", "048d1370-1de3-40f4-820b-3252ee91bf3f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "a2e41213-e3ab-4916-97c6-36bb0d5bc1c9", "AQAAAAIAAYagAAAAEHZbCpXpGJwvK61esoXIo48GNyHfAk+nVm48CHGkLs+I6nTa2V/n3ZGnpUkXEo0PFg==", "0ab7842a-b14e-4011-a676-9dc852a5359a" });

            migrationBuilder.InsertData(
                table: "Statuses",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Active" },
                    { 2, "Canceled" },
                    { 3, "Finished" },
                    { 4, "Full" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_StatusId",
                table: "Classes",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Statuses_StatusId",
                table: "Classes",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Statuses_StatusId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Classes_StatusId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Classes");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Classes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "91f04655-5ce6-4050-9207-5f928b5f6102", "AQAAAAIAAYagAAAAEL9MiKJ9wGsYhM+XZHwsl1hiEIADLhfxJ4y1Tqus36S9jz2aLdstoPlwm6dmz9CJiw==", "89076dd5-94e5-405d-bc92-75fa78a88b23" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c20a2c04-0aaa-48b1-8a1a-c2cf56651fc3", "AQAAAAIAAYagAAAAEOJiw6kUgB/UIRLnTaWEuwFgSBgCR1ZXhR69PRLM0E/gwk9Gil0CRzSHUKGz3A3neA==", "3a513901-e31e-4202-b135-de9bfbf3aca6" });
        }
    }
}
