using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SetupDeleteBehavior : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_Pages_PageId",
                table: "FileUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_Posts_PostId",
                table: "FileUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Pages_ParentId",
                table: "Pages");

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 12, 5, 17, 28, 14, 36, DateTimeKind.Utc).AddTicks(8010), new DateTime(2022, 12, 5, 17, 28, 14, 36, DateTimeKind.Utc).AddTicks(8020) });

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_Pages_PageId",
                table: "FileUploads",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_Posts_PostId",
                table: "FileUploads",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Pages_ParentId",
                table: "Pages",
                column: "ParentId",
                principalTable: "Pages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_Pages_PageId",
                table: "FileUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_FileUploads_Posts_PostId",
                table: "FileUploads");

            migrationBuilder.DropForeignKey(
                name: "FK_Pages_Pages_ParentId",
                table: "Pages");

            migrationBuilder.UpdateData(
                table: "Pages",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "ModifiedDate" },
                values: new object[] { new DateTime(2022, 12, 4, 9, 55, 9, 42, DateTimeKind.Utc).AddTicks(7920), new DateTime(2022, 12, 4, 9, 55, 9, 42, DateTimeKind.Utc).AddTicks(7920) });

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_Pages_PageId",
                table: "FileUploads",
                column: "PageId",
                principalTable: "Pages",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FileUploads_Posts_PostId",
                table: "FileUploads",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Pages_Pages_ParentId",
                table: "Pages",
                column: "ParentId",
                principalTable: "Pages",
                principalColumn: "Id");
        }
    }
}
