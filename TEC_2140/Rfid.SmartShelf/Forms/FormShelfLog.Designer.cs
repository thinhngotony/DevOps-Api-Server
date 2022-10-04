
namespace Vjp.Rfid.SmartShelf.Forms
{
    partial class FormShelfLog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormShelfLog));
            this.olvShelfLog = new BrightIdeasSoftware.ObjectListView();
            this.olvColDate = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColRfid = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColCnt = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColOutTime = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColInTime = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvDiffInSeconds = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.panelShelfView = new System.Windows.Forms.Panel();
            this.panelShelfLogListView = new System.Windows.Forms.Panel();
            this.panelFilter = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.btnReLoad = new System.Windows.Forms.Button();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.lblFilter = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.olvShelfLog)).BeginInit();
            this.panelShelfView.SuspendLayout();
            this.panelShelfLogListView.SuspendLayout();
            this.panelFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // olvShelfLog
            // 
            this.olvShelfLog.AllColumns.Add(this.olvColDate);
            this.olvShelfLog.AllColumns.Add(this.olvColRfid);
            this.olvShelfLog.AllColumns.Add(this.olvColCnt);
            this.olvShelfLog.AllColumns.Add(this.olvColOutTime);
            this.olvShelfLog.AllColumns.Add(this.olvColInTime);
            this.olvShelfLog.AllColumns.Add(this.olvDiffInSeconds);
            this.olvShelfLog.AllowColumnReorder = true;
            this.olvShelfLog.CellEditUseWholeCell = false;
            this.olvShelfLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColDate,
            this.olvColRfid,
            this.olvColCnt,
            this.olvColOutTime,
            this.olvColInTime,
            this.olvDiffInSeconds});
            this.olvShelfLog.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvShelfLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.olvShelfLog.FullRowSelect = true;
            this.olvShelfLog.GridLines = true;
            this.olvShelfLog.HideSelection = false;
            this.olvShelfLog.Location = new System.Drawing.Point(0, 0);
            this.olvShelfLog.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.olvShelfLog.Name = "olvShelfLog";
            this.olvShelfLog.ShowCommandMenuOnRightClick = true;
            this.olvShelfLog.ShowFilterMenuOnRightClick = false;
            this.olvShelfLog.ShowItemCountOnGroups = true;
            this.olvShelfLog.Size = new System.Drawing.Size(838, 425);
            this.olvShelfLog.SpaceBetweenGroups = 10;
            this.olvShelfLog.TabIndex = 0;
            this.olvShelfLog.UseCellFormatEvents = true;
            this.olvShelfLog.UseCompatibleStateImageBehavior = false;
            this.olvShelfLog.UseFilterIndicator = true;
            this.olvShelfLog.UseFiltering = true;
            this.olvShelfLog.View = System.Windows.Forms.View.Details;
            this.olvShelfLog.FormatCell += new System.EventHandler<BrightIdeasSoftware.FormatCellEventArgs>(this.listView_FormatCell);
            // 
            // olvColDate
            // 
            this.olvColDate.AspectName = "DDate";
            this.olvColDate.AspectToStringFormat = "{0:d}";
            this.olvColDate.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColDate.Text = "Date";
            this.olvColDate.Width = 140;
            // 
            // olvColRfid
            // 
            this.olvColRfid.AspectName = "Rfid";
            this.olvColRfid.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColRfid.Text = "Rfid";
            this.olvColRfid.Width = 200;
            // 
            // olvColCnt
            // 
            this.olvColCnt.AspectName = "Cnt";
            this.olvColCnt.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColCnt.Text = "Count";
            this.olvColCnt.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColCnt.Width = 120;
            // 
            // olvColOutTime
            // 
            this.olvColOutTime.AspectName = "OutTime";
            this.olvColOutTime.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColOutTime.Text = "Out Time";
            this.olvColOutTime.Width = 160;
            // 
            // olvColInTime
            // 
            this.olvColInTime.AspectName = "InTime";
            this.olvColInTime.HeaderTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColInTime.Text = "In Time";
            this.olvColInTime.Width = 160;
            // 
            // olvDiffInSeconds
            // 
            this.olvDiffInSeconds.AspectName = "DiffInSeconds";
            this.olvDiffInSeconds.Text = "Diff(s)";
            this.olvDiffInSeconds.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvDiffInSeconds.Width = 100;
            // 
            // panelShelfView
            // 
            this.panelShelfView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.panelShelfView.Controls.Add(this.panelShelfLogListView);
            this.panelShelfView.Controls.Add(this.panelFilter);
            this.panelShelfView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelShelfView.Location = new System.Drawing.Point(0, 0);
            this.panelShelfView.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.panelShelfView.Name = "panelShelfView";
            this.panelShelfView.Size = new System.Drawing.Size(838, 455);
            this.panelShelfView.TabIndex = 1;
            // 
            // panelShelfLogListView
            // 
            this.panelShelfLogListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelShelfLogListView.Controls.Add(this.olvShelfLog);
            this.panelShelfLogListView.Location = new System.Drawing.Point(0, 28);
            this.panelShelfLogListView.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelShelfLogListView.Name = "panelShelfLogListView";
            this.panelShelfLogListView.Size = new System.Drawing.Size(838, 425);
            this.panelShelfLogListView.TabIndex = 2;
            // 
            // panelFilter
            // 
            this.panelFilter.Controls.Add(this.panelMain);
            this.panelFilter.Controls.Add(this.btnReLoad);
            this.panelFilter.Controls.Add(this.txtFilter);
            this.panelFilter.Controls.Add(this.lblFilter);
            this.panelFilter.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelFilter.Location = new System.Drawing.Point(0, 0);
            this.panelFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelFilter.Name = "panelFilter";
            this.panelFilter.Size = new System.Drawing.Size(838, 28);
            this.panelFilter.TabIndex = 1;
            // 
            // panelMain
            // 
            this.panelMain.Location = new System.Drawing.Point(484, 5);
            this.panelMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(89, 18);
            this.panelMain.TabIndex = 3;
            // 
            // btnReLoad
            // 
            this.btnReLoad.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReLoad.Location = new System.Drawing.Point(707, 3);
            this.btnReLoad.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnReLoad.Name = "btnReLoad";
            this.btnReLoad.Size = new System.Drawing.Size(121, 21);
            this.btnReLoad.TabIndex = 2;
            this.btnReLoad.Text = "ReLoad";
            this.btnReLoad.UseVisualStyleBackColor = true;
            this.btnReLoad.Click += new System.EventHandler(this.btnReLoad_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.Location = new System.Drawing.Point(65, 4);
            this.txtFilter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(302, 20);
            this.txtFilter.TabIndex = 1;
            this.txtFilter.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtFilter_KeyDown);
            // 
            // lblFilter
            // 
            this.lblFilter.AutoSize = true;
            this.lblFilter.Location = new System.Drawing.Point(10, 6);
            this.lblFilter.Name = "lblFilter";
            this.lblFilter.Size = new System.Drawing.Size(49, 13);
            this.lblFilter.TabIndex = 0;
            this.lblFilter.Text = "Filter";
            // 
            // FormShelfLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 455);
            this.Controls.Add(this.panelShelfView);
            this.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1, 2, 1, 2);
            this.MaximizeBox = false;
            this.Name = "FormShelfLog";
            this.Text = "Shelf Log View";
            this.Load += new System.EventHandler(this.FormShelfLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.olvShelfLog)).EndInit();
            this.panelShelfView.ResumeLayout(false);
            this.panelShelfLogListView.ResumeLayout(false);
            this.panelFilter.ResumeLayout(false);
            this.panelFilter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private BrightIdeasSoftware.ObjectListView olvShelfLog;
        private System.Windows.Forms.Panel panelShelfView;
        private BrightIdeasSoftware.OLVColumn olvColDate;
        private BrightIdeasSoftware.OLVColumn olvColRfid;
        private BrightIdeasSoftware.OLVColumn olvColCnt;
        private BrightIdeasSoftware.OLVColumn olvColOutTime;
        private BrightIdeasSoftware.OLVColumn olvColInTime;
        private System.Windows.Forms.Panel panelFilter;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Label lblFilter;
        private System.Windows.Forms.Panel panelShelfLogListView;
        private System.Windows.Forms.Button btnReLoad;
        private System.Windows.Forms.Panel panelMain;
        private BrightIdeasSoftware.OLVColumn olvDiffInSeconds;
    }
}