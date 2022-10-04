
namespace Vjp.Rfid.SmartShelf.RfidScanner
{
    partial class RfidScannerCtl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RfidScannerCtl));
            this.axPos = new AxOPOSRFIDLib.AxOPOSRFID();
            this.panelDevice = new System.Windows.Forms.Panel();
            this.grbResult = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.CheckBox_Phase = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Text_ResultCodeExtended = new System.Windows.Forms.TextBox();
            this.Text_ResultCode = new System.Windows.Forms.TextBox();
            this.Text_Result = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Text_MethodName = new System.Windows.Forms.TextBox();
            this.picDeviceSignal = new System.Windows.Forms.PictureBox();
            this.Command_StopReading = new System.Windows.Forms.Button();
            this.Text_OpenName = new System.Windows.Forms.TextBox();
            this.lblDeviceName = new System.Windows.Forms.Label();
            this.Command_EnableDevice = new System.Windows.Forms.Button();
            this.Command_StartReading = new System.Windows.Forms.Button();
            this.Command_DisableDevice = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtTagCount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Text_ReadTagCount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.List_Event = new System.Windows.Forms.ListBox();
            this.lvData = new System.Windows.Forms.ListView();
            this.RFID = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelLeftTop = new System.Windows.Forms.Panel();
            this.panelLeftFooter = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.axPos)).BeginInit();
            this.panelDevice.SuspendLayout();
            this.grbResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeviceSignal)).BeginInit();
            this.panelMain.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelLeftTop.SuspendLayout();
            this.panelLeftFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // axPos
            // 
            this.axPos.Enabled = true;
            this.axPos.Location = new System.Drawing.Point(437, 12);
            this.axPos.Name = "axPos";
            this.axPos.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axPos.OcxState")));
            this.axPos.Size = new System.Drawing.Size(28, 28);
            this.axPos.TabIndex = 0;
            // 
            // panelDevice
            // 
            this.panelDevice.Controls.Add(this.axPos);
            this.panelDevice.Controls.Add(this.grbResult);
            this.panelDevice.Controls.Add(this.picDeviceSignal);
            this.panelDevice.Controls.Add(this.Command_StopReading);
            this.panelDevice.Controls.Add(this.Text_OpenName);
            this.panelDevice.Controls.Add(this.lblDeviceName);
            this.panelDevice.Controls.Add(this.Command_EnableDevice);
            this.panelDevice.Controls.Add(this.Command_StartReading);
            this.panelDevice.Controls.Add(this.Command_DisableDevice);
            this.panelDevice.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDevice.Location = new System.Drawing.Point(0, 0);
            this.panelDevice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panelDevice.Name = "panelDevice";
            this.panelDevice.Size = new System.Drawing.Size(1049, 54);
            this.panelDevice.TabIndex = 1;
            // 
            // grbResult
            // 
            this.grbResult.Controls.Add(this.label4);
            this.grbResult.Controls.Add(this.label3);
            this.grbResult.Controls.Add(this.CheckBox_Phase);
            this.grbResult.Controls.Add(this.label2);
            this.grbResult.Controls.Add(this.Text_ResultCodeExtended);
            this.grbResult.Controls.Add(this.Text_ResultCode);
            this.grbResult.Controls.Add(this.Text_Result);
            this.grbResult.Controls.Add(this.label1);
            this.grbResult.Controls.Add(this.Text_MethodName);
            this.grbResult.Location = new System.Drawing.Point(12, 62);
            this.grbResult.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grbResult.Name = "grbResult";
            this.grbResult.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grbResult.Size = new System.Drawing.Size(800, 117);
            this.grbResult.TabIndex = 4;
            this.grbResult.TabStop = false;
            this.grbResult.Text = "Result";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(466, 32);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(82, 20);
            this.label4.TabIndex = 3;
            this.label4.Text = "-Extended";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(362, 32);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "ResultCode";
            // 
            // CheckBox_Phase
            // 
            this.CheckBox_Phase.AutoSize = true;
            this.CheckBox_Phase.Location = new System.Drawing.Point(596, 57);
            this.CheckBox_Phase.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.CheckBox_Phase.Name = "CheckBox_Phase";
            this.CheckBox_Phase.Size = new System.Drawing.Size(73, 24);
            this.CheckBox_Phase.TabIndex = 3;
            this.CheckBox_Phase.Text = "Phase";
            this.CheckBox_Phase.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(256, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "Result";
            // 
            // Text_ResultCodeExtended
            // 
            this.Text_ResultCodeExtended.Location = new System.Drawing.Point(470, 57);
            this.Text_ResultCodeExtended.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Text_ResultCodeExtended.Name = "Text_ResultCodeExtended";
            this.Text_ResultCodeExtended.Size = new System.Drawing.Size(94, 26);
            this.Text_ResultCodeExtended.TabIndex = 2;
            // 
            // Text_ResultCode
            // 
            this.Text_ResultCode.Location = new System.Drawing.Point(364, 57);
            this.Text_ResultCode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Text_ResultCode.Name = "Text_ResultCode";
            this.Text_ResultCode.Size = new System.Drawing.Size(94, 26);
            this.Text_ResultCode.TabIndex = 2;
            // 
            // Text_Result
            // 
            this.Text_Result.Location = new System.Drawing.Point(260, 57);
            this.Text_Result.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Text_Result.Name = "Text_Result";
            this.Text_Result.Size = new System.Drawing.Size(94, 26);
            this.Text_Result.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 32);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Method(Prop) Name";
            // 
            // Text_MethodName
            // 
            this.Text_MethodName.Location = new System.Drawing.Point(27, 57);
            this.Text_MethodName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Text_MethodName.Name = "Text_MethodName";
            this.Text_MethodName.Size = new System.Drawing.Size(218, 26);
            this.Text_MethodName.TabIndex = 0;
            // 
            // picDeviceSignal
            // 
            this.picDeviceSignal.InitialImage = null;
            this.picDeviceSignal.Location = new System.Drawing.Point(358, 5);
            this.picDeviceSignal.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.picDeviceSignal.Name = "picDeviceSignal";
            this.picDeviceSignal.Size = new System.Drawing.Size(50, 48);
            this.picDeviceSignal.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picDeviceSignal.TabIndex = 2;
            this.picDeviceSignal.TabStop = false;
            // 
            // Command_StopReading
            // 
            this.Command_StopReading.Location = new System.Drawing.Point(903, 6);
            this.Command_StopReading.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Command_StopReading.Name = "Command_StopReading";
            this.Command_StopReading.Size = new System.Drawing.Size(138, 38);
            this.Command_StopReading.TabIndex = 3;
            this.Command_StopReading.TabStop = false;
            this.Command_StopReading.Text = "StopReading";
            this.Command_StopReading.UseVisualStyleBackColor = true;
            // 
            // Text_OpenName
            // 
            this.Text_OpenName.Location = new System.Drawing.Point(124, 12);
            this.Text_OpenName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Text_OpenName.Name = "Text_OpenName";
            this.Text_OpenName.Size = new System.Drawing.Size(223, 26);
            this.Text_OpenName.TabIndex = 1;
            // 
            // lblDeviceName
            // 
            this.lblDeviceName.AutoSize = true;
            this.lblDeviceName.Location = new System.Drawing.Point(8, 17);
            this.lblDeviceName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDeviceName.Name = "lblDeviceName";
            this.lblDeviceName.Size = new System.Drawing.Size(103, 20);
            this.lblDeviceName.TabIndex = 0;
            this.lblDeviceName.Text = "Device Name";
            // 
            // Command_EnableDevice
            // 
            this.Command_EnableDevice.Location = new System.Drawing.Point(472, 8);
            this.Command_EnableDevice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Command_EnableDevice.Name = "Command_EnableDevice";
            this.Command_EnableDevice.Size = new System.Drawing.Size(135, 38);
            this.Command_EnableDevice.TabIndex = 3;
            this.Command_EnableDevice.TabStop = false;
            this.Command_EnableDevice.Text = "EnableDevice";
            this.Command_EnableDevice.UseVisualStyleBackColor = true;
            // 
            // Command_StartReading
            // 
            this.Command_StartReading.Location = new System.Drawing.Point(756, 6);
            this.Command_StartReading.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Command_StartReading.Name = "Command_StartReading";
            this.Command_StartReading.Size = new System.Drawing.Size(138, 38);
            this.Command_StartReading.TabIndex = 3;
            this.Command_StartReading.TabStop = false;
            this.Command_StartReading.Text = "StartReading";
            this.Command_StartReading.UseVisualStyleBackColor = true;
            // 
            // Command_DisableDevice
            // 
            this.Command_DisableDevice.Location = new System.Drawing.Point(616, 8);
            this.Command_DisableDevice.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Command_DisableDevice.Name = "Command_DisableDevice";
            this.Command_DisableDevice.Size = new System.Drawing.Size(135, 38);
            this.Command_DisableDevice.TabIndex = 3;
            this.Command_DisableDevice.TabStop = false;
            this.Command_DisableDevice.Text = "DisableDevice";
            this.Command_DisableDevice.UseVisualStyleBackColor = true;
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelFooter);
            this.panelMain.Controls.Add(this.panelDetail);
            this.panelMain.Controls.Add(this.panelDevice);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1049, 396);
            this.panelMain.TabIndex = 2;
            // 
            // panelDetail
            // 
            this.panelDetail.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelDetail.Controls.Add(this.groupBox2);
            this.panelDetail.Location = new System.Drawing.Point(3, 57);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(1046, 308);
            this.panelDetail.TabIndex = 2;
            // 
            // panelFooter
            // 
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 368);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1049, 28);
            this.panelFooter.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panelLeft);
            this.groupBox2.Controls.Add(this.Text_ReadTagCount);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.List_Event);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(1046, 308);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Received Tag Data";
            // 
            // txtTagCount
            // 
            this.txtTagCount.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTagCount.Location = new System.Drawing.Point(95, 9);
            this.txtTagCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtTagCount.Name = "txtTagCount";
            this.txtTagCount.Size = new System.Drawing.Size(223, 26);
            this.txtTagCount.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 12);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 20);
            this.label6.TabIndex = 4;
            this.label6.Text = "Tag Count";
            // 
            // Text_ReadTagCount
            // 
            this.Text_ReadTagCount.Location = new System.Drawing.Point(699, 201);
            this.Text_ReadTagCount.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Text_ReadTagCount.Name = "Text_ReadTagCount";
            this.Text_ReadTagCount.Size = new System.Drawing.Size(192, 26);
            this.Text_ReadTagCount.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(565, 207);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(126, 20);
            this.label5.TabIndex = 2;
            this.label5.Text = "Read Tag Count";
            // 
            // List_Event
            // 
            this.List_Event.FormattingEnabled = true;
            this.List_Event.ItemHeight = 20;
            this.List_Event.Location = new System.Drawing.Point(569, 27);
            this.List_Event.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.List_Event.Name = "List_Event";
            this.List_Event.Size = new System.Drawing.Size(322, 164);
            this.List_Event.TabIndex = 1;
            // 
            // lvData
            // 
            this.lvData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.RFID});
            this.lvData.HideSelection = false;
            this.lvData.Location = new System.Drawing.Point(8, 5);
            this.lvData.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.lvData.Name = "lvData";
            this.lvData.Size = new System.Drawing.Size(521, 213);
            this.lvData.TabIndex = 0;
            this.lvData.UseCompatibleStateImageBehavior = false;
            this.lvData.View = System.Windows.Forms.View.Details;
            // 
            // RFID
            // 
            this.RFID.Text = "RFID";
            this.RFID.Width = 300;
            // 
            // panelLeft
            // 
            this.panelLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLeft.Controls.Add(this.panelLeftFooter);
            this.panelLeft.Controls.Add(this.panelLeftTop);
            this.panelLeft.Location = new System.Drawing.Point(7, 27);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(539, 272);
            this.panelLeft.TabIndex = 6;
            // 
            // panelLeftTop
            // 
            this.panelLeftTop.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelLeftTop.Controls.Add(this.lvData);
            this.panelLeftTop.Location = new System.Drawing.Point(3, 5);
            this.panelLeftTop.Name = "panelLeftTop";
            this.panelLeftTop.Size = new System.Drawing.Size(533, 227);
            this.panelLeftTop.TabIndex = 6;
            // 
            // panelLeftFooter
            // 
            this.panelLeftFooter.Controls.Add(this.label6);
            this.panelLeftFooter.Controls.Add(this.txtTagCount);
            this.panelLeftFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLeftFooter.Location = new System.Drawing.Point(0, 231);
            this.panelLeftFooter.Name = "panelLeftFooter";
            this.panelLeftFooter.Size = new System.Drawing.Size(539, 41);
            this.panelLeftFooter.TabIndex = 7;
            // 
            // RfidScannerCtl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelMain);
            this.Name = "RfidScannerCtl";
            this.Size = new System.Drawing.Size(1049, 396);
            ((System.ComponentModel.ISupportInitialize)(this.axPos)).EndInit();
            this.panelDevice.ResumeLayout(false);
            this.panelDevice.PerformLayout();
            this.grbResult.ResumeLayout(false);
            this.grbResult.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picDeviceSignal)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelDetail.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panelLeft.ResumeLayout(false);
            this.panelLeftTop.ResumeLayout(false);
            this.panelLeftFooter.ResumeLayout(false);
            this.panelLeftFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AxOPOSRFIDLib.AxOPOSRFID axPos;
        private System.Windows.Forms.Panel panelDevice;
        private System.Windows.Forms.GroupBox grbResult;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox CheckBox_Phase;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Text_ResultCodeExtended;
        private System.Windows.Forms.TextBox Text_ResultCode;
        private System.Windows.Forms.TextBox Text_Result;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Text_MethodName;
        private System.Windows.Forms.PictureBox picDeviceSignal;
        private System.Windows.Forms.Button Command_StopReading;
        private System.Windows.Forms.TextBox Text_OpenName;
        private System.Windows.Forms.Label lblDeviceName;
        private System.Windows.Forms.Button Command_EnableDevice;
        private System.Windows.Forms.Button Command_StartReading;
        private System.Windows.Forms.Button Command_DisableDevice;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtTagCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Text_ReadTagCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ListBox List_Event;
        private System.Windows.Forms.ListView lvData;
        private System.Windows.Forms.ColumnHeader RFID;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelLeftFooter;
        private System.Windows.Forms.Panel panelLeftTop;
    }
}
