using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDurationProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Classes");

            migrationBuilder.RenameColumn(
                name: "MaxCapacity",
                table: "Classes",
                newName: "Capacity");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e0cab133-db25-477b-b773-92df0d43fb2e", "AQAAAAIAAYagAAAAEEJkIxqXcifO2R9hz71+Wltwm20E++vWhkGUZrTQHTElijGbsgJNAaPojUVW21N/hw==", "cc9082b2-752c-4408-9c9e-3b3d28f5149d" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "26e32390-428c-4af3-936b-59478f35fc54", "AQAAAAIAAYagAAAAEO+3/CDQZPDAa6M9Vt+G7kdKoKxTc04zuSkib//FWJEfvOodrUSylKN1nMkqwJHoEQ==", "bd537afb-5528-42da-bb28-ebce407932b3" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Capacity",
                table: "Classes",
                newName: "MaxCapacity");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "Duration",
                table: "Classes",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));

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
    }
}
