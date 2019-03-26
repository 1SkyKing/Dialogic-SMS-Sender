using System.Threading.Tasks;

namespace Sky.Dialogic.Sender.Channel
{
    public interface IDistributor
    {
        Task<int> PostXml(string url, string content, long mesID, long sysHash);
    }
}