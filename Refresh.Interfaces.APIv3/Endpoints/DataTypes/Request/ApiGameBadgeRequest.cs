namespace Refresh.Interfaces.APIv3.Endpoints.DataTypes.Request;

#nullable disable

[JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
public class ApiGameBadgeResponse
{
    public string Title { get; set; }
    public string Text { get; set; }
    public string Icon { get; set; }
}