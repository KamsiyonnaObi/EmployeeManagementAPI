using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheEmployeeAPI.Migrations
{
    /// <inheritdoc />
    public partial class add_EmployeeBenefits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BenefitType",
                table: "EmployeeBenefits",
                newName: "BenefitId");

            migrationBuilder.CreateTable(
                name: "Benefits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    BaseCost = table.Column<decimal>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Benefits", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeBenefits_BenefitId",
                table: "EmployeeBenefits",
                column: "BenefitId");

            migrationBuilder.AddForeignKey(
                name: "FK_EmployeeBenefits_Benefits_BenefitId",
                table: "EmployeeBenefits",
                column: "BenefitId",
                principalTable: "Benefits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmployeeBenefits_Benefits_BenefitId",
                table: "EmployeeBenefits");

            migrationBuilder.DropTable(
                name: "Benefits");

            migrationBuilder.DropIndex(
                name: "IX_EmployeeBenefits_BenefitId",
                table: "EmployeeBenefits");

            migrationBuilder.RenameColumn(
                name: "BenefitId",
                table: "EmployeeBenefits",
                newName: "BenefitType");
        }
    }
}
