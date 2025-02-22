using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Movie.Migrations
{
    /// <inheritdoc />
    public partial class addRelationshipBtActorAndMovie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_actorMovies_MovieFilmId",
                table: "actorMovies",
                column: "MovieFilmId");

            migrationBuilder.AddForeignKey(
                name: "FK_actorMovies_actors_ActorId",
                table: "actorMovies",
                column: "ActorId",
                principalTable: "actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_actorMovies_movies_MovieFilmId",
                table: "actorMovies",
                column: "MovieFilmId",
                principalTable: "movies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_actorMovies_actors_ActorId",
                table: "actorMovies");

            migrationBuilder.DropForeignKey(
                name: "FK_actorMovies_movies_MovieFilmId",
                table: "actorMovies");

            migrationBuilder.DropIndex(
                name: "IX_actorMovies_MovieFilmId",
                table: "actorMovies");
        }
    }
}
