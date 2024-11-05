using Ems.BusinessLogic.Abstract;
using Microsoft.AspNetCore.Authorization;

namespace Ems.UI.Middleware;

public class AuthenticationHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public AuthenticationHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public Task InvokeAsync(HttpContext context)
    {
        var endpointList = context.GetEndpoint()?.Metadata.ToList();

        if (endpointList is not null)
        {
            var authAttrIndex = endpointList.FindIndex(
            x => x.GetType() == typeof(AuthorizeAttribute));

            if (authAttrIndex != -1)
            {
                var userService = context.RequestServices.GetService<IUserService>();
                userService.SetRegisterUser(context.User);
            }
        }
        return _next(context);
    }
}
