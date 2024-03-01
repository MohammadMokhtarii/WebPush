using Core.Application.Common;
using Google.Protobuf.WellKnownTypes;
using Google.Rpc;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Core.Api.Core;
public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception, "Exception occurred: {Message}", exception.Message);

        ProblemDetails problemDetails;
        if (exception is BussinessValidtion bussinessValidtion)
        {
            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status400BadRequest,
                Type = "ValidationFailure",
                Detail = "One or more validation errors has occurred"
            };
            if (bussinessValidtion.Errors is not null)
            {
                problemDetails.Extensions["errors"] = bussinessValidtion.Errors;
            }
        }
        else
        {
            problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
            };
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response
            .WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}

public class GrpcGlobalExceptionHandlerInterceptor(ILogger<GrpcGlobalExceptionHandlerInterceptor> logger) : Interceptor
{
    private readonly ILogger<GrpcGlobalExceptionHandlerInterceptor> _logger = logger;
    public override async Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request,
        ServerCallContext context,
        UnaryServerMethod<TRequest, TResponse> continuation)
    {
        try
        {
            return await base.UnaryServerHandler(request, context, continuation);
        }
        catch (BussinessValidtion bussinessValidtion)
        {

            _logger.LogError(bussinessValidtion, "Exception occurred: {Message}", bussinessValidtion.Message);

            var errors = bussinessValidtion.Errors.Select(row => new BadRequest.Types.FieldViolation
            {
                Field = row.PropertyName,
                Description = $"{row.Code}:{row.Message}"
            }).ToList();

            throw new Google.Rpc.Status
            {
                Code = (int)Code.InvalidArgument,
                Message = "One or more validation errors has occurred",
                Details =
                {
                    Any.Pack(new BadRequest
                    {
                        FieldViolations =  {errors},
                    })
                }

            }.ToRpcException();
        }
    }

}
