using Refresh.Database.Models.Authentication;
using Refresh.Database.Query;

namespace Refresh.Interfaces.APIv3.Endpoints.DataTypes.Request;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ApiAdminAppendBadgeRequest
{
    public string? UserId {get;set;}
    public string? BadgeId {get;set;}

}

public class ApiAdminRemoveBadgeRequest
{
    public string?  UserId { get; set; }
    public string?  BadgeId { get; set; }
}

public class ApiAdminCreateBadgeRequest
{
    public required string Name { get; set; }
    public required string IconUrl { get; set; }
    public string Description { get; set; } = string.Empty;
}
