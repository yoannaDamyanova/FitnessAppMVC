using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedBaseFitnessClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "6ba71b19-1e3c-4010-9728-37cd773a9c59", "AQAAAAIAAYagAAAAEIp2/CMqoGP5qMUaVG9d85IsQT/rCt6rOZv26xa9WMEpZ9tfUZlyAG9yjolzX1b8LQ==", "ad818c02-f76b-4ad4-82de-42211cf9f74a" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "e7e8273f-3444-44cc-9bbc-e0cfbef1ca26", "AQAAAAIAAYagAAAAEP/itV32QJnWUgd3m0wFFjiYXOLYh1TMrV9lTrDeQIr2lLOPngazVn/wfBw1tN7oVw==", "b9a21ae1-b502-42e6-b0f6-26aa06d51656" });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "Capacity", "CategoryId", "Description", "Duration", "InstructorId", "IsApproved", "LeftCapacity", "StartTime", "StatusId", "Title" },
                values: new object[,]
                {
                    { new Guid("39cb58ae-25a0-4eae-8021-16f73996b0e3"), 20, 6, "Build strength and stamina with high-intensity interval training.", 40, 1, true, 0, new DateTime(2024, 12, 11, 14, 30, 0, 0, DateTimeKind.Unspecified), 4, "HIIT Strength Training" },
                    { new Guid("919668ba-2eab-4916-8315-28500333882f"), 20, 1, "Start your day with a relaxing yoga session to stretch and recharge.", 60, 1, true, 20, new DateTime(2024, 12, 22, 7, 0, 0, 0, DateTimeKind.Unspecified), 1, "Morning Yoga" },
                    { new Guid("9e2d0031-0c5b-4918-8e2a-32e4df8c349c"), 25, 3, "Join us for a fun and energetic Zumba dance workout.", 60, 1, true, 0, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), 3, "Zumba Dance Party" },
                    { new Guid("9eaa793b-c65e-4fb1-aecc-58690967bb0f"), 12, 2, "Improve your flexibility with this focused Pilates class.", 50, 1, true, 7, new DateTime(2024, 12, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, "Pilates for Flexibility" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("39cb58ae-25a0-4eae-8021-16f73996b0e3"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("919668ba-2eab-4916-8315-28500333882f"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("9e2d0031-0c5b-4918-8e2a-32e4df8c349c"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("9eaa793b-c65e-4fb1-aecc-58690967bb0f"));

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
    }
}
