using ElfinURL.Models;
using Microsoft.EntityFrameworkCore;

namespace ElfinURL.DB;

public class ElfinDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public ElfinDbContext(DbContextOptions<ElfinDbContext> options) : base(options) { }
    
    public DbSet<ShorterURL> ShorterURLs { get; set; }
}