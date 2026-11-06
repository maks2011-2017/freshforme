using MongoDB.Bson;
using Bunkum.Core.RateLimit;
using Refresh.Database.Models.Statistics;
using Refresh.Database.Models.Users;

[JsonObject(MemberSerialization.OptIn)]
// [Index(nameof(Username), nameof(UsernameLower), nameof(EmailAddress), nameof(PasswordBcrypt))]
public partial class UserBadge
{
    [Key] public ObjectId BadgeId { get; set; } = ObjectId.GenerateNewId();
    public string Name { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public List<GameUser> Users { get; set; } = new();
    public DateTimeOffset CreatedAt { get; set; }

}
