using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Services.Common;

namespace Core.Api.Filters;

public class ApiResultAttribute : ActionFilterAttribute
{
    public override void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is OkObjectResult okObjectResult && okObjectResult.Value is Result result)
        {
            if (result.Error != Error.None)
            {
                ProblemDetails problemDetails = new()
                {
                    Status = (int)result.Error.ErrorType,
                    Type = result.Error.ErrorType.ToString(),
                    Detail = result.Error.ErrorType switch
                    {
                        ErrorType.Validation => "One or more validation errors has occurred",
                        ErrorType.NotFound => "Required Resource Not Found",
                        ErrorType.ServerError => "Server Error has occurred",
                        _ => ""
                    }
                };
                problemDetails.Extensions["errors"] = new List<Error> { result.Error };
                context.Result = new OkObjectResult(problemDetails) { StatusCode = problemDetails.Status };
            }
            else if (result.GetType().IsAssignableToGenericType(typeof(Result<>)))
            {
                var data = result?.GetType()?.GetProperty(nameof(Result<object>.Data))?.GetValue(result, null);
                context.Result = new OkObjectResult(data);
            }
            else
                context.Result = new OkResult();

        }
    }
}
