﻿using Core.Domain.Segment;

namespace Core.Application.Segment;
public record AddDeviceCommand(string Name, PushManager PushManager, ClientMetadata ClientMetadata, int SubscriberId) : IRequest<Result<int>>
{
    public sealed record AddDeviceDto(string Name, PushManager PushManager, int SubscriberId);

}

