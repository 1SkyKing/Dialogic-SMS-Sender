using Dlgc.Data.UDH;

namespace Sky.Dialogic.XmlModel
{
    public interface ISendXML
    {
        string GetOpenTypeXml();
        string GetUdhTypeXml(EncodeUDH msg, int index);
    }
}