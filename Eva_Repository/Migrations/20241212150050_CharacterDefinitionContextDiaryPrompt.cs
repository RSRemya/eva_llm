using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eva_Repository.Migrations
{
    /// <inheritdoc />
    public partial class CharacterDefinitionContextDiaryPrompt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DiaryPrompt",
                table: "CharacterDefinitions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiaryPrompt",
                table: "CharacterDefinitions");
        }
    }
}
