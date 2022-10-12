using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Endava.BookSharing.Infrastructure.Migrations
{
    public partial class MakeBookCoverNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Files_CoverId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_PostedById",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "PostedById",
                table: "Reviews",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CoverId",
                table: "Books",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Files_CoverId",
                table: "Books",
                column: "CoverId",
                principalTable: "Files",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_PostedById",
                table: "Reviews",
                column: "PostedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_Files_CoverId",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_AspNetUsers_PostedById",
                table: "Reviews");

            migrationBuilder.AlterColumn<string>(
                name: "PostedById",
                table: "Reviews",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "CoverId",
                table: "Books",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Books_Files_CoverId",
                table: "Books",
                column: "CoverId",
                principalTable: "Files",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_AspNetUsers_PostedById",
                table: "Reviews",
                column: "PostedById",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
