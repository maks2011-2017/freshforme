using System.Net;
using AttribDoc.Attributes;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Http;
using MongoDB.Bson;
using Refresh.Core.Authentication.Permission;
using Refresh.Core.Types.Data;
using Refresh.Database;
using Refresh.Database.Models.Users;
using Refresh.Interfaces.APIv3.Endpoints.ApiTypes;
using Refresh.Interfaces.APIv3.Endpoints.ApiTypes.Errors;
using Refresh.Interfaces.APIv3.Endpoints.DataTypes.Request;
using Refresh.Interfaces.APIv3.Endpoints.DataTypes.Response;
using Refresh.Interfaces.APIv3.Endpoints.DataTypes.Response.Badge;

namespace Refresh.Interfaces.APIv3.Endpoints.Admin;

public class AdminBadgeApiEndpoints : EndpointGroup
{
    [ApiV3Endpoint("admin/badge/append", HttpMethods.Post), MinimumRole(GameUserRole.Admin)]
    [DocError(typeof(ApiValidationError), ApiValidationError.ObjectIdParseErrorWhen)]
    [DocError(typeof(ApiNotFoundError), "юзера или бейджа нет")]
    [DocSummary("дает бейдж комуто")]
    public ApiResponse<ApiBadgeResponse> GrantBadge(RequestContext context,
        GameDatabaseContext database, ApiAdminAppendBadgeRequest body, DataContext dataContext)
    {
        bool parsed = ObjectId.TryParse(body.BadgeId, out ObjectId badgeId);
        if (!parsed) return ApiValidationError.ObjectIdParseError;

        GameUser? user = database.GetUserByUuid(body.UserId);
        UserBadge? globalBadge = database.GetBadgeById(badgeId);

        if (user == null || globalBadge == null) 
            return ApiNotFoundError.Instance;

        if (user.Badges != null && user.Badges.Any(b => b.BadgeId == badgeId))
            return ApiValidationError.ObjectIdParseError;

        database.AppendBadgeToUser(user, globalBadge);

        return new ApiBadgeResponse
        {
            BadgeId = globalBadge.BadgeId.ToString(),
            Name = globalBadge.Name,
            IconUrl = globalBadge.IconUrl,
            Description = globalBadge.Description
        };

    }

    [ApiV3Endpoint("admin/badge/revoke", HttpMethods.Post), MinimumRole(GameUserRole.Admin)]
    [DocError(typeof(ApiValidationError), ApiValidationError.ObjectIdParseErrorWhen)]
    [DocError(typeof(ApiNotFoundError), "нет игрока")]
    [DocSummary("отзывает от игрока эту каке го бейдж!")]
    public ApiOkResponse RevokeBadgeFromUser(RequestContext context,
        GameDatabaseContext database, ApiAdminAppendBadgeRequest body)
    {
        bool parsed = ObjectId.TryParse(body.BadgeId, out ObjectId badgeId);
        if (!parsed) return ApiValidationError.ObjectIdParseError;

        GameUser? user = database.GetUserByUuid(body.UserId);
        UserBadge? globalBadge = database.GetBadgeById(badgeId);
        
        if (user == null || globalBadge == null) return ApiNotFoundError.Instance;

        database.RemoveBadgeFromUser(user, globalBadge);
        return new ApiOkResponse();
    }

    [ApiV3Endpoint("admin/badge/global/create", HttpMethods.Post), MinimumRole(GameUserRole.Admin)]
    [DocSummary("новый бейдж")]
    public ApiResponse<ApiBadgeResponse> CreateGlobalBadge(RequestContext context,
        GameDatabaseContext database, ApiAdminCreateBadgeRequest body)
    {
        UserBadge badge = database.AddBadge(body.Name, body.Description ?? string.Empty, body.IconUrl);

        ApiBadgeResponse response = new()
        {
            BadgeId = badge.BadgeId.ToString(), 
            Name = badge.Name,
            IconUrl = badge.IconUrl,
            Description = badge.Description
        };

        return response;
    }

    [ApiV3Endpoint("admin/badge/global/{idStr}", HttpMethods.Delete), MinimumRole(GameUserRole.Admin)]
    [DocError(typeof(ApiValidationError), ApiValidationError.ObjectIdParseErrorWhen)]
    [DocError(typeof(ApiNotFoundError), "нету такого")]
    [DocSummary("удаление бейджика с сервака")]
    public ApiOkResponse DeleteGlobalBadge(RequestContext context, GameDatabaseContext database, string idStr)
    {
        bool parsed = ObjectId.TryParse(idStr, out ObjectId id);
        if (!parsed) return ApiValidationError.ObjectIdParseError;

        UserBadge? badge = database.GetBadgeById(id);
        if (badge == null) return ApiNotFoundError.Instance;

        database.DeleteBadge(badge);
        return new ApiOkResponse();
    }
}


// {
//     [ApiV3Endpoint("admin/badge/new", HttpMethods.Post), MinimumRole(GameUserRole.Admin)]
//     [DocSummary("Creates a badge that shows up in the Instance API endpoint")]
//     public ApiResponse<ApiGameAnnouncementResponse> CreateBadge(RequestContext context,
//         GameDatabaseContext database, ApiGameBadgeRequest body, DataContext dataContext)
//     {
//         UserBadge badge = database.AddBadge(body.Title, body.Text, body.IconUrl);
//         return ApiGameBadgeResponse.FromOld(badge, dataContext);
//     }

//     [ApiV3Endpoint("admin/badge/{idStr}", HttpMethods.Delete), MinimumRole(GameUserRole.Admin)]
//     [DocError(typeof(ApiValidationError), ApiValidationError.ObjectIdParseErrorWhen)]
//     [DocError(typeof(ApiNotFoundError), "The badge could not be found")]
//     [DocSummary("Removes a badge")]
//     public ApiOkResponse RemoveBadge(RequestContext context, GameDatabaseContext database, string idStr)
//     {
//         bool parsed = ObjectId.TryParse(idStr, out ObjectId id);
//         if (!parsed) return ApiValidationError.ObjectIdParseError;

//         GameAnnouncement? announcement = database.GetAnnouncementById(id);
//         if (announcement == null) return ApiNotFoundError.Instance;

//         database.DeleteAnnouncement(announcement);
//         return new ApiOkResponse();
//     }

//     [ApiV3Endpoint("admin/notifications", HttpMethods.Post), MinimumRole(GameUserRole.Admin)]
//     [DocError(typeof(ApiNotFoundError), "The user could not be found")]
//     [DocSummary("Sends a player a personalized notification using his UUID")]
//     public ApiOkResponse CreateNotification(RequestContext context,
//         GameDatabaseContext database, ApiCreateNotificationRequest body)
//     {
//         GameUser? user = database.GetUserByUuid(body.UserId);
        
//         if (user == null) 
//         {
//             return ApiNotFoundError.Instance; 
//         }

//         database.AddNotification(body.Title, body.Text, user, body.Icon);

//         return new ApiOkResponse();
//     }
// }