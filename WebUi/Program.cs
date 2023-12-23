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
    .MapPostRoute("conway/states", ([AsParameters] Criteria criteria, Seed seed) => Game // TODO: pass along entire frontend control 
        .Init(seed.Turn, seed.Grid)
        .Play()
        .Take(criteria.Turns));

app.Run();