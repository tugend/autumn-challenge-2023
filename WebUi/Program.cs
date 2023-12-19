using Domain;
using Domain.Catalog;
using WebUi.Models;
using MvcExtensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles("/resources");

app
    .MapGroup("api")
    .MapGetRoute("health", () => "healthy")
    .MapGetRoute("catalog", () => StillLife.All.Concat(Oscillators.All).Concat(Spaceships.All))
    .MapPostRoute("conway", ([AsParameters] Criteria criteria, Seed seed) => Game
        .Init(seed.Turn, seed.Grid)
        .Play()
        .Take(criteria.Turns)); // super duper improvement would be to just send the diffs. ^_^

app.Run();