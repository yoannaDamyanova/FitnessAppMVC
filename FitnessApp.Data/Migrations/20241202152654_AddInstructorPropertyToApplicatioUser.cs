using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddInstructorPropertyToApplicatioUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors");

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

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors");

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

            migrationBuilder.CreateIndex(
                name: "IX_Instructors_UserId",
                table: "Instructors",
                column: "UserId");
        }
    }
}
