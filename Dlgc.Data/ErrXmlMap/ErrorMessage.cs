using System.Xml.Serialization;

namespace Dlgc.Data.ErrXmlMap
{
    [XmlRoot("error")]
    public class ErrorMessage
    {
        /// <summary>
        /// Mesaj ID
        /// </summary>
        public long ID { get; set; }

        [XmlElement("text")]
        public string Text { get; set; }

        [XmlElement("code")]
        public Code code { get; set; }
    }

    [XmlRoot("code")]
    public class Code
    {
        [XmlAttribute("type")]
        public string Type { get; set; }

        [XmlElement("mapErrorCode")]
        public string MapErrorCode { get; set; }
    }
}
