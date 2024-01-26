using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace MvcExtensions;

public static class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapGetRoute(this RouteGroupBuilder group, string route, Delegate handler)
    {
        group.MapGet(route, handler);
        return group;
    }
    
    public static void MapPostRoute(this RouteGroupBuilder group, string route, Delegate handler)
    {
        group.MapPost(route, handler);
    }
}