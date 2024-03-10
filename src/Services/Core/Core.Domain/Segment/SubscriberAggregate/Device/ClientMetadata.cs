namespace Core.Domain.Segment;

public readonly record struct ClientMetadata
{
    private ClientMetadata(string os)
    {
        OS = os;
    }
    public string OS { get; init; }


    public static ClientMetadata Create(string os) => new(os);
}