using System.Net;
using System.Net.Http;

namespace Sky.Dialogic.Connection
{

    public class DlgRequest
    {
        public static HttpClient _httpClient;
        string username = "3C1BWsAPI";
        string passWord = "DLc9q192@";

        private HttpClient createRequest()
        {
            var credentials = new NetworkCredential(username, passWord);
            var handler = new HttpClientHandler { Credentials = credentials };
            _httpClient = new HttpClient(handler);
            return _httpClient;
        }
        

        //public async Task<bool> SendXml(string Xml, string url)
        //{
        //    var client = createRequest();
        //    client.BaseAddress = new Uri(url);
        //    client.DefaultRequestHeaders.Accept.Clear();
        //    client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/xml"));

        //    HttpResponseMessage response = await client.PostAsXmlAsync<CustomFormLog>("logdata/customform/1234", customFormLog);
        //}


    }
}
