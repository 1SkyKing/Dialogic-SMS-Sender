namespace Sky.Dialogic.Util
{
    public interface ISmsUtil
    {
        string GetSMSURL(int channelId);
        T FromXml<T>(string xml);
        string GetEncoding(int charsetId);
        string MesajMetniniTemizle(string body);
        string GetProfile(int channelId);
        string CheckNumber(string number);
    }
}