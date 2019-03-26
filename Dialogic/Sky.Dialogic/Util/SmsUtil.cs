using System;
using System.IO;
using System.Xml.Serialization;

namespace Sky.Dialogic.Util
{
    public  class SmsUtil : ISmsUtil
    {
        private string _dialogicBaseAddres = "http://37.202.48.114:81/";
        private string _dialogicServiceAddres = "dialogicwebservice/signaling/profile/";
        private string _dialogicSMSMethod = "/sms";


        public string GetSMSURL(int channelId)
        {
            var smsUrl = _dialogicBaseAddres + _dialogicServiceAddres + GetProfile(channelId) + _dialogicSMSMethod;
            return smsUrl;
        }

        /// <summary>
        /// XML den nesne döndürür
        /// YourStrongTypedEntity entity = FromXml<YourStrongTypedEntity>(YourMsgString);
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public T FromXml<T>(string xml)
        {
            T returnedXmlClass = default(T);

            try
            {
                using (TextReader reader = new StringReader(xml))
                {
                    try
                    {
                        returnedXmlClass =
                            (T)new XmlSerializer(typeof(T)).Deserialize(reader);
                    }
                    catch (InvalidOperationException)
                    {
                        // String passed is not XML, simply return defaultXmlClass
                    }
                }
            }
            catch (Exception ex)
            {

            }

            return returnedXmlClass;
        }


        public string MesajMetniniTemizle(string body)
        {
            body = body.Replace('`', '\'');
            body = body.Replace("‘", "\'");
            body = body.Replace("’", "\'");
            body = body.Replace("“", "\"");
            body = body.Replace("”", "\"");
            body = body.Replace("-", "-");
            body = body.Replace("…", "...");

            body = body.Replace("İ", "i");
            body = body.Replace("ı", "i");

            body = body.Replace("ş", "s");
            body = body.Replace("Ş", "S");

            body = body.Replace("ç", "c");
            body = body.Replace("Ç", "C");

            body = body.Replace("ğ", "g");
            body = body.Replace("Ğ", "G");

            body = body.Replace("ü", "u");
            body = body.Replace("Ü", "U");
            body = body.Replace(' ', ' ');//Görünmeyen tire işareti!
            return body;
        }
        public string GetEncoding(int charsetId)
        {
            //
            switch (charsetId)
            {
                case 1:
                    return "latin1";
                case 2:
                    return "tr";
                case 3:
                    return "ucs2";
                default:
                    return "";
            }
        }

        public string GetProfile(int channelId)
        {
            var res = "";
            switch (channelId)
            {
                case 1102:
                    res = "TURKCELL_BASKENT";
                    break;
                case 1125:
                    res = "TURKCELL_GEBZE";
                    break;
                case 1112:
                    res = "VODAFONE_PURSAKLAR";
                    break;
                case 1104:
                    res = "AVEA";
                    break;
                case 1105:
                    res = "AVEA";
                    break;
            }

            return res;
        }

        public string CheckNumber(string number)
        {
            var tmpNumber = number.Trim();
            switch (tmpNumber.Length)
            {
                case 12://
                    return tmpNumber.Substring(0, 2) == "90" ? (IsNumeric(tmpNumber) ? tmpNumber : "") : "";
                case 11://
                    var retNumber = "";
                    var firstTwoDigit = tmpNumber.Substring(0, 2);
                    if (firstTwoDigit == "05")
                    {
                        if (IsNumeric(tmpNumber))
                        {
                            retNumber = "9" + tmpNumber;
                        }
                        else
                        {
                            retNumber = "";
                        }
                    }
                    else if (firstTwoDigit == "08")
                    {
                        if (IsNumeric(tmpNumber))
                        {
                            retNumber = "9" + tmpNumber;
                        }
                        else
                        {
                            retNumber = "";
                        }
                    }
                    return retNumber;
                //tmpNumber.Substring(0, 2) == "05"
                //    ? (Kontrol.IsNumeric(tmpNumber) ? "9" + tmpNumber : "")
                //    : "";
                case 10://
                    return tmpNumber.Substring(0, 1) == "5"
                        ? (IsNumeric(tmpNumber) ? "90" + tmpNumber : "")
                        : "";
                case 13://
                    if (tmpNumber.Substring(0, 2) == "90")
                    {
                        //var num = tmpNumber.Remove(0, 2);
                        if (IsNumeric(tmpNumber))
                        {
                            return tmpNumber;
                        }
                        return "";
                    }
                    return "";
                default:
                    return "";
            }
        }

        public static bool IsNumeric(string value) //Numeric Kontrolu 
        {
            float output;
            return float.TryParse(value, out output);
        }

    }
}
