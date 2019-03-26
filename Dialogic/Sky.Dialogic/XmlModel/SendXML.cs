using Dlgc.Data.UDH;
using Sky.Dialogic.Util;

namespace Sky.Dialogic.XmlModel
{
    public class SendXML : ISendXML
    {

        ISmsUtil _smsUtil = new SmsUtil();
        public string GetOpenTypeXml()
        {
            var adress = "http://37.202.48.114:81/dialogicwebservice/signaling/profile/TURKCELL_BASKENT/905338129142/sms";

            var xml = @"<?xml version='1.0' encoding='utf-8'?>
                        <sms xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
                        <message encodingScheme='#encodingScheme#'>#Body#</message>
                        <orig #OrigAttribute# >#Origin#</orig>
                     </sms>";

            //#OrigAttribute#
            //ton='Alphanumeric'
            //ton='International' npi='ISDN'
            return xml;
        }


        public string GetUdhTypeXml(EncodeUDH msg, int index)
        {
            //Numara adreste yok
            //var adress = "http://37.202.48.114:81/dialogicwebservice/signaling/profile/TURKCELL_BASKENT/sms";
            var tmpOriginator = _smsUtil.CheckNumber(msg.Originator);
            var originAtrr = "";
            if (!string.IsNullOrEmpty(tmpOriginator))
            {
                originAtrr = " ton='International' npi='ISDN' ";
            }
            else
            {
                originAtrr = " ton='Alphanumeric' ";
            }

            var a = @"<?xml version='1.0' encoding='utf-8' ?>
            <sms xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>
            <dest>#ToAddress#</dest>
            <orig #OrigAttribute#>#Origin#</orig>
                <smsDeliver>
                    <mms>#mms#</mms>
                    <rp>#rp#</rp>
                    <udhi>#udhi#</udhi>
                    <udl>#udl#</udl>
                    <sri>#sri#</sri>
                    <dcs>#dsc#</dcs>
                    <pid>#pid#</pid>
                    <scts>#scts#</scts>
                    <ud>#ud#</ud>
                </smsDeliver>
            </sms>";
            a = a.Replace("#ToAddress#", msg.GSMNO)
                .Replace("#OrigAttribute#", originAtrr)
                .Replace("#Origin#", msg.Originator)
                .Replace("#mms#", msg.mms.ToString())
                .Replace("#rp#", false.ToString())
                .Replace("#udhi#", msg.udhi.ToString())
                .Replace("#udl#", msg.Contents[index].udl.ToString())
                .Replace("#sri#", true.ToString())
                .Replace("#dsc#", 0.ToString())
                .Replace("#pid#", 0.ToString())
                .Replace("#scts#", msg.Scts)
                .Replace("#ud#", msg.Contents[index].ud);
            //#OrigAttribute#
            //ton='Alphanumeric'
            //ton='International' npi='ISDN'
            return a;
        }
    }
}
