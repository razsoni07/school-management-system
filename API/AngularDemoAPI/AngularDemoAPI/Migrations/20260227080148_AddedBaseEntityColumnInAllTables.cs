using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularDemoAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddedBaseEntityColumnInAllTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Section_ClassMaster_ClassId",
                table: "Section");

            migrationBuilder.DropIndex(
                name: "IX_Section_ClassId",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "ClassId",
                table: "Section");

            migrationBuilder.RenameColumn(
                name: "GradeId",
                table: "Section",
                newName: "ClassMasterId");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "StudentEnrollment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "StudentEnrollment",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "StudentEnrollment",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "StudentEnrollment",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Section",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Section",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Section",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Section",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Roles",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Department",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "Department",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "Department",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "ClassMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "ClassMaster",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "ClassMaster",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "ClassMaster",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "AcademicYear",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "AcademicYear",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UpdatedBy",
                table: "AcademicYear",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedDate",
                table: "AcademicYear",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Section_ClassMasterId",
                table: "Section",
                column: "ClassMasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_ClassMaster_ClassMasterId",
                table: "Section",
                column: "ClassMasterId",
                principalTable: "ClassMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Section_ClassMaster_ClassMasterId",
                table: "Section");

            migrationBuilder.DropIndex(
                name: "IX_Section_ClassMasterId",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "StudentEnrollment");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "StudentEnrollment");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "StudentEnrollment");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "StudentEnrollment");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Section");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "Department");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "ClassMaster");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "ClassMaster");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "ClassMaster");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "ClassMaster");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "AcademicYear");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "AcademicYear");

            migrationBuilder.DropColumn(
                name: "UpdatedBy",
                table: "AcademicYear");

            migrationBuilder.DropColumn(
                name: "UpdatedDate",
                table: "AcademicYear");

            migrationBuilder.RenameColumn(
                name: "ClassMasterId",
                table: "Section",
                newName: "GradeId");

            migrationBuilder.AddColumn<int>(
                name: "ClassId",
                table: "Section",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Section_ClassId",
                table: "Section",
                column: "ClassId");

            migrationBuilder.AddForeignKey(
                name: "FK_Section_ClassMaster_ClassId",
                table: "Section",
                column: "ClassId",
                principalTable: "ClassMaster",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
