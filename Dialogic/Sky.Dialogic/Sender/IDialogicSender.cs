using System.Threading.Tasks;
using Dlgc.Data.UDH;

namespace Sky.Dialogic.Sender
{
    public interface IDialogicSender
    {
        Task<int> SendManyMessage(EncodeUDH message, long sysHash);
        Task<int> SendOneMessage(EncodeUDH message, long sysHash);
    }
}