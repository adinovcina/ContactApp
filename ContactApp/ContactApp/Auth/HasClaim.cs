using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactApp.Api.Auth
{
    public class HasClaim : AuthorizeAttribute, IActionFilter
    {
        private readonly string _claim;

        public HasClaim(string claim)
        {
            _claim = claim;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            var claim = context.HttpContext.User.Claims.SingleOrDefault(x => x.Type == _claim);

            if (claim?.Value is null)
            {
                context.Result = new UnauthorizedResult();
                return;
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
