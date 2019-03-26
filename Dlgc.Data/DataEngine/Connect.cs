using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Dlgc.Data.DataEngine
{
    public class Connect
    {
        private IDbCommand dbCommand;
        public Connect(DbContext context)
        {
            dbCommand = context.Database.GetDbConnection().CreateCommand();
        }

        public Connect(DbContext context,string strCommand, CommandType cmdType)
        {
            dbCommand = context.Database.GetDbConnection().CreateCommand();
        }
    }
}
