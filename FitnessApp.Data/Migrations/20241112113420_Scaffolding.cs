using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Scaffolding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "1a081385-adf2-4cbf-bf1e-dd28bfe7285f", "AQAAAAIAAYagAAAAEAVTQFl5Uj3sOJ396WOmEKbbtf1HqWej2Hxyf8NEZSV6v+l2vsIWzL9sJgDlzoad2Q==", "908bcf41-b3ad-4422-b63a-174aad647f4d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "515dec4f-713c-4795-a09e-0c7f88b50cd4", "AQAAAAIAAYagAAAAECjc9oiajNFqTBbVdUBr7Kb95bII322hMLLb6Vo7tnrfXrp1aD26yWc9Lg0zkqlC8A==", "7a124985-2787-49b7-97a3-4262eeec6cef" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "300fd942-3512-4143-8fe1-2dca2d064c3d", "AQAAAAIAAYagAAAAEGoppdk3z66xMfXh5PzKG/M8tmrE/tP5iva/5ytimMygu7J/vyjlQjpnqt76bSaeXg==", "3006c63b-2e92-4376-a01f-6ab113151333" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95e84b7b-cb74-44bf-bd93-84a9a45ce332", "AQAAAAIAAYagAAAAEAQnYQbr98Qo/2lI6oQaEnJc/fGYdyQCJ5TOMHGWeQQWoKImoR2wmin+F9PPIHQynA==", "d120a5d2-f158-4e5b-9744-648b154f51b5" });
        }
    }
}
