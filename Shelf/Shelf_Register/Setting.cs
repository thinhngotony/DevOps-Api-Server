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
    public partial class Setting : Form
    {
        public Setting()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            // Add checkbox to settingLayer - tableLayout
            CheckBox antenaNo1 = new CheckBox();
            antenaNo1.Text = "ANTENA 1";
            settingLayer.Controls.Add(antenaNo1, 0, 0);

            CheckBox antenaNo2 = new CheckBox();
            antenaNo2.Text = "ANTENA 2";
            settingLayer.Controls.Add(antenaNo2, 7, 0);

            CheckBox antenaNo3 = new CheckBox();
            antenaNo3.Text = "ANTENA 3";
            settingLayer.Controls.Add(antenaNo3, 0, 1);

            CheckBox antenaNo4 = new CheckBox();
            antenaNo4.Text = "ANTENA 4";
            settingLayer.Controls.Add(antenaNo4, 7, 1);

            CheckBox antenaNo5 = new CheckBox();
            antenaNo5.Text = "ANTENA 5";
            settingLayer.Controls.Add(antenaNo5, 0, 2);

            CheckBox antenaNo6 = new CheckBox();
            antenaNo6.Text = "ANTENA 6";
            settingLayer.Controls.Add(antenaNo6, 7, 2);

            CheckBox antenaNo7 = new CheckBox();
            antenaNo7.Text = "ANTENA 7";
            settingLayer.Controls.Add(antenaNo7, 0, 3);

            CheckBox antenaNo8 = new CheckBox();
            antenaNo8.Text = "ANTENA 8";
            settingLayer.Controls.Add(antenaNo8, 7, 3);

            foreach (CheckBox checkItem in settingLayer.Controls.OfType<CheckBox>())
            {
                checkItem.CheckAlign = ContentAlignment.MiddleCenter;
            }

            // Add pictureBox to settingLayer
            // Solution 1: Loop all settingLayer, find position not contain checkbox and insert picturebox
            // Solution 2: Manual insert
            // Solution 3: Loop with location

            PictureBox pictureBoxSetting_1_1 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_1_1, 1, 0);

            PictureBox pictureBoxSetting_1_2 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_1_2, 2, 0);

            PictureBox pictureBoxSetting_1_3 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_1_3, 3, 0);

            PictureBox pictureBoxSetting_1_4 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_1_4, 4, 0);

            PictureBox pictureBoxSetting_1_5 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_1_5, 5, 0);

            PictureBox pictureBoxSetting_1_6 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_1_6, 6, 0);

            PictureBox pictureBoxSetting_2_1 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_2_1, 1, 1);

            PictureBox pictureBoxSetting_2_2 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_2_2, 2, 1);

            PictureBox pictureBoxSetting_2_3 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_2_3, 3, 1);

            PictureBox pictureBoxSetting_2_4 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_2_4, 4, 1);

            PictureBox pictureBoxSetting_2_5 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_2_5, 5, 1);

            PictureBox pictureBoxSetting_2_6 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_2_6, 6, 1);

            PictureBox pictureBoxSetting_3_1 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_3_1, 1, 2);

            PictureBox pictureBoxSetting_3_2 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_3_2, 2, 2);

            PictureBox pictureBoxSetting_3_3 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_3_3, 3, 2);

            PictureBox pictureBoxSetting_3_4 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_3_4, 4, 2);

            PictureBox pictureBoxSetting_3_5 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_3_5, 5, 2);

            PictureBox pictureBoxSetting_3_6 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_3_6, 6, 2);

            PictureBox pictureBoxSetting_4_1 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_4_1, 1, 3);

            PictureBox pictureBoxSetting_4_2 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_4_2, 2, 3);

            PictureBox pictureBoxSetting_4_3 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_4_3, 3, 3);

            PictureBox pictureBoxSetting_4_4 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_4_4, 4, 3);

            PictureBox pictureBoxSetting_4_5 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_4_5, 5, 3);

            PictureBox pictureBoxSetting_4_6 = new PictureBox();
            settingLayer.Controls.Add(pictureBoxSetting_4_6, 6, 3);


            foreach(PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
            {
                pic.Dock = DockStyle.Fill;
                pic.Size = MaximumSize;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Load("blank_background.png");
            }

        }
    }
}
