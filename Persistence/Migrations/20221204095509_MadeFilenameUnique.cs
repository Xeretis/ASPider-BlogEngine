using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MadeFilenameUnique : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 12, 4, 9, 55, 9, 42, DateTimeKind.Utc).AddTicks(7920), new DateTime(2022, 12, 4, 9, 55, 9, 42, DateTimeKind.Utc).AddTicks(7920) });

            migrationBuilder.CreateIndex(
                name: "IX_FileUploads_Filename",
                table: "FileUploads",
                column: "Filename",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FileUploads_Filename",
                table: "FileUploads");

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 12, 1, 20, 16, 49, 355, DateTimeKind.Utc).AddTicks(9820), new DateTime(2022, 12, 1, 20, 16, 49, 355, DateTimeKind.Utc).AddTicks(9820) });
        }
    }
}
