namespace Vjp.Rfid.SmartShelf.Models
{

    public class RfidToJanResponse
    {
        public string Code { get; set; }
        public RfidDataHeaderResponse Data { get; set; }
        public string Message { get; set; }

    }
    public class RfidDataHeaderResponse
    {
        public string jancode_1{ get; set; }
        public string jancode_2 { get; set; }
        public string rfid { get; set; }

    }

}
