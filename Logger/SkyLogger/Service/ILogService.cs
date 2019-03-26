using SkyLogger.Models;
using System.Threading.Tasks;

namespace SkyLogger.Service
{
    public interface ILogService
    {
        #region SYS LOG
        Task<decimal> SysLogKaydetAsync(ESystemLog val);

        #endregion

        //#region VISITOR LOG

        //long VisitorLogKaydet(EVisitorLog val);

        //#endregion
    }
}
