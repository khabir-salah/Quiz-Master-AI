using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class thy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assesment_DocumentId",
                table: "Assesment");

            migrationBuilder.CreateIndex(
                name: "IX_Assesment_DocumentId",
                table: "Assesment",
                column: "DocumentId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Assesment_DocumentId",
                table: "Assesment");

            migrationBuilder.CreateIndex(
                name: "IX_Assesment_DocumentId",
                table: "Assesment",
                column: "DocumentId");
        }
    }
}
