// using Refresh.Core.Types.Data;
// using Refresh.Database.Models.Notifications;

// namespace Refresh.Interfaces.APIv3.Endpoints.DataTypes.Response;

// [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
// public class ApiGameBadgeResponse
// {
//     public required string BadgeId { get; set; }
//     public required string Title { get; set; }
//     public required string Description { get; set; }
//     public required DateTimeOffset CreatedAt { get; set; }
    
//     public static ApiGameBadgeResponse? FromOld(UserBadge? old, DataContext dataContext)
//     {
//         if (old == null) return null;

//         return new ApiGameBadgeResponse
//         {
//             BadgeId = old.BadgeId.ToString()!,
//             Name = old.Name,
//             Description = old.Description,
//             CreatedAt = old.CreatedAt,
//         };
//     }

//     public static IEnumerable<ApiGameAnnouncementResponse> FromOldList(IEnumerable<GameAnnouncement> oldList,
//         DataContext dataContext) => oldList.Select(old => FromOld(old, dataContext)).ToList()!;
// }