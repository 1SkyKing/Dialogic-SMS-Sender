using System.Threading.Tasks;
using SkyLogger.Models;

namespace SkyLogger.Dal
{
    public interface ISystemLogDal
    {
        Task<decimal> KaydetAsync(ESystemLog log);
    }
}