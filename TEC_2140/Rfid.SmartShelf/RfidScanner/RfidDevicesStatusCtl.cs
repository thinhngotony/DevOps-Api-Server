using BrightIdeasSoftware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vjp.Rfid.SmartShelf.Enums;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.RfidScanner
{
    public partial class RfidDevicesStatusCtl : UserControl
    {
        public List<ScannerDeviceInfo> DeviceNamesInfo { get; set; }
        public RfidDevicesStatusCtl()
        {
            InitializeComponent();
        }

        private void RfidDevicesStatusCtl_Load(object sender, EventArgs e)
        {
            ListViewInitialize();
        }

        protected void ListViewInitialize()
        {

            this.SetupDescibedTaskColumn();
            this.SetupColumns();
            this.SetupColumnEnableButton();
            this.SetupColumnDisableButton();
            this.SetupColumnStartReadingTagsButton();
            this.SetupColumnStopReadingTagsButton();
            

            // How much space do we want to give each row? Obviously, this should be at least
            // the height of the images used by the renderer
            this.olvData.ShowGroups = false;

            this.olvData.RowHeight = 54;
            this.olvData.SmallImageList = imageList;
            this.olvData.EmptyListMsg = "No device";
            this.olvData.UseAlternatingBackColors = false;
            this.olvData.UseHotItem = false;

            DeviceNamesInfo = new List<ScannerDeviceInfo>
            {
                new ScannerDeviceInfo
                {
                     DeviceName ="ABC",
                      DeviceStatus = DeviceStatus.Ready
                },
                new ScannerDeviceInfo
                {
                     DeviceName ="DEF",
                      DeviceStatus = DeviceStatus.NotAvailable
                },

            };

            // Make and display a list of tasks
            this.olvData.SetObjects(DeviceNamesInfo);
        }

        private void SetupColumnEnableButton()
        {

            // Tell the columns that it is going to show buttons.
            // The label that goes into the button is the Aspect that would have been
            // displayed in the cell.
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
            this.olvData.ButtonClick += delegate (object sender, CellClickEventArgs e) {
                Console.WriteLine(String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model));
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
            this.olvData.ButtonClick += delegate (object sender, CellClickEventArgs e) {
                Console.WriteLine(String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model));
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

        private void SetupColumnStartReadingTagsButton()
        {

            this.olvColumnStartReadingTags.IsButton = true;
            this.olvColumnStartReadingTags.ButtonSizing = OLVColumn.ButtonSizingMode.FixedBounds;
            this.olvColumnStartReadingTags.ButtonSize = new Size(80, 26);
            this.olvColumnStartReadingTags.TextAlign = HorizontalAlignment.Center;

            // Listen for button clicks -- which for the purpose of the demo will cycle the state of the service task
            this.olvData.ButtonClick += delegate (object sender, CellClickEventArgs e) {

                Console.WriteLine(String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model));
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

        private void SetupColumnStopReadingTagsButton()
        {

            this.olvColumnStopReadingTags.IsButton = true;
            this.olvColumnStopReadingTags.ButtonSizing = OLVColumn.ButtonSizingMode.FixedBounds;
            this.olvColumnStopReadingTags.ButtonSize = new Size(80, 26);
            this.olvColumnStopReadingTags.TextAlign = HorizontalAlignment.Center;

            // Listen for button clicks -- which for the purpose of the demo will cycle the state of the service task
            this.olvData.ButtonClick += delegate (object sender, CellClickEventArgs e) {

                
                Console.WriteLine(String.Format("Button clicked: ({0}, {1}, {2})", e.RowIndex, e.SubItem, e.Model));
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


        private void SetupDescibedTaskColumn()
        {
            // Setup a described task renderer, which draws a large icon
            // with a title, and a description under the title.
            // Almost all of this configuration could be done through the Designer
            // but I've done it through code that make it clear what's going on.

            // Create and install an appropriately configured renderer 
            //this.olvColumnTask.Renderer = CreateDescribedTaskRenderer();

            //// Now let's setup the couple of other bits that the column needs

            //// Tell the column which property should be used to get the title
            //this.olvColumnTask.AspectName = "Task";

            //// Tell the column which property holds the identifier for the image for row.
            //// We could also have installed an ImageGetter
            //this.olvColumnTask.ImageAspectName = "ImageName";

            //// Put a little bit of space around the task and its description
            //this.olvColumnTask.CellPadding = new Rectangle(4, 2, 4, 2);
        }

        private void SetupColumns()
        {
            // Draw the priority column as a collection of coins (first parameter).
            // We want the renderer to draw at most 4 stars (second parameter).
            // Priority has a value range from 0-5 (the last two parameters).
            //this.olvColumnPriority.TextAlign = HorizontalAlignment.Center;
            //MultiImageRenderer multiImageRenderer = new MultiImageRenderer("Lamp", 4, 0, 5);
            //multiImageRenderer.Spacing = -12; // We want the coins to overlap
            //this.olvColumnPriority.Renderer = multiImageRenderer;

            //this.olvColumnStatus.AspectToStringConverter = delegate (object model) {
            //    ServiceTask.TaskStatus status = (ServiceTask.TaskStatus)model;
            //    switch (status)
            //    {
            //        case ServiceTask.TaskStatus.InProgress:
            //            return "In progress";
            //        case ServiceTask.TaskStatus.NotStarted:
            //            return "Not started";
            //        case ServiceTask.TaskStatus.Complete:
            //            return "Complete";
            //        case ServiceTask.TaskStatus.Frozen:
            //            return "Frozen";
            //        default:
            //            return "";
            //    }
            //};

            this.olvColumnStatus.ImageGetter = delegate (object model) {
                ScannerDeviceInfo dvi = (ScannerDeviceInfo)model;
                switch (dvi.DeviceStatus)
                {
                    case  DeviceStatus.Ready:
                        return 1;
                        case DeviceStatus.NotAvailable:
                        return 0;
                    default:
                        return "";
                }
            };
        }


    }
}
