using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace MvcExtensions;

public static partial class RouteGroupBuilderExtensions
{
    public static RouteGroupBuilder MapGetRoute(this RouteGroupBuilder group, string route, Delegate handler)
    {
        group.MapGet(route, handler);
        return group;
    }
}