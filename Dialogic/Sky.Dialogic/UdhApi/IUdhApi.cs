using Dlgc.Data.UDH;
using System.Threading.Tasks;

namespace Sky.Dialogic.UdhApi
{
    public interface IUdhService
    {
        Task<EncodeUDH> GetUdhAsync(string content, string encoding, bool base64);
    }
}