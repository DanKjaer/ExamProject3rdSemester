using System.Security.Authentication;
using Microsoft.AspNetCore.Mvc.Filters;

namespace api.filters;

public class RequireAuthentication : ActionFilterAttribute
{
    //kommenter dette kode så vi forstår det!
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.HttpContext.GetSessionData() == null) throw new AuthenticationException();
    }
}