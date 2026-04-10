using GameStore.api.Data;
using GameStore.api.Endpoints;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStoreDb");

builder.Services.AddSqlServer<GameStoreContext>(connectionString);

var app = builder.Build();

app.MapGamesEndpoints();

app.MigrateDb();

app.Run();
