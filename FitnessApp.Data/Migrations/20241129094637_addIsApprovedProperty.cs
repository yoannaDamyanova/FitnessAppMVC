using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class addIsApprovedProperty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Classes",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "30cc7e05-7bc9-4e23-8680-fa15df7171fe", "AQAAAAIAAYagAAAAEAL+T/7nrq1AuaukDLLXwh1buqqlZpg5EitTwYSBsuzb0IggAIzP8ifHD3utcUtLfA==", "3db99f63-5e60-4464-aa93-ec4adee85b80" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e8ce60b2-822f-4410-b146-14d15e783ef5", "AQAAAAIAAYagAAAAEK522ANLD8zpgRMLGhkEJgR3uBTX8XklP7JNcr+U/DtrNrts1oK6pjIc4X7Rs8kuZg==", "8df83e34-bb10-4d6c-bb41-7af6b3b82548" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Classes");

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
    }
}
