namespace Core.Domain.Segment;

public sealed class Device : Entity
{
    private Device(string name, PushManager pushManager, ClientMetadata clientMetadata, int subscriberId)
    {
        Name = name;
        PushManager = pushManager;
        ClientMetadata = clientMetadata;
        CreatedOnUtc = DateTime.UtcNow;
        SubscriberId = subscriberId;
    }
    private Device() { }
    public int Id { get; private set; }
    public string Name { get; private set; }
    public PushManager PushManager { get; private set; }
    public ClientMetadata ClientMetadata { get; private set; }
    public DateTime CreatedOnUtc { get; private set; }

    public int SubscriberId { get; private set; }

    public static Device Create(string name, PushManager pushManager, ClientMetadata clientMetadata, int subscriberId)
        => new(name, pushManager, clientMetadata, subscriberId);
}
