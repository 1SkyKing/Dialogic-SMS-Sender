using Microsoft.EntityFrameworkCore;
using SkyLogger.Models;

namespace SkyLogger
{
    public class SkyLoggerContext : DbContext
    {
        private string ConnectionString { get; set; }

        public SkyLoggerContext()
        {

        }

        public SkyLoggerContext(DbContextOptions<SkyLoggerContext> options)
            : base(options)
        {

        }

        public SkyLoggerContext(string constr)
        {
            ConnectionString = constr;
        }

        public virtual DbSet<ESystemLog> SystemLog { get; set; }
    }
}
