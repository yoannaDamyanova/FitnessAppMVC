using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class FixReviewProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "Reviews",
                type: "nvarchar(1024)",
                maxLength: 1024,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "beae3d01-5284-47c6-9660-0f24228352c5", "AQAAAAIAAYagAAAAEPReF/RkYm4jMyK+it2Jz00a2JFUSXJIg0hY3Fcwt6kDx36raqIOgT8wyCcJPNY43Q==", "4a0c877c-667d-4afe-9dad-7d5c69ab0f82" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "47b9ffbf-59fb-4a9b-bc5e-842727152429", "AQAAAAIAAYagAAAAENayOZM4asGZAoY2ZgKABYedzJ2dqL7NxN6v2rMN4aF9ihBFqXTJFPbYTskZLhqhjw==", "c44e6fa1-5183-47f4-b47e-f471dbba7fc5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "Reviews",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(1024)",
                oldMaxLength: 1024);

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
        }
    }
}
