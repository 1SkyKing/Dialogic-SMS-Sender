using Sky.Dialogic.XmlModel;
using System.Threading.Tasks;
using Sky.Dialogic.Util;
using Dlgc.Data.UDH;
using Dlgc.Data.Dal;
using Sky.Dialogic.Sender.Channel;

namespace Sky.Dialogic.Sender
{
    public class DialogicSender : IDialogicSender
    {
        private ISendXML _sendXML = new SendXML();
        private ISmsUtil _smsUtil = new SmsUtil();
        readonly IMessagesDal _messagesDal;
        private readonly IDistributor distributor;
        //private SkyLoggerContext lctx;// = new SkyLoggerContext();
        //private ISysLogger _sysLoger;




        public DialogicSender(IMessagesDal messagesDal, int channelId, out int OchannelId)
        {
            _messagesDal = messagesDal;
            //_sysLoger = sysLoger;
            // _cHannelID = channelId;
            distributor = new Distributor(messagesDal, channelId, out OchannelId);
            //_sysLoger = new SysLogger(lctx);
    }

        

        public async Task<int> SendOneMessage(EncodeUDH message,long sysHash)
        {
            var dbMessage = await _messagesDal.GetMessageByIdAsync(message.ID,sysHash);
            if (dbMessage.Modifier == 2)
            {
                var res = 0;
                var smsUrl = _smsUtil.GetSMSURL(message.ChannelId);
                var xmlContent = _sendXML.GetUdhTypeXml(message, 0);
                if (!string.IsNullOrEmpty(xmlContent))
                {
                    res = await distributor.PostXml(smsUrl, xmlContent, message.ID, sysHash);
                }
                else
                {
                    
                    res = -9;
                }

                return res;
            }
            else
            {
                //await _sysLoger.LogAsync(LogTipi.CoreGWErr, message.ID + " SendOneMessage Err-Modifier:" + dbMessage.Modifier);
                return -1;
            }
            
        }

        public async Task<int> SendManyMessage(EncodeUDH message, long sysHash)
        {
            var dbMessage = await _messagesDal.GetMessageByIdAsync(message.ID,sysHash);
            if (dbMessage.Modifier == 2)
            {

                var res = 0;
                var smsUrl = _smsUtil.GetSMSURL(message.ChannelId);
                for (int i = 0; i < message.Contents.Count; i++)
                {
                    if (i == message.Contents.Count - 1)
                    {
                        message.mms = false;
                    }
                    var xmlContent = _sendXML.GetUdhTypeXml(message, i);
                    if (!string.IsNullOrEmpty(xmlContent))
                    {
                        res = await distributor.PostXml(smsUrl, xmlContent, message.ID, sysHash);
                        //return res;
                    }
                    else
                    {
                        res= -9;
                        //await _sysLoger.LogAsync(LogTipi.CoreGWErr, message.ID + " SendManyMessage Err-XML:" + dbMessage.Modifier);
                    }
                    
                }
                return res;

                
            }
            else
            {
                //await _sysLoger.LogAsync(LogTipi.CoreGWErr, message.ID + " SendManyMessage Err-Modifier:" + dbMessage.Modifier);
                return -1;
            }
        }

      
    }
}
