using SkyLogger.Dal;
using SkyLogger.Models;
using System.Threading.Tasks;

namespace SkyLogger.Service
{
    public class LogService  :ILogService
    {
        readonly ISystemLogDal _systemLog;
        public LogService(SkyLoggerContext lctx)
        {
            _systemLog = new SystemLogDal(lctx);
        }
        
        public async Task<decimal> SysLogKaydetAsync(ESystemLog val)
        {
            return await _systemLog.KaydetAsync(val);
        }
    }
}
