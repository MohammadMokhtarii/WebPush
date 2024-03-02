namespace Core.Domain.Segment;

public readonly record struct PushManager
{
    public PushManager() { }
    private PushManager(string endpoint, string p256DH, string auth)
    {
        Endpoint = endpoint;
        P256DH = p256DH;
        Auth = auth;
    }
    public string Endpoint { get; init; }
    public string P256DH { get; init; }
    public string Auth { get; init; }

    public static PushManager Create(string endpoint, string p256DH, string auth)
    {
        return new PushManager(endpoint, p256DH, auth);
    }
}
