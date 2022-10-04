using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Vjp.Rfid.SmartShelf.Db;
using Vjp.Rfid.SmartShelf.Helper;
using Vjp.Rfid.SmartShelf.Interface;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Services
{
    public class RfidShelfProductService : IRfidShelfProductService
    {
        
        //private DbAccess dbAccess;
        //private IDbAccess dbAccess;
        private readonly string RfidShelfProductTableName = "drfid_product_pos";
        private readonly string ShelfNoColName = "dpp_shelf_pos";
        private readonly string ShelfColIndexColName = "dpp_shelf_col_pos";
        private readonly string RfidColName = "dpp_rfid_cd";
        private readonly string IsbnCodeColName = "dpp_isbn";
        private readonly string ProductNameColName = "dpp_product_name";
        private readonly string ScannerNameColName = "dpp_scaner_name";
        private readonly string ShelfNameColName = "dpp_shelf_name";
        private readonly string JanCdColName = "dpp_jan_cd";
        private static log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static List<RfidShelfProduct> unImportRfidShelfProducts = new List<RfidShelfProduct>();
        public RfidShelfProductService()
        {
            //dbAccess = Program.GetService<IDbAccess>();
            //dbAccess = new DbAccess();
        }

        //Get shelf product list ( product list master )
        public List<RfidShelfProduct> GetRfidShelfProducts()
        {
            List<RfidShelfProduct> rfidShelfProducts = new List<RfidShelfProduct>();
            try
            {
                using (var db = new DbAccess())
                {
                    //string sql = $"SELECT * FROM {RfidShelfProductTableName} ORDER BY {ShelfNoColName} , {ShelfColIndexColName} ";
                    string sql = $"SELECT * FROM {RfidShelfProductTableName} WHERE {RfidColName} <> '' ORDER BY {ShelfNoColName} , {ShelfColIndexColName} ";
                    DataTable dt = db.Query(sql);

                    rfidShelfProducts = dt.AsEnumerable().Select(item =>
                    new RfidShelfProduct
                    {
                        ShelfNo = item.FieldOrDefaultValue<int>(ShelfNoColName),
                        ShelfColIndex = item.FieldOrDefaultValue<int>(ShelfColIndexColName),
                        IsbnCode = item.FieldOrDefaultValue<string>(IsbnCodeColName),
                        Rfid = item.FieldOrDefaultValue<string>(RfidColName),
                        ProductName = item.FieldOrDefaultValue<string>(ProductNameColName),
                        ScannerName = item.FieldOrDefaultValue<string>(ScannerNameColName),
                        ShelfName = item.FieldOrDefaultValue<string>(ShelfNameColName),
                        JanCd = item.FieldOrDefaultValue<string>(JanCdColName),
                    }).ToList();
                }
            }
            catch(Exception ex)
            {
                logger.Error($"Get shelf product list .{ex.Message}");
            }
            
            return rfidShelfProducts;
        }

        public List<RfidShelfProduct> GetRfidShelfProductsByDeviceName(string scannerName , string shelfName)
        {
            List<RfidShelfProduct> rfidShelfProducts = new List<RfidShelfProduct>();

            try
            {
                using (var db = new DbAccess())
                {
                    string sql = $"SELECT * FROM {RfidShelfProductTableName} WHERE {ScannerNameColName} = {scannerName} AND {ShelfNameColName} = {shelfName}  ORDER BY {ShelfNoColName} , {ShelfColIndexColName} ";

                    DataTable dt = db.Query(sql);

                    rfidShelfProducts = dt.AsEnumerable().Select(item =>
                    new RfidShelfProduct
                    {
                        ShelfNo = item.FieldOrDefaultValue<int>(ShelfNoColName),
                        ShelfColIndex = item.FieldOrDefaultValue<int>(ShelfColIndexColName),
                        IsbnCode = item.FieldOrDefaultValue<string>(IsbnCodeColName),
                        Rfid = item.FieldOrDefaultValue<string>(RfidColName),
                        ProductName = item.FieldOrDefaultValue<string>(ProductNameColName),
                        ScannerName = item.FieldOrDefaultValue<string>(ScannerNameColName),
                         ShelfName = item.FieldOrDefaultValue<string>(ShelfNameColName),
                        JanCd = item.FieldOrDefaultValue<string>(JanCdColName),
                    }).ToList();
                }
            }
            catch(Exception ex)
            {
                logger.Error($"Get shelf product list by device name {scannerName} .{ex.Message}");
            }
            return rfidShelfProducts;
        }

        public List<RfidShelfProduct> GetRfidShelfProductsByShelfNo(string shelfNo)
        {
            List<RfidShelfProduct> rfidShelfProducts = new List<RfidShelfProduct>();

            using (var db = new DbAccess())
            {
                string sql = $"SELECT * FROM {RfidShelfProductTableName} WHERE {ShelfNoColName} = {shelfNo} ORDER BY {ShelfNoColName} , {ShelfColIndexColName} ";

                DataTable dt = db.Query(sql);

                rfidShelfProducts = dt.AsEnumerable().Select(item =>
                new RfidShelfProduct
                {
                    ShelfNo = item.FieldOrDefaultValue<int>(ShelfNoColName),
                    ShelfColIndex = item.FieldOrDefaultValue<int>(ShelfColIndexColName),
                    IsbnCode = item.FieldOrDefaultValue<string>(IsbnCodeColName),
                    Rfid = item.FieldOrDefaultValue<string>(RfidColName),
                    ProductName = item.FieldOrDefaultValue<string>(ProductNameColName),
                    ScannerName = item.FieldOrDefaultValue<string>(ScannerNameColName),
                    ShelfName = item.FieldOrDefaultValue<string>(ShelfNameColName),
                    JanCd = item.FieldOrDefaultValue<string>(JanCdColName)
                }).ToList();
            }
            return rfidShelfProducts;
        }

        public int InsertShelftProductToDb(List<RfidShelfProduct> rfidShelfProducts)
        {

            int result = 0;
            int created = 0;
            try
            {
                unImportRfidShelfProducts.Clear();
                using (var db = new DbAccess())
                {
                    foreach (var item in rfidShelfProducts)
                    {
                        db.Bind(new string[] { "shelfno", item.ShelfNo.ToString(), "ShelfColIndex", item.ShelfColIndex.ToString(), "Rfid", item.Rfid, "IsbnCode", item.IsbnCode, "ProductName", item.ProductName, "ScannerName", item.ScannerName, "ShelfName", item.ShelfName });
                        // delete if existed record 
                        //int deleted = dbAccess.NonQuery($"DELETE FROM {RfidShelfProductTableName} WHERE {ShelfNoColName} = @shelfno AND {ShelfColIndexColName} = @ShelfColIndex AND {RfidColName} = @Rfid AND {IsbnCodeColName} = @IsbnCode");
                        //int deleted = db.NonQuery($"DELETE FROM {RfidShelfProductTableName} WHERE {RfidColName} = @Rfid AND {ShelfNameColName}=@ShelfName");

                        var selectResult = db.Query($"SELECT * FROM {RfidShelfProductTableName} WHERE {RfidColName} = @Rfid ");
                        logger.Info($"InsertShelftProductToDb sql = SELECT * FROM {RfidShelfProductTableName} WHERE {RfidColName} = @Rfid ");
                        if (selectResult.Rows.Count == 0)
                        {
                            //insert record
                            db.Bind(new string[] { "shelfno", item.ShelfNo.ToString(), "ShelfColIndex", item.ShelfColIndex.ToString(), "JanCd", item.JanCd, "Rfid", item.Rfid, "IsbnCode", item.IsbnCode, "ProductName", item.ProductName, "ScannerName", item.ScannerName, "ShelfName", item.ShelfName });
                            created = db.NonQuery($"INSERT INTO {RfidShelfProductTableName} ({ShelfNoColName} ,{ShelfColIndexColName},{JanCdColName},{RfidColName},{IsbnCodeColName},{ProductNameColName},{ScannerNameColName},{ShelfNameColName}) VALUES( @shelfno,@ShelfColIndex ,@JanCd,@Rfid ,@IsbnCode,@ProductName,@ScannerName,@ShelfName)");

                        }
                        else
                        {
                            unImportRfidShelfProducts.Add(item);
                        }

                        result += created;

                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error($"Import csv error.{ex.Message}");
            }
            
            return result;

        }
  
        public int InsertShelftProductToDb(List<RfidShelfProduct> rfidShelfProducts, bool deleteIfExisted)
        {

            int result = 0;
            try
            {
                using (var db = new DbAccess())
                {
                    foreach (var item in rfidShelfProducts)
                    {
                        int changeRecords = 0 ;
                        db.Bind(new string[] { "shelfno", item.ShelfNo.ToString(), "ShelfColIndex", item.ShelfColIndex.ToString(), "Rfid", item.Rfid, "IsbnCode", item.IsbnCode, "ProductName", item.ProductName, "ScannerName", item.ScannerName, "ShelfName", item.ShelfName });
                        // delete if existed record 
                        //int deleted = dbAccess.NonQuery($"DELETE FROM {RfidShelfProductTableName} WHERE {ShelfNoColName} = @shelfno AND {ShelfColIndexColName} = @ShelfColIndex AND {RfidColName} = @Rfid AND {IsbnCodeColName} = @IsbnCode");
                        //check existed record 

                        //find by rfid and shelf name
                        var selectResult = db.Query($"SELECT * FROM {RfidShelfProductTableName} WHERE {RfidColName} = @Rfid AND {ShelfNameColName}=@ShelfName");
                        logger.Info($"InsertShelftProductToDb sql = SELECT * FROM {RfidShelfProductTableName} WHERE {RfidColName} = @Rfid AND {ShelfNameColName}=@ShelfName");
                        if(selectResult.Rows.Count > 0)
                        {
                            if (deleteIfExisted)
                            {
                                int deleted = db.NonQuery($"DELETE FROM {RfidShelfProductTableName} WHERE {RfidColName} = @Rfid AND {ShelfNameColName}=@ShelfName");
                                //insert record
                                db.Bind(new string[] { "shelfno", item.ShelfNo.ToString(), "ShelfColIndex", item.ShelfColIndex.ToString(), "JanCd", item.JanCd, "Rfid", item.Rfid, "IsbnCode", item.IsbnCode, "ProductName", item.ProductName, "ScannerName", item.ScannerName, "ShelfName", item.ShelfName });
                                changeRecords = db.NonQuery($"INSERT INTO {RfidShelfProductTableName} ({ShelfNoColName} ,{ShelfColIndexColName},{JanCdColName},{RfidColName},{IsbnCodeColName},{ProductNameColName},{ScannerNameColName},{ShelfNameColName}) VALUES( @shelfno,@ShelfColIndex,@JanCd ,@Rfid ,@IsbnCode,@ProductName,@ScannerName,@ShelfName)");

                                logger.Info($"InsertShelftProductToDb sql = INSERT INTO {RfidShelfProductTableName} ({ShelfNoColName} ,{ShelfColIndexColName},{JanCdColName},{RfidColName},{IsbnCodeColName},{ProductNameColName},{ScannerNameColName},{ShelfNameColName}) VALUES( @shelfno,@ShelfColIndex,@JanCd ,@Rfid ,@IsbnCode,@ProductName,@ScannerName,@ShelfName)");
                            }
                            else
                            {
                                //update some information
                                //db.Bind(new string[] { "shelfno", item.ShelfNo.ToString(), "ShelfColIndex", item.ShelfColIndex.ToString(), "Rfid", item.Rfid, "IsbnCode", item.IsbnCode, "ProductName", item.ProductName, "ScannerName", item.ScannerName, "ShelfName", item.ShelfName });
                                //changeRecords = db.NonQuery($"UPDATE {RfidShelfProductTableName} SET {ProductNameColName} = @ProductName , {ShelfNameColName} =@ShelfName WHERE {RfidColName}=@Rfid  AND {ShelfNameColName}=@ShelfName");
                                //logger.Debug($"InsertShelftProductToDb sql = UPDATE {RfidShelfProductTableName} SET {ProductNameColName} = @ProductName , {ShelfNameColName} =@ShelfName WHERE {RfidColName}=@Rfid  AND {ShelfNameColName}=@ShelfName");

                            }
                        }
                        else
                        {
                            //insert record
                            //db.Bind(new string[] { "shelfno", item.ShelfNo.ToString(), "ShelfColIndex", item.ShelfColIndex.ToString(), "Rfid", item.Rfid, "IsbnCode", item.IsbnCode, "ProductName", item.ProductName, "ScannerName", item.ScannerName, "ShelfName", item.ShelfName });
                            db.Bind(new string[] { "shelfno", item.ShelfNo.ToString(), "ShelfColIndex", item.ShelfColIndex.ToString(), "JanCd", item.JanCd, "Rfid", item.Rfid, "IsbnCode", item.IsbnCode, "ProductName", item.ProductName, "ScannerName", item.ScannerName, "ShelfName", item.ShelfName });
                            //changeRecords = db.NonQuery($"INSERT INTO {RfidShelfProductTableName} ({ShelfNoColName} ,{ShelfColIndexColName},{RfidColName},{IsbnCodeColName},{ProductNameColName},{ScannerNameColName},{ShelfNameColName}) VALUES( @shelfno,@ShelfColIndex ,@Rfid ,@IsbnCode,@ProductName,@ScannerName,@ShelfName)");
                            changeRecords = db.NonQuery($"INSERT INTO {RfidShelfProductTableName} ({ShelfNoColName} ,{ShelfColIndexColName},{JanCdColName},{RfidColName},{IsbnCodeColName},{ProductNameColName},{ScannerNameColName},{ShelfNameColName}) VALUES( @shelfno,@ShelfColIndex,@JanCd ,@Rfid ,@IsbnCode,@ProductName,@ScannerName,@ShelfName)");

                            logger.Info($"InsertShelftProductToDb sql = INSERT INTO {RfidShelfProductTableName} ({ShelfNoColName} ,{ShelfColIndexColName},{JanCdColName},{RfidColName},{IsbnCodeColName},{ProductNameColName},{ScannerNameColName},{ShelfNameColName}) VALUES( @shelfno,@ShelfColIndex,@JanCd ,@Rfid ,@IsbnCode,@ProductName,@ScannerName,@ShelfName)");

                        }

                        result += changeRecords;

                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Append pos master data error.{ex.Message}");                
            }

            return result;

        }

        public RfidShelfProduct[] ReadShelftProductCsv(string pathToFile)
        {
            var result = File.ReadAllLines(pathToFile)
                .Select(line => line.Split(','))
                .Select(item => new RfidShelfProduct
                {
                    //棚番号、列番号、RFIDタグ情報、ISBNコード、商品名
                    ShelfNo = Int32.Parse(item[0]),
                    ShelfColIndex = Int32.Parse(item[1]),
                    Rfid = item[2],
                    IsbnCode = item[3],
                    ProductName = item[4],
                    ScannerName = item[5],
                    ShelfName = item[6]
                    
                })
                .ToArray();

            //return list
            return result;
        }

        public List<RfidShelfProduct> ReadShelftProductCsvUseCsvHelper(string pathToFile)
        {
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                NewLine = Environment.NewLine,
                Delimiter = ",",
                HasHeaderRecord = true,
                Encoding = System.Text.Encoding.GetEncoding(932) 
            };

            var records = new List<RfidShelfProduct>();

            try {
                using (var reader = new StreamReader(pathToFile, Encoding.GetEncoding(932)))
                using (var csv = new CsvReader(reader, config))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        var record = new RfidShelfProduct
                        {
                            ShelfNo = csv.GetField<int>(ShelfNoColName),
                            ShelfColIndex = csv.GetField<int>(ShelfColIndexColName),
                            Rfid = csv.GetField(RfidColName),
                            IsbnCode = csv.GetField(IsbnCodeColName),
                            ProductName = csv.GetField(ProductNameColName),
                            ScannerName = csv.GetField(ScannerNameColName),
                            ShelfName = csv.GetField(ShelfNameColName),
                            JanCd = csv.GetField(JanCdColName)
                        };
                        records.Add(record);
                    }
                }

            }
            catch (Exception ex)
            {
                logger.Error($"Read csv error.{ex.Message}");
            }

            return records;
        }

        public List<RfidShelfProduct> GetUnImportRfidShelfProducts()
        {
            return unImportRfidShelfProducts;
        }
    }
}
