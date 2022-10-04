using AxOPOSRFIDLib;
using System;
using System.Collections.Generic;
using Vjp.Rfid.SmartShelf.Constants;
using Vjp.Rfid.SmartShelf.Enums;
using Vjp.Rfid.SmartShelf.Helper;

namespace Vjp.Rfid.SmartShelf.RfidScanner
{
    public class RfidScanner : IDisposable
    {
        private AxOPOSRFID axPosLib = null;

        //event when DataEvent read finished
        public Action DataEventFinished;
        public Action<object , _DOPOSRFIDEvents_DataEventEvent> DataEvent;
        public Action<object ,_DOPOSRFIDEvents_DirectIOEventEvent> DirectIOEvent;
        public Action<object, _DOPOSRFIDEvents_ErrorEventEvent> ErrorEvent;
        public Action<object, _DOPOSRFIDEvents_OutputCompleteEventEvent> OutputCompleteEvent;
        public Action<object, _DOPOSRFIDEvents_StatusUpdateEventEvent> StatusUpdateEvent;

        public delegate void DelegateFnc(string message);
        private DelegateFnc callbackFunction;

        public string OpenDeviceName { get; set; }
        private bool CheckBox_Phase { get; set; }

        private static Dictionary<string, string> currentReadTags = new Dictionary<string, string>();

        public RfidScanner()
        {
            CreateOposControl();
            //add event
            axPosLib.DataEvent += AxPosLib_DataEvent; ;
            axPosLib.DirectIOEvent += AxPosLib_DirectIOEvent; ;
            axPosLib.ErrorEvent += AxPosLib_ErrorEvent; ;
            axPosLib.OutputCompleteEvent += AxPosLib_OutputCompleteEvent; ;
            axPosLib.StatusUpdateEvent += AxPosLib_StatusUpdateEvent; ;
            
        }

        public bool EnableDevice()
        {
            return EnableRfidScannerDeviceHandler(); 
        }
        public bool DisableDevice()
        {
            return DisableRfidScannerDeviceHandler();
        }

        public bool ReadSingleTag()
        {
            return ReadSingleTagHandler();
        }

        public bool StartReadingTags()
        {
            return StartReadingDataHandler();
        }

        public bool StopReadTags()
        {
            return StopReadingDataHandler();
        }

        private void AxPosLib_StatusUpdateEvent(object sender, _DOPOSRFIDEvents_StatusUpdateEventEvent e)
        {
            //Console.WriteLine("AxPos_StatusUpdateEvent : " + e.status.ToString());
            StatusUpdateEvent?.Invoke(sender, e);
        }

        private void AxPosLib_OutputCompleteEvent(object sender, _DOPOSRFIDEvents_OutputCompleteEventEvent e)
        {
            OutputCompleteEvent?.Invoke(sender, e);
        }

        private void AxPosLib_ErrorEvent(object sender, _DOPOSRFIDEvents_ErrorEventEvent e)
        {
            ErrorEvent?.Invoke(sender , e);
        }

        private void AxPosLib_DirectIOEvent(object sender, _DOPOSRFIDEvents_DirectIOEventEvent e)
        {
            DirectIOEvent?.Invoke(sender , e);

            //List_Event.Items.Add("<<DirectIOEvent Start. eventNumber=" + e.eventNumber);
            //if (e.eventNumber == 1)
            //{
            //    // トリガSW状態通知
            //    List_Event.Items.Add(e.pData);
            //}
            //else if (e.eventNumber == 2)
            //{
            //    // バーコード読込通知
            //    List_Event.Items.Add(e.pString);
            //}
            //List_Event.Items.Add(">>DirectIOEvent End");
            //List_Event.SelectedIndex = List_Event.Items.Count - 1;
        }

        private void AxPosLib_DataEvent(object sender, _DOPOSRFIDEvents_DataEventEvent e)
        {
            DataEventHandler();
        }

        public  AxOPOSRFID GetPosControl()
        {
            return axPosLib;
        }

        public Dictionary<string,string> GetCurrentReadTags()
        {
            return currentReadTags;
        }

        private void CreateOposControl()
        {
            try
            {
                axPosLib = new AxOPOSRFID();
                axPosLib.CreateControl();
            }
            catch (Exception ex)
            {
                //error log
            }
        }
        private bool EnableRfidScannerDeviceHandler()
        {
            int Result;
            int phase;
            string strData;
            bool isSuccess = false;

            if (axPosLib.DeviceEnabled)
                return true;

            // Open Device
            Result = axPosLib.Open(OpenDeviceName);
            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"Open Device error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");
                return isSuccess; // Error
            }

            // ClaimDevice
            
            Result = axPosLib.ClaimDevice(3000);
            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"ClaimDevice error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");
                axPosLib.Close(); // Close for re-open
                return isSuccess; // Error
            }


            // DeviceEnabled=True

            axPosLib.DeviceEnabled = true;
            Result = axPosLib.ResultCode;
            
            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"DeviceEnabled=True error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");
                axPosLib.Close(); // Close for re-open
                return isSuccess; // Error
            }

            // DirectIOを用いて現在の位相状態を取得する
            phase = 0;
            strData = "";
            Result = axPosLib.DirectIO(115, ref phase, ref strData);
            if (Result == OposConstants.OposSuccess)
            {
                // 現在の位相の状態を画面に表示する。ただし、アンテナは常に有効になるので除外して判定する
                if (Convert.ToBoolean(phase) && phase != 128)
                {
                    CheckBox_Phase = true;
                }
                else
                {
                    CheckBox_Phase = false;
                }
            }

            // BinaryConversion=OPOS_BC_NIBBLE
            axPosLib.BinaryConversion = OposConstants.OposBcNibble;
            Result = axPosLib.ResultCode;
            
            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"BinaryConversion=OPOS_BC_NIBBLE error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");
                axPosLib.Close(); // Close for re-open
                return isSuccess; // Error
            }

            // Protocolmask=RFID_PR_EPC1G2
            axPosLib.ProtocolMask = OposConstants.RfidPrEpc1g2;
            Result = axPosLib.ResultCode;

            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"Protocolmask=RFID_PR_EPC1G2 error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");              
                axPosLib.Close(); // Close for re-open
                return isSuccess; // Error
            }

            //logger.Info($"Open decive {OpenDeviceName} successfully");

            return true;

        }

        private bool DisableRfidScannerDeviceHandler()
        {
            bool isSuccess = false;
            int Result;

            if (axPosLib.DeviceEnabled == false)
                return true;

            // DeviceEnabled=False
            axPosLib.DeviceEnabled = false;
            Result = axPosLib.ResultCode;
          
            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"DeviceEnabled=False error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");
                return isSuccess; // Error
            }

            // ReleaseDevice
            Result = axPosLib.ReleaseDevice();
            
            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"ReleaseDevice error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");

                return isSuccess; // Error
            }

            // Close Device
            Result = axPosLib.Close();
            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"Close error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");

                return isSuccess; // Error
            }

            //logger.Info("Close device {OpenDeviceName} successfully");
            return true;
        }

        private bool ReadSingleTagHandler()
        {
            int Result;

            currentReadTags.Clear();
            // ClearInputProperties
            axPosLib.ClearInputProperties();

            // DataEventEnabled=True
            axPosLib.DataEventEnabled = true;

            PhaseChange();

            //ReadTags : TagID only, no filter, timeout=5000, no password

            Result = axPosLib.ReadTags(OposConstants.RfidRtId, AppConstants.PosFilterId, AppConstants.PosFilterMask, 0, 0, AppConstants.PosStartReadTimeout, AppConstants.PosPassword);

            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"StartReadTags error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");
                return false;
            }
            return true;
        }

        private bool StartReadingDataHandler()
        {
            
            bool isSuccess = true;
            int Result;

            currentReadTags.Clear();
            
            // ClearInputProperties
            axPosLib.ClearInputProperties();

            // ReadTimerInterval=1000
            axPosLib.ReadTimerInterval = AppConstants.PosReadTimerInterval;

            // DataEventEnabled=True
            axPosLib.DataEventEnabled = true;

            PhaseChange();

            // StartReadTags : TagID only, no filter, timeout=5000, no password
            Result = axPosLib.StartReadTags(OposConstants.RfidRtId, AppConstants.PosFilterId, AppConstants.PosFilterMask, 0, 0, AppConstants.PosStartReadTimeout, AppConstants.PosPassword);
            
            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"StartReadTags error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");
                
                isSuccess = false;
                return isSuccess;
            }
            
            return isSuccess;

        }

        private bool StopReadingDataHandler()
        {
            int Result;
            bool isSuccess = true;

            // StopReadTags : no password
            Result = axPosLib.StopReadTags(AppConstants.PosPassword);
            if (Result != OposConstants.OposSuccess)
            {
                //logger.Error($"StopReadTags error.Result = {Result.ToString()} / ResultCode ={axPosLib.ResultCode.ToString()} / ResultCodeExtended={axPosLib.ResultCodeExtended.ToString()}");
                isSuccess = false;
            }
            //logger.Info("StopReadTags end");
            
            return isSuccess;
        }

        private void DataEventHandler()
        {
            string CurrentTagID;
            int TagCount;
            int LoopCnt;
            string UserData;

            //logger.Info("↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓-DataEvent Start-↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓");
            // TagCount
            TagCount = axPosLib.TagCount;

            //logger.Info("<<DataEvent Start TagCount=" + TagCount.ToString());

            var loopTo = TagCount;
            //remove all rfid
            currentReadTags.Clear();

            for (LoopCnt = 1; LoopCnt <= loopTo; LoopCnt++)
            {
                // CurrentTagID
                // UPGRADE_WARNING: オブジェクト CurrentTagID の既定プロパティを解決できませんでした。 詳細については、'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"' をクリックしてください。
                CurrentTagID = axPosLib.CurrentTagID;
                // CurrentTagUserData
                UserData = " Userdata=" + axPosLib.CurrentTagUserData;

                if (UserData == " Userdata=")
                {
                    UserData = "";
                }
                //logger.Info(Ultil.ConvertTagIDCode(axPosLib.CurrentTagID) + UserData);

                //logger.Info(" Userdata=" + axPosLib.CurrentTagUserData);
                //logger.Info(" CurrentTagID=" + CurrentTagID);
                //logger.Info(" Decode=" + Ultil.ConvertTagIDCode(CurrentTagID));

                //save to dictionary to remove duplicate data 
                if (currentReadTags.ContainsKey(Ultil.ConvertTagIDCode(CurrentTagID)) == false)
                {
                    currentReadTags.Add(Ultil.ConvertTagIDCode(CurrentTagID), CurrentTagID);
                }
                
                // NextTag
                axPosLib.NextTag();
            }
            //invoke delegate
            DataEventFinished?.Invoke();

            //DataEvent End
            // DataEventEnabled=True for next DataEvent
            axPosLib.DataEventEnabled = true;
            //logger.Info("↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑-DataEvent End-↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑");
        }

        private void PhaseChange()
        {
            // Phaseチェックボックス
            int Result;
            int intData;
            string strData;
            // DirectIOを使用して位相の有効／無効を制御する
            if (CheckBox_Phase)
            {
                // 位相を有効にするDirectIOを実行する
                intData = 224;
                strData = "";
                Result = axPosLib.DirectIO(116, ref intData, ref strData);
                if (Result == OposConstants.OposEBusy)
                {
                    //logger.Error($"PhaseChange : 読み取り中です。StopReadTagsを実行してください");
                }
                else if (Result == OposConstants.OposEIllegal)
                {
                    //logger.Error($"PhaseChange : 共存できない機能を使用している可能性があります");
                }
                else if (Result != OposConstants.OposSuccess)
                {
                    //logger.Error($"PhaseChange : 位相設定失敗しました");
                }
            }
            else
            {
                // 位相を無効にするDirectIOを実行する
                intData = 0;     // 未使用
                strData = "";    // 未使用
                Result = axPosLib.DirectIO(116, ref intData, ref strData);
                if (Result == OposConstants.OposEBusy)
                {
                    //logger.Error($"PhaseChange : 読み取り中です。StopReadTagsを実行してください");
                }
                else if (Result == OposConstants.OposEIllegal)
                {
                    //logger.Error($"PhaseChange : 共存できない機能を使用している可能性があります");
                }
                else if (Result != OposConstants.OposSuccess)
                {
                    //logger.Error($"PhaseChange : 位相設定失敗しました");
                }
            }
        }

        public void Dispose()
        {
            if (axPosLib != null)
                axPosLib.Dispose();


        }
    }
}
