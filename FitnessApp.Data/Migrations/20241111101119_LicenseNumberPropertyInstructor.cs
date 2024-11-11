using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class LicenseNumberPropertyInstructor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LicenseNumber",
                table: "Instructors",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "2b3859c5-0f7e-4095-b4b3-11dc1e680436", "AQAAAAIAAYagAAAAEPRcWfOT7weDaSQegT58NpUiV62yjIFwb01F8AVliiT3Yy+8cBq0copqxZNS7iPYPA==", "78ba1694-2986-4662-bc01-ace4f078cbfa" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "093825d0-f99e-40da-a2e4-28ae9323d3f6", "AQAAAAIAAYagAAAAECUM/IR1URAmY6Mru9YIl4VyxNQAny5QMdyBNQEdiRL2StXvYmNx5QmTKY6z8A++PA==", "bbcaa477-b260-4755-b96c-eb432545155c" });

            migrationBuilder.UpdateData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 1,
                column: "LicenseNumber",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Instructors");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "0127caae-051f-4bd0-bd01-ace6a5e5387b", "AQAAAAIAAYagAAAAEOrL+VVqjqoKMswAtSvVRy3+TevSchzKdYe9Uqdtvkcr9pTwhBNUJqX7lDKVMNxTkA==", "46b38f9d-0f61-4fbe-bf85-9efcbefac23e" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "19f98ad1-b524-4455-9329-1277c3406d25", "AQAAAAIAAYagAAAAELbl1yQVVpL1p4bP2m+yEpkU8k+OV3iqJAuyg4Ecl2bBfMNzXaCaaUFgtGK1YSKLsg==", "084e0a26-84ab-4777-a618-946e31b49933" });
        }
    }
}
