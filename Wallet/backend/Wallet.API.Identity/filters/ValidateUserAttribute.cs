using Microsoft.AspNetCore.Mvc.Filters;

namespace Wallet.API.Identity.filters;

public class ValidateUserAttribute : ActionFilterAttribute {
    public override void OnActionExecuting(ActionExecutingContext context) {

    }
}
