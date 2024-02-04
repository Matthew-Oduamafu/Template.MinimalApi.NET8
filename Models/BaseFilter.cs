namespace Template.MinimalApi.NET8.Models;

public class BaseFilter
{
    private int PageIndex { get; set; } = 1;

    public int Page
    {
        get => PageIndex;
        set => PageIndex = value > 0 ? value : 1;
    }

    private int MinPageSize { get; set; } = 10;

    public int PageSize
    {
        get => MinPageSize;
        set => MinPageSize = value > 0 ? value : 10;
    }
    
    public static bool TryParse(string s, IFormatProvider provider, out BaseFilter filter)
    {
        filter = new BaseFilter();
        var parts = s.Split(',');

        if (parts.Length != 2)
            return false;

        if (!int.TryParse(parts[0], out var page) || !int.TryParse(parts[1], out var pageSize))
            return false;

        filter = new BaseFilter { Page = page, PageSize = pageSize };
        return true;
    }
}