using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vjp.Rfid.SmartShelf.Constants;
using Vjp.Rfid.SmartShelf.Enums;
using Vjp.Rfid.SmartShelf.Helper;

namespace Vjp.Rfid.SmartShelf.Models
{
    public delegate void StatusChangedNotify();

    public class ScannerDeviceInfo : IDisposable
    {
        public string ShelfName { get; set; } = "";

        private DeviceStatus deviceStatus;
        public string DeviceName { get; set; }
        public DeviceStatus  DeviceStatus{ 
            get {
                return deviceStatus;
            }
            set {

                if (value != DeviceStatus)
                    OnDeviceStatusChangedNotify();

                deviceStatus = value;
            }
        }

        public string EnableDevice { get; set; } = "Enable";
        public string DisableDevice { get; set; } = "Disable";
        public string StartReadingTags { get; set; } = "Start";
        public string StopReadingTags { get; set; } = "Stop";
        public bool CheckBox_Phase { get; set; } = true;

        public AlarmTimer ReadEmptyTagTimer { get; set; }

        public AlarmTimer ReadHasTagsTimer { get; set; }

        public Dictionary<string, RfidView> RfidReadingTagsList { get; set; } = new Dictionary<string, RfidView>();
        public Dictionary<string, RfidView> RfidUnReadingTagsList { get; set; } = new Dictionary<string, RfidView>();
        public int ReadHasTagsCounter { get; set; } = 0;

        public int ReadEmptyTagCounter { get; set; } = 0;

        public event StatusChangedNotify DeviceStatusChangedNotify;
        public ScannerDeviceInfo()
        {
            ReadEmptyTagTimer = new AlarmTimer();
            ReadEmptyTagTimer.SetTime(AppConstants.ReadEmptyTagWaitTimeout);

            ReadHasTagsTimer = new AlarmTimer();
            ReadHasTagsTimer.SetTime(AppConstants.ReadHasTagsWaitTimeout);
        }

        public virtual void OnDeviceStatusChangedNotify()
        {
            DeviceStatusChangedNotify?.Invoke();
        }

        public void Dispose()
        {
            ReadEmptyTagTimer = null;
            ReadHasTagsTimer = null;
            DeviceStatusChangedNotify = null;
            RfidReadingTagsList.Clear();
        }
    }
}
