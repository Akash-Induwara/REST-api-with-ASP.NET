using GameStore.api.Data;
using GameStore.api.Endpoints;
using GameStore.api.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});



builder.Services.AddValidation();
builder.AddGameStoreDb();

var app = builder.Build();

app.UseCors();
app.MapGamesEndpoints();
app.MapGenresEndpoints();

app.MigrateDb();

app.Run();
