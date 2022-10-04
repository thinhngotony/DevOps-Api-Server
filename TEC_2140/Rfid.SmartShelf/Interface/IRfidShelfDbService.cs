using System.Collections.Generic;
using Vjp.Rfid.SmartShelf.Enums;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Interface
{
    public interface IRfidShelfDbService
    {
        List<RfidShelfLogTable> GetRfidShelfLogs();

        List<RfidShelfLogTable> GetRfidShelfLogsForView();

        List<RfidShelfLogTable> GetLatestRfidShelfLogs();

        int PopItem(string tagName);
        int PutItem(string tagName);
        int InitShelfLogRecordByRfid(string tagName);

        RfidShelfLogTable GetLatestRecordRfidShelfLogByRfid(string tagName);
        RfidShelfLogTable GetLatestRecordRfidShelfLogByRfidInDb(string tagName);        
        RfidShelfLogTable GetLatestRecordRfidShelfLogByRfidInLocal(List<RfidShelfLogTable> latestTagLogs, string tagName);

        int InsertRfidShelfLogTable(RfidShelfLogTable rfidShelfLog, ShelfRfidAction action);
        int UpdateRfidShelfLogTable(RfidShelfLogTable rfidShelfLog, ShelfRfidAction action);

        DbQueryStatement PopItemGetSql(string tagName);
        DbQueryStatement PutItemGetSql(string tagName);
        string InitShelfLogRecordByRfidGetSql(string tagName);

        string InsertRfidShelfLogTableGetSql(RfidShelfLogTable rfidShelfLog, ShelfRfidAction action);
        string UpdateRfidShelfLogTableGetSql(RfidShelfLogTable rfidShelfLog, ShelfRfidAction action);

        int NonQueryExecRfidsShelfLogTable(string sql);

        int NonQueryExec(string sql);
        //int InsertRfidsShelfLogTable(string sqlInsertBatch);
        //int UpdateRfidsShelfLogTable(string sqlUpdateBatch);
    }
}
