using Microsoft.EntityFrameworkCore;
using Profile_app_backend.Models;

namespace Profile_app_backend.Data;
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<UserProfile> UserProfiles { get; set; }
}