using Microsoft.EntityFrameworkCore.Migrations;

namespace aemtest.Migrations
{
    public partial class addsyncId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SyncId",
                table: "Wells",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SyncId",
                table: "Platforms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Wells_SyncId",
                table: "Wells",
                column: "SyncId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Platforms_SyncId",
                table: "Platforms",
                column: "SyncId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wells_SyncId",
                table: "Wells");

            migrationBuilder.DropIndex(
                name: "IX_Platforms_SyncId",
                table: "Platforms");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Wells");

            migrationBuilder.DropColumn(
                name: "SyncId",
                table: "Platforms");
        }
    }
}
