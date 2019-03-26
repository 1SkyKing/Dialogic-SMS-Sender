using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace Sky.Dialogic.Client
{
    public class MyHttpClient 
    {
        public static HttpClient _httpClient;
        public MyHttpClient (NetworkCredential credantial )
        {
            var handler = new HttpClientHandler { Credentials = credantial };
            _httpClient = new HttpClient(handler);
        }

    }
}
