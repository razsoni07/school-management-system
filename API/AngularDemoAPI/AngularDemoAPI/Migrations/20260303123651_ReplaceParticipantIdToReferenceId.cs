using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AngularDemoAPI.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceParticipantIdToReferenceId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipantId",
                table: "MeetingParticipants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ParticipantId",
                table: "MeetingParticipants",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
