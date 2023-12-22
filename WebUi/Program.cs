using Domain;
using WebUi.Models;
using MvcExtensions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles("/resources");

app
    .MapGroup("api")
    .MapGetRoute("health", () => "healthy")
    .MapGetRoute("conway/catalog", () => Domain.Catalog.Index.All)
    .MapPostRoute("conway/states", ([AsParameters] Criteria criteria, SeedRequest seed) => Game
        .Init(seed.Turn, seed.Grid)
        .Play()
        .Take(criteria.Turns)); // super duper improvement would be to just send the diffs. ^_^

app.Run();


// TODO: clean up
public record SeedRequest(int Turn, string[][] Grid);