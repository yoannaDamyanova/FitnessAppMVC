using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class StatusToBool : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Statuses_StatusId",
                table: "Classes");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Classes",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Classes",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Statuses_StatusId",
                table: "Classes",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Classes_Statuses_StatusId",
                table: "Classes");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Classes");

            migrationBuilder.AlterColumn<int>(
                name: "StatusId",
                table: "Classes",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Classes_Statuses_StatusId",
                table: "Classes",
                column: "StatusId",
                principalTable: "Statuses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
