using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Dlgc.Data.UDH;

namespace Sky.Dialogic.UdhApi
{
    public class UdhService : IUdhService
    {
        private static HttpClient Client = new HttpClient();
        private string ServisUrl { get; set; }
        public UdhService(string UDHServiceAddres)
        {
            //var tt = DialogicNetCoreTest.Program.Configuration;
            //http://localhost:8080/api/v1/encodeudh?content=Test%20Mesaj%C4%B1&encoding=tr&base64=false&pretty
            ServisUrl = UDHServiceAddres;
            //ServisUrl = ConfigurationManager.AppSettings["UDHServiceAddres"];
        }
        
        public async Task<EncodeUDH> GetUdhAsync(string content, string encoding, bool base64)
        {
            var content4Url = WebUtility.UrlEncode(content);
            try
            {
                content = content.Replace("\t", " ");
                //content = content.Replace("\"", " ");
                content = content.Replace(' ', ' ');//Görünmeyen tire işareti!

                var result = await Client.GetAsync(ServisUrl + "encodeudh?content=" + content4Url + "&encoding=" + encoding + "&base64=" + base64);
                result.EnsureSuccessStatusCode();
                string responseBody = await result.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(responseBody))
                {
                    var jSonContent = JsonConvert.DeserializeObject<EncodeUDH>(responseBody);
                    
                    //Console.WriteLine($"responseBody rr:{responseBody}");
                    return jSonContent;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetUdhAsync2Async Exception:{ex.Message}");
            }

           
            var dd = new EncodeUDH();
            return dd;
        }

        
    }
}
