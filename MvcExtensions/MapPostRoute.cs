using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace MvcExtensions;

public static partial class RouteGroupBuilderExtensions
{
    public static void MapPostRoute(this RouteGroupBuilder group, string route, Delegate handler)
    {
        group.MapPost(route, handler);
    }
}