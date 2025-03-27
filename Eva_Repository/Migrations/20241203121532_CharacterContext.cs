using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eva_Repository.Migrations
{
    /// <inheritdoc />
    public partial class CharacterContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CharacterDefinitions",
                columns: table => new
                {
                    CharacterId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SystemPrompt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CombinedKB = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CharacterDefinitions", x => x.CharacterId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CharacterDefinitions");
        }
    }
}
