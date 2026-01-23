using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieStore.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMovies : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PublisherProfileId",
                table: "Movie",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Movie_PublisherProfileId",
                table: "Movie",
                column: "PublisherProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Movie_PublisherProfile_PublisherProfileId",
                table: "Movie",
                column: "PublisherProfileId",
                principalTable: "PublisherProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Movie_PublisherProfile_PublisherProfileId",
                table: "Movie");

            migrationBuilder.DropIndex(
                name: "IX_Movie_PublisherProfileId",
                table: "Movie");

            migrationBuilder.DropColumn(
                name: "PublisherProfileId",
                table: "Movie");
        }
    }
}
