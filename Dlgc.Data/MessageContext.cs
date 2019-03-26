using Dlgc.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Dlgc.Data
{
    //Scaffold-DbContext "server=192.168.10.201;database=DialogicDSISWS;uid=CRMSky;pwd=357951Sky.;connection timeout=1000;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
    public class MessageContext:DbContext
    {
        private string ConnectionString { get; set; }

        public MessageContext()
        {

        }

        public MessageContext(DbContextOptions<MessageContext> options)
            :base(options)
        {

        }

        public MessageContext(string constr)
        {
            ConnectionString = constr;
        }

        public  virtual DbSet<Messages> Messages { get; set; }


//        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
//        {
//            if (!optionsBuilder.IsConfigured)
//            {
////#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
//                optionsBuilder.UseSqlServer("");
//            }
//        }

    }
}
