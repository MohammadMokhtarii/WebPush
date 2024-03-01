using Core.Domain.Segment;
using Google.Rpc;
using Grpc.Core;
using Services.Common;

namespace Core.Api.Core;

public static class Extentions
{
    public static ClientMetadata ExtractClientMetadata(this HttpContext httpContext)
    {
        return ClientMetadata.Create("Android");
    }

    public static void MapToGrpcException(this Error error)
    {
        var grpcStatus = error.ErrorType switch
        {
            ErrorType.NotFound => Code.NotFound,
            ErrorType.Validation => Code.InvalidArgument,
            ErrorType.ServerError => Code.Internal,
            _ => Code.Internal
        };
        throw new Google.Rpc.Status
        {
            Code = (int)grpcStatus,
            Message = $"{error.Code}:{error.Message}",
        }.ToRpcException();
    }
}

