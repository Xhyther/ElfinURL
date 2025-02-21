namespace ElfinURL.Models;

public class ShorterURL
{
    public int Id { get; set; }
    public required string OriginalUrl {get;set;}
    public string ShortCode {get;set;}
    public DateTime CreatedAt {get;set;}
    public bool IsActive {get;set;}
    public int ClickCount {get;set;}
    
}