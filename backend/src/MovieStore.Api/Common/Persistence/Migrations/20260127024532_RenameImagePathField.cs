using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RenameImagePathField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "Actor",
                newName: "ImageKey");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageKey",
                table: "Actor",
                newName: "ImagePath");
        }
    }
}
