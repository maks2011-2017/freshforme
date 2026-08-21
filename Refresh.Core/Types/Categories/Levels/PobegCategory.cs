using Bunkum.Core;
using Refresh.Core.Types.Data;
using Refresh.Database.Models.Contests;
using Refresh.Database.Models.Users;
using Refresh.Database.Query;

namespace Refresh.Core.Types.Categories.Levels;

public class PobegCategory : GameCategory
{
    public PobegCategory() : base("escapes", [], false)
    {
        this.Name = "Побеги";
        this.Description = "ПОБЕГ ОТ...";
        this.FontAwesomeIcon = "certificate";
        // g82777
        this.IconHash = "g718426";
        this.PrimaryResultType = ResultType.Level;
    }
    
    public override DatabaseResultList? Fetch(RequestContext context, int skip, int count, DataContext dataContext,
        LevelFilterSettings levelFilterSettings, GameUser? _)
    {
        // Передаем массив ключевых слов, которые нужно найти
        string[] searchTags = { "escape", "побег", "сбеги" };
        
        var levels = dataContext.Database.SearchForLevelsv2(count, skip, dataContext.User, levelFilterSettings, searchTags);
        
        return new DatabaseResultList(levels); 
    }
}