using Microsoft.EntityFrameworkCore;
using Multi_Request.Models;

namespace Multi_Request.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
        public DbSet<User> Users => Set<User>();

    }
}
