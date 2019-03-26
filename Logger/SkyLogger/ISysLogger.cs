using System.Threading.Tasks;
using SkyLogger.SkyEnum;

namespace SkyLogger
{
    public interface ISysLogger
    {
        Task<bool> LogAsync(LogTipi tip, string logData);
    }
}