﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FitnessApp.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddFirstLicenseNumberManually : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "06e4a52b-0ee1-426f-b7a2-6b009a0c1f65",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "300fd942-3512-4143-8fe1-2dca2d064c3d", "AQAAAAIAAYagAAAAEGoppdk3z66xMfXh5PzKG/M8tmrE/tP5iva/5ytimMygu7J/vyjlQjpnqt76bSaeXg==", "3006c63b-2e92-4376-a01f-6ab113151333" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "69b38fdd-0aba-47f5-9f2b-6c7bb549d7ab",
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { "95e84b7b-cb74-44bf-bd93-84a9a45ce332", "AQAAAAIAAYagAAAAEAQnYQbr98Qo/2lI6oQaEnJc/fGYdyQCJ5TOMHGWeQQWoKImoR2wmin+F9PPIHQynA==", "d120a5d2-f158-4e5b-9744-648b154f51b5" });

            migrationBuilder.UpdateData(
                table: "Instructors",
                keyColumn: "Id",
                keyValue: 1,
                column: "LicenseNumber",
                value: 123456);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
