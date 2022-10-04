
namespace Vjp.Rfid.SmartShelf.Forms
{
    partial class FormImportShelfProduct
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormImportShelfProduct));
            this.panelHeader = new System.Windows.Forms.Panel();
            this.btnShelfProductOpenFileDlg = new System.Windows.Forms.Button();
            this.txtShelfProductCsvPath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelDetail = new System.Windows.Forms.Panel();
            this.grpBeforeImp = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.olvDataImp = new BrightIdeasSoftware.DataListView();
            this.olvColumn1 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn2 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn3 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn4 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn5 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn6 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.grpResult = new System.Windows.Forms.GroupBox();
            this.panelData = new System.Windows.Forms.Panel();
            this.olvDataImpResult = new BrightIdeasSoftware.DataListView();
            this.colShelfNo = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colShelfColIndex = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colRfid = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colIsbnCode = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colProductName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.colScannerName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.chkDeleteAll = new System.Windows.Forms.CheckBox();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnImport = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelImpResult = new System.Windows.Forms.Panel();
            this.ShelfName = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.JanCd = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.olvDataUnImpResult = new BrightIdeasSoftware.DataListView();
            this.olvColumn7 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn8 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn9 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn10 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn11 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn12 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn13 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn14 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn15 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.olvColumn16 = ((BrightIdeasSoftware.OLVColumn)(new BrightIdeasSoftware.OLVColumn()));
            this.panelHeader.SuspendLayout();
            this.panelDetail.SuspendLayout();
            this.grpBeforeImp.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvDataImp)).BeginInit();
            this.grpResult.SuspendLayout();
            this.panelData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvDataImpResult)).BeginInit();
            this.panelFooter.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelImpResult.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.olvDataUnImpResult)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.btnShelfProductOpenFileDlg);
            this.panelHeader.Controls.Add(this.txtShelfProductCsvPath);
            this.panelHeader.Controls.Add(this.label1);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(838, 33);
            this.panelHeader.TabIndex = 0;
            // 
            // btnShelfProductOpenFileDlg
            // 
            this.btnShelfProductOpenFileDlg.Location = new System.Drawing.Point(731, 4);
            this.btnShelfProductOpenFileDlg.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnShelfProductOpenFileDlg.Name = "btnShelfProductOpenFileDlg";
            this.btnShelfProductOpenFileDlg.Size = new System.Drawing.Size(39, 26);
            this.btnShelfProductOpenFileDlg.TabIndex = 2;
            this.btnShelfProductOpenFileDlg.Text = "...";
            this.btnShelfProductOpenFileDlg.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnShelfProductOpenFileDlg.UseVisualStyleBackColor = true;
            this.btnShelfProductOpenFileDlg.Click += new System.EventHandler(this.btnShelfProductOpenFileDlg_Click);
            // 
            // txtShelfProductCsvPath
            // 
            this.txtShelfProductCsvPath.Location = new System.Drawing.Point(117, 7);
            this.txtShelfProductCsvPath.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.txtShelfProductCsvPath.Name = "txtShelfProductCsvPath";
            this.txtShelfProductCsvPath.Size = new System.Drawing.Size(608, 20);
            this.txtShelfProductCsvPath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Csv File Path";
            // 
            // panelDetail
            // 
            this.panelDetail.Controls.Add(this.grpBeforeImp);
            this.panelDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelDetail.Location = new System.Drawing.Point(0, 33);
            this.panelDetail.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.panelDetail.Name = "panelDetail";
            this.panelDetail.Size = new System.Drawing.Size(838, 168);
            this.panelDetail.TabIndex = 1;
            // 
            // grpBeforeImp
            // 
            this.grpBeforeImp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpBeforeImp.Controls.Add(this.panel1);
            this.grpBeforeImp.Location = new System.Drawing.Point(4, 4);
            this.grpBeforeImp.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpBeforeImp.Name = "grpBeforeImp";
            this.grpBeforeImp.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpBeforeImp.Size = new System.Drawing.Size(821, 164);
            this.grpBeforeImp.TabIndex = 1;
            this.grpBeforeImp.TabStop = false;
            this.grpBeforeImp.Text = "Data import";
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.olvDataImp);
            this.panel1.Location = new System.Drawing.Point(6, 18);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(804, 141);
            this.panel1.TabIndex = 1;
            // 
            // olvDataImp
            // 
            this.olvDataImp.AllColumns.Add(this.olvColumn1);
            this.olvDataImp.AllColumns.Add(this.olvColumn2);
            this.olvDataImp.AllColumns.Add(this.JanCd);
            this.olvDataImp.AllColumns.Add(this.olvColumn3);
            this.olvDataImp.AllColumns.Add(this.olvColumn4);
            this.olvDataImp.AllColumns.Add(this.olvColumn5);
            this.olvDataImp.AllColumns.Add(this.olvColumn6);
            this.olvDataImp.AllColumns.Add(this.ShelfName);
            this.olvDataImp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvDataImp.CellEditUseWholeCell = false;
            this.olvDataImp.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn1,
            this.olvColumn2,
            this.JanCd,
            this.olvColumn3,
            this.olvColumn4,
            this.olvColumn5,
            this.olvColumn6,
            this.ShelfName});
            this.olvDataImp.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvDataImp.DataSource = null;
            this.olvDataImp.FullRowSelect = true;
            this.olvDataImp.GridLines = true;
            this.olvDataImp.HideSelection = false;
            this.olvDataImp.Location = new System.Drawing.Point(3, 2);
            this.olvDataImp.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.olvDataImp.Name = "olvDataImp";
            this.olvDataImp.Size = new System.Drawing.Size(799, 137);
            this.olvDataImp.TabIndex = 0;
            this.olvDataImp.UseCompatibleStateImageBehavior = false;
            this.olvDataImp.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn1
            // 
            this.olvColumn1.AspectName = "ShelfNo";
            this.olvColumn1.Text = "Shelf No";
            this.olvColumn1.Width = 80;
            // 
            // olvColumn2
            // 
            this.olvColumn2.AspectName = "ShelfColIndex";
            this.olvColumn2.Text = "Shelf Col";
            this.olvColumn2.Width = 80;
            // 
            // olvColumn3
            // 
            this.olvColumn3.AspectName = "Rfid";
            this.olvColumn3.Text = "Rfid";
            this.olvColumn3.Width = 160;
            // 
            // olvColumn4
            // 
            this.olvColumn4.AspectName = "IsbnCode";
            this.olvColumn4.Text = "ISBN";
            this.olvColumn4.Width = 120;
            // 
            // olvColumn5
            // 
            this.olvColumn5.AspectName = "ProductName";
            this.olvColumn5.Text = "Product Name";
            this.olvColumn5.Width = 300;
            // 
            // olvColumn6
            // 
            this.olvColumn6.AspectName = "ScannerName";
            this.olvColumn6.Text = "Scanner Name";
            this.olvColumn6.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.olvColumn6.Width = 160;
            // 
            // grpResult
            // 
            this.grpResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpResult.Controls.Add(this.panelData);
            this.grpResult.Location = new System.Drawing.Point(0, 5);
            this.grpResult.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpResult.Name = "grpResult";
            this.grpResult.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.grpResult.Size = new System.Drawing.Size(821, 203);
            this.grpResult.TabIndex = 0;
            this.grpResult.TabStop = false;
            this.grpResult.Text = "Import result";
            // 
            // panelData
            // 
            this.panelData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelData.Controls.Add(this.olvDataImpResult);
            this.panelData.Location = new System.Drawing.Point(6, 18);
            this.panelData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelData.Name = "panelData";
            this.panelData.Size = new System.Drawing.Size(804, 179);
            this.panelData.TabIndex = 1;
            // 
            // olvDataImpResult
            // 
            this.olvDataImpResult.AllColumns.Add(this.colShelfNo);
            this.olvDataImpResult.AllColumns.Add(this.colShelfColIndex);
            this.olvDataImpResult.AllColumns.Add(this.olvColumn14);
            this.olvDataImpResult.AllColumns.Add(this.colRfid);
            this.olvDataImpResult.AllColumns.Add(this.colIsbnCode);
            this.olvDataImpResult.AllColumns.Add(this.colProductName);
            this.olvDataImpResult.AllColumns.Add(this.colScannerName);
            this.olvDataImpResult.AllColumns.Add(this.olvColumn13);
            this.olvDataImpResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvDataImpResult.CellEditUseWholeCell = false;
            this.olvDataImpResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colShelfNo,
            this.colShelfColIndex,
            this.olvColumn14,
            this.colRfid,
            this.colIsbnCode,
            this.colProductName,
            this.colScannerName,
            this.olvColumn13});
            this.olvDataImpResult.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvDataImpResult.DataSource = null;
            this.olvDataImpResult.FullRowSelect = true;
            this.olvDataImpResult.GridLines = true;
            this.olvDataImpResult.HideSelection = false;
            this.olvDataImpResult.Location = new System.Drawing.Point(0, 2);
            this.olvDataImpResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.olvDataImpResult.Name = "olvDataImpResult";
            this.olvDataImpResult.Size = new System.Drawing.Size(802, 169);
            this.olvDataImpResult.TabIndex = 0;
            this.olvDataImpResult.UseCompatibleStateImageBehavior = false;
            this.olvDataImpResult.View = System.Windows.Forms.View.Details;
            // 
            // colShelfNo
            // 
            this.colShelfNo.AspectName = "ShelfNo";
            this.colShelfNo.Text = "Shelf No";
            this.colShelfNo.Width = 80;
            // 
            // colShelfColIndex
            // 
            this.colShelfColIndex.AspectName = "ShelfColIndex";
            this.colShelfColIndex.Text = "Shelf Col";
            this.colShelfColIndex.Width = 80;
            // 
            // colRfid
            // 
            this.colRfid.AspectName = "Rfid";
            this.colRfid.Text = "Rfid";
            this.colRfid.Width = 160;
            // 
            // colIsbnCode
            // 
            this.colIsbnCode.AspectName = "IsbnCode";
            this.colIsbnCode.Text = "ISBN";
            this.colIsbnCode.Width = 120;
            // 
            // colProductName
            // 
            this.colProductName.AspectName = "ProductName";
            this.colProductName.Text = "Product Name";
            this.colProductName.Width = 300;
            // 
            // colScannerName
            // 
            this.colScannerName.AspectName = "ScannerName";
            this.colScannerName.Text = "Scanner Name";
            this.colScannerName.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.colScannerName.Width = 160;
            // 
            // chkDeleteAll
            // 
            this.chkDeleteAll.AutoSize = true;
            this.chkDeleteAll.Checked = true;
            this.chkDeleteAll.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDeleteAll.Enabled = false;
            this.chkDeleteAll.Location = new System.Drawing.Point(14, 3);
            this.chkDeleteAll.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.chkDeleteAll.Name = "chkDeleteAll";
            this.chkDeleteAll.Size = new System.Drawing.Size(194, 17);
            this.chkDeleteAll.TabIndex = 2;
            this.chkDeleteAll.Text = "Delete all before import";
            this.chkDeleteAll.UseVisualStyleBackColor = true;
            this.chkDeleteAll.Visible = false;
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.btnImport);
            this.panelFooter.Controls.Add(this.chkDeleteAll);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 619);
            this.panelFooter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(838, 60);
            this.panelFooter.TabIndex = 3;
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(14, 28);
            this.btnImport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(120, 24);
            this.btnImport.TabIndex = 3;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // panelMain
            // 
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.panelMain.Controls.Add(this.panel2);
            this.panelMain.Controls.Add(this.panelImpResult);
            this.panelMain.Controls.Add(this.panelDetail);
            this.panelMain.Controls.Add(this.panelFooter);
            this.panelMain.Controls.Add(this.panelHeader);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(838, 679);
            this.panelMain.TabIndex = 3;
            // 
            // panelImpResult
            // 
            this.panelImpResult.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelImpResult.Controls.Add(this.grpResult);
            this.panelImpResult.Location = new System.Drawing.Point(4, 206);
            this.panelImpResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelImpResult.Name = "panelImpResult";
            this.panelImpResult.Size = new System.Drawing.Size(834, 211);
            this.panelImpResult.TabIndex = 4;
            // 
            // ShelfName
            // 
            this.ShelfName.AspectName = "ShelfName";
            this.ShelfName.Text = "Shelf Name";
            // 
            // JanCd
            // 
            this.JanCd.AspectName = "JanCd";
            this.JanCd.Text = "Jan Code";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Location = new System.Drawing.Point(4, 424);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(831, 183);
            this.panel2.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Location = new System.Drawing.Point(0, 5);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.groupBox1.Size = new System.Drawing.Size(820, 175);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "UnImport result(existed rfid)";
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.olvDataUnImpResult);
            this.panel3.Location = new System.Drawing.Point(6, 18);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(803, 151);
            this.panel3.TabIndex = 1;
            // 
            // olvDataUnImpResult
            // 
            this.olvDataUnImpResult.AllColumns.Add(this.olvColumn7);
            this.olvDataUnImpResult.AllColumns.Add(this.olvColumn8);
            this.olvDataUnImpResult.AllColumns.Add(this.olvColumn16);
            this.olvDataUnImpResult.AllColumns.Add(this.olvColumn9);
            this.olvDataUnImpResult.AllColumns.Add(this.olvColumn10);
            this.olvDataUnImpResult.AllColumns.Add(this.olvColumn11);
            this.olvDataUnImpResult.AllColumns.Add(this.olvColumn12);
            this.olvDataUnImpResult.AllColumns.Add(this.olvColumn15);
            this.olvDataUnImpResult.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.olvDataUnImpResult.CellEditUseWholeCell = false;
            this.olvDataUnImpResult.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.olvColumn7,
            this.olvColumn8,
            this.olvColumn16,
            this.olvColumn9,
            this.olvColumn10,
            this.olvColumn11,
            this.olvColumn12,
            this.olvColumn15});
            this.olvDataUnImpResult.Cursor = System.Windows.Forms.Cursors.Default;
            this.olvDataUnImpResult.DataSource = null;
            this.olvDataUnImpResult.FullRowSelect = true;
            this.olvDataUnImpResult.GridLines = true;
            this.olvDataUnImpResult.HideSelection = false;
            this.olvDataUnImpResult.Location = new System.Drawing.Point(0, 2);
            this.olvDataUnImpResult.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.olvDataUnImpResult.Name = "olvDataUnImpResult";
            this.olvDataUnImpResult.Size = new System.Drawing.Size(801, 141);
            this.olvDataUnImpResult.TabIndex = 0;
            this.olvDataUnImpResult.UseCompatibleStateImageBehavior = false;
            this.olvDataUnImpResult.View = System.Windows.Forms.View.Details;
            // 
            // olvColumn7
            // 
            this.olvColumn7.AspectName = "ShelfNo";
            this.olvColumn7.Text = "Shelf No";
            this.olvColumn7.Width = 80;
            // 
            // olvColumn8
            // 
            this.olvColumn8.AspectName = "ShelfColIndex";
            this.olvColumn8.Text = "Shelf Col";
            this.olvColumn8.Width = 80;
            // 
            // olvColumn9
            // 
            this.olvColumn9.AspectName = "Rfid";
            this.olvColumn9.Text = "Rfid";
            this.olvColumn9.Width = 160;
            // 
            // olvColumn10
            // 
            this.olvColumn10.AspectName = "IsbnCode";
            this.olvColumn10.Text = "ISBN";
            this.olvColumn10.Width = 120;
            // 
            // olvColumn11
            // 
            this.olvColumn11.AspectName = "ProductName";
            this.olvColumn11.Text = "Product Name";
            this.olvColumn11.Width = 300;
            // 
            // olvColumn12
            // 
            this.olvColumn12.AspectName = "ScannerName";
            this.olvColumn12.Text = "Scanner Name";
            this.olvColumn12.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.olvColumn12.Width = 160;
            // 
            // olvColumn13
            // 
            this.olvColumn13.AspectName = "ShelfName";
            this.olvColumn13.Text = "Shelf Name";
            // 
            // olvColumn14
            // 
            this.olvColumn14.AspectName = "JanCd";
            this.olvColumn14.Text = "Jan Cd";
            // 
            // olvColumn15
            // 
            this.olvColumn15.AspectName = "ShelfName";
            this.olvColumn15.Text = "Shelf Name";
            // 
            // olvColumn16
            // 
            this.olvColumn16.AspectName = "JanCd";
            this.olvColumn16.Text = "Jan Cd";
            // 
            // FormImportShelfProduct
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 679);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("MS Gothic", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.Name = "FormImportShelfProduct";
            this.Text = "Import Shelf Products";
            this.Load += new System.EventHandler(this.FormImportShelfProduct_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.panelDetail.ResumeLayout(false);
            this.grpBeforeImp.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvDataImp)).EndInit();
            this.grpResult.ResumeLayout(false);
            this.panelData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvDataImpResult)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelImpResult.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.olvDataUnImpResult)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Button btnShelfProductOpenFileDlg;
        private System.Windows.Forms.TextBox txtShelfProductCsvPath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelDetail;
        private System.Windows.Forms.CheckBox chkDeleteAll;
        private System.Windows.Forms.GroupBox grpResult;
        private BrightIdeasSoftware.DataListView olvDataImpResult;
        private BrightIdeasSoftware.OLVColumn colShelfNo;
        private BrightIdeasSoftware.OLVColumn colShelfColIndex;
        private System.Windows.Forms.Panel panelData;
        private BrightIdeasSoftware.OLVColumn colRfid;
        private BrightIdeasSoftware.OLVColumn colIsbnCode;
        private BrightIdeasSoftware.OLVColumn colProductName;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.GroupBox grpBeforeImp;
        private System.Windows.Forms.Panel panel1;
        private BrightIdeasSoftware.DataListView olvDataImp;
        private BrightIdeasSoftware.OLVColumn olvColumn1;
        private BrightIdeasSoftware.OLVColumn olvColumn2;
        private BrightIdeasSoftware.OLVColumn olvColumn3;
        private BrightIdeasSoftware.OLVColumn olvColumn4;
        private BrightIdeasSoftware.OLVColumn olvColumn5;
        private System.Windows.Forms.Panel panelImpResult;
        private BrightIdeasSoftware.OLVColumn olvColumn6;
        private BrightIdeasSoftware.OLVColumn colScannerName;
        private BrightIdeasSoftware.OLVColumn JanCd;
        private BrightIdeasSoftware.OLVColumn ShelfName;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel3;
        private BrightIdeasSoftware.DataListView olvDataUnImpResult;
        private BrightIdeasSoftware.OLVColumn olvColumn7;
        private BrightIdeasSoftware.OLVColumn olvColumn8;
        private BrightIdeasSoftware.OLVColumn olvColumn9;
        private BrightIdeasSoftware.OLVColumn olvColumn10;
        private BrightIdeasSoftware.OLVColumn olvColumn11;
        private BrightIdeasSoftware.OLVColumn olvColumn12;
        private BrightIdeasSoftware.OLVColumn olvColumn14;
        private BrightIdeasSoftware.OLVColumn olvColumn13;
        private BrightIdeasSoftware.OLVColumn olvColumn16;
        private BrightIdeasSoftware.OLVColumn olvColumn15;
    }
}