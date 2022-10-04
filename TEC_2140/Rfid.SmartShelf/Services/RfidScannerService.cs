using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Vjp.Rfid.SmartShelf.Helper;
using Vjp.Rfid.SmartShelf.Interface;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Services
{
    public class RfidScannerService
    {
        //Get product list 

        //private readonly static RfidShelfProductService rfidShelfProductService = new RfidShelfProductService();
        //private readonly static RfidShelfDbService rfidShelfDbService = new RfidShelfDbService();
        private static log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private readonly  IRfidShelfProductService rfidShelfProductService ;
        private readonly  IRfidShelfDbService rfidShelfDbService ;
        private readonly IRfidShelfHttpService rfidShelfHttpService;
        public string[] ReadedRfids { get; set; }
        public List<RfidShelfProduct> RfidCurrentShelfProducts; 
        public static List<RfidShelfLogTable> RfidShelfLogTableLatest; //latest data of rfid log 

        public RfidScannerService()
        {
            //get instance from service
            rfidShelfProductService = Program.GetService<IRfidShelfProductService>();
            rfidShelfDbService = Program.GetService<IRfidShelfDbService>();

            rfidShelfHttpService = Program.GetService<IRfidShelfHttpService>();

            //rfidShelfProductService = new RfidShelfProductService();
            //rfidShelfDbService = new RfidShelfDbService();

            logger.Info("Load latest log data");
            RfidShelfLogTableLatest = rfidShelfDbService.GetLatestRfidShelfLogs();
        }

        public List<RfidShelfLogTable> GetLatestRfidShelfLogs()
        {
            return rfidShelfDbService.GetLatestRfidShelfLogs();
        }

        public List<RfidShelfProduct> GetRfidShelfProducts()
        {
            return rfidShelfProductService.GetRfidShelfProducts();
        }

        public bool ItemInOutShelfProcess(List<string> readedTags)
        {
            bool result = true;

            if (RfidCurrentShelfProducts == null)
                return result;

            var shelfProductsList = RfidCurrentShelfProducts?.Select(x => x.Rfid).ToList();
            if (readedTags.Count == 0)
            {
                //no data
                // all data in rfidCurrentShelfProducts is extract out of shelf
                PopItems(RfidCurrentShelfProducts.Select(x => x.Rfid).ToList());
                //for (int i=0; i< rfidCurrentShelfProducts.Count -1; i++)
                //{
                //    rfidShelfDbService.PopItem(rfidCurrentShelfProducts[i].Rfid);
                //}
            }
            else
            {
                //has data
                // A = get tags where : existed in readedTags but not in rfidCurrentShelfProducts
                var tagsA = Ultil.SrcNotInDesList(readedTags, shelfProductsList);

                PutItems(readedTags);

                logger.Info(" PutItems tags list " + string.Join("," , readedTags));
                // B = get tags where : existed in rfidCurrentShelfProducts but not in readedTags ==> B will extract
                var tagsB = Ultil.SrcNotInDesList(shelfProductsList, readedTags);

                logger.Info(" PopItems tags list " + string.Join(",", tagsB));

                PopItems(tagsB);
                // C = get tags where : existed in readedTags and existed in rfidCurrentShelfProducts ==> C will return ( already item push to shelf)
                if (tagsA.Count  == 0 && tagsB.Count == 0)
                {
                    //two list the same( all book in the shelf)
                    PutItems(shelfProductsList);
                }

            }

            return result;
        }


        public Task<bool> ItemInOutShelfProcessAsync(List<string> readedTags)
        {
            return Task.Run(() =>
           {
               bool result = true;
               //Console.WriteLine("Start run ItemInOutShelfProcessAsync");

               if (RfidCurrentShelfProducts == null)
                   return result;

               var shelfProductsList = RfidCurrentShelfProducts?.Select(x => x.Rfid).ToList();
               if (readedTags.Count == 0)
               {
                   //no data
                   // all data in rfidCurrentShelfProducts is extract out of shelf
                   PopItems(RfidCurrentShelfProducts.Select(x => x.Rfid).ToList());
                   //for (int i=0; i< rfidCurrentShelfProducts.Count -1; i++)
                   //{
                   //    rfidShelfDbService.PopItem(rfidCurrentShelfProducts[i].Rfid);
                   //}
               }
               else
               {
                   //has data
                   // A = get tags where : existed in readedTags but not in rfidCurrentShelfProducts
                   var tagsA = Ultil.SrcNotInDesList(readedTags, shelfProductsList);

                   PutItems(readedTags);
                   // B = get tags where : existed in rfidCurrentShelfProducts but not in readedTags ==> B will extract
                   var tagsB = Ultil.SrcNotInDesList(shelfProductsList, readedTags);

                   PopItems(tagsB);
                   // C = get tags where : existed in readedTags and existed in rfidCurrentShelfProducts ==> C will return ( already item push to shelf)
                   if (tagsA.Count == 0 && tagsB.Count == 0)
                   {
                       //two list the same( all book in the shelf)
                       PutItems(shelfProductsList);
                   }

               }
               //Console.WriteLine("End run ItemInOutShelfProcessAsync");
               return result;

           });

        }

        public Task<bool> ItemInOutShelfProcessAsync(List<string> readedTags , List<string> unReadedTags, string shelfName, string scannerDeviceName )
        {
            return Task.Run(() =>
            {
                bool result = true;

                logger.Info($"{shelfName} ItemInOutShelfProcessAsync Tags Count : " + readedTags.Count());
                logger.Info($"{shelfName}----------ReadedTags-----------");
                foreach (var tag in readedTags)
                {
                    logger.Info($"{shelfName} readedTags >> " + tag);
                }
                logger.Info($"{shelfName}----------unReadedTags-----------");
                foreach (var tag in unReadedTags)
                {
                    logger.Info($"{shelfName} unReadedTags >> " + tag);
                }
                logger.Info($"{shelfName}---------------------");

                if (RfidCurrentShelfProducts == null || RfidCurrentShelfProducts?.Count==0)
                    return result;

                //var shelfProductsList = RfidCurrentShelfProducts?.Where(x=>x.ScannerName == scannerDeviceName )?.Select(x => x.Rfid ).ToList();
                var shelfProductsList = new List<string>() ;
                if (ConfigFile.InOutItemDependOnShelf == "0")
                {
                    shelfProductsList = RfidCurrentShelfProducts.Select(x => x.Rfid).ToList();
                }
                else
                {
                    shelfProductsList = RfidCurrentShelfProducts?.Where(x => x.ScannerName.ToLower() == scannerDeviceName.ToLower()
                                                                        && x.ShelfName.ToLower() == shelfName.ToLower())?.Select(x => x.Rfid).ToList();
                }
                if (readedTags.Count == 0)
                {
                    //no data
                    // all data in rfidCurrentShelfProducts is extract out of shelf
                    PopItems(shelfProductsList);
                    //for (int i=0; i< rfidCurrentShelfProducts.Count -1; i++)
                    //{
                    //    rfidShelfDbService.PopItem(rfidCurrentShelfProducts[i].Rfid);
                    //}
                }
                else
                {
                    //has data
                    // A = get tags where : existed in readedTags but not in shelfProductsList
                    var tagsA = Ultil.SrcNotInDesList(readedTags, shelfProductsList);

                    //only tags existed in shelf product list table
                    //PutItems(readedTags);
                    PutItems(readedTags.Where( x=> shelfProductsList.Contains(x)).ToList());

                    // B = get tags where : existed in rfidCurrentShelfProducts but not in readedTags ==> B will extract
                    var tagsB = Ultil.SrcNotInDesList(shelfProductsList, readedTags);

                    PopItems(unReadedTags);
                    // C = get tags where : existed in readedTags and existed in rfidCurrentShelfProducts ==> C will return ( already item push to shelf)
                    if (tagsA.Count == 0 && tagsB.Count == 0)
                    {
                        //two list the same( all book in the shelf)
                        logger.Info($"{scannerDeviceName}----------Put Item In Case (tagsA.Count == 0 && tagsB.Count == 0)-----------");
                        PutItems(shelfProductsList);
                    }

                }
                
                return result;

            });
        }


        private int PopItems(List<string> tags)
        {
            int result = 0;
            //logger.Info("PopItems Start");
            //for (int i = 0; i <= tags.Count-1 ; i++)
            //{
            //    logger.Info($"PopItems {tags[i]}");
            //    result += rfidShelfDbService.PopItem(tags[i]);
            //}
            //logger.Info("PopItems End");

            string sqlInsertStr = "", sqlUpdateStr = "";
            DbQueryStatement stm = new DbQueryStatement();

            logger.Info("PopItems Start");
            for (int i = 0; i <= tags.Count - 1; i++)
            {
                logger.Info($"PopItems {tags[i]}");
                stm = rfidShelfDbService.PopItemGetSql(tags[i]);

                //if (i != tags.Count - 1 && !string.IsNullOrEmpty(stm.QueryStr.Trim()) && stm.DbQueryType == Enums.DbQueryType.InsertQuery)
                //{
                //    stm.QueryStr = stm.QueryStr + ",";
                //}
                switch (stm.DbQueryType)
                {
                    case Enums.DbQueryType.InsertQuery:
                        if (!string.IsNullOrEmpty(stm.QueryStr.Trim()))
                            sqlInsertStr += stm.QueryStr + ",";

                        break;

                    case Enums.DbQueryType.UpdateQuery:
                        if (!string.IsNullOrEmpty(stm.QueryStr.Trim()))
                            sqlUpdateStr += stm.QueryStr;
                        break;
                }
            }
            
            //if (sqlStr.Trim().Length > 10)
            //{
            //    result = rfidShelfDbService.NonQueryExecRfidsShelfLogTable(sqlStr);
            //}
            if (sqlInsertStr.Trim().Length > 0)
            {
                char lastCharacter = sqlInsertStr[sqlInsertStr.Length - 1];
                if(lastCharacter==',')
                {
                    sqlInsertStr = sqlInsertStr.Remove(sqlInsertStr.Length - 1, 1);
                }
                logger.Info($"PopItems insert statement");
                result += rfidShelfDbService.NonQueryExecRfidsShelfLogTable(RfidShelfDbService.InsertIntoRfidShelfLogTableHeadSql + sqlInsertStr);
            }
            if (sqlUpdateStr.Trim().Length > 0)
            {
                char lastCharacter = sqlUpdateStr[sqlUpdateStr.Length - 1];
                if (lastCharacter == ',')
                {
                    sqlUpdateStr = sqlUpdateStr.Remove(sqlUpdateStr.Length - 1, 1);
                }
                logger.Info($"PopItems update statement");
                result += rfidShelfDbService.NonQueryExecRfidsShelfLogTable(sqlUpdateStr);
            }
            //reload log latest
            //if (result >0)
            if (result > 0 && ConfigFile.CheckLatestShelfLogMethods == Enums.CheckLatestShelfLogMethods.CheckInLocal)
            {
                logger.Info($"Shelf log data inserted or updated {result} records");
                logger.Info($"Reload latest log data");
                RfidShelfLogTableLatest = rfidShelfDbService.GetLatestRfidShelfLogs();
            }

            //notify realtime
            if (result > 0) {
                //RfidCurrentShelfProducts = GetRfidShelfProducts();
                SendItemInOutShelfChangeNotify();
            }
            logger.Info("PopItems End");
            return result;
        }

        private int PutItems(List<string> tags)
        {
            //int result = 0;
            //logger.Info("PutItems Start");
            //for (int i = 0; i <= tags.Count-1; i++)
            //{
            //    logger.Info($"PutItems {tags[i]}");
            //    result += rfidShelfDbService.PutItem(tags[i]);
            //}
            //logger.Info("PutItems End");

            int result = 0;
            string sqlInsertStr = "" , sqlUpdateStr = "" ;
            DbQueryStatement stm = new DbQueryStatement();

            logger.Info("PutItems Start");
            for (int i = 0; i <= tags.Count - 1; i++)
            {
                logger.Info($"PutItems {tags[i]}");

                stm = rfidShelfDbService.PutItemGetSql(tags[i]);

                //if (i != tags.Count - 1 && !string.IsNullOrEmpty(stm.QueryStr.Trim()))
                //{
                //    stm.QueryStr = stm.QueryStr + ",";
                //}

                switch (stm.DbQueryType)
                {
                    case Enums.DbQueryType.InsertQuery:
                        if (!string.IsNullOrEmpty(stm.QueryStr.Trim()))
                            sqlInsertStr += stm.QueryStr + ",";
                        
                        break;

                    case Enums.DbQueryType.UpdateQuery:
                        if (!string.IsNullOrEmpty(stm.QueryStr.Trim()))
                            sqlUpdateStr += stm.QueryStr;
                        break;
                }
            }
            
            //exec 
            if(sqlInsertStr.Trim().Length > 0)
            {
                char lastCharacter = sqlInsertStr[sqlInsertStr.Length - 1];
                if (lastCharacter == ',')
                {
                    sqlInsertStr = sqlInsertStr.Remove(sqlInsertStr.Length - 1, 1);
                }
                logger.Info($"PutItems insert statement");
                result += rfidShelfDbService.NonQueryExecRfidsShelfLogTable(RfidShelfDbService.InsertIntoRfidShelfLogTableHeadSql + sqlInsertStr);
            }
            if (sqlUpdateStr.Trim().Length > 0)
            {
                char lastCharacter = sqlUpdateStr[sqlUpdateStr.Length - 1];
                if (lastCharacter == ',')
                {
                    sqlUpdateStr = sqlUpdateStr.Remove(sqlUpdateStr.Length - 1, 1);
                }
                logger.Info($"PutItems update statement");
                result += rfidShelfDbService.NonQueryExecRfidsShelfLogTable(sqlUpdateStr);
            }

            //reload log latest
            if (result > 0 && ConfigFile.CheckLatestShelfLogMethods == Enums.CheckLatestShelfLogMethods.CheckInLocal)
            {
                logger.Info($"Shelf log data inserted or updated {result} records");
                logger.Info("Reload latest log data");
                RfidShelfLogTableLatest = rfidShelfDbService.GetLatestRfidShelfLogs();
            }

            //notify realtime
            if (result > 0)
            {
                //RfidCurrentShelfProducts = GetRfidShelfProducts();
                SendItemInOutShelfChangeNotify();
            }

            logger.Info("PutItems End");
            return result;
        }

        private void SendItemInOutShelfChangeNotify()
        {
            if(ConfigFile.RealTimeItemShiftNotify =="1")
                Task.Run(() => rfidShelfHttpService.ItemInOutShelfChangeNotify());

        }

        public int InsertRawRfidData(string sql)
        {
            int result = 0;

            result = rfidShelfDbService.NonQueryExec(sql);
            return result;
        }

    }
}
