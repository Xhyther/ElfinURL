using ElfinURL.Models;
using Microsoft.EntityFrameworkCore;

namespace ElfinURL.DB;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DbContext> options) : base(options) { }
    
    public DbSet<Shorter> Shorters => Set<Shorter>();
}