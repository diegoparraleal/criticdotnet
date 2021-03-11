using Critic.Models;
using Critic.Services;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Critic.Authorization
{
    public class GoogleAuthorizeAttribute : TypeFilterAttribute
    {
        public GoogleAuthorizeAttribute(params AppUser.Roles[] roles) : base(typeof(GoogleAuthorizeFilter)) {
            Arguments = new object[] { roles };
        }
    }


    public class GoogleAuthorizeFilter : IAuthorizationFilter
    {

        private AppUser.Roles[] _roles;
        public GoogleAuthorizeFilter(params AppUser.Roles[] roles)
        {
            this._roles = roles;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var configuration = (IConfiguration) context.HttpContext.RequestServices.GetRequiredService(typeof(IConfiguration));
            if (configuration.GetValue<bool?>("DisableTokenValidation") == true) return;

            var businessService = (CriticBusinessService) context.HttpContext.RequestServices.GetService(typeof(CriticBusinessService));
            try
            {
                if (!context.HttpContext.User.Identity.IsAuthenticated) {
                    context.Result = new ForbidResult();
                    return;
                }
                var email = context.HttpContext.User.FindFirst("email")?.Value;
                var user = businessService.FindUserByEmailSync(email);
                
                if (_roles.Length > 0 && !_roles.Any(r => r == user.Role)) {
                    context.Result = new ForbidResult();
                    return;
                }
            }
            catch (Exception)
            {
                context.Result = new ForbidResult();
            }
        }
    }

}
