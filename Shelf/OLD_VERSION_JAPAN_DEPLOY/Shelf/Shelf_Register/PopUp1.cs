using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Shelf_Register
{
    public partial class PopUp1 : Form
    {
        public PopUp1()
        {
            InitializeComponent();
            this.CenterToScreen();
        }

        private void ImageLayer_Paint(object sender, PaintEventArgs e)
        {

        }

        protected override void OnLoad(EventArgs e)
        {

            base.OnLoad(e);
            //Task.Factory.StartNew(Worker).ContinueWith(t => { this.Close(); }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void textBox_1_1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
