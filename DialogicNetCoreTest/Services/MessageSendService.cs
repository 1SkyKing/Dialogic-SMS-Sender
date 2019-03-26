using DialogicNetCoreTest.Config;
using Dlgc.Data;
using Dlgc.Data.Dal;
using Dlgc.Data.Models;
using Dlgc.Data.UDH;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Sky.Dialogic.Sender;
using Sky.Dialogic.UdhApi;
using Sky.Dialogic.Util;
using SkyLogger;
using SkyLogger.SkyEnum;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace DialogicNetCoreTest.Services
{
    public class MessageSendService : IHostedService, IDisposable
    {
        #region TEST DATA
        //WHERE     (ID IN (1081379, 1059632, 753545, 378951, 332553, 327726, 323265, 159322, 158149, 158126, 158094, 156951, 156928, 156836))
        #endregion


        private readonly IOptions<AppConfig> _appConfig;
        private readonly MessageContext _ctx;
        private readonly SkyLoggerContext _lctx;
        readonly IMessagesDal _messagesDal;
        readonly ISysLogger _sysLoger;
        readonly IUdhService _udhApi;
        readonly ISmsUtil _smsUtil = new SmsUtil();
        private Timer _timer;
        private static readonly Random Getrandom = new Random();
        //readonly 
        private int _chanelId = 1;
        private int OchannelId;
        public MessageSendService(IOptions<AppConfig> appConfig, MessageContext ctx, SkyLoggerContext lctx)
        {
            _appConfig = appConfig;
            _ctx = ctx;
            _lctx = lctx;
            _sysLoger = new SysLogger(lctx);
            _messagesDal = new MessagesDal(_ctx, _sysLoger);
            _udhApi = new UdhService(_appConfig.Value.UDHServiceAddres);
           
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            //_sysLoger.LogAsync(LogTipi.CoreGWSend, "StartAsync:"+ DateTime.UtcNow.ToString("O"));
            var timersec = _appConfig.Value.TimerMiliSecond;
            if (timersec == 0)
            {
                timersec = 15000;
            }
            _timer = new Timer(
                async (e) => await SendSMSToDialogicAsync(cancellationToken),
                null,
                TimeSpan.Zero,
                TimeSpan.FromMilliseconds(timersec));

            return Task.CompletedTask;
        }
       
        public async Task SendSMSToDialogicAsync(CancellationToken cancellationToken)
        {
            //await _sysLoger.LogAsync(LogTipi.CoreGWErr, "StartAsync");
            try
            {
                var sysHash = GetRandomNumber(1, 999999999);
                //Sender Core dan bodyFormat 7 gelir
                var mesList = await _messagesDal.GetHemenSmsAsync(sysHash, 7);


                foreach (MesId id in mesList)
                {
                    //await _sysLoger.LogAsync(LogTipi.CoreGWSend, "SendSMSToDialogicAsync foreach ID:"+ id.ID);
                    var message = await _messagesDal.GetMessageByIdAsync(id.ID, sysHash); //await 
                    //await _sysLoger.LogAsync(LogTipi.CoreGWSend, "SendSMSToDialogicAsync foreach GetMessageByIdAsync ID:" + message.ID);                                                          //Kilitli değil ise kilitle
                    if (message.sysLock)
                    {
                        if (message.Modifier == 0)
                        {
                            if (await _messagesDal.UpdateMessageModifierAsync(message.ID, 2, sysHash))
                            {
                                await GonderAsync(message, sysHash, cancellationToken);
                            }
                            else
                            {
                                //-2 Modifier Güncellenemedi
                                await _sysLoger.LogAsync(LogTipi.CoreGWErr, message.ID + " sms -2");
                            }
                        }
                        else
                        {
                            await _sysLoger.LogAsync(LogTipi.CoreGWErr, message.ID + " sms -7");
                        }
                    }
                    else
                    {
                        //await _sysLoger.LogAsync(LogTipi.CoreGWErr, message.ID + " SendSMSToDialogicAsync foreach: Message Not Lock!");
                    }
                }
            }
            catch (Exception ex)
            {
                await _sysLoger.LogAsync(LogTipi.CoreGWErr, "SendSMSToDialogicAsync Err:"+ ex.Message);
            }
        }

        public async Task GonderAsync(Messages sms, long syshash, CancellationToken cancellationToken)
        {
            var encoding = _smsUtil.GetEncoding(sms.CharsetID);
            if (!string.IsNullOrEmpty(encoding))
            {
                if (encoding == "latin1")
                {
                    sms.Body = _smsUtil.MesajMetniniTemizle(sms.Body);
                }
            }

            //UDH API üzerinden mesaj içeriğini al
            var ud = await _udhApi.GetUdhAsync(sms.Body, encoding, false);
            //Console.WriteLine($"GetUdhAsync2Async RESULT:{res1}");

            //var resCon = res1;
            //var ud = res1;
            if (ud != null)
            {
                //Sms ile ilgili bilgiler dolduruluyor
                ud.ChannelId = sms.ChannelID;
                ud.GSMNO = sms.ToAddress;
                ud.Originator = sms.FromAddress;
                ud.ID = sms.ID;
                IDialogicSender dialogicSender = new DialogicSender(_messagesDal, _chanelId, out OchannelId);
                _chanelId = OchannelId;
                if (ud.Contents.Count <= 1)
                {
                    var res = await dialogicSender.SendOneMessage(ud,syshash);
                    //Mesaj tek parçamı ?
                  await _sysLoger.LogAsync(LogTipi.CoreGWSend, sms.ID+ " SinglePart RES:" +res+ " Chnl:"+OchannelId);
                }
                else
                {
                    var res = await dialogicSender.SendManyMessage(ud,syshash);
                    //Mesaj birden fazla parça mı ?
                    await _sysLoger.LogAsync(LogTipi.CoreGWSend, sms.ID + " ManyPart RES:" + res + " Chnl:" + OchannelId);
                }

               
            }
            else
            {
                await _sysLoger.LogAsync(LogTipi.CoreGWErr, sms.ID + " GetUdhAsync2Async is NULL");
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
           // _sysLoger.LogAsync(LogTipi.CoreGWSend, "StopAsync:" + DateTime.UtcNow.ToString("O"));
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private static long GetRandomNumber(int min, int max)
        {
            lock (Getrandom) // synchronize
            {
                return Getrandom.Next(min, max);
            }
        }
    }
}
