using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularDemoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddSchoolIdToSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "Section",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Section_SchoolId",
                table: "Section",
                column: "SchoolId");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_School_SchoolId",
                table: "Section",
                column: "SchoolId",
                principalTable: "School",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Section_School_SchoolId",
                table: "Section");

            migrationBuilder.DropIndex(
                name: "IX_Section_SchoolId",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "Section");
        }
    }
}
