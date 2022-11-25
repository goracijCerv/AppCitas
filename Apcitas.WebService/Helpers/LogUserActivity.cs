﻿using Apcitas.WebService.Extensions;
using Apcitas.WebService.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Apcitas.WebService.Helpers;

public class LogUserActivity : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();

        if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

        var userid = resultContext.HttpContext.User.GetUserId();

        var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();
        var user = await repo.GetUserByIdAsync(userid);
        user.LastActive = DateTime.Now;
        await repo.SaveAllAsync();
    }
}
