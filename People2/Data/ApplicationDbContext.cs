using Microsoft.EntityFrameworkCore;
using People2.Models;

namespace People2.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<PersonMod> Persons { get; set; }


    }
}
