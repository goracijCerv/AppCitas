namespace Apcitas.WebService.Helpers;

public class MessageParams : PaginationParams
{
    public string UserName { get; set; }
    public string Conteiner { get; set; } = "Unread";
}

