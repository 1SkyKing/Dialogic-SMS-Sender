using Dlgc.Data.Dal;
using System;
using System.Threading.Tasks;

namespace Sky.Dialogic.Sender.Channel
{
    public class Distributor : IDistributor
    {
        private IChannel _Ichannel;
        
        private readonly string _path = @"c:\temp\CoreService-" + DateTime.Now.Year + "-" + DateTime.Now.Month.ToString().PadLeft(2, '0') + "-" + DateTime.Now.Day.ToString().PadLeft(2, '0') + ".txt";
        public Distributor(IMessagesDal messagesDal,int channelId, out int OchannelId)
        {
            
            switch (channelId)
            {
                case 1:
                    _Ichannel = new channel_1(messagesDal,_path);
                    channelId++;
                    break;
                case 2:
                    _Ichannel = new Channel_2(messagesDal, _path);
                    channelId++;
                    break;
                case 3:
                    _Ichannel = new channel_3(messagesDal, _path);
                    channelId++;
                    break;
                case 4:
                    _Ichannel = new channel_4(messagesDal, _path);
                    channelId++;
                    break;
                case 5:
                    _Ichannel = new channel_5(messagesDal, _path);
                    channelId = 1;
                    break;
                default:
                    _Ichannel = new channel_1(messagesDal, _path);
                    channelId = 2;                      
                    break;
            }
            OchannelId = channelId;
        }

        public Task<int> PostXml(string url, string content, long mesID, long sysHash)
        {
            return _Ichannel.PostXml(url, content, mesID, sysHash);
        }
    }
}
