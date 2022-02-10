using Microsoft.EntityFrameworkCore.Migrations;

namespace MonitoringFinances.Migrations
{
    public partial class updateCategoryToIncludeCategoryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CategoryTypeId",
                table: "Category",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Category_CategoryTypeId",
                table: "Category",
                column: "CategoryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Category_CategoryType_CategoryTypeId",
                table: "Category",
                column: "CategoryTypeId",
                principalTable: "CategoryType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Category_CategoryType_CategoryTypeId",
                table: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Category_CategoryTypeId",
                table: "Category");

            migrationBuilder.DropColumn(
                name: "CategoryTypeId",
                table: "Category");
        }
    }
}
