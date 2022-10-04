namespace Vjp.Rfid.SmartShelf.Models
{
    public class OpenDbInfo
    {
        public OpenDbInfo()
        {
            Onix = new OnixInfo();
            Hanmoto = new HanmotoInfo();
            Summary = new SummaryInfo();
        }

        public OnixInfo Onix { get; set; }
        public HanmotoInfo Hanmoto { get; set; }

        public SummaryInfo Summary { get; set; }

        
    }
    public class SummaryInfo
    {
        public SummaryInfo()
        {
        }

        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Volume { get; set; }
        public string Series { get; set; }
        public string Publisher { get; set; }
        public string Pubdate { get; set; }
        public string Cover { get; set; }
        public string Author { get; set; }


    }

    public class OnixInfo
    {
        public OnixInfo()
        {
        }
    }

    public class HanmotoInfo
    {
        public HanmotoInfo()
        {
        }
    }
}
