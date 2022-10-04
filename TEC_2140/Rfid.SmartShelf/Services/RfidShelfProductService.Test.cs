using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjp.Rfid.SmartShelf.Services
{
    public class RfidShelfProductServiceTest
    {
        private readonly static RfidShelfProductService rfidShelfProductService = new RfidShelfProductService();
        public RfidShelfProductServiceTest()
        {

            var rfidArr = rfidShelfProductService.ReadShelftProductCsvUseCsvHelper(@"D:\workspace\Rfid.SmartShelf\Rfid.SmartShelf\Data\productlistsample.csv");

            var result = rfidShelfProductService.InsertShelftProductToDb(rfidArr);
        }
    }
}
