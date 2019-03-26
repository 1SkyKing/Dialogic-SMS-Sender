using System.Threading.Tasks;

namespace Sky.Dialogic.Sender.Channel
{
    public interface IChannel
    {
        Task<int> PostXml(string url, string content, long mesID, long sysHash);
    }
}