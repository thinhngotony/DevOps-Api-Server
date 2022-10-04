using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjp.Rfid.SmartShelf.Models
{
    public class RfidShelfLogTable
    {
        public long Id { get; set; }
        public DateTime DDate { get; set; }
        public string Rfid { get; set; }
        public int Cnt { get; set; } = 0;
        public DateTime? OutTime { get; set; }
        public DateTime? InTime { get; set; }

        public long DiffInSeconds { get; set; }
    }
}
