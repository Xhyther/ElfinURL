namespace ElfinURL.Models;

public class Shorter
{
    public int Id { get; set; }
    public string Name {get;set;}
    public string LongUrl {get;set;}
    public string ShortUrl {get;set;}
    public DateTime CreatedAt {get;set;}
    public bool IsActive {get;set;}
    public int ClickCount {get;set;}
    
}