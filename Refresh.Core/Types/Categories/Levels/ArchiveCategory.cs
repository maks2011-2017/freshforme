using Bunkum.Core;
using Refresh.Core.Types.Data;
using Refresh.Database.Models.Contests;
using Refresh.Database.Models.Users;
using Refresh.Database.Query;

namespace Refresh.Core.Types.Categories.Levels;

public class ArchCategory : GameCategory
{
    public ArchCategory() : base("archive", [], false)
    {
        this.Name = "Архив";
        this.Description = "Архивные уровни! (тест)";
        this.FontAwesomeIcon = "certificate";
        this.IconHash = "g281132";
        this.PrimaryResultType = ResultType.Level;
    }
    
    public override DatabaseResultList? Fetch(RequestContext context, int skip, int count, DataContext dataContext,
        LevelFilterSettings levelFilterSettings, GameUser? _)
    {
        // Передаем массив ключевых слов, которые нужно найти
        string[] searchTags = { "Архив", "archive", "reupload", "перезалив" };
        
        var levels = dataContext.Database.SearchForLevelsv2(count, skip, dataContext.User, levelFilterSettings, searchTags);
        
        return new DatabaseResultList(levels); 
    }
}