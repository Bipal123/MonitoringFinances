using Microsoft.EntityFrameworkCore.Migrations;

namespace MonitoringFinances.Migrations
{
    public partial class changeTransactionTableNameToRecord : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_AspNetUsers_UserId",
                table: "Transaction");

            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                table: "Transaction");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction");

            migrationBuilder.RenameTable(
                name: "Transaction",
                newName: "Record");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_UserId",
                table: "Record",
                newName: "IX_Record_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Transaction_CategoryId",
                table: "Record",
                newName: "IX_Record_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Record",
                table: "Record",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Record_AspNetUsers_UserId",
                table: "Record",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Record_Category_CategoryId",
                table: "Record",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Record_AspNetUsers_UserId",
                table: "Record");

            migrationBuilder.DropForeignKey(
                name: "FK_Record_Category_CategoryId",
                table: "Record");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Record",
                table: "Record");

            migrationBuilder.RenameTable(
                name: "Record",
                newName: "Transaction");

            migrationBuilder.RenameIndex(
                name: "IX_Record_UserId",
                table: "Transaction",
                newName: "IX_Transaction_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Record_CategoryId",
                table: "Transaction",
                newName: "IX_Transaction_CategoryId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Transaction",
                table: "Transaction",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_AspNetUsers_UserId",
                table: "Transaction",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                table: "Transaction",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
