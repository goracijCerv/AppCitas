using Apcitas.WebService.Entities;
using Microsoft.EntityFrameworkCore;

namespace Apcitas.WebService.Data;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<AppUser> Users { get; set; }
}
