using Microsoft.EntityFrameworkCore;

namespace SwaggerApi.Models
{
    public class Context : DbContext
    {
        public Context(DbContextOptions<Context> options):base(options)
        {

        }

        public DbSet<Student> studentItem { get; set; }
    }
}
