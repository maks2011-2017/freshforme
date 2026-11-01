using Bunkum.Core;
using Refresh.Core.Types.Data;
using Refresh.Database.Models.Users;
using Refresh.Database.Query;

namespace Refresh.Core.Types.Categories.Levels;

public class OldestLevelsCategory : GameCategory
{
    internal OldestLevelsCategory() : base("oldest", "oldest", false)
    {
        this.Name = "Старейшие уровни!";
        this.Description = "Уровни, которые были загружены совсем давно!";
        this.IconHash = "g820623";
        this.FontAwesomeIcon = "calendar";
        this.PrimaryResultType = ResultType.Level;
    }
    
    public override DatabaseResultList? Fetch(RequestContext context, int skip, int count, DataContext dataContext,
        LevelFilterSettings levelFilterSettings, GameUser? _) 
        => new(dataContext.Database.GetOldestLevels(count, skip, dataContext.User, levelFilterSettings));
}