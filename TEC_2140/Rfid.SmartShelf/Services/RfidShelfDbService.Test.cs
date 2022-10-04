using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Services
{
    public class RfidShelfDbServiceTest
    {
        private readonly static RfidShelfDbService rfidShelfDbService = new RfidShelfDbService();
        private static string tagId = "";
        public RfidShelfDbServiceTest()
        {
            Random rand = new Random();
            int number = rand.Next(0, int.MaxValue);

            tagId = "TAG_A" ;

            //put item to shelf
            rfidShelfDbService.PutItem(tagId);

            //pop item from shelf 
            rfidShelfDbService.PopItem(tagId);

            //put item to shelf            
            rfidShelfDbService.PutItem(tagId);
            
        }

        public void InitRecordTest()
        {
           

            int result = rfidShelfDbService.InitShelfLogRecordByRfid(tagId);
            if (result > 0)
                Console.WriteLine("INSERT INIT RECORD OK");
            else
                Console.WriteLine("INSERT INIT RECORD FAILED");
        }

    }
}
