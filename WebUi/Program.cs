using Domain;
using WebUi;
using WebUi.Models;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.UseStaticFiles("/resources");

var group = app.MapGroup("api");
group.MapGet("conway", () => "hello world!");
group.MapPost("conway", ([AsParameters] Criteria criteria, Seed seed) =>
    Game
        .Init(seed.Turn, seed.Grid)
        .Play()
        .Take(criteria.Turns)); // super duper improvement would be to just send the diffs. ^_^

app.Run();

namespace WebUi
{
    public record Criteria(int Turns = 10);
    public partial class Program { }

}