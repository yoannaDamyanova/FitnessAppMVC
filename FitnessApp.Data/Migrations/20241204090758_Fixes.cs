using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class Fixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "fc1d5380-f031-415a-9b28-187fcd14148c", "AQAAAAIAAYagAAAAEGK75cAtbjjRuxiaBQFAACThc7vGIy1Bt0HjdmTg0aCPcZVzlwNPO1cH+MlhaF5RHg==", "b4b1b7d3-15ee-45b8-9af6-6929f1d66768" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "aeb067f7-4cd7-4b42-b308-74d7dd55badc", "AQAAAAIAAYagAAAAEK28/zl+Qw2T2ZpOSxmezCB/7brJYy9IrCojm9ehyrxJKOYZJuJppOw238hAJLEBSQ==", "d857ffeb-f1ae-4378-9427-b405a97f0af1" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "544eeb57-9ce9-422c-ac1a-0fbda5405195", "AQAAAAIAAYagAAAAEA2Db7Q+t5NPFYTpTODQDwRzKpzRY5wyEg388V48ZEevJqiSAoY6W6UjgJm238N2Bw==", "a2785947-0ec8-42f9-a1f0-021a0f9f0b9f" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5234ddf5-a048-4c8e-a191-9cb544f97ca2", "AQAAAAIAAYagAAAAEGBms2C98NUJ5frrtzyEvOSlJnI+mGFHSMpnJKY8bG/0wid8yMBoTn9APk4fcW1jxA==", "aafe2f25-dc60-4a4c-a4dd-ed6a335ad2e4" });
        }
    }
}
