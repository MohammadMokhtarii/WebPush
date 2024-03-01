using Services.Common;

namespace Core.Domain.Segment;

public readonly record struct SubscriberId(int Value);
public sealed class Subscriber : Entity, IAggregateRoot
{
    private readonly List<Device> _devices = [];
    private Subscriber(Guid token, string name, WebsiteUrl url)
    {
        Token = token;
        Name = name;
        Url = url;
    }
    private Subscriber() { }
    public SubscriberId Id { get; private set; }
    public Guid Token { get; private set; }
    public string Name { get; private set; }
    public WebsiteUrl Url { get; private set; }
    public bool InActive { get; private set; }
    public IReadOnlyCollection<Device> Devices => _devices;

    public static Result<Subscriber> Create(string name, string url)
    {
        var websiteUrlResult = WebsiteUrl.Create(url);
        if (websiteUrlResult.IsFailure)
            return websiteUrlResult.Error;

        Subscriber model = new(Guid.NewGuid(), name, websiteUrlResult.Data);

        return model;
    }

    public Result<Device> AddDevice(string name, PushManager pushManager, ClientMetadata clientMetadata)
    {
        if (InActive)
            return SegmentDomainErrors.Subscriber.SubscriberIsInActive;

        Device device = Device.Create(name, pushManager, clientMetadata, Id);

        _devices.Add(device);

        return device;
    }
    public Result DeActivate()
    {
        if (InActive)
            return SegmentDomainErrors.Subscriber.SubscriberIsAlreadyInActive;

        InActive = true;

        return Result.Success();
    }
}
