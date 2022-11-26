using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatePages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_AspNetUsers_CreatorId",
                table: "Pages");

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Pages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "Pages",
                columns: new[] { "Id", "Content", "CreatedDate", "CreatorId", "Description", "ModifiedDate", "ParentId", "ThumbnailUrl", "Title", "Visible" },
                values: new object[] { 1, "Index content, only shown on the page itself", new DateTime(2022, 11, 26, 10, 8, 53, 722, DateTimeKind.Utc).AddTicks(570), null, "Index description", new DateTime(2022, 11, 26, 10, 8, 53, 722, DateTimeKind.Utc).AddTicks(570), null, null, "Index", true });

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_AspNetUsers_CreatorId",
                table: "Pages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pages_AspNetUsers_CreatorId",
                table: "Pages");

            migrationBuilder.DeleteData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "CreatorId",
                table: "Pages",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_AspNetUsers_CreatorId",
                table: "Pages",
                column: "CreatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
