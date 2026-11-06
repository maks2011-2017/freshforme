using Refresh.Core.Types.Data;
using Refresh.Database.Models.Users;

namespace Refresh.Interfaces.APIv3.Endpoints.DataTypes.Response.Badge;

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ApiBadgeResponse : IApiResponse, IDataConvertableFrom<ApiBadgeResponse, UserBadge>
{
    public required string BadgeId { get; set; }
    public required string Name { get; set; }

    public required string Description { get; set; }
    public required string IconUrl { get; set; }

    public static ApiBadgeResponse? FromOld(UserBadge? old, DataContext dataContext)
    {
        if (old == null) return null;

        return new ApiBadgeResponse
        {
            BadgeId = old.BadgeId.ToString(),
            Name = old.Name,
            Description = old.Description,
            IconUrl = old.IconUrl,
        };
    }

    public static IEnumerable<ApiBadgeResponse> FromOldList(IEnumerable<UserBadge> oldList, DataContext dataContext) 
        => oldList.Select(old => FromOld(old, dataContext)).ToList()!;
}