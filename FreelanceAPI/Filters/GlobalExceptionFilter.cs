using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FreelanceAPI.Filters
{
    public class GlobalExceptionFilter : IAsyncExceptionFilter
    {
        public Task OnExceptionAsync(ExceptionContext context)
        {
          var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
          {
              Title = "An unexpected error occurred.",
              Status = StatusCodes.Status500InternalServerError,
              Detail = context.Exception.Message
          };
            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = problemDetails.Status
            };
            // Log the exception (you can use a logging framework here)
            Console.WriteLine($"Exception: {context.Exception.Message}");
            Console.WriteLine($"Stack Trace: {context.Exception.StackTrace}");
            context.ExceptionHandled = true;
            return Task.CompletedTask;
        }
    }
}
