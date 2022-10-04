using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Vjp.Rfid.SmartShelf.Constants;
using Vjp.Rfid.SmartShelf.Interface;
using Vjp.Rfid.SmartShelf.Models;
using Vjp.Rfid.SmartShelf.Services;

namespace Vjp.Rfid.SmartShelf.Forms
{
    public partial class FormImportShelfProduct : Form
    {
        OpenFileDialog ofdShelfProductCsv = new OpenFileDialog();
        private readonly IRfidShelfProductService rfidShelfProductService ;
        private int borderSize = 2;
        private List<RfidShelfProduct> rfidShelfProductsImport = new List<RfidShelfProduct>();
        private static log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        #region"constructor"
        public FormImportShelfProduct()
        {
            InitializeComponent();

            rfidShelfProductService = Program.GetService<IRfidShelfProductService>();
            this.Padding = new Padding(borderSize);//Border size
            //this.BackColor = Color.FromArgb(98, 102, 244);//Border color

        }

        #endregion

        #region"event"
        private void FormImportShelfProduct_Load(object sender, EventArgs e)
        {

        }

        private void btnShelfProductOpenFileDlg_Click(object sender, EventArgs e)
        {
            String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            OpenFileDialog openFileDialog = new OpenFileDialog
            {

                InitialDirectory = "",
                Title = "Choose shelf product csv file to import",

                CheckFileExists = true,
                CheckPathExists = true,

                DefaultExt = "csv",
                Filter = "csv files (*.csv)|*.csv",
                FilterIndex = 1,
                RestoreDirectory = true

            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtShelfProductCsvPath.Text = openFileDialog.FileName;

                DipslayImportDataListView(txtShelfProductCsvPath.Text);

                olvDataImpResult.Items.Clear();
                olvDataUnImpResult.Items.Clear();
            }
        }

        private void DipslayImportDataListView(string filePath)
        {
            btnImport.Enabled = false;

            rfidShelfProductsImport = rfidShelfProductService.ReadShelftProductCsvUseCsvHelper(filePath);
            //display data on grid view 
            InitializeDataImportListView(rfidShelfProductsImport);
            //active import button
            if(rfidShelfProductsImport.Count > 0)
            {
                btnImport.Enabled = true;
            }
        }

        /// <summary>
        /// Import process
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnImport_Click(object sender, EventArgs e)
        {

            if (olvDataImp.Items.Count == 0)
            {
                MessageBox.Show("No record data import.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtShelfProductCsvPath.Focus();
                return;

            }

            if (olvDataImp.Items.Count  > 0 && rfidShelfProductsImport.Count>0)
            {
                int result = 0;
                if (MessageBox.Show("Do you want to import shelf products?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    //import csv file 
                    result = ImportCsvProcess(txtShelfProductCsvPath.Text);

                    //get imported data 
                    var importedData = GetRfidShelfProducts();

                    //display data 
                    DisplayImportResult(importedData);
                    //unimport result
                    DisplayUnImportResult(rfidShelfProductService.GetUnImportRfidShelfProducts());

                    MessageBox.Show($"Imported {result} records successfully.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }

        }

        #endregion

        #region"methods"
        private List<RfidShelfProduct> GetRfidShelfProducts()
        {
            var impResult = rfidShelfProductService.GetRfidShelfProducts();
            return impResult;

        }

        private void DisplayImportData(List<RfidShelfProduct> rfidShelfProducts)
        {
            InitializeResultListView(rfidShelfProducts);
        }

        private void DisplayImportResult(List<RfidShelfProduct> rfidShelfProducts)
        {
            InitializeResultListView(rfidShelfProducts);
        }

        private void DisplayUnImportResult(List<RfidShelfProduct> rfidShelfProducts)
        {
            InitializeUnImportResultListView(rfidShelfProducts);
        }



        private int ImportCsvProcess(string filePath)
        {
            var rfidArr = rfidShelfProductService.ReadShelftProductCsvUseCsvHelper(filePath);

            var result = rfidShelfProductService.InsertShelftProductToDb(rfidArr);

            return result;
        }
        //-------ObjectListView data import
        private void InitializeDataImportListView(List<RfidShelfProduct> rfidShelfProducts)
        {
            //if (ObjectListView.IsVistaOrLater)
            //    this.Font = new Font(AppConstants.AppFontName, AppConstants.AppMediumFontSize);

            olvDataImp.CopySelectionOnControlC = true;
            SetDataImportListView(rfidShelfProducts);
        }
        private void SetDataImportListView(List<RfidShelfProduct> list)
        {
            this.olvDataImp.ShowGroups = false;
            // Just one line of code make everything happen.
            this.olvDataImp.SetObjects(list);
        }

        //-------ObjectListView result
        private void InitializeResultListView(List<RfidShelfProduct> rfidShelfProducts)
        {
            //if (ObjectListView.IsVistaOrLater)
            //    this.Font = new Font(AppConstants.AppFontName, AppConstants.AppMediumFontSize);

            olvDataImpResult.CopySelectionOnControlC = true;
            SetDataImportResultListView(rfidShelfProducts);
        }

        private void InitializeUnImportResultListView(List<RfidShelfProduct> rfidShelfProducts)
        {
            //if (ObjectListView.IsVistaOrLater)
            //    this.Font = new Font(AppConstants.AppFontName, AppConstants.AppMediumFontSize);

            olvDataUnImpResult.CopySelectionOnControlC = true;
            SetDataUnImportResultListView(rfidShelfProducts);
        }

        private void SetDataImportResultListView(List<RfidShelfProduct> list)
        {
            this.olvDataImpResult.ShowGroups = false;
            // Just one line of code make everything happen.
            this.olvDataImpResult.SetObjects(list);
        }

        private void SetDataUnImportResultListView(List<RfidShelfProduct> list)
        {
            this.olvDataUnImpResult.ShowGroups = false;
            // Just one line of code make everything happen.
            this.olvDataUnImpResult.SetObjects(list);
        }

        #endregion



    }
}
