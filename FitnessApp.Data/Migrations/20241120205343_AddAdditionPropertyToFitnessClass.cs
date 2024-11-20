using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAdditionPropertyToFitnessClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LeftCapacity",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeftCapacity",
                table: "Classes");

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
        }
    }
}
