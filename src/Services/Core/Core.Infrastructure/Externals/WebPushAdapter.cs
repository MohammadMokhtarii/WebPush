using Core.Application.Common;
using Core.Domain.Segment;
using Microsoft.Extensions.Logging;
using Services.Common;
using WebPush;

namespace Core.Infrastructure.Externals;

internal class WebPushAdapter(ILogger<WebPushAdapter> logger) : IWebPushAdapter
{
    //TODO:This Will be Removed in Final Product
    public const string PublicKey = "BGbfXTgngEgiWH6KncqDZTUjiZWCR77IH9VWnnfEq6Iw5x9ranmhFfiRr32am-KYEw6pyl1uCU8N_zE1UETSpOQ";
    public const string PrivateKey = "LnEYrOzDhPLk0Jnr5bRGmEFgIVv6lHdNEIuMfnXylxk";


    private readonly ILogger<WebPushAdapter> _logger = logger;
    public async Task<Result> SendAsync(PushManager item, string body, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Sending Push Notification {@item}({body}) ", item, body);
        PushSubscription subscription = new(item.Endpoint, item.P256DH, item.Auth);
        VapidDetails vapidDetails = new("mailto:WebPush@Agah.com", PublicKey, PrivateKey);

        var webPushClient = new WebPushClient();
        try
        {
            await webPushClient.SendNotificationAsync(subscription, body, vapidDetails, cancellationToken);
            _logger.LogInformation("Push Notification {@Subscription} Sent ", item);
            return Result.Success();
        }
        catch (Exception exception)
        {
            _logger.LogError("Sending Push Notification {@Item} ({Body}) Falied - {ExceptionMessage} ({Exception})", item, body, exception.Message, exception);
            return Error.Exception("InternalError", "");
        }
    }
}
