using log4net.Config;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Vjp.Rfid.SmartShelf.Constants;
using Vjp.Rfid.SmartShelf.Db;
using Vjp.Rfid.SmartShelf.Enums;
using Vjp.Rfid.SmartShelf.Helper;
using Vjp.Rfid.SmartShelf.Interface;
using Vjp.Rfid.SmartShelf.Models;
using Vjp.Rfid.SmartShelf.Services;

namespace Vjp.Rfid.SmartShelf
{
    static class Program
    {
        public static IServiceProvider ServiceProvider { get; set; }
        private static IniFile iniFile = new IniFile("Settings.ini");
        private static log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        static void ConfigureServices()
        {
            var services = new ServiceCollection();
            //begin register service 
            services.AddTransient<IDbAccess, DbAccess>();
            services.AddTransient<IRfidShelfProductService, RfidShelfProductService>();
            services.AddTransient<IRfidShelfDbService, RfidShelfDbService>();
            services.AddTransient<IRfidShelfHttpService, RfidShelfHttpService>();

            ServiceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : class
        {
            try
            {
                return (T)ServiceProvider.GetService(typeof(T));
            }
            catch(Exception ex)
            {
                throw;
            }
            
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                //load config file
                XmlConfigurator.ConfigureAndWatch(new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "log4net.config"));

                logger.Info("Read config file");
                //read ini setting file 
                ReadConfigFile();

                logger.Info("Register services");
                //call configuration service
                ConfigureServices();

                Application.Run(new FormMenu());
                //Application.Run(new FrmCheckDbConnection());
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Cannot load form. {Ultil.GetSystemErrorMsg(ex)}" , Application.ProductName , MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private static void ReadConfigFile()
        {
            //ConfigFile.Url = iniFile.Read("Url");

            ConfigFile.RfidDeviceNames = iniFile.Read("RfidDeviceNames").Split(',');
            AppConstants.PosDeviceCnt = ConfigFile.RfidDeviceNames.Length;
            logger.Info("Read config file RfidDeviceNames = " + string.Join(",", ConfigFile.RfidDeviceNames));

            ConfigFile.RfidDeviceNamesMapToShelfNames = iniFile.Read("RfidDeviceNamesMapToShelfNames").Split(',');
            logger.Info("Read config file RfidDeviceNamesMapToShelfNames = " + string.Join(",", ConfigFile.RfidDeviceNamesMapToShelfNames));

            ConfigFile.ApiKey = iniFile.Read("ApiKey");
            //ConfigFile.ShelfNo = iniFile.Read("ShelfNo");

            //ShelfProductAppendMode---------------------
            //ConfigFile.ShelfProductAppendMode = Int16.Parse(iniFile.Read("ShelfProductAppendMode"));
            string shelfProductAppendModeStr = iniFile.Read("ShelfProductAppendMode");
            if (string.IsNullOrEmpty(shelfProductAppendModeStr))
            {
                ConfigFile.ShelfProductAppendMode = 0;
            }
            else
            {
                ConfigFile.ShelfProductAppendMode = Int32.Parse(shelfProductAppendModeStr);
            }
            logger.Info("Read config file ShelfProductAppendMode = " + ConfigFile.ShelfProductAppendMode);

            ConfigFile.DbHost = iniFile.Read("DbHost");
            logger.Info("Read config file DbHost = " + ConfigFile.DbHost);

            ConfigFile.DbName = iniFile.Read("DbName");
            logger.Info("Read config file DbName = " + ConfigFile.DbName);

            ConfigFile.DbUser = iniFile.Read("DbUser");
            logger.Info("Read config file DbUser = " + ConfigFile.DbUser);
            ConfigFile.DbPass = iniFile.Read("DbPass");

            //ReadEmptyTagInterval
            string scannerReadTimerIntervalStr = iniFile.Read("ScannerReadTimerInterval");
            if (string.IsNullOrEmpty(scannerReadTimerIntervalStr))
                scannerReadTimerIntervalStr = AppConstants.PosReadTimerInterval.ToString();

            ConfigFile.ScannerReadTimerInterval = Int32.Parse(scannerReadTimerIntervalStr);
            logger.Info("Read config file ScannerReadTimerInterval = " + ConfigFile.ScannerReadTimerInterval);

            //ReadHasTagsCounter
            string readHasTagsCounterStr = iniFile.Read("ReadHasTagsCounter");
            if (string.IsNullOrEmpty(readHasTagsCounterStr))
                readHasTagsCounterStr = "3";

            ConfigFile.ReadHasTagsCounter = Int32.Parse(readHasTagsCounterStr);
            logger.Info("Read config file ReadHasTagsCounter = " + ConfigFile.ReadHasTagsCounter);

            //ReadEmptyTagCounter
            string readEmptyTagCounterStr = iniFile.Read("ReadEmptyTagCounter");
            if (string.IsNullOrEmpty(readEmptyTagCounterStr))
                readEmptyTagCounterStr = "3";

            ConfigFile.ReadEmptyTagCounter = Int32.Parse(readEmptyTagCounterStr);
            logger.Info("Read config file ReadEmptyTagCounter = " + ConfigFile.ReadEmptyTagCounter);

            //CheckLatestShelfLogMethods
            string checkLatestLogDirectInDbStr = iniFile.Read("CheckLatestShelfLogMethods");
            if (string.IsNullOrEmpty(checkLatestLogDirectInDbStr))
                checkLatestLogDirectInDbStr = ((int)CheckLatestShelfLogMethods.CheckInLocal).ToString()  ;

            ConfigFile.CheckLatestShelfLogMethods = (CheckLatestShelfLogMethods)Int32.Parse(checkLatestLogDirectInDbStr);
            logger.Info("Read config file CheckLatestShelfLogMethods = " + ConfigFile.CheckLatestShelfLogMethods);

            //RealTimeItemShiftNotify
            string realTimeItemShiftNotifyStr = iniFile.Read("RealTimeItemShiftNotify");
            if (string.IsNullOrEmpty(realTimeItemShiftNotifyStr))
                realTimeItemShiftNotifyStr = "0";

            ConfigFile.RealTimeItemShiftNotify = realTimeItemShiftNotifyStr;

            //ItemShiftNotifyUrl
            string itemShiftNotifyUrlStr = iniFile.Read("ItemShiftNotifyUrl");
            if (string.IsNullOrEmpty(itemShiftNotifyUrlStr))
            {
                ConfigFile.RealTimeItemShiftNotify = "0";
                //itemShiftNotifyUrlStr = "http://192.168.1.42:8000";
            }

            ConfigFile.ItemShiftNotifyUrl = itemShiftNotifyUrlStr;

            //InOutItemDoNotRelationWithShelf
            ConfigFile.InOutItemDependOnShelf = iniFile.Read("InOutItemDependOnShelf");
            if (ConfigFile.InOutItemDependOnShelf == "")
                ConfigFile.InOutItemDependOnShelf = "1";

            //RfidToJanUrl
            ConfigFile.RfidToJanUrl = iniFile.Read("RfidToJanUrl");
            logger.Info("Read config file RfidToJanUrl = " + ConfigFile.RfidToJanUrl);

            //JanToItemInfoUrl
            ConfigFile.JanToItemInfoUrl = iniFile.Read("JanToItemInfoUrl");
            logger.Info("Read config file JanToItemInfoUrl = " + ConfigFile.JanToItemInfoUrl);


            //DoNotCheckWhenAggLtHexValue
            if (iniFile.Read("DoNotCheckWhenAggLtHexValue") == "")
            {
                ConfigFile.DoNotCheckWhenAggLtHexValue = "0";
            }
            else
            {
                ConfigFile.DoNotCheckWhenAggLtHexValue = iniFile.Read("DoNotCheckWhenAggLtHexValue");
            }
            logger.Info("Read config file DoNotCheckWhenAggLtHexValue = " + ConfigFile.DoNotCheckWhenAggLtHexValue);

            //ReConnectDeviceInterval
            if (iniFile.Read("ReConnectDeviceInterval") == "")
            {
                ConfigFile.ReConnectDeviceInterval = "0";
            }
            else
            {
                ConfigFile.ReConnectDeviceInterval = iniFile.Read("ReConnectDeviceInterval");
            }
            logger.Info("Read config file ReConnectDeviceInterval = " + ConfigFile.ReConnectDeviceInterval);

            //IsReadingBigVolumnRfidsMode
            string isReadingBigVolumnRfidsModeStr =  iniFile.Read("IsReadingBigVolumnRfidsMode"); 
            if (isReadingBigVolumnRfidsModeStr == "" || (isReadingBigVolumnRfidsModeStr?.ToLower() !="true" && isReadingBigVolumnRfidsModeStr?.ToLower() != "false"))
            {
                ConfigFile.IsReadingBigVolumnRfidsMode = false;
            }
            else
            {
                ConfigFile.IsReadingBigVolumnRfidsMode = Boolean.Parse(isReadingBigVolumnRfidsModeStr.ToLower().ToString());
            }
            logger.Info("Read config file IsReadingBigVolumnRfidsMode = " + ConfigFile.IsReadingBigVolumnRfidsMode);

            //ReConnectDBInterval
            if (iniFile.Read("ReConnectDBInterval") == "")
            {
                ConfigFile.ReConnectDBInterval = 60*1000; //60 seconds
            }
            else
            {
                ConfigFile.ReConnectDBInterval = Int32.Parse( iniFile.Read("ReConnectDBInterval"));
            }
            logger.Info("Read config file ReConnectDBInterval = " + ConfigFile.ReConnectDBInterval);

            //MaxRetryConnectDB
            if (iniFile.Read("MaxRetryConnectDB") == "")
            {
                ConfigFile.MaxRetryConnectDB = 10;
            }
            else
            {
                ConfigFile.MaxRetryConnectDB = Int32.Parse(iniFile.Read("MaxRetryConnectDB"));
            }
            logger.Info("Read config file MaxRetryConnectDB = " + ConfigFile.MaxRetryConnectDB);

            //ReloadShelfPosMasterRfid
            ConfigFile.ReloadShelfPosMasterRfid = iniFile.Read("ReloadShelfPosMasterRfid");

            //TcpHost
            ConfigFile.TcpHost = iniFile.Read("TcpHost");

            //Convert Antena
            ConfigFile.AntenaList = iniFile.Read("AntenaList");
            ConfigFile.NumberOfAntena = iniFile.Read("NumberOfAntena");

        }
    }
}
