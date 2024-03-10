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
    public string Endpoint { get; init; } = default!;
    public string P256DH { get; init; } = default!;
    public string Auth { get; init; } = default!;

    public static PushManager Create(string endpoint, string p256DH, string auth) => new(endpoint, p256DH, auth);
}
