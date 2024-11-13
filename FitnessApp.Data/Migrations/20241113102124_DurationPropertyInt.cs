using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class DurationPropertyInt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "7a489956-51d5-4697-81f5-eb546266416e", "AQAAAAIAAYagAAAAEPDzPaZAqKeNwst5ADgqUdEIP+jJ2mhPxrjEjUfbRO19LR8q8HARvN+61J676pGwAQ==", "834a8855-dc56-4b61-bd02-5201b08f57cc" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "c05c9cd6-143e-4517-b429-50e2be73d11a", "AQAAAAIAAYagAAAAEKKw1oOYa4bK2OkwtO0YBXCwNSnuDfe7zRzVhOgfJ6eKuip69Kf0z2C4ElIPah00XA==", "6c472235-b7e0-4ce3-a3e7-69b3da12e680" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Classes");

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
    }
}
