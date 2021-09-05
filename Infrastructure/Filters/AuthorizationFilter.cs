using Infrastructure.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Shared.Infrastructure.Core;
using System.Linq;
using System.Security.Claims;
using static Shared.Infrastructure.Core.Constants;

namespace Shared.Infrastructure.Filters
{
    public class AuthorizationFilter : IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            bool hasAllowAnonymous =
                context
                    .ActionDescriptor
                    .EndpointMetadata
                        .Any(attribute => attribute.GetType() == typeof(AllowAnonymousAttribute));

            if (hasAllowAnonymous)
                return;

            var _loggedUserService = context.HttpContext.RequestServices.GetService(typeof(ILoggedUserService)) as ILoggedUserService;
            if (_loggedUserService == null)
                throw new AppException(Messages.Error.ContactAdministrator);

            var requestTenant = context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == Headers.TENANT).Value.ToString();
            if (requestTenant == null)
                throw new AppException(Messages.Error.ContactAdministrator);

            var claims = ((ClaimsIdentity)context.HttpContext.User.Identity).Claims;
            var tenantClaim = claims.Where(x => x.Type == Claims.Tenant);
            var idClaim = claims.Where(x => x.Type == ClaimTypes.NameIdentifier).FirstOrDefault();

            _loggedUserService.SetUserId(idClaim);
            _loggedUserService.SetEmail(context.HttpContext.User.Identity.Name);
            _loggedUserService.SetClaims(claims.Except(tenantClaim));
            _loggedUserService.SetTenants(tenantClaim);
            _loggedUserService.SetRequestTenant(requestTenant);
        }
    }
}
