using SkyLogger.Models;
using System.Threading.Tasks;

namespace SkyLogger.Dal
{
    public class SystemLogDal : ISystemLogDal
    {
        private readonly SkyLoggerContext _lctx;

        public SystemLogDal(SkyLoggerContext lctx)
        {
            _lctx = lctx;
        }

        public async Task<decimal> KaydetAsync(ESystemLog log)
        {

            _lctx.SystemLog.Add(log);
            await _lctx.SaveChangesAsync();
            return log.ID;

        }
    }
}
