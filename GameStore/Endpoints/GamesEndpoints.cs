using GameStore.api.DTOs;

namespace GameStore.api.Endpoints
{
    public static class GamesEndpoints
    {
        const string GetGameEndpointName = "GetGame";

        private static readonly List<GameDto> games = [
            new(1, "The Witcher 3: Wild Hunt", "RPG", 39.99m, new DateTime(2015, 5, 19)),
            new(2, "Cyberpunk 2077", "Action RPG", 59.99m, new DateTime(2020, 12, 10)),
            new(3, "Stardew Valley", "Simulation", 14.99m, new DateTime(2016, 2, 26)),
            new(4, "Elden Ring", "Souls-like", 59.99m, new DateTime(2022, 2, 25)),
            new(5, "Minecraft", "Sandbox", 26.95m, new DateTime(2011, 11, 18))
    ];

        public static void MapGamesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/games");

            //GET /games
            group.MapGet("/", () => games);


            //GET /games/{id}
            group.MapGet("/{id}", (int id) =>
            {
                var game = games.Find(g => g.Id == id);
                return game is null ? Results.NotFound() : Results.Ok(game);
            }).WithName(GetGameEndpointName);

            //POST /games
            group.MapPost("/", (CreateGameDto newGame) =>
            {

                GameDto game = new(
                    games.Count + 1,
                    newGame.Title,
                    newGame.Genre,
                    newGame.Price,
                    newGame.ReleaseDate
                );
                games.Add(game);
                return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id }, game);
            });

            //PUT /games/{id}
            group.MapPut("/{id}", (int id, UpdateGameDto updateGame) =>
            {
                var index = games.FindIndex(g => g.Id == id);

                if (index == -1)
                {
                    return Results.NotFound();
                }

                games[index] = new GameDto(
                    id,
                    updateGame.Title,
                    updateGame.Genre,
                    updateGame.Price,
                    updateGame.ReleaseDate
                );
                return Results.NoContent();
            });

            //DELETE /games/{id}
            group.MapDelete("/{id}", (int id) =>
            {
                games.RemoveAll(g => g.Id == id);
                return Results.NoContent();
            });
        }
    }
}
