using System;
using System.Windows.Forms;

namespace Vjp.Rfid.SmartShelf
{
    public partial class FrmCheckDbConnection : Form
    {
        public FrmCheckDbConnection()
        {
            InitializeComponent();
        }

        private void FrmCheckDbConnection_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "Retrying Database Connection...";
        }

        private void lblMsg_Click(object sender, EventArgs e)
        {
            
        }


    }
}
