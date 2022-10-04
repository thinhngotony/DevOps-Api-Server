using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using Vjp.Rfid.SmartShelf.Db;
using Vjp.Rfid.SmartShelf.Enums;
using Vjp.Rfid.SmartShelf.Helper;
using Vjp.Rfid.SmartShelf.Interface;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Services
{
    public class RfidShelfDbService : IRfidShelfDbService
    {
        
        private static readonly string RfidShelfLogTableName = "drfid_log_move";
        private static readonly string RfidShelfLogCntColName = "dlm_cnt";
        private static readonly string RfidShelfLogOutTimeColName = "dlm_outdate";
        private static readonly string RfidShelfLogInTimeColName = "dlm_indate";
        private static readonly string RfidShelfLogDDateColName = "dlm_date";
        private static readonly string RfidShelfLogTagNameColName = "dlm_rfid_cd";
        private static readonly string RfidShelfLogIdColName = "dlm_id";
        private static log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public static string InsertIntoRfidShelfLogTableHeadSql = $"INSERT INTO {RfidShelfLogTableName} ({RfidShelfLogDDateColName}, {RfidShelfLogTagNameColName}, {RfidShelfLogCntColName}, {RfidShelfLogOutTimeColName}, {RfidShelfLogInTimeColName}) VALUES";
        //private IDbAccess dbAccess;
        //private DbAccess dbAccess;
        public RfidShelfDbService()
        {
            //dbAccess = Program.GetService<IDbAccess>();
            //dbAccess = new DbAccess();

        }
        public List<RfidShelfLogTable> GetRfidShelfLogs()
        {
            using (var db = new DbAccess())
            {
                string sql = $"SELECT * FROM {RfidShelfLogTableName}"
                        + $" ORDER BY {RfidShelfLogDDateColName}, {RfidShelfLogTagNameColName} ,{ RfidShelfLogDDateColName } , {RfidShelfLogCntColName} ";

                DataTable dt = db.Query(sql);

                List<RfidShelfLogTable> items = dt.AsEnumerable().Select(item =>
                new RfidShelfLogTable
                {
                    Id = item.FieldOrDefaultValue<int>(RfidShelfLogIdColName),
                    DDate = item.FieldOrDefaultValue<DateTime>(RfidShelfLogDDateColName),
                    Rfid = item.FieldOrDefaultValue<string>(RfidShelfLogTagNameColName),
                    Cnt = item.FieldOrDefaultValue<int>(RfidShelfLogCntColName),
                    OutTime = item.FieldOrDefaultValue<DateTime>(RfidShelfLogOutTimeColName),
                    InTime = item.FieldOrDefaultValue<DateTime>(RfidShelfLogInTimeColName)
                }).ToList();


                return items;
            }
            
        }

        public List<RfidShelfLogTable> GetRfidShelfLogsForView()
        {
            using (var db = new DbAccess()) {
                string sql = $"SELECT * , TIMESTAMPDIFF(SECOND, dlm_outdate ,dlm_indate) DiffInSeconds FROM {RfidShelfLogTableName}"
                        + $" ORDER BY {RfidShelfLogDDateColName}, {RfidShelfLogTagNameColName} ,{ RfidShelfLogDDateColName } , {RfidShelfLogCntColName} ";

                DataTable dt = db.Query(sql);

                List<RfidShelfLogTable> items = dt.AsEnumerable().Select(item =>
                new RfidShelfLogTable
                {
                    Id = item.FieldOrDefaultValue<int>(RfidShelfLogIdColName),
                    DDate = item.FieldOrDefaultValue<DateTime>(RfidShelfLogDDateColName),
                    Rfid = item.FieldOrDefaultValue<string>(RfidShelfLogTagNameColName),
                    Cnt = item.FieldOrDefaultValue<int>(RfidShelfLogCntColName),
                    OutTime = item.FieldOrDefaultValue<DateTime>(RfidShelfLogOutTimeColName),
                    InTime = item.FieldOrDefaultValue<DateTime>(RfidShelfLogInTimeColName),
                    DiffInSeconds = item.FieldOrDefaultValue<long>("DiffInSeconds")

                }).ToList();


                return items;
            }
                
        }

        public List<RfidShelfLogTable> GetLatestRfidShelfLogs()
        {
            using (var db = new DbAccess())
            {
                //string sql = $" SELECT dlm.* FROM {RfidShelfLogTableName} dlm WHERE {RfidShelfLogCntColName} = ( SELECT MAX(dlm2.{RfidShelfLogCntColName}) FROM {RfidShelfLogTableName} dlm2 WHERE dlm2.{RfidShelfLogTagNameColName} = dlm.{RfidShelfLogTagNameColName} ) "
                //        + $" ORDER BY dlm.{RfidShelfLogDDateColName}, dlm.{RfidShelfLogTagNameColName} ,dlm.{ RfidShelfLogDDateColName } , dlm.{RfidShelfLogCntColName} ";

                string sql = @"SELECT dlm.*
                        FROM drfid_log_move dlm
                        INNER JOIN (
	                        SELECT dlm2.dlm_rfid_cd
		                        ,MAX(dlm2.dlm_cnt) dlm_cnt
	                        FROM drfid_log_move dlm2
	                        GROUP BY dlm2.dlm_rfid_cd
	                        ) dmax ON dlm.dlm_rfid_cd = dmax.dlm_rfid_cd
	                        AND dlm.dlm_cnt = dmax.dlm_cnt
                        ORDER BY dlm.dlm_date
	                        ,dlm.dlm_rfid_cd
	                        ,dlm.dlm_date
	                        ,dlm.dlm_cnt
                        ";

                DataTable dt = db.Query(sql);

                List<RfidShelfLogTable> items = dt.AsEnumerable().Select(item =>
                new RfidShelfLogTable
                {
                    Id = item.FieldOrDefaultValue<int>(RfidShelfLogIdColName),
                    DDate = item.FieldOrDefaultValue<DateTime>(RfidShelfLogDDateColName),
                    Rfid = item.FieldOrDefaultValue<string>(RfidShelfLogTagNameColName),
                    Cnt = item.FieldOrDefaultValue<int>(RfidShelfLogCntColName),
                    OutTime = item.FieldOrDefaultValue<DateTime>(RfidShelfLogOutTimeColName),
                    InTime = item.FieldOrDefaultValue<DateTime>(RfidShelfLogInTimeColName)

                }).ToList();

                foreach (var item in items)
                {
                    //var combined = string.Join(", ", item);
                    logger.Info($"Get latest log record {item.Rfid} / Cnt = { item.Cnt} ");
                }

                return items;
            }
        }


        /// <summary>
        /// Extract item from shelf
        /// </summary>
        /// <param name="tagName">RFID情報(RFID info)</param>
        /// <returns></returns>
        public int PopItem(string tagName)
        {
            int result = 0;
            // get latest record of log table
            var latestItem = GetLatestRecordRfidShelfLogByRfid(tagName);

            if (latestItem == null || string.IsNullOrEmpty(latestItem?.Rfid))
            {
                //if do not exist record => create new record with 回数 = 1 
                latestItem.Cnt = 0;
                latestItem.Rfid = tagName;
            }
            else
            {
                //Check 抜き取り時間 and 戻し時間
                if (!string.IsNullOrEmpty(latestItem.Rfid)
                    && (latestItem?.InTime == default(DateTime) || latestItem?.InTime == null)
                     && (latestItem?.OutTime != default(DateTime) || latestItem?.OutTime != null)
                  )
                {
                    //tag already out of shelf : no need update or insert record
                    return result;
                }

                //if existed record
                latestItem.Cnt = latestItem.Cnt + 1;

            }
            logger.Info($"PopItem insert {latestItem.Rfid} with {latestItem.Cnt}");
            //insert new record with extract item action
            result = InsertRfidShelfLogTable(latestItem, ShelfRfidAction.ShelfOutAction);

            return result;
        }

        /// <summary>
        /// Return item to shelf
        /// </summary>
        /// <param name="tagName">RFID情報(RFID info)</param>
        /// <returns></returns>
        public int PutItem(string tagName)
        {
            int result = 0;
            // get latest record of log table
            //var latestItem = GetLatestRecordRfidShelfLogByRfid(tagName);
            //var latestItem = GetLatestRecordRfidShelfLogByRfidInLocal(RfidScannerService.RfidShelfLogTableLatest, tagName);
            var latestItem = GetLatestRecordRfidShelfLogByRfid(tagName);

            if (latestItem == null || string.IsNullOrEmpty(latestItem?.Rfid))
            {
                //init record with 回数 = 0
                result = InitShelfLogRecordByRfid(tagName);
                logger.Info($"PutItem init record {latestItem.Rfid} with {latestItem.Cnt}");
            }
            else
            {
                //if existed record => update if 戻し時間 is null
                if (latestItem.InTime == default(DateTime) || latestItem.InTime ==null)
                {
                    result = UpdateRfidShelfLogTable(latestItem, ShelfRfidAction.ShelfInAction);
                    logger.Info($"PutItem Update {latestItem.Rfid} with {latestItem.Cnt}");
                }

            }
            
            return result;

        }
        /// <summary>
        /// Create new init record with 回数 = 0 
        /// データ発生日
        ///・RFID情報(RFID info)
        ///・回数
        ///・抜き取り時間
        ///・戻し時間
        /// </summary>
        /// <param name="tagName">RFID情報(RFID info)</param>
        public int InitShelfLogRecordByRfid(string tagName)
        {
            using (var db = new DbAccess())
            {
                db.Bind(new string[] { "tag", tagName, "cnt", "0" });

                int created = db.NonQuery($"INSERT INTO {RfidShelfLogTableName} ({RfidShelfLogDDateColName}, {RfidShelfLogTagNameColName}, {RfidShelfLogCntColName}, {RfidShelfLogOutTimeColName}, {RfidShelfLogInTimeColName}) VALUES( CURRENT_DATE(),@tag,@cnt,null,CURRENT_TIMESTAMP())");

                return created;
            }
        }

        /// <summary>
        /// Get latest record by rfid
        /// </summary>
        /// <param name="tagName">RFID情報(RFID info)</param>
        /// <returns></returns>
        public RfidShelfLogTable GetLatestRecordRfidShelfLogByRfid(string tagName)
        {
            RfidShelfLogTable result = new RfidShelfLogTable();

            switch (ConfigFile.CheckLatestShelfLogMethods)
            {
                case CheckLatestShelfLogMethods.CheckInLocal:
                    result = GetLatestRecordRfidShelfLogByRfidInLocal(RfidScannerService.RfidShelfLogTableLatest, tagName);
                    break;

                case CheckLatestShelfLogMethods.CheckInDatabase:
                    result = GetLatestRecordRfidShelfLogByRfidInDb(tagName);
                    break;
            }

            return result;
        }

        /// <summary>
        /// Get latest record by rfid
        /// </summary>
        /// <param name="tagName">RFID情報(RFID info)</param>
        /// <returns></returns>
        public RfidShelfLogTable GetLatestRecordRfidShelfLogByRfidInDb(string tagName)
        {
            using (var db = new DbAccess())
            {
                RfidShelfLogTable rfidShelfLog = new RfidShelfLogTable();
                string sql = "SELECT * FROM " + RfidShelfLogTableName
                            + " WHERE " + RfidShelfLogTagNameColName + "= @tagName"
                            + " ORDER BY " + RfidShelfLogCntColName + " DESC LIMIT 1 ";

                DataTable dt = db.Query(sql, new string[] { "tagName", tagName });

                IList<RfidShelfLogTable> items = dt.AsEnumerable().Select(item =>
                new RfidShelfLogTable
                {
                    DDate = item.FieldOrDefaultValue<DateTime>(RfidShelfLogDDateColName),
                    Rfid = item.FieldOrDefaultValue<string>(RfidShelfLogTagNameColName),
                    Cnt = item.FieldOrDefaultValue<int>(RfidShelfLogCntColName),
                    OutTime = item.FieldOrDefaultValue<DateTime>(RfidShelfLogOutTimeColName),
                    InTime = item.FieldOrDefaultValue<DateTime>(RfidShelfLogInTimeColName)
                }).ToList();

                if (items.Count > 0)
                    rfidShelfLog = items[0];

                return rfidShelfLog;
            }
        }

        /// <summary>
        /// Get latest record by rfid
        /// </summary>
        /// <param name="tagName">RFID情報(RFID info)</param>
        /// <returns></returns>
        public RfidShelfLogTable GetLatestRecordRfidShelfLogByRfidInLocal(List<RfidShelfLogTable> latestTagLogs  , string tagName)
        {
            var  rfidShelfLogTable =  latestTagLogs.Find(x => x.Rfid.ToLower() == tagName.ToLower());
            if (rfidShelfLogTable == null)
                rfidShelfLogTable = new RfidShelfLogTable();

            return rfidShelfLogTable;
        }

        /// <summary>
        /// Insert to shelf log table
        /// </summary>
        /// <param name="rfidShelfLog"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int InsertRfidShelfLogTable(RfidShelfLogTable rfidShelfLog, ShelfRfidAction action)
        {

            int result = 0;
            using (var db = new DbAccess())
            {
                switch (action)
                {
                    case ShelfRfidAction.ShelfInAction:
                        //dbAccess.bind(new string[] { "ddate", "CURRENT_DATE()", "tag", rfidShelfLog.Rfid, "cnt", rfidShelfLog.Cnt.ToString(), "out", "", "in", "CURRENT_TIME()" });
                        //result = dbAccess.nQuery($"INSERT INTO `{RfidShelfLogTableName}` (`{RfidShelfLogDDateColName}`, `{RfidShelfLogTagNameColName}`, `{RfidShelfLogCntColName}`, `{RfidShelfLogOutTimeColName}`, `{RfidShelfLogInTimeColName}`) "
                        //            + " VALUES(@ddate,@tag,@cnt,@out,@in)");
                        break;

                    case ShelfRfidAction.ShelfOutAction:
                        db.Bind(new string[] { "tag", rfidShelfLog.Rfid, "cnt", rfidShelfLog.Cnt.ToString(), "in", "NULL" });
                        result = db.NonQuery($"INSERT INTO `{RfidShelfLogTableName}` (`{RfidShelfLogDDateColName}`, `{RfidShelfLogTagNameColName}`, `{RfidShelfLogCntColName}`, `{RfidShelfLogOutTimeColName}`, `{RfidShelfLogInTimeColName}`) "
                                    + " VALUES(CURRENT_DATE(),@tag,@cnt,CURRENT_TIMESTAMP(),NULL)");
                        break;
                }
            }
            return result;
        }

        /// <summary>
        /// Update to log
        /// </summary>
        /// <param name="rfidShelfLog"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public int UpdateRfidShelfLogTable(RfidShelfLogTable rfidShelfLog , ShelfRfidAction action)
        {
            int result = 0;
            //RfidShelfLogTable latestRecordByTag = GetLatestRecordRfidShelfLogByRfid(rfidShelfLog.Rfid);

            using (var db = new DbAccess())
            {
                db.Bind(new string[] { "ddate", rfidShelfLog.DDate.ToString(), "tag", rfidShelfLog.Rfid, "cnt", rfidShelfLog.Cnt.ToString() });
                switch (action)
                {
                    case ShelfRfidAction.ShelfInAction:
                        result = db.NonQuery($"UPDATE {RfidShelfLogTableName} SET {RfidShelfLogInTimeColName}=CURRENT_TIMESTAMP() WHERE {RfidShelfLogDDateColName} = @ddate AND {RfidShelfLogTagNameColName}=@tag AND  {RfidShelfLogCntColName}=@cnt ");
                        break;

                    case ShelfRfidAction.ShelfOutAction:
                        result = db.NonQuery($"UPDATE {RfidShelfLogTableName} SET {RfidShelfLogOutTimeColName}=CURRENT_TIMESTAMP() WHERE {RfidShelfLogDDateColName} = @ddate AND {RfidShelfLogTagNameColName}=@tag AND  {RfidShelfLogCntColName}=@cnt ");
                        break;
                }
            }
            return result;
        }

        //public int InsertRfidsShelfLogTable(string sqlInsertBatch)
        //{
        //    int result = 0;
        //    using (var db = new DbAccess())
        //    {
        //        result = db.NonQuery(sqlInsertBatch);
        //    }
        //    return result;
        //}

        //public int UpdateRfidsShelfLogTable(string sqlUpdateBatch)
        //{
        //    int result = 0;
        //    using (var db = new DbAccess())
        //    {
        //        result = db.NonQuery(sqlUpdateBatch);
        //    }
        //    return result;
        //}

        public int  NonQueryExecRfidsShelfLogTable(string sql)
        {
            int result = 0;
            using (var db = new DbAccess())
            {
                logger.Info($" NonQueryExecRfidsShelfLogTable >> {sql}");
                result = db.NonQuery(sql);
            }
            return result;
        }

        public int NonQueryExec(string sql)
        {
            int result = 0;
            using (var db = new DbAccess())
            {
                logger.Info($" NonQueryExec >> {sql}");
                result = db.NonQuery(sql);
            }
            return result;
        }
        public DbQueryStatement PopItemGetSql(string tagName)
        {
            DbQueryStatement result = new DbQueryStatement();
            // get latest record of log table
            var latestItem = GetLatestRecordRfidShelfLogByRfid(tagName);

            if (latestItem == null || string.IsNullOrEmpty(latestItem?.Rfid))
            {
                //if do not exist record => create new record with 回数 = 1 
                latestItem.Cnt = 0;
                latestItem.Rfid = tagName;
            }
            else
            {
                //Check 抜き取り時間 and 戻し時間
                if (!string.IsNullOrEmpty(latestItem.Rfid)
                    && (latestItem?.InTime == default(DateTime) || latestItem?.InTime == null)
                     && (latestItem?.OutTime != default(DateTime) || latestItem?.OutTime != null)
                  )
                {
                    //tag already out of shelf : no need update or insert record
                    return result;
                }

                //if existed record
                latestItem.Cnt = latestItem.Cnt + 1;

            }

            //insert new record with extract item action
            //result = InsertRfidShelfLogTable(latestItem, ShelfRfidAction.ShelfOutAction);

            result.DbQueryType = DbQueryType.InsertQuery;
            result.QueryStr = InsertRfidShelfLogTableGetSql(latestItem, ShelfRfidAction.ShelfOutAction);

            logger.Info($"PopItem insert {latestItem.Rfid} with {latestItem.Cnt} : {result}");

            //generate sql

            return result;
        }

        public DbQueryStatement PutItemGetSql(string tagName)
        {
            //string result = "";
            DbQueryStatement result = new DbQueryStatement();
            // get latest record of log table
            //var latestItem = GetLatestRecordRfidShelfLogByRfid(tagName);
            //var latestItem = GetLatestRecordRfidShelfLogByRfidInLocal(RfidScannerService.RfidShelfLogTableLatest, tagName);
            var latestItem = GetLatestRecordRfidShelfLogByRfid(tagName);

            if (latestItem == null || string.IsNullOrEmpty(latestItem?.Rfid))
            {
                //init record with 回数 = 0
                //result = InitShelfLogRecordByRfid(tagName);
                result.DbQueryType = DbQueryType.InsertQuery;
                result.QueryStr = InitShelfLogRecordByRfidGetSql(tagName);
                logger.Info($"PutItem init record {latestItem.Rfid} with {latestItem.Cnt} : {result}");
            }
            else
            {
                //if existed record => update if 戻し時間 is null
                if (latestItem.InTime == default(DateTime) || latestItem.InTime == null)
                {
                    //result = UpdateRfidShelfLogTable(latestItem, ShelfRfidAction.ShelfInAction);
                    result.DbQueryType = DbQueryType.UpdateQuery;
                    result.QueryStr = UpdateRfidShelfLogTableGetSql(latestItem, ShelfRfidAction.ShelfInAction);
                    logger.Info($"PutItem Update {latestItem.Rfid} with {latestItem.Cnt} : {result}");
                }

            }

            return result;
        }

        public string InitShelfLogRecordByRfidGetSql(string tagName)
        {
            string result = "";
            //result = $"INSERT INTO {RfidShelfLogTableName} ({RfidShelfLogDDateColName}, {RfidShelfLogTagNameColName}, {RfidShelfLogCntColName}, {RfidShelfLogOutTimeColName}, {RfidShelfLogInTimeColName}) VALUES( CURRENT_DATE(),'{tagName}',0,null,CURRENT_TIMESTAMP());";
            result = $"( CURRENT_DATE(),'{tagName}',0,null,CURRENT_TIMESTAMP())";

            return result;
        }

        public string InsertRfidShelfLogTableGetSql(RfidShelfLogTable rfidShelfLog, ShelfRfidAction action )
        {
            string result = "";
            //result += $"INSERT INTO `{RfidShelfLogTableName}` (`{RfidShelfLogDDateColName}`, `{RfidShelfLogTagNameColName}`, `{RfidShelfLogCntColName}`, `{RfidShelfLogOutTimeColName}`, `{RfidShelfLogInTimeColName}`) "
            //                        + $" VALUES(CURRENT_DATE(),'{rfidShelfLog.Rfid}', {rfidShelfLog.Cnt},CURRENT_TIMESTAMP(),NULL);";
            result += $"(CURRENT_DATE(),'{rfidShelfLog.Rfid}', {rfidShelfLog.Cnt},CURRENT_TIMESTAMP(),NULL)";
            return result;
        }

        public string UpdateRfidShelfLogTableGetSql(RfidShelfLogTable rfidShelfLog, ShelfRfidAction action)
        {
            string result = "";
            //RfidShelfLogTable latestRecordByTag = GetLatestRecordRfidShelfLogByRfid(rfidShelfLog.Rfid);
            switch (action)
            {
                case ShelfRfidAction.ShelfInAction:
                    //result = $"UPDATE {RfidShelfLogTableName} SET {RfidShelfLogInTimeColName}=CURRENT_TIMESTAMP() WHERE {RfidShelfLogDDateColName} = '{rfidShelfLog.DDate}' AND {RfidShelfLogTagNameColName}='{rfidShelfLog.Rfid}' AND  {RfidShelfLogCntColName}={rfidShelfLog.Cnt};";
                    result = $"UPDATE {RfidShelfLogTableName} SET {RfidShelfLogInTimeColName}=CURRENT_TIMESTAMP() WHERE {RfidShelfLogIdColName} = {rfidShelfLog.Id};";
                    break;

                case ShelfRfidAction.ShelfOutAction:
                    //result = $"UPDATE {RfidShelfLogTableName} SET {RfidShelfLogOutTimeColName}=CURRENT_TIMESTAMP() WHERE {RfidShelfLogDDateColName} =  '{rfidShelfLog.DDate}' AND {RfidShelfLogTagNameColName}='{rfidShelfLog.Rfid}' AND  {RfidShelfLogCntColName}={rfidShelfLog.Cnt};";
                    result = $"UPDATE {RfidShelfLogTableName} SET {RfidShelfLogOutTimeColName}=CURRENT_TIMESTAMP() WHERE {RfidShelfLogIdColName} =  {rfidShelfLog.Id};";
                    break;
            }
            return result;
        }
    }
}
