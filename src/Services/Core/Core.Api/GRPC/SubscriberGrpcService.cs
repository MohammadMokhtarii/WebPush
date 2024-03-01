using Core.Api.Core;
using Core.Application.Segment;
using Grpc.Core;
using MediatR;
using SubscriberServiceGrpc;

namespace Core.Api.GRPC;

public class SubscriberGrpcService(ISender sender) : Subscribers.SubscribersBase
{
    private readonly ISender _sender = sender;
    public override async Task<CreateSubscriberResponse> Create(CreateSubscriberRequest request, ServerCallContext context)
    {
        var result = await _sender.Send(new AddSubscriberCommand(request.Name, request.Url));
        if (result.IsFailure)
            result.Error.MapToGrpcException();

        return new CreateSubscriberResponse
        {
            Id = result.Data,
        };
    }
}

