using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class fixAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "c9580644-1fe7-486f-aab4-4fbc0aecfb11", "AQAAAAIAAYagAAAAECSsEoul5XL6b6UghX5pltFxQ4j9cKMukvU9y/uXV7cKE1tGzgwS5MJX4SeRhiNuCg==", "0ac7feca-0ea2-408d-aa2e-b335a60b6713" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "4fbcf0da-af7a-44d1-88c1-b19df38b7283", "AQAAAAIAAYagAAAAEA5ILfN7cOPJP0sFxttyl90gvT1P5CQoLW5iXy6H8PyLy0DDM8CjvDnqhjhpoxdLvQ==", "f5166c65-a2e9-44fd-9ead-e4b9ab7d1367" });

            migrationBuilder.InsertData(
                table: "Classes",
                columns: new[] { "Id", "Capacity", "CategoryId", "Description", "Duration", "InstructorId", "IsApproved", "LeftCapacity", "StartTime", "StatusId", "Title" },
                values: new object[,]
                {
                    { new Guid("1e5443fb-c689-4aad-96ec-1cdaa720d7ca"), 25, 3, "Join us for a fun and energetic Zumba dance workout.", 60, 1, true, 0, new DateTime(2024, 12, 5, 18, 0, 0, 0, DateTimeKind.Unspecified), 3, "Zumba Dance Party" },
                    { new Guid("42a2a803-8689-4555-912b-d459a9a750c8"), 12, 2, "Improve your flexibility with this focused Pilates class.", 50, 1, true, 7, new DateTime(2024, 12, 12, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, "Pilates for Flexibility" },
                    { new Guid("a328c0b3-2b48-433b-9e36-fc89f220bf4e"), 20, 1, "Start your day with a relaxing yoga session to stretch and recharge.", 60, 1, true, 20, new DateTime(2024, 12, 22, 7, 0, 0, 0, DateTimeKind.Unspecified), 1, "Morning Yoga" },
                    { new Guid("e8431f68-85a7-40ff-9f5b-681bda10a3e8"), 20, 6, "Build strength and stamina with high-intensity interval training.", 40, 1, true, 0, new DateTime(2024, 12, 11, 14, 30, 0, 0, DateTimeKind.Unspecified), 4, "HIIT Strength Training" }
                });

            migrationBuilder.InsertData(
                table: "Instructors",
                columns: new[] { "Id", "Biography", "LicenseNumber", "Rating", "Specializations", "UserId" },
                values: new object[] { 2, "Great Admin is a dedicated fitness instructor with over a decade of experience in personal training and group fitness.Known for his motivating style and tailored workout plans, he specializes in strength training, HIIT, and functional fitness.John's mission is to help clients achieve their health goals while fostering a love for fitness. His approach combines expert guidance with an emphasis on form, endurance, and mental resilience, making him a trusted coach for all fitness levels.", 300769, 5.0, "Strength training, HIIT, functional fitness, and personalized programs for all fitness levels.", "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("1e5443fb-c689-4aad-96ec-1cdaa720d7ca"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("42a2a803-8689-4555-912b-d459a9a750c8"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("a328c0b3-2b48-433b-9e36-fc89f220bf4e"));

            migrationBuilder.DeleteData(
                table: "Classes",
                keyColumn: "Id",
                keyValue: new Guid("e8431f68-85a7-40ff-9f5b-681bda10a3e8"));

            migrationBuilder.DeleteData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 2);

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
    }
}
