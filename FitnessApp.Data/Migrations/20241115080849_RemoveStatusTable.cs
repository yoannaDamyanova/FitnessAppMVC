using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveStatusTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Statuses_StatusId",
                table: "Classes");

            migrationBuilder.DropTable(
                name: "Statuses");

            migrationBuilder.DropIndex(
                name: "IX_Classes_StatusId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Classes");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "5731a6db-ed86-431a-9d00-4c48dac1de4d", "AQAAAAIAAYagAAAAENWo39GYx6NQ/To/ZfMw5MJcZngdQsnhySoNTG7S5qQJTJsDdDLhLyyh4jr/r+3IJA==", "058cdf1d-1c5d-497d-b0ec-23c4fd68b4d0" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "33eb341c-6b2a-4b73-8bb1-9c3519e2db8c", "AQAAAAIAAYagAAAAEOXl6TYYucxtMWXjKmGn/JAO0RiaGnA08UPQGlBE/c/0yVCH7aO9Qe0hoPKQW6lvXw==", "6b90603e-2b41-46d7-884c-4724a25c99f2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Classes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Statuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statuses", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "76731596-ac75-415b-9870-dc887b930410", "AQAAAAIAAYagAAAAEI89r+37iLfZKD3P4NfaR5qf0m4x2XHzZE1/bMCZE6q9Pg0z93M33/7XtVS60JzmAw==", "2ca3978c-96ac-4943-b977-dab8f4f01e73" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "ac0ae362-601d-4fc6-9292-28c01b3c6af5", "AQAAAAIAAYagAAAAEE5Ap8hEnOpsLXMa/xDfAJ4vY++6sVBVIHv/IKNV8zYwc9AEnpoVmaMAxVo5t7lb+g==", "2d151de4-38b4-4051-a2fa-8dfa60f39b86" });

            migrationBuilder.CreateIndex(
                name: "IX_Classes_StatusId",
                table: "Classes",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Statuses_StatusId",
                table: "Classes",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id");
        }
    }
}
