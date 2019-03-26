using System.Collections.Generic;
using System.Threading.Tasks;
using Dlgc.Data.ErrXmlMap;
using Dlgc.Data.Models;

namespace Dlgc.Data.Dal
{
    public interface IMessagesDal
    {
        Task<Messages> GetMessageByIdAsync(long id, long sysHash);
        Task<bool> LockMessageAsync(Messages mes);
        Task<bool> UpdateSuccessMessageAsync(long mesId, long sysHash);
        Task<bool> UpdateMessageModifierAsync(long ID, int modifier, long sysHash);
        Task<bool> UpdateErrorMessageAsync(ErrorMessage erMessage, long sysHash);
        Task<IEnumerable<MesId>> GetHemenSmsAsync(long sysHash, int bodyFormat);
    }
}