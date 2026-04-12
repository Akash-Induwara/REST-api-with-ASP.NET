using GameStore.api.DTOs;
using GameStore.api.Data;
using GameStore.api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.api.Endpoints
{
    public static class GamesEndpoints
    {
        const string GetGameEndpointName = "GetGame";

        public static void MapGamesEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/games");

            //GET /games
            group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games.Include(game => game.Genre).Select(game => new GameSummaryDto(
                game.Id,
                game.Title,
                game.Genre!.Name,
                game.Price,
                game.ReleaseDate)).AsNoTracking().ToListAsync());


            //GET /games/{id}
            group.MapGet("/{id}",async (int id, GameStoreContext dbContext) =>
            {
                var game = await dbContext.Games.FindAsync(id);
                return game is null ? Results.NotFound() : Results.Ok( new
                    GameDetailsDto(
                    game.Id,
                    game.Title,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate));
            }).WithName(GetGameEndpointName);
            
            //POST /games
            group.MapPost("/", async (CreateGameDto newGame, GameStoreContext dbContext) =>
            {

                Game game = new()
                {
                    Title = newGame.Title,
                    GenreId = newGame.GenreId,
                    Price = newGame.Price,
                    ReleaseDate = newGame.ReleaseDate
                };
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();

                GameDetailsDto gameDto = new(
                    game.Id,
                    game.Title,
                    game.GenreId,
                    game.Price,
                    game.ReleaseDate
                );

                return Results.CreatedAtRoute(GetGameEndpointName, new { id = gameDto.Id }, gameDto);
            });

            //PUT /games/{id}
            group.MapPut("/{id}", async (int id, UpdateGameDto updateGame , GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.FindAsync(id);

                if (existingGame is null)
                {
                    return Results.NotFound();
                }

                existingGame.Title = updateGame.Title;
                existingGame.GenreId = updateGame.GenreId;
                existingGame.Price = updateGame.Price;
                existingGame.ReleaseDate = updateGame.ReleaseDate;

                await dbContext.SaveChangesAsync();

                return Results.NoContent();
            });

            //DELETE /games/{id}
            group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                var existingGame = await dbContext.Games.Where(g => g.Id == id).ExecuteDeleteAsync();

                return Results.NoContent();
            });
        }
    }
}
