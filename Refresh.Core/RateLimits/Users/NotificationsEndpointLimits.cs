namespace Refresh.Core.RateLimits.Users;

public static class NotificationsEndpointLimits
{
    public const int TimeoutDuration = 100;
    public const int GameRequestAmount = 800;
    public const int ApiRequestAmount = 20;
    public const int BlockDuration = 180;
    public const string GameRequestBucket = "notifications-game";
    public const string ApiRequestBucket = "notifications-api";
}