// using AttribDoc.Attributes;
// using Bunkum.Core;
// using Bunkum.Core.Endpoints;
// using Bunkum.Protocols.Http;
// using MongoDB.Bson;
// using Refresh.Core.Authentication.Permission;
// using Refresh.Core.Types.Data;
// using Refresh.Database;
// using Refresh.Database.Models.Notifications;
// using Refresh.Database.Models.Users;
// using Refresh.Interfaces.APIv3.Endpoints.ApiTypes;
// using Refresh.Interfaces.APIv3.Endpoints.ApiTypes.Errors;
// using Refresh.Interfaces.APIv3.Endpoints.DataTypes.Request;
// using Refresh.Interfaces.APIv3.Endpoints.DataTypes.Response;

// namespace Refresh.Interfaces.APIv3.Endpoints.Admin;
// public class AdminBadgeApiEndpoints : EndpointGroup
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