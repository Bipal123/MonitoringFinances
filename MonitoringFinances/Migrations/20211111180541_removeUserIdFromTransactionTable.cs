using Microsoft.EntityFrameworkCore.Migrations;

namespace MonitoringFinances.Migrations
{
    public partial class removeUserIdFromTransactionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_AspNetUsers_UserId",
                table: "Record");

            migrationBuilder.DropIndex(
                name: "IX_Record_UserId",
                table: "Record");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Record");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Record",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Record_UserId",
                table: "Record",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_AspNetUsers_UserId",
                table: "Record",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
