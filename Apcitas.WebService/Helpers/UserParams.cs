﻿namespace Apcitas.WebService.Helpers;

public class UserParams : PaginationParams
{
    public string CurrentUsername { get; set; }
    public string Gender { get; set; }
    public int MinAge { get; set; } = 10;
    public int MaxAge { get; set; } = 150;
    public string OrderBy { get; set; } = "LastActive";
}
