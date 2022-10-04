using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Vjp.Rfid.SmartShelf.Constants;
using Vjp.Rfid.SmartShelf.Models;
using Vjp.Rfid.SmartShelf.Services;

namespace Vjp.Rfid.SmartShelf.Forms
{
    public partial class FormShelfLog : Form
    {
        private readonly static RfidShelfDbService rfidShelfDbService = new RfidShelfDbService();
        private List<RfidShelfLogTable> rfidShelfLogs;
        private int borderSize = 2;

        #region"constructor"
        public FormShelfLog()
        {
            InitializeComponent();

            this.Padding = new Padding(borderSize);//Border size
            //this.BackColor = Color.FromArgb(98, 102, 244);//Border color
        }

        #endregion
       
        #region"event"
        private void FormShelfLog_Load(object sender, EventArgs e)
        {
            GetRfidShelfLogs();

            InitializeListView();
        }
        private void btnReLoad_Click(object sender, EventArgs e)
        {
            GetRfidShelfLogs();

            InitializeListView();
        }

        private void txtFilter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.TimedFilter(this.olvShelfLog, txtFilter.Text);

            }
        }

        private void listView_FormatCell(object sender, FormatCellEventArgs e)
        {
            RfidShelfLogTable p = (RfidShelfLogTable)e.Model;

            if ((e.ColumnIndex == 3 || e.ColumnIndex == 4) && e.SubItem.Text == default(DateTime).ToString())
            {
                e.SubItem.BackColor = Color.Aqua;
                e.SubItem.Text = "";


            }
        }

        #endregion

        #region"methods"
        /// <summary>
        /// Get all rfid shelf log from DB
        /// </summary>
        private void GetRfidShelfLogs()
        {
            rfidShelfLogs = rfidShelfDbService.GetRfidShelfLogsForView();
        }

        //----ObjectListView START
        private void InitializeListView()
        {
            //if (ObjectListView.IsVistaOrLater)
            //    this.Font = new Font(AppConstants.AppFontName, AppConstants.AppMediumFontSize);

            InitializeSimpleListView(rfidShelfLogs);
        }

        private void InitializeSimpleListView(List<RfidShelfLogTable> list)
        {
            this.olvShelfLog.ShowGroups = false;
            // Just one line of code make everything happen.
            this.olvShelfLog.SetObjects(list);
        }

        void TimedFilter(ObjectListView olv, string txt)
        {
            this.TimedFilter(olv, txt, 2);
        }

        void TimedFilter(ObjectListView olv, string txt, int matchKind)
        {
            TextMatchFilter filter = null;
            if (!String.IsNullOrEmpty(txt))
            {
                switch (matchKind)
                {
                    case 0:
                    default:
                        filter = TextMatchFilter.Contains(olv, txt);
                        break;
                    case 1:
                        filter = TextMatchFilter.Prefix(olv, txt);
                        break;
                    case 2:
                        filter = TextMatchFilter.Regex(olv, txt);
                        break;
                }
            }
            // Setup a default renderer to draw the filter matches
            if (filter == null)
                olv.DefaultRenderer = null;
            else
            {
                olv.DefaultRenderer = new HighlightTextRenderer(filter);

                // Uncomment this line to see how the GDI+ rendering looks
                //olv.DefaultRenderer = new HighlightTextRenderer { Filter = filter, UseGdiTextRendering = false };
            }

            // Some lists have renderers already installed
            HighlightTextRenderer highlightingRenderer = olv.GetColumn(0).Renderer as HighlightTextRenderer;
            if (highlightingRenderer != null)
                highlightingRenderer.Filter = filter;

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            olv.AdditionalFilter = filter;
            //olv.Invalidate();
            stopWatch.Stop();

        }

        //----ObjectListView END

        #endregion

    }
}
