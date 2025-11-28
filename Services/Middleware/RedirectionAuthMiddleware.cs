using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Middleware
{
    public class RedirectionAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public RedirectionAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            // Check whether user is authenticated
            if (context.User.Identity.IsAuthenticated)
            {
                // Check if this request is the login endpoint
                var path = context.Request.Path.Value?.ToLower();

                if (path == "/login" || path == "/account/login")
                {
                    context.Response.Redirect("/home");
                    return; // stop pipeline
                }
            }

            await _next(context);
        }
    }
}
