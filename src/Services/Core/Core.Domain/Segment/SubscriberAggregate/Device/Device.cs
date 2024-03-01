namespace Core.Domain.Segment;

public readonly record struct DeviceId(int Value);
public sealed class Device : Entity
{
    private Device(string name, PushManager pushManager, ClientMetadata clientMetadata, SubscriberId subscriberId)
    {
        Name = name;
        PushManager = pushManager;
        ClientMetadata = clientMetadata;
        CreatedOnUtc = DateTime.UtcNow;
        SubscriberId = subscriberId;
    }
    private Device() { }
    public DeviceId Id { get; private set; }
    public string Name { get; private set; }
    public PushManager PushManager { get; private set; }
    public ClientMetadata ClientMetadata { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    public SubscriberId SubscriberId { get; private set; }

    public static Device Create(string name, PushManager pushManager, ClientMetadata clientMetadata, SubscriberId subscriberId)
        => new(name, pushManager, clientMetadata, subscriberId);
}
