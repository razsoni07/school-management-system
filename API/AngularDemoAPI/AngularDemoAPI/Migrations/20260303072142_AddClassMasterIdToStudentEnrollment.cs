using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularDemoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddClassMasterIdToStudentEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassMasterId",
                table: "StudentEnrollment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StudentEnrollment_ClassMasterId",
                table: "StudentEnrollment",
                column: "ClassMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentEnrollment_ClassMaster_ClassMasterId",
                table: "StudentEnrollment",
                column: "ClassMasterId",
                principalTable: "ClassMaster",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentEnrollment_ClassMaster_ClassMasterId",
                table: "StudentEnrollment");

            migrationBuilder.DropIndex(
                name: "IX_StudentEnrollment_ClassMasterId",
                table: "StudentEnrollment");

            migrationBuilder.DropColumn(
                name: "ClassMasterId",
                table: "StudentEnrollment");
        }
    }
}
