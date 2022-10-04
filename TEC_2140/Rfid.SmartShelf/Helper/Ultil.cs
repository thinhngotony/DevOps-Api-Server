using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Helper
{
    public static class Ultil
    {
        public static string DataFromClient=string.Empty;
        public static string ConvertTagIDCode(string code_value)
        {
            Dictionary<char, char> nibble_code = new Dictionary<char, char> { { ':', 'A' }, { ';', 'B' }, { '<', 'C' }, { '=', 'D' }, { '>', 'E' }, { '?', 'F' } };
            var stringBuilder = new StringBuilder();
            foreach (var character in code_value)
            {
                if (nibble_code.TryGetValue(character, out var value))
                {
                    stringBuilder.Append(value);
                }
                else
                {
                    stringBuilder.Append(character);
                }
            }
            return stringBuilder.ToString();
        }

        public static RfidView  RawTagToReadable(string rawTag)
        {
            RfidView rfidView = new RfidView();
            Dictionary<char, char> nibble_code = new Dictionary<char, char> { { ':', 'A' }, { ';', 'B' }, { '<', 'C' }, { '=', 'D' }, { '>', 'E' }, { '?', 'F' } };
            var stringBuilder = new StringBuilder();
            foreach (var character in rawTag)
            {
                if (nibble_code.TryGetValue(character, out var value))
                {
                    stringBuilder.Append(value);
                }
                else
                {
                    stringBuilder.Append(character);
                }
            }

            rfidView.Rfid = stringBuilder.ToString();

            if (stringBuilder.ToString().Length > 16)
            {
                //get rfid
                rfidView.Rfid = stringBuilder.ToString().Substring(16);

                //get anten no
                rfidView.AntenNo = stringBuilder.ToString().Substring(2, 2);
                //get anten no
                byte bt = byte.Parse(stringBuilder.ToString().Substring(2, 2), System.Globalization.NumberStyles.HexNumber);

                //Console.WriteLine("stringBuilder.ToString().Substring(2, 2) = " + stringBuilder.ToString().Substring(2, 2));
                //Console.WriteLine("Convert to bt =" + bt);

                if (IsBitSet(bt, 0))
                    rfidView.AntenNo = "0001";

                if (IsBitSet(bt, 1))
                    rfidView.AntenNo = "0010";

                if (IsBitSet(bt, 2))
                    rfidView.AntenNo = "0100";

                if (IsBitSet(bt, 3))
                    rfidView.AntenNo = "1000";


                //get rssi
                rfidView.RSSI = stringBuilder.ToString().Substring(4, 2);

                //get orther info


            }
            
            return rfidView;
        }
        public static string HexToBinary(string hexValue)
        {
            Console.WriteLine("hexValue =" + hexValue);

            ulong number = UInt64.Parse(hexValue, System.Globalization.NumberStyles.HexNumber);

            byte[] bytes = BitConverter.GetBytes(number);

            string binaryString = string.Empty;
            foreach (byte singleByte in bytes)
            {
                binaryString += Convert.ToString(singleByte, 2);
            }

            Console.WriteLine("binaryString =" + binaryString);
            return binaryString;
        }
        public static bool IsBitSet(byte b, int pos)
        {
            return (b & (1 << pos)) != 0;
        }
        
        public static List<string> SrcNotInDesList(List<string> src , List<string> des)
        {
            return src.Except(des).ToList();
        }

        public static List<string> DesNotInSrcList(List<string> src, List<string> des)
        {
            return des.Except(src).ToList();
        }

        public static string SaveImage(string imageUrl , string fileName , ImageFormat format)
        {
            string pathFile = "";

            using (WebClient webClient = new WebClient())
            {
                string ext = System.IO.Path.GetExtension(imageUrl);

                webClient.DownloadFile(imageUrl, fileName + ext);

                pathFile = fileName + ext;
            }
            return pathFile;
        }

        public static T FirstOrDefault<T>(this ExpandoObject eo, string key)
        {
            if (eo == null)
                return default(T);

            object r = eo.FirstOrDefault(x => x.Key == key).Value;
            return (r is T) ? (T)r : default(T);
        
        }

        public static string GetSystemErrorMsg(Exception ex)
        {
            string msg = "";
            if (ex != null)
            {
                msg = ex.Message;
                if(ex.InnerException != null)
                {
                    msg += ex.InnerException.Message;
                }
            }
            return msg;
        }

        public static string GetDateTimeForOutputLog()
        {
            return DateTime.Now.ToString("HH:mm:ss:ffff"); 
        }

        public static string GetVersion()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.Diagnostics.FileVersionInfo fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
            if (fvi == null)
            {
                return "";
            }
            else
            {
                return fvi.FileVersion;
            }

        }
    }
}
