﻿using HB.Admin.Models;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace HB.Admin.Services
{

    public class AdminAuthorizationHandler : AuthorizationHandler<AdminAuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AdminAuthorizationRequirement requirement)
        {

            if (context.User.Identity.IsAuthenticated)
            {
                if (context.Resource is Microsoft.AspNetCore.Mvc.Filters.AuthorizationFilterContext mvcContext)
                {
                    var permissionService = mvcContext.HttpContext.RequestServices.GetService<IPermissionService>();

                    if (permissionService != null && permissionService.Authorize(requirement.Policy))
                    {
                        context.Succeed(requirement);
                    }
                    else
                    {
                        context.Fail();
                    }
                }
                else
                {
                    context.Fail();
                }
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
