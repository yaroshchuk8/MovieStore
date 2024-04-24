using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MovieStore.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeedJoinTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Actors",
                columns: new[] { "Id", "ImageUrl", "Name" },
                values: new object[,]
                {
                    { 1, "\\images\\actor\\6753d8f9-22f0-461e-b7b8-ab7f1ea2aa56.jpg", "Leonardo DiCaprio" },
                    { 2, "\\images\\actor\\741cbe51-3a8f-4a11-9b28-dc43d5e3b866.jpg", "Cillian Murphy" },
                    { 3, "\\images\\actor\\902f1b7f-0655-4f50-8a98-f5e9ba6a7990.jpg", "Anne Hathaway" },
                    { 4, "\\images\\actor\\a82fa331-1a40-4c01-8d7a-be71d4e8c2ad.jpg", "Woody Harrelson" },
                    { 5, "\\images\\actor\\b5bb2e2b-d2c4-4dbd-8b4f-e1ac8c9abe29.jpg", "Matthew McConaughey" },
                    { 6, "\\images\\actor\\d572b29f-91c9-4bf9-8d25-2b96f1b52d58.jpg", "Robert Downey Jr." }
                });

            migrationBuilder.InsertData(
                table: "Genres",
                columns: new[] { "Id", "Name" },
                values: new object[] { 6, "Thriller" });

            migrationBuilder.InsertData(
                table: "Movies",
                columns: new[] { "Id", "Description", "ImageUrl", "Price", "Title" },
                values: new object[,]
                {
                    { 1, "When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft, along with a team of researchers, to find a new planet for humans.", "\\images\\movie\\8ea24e62-0af2-4aa6-8ecb-fd46822b06e3.jpg", 49.0, "Interstellar" },
                    { 2, "During World War II, Lt. Gen. Leslie Groves Jr. appoints physicist J. Robert Oppenheimer to work on the top-secret Manhattan Project. Oppenheimer and a team of scientists spend years developing and designing the atomic bomb. Their work comes to fruition on July 16, 1945, as they witness the world's first nuclear explosion, forever changing the course of history.", "\\images\\movie\\e9837bd3-d65d-4a1d-b8e4-9c3bed0cf0ed.jpg", 59.0, "Oppenheimer" },
                    { 3, "Police officers and detectives around the USA are forced to face dark secrets about themselves and the people around them while investigating homicides.", "\\images\\movie\\4c662a58-4bbe-45cc-96d1-7f14e7870c91.jpg", 39.0, "True Detective" },
                    { 4, "Cobb steals information from his targets by entering their dreams. He is wanted for his alleged role in his wife's murder and his only chance at redemption is to perform a nearly impossible task.", "\\images\\movie\\de245963-1f4d-44c4-afdb-117660ccf946.jpg", 69.0, "Inception" }
                });

            migrationBuilder.InsertData(
                table: "MovieActor",
                columns: new[] { "ActorsId", "MoviesId" },
                values: new object[,]
                {
                    { 1, 4 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 1 },
                    { 4, 2 },
                    { 5, 1 },
                    { 5, 2 },
                    { 6, 3 }
                });

            migrationBuilder.InsertData(
                table: "MovieGenre",
                columns: new[] { "GenresId", "MoviesId" },
                values: new object[,]
                {
                    { 1, 1 },
                    { 2, 1 },
                    { 2, 3 },
                    { 2, 4 },
                    { 3, 3 },
                    { 4, 2 },
                    { 5, 1 },
                    { 5, 4 },
                    { 6, 2 },
                    { 6, 3 },
                    { 6, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MovieActor",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 1, 4 });

            migrationBuilder.DeleteData(
                table: "MovieActor",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "MovieActor",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "MovieActor",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 3, 1 });

            migrationBuilder.DeleteData(
                table: "MovieActor",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "MovieActor",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "MovieActor",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 5, 2 });

            migrationBuilder.DeleteData(
                table: "MovieActor",
                keyColumns: new[] { "ActorsId", "MoviesId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 1, 1 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 2, 1 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 2, 3 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 2, 4 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 3, 3 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 4, 2 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 5, 1 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 5, 4 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 6, 3 });

            migrationBuilder.DeleteData(
                table: "MovieGenre",
                keyColumns: new[] { "GenresId", "MoviesId" },
                keyValues: new object[] { 6, 4 });

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Actors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Genres",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Movies",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
