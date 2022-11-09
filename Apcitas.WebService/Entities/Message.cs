namespace Apcitas.WebService.Entities;

public class Message
{
    #region EF Config
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; }
    public AppUser Sender { get; set; }
    public string RecipentUsername { get; set; }
    public AppUser Recipent { get; set; }
    #endregion

    #region Message
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.Now;
    public bool SenderDelate { get; set; }
    public bool RecipientDelate { get; set; }
    #endregion
}
