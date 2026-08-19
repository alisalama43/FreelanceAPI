using Microsoft.AspNetCore.Mvc.Filters;

namespace FreelanceAPI.Filters
{
    public class CalculateActionTimeFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Items["ActionStartTime"] = DateTime.UtcNow;

            await next();
            var startTime = (DateTime)context.HttpContext.Items["ActionStartTime"]!;
            var elapsedTime = DateTime.UtcNow - startTime;
            context.HttpContext.Response.Headers.Append("X-Action-Time", $"{elapsedTime.TotalMilliseconds}");
            Console.WriteLine("Action Took {0} ms ", elapsedTime.TotalMicroseconds);

        }
    }
}
