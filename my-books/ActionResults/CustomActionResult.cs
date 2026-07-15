using Microsoft.AspNetCore.Mvc;
using my_books.Data.ViewModels;

namespace my_books.ActionResults
{
    public class CustomActionResult : ActionResult
    {
        private readonly CustomActionResultVM _result;

        public CustomActionResult(CustomActionResultVM result)
        {
            _result = result;
        }

        public override async Task ExecuteResultAsync(ActionContext context)
        {
            var objectResult = new ObjectResult(_result.Exception ?? _result.Publisher as object)
            {
                StatusCode = _result.Exception != null ? StatusCodes.Status500InternalServerError : StatusCodes.Status200OK
            };

            await objectResult.ExecuteResultAsync(context);
        }
    }
}
