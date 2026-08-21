using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Xml.Serialization;
using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Core.RateLimit;
using Bunkum.Core.Responses.Serialization;
using Bunkum.Listener.Protocol;
using Refresh.Common.Time;
using Refresh.Core.Authentication.Permission;
using Refresh.Core.Configuration;
using Refresh.Core.RateLimits.Users;
using Refresh.Core.Services;
using Refresh.Core.Types.Matching;
using Refresh.Database;
using Refresh.Database.Models.Authentication;
using Refresh.Database.Models.Contests;
using Refresh.Database.Models.Notifications;
using Refresh.Database.Models.Users;
using Refresh.Interfaces.Game.Types.Notifications;

namespace Refresh.Interfaces.Game.Endpoints;

public class AnnouncementEndpoints : EndpointGroup
{
    private static bool AnnounceGetNotifications(StringBuilder output, GameDatabaseContext database, GameUser user, GameServerConfig config)
    {
        List<GameNotification> notifications = database.GetNotificationsByUser(user, 5, 0).Items.ToList();
        int count = database.GetNotificationCountByUser(user);
        if (count == 0) return false;

        string s = count != 1 ? "s" : string.Empty;

        output.Append($"Че как, {user.Username}. У тя есть {count} уведомлений!{s}:\n\n");
        for (int i = 0; i < notifications.Count; i++)
        {
            GameNotification notification = notifications[i];
            output.Append($"  {notification.Title} ({i + 1}/{count}):\n");
            output.Append($"    {notification.Text}\n\n");
        }

        output.Append($"Если надо чекнуть все или очистить то пройди туда братишка  {config.WebExternalUrl}!\n");
        return true;
    }

    private static bool AnnounceGetAnnouncements(StringBuilder output, GameDatabaseContext database)
    {
        IEnumerable<GameAnnouncement> announcements = database.GetAnnouncements().ToList();
        foreach (GameAnnouncement announcement in announcements)
            output.Append($"""

                            ----> {announcement.Title} <----

                            {announcement.Text}

            """);
        
        return announcements.Any();
    }
    
    private static bool AnnounceGetContest(StringBuilder output, Token token, GameDatabaseContext database, GameServerConfig config)
    {
        GameContest? contest = database.GetNewestActiveContest();
        if (contest == null) return false;
        
        // only show contests for the current game
        if (!contest.AllowedGames.Contains(token.TokenGame)) return false;
        
        output.Append("There's a contest live right now!\n\n");
        output.AppendLine($"** {contest.ContestTitle} **");
        
        output.Append("Summary: ");
        output.AppendLine(contest.ContestSummary);
        if (!string.IsNullOrWhiteSpace(contest.ContestTheme))
        {
            output.Append("Theme: ");
            output.AppendLine(contest.ContestTheme);
        }
        
        output.AppendLine($"See more on the website: {config.WebExternalUrl}/contests/{contest.ContestId}");
        
        return true;
    }

    [GameEndpoint("announce")]
    [MinimumRole(GameUserRole.Restricted)]
    [SuppressMessage("ReSharper", "RedundantAssignment")]
    [RateLimitSettings(420, 12, 380, "announcements-game")]
    public string Announce(RequestContext context, GameServerConfig config, GameUser user, GameDatabaseContext database, Token token, IDateTimeProvider timeProvider)
    {
        if (user.Role == GameUserRole.Restricted)
        {
            return $"""
                   Твой аккаунт типа это ну как его там крч не можешь ты ниче делать
                   
                   Причина: {user.BanReason ?? "Нет причины."}
                   Remaining: ~{(user.BanExpiryDate! - timeProvider.Now).Value.Days} days
                   
                   Ты все еще можешь играть но не сможешь никак интерактировать с сообществом. напиши админам
                   """;
        }

        if (!user.EmailAddressVerified)
        {
            return $"ПОДТВЕРДИ ЕМЕЙЛ!!!\n\n" +
                   
                   $"Если чо, играть еще можешь.. нооо не сможешь ничего выкладывать" +                   
                   
                   $"Проверь спам ящик если что... \n\n" +
                   
                   $"{config.WebExternalUrl}.";
        }
        
        // ReSharper disable once JoinDeclarationAndInitializer (makes it easier to follow)
        bool appended;
        StringBuilder output = new();
        
        appended = AnnounceGetAnnouncements(output, database);
        
        if (appended) output.Append('\n');
        appended = AnnounceGetContest(output, token, database, config);
        
        // All games except PSP support real-time notifications.
        // If we're playing on PSP, check for notifications.
        if (token.TokenGame == TokenGame.LittleBigPlanetPSP)
        {
            if (appended) output.Append('\n');
            appended = AnnounceGetNotifications(output, database, user, config);
        }
        
        return output.ToString();
    }

    [GameEndpoint("notification", ContentType.Xml)]
    [MinimumRole(GameUserRole.Restricted)]
    [RateLimitSettings(NotificationsEndpointLimits.TimeoutDuration, NotificationsEndpointLimits.GameRequestAmount, 
                            NotificationsEndpointLimits.BlockDuration, NotificationsEndpointLimits.GameRequestBucket)]
    public string Notification(RequestContext context, GameServerConfig config, Token token, GameDatabaseContext database, MatchService matchService)
    {
        // On LBP1 the only regular ticking request is /notification,
        // so we update the "last contact" of the user's room when we receive a notification request to prevent LBP1 rooms from being auto-closed early
        GameRoom? room = matchService.RoomAccessor.GetRoomByUser(token.User, token.TokenPlatform, token.TokenGame);
        
        if (room != null)
        {
            room.LastContact = DateTimeOffset.Now;
            
            matchService.RoomAccessor.UpdateRoom(room);
        }
        
        DatabaseList<GameNotification> notifications = database.GetNotificationsByUser(token.User, 3, 0);
        
        using MemoryStream ms = new();
        using BunkumXmlTextWriter bunkumXmlTextWriter = new(ms);

        XmlSerializer serializer = new(typeof(SerializedNotification));
        
        XmlSerializerNamespaces namespaces = new();
        namespaces.Add("", "");
        
        foreach (GameNotification notification in notifications.Items.ToList())
        {
            SerializedNotification serializedNotification = new()
            {
                Text = $"{notification.Title}: {notification.Text}",
            };
                
            serializer.Serialize(bunkumXmlTextWriter, serializedNotification, namespaces);
            database.DeleteNotification(notification);
        }

        ms.Seek(0, SeekOrigin.Begin);
        using StreamReader reader = new(ms);
        
        return reader.ReadToEnd();
    }
}