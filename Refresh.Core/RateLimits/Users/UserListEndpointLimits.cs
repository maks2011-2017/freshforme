namespace Refresh.Core.RateLimits.Users;

public static class UserListEndpointLimits
{
    public const int TimeoutDuration = 240;
    public const int RequestAmount = 300;
    public const int BlockDuration = 180;
    public const string RequestBucket = "user-list";
}