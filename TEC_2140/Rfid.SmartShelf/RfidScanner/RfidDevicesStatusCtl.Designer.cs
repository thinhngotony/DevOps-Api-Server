
namespace Vjp.Rfid.SmartShelf.RfidScanner
{
    partial class RfidDevicesStatusCtl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RfidDevicesStatusCtl));
            this.panelMain = new System.Windows.Forms.Panel();
            this.olvData = new BrightIdeasSoftware.ObjectListView();
            this.olvColumnDeviceName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnStatus = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnEnableDevice = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.olvColumnDisableDevice = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnStartReadingTags = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumnStopReadingTags = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.panelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvData)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.olvData);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(604, 415);
            this.panelMain.TabIndex = 0;
            // 
            // olvData
            // 
            this.olvData.AllColumns.Add(this.olvColumnDeviceName);
            this.olvData.AllColumns.Add(this.olvColumnStatus);
            this.olvData.AllColumns.Add(this.olvColumnEnableDevice);
            this.olvData.AllColumns.Add(this.olvColumnDisableDevice);
            this.olvData.AllColumns.Add(this.olvColumnStartReadingTags);
            this.olvData.AllColumns.Add(this.olvColumnStopReadingTags);
            this.olvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvData.CellEditUseWholeCell = false;
            this.olvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumnDeviceName,
            this.olvColumnStatus,
            this.olvColumnEnableDevice,
            this.olvColumnDisableDevice,
            this.olvColumnStartReadingTags,
            this.olvColumnStopReadingTags});
            this.olvData.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvData.HideSelection = false;
            this.olvData.LargeImageList = this.imageList;
            this.olvData.Location = new System.Drawing.Point(3, 14);
            this.olvData.Name = "olvData";
            this.olvData.Size = new System.Drawing.Size(598, 364);
            this.olvData.SmallImageList = this.imageList;
            this.olvData.TabIndex = 0;
            this.olvData.UseCompatibleStateImageBehavior = false;
            this.olvData.View = System.Windows.Forms.View.Details;
            // 
            // olvColumnDeviceName
            // 
            this.olvColumnDeviceName.AspectName = "DeviceName";
            this.olvColumnDeviceName.Text = "Device Name";
            this.olvColumnDeviceName.Width = 160;
            // 
            // olvColumnStatus
            // 
            this.olvColumnStatus.AspectName = "DeviceStatus";
            this.olvColumnStatus.ImageAspectName = "";
            this.olvColumnStatus.Text = "Status";
            this.olvColumnStatus.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumnStatus.Width = 80;
            // 
            // olvColumnEnableDevice
            // 
            this.olvColumnEnableDevice.AspectName = "EnableDevice";
            this.olvColumnEnableDevice.Groupable = false;
            this.olvColumnEnableDevice.IsButton = true;
            this.olvColumnEnableDevice.Text = "Enable Device";
            this.olvColumnEnableDevice.Width = 128;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "unactive.png");
            this.imageList.Images.SetKeyName(1, "active.png");
            // 
            // olvColumnDisableDevice
            // 
            this.olvColumnDisableDevice.AspectName = "DisableDevice";
            this.olvColumnDisableDevice.IsButton = true;
            this.olvColumnDisableDevice.Text = "Disable";
            // 
            // olvColumnStartReadingTags
            // 
            this.olvColumnStartReadingTags.AspectName = "StartReadingTags";
            this.olvColumnStartReadingTags.IsButton = true;
            this.olvColumnStartReadingTags.Text = "Start";
            // 
            // olvColumnStopReadingTags
            // 
            this.olvColumnStopReadingTags.AspectName = "StopReadingTags";
            this.olvColumnStopReadingTags.Text = "Stop";
            // 
            // RfidDevicesStatusCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "RfidDevicesStatusCtl";
            this.Size = new System.Drawing.Size(604, 415);
            this.Load += new System.EventHandler(this.RfidDevicesStatusCtl_Load);
            this.panelMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private BrightIdeasSoftware.ObjectListView olvData;
        private BrightIdeasSoftware.OLVColumn olvColumnDeviceName;
        private BrightIdeasSoftware.OLVColumn olvColumnEnableDevice;
        private BrightIdeasSoftware.OLVColumn olvColumnStatus;
        private System.Windows.Forms.ImageList imageList;
        private BrightIdeasSoftware.OLVColumn olvColumnDisableDevice;
        private BrightIdeasSoftware.OLVColumn olvColumnStartReadingTags;
        private BrightIdeasSoftware.OLVColumn olvColumnStopReadingTags;
    }
}
