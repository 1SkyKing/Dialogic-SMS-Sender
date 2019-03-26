using System.Collections.Generic;
using Newtonsoft.Json;

namespace Dlgc.Data.UDH
{
    public class EncodeUDH
    {
        [JsonProperty(PropertyName = "udhi")]
        public bool udhi { get; set; }

        [JsonProperty(PropertyName = "ref_no")]
        public int refno { get; set; }

        [JsonProperty(PropertyName = "contents")]
        public List<Content> Contents { get; set; }

        [JsonProperty(PropertyName = "scts")]
        public string Scts { get; set; }

        public long WorkMiliSecond { get; set; }

        public string GSMNO { get; set; }
        public string Address { get; set; }
        public string Originator { get; set; }
        public bool mms { get; set; }
        public int ChannelId { get; set; }
        /// <summary>
        /// Mesaj ID
        /// </summary>
        public long ID { get; set; }
    }

    public class DecodeUDH
    {
        [JsonProperty(PropertyName = "ref_no")]
        public int ref_no { get; set; }

        [JsonProperty(PropertyName = "part_count")]
        public int part_count { get; set; }

        [JsonProperty(PropertyName = "part_no")]
        public int part_no { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string content { get; set; }

        [JsonProperty(PropertyName = "base_64")]
        public bool base_64 { get; set; }
    }


    public class Content
    {
        [JsonProperty(PropertyName = "no")]
        public int No { get; set; }
        [JsonProperty(PropertyName = "ud")]
        public string ud { get; set; }
        [JsonProperty(PropertyName = "udl")]
        public int udl { get; set; }

    }
}
