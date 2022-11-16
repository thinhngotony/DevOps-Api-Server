using AxOPOSRFIDLib;
using BrightIdeasSoftware;
using FontAwesome.Sharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vjp.Rfid.SmartShelf.Constants;
using Vjp.Rfid.SmartShelf.Db;
using Vjp.Rfid.SmartShelf.Enums;
using Vjp.Rfid.SmartShelf.Forms;
using Vjp.Rfid.SmartShelf.Helper;
using Vjp.Rfid.SmartShelf.Interface;
using Vjp.Rfid.SmartShelf.Models;
using Vjp.Rfid.SmartShelf.Services;
using Timer = System.Windows.Forms.Timer;

namespace Vjp.Rfid.SmartShelf
{
    /// <summary>
    /// Version history 
    /// 0.1.2 : Add shelf name to 
    /// 0.1.3 : Edit import product pos ( add un import list and setting encoding to SHIFT_JIS)
    /// 0.1.4 : Add db connection checking
    /// 0.1.5 : Add reload shelf master if had in out item on shelf / change Int16 to Int32
    /// 0.1.6 : Add reload shelf master if readed setting tag in .ini file ( comment out 0.1.5)
    ///         Add product name to view 
    /// 0.1.6.1 : thêm log để comfirm cho dễ
    /// 0.1.7.0 : thêm TCP server / add them xử lý insert raw data after scan into table
    /// </summary>
    public partial class FormMenu : Form
    {

        //Fields(UI)
        private int borderSize = 2;
        private IconButton currentBtn;
        private Panel leftBorderBtn;
        private Form currentChildForm;

        //private static DeviceStatus deviceStatus;
        private Timer checkDeviceStatusTimer = new Timer();
        private int checkDeviceStatusInterval = 10000;
        
        private int readTagCount;
        private string selectedScannerName = string.Empty;
        //private AxOPOSRFIDLib.AxOPOSRFID axPos = null;
        private readonly static Dictionary<string, string> tagRfidReadedList = new Dictionary<string, string>();
        //private readonly static Dictionary<string, RfidView> tagRfidReadingList = new Dictionary<string, RfidView>();
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static string captionFormName;
        private AxOPOSRFID[] axPosArray; //array of scanner devices
        private IRfidShelfProductService  rfidShelfProductService;
        private RfidScannerService rfidScannerService ;
        public List<ScannerDeviceInfo> scannerDevices = new List<ScannerDeviceInfo>();

        private IRfidShelfHttpService  rfidShelfHttpService;

        private ScanMode scanMode = ScanMode.InOutShelfMode;
        private static List<RfidView> registerRfidList = new List<RfidView>();
        public FormMenu()
        {
            InitializeComponent();
            
            this.Padding = new Padding(borderSize);//Border size
            //this.BackColor = Color.FromArgb(98, 102, 244);//Border color

            leftBorderBtn = new Panel();
            leftBorderBtn.Size = new Size(7,60);
            panelSideMenu.Controls.Add(leftBorderBtn);

            //Form
            this.DoubleBuffered = true;

        }

        //--------------------------------------Rfid tag scan//--------------------------------------
        #region"Event"
        private void FormMenu_Load(object sender, EventArgs e)
        {
            try
            {
                bool isDbConnected = false;
                //checking DB connect
                logger.Info($"Loading start , checking DB connection");
                using (FrmCheckDbConnection checkForm = new FrmCheckDbConnection())
                {
                    checkForm.Show();
                    //Application.DoEvents();
                    //checkForm.TopMost = true;
                    isDbConnected = CheckDbConnection();

                }
                if (isDbConnected == false)
                    Application.Exit();

                logger.Info($"DB connection ok");
                //create service
                rfidScannerService = new RfidScannerService();

                //Create Tec rfid ocx control
                scannerDevices = CreateOposControls();

                //init value
                SetInitValueForm();

                //get sevice
                rfidShelfProductService = Program.GetService<IRfidShelfProductService>();

                rfidShelfHttpService = Program.GetService<IRfidShelfHttpService>();

                //get shelf product from DB
                rfidScannerService.RfidCurrentShelfProducts = GetRfidShelfProducts();

                //enable device to reading rfid from Uf-2140
                ScannerRun(axPosArray);

                //Hide controls
                HideItems();

                //check status device
                if(Int32.Parse(ConfigFile.ReConnectDeviceInterval) >0 )
                    CheckStatusDeviceTimerHandler();

                //Scanner list view init
                DisplayScannerDevices(scannerDevices);

                btnRfidScanner.PerformClick();

                //create tcp server
                startTcpServer();
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Cannot load form.{Ultil.GetSystemErrorMsg(ex)}" , captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
           
        }

        private void startTcpServer()
        {
            new Thread(() =>
            {
                TCPService.Run(); 
               
            }).Start();
        }

        private bool CheckDbConnection()
        {
            int retryCnt = 0;
            bool isConnected = false;
            while (isConnected == false && retryCnt <= ConfigFile.MaxRetryConnectDB)
            {
                retryCnt = retryCnt + 1;
                logger.Info($"Retrying DB Connection : {retryCnt}");
                isConnected = DbAccess.CheckDBConnect();
                Application.DoEvents();
                if (isConnected == false)
                {
                    Thread.Sleep(ConfigFile.ReConnectDBInterval);
                }
            }

            if (isConnected == false)
            {
               return false;
            }
            return true;
        }

        private void AxPos_StatusUpdateEvent(object sender, AxOPOSRFIDLib._DOPOSRFIDEvents_StatusUpdateEventEvent e)
        {
            //Console.WriteLine("AxPos_StatusUpdateEvent : " + e.status.ToString());

        }

        private void CheckStatusDeviceTimer_Tick(object sender, EventArgs e)
        {
            CheckStatusDevices(axPosArray);

        }

        //private void Command_EnableDevice_Click(object sender, EventArgs e)
        //{
        //    if (axPosArray[0].DeviceEnabled)
        //    {
        //        MessageBox.Show("Rfid scanner device was enabled.", captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }
        //    bool isSuccess = EnableRfidScannerDeviceHandler(axPosArray[0]);
        //    if (isSuccess == false)
        //    { 
        //        MessageBox.Show("Cannot enable rfid scanner device.", captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

        //private void Command_DisableDevice_Click(object sender, EventArgs e)
        //{

        //    if (axPosArray[0].DeviceEnabled==false)
        //    {
        //        MessageBox.Show("Rfid scanner device was disabled.", captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //        return;
        //    }
        //    bool isSuccess = DisableRfidScannerDeviceHandler(axPosArray[0]);
        //    if (isSuccess == false)
        //    {
        //        MessageBox.Show("Cannot disable rfid scanner device.", captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }

        //}
        //private void Command_StartReading_Click(object sender, EventArgs e)
        //{
        //    StartReadingDataHandler(axPosArray[0]);
            
        //}

        //private void Command_StopReading_Click(object sender, EventArgs e)
        //{
        //    StopReadingDataHandler(axPosArray[0]);
            
        //}

        private void AxPos_DataEvent(object sender, AxOPOSRFIDLib._DOPOSRFIDEvents_DataEventEvent ev)
        {
            DataEventHandler((AxOPOSRFID)sender);
        }

        private void AxPos_DirectIOEvent(object sender, AxOPOSRFIDLib._DOPOSRFIDEvents_DirectIOEventEvent e)
        {
            List_Event.Items.Add("<<DirectIOEvent Start. eventNumber=" + e.eventNumber);
            if (e.eventNumber == 1)
            {
                // トリガSW状態通知
                List_Event.Items.Add(e.pData);
            }
            else if (e.eventNumber == 2)
            {
                // バーコード読込通知
                List_Event.Items.Add(e.pString);
            }
            List_Event.Items.Add(">>DirectIOEvent End");
            List_Event.SelectedIndex = List_Event.Items.Count - 1;
        }
        private void AxPos_OutputCompleteEvent(object sender, AxOPOSRFIDLib._DOPOSRFIDEvents_OutputCompleteEventEvent e)
        {
            List_Event.Items.Add("OutputCompleteEvent OutputID=" + e.outputID);
            List_Event.SelectedIndex = List_Event.Items.Count - 1;
            
            logger.Info($"OutputCompleteEvent OutputID={e.outputID}");
        }
        private async void AxPos_ErrorEvent(object sender, AxOPOSRFIDLib._DOPOSRFIDEvents_ErrorEventEvent e)
        {
            try
            {


                //var scannerName = ((AxOPOSRFID)sender).Tag.ToString();
                var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp((AxOPOSRFID)sender);

                var scannerName = deviceInfoFromtagProp.DeviceName;
                var shelfName = deviceInfoFromtagProp.ShelfName;

                List_Event.Items.Add("ErrorEvent ResultCode=" + e.resultCode.ToString() + " Extended=" + e.resultCodeExtended.ToString() + " ErrorLocus=" + e.errorLocus.ToString() + " pErrorResponse=" + e.pErrorResponse.ToString());
                List_Event.SelectedIndex = List_Event.Items.Count - 1;

                if (e.resultCode == OposConstants.OposEFailure && e.resultCodeExtended == 252 && e.errorLocus == OposConstants.OposElInput)
                {
                    SetDeviceStatus(shelfName, scannerName, DeviceStatus.NotAvailable);
                    ShowStatusInformation($"{shelfName} : Rfid scanner {scannerName} service unavailable.");
                }
                else if (e.resultCode == OposConstants.OposETimeout && e.resultCodeExtended == 0 && e.errorLocus == OposConstants.OposElInput)
                {
                    ShowStatusInformation("Reading...");
                    //rfidScannerService.ItemInOutShelfProcess(new List<string>());
                    var deviceInfo = scannerDevices.Find(x => x.DeviceName.ToLower() == scannerName.ToLower() && x.ShelfName.ToLower() == shelfName.ToLower());

                    if (deviceInfo != null)
                    {
                        logger.Info($"{shelfName} : {deviceInfo.DeviceName} Read empty tag : start counter");

                        deviceInfo.ReadEmptyTagCounter = deviceInfo.ReadEmptyTagCounter + 1;

                        logger.Info($"{shelfName} : {deviceInfo.DeviceName} Read empty tag : counter = {deviceInfo.ReadEmptyTagCounter} ");

                        //if (deviceInfo.ReadHasTagsCounter > 0)
                        if (deviceInfo.ReadHasTagsCounter > 0
                            && (ConfigFile.IsReadingBigVolumnRfidsMode == false
                                || (ConfigFile.IsReadingBigVolumnRfidsMode == true && deviceInfo.ReadEmptyTagCounter > ConfigFile.ReadEmptyTagCounter - 3))
                           )
                        {
                            logger.Info($"{shelfName} : {deviceInfo.DeviceName} ReadHasTagsCounter in read empty tag finished counter : " + deviceInfo.ReadHasTagsCounter.ToString());
                            deviceInfo.ReadHasTagsCounter = 0;
                            HasTagsInOutStartUpdate(shelfName, deviceInfo.DeviceName);

                            //var tags = deviceInfo.RfidReadingTagsList.Keys.ToList();
                            //deviceInfo.RfidReadingTagsList.Clear();

                            //await rfidScannerService.ItemInOutShelfProcessAsync(tags, scannerName);

                        }

                        if (deviceInfo.ReadEmptyTagCounter >= (AppConstants.PosReadTimerInterval / 1000) * ConfigFile.ReadEmptyTagCounter)
                        {
                            logger.Info($"{shelfName} : {deviceInfo.DeviceName} Read empty tag : finished counter => run in-out shelf process with empty list(the same all book out of shelf)");
                            deviceInfo.ReadEmptyTagCounter = 0;
                            deviceInfo.ReadHasTagsCounter = 0;
                            deviceInfo.RfidReadingTagsList.Clear();
                            deviceInfo.RfidUnReadingTagsList.Clear();
                            //reset
                            ResetScannerDeviceByName(shelfName, deviceInfo.DeviceName);
                            //in out shelf process
                            await rfidScannerService.ItemInOutShelfProcessAsync(deviceInfo.RfidReadingTagsList.Keys.ToList(), new List<string>(), shelfName, scannerName);
                        }

                        //deviceInfo.ReadEmptyTagCounter = deviceInfo.ReadEmptyTagCounter + 1;

                        //if (deviceInfo.ReadEmptyTagTimer.IsRunning)
                        //{
                        //    logger.Debug($"{deviceInfo.DeviceName} Read empty tag : timer is running.");
                        //    return;
                        //}

                        //deviceInfo.ReadEmptyTagTimer.Start();
                        //deviceInfo.ReadEmptyTagTimer.SetTime(AppConstants.ReadEmptyTagWaitTimeout);
                        //start timer
                        //logger.Debug($"{deviceInfo.DeviceName} Read empty tag : start timer");
                        //deviceInfo.ReadEmptyTagTimer.CountDownFinished += async () =>
                        //{
                        //    //update log data if count > 0 
                        //    if (deviceInfo.ReadHasTagsCounter > 0)
                        //    {
                        //        logger.Debug($"{deviceInfo.DeviceName} ReadHasTagsCounter in read empty tag finished countdown : " + deviceInfo.ReadHasTagsCounter.ToString());
                        //        deviceInfo.ReadHasTagsCounter = 0;
                        //        var tags = deviceInfo.RfidReadingTagsList.Keys.ToList();
                        //        deviceInfo.RfidReadingTagsList.Clear();

                        //        await rfidScannerService.ItemInOutShelfProcessAsync(tags, scannerName);

                        //    }
                        //    deviceInfo.RfidReadingTagsList.Clear();
                        //    //all book get out from shelf
                        //    logger.Debug($"{deviceInfo.DeviceName} Read empty tag : finished count down => run in-out shelf process with empty list(the same all book out of shelf)");
                        //    await rfidScannerService.ItemInOutShelfProcessAsync(deviceInfo.RfidReadingTagsList.Keys.ToList(), scannerName);
                        //    deviceInfo.ReadEmptyTagTimer.Stop();
                        //};

                    }


                }
                else
                {
                    ShowStatusInformation("ErrorEvent: ResultCode=" + e.resultCode.ToString() + "/ Extended=" + e.resultCodeExtended.ToString() + "/ ErrorLocus=" + e.errorLocus.ToString() + "/ pErrorResponse=" + e.pErrorResponse.ToString());
                }
                // No retry
                e.pErrorResponse = OposConstants.OposErClear;
            }
            catch(Exception ex)
            {
                logger.Error($"AxPos_ErrorEvent error : {ex.Message}");
            }
        }

        private ScannerDeviceInfo GetDeviceInfoObjectFromTagProp(AxOPOSRFID axoPos)
        {
            var info = (ScannerDeviceInfo)axoPos.Tag;

            return new ScannerDeviceInfo {
                DeviceName = info?.DeviceName,
                ShelfName = info?.ShelfName,
            };
        }

        //private void btnExit_Click(object sender, EventArgs e)
        //{
        //    Application.Exit();
        //}
        private void FormMenu_FormClosed(object sender, FormClosedEventArgs e)
        {
            foreach(var deviceInfo in scannerDevices)
            {   
                if (deviceInfo.ReadHasTagsCounter >= (AppConstants.PosReadTimerInterval * ConfigFile.ReadHasTagsCounter) / 1000)
                {
                    logger.Info($"{deviceInfo.ShelfName} : {deviceInfo.DeviceName} FormClosed ReadHasTagsCounter : " + deviceInfo.ReadHasTagsCounter.ToString());
                    HasTagsInOutStartUpdate(deviceInfo.ShelfName , deviceInfo.DeviceName);
                }
            }
        }

        private AxOPOSRFID GetOposByName(string sheflName , string deviceName)
        {
            var ctl = axPosArray.FirstOrDefault(c =>((ScannerDeviceInfo)(c.Tag)).DeviceName.ToLower() == deviceName.ToLower() 
                                                && ((ScannerDeviceInfo)(c.Tag)).ShelfName.ToLower() == sheflName.ToLower());
            return ctl;
        }
        private void btnShelfLogView_Click(object sender, EventArgs e)
        {
            var shelfLogViewForm = new FormShelfLog();
            shelfLogViewForm.Show();
        }

        private void olvData_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Console.WriteLine(olvData.SelectedItem.Text);
            selectedScannerName = olvScannerDevices.SelectedItem?.Text;
        }

        private void olvData_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {

        }

        private void olvData_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            selectedScannerName = string.Empty;
        }

        #endregion


        #region"Function"
        private void SetInitValueForm()
        {

            Text_OpenName.Text = ConfigFile.RfidDeviceNames[0];

            captionFormName = this.Text;

            this.Text = this.Text + "(" + Ultil.GetVersion() + ")";
        }
        //private void CreateOposControl()
        //{
        //    try
        //    {
        //        axPos = new AxOPOSRFIDLib.AxOPOSRFID();
        //        logger.Info("Create OPOS control");
        //        axPos.CreateControl();
        //    }
        //    catch (Exception ex)
        //    {
        //        logger.Error("Create OPOS control Failed :" + ex.Message);
        //        MessageBox.Show(ex.Message, CaptionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        //    }
        //}

        private List<ScannerDeviceInfo> CreateOposControls()
        {
            List<ScannerDeviceInfo> scannerList = new List<ScannerDeviceInfo>();
            try
            {
                //create array control
                axPosArray = new AxOPOSRFID[AppConstants.PosDeviceCnt];

                for(int device =0; device < AppConstants.PosDeviceCnt; device++)
                {
                    axPosArray[device] = new AxOPOSRFID();
                    axPosArray[device].CreateControl();

                    axPosArray[device].DataEvent += AxPos_DataEvent;
                    axPosArray[device].DirectIOEvent += AxPos_DirectIOEvent;
                    axPosArray[device].ErrorEvent += AxPos_ErrorEvent;
                    axPosArray[device].OutputCompleteEvent += AxPos_OutputCompleteEvent;
                    axPosArray[device].StatusUpdateEvent += AxPos_StatusUpdateEvent;

                    //axPosArray[device].Tag = ConfigFile.RfidDeviceNames[device];
                    
                    ScannerDeviceInfo scannerDeviceInfo = new ScannerDeviceInfo();
                    //scannerDeviceInfo.DeviceName = axPosArray[device].Tag.ToString();
                    scannerDeviceInfo.RfidUnReadingTagsList = InitShelfProductToRfidDict( rfidScannerService.GetRfidShelfProducts());
                    ScannerDeviceInfoBindingEvent(scannerDeviceInfo);

                    //add more info
                    scannerDeviceInfo.DeviceName = ConfigFile.RfidDeviceNames[device];
                    scannerDeviceInfo.ShelfName = ConfigFile.RfidDeviceNamesMapToShelfNames[device];
                    //add to Tag property
                    axPosArray[device].Tag = scannerDeviceInfo;

                    scannerList.Add(scannerDeviceInfo);
                }
                
                logger.Info($"Create {axPosArray.Length} OPOS controls");

            }
            catch (Exception ex)
            {
                logger.Error($"Create {axPosArray.Length} OPOS controls Failed :" + ex.Message);
                MessageBox.Show(ex.Message, captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            return scannerList;
        }

        private Dictionary<string, RfidView> InitShelfProductToRfidDict(List<RfidShelfProduct> rfidShelfProducts)
        {
            Dictionary<string, RfidView> result = new Dictionary<string, RfidView>();
            foreach (var item in rfidShelfProducts)
            {
                if(result.ContainsKey(item.Rfid) == false)
                {
                    result.Add(item.Rfid, new RfidView
                    {
                        ReadCount = 0,
                        Rfid = item.Rfid,
                        ScannerDeviceName = item.ScannerName,
                        ShelfName = item.ShelfName
                    });
                }

                //result.Add(item.Rfid, new RfidView
                //{
                //    ReadCount = 0,
                //    Rfid = item.Rfid,
                //    ScannerDeviceName = item.ScannerName,
                //    ShelfName = item.ShelfName
                //});

            }
            return result;
        }

        private void HideItems()
        {
            grbResult.Visible = false;
        }
        
        private void CheckStatusDeviceTimerHandler()
        {
            //ConfigFile.ReConnectDeviceInterval
            //checkDeviceStatusTimer.Interval = checkDeviceStatusInterval;
            if (Int32.Parse(ConfigFile.ReConnectDeviceInterval) > 0)
            {
                checkDeviceStatusTimer.Interval = Int32.Parse(ConfigFile.ReConnectDeviceInterval);
                checkDeviceStatusTimer.Tick += CheckStatusDeviceTimer_Tick;
                checkDeviceStatusTimer.Enabled = true;
            }
        }

        private void CheckStatusDevices(AxOPOSRFID[] axPosDevices)
        {
            DeviceStatus deviceStatus = DeviceStatus.Ready;

            for (int i = 0; i < axPosDevices.Length; i++)
            {
                bool isReady = axPosArray[i].DeviceEnabled;
                bool isSuccess = true;
                var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp(axPosArray[i]);

                deviceStatus = DeviceStatus.Ready;
                
                if (isReady)
                {
                    deviceStatus = DeviceStatus.Ready;
                    //ShowStatusInformation($"Reading...");
                    //update device status
                    UpdateDeviceStatusOfScanner(scannerDevices, deviceInfoFromtagProp.ShelfName , deviceInfoFromtagProp.DeviceName, deviceStatus);
                }
                else
                {
                    //deviceStatus = DeviceStatus.NotAvailable;
                    //enable device 

                    isSuccess = EnableRfidScannerDeviceHandler(axPosDevices[i]);
                    if (isSuccess == false)
                    {
                        deviceStatus = DeviceStatus.NotAvailable;
                        //update device status
                        UpdateDeviceStatusOfScanner(scannerDevices, deviceInfoFromtagProp.ShelfName, deviceInfoFromtagProp.DeviceName, deviceStatus);
                        //log 
                        logger.Error($"{deviceInfoFromtagProp.ShelfName} : Check device status : Cannot enable rfid scanner device {deviceInfoFromtagProp.DeviceName}.");
                        ShowStatusInformation($"{deviceInfoFromtagProp.ShelfName}: Check device status : Cannot enable rfid scanner device {deviceInfoFromtagProp.DeviceName}.");

                        return;
                    }
                }
                //start reading
                if(axPosDevices[i].ContinuousReadMode == false)
                {
                    isSuccess = StartReadingDataHandler(axPosDevices[i]);
                }
                
                if (isSuccess == false)
                {
                    deviceStatus = DeviceStatus.NotAvailable;
                }
                else
                {
                    deviceStatus = DeviceStatus.Ready;
                    ShowStatusInformation($"Reading...");
                }
                //update device status
                UpdateDeviceStatusOfScanner(scannerDevices, deviceInfoFromtagProp.ShelfName, deviceInfoFromtagProp.DeviceName, deviceStatus);

                
            }
        }

        private void UpdateDeviceStatusOfScanner(List<ScannerDeviceInfo> scannerDeviceInfos, string shelfName, string deviceName, DeviceStatus deviceStatus)
        {
            foreach (var device in scannerDeviceInfos)
            {
                if (device.DeviceName.ToLower() == deviceName.ToLower() && device.ShelfName.ToLower() == shelfName.ToLower())
                {
                    //update status -> notiry event will update UI
                    device.DeviceStatus = deviceStatus;

                }
            }
        }
        private void DisplayScannerDevices(List<ScannerDeviceInfo> scannerDevices)
        {
            ListViewInitialize(scannerDevices);
        }

        private void ScannerRun(AxOPOSRFID[] axPosDevices)
        {
            //List<ScannerDeviceInfo> scannerList = new List<ScannerDeviceInfo>();

            for (int i = 0; i < axPosDevices.Length; i++)
            {
                var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp(axPosDevices[i]);
                //ScannerDeviceInfo scannerDeviceInfo = scannerDevices.FirstOrDefault(p => p.DeviceName.ToLower() == axPosDevices[i].Tag.ToString().ToLower());
                ScannerDeviceInfo scannerDeviceInfo = scannerDevices.FirstOrDefault(p => p.DeviceName.ToLower() == deviceInfoFromtagProp.DeviceName.ToLower()
                && p.ShelfName.ToLower() == deviceInfoFromtagProp.ShelfName.ToLower());
                //scannerDeviceInfo.DeviceName = axPosDevices[i].Tag.ToString();

                //ScannerDeviceInfoBindingEvent(scannerDeviceInfo);

                bool isSuccess = EnableRfidScannerDeviceHandler(axPosDevices[i]);

                if (isSuccess == false)
                {
                    scannerDeviceInfo.DeviceStatus = DeviceStatus.NotAvailable;

                    //SetDeviceStatusImg(DeviceStatus.NotAvailable);
                }
                else
                {
                    
                    //SetDeviceStatusImg(DeviceStatus.Ready);

                    //start reading
                    isSuccess = StartReadingDataHandler(axPosDevices[i]);
                    
                    if (isSuccess == false)
                    {
                        scannerDeviceInfo.DeviceStatus = DeviceStatus.NotAvailable;
                    }
                    else
                    {
                        scannerDeviceInfo.DeviceStatus = DeviceStatus.Ready;
                    }
                    
                }

                //scannerList.Add(scannerDeviceInfo);
            }

            //return scannerList;
        }

        private void ScannerDeviceInfoBindingEvent(ScannerDeviceInfo scannerDeviceInfo)
        {
            scannerDeviceInfo.DeviceStatusChangedNotify += ScannerDeviceInfo_DeviceStatusChangedNotify;
        }

        private void ScannerDeviceInfo_DeviceStatusChangedNotify()
        {
            ListViewDataRefresh(scannerDevices);
        }

        private bool EnableRfidScannerDeviceHandler(AxOPOSRFID axPos)
        {
            int Result;
            int phase;
            string strData;
            bool isSuccess = false;

            if (axPos.DeviceEnabled)
                return true;
            // Open Device
            Text_MethodName.Text = "Open";
            var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp(axPos);
            Result = axPos.Open(deviceInfoFromtagProp.DeviceName);
            Text_Result.Text = Result.ToString();
            Text_ResultCode.Text = axPos.ResultCode.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"Open Device {deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                ShowStatusInformation($"Open Device {deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                return isSuccess; // Error
            }

            // ClaimDevice
            Text_MethodName.Text = "ClaimDevice";
            Result = axPos.ClaimDevice(3000);
            Text_Result.Text = Result.ToString();
            Text_ResultCode.Text = axPos.ResultCode.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"ClaimDevice {deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                ShowStatusInformation($"ClaimDevice {deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                axPos.Close(); // Close for re-open
                return isSuccess; // Error
            }


            // DeviceEnabled=True
            Text_MethodName.Text = "DeviceEnabled=True";
            axPos.DeviceEnabled = true;
            Result = axPos.ResultCode;
            Text_ResultCode.Text = Result.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} DeviceEnabled=True error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                ShowStatusInformation($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} DeviceEnabled=True error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                axPos.Close(); // Close for re-open
                return isSuccess; // Error
            }

            // DirectIOを用いて現在の位相状態を取得する
            phase = 0;
            strData = "";
            Result = axPos.DirectIO(115, ref phase, ref strData);
            if (Result == OposConstants.OposSuccess)
            {
                // 現在の位相の状態を画面に表示する。ただし、アンテナは常に有効になるので除外して判定する
                if (Convert.ToBoolean(phase) && phase != 128)
                {
                    CheckBox_Phase.Checked = true;
                }
                else
                {
                    CheckBox_Phase.Checked = false;
                }
               
            }

            // BinaryConversion=OPOS_BC_NIBBLE
            Text_MethodName.Text = "BinaryConversion=OPOS_BC_NIBBLE";
            axPos.BinaryConversion = OposConstants.OposBcNibble;
            Result = axPos.ResultCode;
            Text_ResultCode.Text = Result.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} BinaryConversion=OPOS_BC_NIBBLE error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                ShowStatusInformation($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} BinaryConversion=OPOS_BC_NIBBLE error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                axPos.Close(); // Close for re-open
                return isSuccess; // Error
            }

            // Protocolmask=RFID_PR_EPC1G2
            Text_MethodName.Text = "Protocolmask=RFID_PR_EPC1G2";
            axPos.ProtocolMask = OposConstants.RfidPrEpc1g2;
            Result = axPos.ResultCode;
            Text_ResultCode.Text = Result.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} Protocolmask=RFID_PR_EPC1G2 error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                ShowStatusInformation($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} Protocolmask=RFID_PR_EPC1G2 error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                axPos.Close(); // Close for re-open
                return isSuccess; // Error
            }
            
            logger.Info($"Open decive {deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} successfully");
            ShowStatusInformation($"Open decive {deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} successfully");
            return true;

        }

        private void SetDeviceStatus( string shelfName , string deviceName , DeviceStatus status)
        {
            var deviceInfo = GetDeviceInfoByName(shelfName , deviceName );

            if (deviceInfo != null)
            {
                deviceInfo.DeviceStatus = status;
               
            }
        }

        private bool DisableRfidScannerDeviceHandler(AxOPOSRFID axPos)
        {
            bool isSuccess = false;
            int Result;

            if (axPos.DeviceEnabled == false)
                return true;

            var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp(axPos);
            // DeviceEnabled=False
            Text_MethodName.Text = "DeviceEnabled=False";
            axPos.DeviceEnabled = false;
            Result = axPos.ResultCode;
            Text_ResultCode.Text = Result.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"DeviceEnabled=False error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");

                return isSuccess; // Error
            }

            // ReleaseDevice
            Text_MethodName.Text = "ReleaseDevice";
            Result = axPos.ReleaseDevice();
            Text_Result.Text = Result.ToString();
            Text_ResultCode.Text = axPos.ResultCode.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"ReleaseDevice error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");

                return isSuccess; // Error
            }

            // Close Device
            Text_MethodName.Text = "Close";
            Result = axPos.Close();
            Text_Result.Text = Result.ToString();
            Text_ResultCode.Text = axPos.ResultCode.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"Close error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");

                return isSuccess; // Error
            }

            //SetDeviceStatusImg( DeviceStatus.NotAvailable);
            SetDeviceStatus(deviceInfoFromtagProp.ShelfName, deviceInfoFromtagProp.DeviceName, DeviceStatus.NotAvailable);

            ShowStatusInformation($"Close device {deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} successfully");
            logger.Info($"Close device {deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} successfully");
            return true;
        }
        private bool StartReadingDataHandler(AxOPOSRFID axPos)
        {
            logger.Info("StartReadTags start");
            bool isSuccess = true;
            int Result;

            var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp(axPos);
            //Console.WriteLine("StartReadingDataHandler CapContinuousRead = " + axPos.CapContinuousRead);
            //Console.WriteLine("StartReadingDataHandler ContinuousReadMode = " + axPos.ContinuousReadMode);
            tagRfidReadedList.Clear();
            //tagRfidReadingList.Clear();
            var deviceInfo = GetDeviceInfoByName(deviceInfoFromtagProp.ShelfName.ToLower() , deviceInfoFromtagProp.DeviceName.ToLower());
            if (deviceInfo != null)
            {
                deviceInfo.RfidReadingTagsList.Clear();
                deviceInfo.ReadHasTagsCounter = 0;
            }
            
            olvReceivedTagsData.Items.Clear();
            txtTagCount.Text = "0";

            readTagCount = 0;
            Text_ReadTagCount.Text = readTagCount.ToString();

            // ClearInputProperties
            axPos.ClearInputProperties();

            // ReadTimerInterval=1000
            axPos.ReadTimerInterval = ConfigFile.ScannerReadTimerInterval;

            // DataEventEnabled=True
            axPos.DataEventEnabled = true;

            PhaseChange(axPos);

            // StartReadTags : TagID only, no filter, timeout=5000, no password
            Text_MethodName.Text = "StartReadTags";
            Result = axPos.StartReadTags(OposConstants.RfidRtId, AppConstants.PosFilterId, AppConstants.PosFilterMask, 0, 0, AppConstants.PosStartReadTimeout, AppConstants.PosPassword);
            Text_Result.Text = Result.ToString();
            Text_ResultCode.Text = axPos.ResultCode.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"StartReadTags error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                ShowStatusInformation("StartReadTags error");
                isSuccess = false;
                return isSuccess;
            }

            logger.Info($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} StartReadTags end");

            ShowStatusInformation($" {deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} is reading...");
            return isSuccess;
            
        }

        //private ScannerDeviceInfo GetDeviceInfoByName(string scannerName)
        //{
        //    return scannerDevices.Find(x => x.DeviceName.ToLower() == scannerName.ToLower());
        //}

        private ScannerDeviceInfo GetDeviceInfoByName( string shelfName , string scannerName)
        {
            return scannerDevices.Find(x => x.DeviceName.ToLower() == scannerName.ToLower() && x.ShelfName.ToLower() == shelfName.ToLower());
        }

        private bool StopReadingDataHandler(AxOPOSRFID axPos)
        {
            int Result;
            bool isSuccess = true;
            logger.Info("StopReadTags start");
            var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp(axPos);
            // StopReadTags : no password
            Text_MethodName.Text = "StopReadTags";
            Result = axPos.StopReadTags(AppConstants.PosPassword);
            Text_Result.Text = Result.ToString();
            Text_ResultCode.Text = axPos.ResultCode.ToString();
            Text_ResultCodeExtended.Text = axPos.ResultCodeExtended.ToString();
            if (Result != OposConstants.OposSuccess)
            {
                logger.Error($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} StopReadTags error.Result = {Result.ToString()} / ResultCode ={axPos.ResultCode.ToString()} / ResultCodeExtended={axPos.ResultCodeExtended.ToString()}");
                isSuccess = false;
            }
            logger.Info($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} StopReadTags end");
            ShowStatusInformation($"{deviceInfoFromtagProp.ShelfName} / {deviceInfoFromtagProp.DeviceName} Stop reading successfully");
            return isSuccess;
        }

        private  void DataEventHandler(AxOPOSRFID axPos)
        {
            string CurrentTagID;
            int TagCount;
            int LoopCnt;
            string UserData;
            //string scannerName = axPos.Tag.ToString();

            try
            {
                var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp(axPos);

                if (List_Event.Items.Count > AppConstants.MAX_ROW_DISPLAY)
                {
                    List_Event.Items.Clear();
                    readTagCount = 0;
                    Text_ReadTagCount.Text = readTagCount.ToString();

                }
                Console.WriteLine($"{Ultil.GetDateTimeForOutputLog()} >> Device name {deviceInfoFromtagProp.DeviceName} and Shelf name {deviceInfoFromtagProp.ShelfName}");
                //if(olvReceivedTagsData.Items.Count > AppConstants.LvMaxViewItems)
                //{
                //    olvReceivedTagsData.Items.Clear();
                //}

                logger.Info($"↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓-Device name: {deviceInfoFromtagProp.ShelfName} { deviceInfoFromtagProp.DeviceName} DataEvent Start-↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓");

                //clear timer 
                //scannerDevices.Find(x => x.DeviceName.ToLower() == scannerName.ToLower());
                var deviceInfo = GetDeviceInfoByName(deviceInfoFromtagProp.ShelfName, deviceInfoFromtagProp.DeviceName);

                if (deviceInfo != null)
                {
                    deviceInfo.ReadEmptyTagCounter = 0;
                    deviceInfo.ReadHasTagsCounter = deviceInfo.ReadHasTagsCounter + 1;

                    //logger.Debug($"Read empty tag : stop timer");
                    //deviceInfo.ReadEmptyTagTimer.CountDownFinished = null;
                    //deviceInfo.ReadEmptyTagTimer.Stop();

                    logger.Info($"{deviceInfo.ShelfName} / {deviceInfo.DeviceName} ReadHasTagsCounter = {deviceInfo.ReadHasTagsCounter}");
                }
                // TagCount
                TagCount = axPos.TagCount;
                readTagCount = readTagCount + TagCount;
                Text_ReadTagCount.Text = readTagCount.ToString();

                logger.Info($"<<DataEvent Start TagCount=" + TagCount.ToString());
                List_Event.Items.Add($"<<DataEvent Start TagCount=" + TagCount.ToString());

                var loopTo = TagCount;
                //remove all rfid
                //tagRfidReadingList.Clear();
                //deviceInfo.RfidReadingTagsList.Clear();
                registerRfidList.Clear(); 
                //StartHasTagsTimmerByScannerName(scannerName , );
                for (LoopCnt = 1; LoopCnt <= loopTo; LoopCnt++)
                {
                    switch (Ultil.DataFromClient.ToUpper())
                    {
                        //Tony - Working with Stop and Start
                        case  AppConstants.ActionRegisterShelfStart:
                            //Tony - Stop devices
                            //var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp(axPos);
                            StopReadingDataHandler(axPosArray[0]);
                            StartReadingDataHandler(axPosArray[0]);
                            var ctl = GetOposByName(deviceInfoFromtagProp.ShelfName, deviceInfoFromtagProp.DeviceName);

                            scanMode = ScanMode.RegisterShelfMode;
                            labScanMode.Text = "REG";
                            labScanMode.Visible = true;
                            logger.Info($"RegisterShelfMode");
                            Ultil.DataFromClient = "";

                            break;

                        //case AppConstants.ActionReloadShelfEnd:
                        //    scanMode = ScanMode.ReloadShelfMaster;
                        //    labScanMode.Text = "REL";
                        //    labScanMode.Visible = false;
                        //    logger.Info($"ReloadShelfMaster");
                        //    break;

                        case AppConstants.ActionRegisterShelfEnd:
                            //Tony add
                            scanMode = ScanMode.ReloadShelfMaster;
                            labScanMode.Text = "REL";
                            labScanMode.Visible = false;
                            logger.Info($"ReloadShelfMaster");
                            ReloadShelfPosMasterWithoutRFID();
                            Ultil.DataFromClient = "";
                            scanMode = ScanMode.InOutShelfMode;
                            break;

                        default:
                            if (Ultil.DataFromClient == AppConstants.ActionRegisterShelfEnd)
                            {
                                scanMode = ScanMode.InOutShelfMode;
                                labScanMode.Visible = false;
                                logger.Info($"ActionRegisterShelfEnd");                                
                            } else
                            {
                                //Do nothing
                            }
                            break;
                    }

                    // CurrentTagID
                    // UPGRADE_WARNING: オブジェクト CurrentTagID の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                    CurrentTagID = axPos.CurrentTagID;
                    // CurrentTagUserData
                    UserData = " Userdata=" + axPos.CurrentTagUserData;

                    if (UserData == " Userdata=")
                    {
                        UserData = "";
                    }

                    List_Event.Items.Add(Ultil.ConvertTagIDCode(axPos.CurrentTagID) + UserData);

                    //logger.Debug(" Userdata=" + axPos.CurrentTagUserData +"/ CurrentTagID=" + CurrentTagID + "/ Decode=" + Ultil.ConvertTagIDCode(CurrentTagID));

                    //save to dictionary to remove duplicate data 
                    //if (tagRfidReadedList.ContainsKey(Ultil.ConvertTagIDCode(CurrentTagID)) == false)
                    //{
                    //    tagRfidReadedList.Add(Ultil.ConvertTagIDCode(CurrentTagID), RfidStatus.UnCheck.ToString());
                    //    //add to current dict to check rfid status
                    //    if (tagRfidReadingList.ContainsKey(Ultil.ConvertTagIDCode(CurrentTagID)) == false)
                    //    {
                    //        tagRfidReadingList.Add(Ultil.ConvertTagIDCode(CurrentTagID), RfidStatus.UnCheck.ToString());

                    //    }
                    //}

                    //tagRfidReadingList.Add(Ultil.ConvertTagIDCode(CurrentTagID), axPos.Tag.ToString());
                    var convertInfo = Ultil.RawTagToReadable(CurrentTagID);
                    //convertInfo.ScannerDeviceName = axPos.Tag.ToString();
                    convertInfo.ScannerDeviceName = deviceInfoFromtagProp.DeviceName;
                    convertInfo.ShelfName = deviceInfoFromtagProp.ShelfName;
                    //logger.Info($"ShelfName = {convertInfo.ShelfName}, Rfid={convertInfo.Rfid} / RSSI ={convertInfo.RSSI}" );

                    switch (scanMode){
                        case ScanMode.RegisterShelfMode:
                            //scan and register to shelf master
                            //Tony stop start in loop is wrong
                            //StopReadingDataHandler(axPosArray[0]);
                            //StartReadingDataHandler(axPosArray[0]);
                            logger.Info($"scan mode >> {convertInfo.Rfid} : {convertInfo.AntenNo} : {convertInfo.RSSI} : {convertInfo.ShelfName}");
                            registerRfidList.Add(convertInfo);
                            break;

                        case ScanMode.InOutShelfMode:

                            //process only rfid depend on shelf
                            if (IsExistedRfidinShelf(convertInfo.ShelfName, convertInfo.Rfid))
                            {
                                //tagRfidReadingList.Add(convertInfo.Rfid, convertInfo);
                                if (deviceInfo.RfidReadingTagsList.ContainsKey(convertInfo.Rfid) == false)
                                {
                                    convertInfo.ReadCount = 1;
                                    deviceInfo.RfidReadingTagsList.Add(convertInfo.Rfid, convertInfo);
                                }
                                else
                                {
                                    //get current read count 
                                    int currentReadCount = deviceInfo.RfidReadingTagsList.First(x => x.Key == convertInfo.Rfid).Value.ReadCount;
                                    deviceInfo.RfidReadingTagsList.First(x => x.Key == convertInfo.Rfid).Value.ReadCount = currentReadCount + 1;
                                }
                            }
                            else
                            {
                                logger.Info($"Shelf name {convertInfo.ShelfName} do not have rfid {convertInfo.Rfid}");
                            }

                            break;
                    }

                    //reload master if rfid is special tag.
                    ReloadShelfPosMaster(convertInfo.Rfid);

                    //Console.WriteLine($"Reading tags : {deviceInfo.ShelfName} / {deviceInfo.DeviceName} / Tags Count ={deviceInfo.ReadHasTagsCounter} ");
                    logger.Info($"Reading tags : {deviceInfo.ShelfName} / {deviceInfo.DeviceName}/ReadHasTagsCounter ={deviceInfo.ReadHasTagsCounter}  / Rfid={convertInfo.Rfid} / RSSI ={convertInfo.RSSI}");
                    //Console.WriteLine($"Readable {CurrentTagID} rfid {Ultil.RawTagToReadable(CurrentTagID).Rfid} with anten no {Ultil.RawTagToReadable(CurrentTagID).AntenNo}");
                    // NextTag
                    axPos.NextTag();
                }
                //register mode 

                logger.Info("Check status " +  scanMode.ToString());
                if (scanMode == ScanMode.RegisterShelfMode)
                {
                    //TonyInsert
                    InsertRawRfidReaded(registerRfidList);
                    labScanMode.Visible = false;
                }
                //append 
                //await ShelfProductAppendMode(deviceInfo.RfidReadingTagsList, rfidScannerService.RfidCurrentShelfProducts , deviceInfo.ShelfName , deviceInfo.DeviceName);
                //update unreding tags list
                if (scanMode == ScanMode.InOutShelfMode)
                    UpdateUnReadingTags(deviceInfo);

                //抜き取り時間 / 戻し時間 update 
                //rfidScannerService.RfidCurrentShelfProducts = rfidScannerService.GetRfidShelfProducts();

                //insert to shelf product if tag is not existed
                //ShelfProductAppendMode(tagRfidReadingList , rfidScannerService.RfidCurrentShelfProducts);

                //抜き取り時間 / 戻し時間 process 
                //rfidScannerService.ItemInOutShelfProcess(tagRfidReadingList.Keys.ToList());

                //Console.WriteLine("Start insert");

                //await rfidScannerService.ItemInOutShelfProcessAsync(tagRfidReadingList.Keys.ToList());

                //await rfidScannerService.ItemInOutShelfProcessAsync(tagRfidReadingList.Keys.ToList() , axPos.Tag.ToString());
                if (scanMode == ScanMode.InOutShelfMode && deviceInfo.ReadHasTagsCounter >= (AppConstants.PosReadTimerInterval * ConfigFile.ReadHasTagsCounter) / 1000)
                {

                    //Console.WriteLine($"{deviceInfo.ShelfName} / {deviceInfo.DeviceName} / Tags Count ={deviceInfo.ReadHasTagsCounter} Update DB Starting ...");
                    logger.Info($"{deviceInfo.ShelfName} / {deviceInfo.DeviceName} / Tags Count ={deviceInfo.ReadHasTagsCounter} Update DB Starting ...");
                    logger.Info($"{deviceInfo.ShelfName} / {deviceInfo.DeviceName} ReadHasTagsCounter : " + deviceInfo.ReadHasTagsCounter.ToString());

                    HasTagsInOutStartUpdate(deviceInfo.ShelfName, deviceInfo.DeviceName);

                    logger.Info($"{deviceInfo.ShelfName} / {deviceInfo.DeviceName} / Tags Count ={deviceInfo.ReadHasTagsCounter} Update DB Finished");
                    //Console.WriteLine($"{deviceInfo.ShelfName} / {deviceInfo.DeviceName} / Tags Count ={deviceInfo.ReadHasTagsCounter} Update DB Finished");
                    //deviceInfo.ReadHasTagsCounter = 0;
                    //var tags = deviceInfo.RfidReadingTagsList.Keys.ToList();
                    //deviceInfo.RfidReadingTagsList.Clear();

                    //await rfidScannerService.ItemInOutShelfProcessAsync(tags, axPos.Tag.ToString());

                }

                //update to view 
                //Console.WriteLine("Update to screen");
                logger.Info("Update to screen");
                //UpdateToListView(tagRfidReadingList);
                //UpdateToListViewByScannerName(tagRfidReadingList, selectedScannerName);
                //UpdateToListViewByScannerName(deviceInfo.RfidReadingTagsList, selectedScannerName);
                UpdateToListViewByScannerName(deviceInfo.RfidReadingTagsList, "");

                List_Event.Items.Add($">>DataEvent End TagCount=" + TagCount.ToString());
                List_Event.SelectedIndex = List_Event.Items.Count - 1;

                // DataEventEnabled=True for next DataEvent
                axPos.DataEventEnabled = true;

                logger.Info($"↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑-Device name: {deviceInfoFromtagProp.ShelfName} { deviceInfoFromtagProp.DeviceName} DataEvent End-↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑");
            }
            catch(Exception ex)
            {
                logger.Error($"DataEventHandler error : { ex.Message} ");
            }
            
        }

        private void InsertRawRfidReaded(List<RfidView> readedRawData)
        {
            Task.Run(() =>
            {
                string sql = "";
                string InsertIntoHeadSql = $"INSERT INTO drfid_raw_data(drd_rfid_cd, drd_anten_no, drd_rssi, drd_shelf_no) VALUES";
                int i = 0;



                if (readedRawData.Count > 0)
                {
                    foreach (var item in readedRawData)
                    {
                        string ConvertedAntena = ConvertAntenNo(item.AntenNo);
                        i = i + 1;
                        if (i == readedRawData.Count) 
                        {
                            sql += $"('{item.Rfid}','{ConvertedAntena}', '{item.RSSI }', '{item.ShelfName}')";
                        }
                        else
                        {
                            sql += $"('{item.Rfid}','{ConvertedAntena}', '{item.RSSI }', '{item.ShelfName}'),";
                        }
                        
                    }

                    //create insert query
                    InsertIntoHeadSql = InsertIntoHeadSql + sql;
                    logger.Info($"InsertRawRfidReaded >> {InsertIntoHeadSql}");

                    //insert DB 
                    int result = rfidScannerService.InsertRawRfidData(InsertIntoHeadSql);
                }
            });
            
        }

        private string ConvertAntenNo(string value)
        {
            string[] data = ConfigFile.AntenaList.Split(',');
            for (int i = 0; i < Int64.Parse(ConfigFile.NumberOfAntena); i++)
            {
                string data_antena = data[i].Split(':').First();
                if (value == data_antena)
                {
                    return data[i].Split(':').Last();
                }
            }
            return value;
                

            //switch (value)
            //{
            //    case "0001":
            //        value = ConfigFile.Anten_No1;
            //        return value;
            //    case "0010":
            //        value = ConfigFile.Anten_No2;
            //        return value;
            //    case "0011":
            //        value = ConfigFile.Anten_No3;
            //        return value;
            //    case "0100":
            //        value = ConfigFile.Anten_No4;
            //        return value;
            //}
            //}
            //return value;

        }


        private void ReloadShelfPosMaster(string rfid)
        {
            try
            {
                if (!string.IsNullOrEmpty(ConfigFile.ReloadShelfPosMasterRfid))
                {
                    if (rfid.ToLower() == ConfigFile.ReloadShelfPosMasterRfid.ToLower())
                    {
                        //reload shelf position master 
                        rfidScannerService.RfidCurrentShelfProducts = GetRfidShelfProducts();
                        logger.Info($"Reload shelf position master finished");
                    }
                }
            }
            catch(Exception ex)
            {
                logger.Error($"Reload shelf position master error.{ex.Message}");
            }
        }

        private void ReloadShelfPosMasterWithoutRFID()
        {
            try
            {

                //reload shelf position master 
                rfidScannerService.RfidCurrentShelfProducts = GetRfidShelfProducts();
                logger.Info($"Reload shelf position master finished");
                                   
            }
            catch (Exception ex)
            {
                logger.Error($"Reload shelf position master error.{ex.Message}");
            }
        }

        private bool IsExistedRfidinShelf(string shelfName , string rfid)
        {
            if (ConfigFile.InOutItemDependOnShelf == "0")
                return true;

            //logger.Info($"shelfName = {shelfName}, rfid={rfid}");
            //logger.Info($"RfidCurrentShelfProducts count = {rfidScannerService.RfidCurrentShelfProducts.Count()}");
            var result = rfidScannerService.RfidCurrentShelfProducts.Where(p => p.Rfid?.ToLower() == rfid.ToLower() && p.ShelfName?.ToLower() == shelfName.ToLower());
            if (result == null)
            {
                logger.Info($"IsExistedRfidinShelf is false because cannot get {shelfName} /{rfid} data from drfid_product_pos ");
                return false;
            }
            //logger.Info($"result count = {result.Count()}");

            return result.Count() > 0;
        }
        private void UpdateUnReadingTags(ScannerDeviceInfo deviceInfo )
        {
            //var unReadingTags = deviceInfo.RfidUnReadingTagsList.Where(x => x.Value.ScannerDeviceName.ToLower() == deviceInfo.DeviceName.ToLower());
            var unReadingTags = deviceInfo.RfidUnReadingTagsList.Where(x => x.Value.ScannerDeviceName.ToLower() == deviceInfo.DeviceName.ToLower()
                                                                        && x.Value.ShelfName.ToLower() == deviceInfo.ShelfName.ToLower());
            foreach (var item in unReadingTags)
            {
                logger.Info($"UpdateUnReadingTags before update -- tag {deviceInfo.RfidUnReadingTagsList.First(x => x.Key == item.Key).Key} unreading count {deviceInfo.RfidUnReadingTagsList.First(x => x.Key == item.Key).Value.ReadCount}");
                bool  existedInReadingList = deviceInfo.RfidReadingTagsList.ContainsKey(item.Key);
                //var readingListWithReadCountLessCurrentCount = deviceInfo.RfidReadingTagsList.Values.Where(x => x.ReadCount < deviceInfo.ReadHasTagsCounter);


                if (existedInReadingList == false)
                    //if (deviceInfo.RfidReadingTagsList.ContainsKey(item.Key) == false)
                {
                    var updateItem = deviceInfo.RfidUnReadingTagsList.First(x => x.Key == item.Key);
                    
                    updateItem.Value.ReadCount = item.Value.ReadCount + 1;
                    logger.Info($"UpdateUnReadingTags -- not existedInReadingList tag {deviceInfo.RfidUnReadingTagsList.First(x => x.Key == item.Key).Key} unreading count {deviceInfo.RfidUnReadingTagsList.First(x => x.Key == item.Key).Value.ReadCount}");
                }
                else
                {
                    //existed in reading list => update ReadCount of unreading list to ReadHasTagsCounter - readed count
                    //   current reading        1               2                   3 
                    //Readed :     tag    readed_cnt_1    readed_cnt_2        readed_cnt_3
                    //              A           1               2               2
                    //              B           1               1               1
                    //              C           1               1               2

                    //UnReaded :   tag    readed_cnt_1    readed_cnt_2        readed_cnt_3
                    //              A           0 =1-1           0=2-2          1=3-2
                    //              B           0 =1-1           1=2-1          2=3-1
                    //              C           0 =1-1           1=2-1          1=3-2


                    deviceInfo.RfidUnReadingTagsList.First(x => x.Key == item.Key).Value.ReadCount = deviceInfo.ReadHasTagsCounter - deviceInfo.RfidReadingTagsList.First(x => x.Key == item.Key).Value.ReadCount;
                    logger.Info($"UpdateUnReadingTags -- existedInReadingList tag {deviceInfo.RfidUnReadingTagsList.First(x => x.Key == item.Key).Key} unreading count {deviceInfo.RfidUnReadingTagsList.First(x => x.Key == item.Key).Value.ReadCount}");
                }
            }
        }

        private async void HasTagsInOutStartUpdate(string shelfName , string scannerName)
        {
            try
            {

                var deviceInfo = GetDeviceInfoByName(shelfName, scannerName);
                if (deviceInfo != null)
                {
                    deviceInfo.ReadHasTagsCounter = 0;
                    //var tags = deviceInfo.RfidReadingTagsList.Keys.ToList();
                    //get max read count 
                    //int maxReadCount = 0;
                    //if(deviceInfo.RfidReadingTagsList.Count > 0)
                    //{
                    //    maxReadCount = deviceInfo.RfidReadingTagsList.Max(x => x.Value.ReadCount);
                    //}
                    //logger.Debug($"{shelfName} : {scannerName} : Max read count " + maxReadCount);

                    //var tagsEnum = deviceInfo.RfidReadingTagsList.Where(p => p.Value.ReadCount >= ConfigFile.ReadHasTagsCounter);
                    //320 items => readed in case readcount >=1 
                    IEnumerable<KeyValuePair<string, RfidView>> tagsEnum;

                    if (ConfigFile.IsReadingBigVolumnRfidsMode)
                    {
                        logger.Info($"{shelfName} : {scannerName} : reading big volume tags mode");
                        tagsEnum = deviceInfo.RfidReadingTagsList.Where(p => p.Value.ReadCount >= 1); // use for TABLE ( multi tags rfids )
                    }
                    else
                    {
                        logger.Info($"{shelfName} : {scannerName} : reading small volume tags mode");
                        tagsEnum = deviceInfo.RfidReadingTagsList.Where(p => p.Value.ReadCount >= ConfigFile.ReadHasTagsCounter); // use only SHELF ( small vol tags only)

                    }

                    if (tagsEnum.Count() > 0)
                    {
                        logger.Info($"Readed tags count > 0 -- HasTagsInOutStartUpdate will update {tagsEnum.Count()} records");

                        var tags = tagsEnum.ToDictionary(p => p.Key).Keys.ToList();

                        deviceInfo.RfidReadingTagsList.Clear();

                        ////append some tag in unreading list
                        //var unReadTagsEnum = deviceInfo.RfidUnReadingTagsList.Where(p => p.Value.ReadCount < ConfigFile.ReadHasTagsCounter);
                        //foreach (var item in unReadTagsEnum)
                        //{
                        //    var existedTag = tags.Where(x => x == item.Key);
                        //    if (existedTag.Count() == 0)
                        //    {
                        //        //add to unreading tags list
                        //        tags.Add(item.Key);
                        //        logger.Debug($"HasTagsInOutStartUpdate append to reading tags list item >> {item.Key}");
                        //    }

                        //}

                        //Tags in unreading list 
                        var unReadTagsEnum = deviceInfo.RfidUnReadingTagsList.Where(p => p.Value.ReadCount >= ConfigFile.ReadHasTagsCounter);
                        //Excep tags already existed in readed tags list
                        unReadTagsEnum = unReadTagsEnum.Where(p => !tags.Contains(p.Key.ToLower()));

                        var unReadTags = new List<string>();
                        foreach (var item in unReadTagsEnum)
                        {
                            var existedTag = unReadTags.Where(x => x == item.Key);


                            if (existedTag.Count() == 0)
                            {
                                //add to unreading tags list
                                unReadTags.Add(item.Key);
                                logger.Info($"HasTagsInOutStartUpdate unreading tags list item >> {item.Key}");
                            }

                        }

                        deviceInfo.RfidUnReadingTagsList.Clear();
                        deviceInfo.RfidUnReadingTagsList = InitShelfProductToRfidDict(rfidScannerService.RfidCurrentShelfProducts);

                        await rfidScannerService.ItemInOutShelfProcessAsync(tags, unReadTags, shelfName, scannerName);
                    }
                    else
                    {
                        logger.Info($"Readed tags count = 0 -- HasTagsInOutStartUpdate will update {tagsEnum.Count()} records");

                        deviceInfo.RfidReadingTagsList.Clear();
                        //update unreading tags list
                        ////append some tag in unreading list
                        //var tags = new List<string>();
                        //var unReadTagsEnum = deviceInfo.RfidUnReadingTagsList.Where(p => p.Value.ReadCount < ConfigFile.ReadHasTagsCounter);
                        //foreach (var item in unReadTagsEnum)
                        //{
                        //    var existedTag = tags.Where(x => x == item.Key);
                        //    if (existedTag.Count() == 0)
                        //    {
                        //        //add to unreading tags list
                        //        tags.Add(item.Key);
                        //        logger.Debug($"HasTagsInOutStartUpdate append to reading tags list item >> {item.Key}");
                        //    }

                        //}

                        //Tags in unreading list 
                        var unReadTagsEnum = deviceInfo.RfidUnReadingTagsList.Where(p => p.Value.ReadCount >= ConfigFile.ReadHasTagsCounter);
                        var unReadTags = new List<string>();
                        foreach (var item in unReadTagsEnum)
                        {
                            var existedTag = unReadTags.Where(x => x == item.Key);
                            if (existedTag.Count() == 0)
                            {
                                //add to unreading tags list
                                unReadTags.Add(item.Key);
                                logger.Info($"HasTagsInOutStartUpdate unreading tags list item >> {item.Key}");
                            }

                        }

                        deviceInfo.RfidUnReadingTagsList.Clear();
                        deviceInfo.RfidUnReadingTagsList = InitShelfProductToRfidDict(rfidScannerService.RfidCurrentShelfProducts);

                        await rfidScannerService.ItemInOutShelfProcessAsync(new List<string>(), unReadTags, shelfName, scannerName);
                    }

                }
            }
            catch(Exception ex)
            {
                logger.Error($"HasTagsInOutStartUpdate error : {ex.Message}");
                ShowStatusInformation($"Update processing failed.{ex.Message}");
            }
        }

        //private  void ResetScannerDeviceByName(string scannerName)
        //{
        //    var deviceInfo = GetDeviceInfoByName(scannerName);
        //    if (deviceInfo != null)
        //    {
        //        logger.Debug($"{deviceInfo.DeviceName} Reset to init value ");
        //        deviceInfo.ReadEmptyTagCounter = 0;
        //        deviceInfo.ReadHasTagsCounter = 0;
        //        deviceInfo.RfidReadingTagsList = new Dictionary<string, RfidView>();
        //        deviceInfo.ReadEmptyTagTimer = new AlarmTimer();
        //        deviceInfo.ReadHasTagsTimer = new AlarmTimer();

        //        //reload master data
        //        //get shelf product from DB
        //        rfidScannerService.RfidCurrentShelfProducts = GetRfidShelfProducts();

        //        RfidScannerService.RfidShelfLogTableLatest = rfidScannerService.GetLatestRfidShelfLogs();
        //    }
        //}

        private void ResetScannerDeviceByName(string shelfName , string scannerName )
        {
            var deviceInfo = GetDeviceInfoByName(shelfName,scannerName);
            if (deviceInfo != null)
            {
                logger.Info($"{deviceInfo.ShelfName}/{deviceInfo.DeviceName} Reset to init value ");
                deviceInfo.ReadEmptyTagCounter = 0;
                deviceInfo.ReadHasTagsCounter = 0;
                deviceInfo.RfidReadingTagsList = new Dictionary<string, RfidView>();
                deviceInfo.ReadEmptyTagTimer = new AlarmTimer();
                deviceInfo.ReadHasTagsTimer = new AlarmTimer();

                //reload master data
                //get shelf product from DB
                rfidScannerService.RfidCurrentShelfProducts = GetRfidShelfProducts();

                RfidScannerService.RfidShelfLogTableLatest = rfidScannerService.GetLatestRfidShelfLogs();
            }
        }


        private async Task<int> ShelfProductAppendMode(Dictionary<string, RfidView> readingTags , List<RfidShelfProduct> rfidShelfProducts , string shelfName , string scannerName)
        {
            int result = 0;
            if(ConfigFile.ShelfProductAppendMode == 1)
            {
                //var diffTags = Ultil.SrcNotInDesList(readingTags.Keys.ToList(), rfidShelfProducts?.Select(x => x.Rfid).ToList());
                var diffTags =readingTags.Keys.ToList();
                if (diffTags?.Count > 0)
                {
                    logger.Info($"ShelfProductAppendModeReset action : insert /update DB ");
                    var res =  await InsertToRfidShelfProduct(diffTags , shelfName, scannerName);

                    //get shelf product from DB
                    if (res > 0)
                        rfidScannerService.RfidCurrentShelfProducts = GetRfidShelfProducts();

                }
            }

            return result;
        }

        private List<RfidShelfProduct>  GetRfidShelfProducts()
        {
            return rfidScannerService.GetRfidShelfProducts();
        }

        private async Task<int> InsertToRfidShelfProduct(List<string> tags, string shelfName, string scannerName)
        {
            int result = 0;
            try
            {
                List<RfidShelfProduct> rfidShelfProducts = new List<RfidShelfProduct>();
                foreach (var tag in tags)
                {
                    RfidShelfProduct rfidShelfProduct = new RfidShelfProduct();

                    //get item other info from api
                    //rfid to jan 
                    logger.Info($"InsertToRfidShelfProduct action : call GetJanByRfid api with rfid : {tag}");
                    var janIfo = await rfidShelfHttpService.GetJanByRfid(tag);
                    //jan get item(book) info
                    if (janIfo != null)
                    {
                        if (janIfo.Data?.jancode_1 != null)
                        {
                            logger.Info($"InsertToRfidShelfProduct action : call GetItemInfoByJanCode api with jancode : {janIfo.Data?.jancode_1}");
                            var itemInfo = await rfidShelfHttpService.GetItemInfoByJanCode(janIfo.Data?.jancode_1);

                            if (itemInfo != null)
                            {
                                rfidShelfProduct.ProductName = itemInfo.Data[0]?.drgm_goods_name;
                                rfidShelfProduct.JanCd = itemInfo.Data[0]?.drgm_jan;
                            }
                        }
                        
                    }

                    rfidShelfProduct.Rfid = tag;
                    rfidShelfProduct.ShelfName = shelfName;
                    rfidShelfProduct.ScannerName = scannerName;

                    rfidShelfProducts.Add(rfidShelfProduct);
                }

                if (rfidShelfProducts?.Count > 0)
                {
                    logger.Info($"InsertToRfidShelfProduct action: call insert/update DB function");
                    result = rfidShelfProductService.InsertShelftProductToDb(rfidShelfProducts, false);
                }
            }
            catch(Exception ex)
            {
                logger.Error($"InsertToRfidShelfProduct action error {ex.Message}");
            }
            
            return result;
        }


        private void PhaseChange(AxOPOSRFID axPos)
        {
            // Phaseチェックボックス
            int Result;
            int intData;
            string strData;
            var deviceInfoFromtagProp = GetDeviceInfoObjectFromTagProp(axPos);

            // DirectIOを使用して位相の有効／無効を制御する
            //if (CheckBox_Phase.Checked)
            var checkPhase = scannerDevices.FirstOrDefault(x => x.DeviceName.ToLower() == deviceInfoFromtagProp.DeviceName.ToLower()
            && x.ShelfName.ToLower() == deviceInfoFromtagProp.ShelfName.ToLower())?.CheckBox_Phase;
            if (checkPhase.HasValue && checkPhase.Value == true)
                {
                // 位相を有効にするDirectIOを実行する
                intData = 224;
                strData = "";
                Result = axPos.DirectIO(116, ref intData, ref strData);
                if (Result == OposConstants.OposEBusy)
                {
                    //MessageBox.Show("読み取り中です。StopReadTagsを実行してください");
                    logger.Error($"PhaseChange : 読み取り中です。StopReadTagsを実行してください");
                }
                else if (Result == OposConstants.OposEIllegal)
                {
                    //MessageBox.Show("共存できない機能を使用している可能性があります");
                    logger.Error($"PhaseChange : 共存できない機能を使用している可能性があります");
                }
                else if (Result != OposConstants.OposSuccess)
                {
                    //MessageBox.Show("位相設定失敗しました");
                    logger.Error($"PhaseChange : 位相設定失敗しました");
                }
            }
            else
            {
                // 位相を無効にするDirectIOを実行する
                intData = 0;     // 未使用
                strData = "";    // 未使用
                Result = axPos.DirectIO(116, ref intData, ref strData);
                if (Result == OposConstants.OposEBusy)
                {
                    //MessageBox.Show("読み取り中です。StopReadTagsを実行してください");
                    logger.Error($"PhaseChange : 読み取り中です。StopReadTagsを実行してください");
                }
                else if (Result == OposConstants.OposEIllegal)
                {
                    //MessageBox.Show("共存できない機能を使用している可能性があります");
                    logger.Error($"PhaseChange : 共存できない機能を使用している可能性があります");
                }
                else if (Result != OposConstants.OposSuccess)
                {
                    //MessageBox.Show("位相設定失敗しました");
                    logger.Error($"PhaseChange : 位相設定失敗しました");
                }
            }
        }
        private void AddItemToList(RfidView data)
        {
            if (olvReceivedTagsData.Items.Count == AppConstants.MAX_ROW_DISPLAY)
                olvReceivedTagsData.Items.Clear();

            ListViewItem newItem = new ListViewItem();

            var orgShelf = rfidScannerService.RfidCurrentShelfProducts.Where(p => p.Rfid.ToLower() == data.Rfid.ToLower());
            string productName = orgShelf == null || orgShelf?.Count() == 0 ? "" : "(" + orgShelf.First().ProductName + ")";

            newItem.Text = data.Rfid + (productName=="" ? "" : productName);
            newItem.SubItems.Add(orgShelf ==null || orgShelf?.Count() ==0  ?data.ShelfName: orgShelf.First().ShelfName);
            newItem.SubItems.Add(data.ScannerDeviceName);
            newItem.SubItems.Add(data.AntenNo);
            newItem.SubItems.Add(data.RSSI);

            olvReceivedTagsData.Items.Add(newItem);

            //scroll down
            olvReceivedTagsData.Items[olvReceivedTagsData.Items.Count - 1].EnsureVisible();
        }

        private void UpdateToListViewByScannerName(Dictionary<string, RfidView> tagRfidReadList , string deviceName)
        {

            var filterByTagDeviceName = tagRfidReadList;
            if(!string.IsNullOrEmpty(deviceName))
                filterByTagDeviceName = tagRfidReadList.Where(p => p.Value.ScannerDeviceName.ToUpper() == deviceName.ToUpper()).ToDictionary(x=> x.Key , x=> x.Value);

            foreach (var tagRfid in filterByTagDeviceName)
            {
                //AddItemToList(rfidStatus);
                AddItemToList(tagRfid.Value);

            }
            //show tag count
            UpdateTagCountDisplay();
        }

        private void UpdateTagCountDisplay()
        {
            txtTagCount.Text = olvReceivedTagsData.Items.Count.ToString();
        }

        private void ShowStatusInformation(string msg)
        {
            toolStripStatusInfo.Text = "";
            if (msg != null)
            {
                toolStripStatusInfo.Text = msg;
            }
            
        }

        #endregion

        #region"Device status list view"
        protected void ListViewInitialize(List<ScannerDeviceInfo> scannerDevices)
        {

            this.SetupColumns();
            this.SetupColumnEnableButton();
            this.SetupColumnDisableButton();
            this.SetupColumnStartReadingTagsButton();
            this.SetupColumnStopReadingTagsButton();


            // How much space do we want to give each row? Obviously, this should be at least
            // the height of the images used by the renderer
            this.olvScannerDevices.ShowGroups = false;

            this.olvScannerDevices.RowHeight = 30;
            this.olvScannerDevices.SmallImageList = imageList;
            this.olvScannerDevices.EmptyListMsg = "No device";
            this.olvScannerDevices.UseAlternatingBackColors = false;
            this.olvScannerDevices.UseHotItem = false;

            // Make and display a list of tasks
            this.olvScannerDevices.SetObjects(scannerDevices);
        }

        private void ListViewDataRefresh(List<ScannerDeviceInfo>  scannerDeviceInfos)
        {
            this.olvScannerDevices.SetObjects(scannerDeviceInfos);
        }

        private void SetupColumnEnableButton()
        {

            this.olvColumnEnableDevice.IsButton = true;

            // How will the button be sized? That can either be:
            //   - FixedBounds. Each button is ButtonSize in size
            //   - CellBounds. Each button is as wide as the cell, inset by CellPadding
            //   - TextBounds. Each button resizes to match the width of the text plus ButtonPadding
            this.olvColumnEnableDevice.ButtonSizing = OLVColumn.ButtonSizingMode.FixedBounds;
            this.olvColumnEnableDevice.ButtonSize = new Size(80, 26);

            // Make the buttons clickable even if the row itself is disabled
            //this.olvColumnEnableAction.EnableButtonWhenItemIsDisabled = true;
            //this.olvColumnEnableAction.AspectName = "NextAction";
            this.olvColumnEnableDevice.TextAlign = HorizontalAlignment.Center;

            // Listen for button clicks -- which for the purpose of the demo will cycle the state of the service task
            this.olvScannerDevices.ButtonClick += delegate (object sender, CellClickEventArgs e) {
                //Console.WriteLine(String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model));

                if(e.SubItem.Text == "Enable")
                {
                    //get device name 
                    string deviceName = ((ScannerDeviceInfo)e.Model).DeviceName;
                    string shelfName = ((ScannerDeviceInfo)e.Model).ShelfName;

                    
                    //var ctl = axPosArray.FirstOrDefault(c => c.Tag.ToString() == deviceName);
                    var ctl = GetOposByName(shelfName, deviceName);

                    if (ctl != null)
                    {
                        if (ctl.DeviceEnabled)
                        {
                            MessageBox.Show($"{shelfName}/{deviceName} : Rfid scanner device was enabled.", captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        bool isSuccess = EnableRfidScannerDeviceHandler(ctl);
                        if (isSuccess == false)
                        {
                            MessageBox.Show($"{shelfName}/{deviceName} : Cannot enable rfid scanner device.", captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        ResetScannerDeviceByName(shelfName , deviceName);
                    }
                        
                }
                

                //Coordinator.ToolStripStatus1 = String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model);

                //// We only have one column with a button, but if there was more than one, you would have to check ColumnIndex to see which button was clicked
                //ServiceTask task = (ServiceTask)e.Model;
                //task.AdvanceToNextState();

                //// Just to show off disabled rows, make tasks that are frozen be disabled.
                //if (task.Status == ServiceTask.TaskStatus.Frozen)
                //    this.olvTasks.DisableObject(e.Model);
                //else
                //    this.olvTasks.EnableObject(e.Model);

                //this.olvTasks.RefreshObject(e.Model);


            };


        }

        private void SetupColumnDisableButton()
        {

            this.olvColumnDisableDevice.IsButton = true;
            this.olvColumnDisableDevice.ButtonSizing = OLVColumn.ButtonSizingMode.FixedBounds;
            this.olvColumnDisableDevice.ButtonSize = new Size(80, 26);
            this.olvColumnDisableDevice.TextAlign = HorizontalAlignment.Center;

            // Listen for button clicks -- which for the purpose of the demo will cycle the state of the service task
            this.olvScannerDevices.ButtonClick += delegate (object sender, CellClickEventArgs e) {
                Console.WriteLine(String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model));
                //Coordinator.ToolStripStatus1 = String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model);

                if (e.SubItem.Text == "Disable")
                {
                    //get device name 
                    string deviceName = ((ScannerDeviceInfo)e.Model).DeviceName;
                    string shelfName = ((ScannerDeviceInfo)e.Model).ShelfName;


                    //var ctl = axPosArray.FirstOrDefault(c => c.Tag.ToString() == deviceName);
                    var ctl = GetOposByName(shelfName, deviceName);

                    if (ctl != null)
                    {
                        if (ctl.DeviceEnabled == false)
                        {
                            MessageBox.Show($"{shelfName}/{deviceName} : Rfid scanner device was disabled.", captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        bool isSuccess = DisableRfidScannerDeviceHandler(ctl);
                        if (isSuccess == false)
                        {
                            MessageBox.Show($"{shelfName}/{deviceName} : Cannot disable rfid scanner device.", captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        //update remain data
                        var deviceInfo = GetDeviceInfoByName(shelfName, deviceName);
                        if (deviceInfo != null)
                        {
                            if (deviceInfo.ReadHasTagsCounter >= (AppConstants.PosReadTimerInterval * ConfigFile.ReadHasTagsCounter) / 1000)
                            {
                                logger.Info($"{deviceInfo.ShelfName} /{deviceInfo.DeviceName} Stop ReadHasTagsCounter : " + deviceInfo.ReadHasTagsCounter.ToString());
                                HasTagsInOutStartUpdate(deviceInfo.ShelfName , deviceInfo.DeviceName);
                            }
                        }
                    }
                }

            };

        }

        private void SetupColumnStartReadingTagsButton()
        {

            this.olvColumnStartReadingTags.IsButton = true;
            this.olvColumnStartReadingTags.ButtonSizing = OLVColumn.ButtonSizingMode.FixedBounds;
            this.olvColumnStartReadingTags.ButtonSize = new Size(80, 26);
            this.olvColumnStartReadingTags.TextAlign = HorizontalAlignment.Center;

            // Listen for button clicks -- which for the purpose of the demo will cycle the state of the service task
            this.olvScannerDevices.ButtonClick += delegate (object sender, CellClickEventArgs e) {

                //Console.WriteLine(String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model));
                if (e.SubItem.Text == "Start")
                {
                    //get device name 
                    string deviceName = ((ScannerDeviceInfo)e.Model).DeviceName;
                    string shelfName = ((ScannerDeviceInfo)e.Model).ShelfName;

                    //var ctl = axPosArray.FirstOrDefault(c => c.Tag.ToString() == deviceName);
                    var ctl = GetOposByName(shelfName, deviceName);

                    if (ctl != null)
                    {
                        ResetScannerDeviceByName(shelfName, deviceName);
                        var result = StartReadingDataHandler(ctl);
                        if (result)
                        {
                            SetDeviceStatus(shelfName,deviceName, DeviceStatus.Ready);
                        }
                        else
                        {
                            SetDeviceStatus(shelfName,deviceName, DeviceStatus.NotAvailable);
                        }
                    }
                }

            };

        }

        private void SetupColumnStopReadingTagsButton()
        {
            this.olvColumnStopReadingTags.IsButton = true;
            this.olvColumnStopReadingTags.ButtonSizing = OLVColumn.ButtonSizingMode.FixedBounds;
            this.olvColumnStopReadingTags.ButtonSize = new Size(80, 26);
            this.olvColumnStopReadingTags.TextAlign = HorizontalAlignment.Center;

            // Listen for button clicks -- which for the purpose of the demo will cycle the state of the service task
            this.olvScannerDevices.ButtonClick += delegate (object sender, CellClickEventArgs e) {
                //Console.WriteLine(String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model));

                if (e.SubItem.Text == "Stop")
                {
                    //get device name 
                    string deviceName = ((ScannerDeviceInfo)e.Model).DeviceName;
                    string shelfName = ((ScannerDeviceInfo)e.Model).ShelfName;
                    //var ctl = axPosArray.FirstOrDefault(c => c.Tag.ToString() == deviceName);
                    var ctl = GetOposByName(shelfName, deviceName);

                    if (ctl != null)
                    {
                        var deviceInfo = GetDeviceInfoByName(shelfName,deviceName);
                        if (deviceInfo != null)
                        {
                            if (deviceInfo.ReadHasTagsCounter >= (AppConstants.PosReadTimerInterval * ConfigFile.ReadHasTagsCounter) / 1000)
                            {
                                logger.Info($"{deviceInfo.ShelfName}/{deviceInfo.DeviceName} Stop ReadHasTagsCounter : " + deviceInfo.ReadHasTagsCounter.ToString());
                                HasTagsInOutStartUpdate(deviceInfo.ShelfName, deviceInfo.DeviceName);
                            }
                        }
                        
                        var result = StopReadingDataHandler(ctl);
                        if (result)
                        {
                            SetDeviceStatus(shelfName, deviceName, DeviceStatus.NotAvailable);
                        }
                        
                    }
                }

            };

        }

        private void SetupColumns()
        {
            //this.olvData.OwnerDraw = true;

            //var imgUnActive = new Bitmap(32, 32);
            //var g1 = Graphics.FromImage(imgUnActive);
            //g1.FillRectangle(new SolidBrush(Color.Red), 2, 2, 24, 24);

            //var imgActive = new Bitmap(32, 32);
            //var g2 = Graphics.FromImage(imgActive);
            //g2.FillRectangle(new SolidBrush(Color.Blue), 2, 2, 24, 24);


            this.olvColumnStatus.AspectName = "";
            this.olvColumnStatus.ImageGetter = delegate (object model) {
                ScannerDeviceInfo dvi = (ScannerDeviceInfo)model;
                switch (dvi.DeviceStatus)
                {
                    case DeviceStatus.Ready:
                        return 1;
                    case DeviceStatus.NotAvailable:
                        return 0;
                    default:
                        return null;
                }
            };
        }

        #endregion

        #region "Test"

        private void btnPutPopDbTest_Click(object sender, EventArgs e)
        {
            RfidShelfDbServiceTest rfidShelfDbServiceTest = new RfidShelfDbServiceTest();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            //RfidShelfProductServiceTest rfidShelfProductServiceTest = new RfidShelfProductServiceTest();
            var form = new FormImportShelfProduct();
            form.Show();
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            List<string> tagsList = new List<string>();
            string line;

            using (StringReader reader = new StringReader(txtRfidTagTestList.Text))
            {

                while ((line = reader.ReadLine()) != null)
                    tagsList.Add(line);
            }

            //string[] tagsReaded = txtRfidTagTestList.Text.Split(new string[] { Environment.NewLine}, StringSplitOptions.None);
            //string[] tagsReaded = txtRfidTagTestList.Text.Split('\n');

            rfidScannerService.RfidCurrentShelfProducts = rfidScannerService.GetRfidShelfProducts();
            rfidScannerService.ItemInOutShelfProcess(tagsList);

        }
        private void button4_Click(object sender, EventArgs e)
        {
            Random rd = new Random();

            int rdn = rd.Next(scannerDevices.Count);

            int rdStatus = rd.Next(2);

            scannerDevices[rdn].DeviceStatus = (DeviceStatus)rdStatus;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                var reOrderForm = new FormShelfProductsView();
                reOrderForm.Show();
            }catch(Exception ex)
            {
                logger.Error($"FormShelfProductsView cannot load." + ex.InnerException !=null?ex.InnerException?.Message :ex.Message);
                MessageBox.Show(Ultil.GetSystemErrorMsg(ex), captionFormName, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }
        private async void btnInsertLogTest_Click(object sender, EventArgs e)
        {
            //insert into product position
            //InsertShelfProductTest();

            //IN
            List<string> tags = new List<string>();
            //create list 
            for (int i = 1; i <= 320; i++)
            {
                tags.Add("RFID_" + i);
            }
            //in out shelf process
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("Exec IN Start ");
            await rfidScannerService.ItemInOutShelfProcessAsync(tags, new List<string>(), "TABLE", "TECRFIDSCANNERUSB5");
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Exec IN End ");
            Console.WriteLine("Exec IN time : " + elapsedMs);
        }

        private async void button5_Click(object sender, EventArgs e)
        {
            //OUT
            List<string> tags = new List<string>();
            //create list 
            for (int i = 1; i <= 320; i++)
            {
                tags.Add("RFID_" + i);
            }
            //in out shelf process
            var watch = System.Diagnostics.Stopwatch.StartNew();
            Console.WriteLine("Exec OUT Start ");
            await rfidScannerService.ItemInOutShelfProcessAsync(new List<string>(), tags, "TABLE", "TECRFIDSCANNERUSB5");
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine("Exec OUT End ");
            Console.WriteLine("Exec OUT time : " + elapsedMs);
        }

        private int InsertShelfProductTest()
        {
            int idx = 1;
            int result = 0;
            List<RfidShelfProduct> rfidShelfProducts = new List<RfidShelfProduct>();
            for(int row =1; row <=4; row++)
            {
                for (int col = 1; col <= 8; col++)
                {
                    for (int i = 1; i <= 10; i++)
                    {
                        rfidShelfProducts.Add(new RfidShelfProduct
                        {
                            Rfid = "RFID_" + idx,
                            ProductName = "RFID_" + idx,
                            ScannerName = "TECRFIDSCANNERUSB5",
                            ShelfName ="TABLE",
                            IsbnCode = "4774148911",
                            ShelfNo = row,
                            ShelfColIndex = col
                        }) ;
                        idx++;
                    }
                }
            }
            //insert to DB
            //result = rfidShelfProductService.InsertShelftProductToDb(rfidShelfProducts);
            return result;
        }

        #endregion

        #region"UI"
        //Structs
        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(172, 126, 241);
            public static Color color2 = Color.FromArgb(249, 118, 176);
            public static Color color3 = Color.FromArgb(253, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
        }
        //Methods
        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();
                //Button
                currentBtn = (IconButton)senderBtn;
                //currentBtn.BackColor = Color.FromArgb(37, 36, 81);
                currentBtn.BackColor = Color.FromArgb(245, 245, 255);
                currentBtn.ForeColor = color;
                currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentBtn.IconColor = color;
                currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                //Left border button
                leftBorderBtn.BackColor = color;
                leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                leftBorderBtn.Visible = true;
                leftBorderBtn.BringToFront();
                //Current Child Form Icon
                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                iconCurrentChildForm.IconColor = color;
            }
        }
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                //currentBtn.BackColor = Color.FromArgb(31, 30, 68);
                //System.Drawing.SystemColors.ButtonFace;
                currentBtn.BackColor = Color.FromArgb(245, 245, 255);
                currentBtn.ForeColor = Color.Black;
                currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentBtn.IconColor = Color.Gray;
                currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
            }
        }
        private void OpenChildForm(Form childForm)
        {
            
            //open only form
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
            currentChildForm = childForm;
            //End
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            panelDesktop.Controls.Add(childForm);
            panelDesktop.Tag = childForm;

            childForm.BringToFront();
            childForm.Show();
            lblTitleChildForm.Text = childForm.Text;
        }
        private void btnRfidScanner_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color1);
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color2);
            if(currentChildForm!=null)
                if (((IconButton)sender).Tag.ToString().ToLower() == currentChildForm.Text.ToLower())
                    return;

            OpenChildForm(new FormImportShelfProduct());
        }

        private void btnLogView_Click(object sender, EventArgs e)
        {
            ActivateButton(sender, RGBColors.color3);
            if (currentChildForm != null)
                if (((IconButton)sender).Tag.ToString().ToLower() == currentChildForm.Text.ToLower())
                    return;

            OpenChildForm(new FormShelfLog());
        }

        private void btnHome_Click(object sender, EventArgs e)
        {
            Reset();
        }
        private void Reset()
        {
            DisableButton();
            leftBorderBtn.Visible = false;
            iconCurrentChildForm.IconChar = IconChar.Home;
            iconCurrentChildForm.IconColor = Color.MediumPurple;
            lblTitleChildForm.Text = "Home";
            if (currentChildForm != null)
            {
                currentChildForm.Close();
            }

        }


        #endregion

       
    }
}
