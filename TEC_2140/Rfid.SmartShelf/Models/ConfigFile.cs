using Vjp.Rfid.SmartShelf.Enums;

namespace Vjp.Rfid.SmartShelf.Models
{
    public static class ConfigFile
    {
        public static string Url { get; set; }
        public static string[] RfidDeviceNames { get; set; }
        public static string[] RfidDeviceNamesMapToShelfNames { get; set; }
        public static string ApiKey { get; set; }
        public static string ShelfNo { get; set; }
        public static int ShelfProductAppendMode { get; set; } = 0;
        public static string DbHost { get; set; }
        public static string DbName { get; set; }
        public static string DbUser { get; set; }
        public static string DbPass { get; set; }

        public static int ScannerReadTimerInterval { get; set; } = 1000;
        public static int ReadHasTagsCounter { get; set; }
        public static int ReadEmptyTagCounter { get; set; }
        public static string RealTimeItemShiftNotify { get; set; } = "0";
        public static string ItemShiftNotifyUrl { get; set; } = "http://192.168.1.42:8000";
        public static string InOutItemDependOnShelf { get; set; }
        public static CheckLatestShelfLogMethods CheckLatestShelfLogMethods { get; set; }
        public static string RfidToJanUrl { get; set; }
        public static string JanToItemInfoUrl { get; set; }

        public static string DoNotCheckWhenAggLtHexValue { get; set; }
        /// <summary>
        /// If value set is 0 then do not reconnect
        /// </summary>
        public static string ReConnectDeviceInterval { get; set; } = "0";
        /// <summary>
        /// if scanner reading large volume tags => true 
        /// </summary>
        public static bool IsReadingBigVolumnRfidsMode { get; set; }
        /// <summary>
        /// reconnect db check
        /// </summary>
        public static int ReConnectDBInterval { get; set; } = 60 * 1000; //60 seconds
        /// <summary>
        /// Maximun retry DB connect
        /// </summary>
        public static int MaxRetryConnectDB { get; set; } = 10;

        public static string ReloadShelfPosMasterRfid { get; set; }

        //TcpHost
        public static string TcpHost { get; set; }

        // Convert Antena
        public static string AntenaList { get; set; }
        public static string NumberOfAntena { get; set; }



    }
}
