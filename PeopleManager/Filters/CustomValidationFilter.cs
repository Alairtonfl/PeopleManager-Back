using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PeopleManager.API.Filters
{
    public class CustomValidationFilter : IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(e => e.Value.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(err => err.ErrorMessage).ToArray()
                    );

                context.Result = new BadRequestObjectResult(new
                {
                    success = false,
                    errors = errors
                });
            }
        }
    }
}
