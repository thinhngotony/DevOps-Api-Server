using Vjp.Rfid.SmartShelf.Enums;

namespace Vjp.Rfid.SmartShelf.Models
{
    public class RfidView
    {
        public string Rfid { get; set; }
        public string AntenNo { get; set; }
        public string RSSI { get; set; }
        public string ScannerDeviceName { get; set; }
        public string ShelfName { get; set; }
        public int ReadCount { get; set; }
        public ShelfRfidAction ShelfOperation { get; set; }

    }
}
