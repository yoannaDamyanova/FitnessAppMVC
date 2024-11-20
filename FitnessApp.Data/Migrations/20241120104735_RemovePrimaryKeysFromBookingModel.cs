using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePrimaryKeysFromBookingModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f966355e-75b3-445e-9350-55b9900ce401", "AQAAAAIAAYagAAAAEEf5Xquymjmt8AzpOk1owPD+FISsdHq2cxGLulyBSSiYuY3DXQWPOl5gK/sA8KUkCQ==", "22d15a39-7891-4516-82e2-c5b7a13114d8" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "019092c6-e91e-491a-a9d2-cd9c4d1f3511", "AQAAAAIAAYagAAAAECd4MapheqZl9Fu4jpfqvZvVVAyxPY2kpKPwYYEh7ffoejlfYY0n3DUzpcL/kqwh3Q==", "05a63737-a250-4ea6-99a3-573403c7c8ac" });

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_UserId_FitnessClassId",
                table: "Bookings",
                columns: new[] { "UserId", "FitnessClassId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_UserId_FitnessClassId",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Bookings");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                columns: new[] { "UserId", "FitnessClassId" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5731a6db-ed86-431a-9d00-4c48dac1de4d", "AQAAAAIAAYagAAAAENWo39GYx6NQ/To/ZfMw5MJcZngdQsnhySoNTG7S5qQJTJsDdDLhLyyh4jr/r+3IJA==", "058cdf1d-1c5d-497d-b0ec-23c4fd68b4d0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "33eb341c-6b2a-4b73-8bb1-9c3519e2db8c", "AQAAAAIAAYagAAAAEOXl6TYYucxtMWXjKmGn/JAO0RiaGnA08UPQGlBE/c/0yVCH7aO9Qe0hoPKQW6lvXw==", "6b90603e-2b41-46d7-884c-4724a25c99f2" });
        }
    }
}
