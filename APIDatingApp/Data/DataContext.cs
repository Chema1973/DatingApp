using APIDatingApp.Entities;
using Microsoft.EntityFrameworkCore;

namespace APIDatingApp.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users {get; set;}
    }
}