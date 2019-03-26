﻿using Dlgc.Data.Dal;
using Dlgc.Data.ErrXmlMap;
using Sky.Dialogic.Util;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Sky.Dialogic.Sender.Channel
{
    public class Channel_2 : IChannel
    {
        string username = "3C1BWsAPI";
        string passWord = "DLc9q192@";

        readonly IMessagesDal _messagesDal;
        private ISmsUtil _smsUtil = new SmsUtil();
        private string Path;

        public Channel_2(IMessagesDal messagesDal, string path)
        {
            _messagesDal = messagesDal;
            Path = path;
        }




        public static HttpClient _httpClient;
        private HttpClient createRequest()
        {
            var credentials = new NetworkCredential(username, passWord);
            var handler = new HttpClientHandler { Credentials = credentials };
            _httpClient = new HttpClient(handler);
            return _httpClient;
        }


        public async Task<int> PostXml(string url, string content, long mesID, long sysHash)
        {
            var client = createRequest();
            var httpContent = new StringContent(content, Encoding.UTF8, "application/xml");
            var response = await client.PostAsync(url, httpContent);
            if (response.StatusCode == HttpStatusCode.NoContent)
            {
                var messageContents = await response.Content.ReadAsStringAsync();

                if (await _messagesDal.UpdateSuccessMessageAsync(mesID, sysHash))
                {
                    #region File Log
                    if (!File.Exists(Path))
                    {
                        using (var sw = File.CreateText(Path))
                        {
                            sw.WriteLine(DateTime.UtcNow.ToString("O") + " " + mesID + " Chnl02 OK");
                        }
                    }
                    else
                    {
                        using (var sw = File.AppendText(Path))
                        {
                            sw.WriteLine(DateTime.UtcNow.ToString("O") + " " + mesID + " Chnl02 OK");
                        }
                    }
                    #endregion
                    return 1;
                }
                else
                {
                    //Bir kez daha dene
                    if (await _messagesDal.UpdateSuccessMessageAsync(mesID, sysHash))
                    {
                        return 1;
                    }
                    else
                    {
                        return -6;
                    }
                }
            }
            else
            {
                var messageContents = await response.Content.ReadAsStringAsync();

                ErrorMessage err = _smsUtil.FromXml<ErrorMessage>(messageContents);
                err.ID = mesID;

                #region File Log
                if (!File.Exists(Path))
                {
                    using (var sw = File.CreateText(Path))
                    {
                        sw.WriteLine(DateTime.UtcNow.ToString("O") + " " + mesID + "Chnl02 Content:" + content + " PostXml Error:" + err.Text);
                    }
                }
                else
                {
                    using (var sw = File.AppendText(Path))
                    {
                        sw.WriteLine(DateTime.UtcNow.ToString("O") + " " + mesID + "Chnl02 Content:" + content + " PostXml Error:" + err.Text);
                    }
                }
                #endregion

                if (await _messagesDal.UpdateErrorMessageAsync(err, sysHash))
                {
                    return -5;
                }
                else
                {
                    return -8;
                }
            }
        }
    }
}
