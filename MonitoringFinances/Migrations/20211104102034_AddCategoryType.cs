using Microsoft.EntityFrameworkCore.Migrations;

namespace MonitoringFinances.Migrations
{
    public partial class AddCategoryType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CategoryType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryType", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CategoryType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Income" });

            migrationBuilder.InsertData(
                table: "CategoryType",
                columns: new[] { "Id", "Name" },
                values: new object[] { 2, "Expense" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CategoryType");
        }
    }
}
