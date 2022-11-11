﻿namespace Apcitas.WebService.DTOs;

public class MessageDto
{
    public int Id { get; set; }
    public int SenderId { get; set; }
    public string SenderUsername { get; set; }
    public string SenderPhotoUrl { get; set; }
    public string RecipentId { get; set; }
    public string RecipentUsername { get; set; }
    public string RecipentPothoUrl { get; set; }
    public string Content { get; set; }
    public DateTime? DateRead { get; set; }
    public DateTime MessageSent { get; set; } = DateTime.Now;
    
}
