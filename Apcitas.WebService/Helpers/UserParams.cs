namespace Apcitas.WebService.Helpers;

public class UserParams
{
    private const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    public int _pageSize = 10;

    public int PageSie
    {
        get => _pageSize;
        set=> _pageSize = (value > MaxPageSize) ? MaxPageSize :value;
    }

    public string CurrentUsername { get; set; }
    public string Gender { get; set; }
    public int MinAge { get; set; } = 10;
    public int MaxAge { get; set; } = 150;
}
