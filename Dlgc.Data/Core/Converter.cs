using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Dlgc.Data.Core
{
    public class Converter
    {

        #region List To DataTable
        public static System.Data.DataTable ConvertListToDatatable<T>(List<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            System.Data.DataTable table = new System.Data.DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                if (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    table.Columns.Add(prop.Name, prop.PropertyType.GetGenericArguments()[0]);
                else
                    table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
        #endregion

        #region YaziyaÇevir
        public string YaziyaCevir(decimal tutar)
        {
            string sTutar = tutar.ToString("F2").Replace('.', ','); // Replace('.',',') ondalık ayracının . olma durumu için            
            string lira = sTutar.Substring(0, sTutar.IndexOf(',')); //tutarın tam kısmı
            string kurus = sTutar.Substring(sTutar.IndexOf(',') + 1, 2);
            string yazi = "";

            string[] birler = { "", "BİR", "İKİ", "ÜÇ", "DÖRT", "BEŞ", "ALTI", "YEDİ", "SEKİZ", "DOKUZ" };
            string[] onlar = { "", "ON", "YİRMİ", "OTUZ", "KIRK", "ELLİ", "ALTMIŞ", "YETMİŞ", "SEKSEN", "DOKSAN" };
            string[] binler = { "KATRİLYON", "TRİLYON", "MİLYAR", "MİLYON", "BİN", "" }; //KATRİLYON'un önüne ekleme yapılarak artırabilir.

            int grupSayisi = 6; //sayıdaki 3'lü grup sayısı. katrilyon içi 6. (1.234,00 daki grup sayısı 2'dir.)
            //KATRİLYON'un başına ekleyeceğiniz her değer için grup sayısını artırınız.

            lira = lira.PadLeft(grupSayisi * 3, '0'); //sayının soluna '0' eklenerek sayı 'grup sayısı x 3' basakmaklı yapılıyor.            

            string grupDegeri;

            for (int i = 0; i < grupSayisi * 3; i += 3) //sayı 3'erli gruplar halinde ele alınıyor.
            {
                grupDegeri = "";

                if (lira.Substring(i, 1) != "0")
                    grupDegeri += birler[Convert.ToInt32(lira.Substring(i, 1))] + "YÜZ"; //yüzler                

                if (grupDegeri == "BİRYÜZ") //biryüz düzeltiliyor.
                    grupDegeri = "YÜZ";

                grupDegeri += onlar[Convert.ToInt32(lira.Substring(i + 1, 1))]; //onlar

                grupDegeri += birler[Convert.ToInt32(lira.Substring(i + 2, 1))]; //birler                

                if (grupDegeri != "") //binler
                    grupDegeri += binler[i / 3];

                if (grupDegeri == "BİRBİN") //birbin düzeltiliyor.
                    grupDegeri = "BİN";

                yazi += grupDegeri;
            }

            if (yazi != "")
                yazi += " TL ";

            int yaziUzunlugu = yazi.Length;

            if (kurus.Substring(0, 1) != "0") //kuruş onlar
                yazi += onlar[Convert.ToInt32(kurus.Substring(0, 1))];

            if (kurus.Substring(1, 1) != "0") //kuruş birler
                yazi += birler[Convert.ToInt32(kurus.Substring(1, 1))];

            if (yazi.Length > yaziUzunlugu)
                yazi += " Kr.";
            else
                yazi += "SIFIR Kr.";

            return yazi;
        }
        #endregion

        #region MaskedTLtoDecimal

        public static decimal ConverdMaskedTLtoDecimal(string fiyat)
        {
            try
            {
                var dfiyat = decimal.Parse(0.ToString());
                var replaced = fiyat.Replace("_", "");
                replaced = replaced.Replace("₺", "");
                replaced = replaced.Replace(".", "");
                replaced = replaced.Replace(" ", "");
                if (!string.IsNullOrEmpty(replaced))
                {
                    dfiyat = decimal.Parse(replaced.Trim());

                }
                return dfiyat;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        #endregion

        #region ConvertToChar

        public static char ToChar(object val)
        {
            try
            {
                return Convert.ToChar(val);
            }
            catch
            {
                return Convert.ToChar("");
            }
        }
        #endregion

        #region ConvertToInt32
        public static int ToInt32(object val)
        {
            try
            {
                return Convert.ToInt32(val);
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region ConvertToInt64
        public static long ConvertToInt64(object val)
        {
            try
            {
                return Convert.ToInt64(val);
            }
            catch
            {
                return -1;
            }
        }
        #endregion





        #region ConvertToInt64
        public static long ToInt64(object val)
        {
            try
            {
                return Convert.ToInt64(val);
            }
            catch
            {
                return -1;
            }
        }
        #endregion


        #region ConvertToByte

        public static byte ToByte(object val)
        {
            try
            {
                return Convert.ToByte(val);
            }
            catch
            {
                return 0;
            }
        }
        #endregion

        #region ConvertToDecimal

        public static decimal ToDecimal(object val)
        {
            try
            {
                return Convert.ToDecimal(val);
            }
            catch
            {
                return -1;
            }
        }
        #endregion

        #region ConvertToBool
        public static bool ToBool(object val)
        {
            try
            {
                return Convert.ToBoolean(val);
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region ConvertToString
        public static string FormatTo2Dp(decimal myNumber)
        {
            // Use schoolboy rounding, not bankers.
            myNumber = Math.Round(myNumber, 2, MidpointRounding.AwayFromZero);

            return $"{myNumber:0.00}";
        }

        public static string ToString(object val)
        {
            try
            {
                return val.ToString();
            }
            catch
            {
                return "";
            }
        }
        #endregion

        #region ConvertToFloat

        public static float ToFloat(object val)
        {
            try
            {
                return float.Parse(val.ToString());
            }
            catch (Exception)
            {
                return -1f;
            }
        }
        #endregion

        #region ConvertToDateTime
        public static DateTime ToDateTime(object val)
        {
            try
            {
                return Convert.ToDateTime(val);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
        #endregion

        #region ArrayToHexString
        public static string ArrayToHexString(byte[] ar, int len)
        {
            //'İ':t:='0130';48
            //'ı':t:='0131';49
            //'Ğ':t:='011E';30
            //'ğ':t:='011F';31
            //'Ş':t:='015E';94
            //'ş':t:='015F';95

            int i;
            const string hex = "0123456789ABCDEF";
            var sTmp = "";
            for (i = 0; i < len; i++)
            {
                var strHx = "";
                var byt = ar[i];

                switch (byt)
                {
                    case 48:
                        strHx = "0130";
                        break;
                    case 49:
                        strHx = "0131";
                        break;
                    case 30:
                        strHx = "011E";
                        break;
                    case 31:
                        strHx = "011F";
                        break;
                    case 94:
                        strHx = "015E";
                        break;
                    case 95:
                        strHx = "015F";
                        break;
                }


                if (strHx == "")
                {


                    strHx = "00";
                    strHx = strHx + hex[(ar[i] >> 4) & 0x0F];
                    strHx = strHx + hex[ar[i] & 0x0F];

                }

                sTmp = sTmp + strHx;

            }
            return sTmp;
        }

        #endregion

        #region ArrayToHex
        public string ArrayToHex(byte[] ar, int len)
        {
            //'İ':t:='0130';48
            //'ı':t:='0131';49
            //'Ğ':t:='011E';30
            //'ğ':t:='011F';31
            //'Ş':t:='015E';94
            //'ş':t:='015F';95

            int i;
            const string hex = "0123456789ABCDEF";
            var sTmp = "";
            for (i = 0; i < len; i++)
            {
                var strHx = "";

                strHx = "";
                strHx = strHx + hex[(ar[i] >> 4) & 0x0F];
                strHx = strHx + hex[ar[i] & 0x0F];
                sTmp = sTmp + strHx;

            }
            return sTmp;
        }
        #endregion

        #region Inttobin
        public string Inttobin(int ina) //integer to Binary
        {
            var sBinnary = "";
            int b, c;
            var temp = 1;
            for (c = 0; c < 15; c++)
            {
                temp = temp * 2;
            }

            for (b = 15; b >= 0; b--)
            {
                if (ina / temp > 0)
                {
                    sBinnary = sBinnary + "1";
                    ina = ina - temp;
                }
                else
                {
                    sBinnary = sBinnary + "0";
                }

                temp = temp / 2;
            }
            return sBinnary;
        }
        #endregion

        #region StringToByteArray
        public static byte[] StringToByteArray(string str)
        {
            int i;
            var n = str.Length;
            var x = new byte[n];
            for (i = 0; i < n; i++)
            {
                x[i] = (byte)str[i];
            }
            return x;
        }
        #endregion

        //#region ConvertByteArrayToImage
        //public static System.Net.Mime.MediaTypeNames.Image ConvertByteArrayToImage(byte[] byteArray)
        //{
        //    if (byteArray != null)
        //    {
        //        var ms = new MemoryStream(byteArray, 0, byteArray.Length);
        //        ms.Write(byteArray, 0, byteArray.Length);
        //        return Image.FromStream(ms, true);
        //    }
        //    return null;
        //}
        //#endregion
    }
}
