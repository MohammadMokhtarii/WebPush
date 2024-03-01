using Services.Common;

namespace Core.Domain.Segment;

public readonly record struct ClientMetadata
{
    public ClientMetadata() { }
    private ClientMetadata(string os)
    {
        OS = os;
    }
    public string OS { get; init; }


    public static ClientMetadata Create(string os)
    {
        return new ClientMetadata(os);
    }
}