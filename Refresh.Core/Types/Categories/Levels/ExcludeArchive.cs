using Bunkum.Core;
using Refresh.Core.Types.Data;
using Refresh.Database.Models.Users;
using Refresh.Database.Query;

namespace Refresh.Core.Types.Categories.Levels;

public class ExcludeArchiveCategory : GameCategory
{
    public ExcludeArchiveCategory() : base("excludeescapes", [], false)
    {
        this.Name = "БЕЗ АРХИВОВ";
        this.Description = "БЕЗ АРХИВОВ ЧАТ БЕЕЕЗ АРХИВОВ";
        this.FontAwesomeIcon = "certificate";
        // g82777
        this.IconHash = "g630194";
        this.PrimaryResultType = ResultType.Level;
    }
    
    public override DatabaseResultList? Fetch(RequestContext context, int skip, int count, DataContext dataContext,
        LevelFilterSettings levelFilterSettings, GameUser? _)
    {
        // Передаем массив ключевых слов, которые нужно найти
        string[] searchTags = { "АРХИВ" };
        
        var levels = dataContext.Database.SearchExcludeLevelsv1(count, skip, dataContext.User, levelFilterSettings, searchTags);
        
        return new DatabaseResultList(levels); 
    }
}