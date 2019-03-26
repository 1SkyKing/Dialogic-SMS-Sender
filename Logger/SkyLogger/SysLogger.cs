
using SkyLogger.Models;
using SkyLogger.Service;
using SkyLogger.SkyEnum;
using System;
using System.Threading.Tasks;

namespace SkyLogger
{
    
    public class SysLogger : ISysLogger
    {
        private  SkyLoggerContext _lctx;
        public SysLogger(SkyLoggerContext lctx)
        {
            _lctx = lctx;
        }

        public  async Task<bool> LogAsync(LogTipi tip, string logData)
        {
            ILogService _logService = new LogService(_lctx);
            var log = new ESystemLog
            {
                CreateDate = DateTime.Now,
                Log_Data = logData,
                Proc = (int)Enum.Parse(tip.GetType(), tip.ToString()),//,
                ProcDate = DateTime.Now
            };
            var s = await _logService.SysLogKaydetAsync(log);
            return s > 0;
        }
    }
}
