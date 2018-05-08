using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BinaryCook.Application.Web.Mvc.Attributes
{
    public class ClientRequirementAttribute : TypeFilterAttribute
    {
        public ClientRequirementAttribute(params string[] clientIds) : base(typeof(ClientRequirementFilter))
        {
            Arguments = new object[] {clientIds};
        }

        private class ClientRequirementFilter : IAuthorizationFilter
        {
            private readonly IList<string> _clientIds;

            public ClientRequirementFilter(IList<string> clientIds)
            {
                _clientIds = clientIds;
            }

            public void OnAuthorization(AuthorizationFilterContext context)
            {
                var user = context.HttpContext.User;
                //TODO: Improve this
                var hasClaim = user.Claims.Any(c => /*c.Type == _claim.Type && */_clientIds.Contains(c.Value));

                if (hasClaim) return;

                context.Result = new ForbidResult();
            }
        }
    }
}