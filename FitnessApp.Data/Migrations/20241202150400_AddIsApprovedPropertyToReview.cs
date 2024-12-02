using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddIsApprovedPropertyToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                table: "Reviews",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "f5895290-6474-40c3-94ea-79aef3687c5d", "AQAAAAIAAYagAAAAEFcuccGgWansU+cAs4DTSWkpHRLDAcMAk8ot4ks7JhkEMe80lCQ6mQwP6jJlUsJlIA==", "05576fee-d603-422f-8292-172b26ae9280" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "cecd0b10-5016-461d-a86b-e0405c493911", "AQAAAAIAAYagAAAAEDGX/LYD7jXiOY1APkfTdFZkziv9IBtV0IzpD+aXq9qHEmHqsXuNDAkpOhL1TMqcHQ==", "670b9f12-c5b4-43c2-9e0c-58051a5b3b6c" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsApproved",
                table: "Reviews");

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
    }
}
