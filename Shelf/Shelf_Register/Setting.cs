using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

        private void setPositionForRow(int row, int antena)
        {
            foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
            {
                {
                    if (int.Parse(pic.Name.Substring(0, 1)) == row)
                    {
                        if (antena % 2  == 0)
                        {
                            pic.Click += new System.EventHandler(pictureBoxOnClick_Right);
                        }
                        else
                        {
                            pic.Click += new System.EventHandler(pictureBoxOnClick_Left);
                        }                      
                    }
                }

            }
        }

        private void unSetPositionForRow(int row, int antena)
        {
            foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
            {
                {
                    if (int.Parse(pic.Name.Substring(0, 1)) == row)
                    {
                        if (antena % 2 == 0)
                        {
                            pic.Click += new System.EventHandler(pictureBoxOnClick_Right);
                        }
                        else
                        {
                            pic.Click += new System.EventHandler(pictureBoxOnClick_Left);
                        }
                    }
                }

            }
        }

        //EventOnClick for checkbox
        private void checkBoxOnClick(object sender, EventArgs e)
        {
            foreach (CheckBox checkItem in settingLayer.Controls.OfType<CheckBox>())
            {
                if (checkItem.Checked)
                {
                    switch (checkItem.Text)
                    {
                        case "ANTENA 1":
                            // Make picture box in row 1 can be clicked
                            setPositionForRow(1, 1);
                            break;
                        case "ANTENA 2":
                            // code block
                            setPositionForRow(1, 2);
                            break;
                        case "ANTENA 3":
                            setPositionForRow(2, 3);
                            // code block
                            break;
                        case "ANTENA 4":
                            setPositionForRow(2, 4);
                            // code block
                            break;
                        case "ANTENA 5":
                            setPositionForRow(3, 5);
                            // code block
                            break;
                        case "ANTENA 6":
                            setPositionForRow(3, 6);
                            // code block
                            break;
                        case "ANTENA 7":
                            setPositionForRow(4, 7);
                            // code block
                            break;
                        case "ANTENA 8":
                            setPositionForRow(4, 8);
                            // code block
                            break;
                        default:
                            // code block
                            break;
                    }
                } else //Handle unclick check box 
                {

                }    
            }
        }

        private void pictureBoxOnClick_Left(object sender, EventArgs e)
        {
            PictureBox pictureBoxClicked = sender as PictureBox;
            foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
            {
                if (int.Parse(pic.Name.Substring(0, 1)) == int.Parse(pictureBoxClicked.Name.Substring(0, 1)))
                {
                    pic.Load("blank_background.png");
                    if (int.Parse(pic.Name.Substring(2, 1)) >= int.Parse(pictureBoxClicked.Name.Substring(2, 1)))
                    {
                        pic.Load("selected.png");
                        //Call API
                       

                    }

                }
            }
        }

        private void pictureBoxOnClick_Right(object sender, EventArgs e)
        {
            PictureBox pictureBoxClicked = sender as PictureBox;
            foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
            {
                if (int.Parse(pic.Name.Substring(0, 1)) == int.Parse(pictureBoxClicked.Name.Substring(0, 1)))
                {
                    pic.Load("blank_background.png");
                    if (int.Parse(pic.Name.Substring(2, 1)) <= int.Parse(pictureBoxClicked.Name.Substring(2, 1)))
                    {
                        pic.Load("selected.png");
                    }
                }
            }
        }

        private void btnSubmitOnClick(object sender, EventArgs e)
        {
            int max = 0;
            int min = 7;

            foreach (CheckBox checkItem in settingLayer.Controls.OfType<CheckBox>())
            {
                foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
                { 
                    if ( pic.ImageLocation == "selected.png") 
                    {
                        if (checkItem.Checked)
                        {
                            // Left to right
                            if (int.Parse(checkItem.Name) % 2 == 0)
                            {
                                // Lấy giá trị lớn nhất của row để làm start
                                for (var scan_col_start = 0; scan_col_start < 7; scan_col_start++)
                                {
                                    // Xử lí chỉ trong 1 row
                                    if (int.Parse(pic.Name.Substring(2, 1)) == scan_col_start)
                                    {
                                        if (int.Parse(pic.Name.Substring(2, 1)) > max)
                                        {
                                            max = int.Parse(pic.Name.Substring(2, 1));
                                        }
                                    }

                                }

                            }
                            // Right to left
                            else
                            {
                                // Lấy giá trị nhỏ nhất của row để làm start
                                for (var scan_col_start = 7; scan_col_start > 0; scan_col_start--)
                                {
                                    // Xử lí chỉ trong 1 row
                                    if (int.Parse(pic.Name.Substring(2, 1)) == scan_col_start)
                                    {
                                        
                                        if (int.Parse(pic.Name.Substring(2, 1)) < min)
                                        {
                                            min = int.Parse(pic.Name.Substring(2, 1));
                                            MessageBox.Show(pic.Name);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                    
            }

        }

        private void checkAPI(int max)
        {
            MessageBox.Show(max.ToString());
        }

        private static void ApiUpdatePositionMSTAntena(string shelf_no, string antena, int row, int col, int scan_col_start, int scan_col_end)
        {
            string shelfNo = Session.nameOfShelf;

        }



        private void init()
        {
            // Add checkbox to settingLayer - tableLayout
            CheckBox antenaNo1 = new CheckBox();
            antenaNo1.Text = "ANTENA 1";
            antenaNo1.Name = "1";
            settingLayer.Controls.Add(antenaNo1, 0, 0);

            CheckBox antenaNo2 = new CheckBox();
            antenaNo2.Text = "ANTENA 2";
            antenaNo2.Name = "2";
            settingLayer.Controls.Add(antenaNo2, 7, 0);

            CheckBox antenaNo3 = new CheckBox();
            antenaNo3.Text = "ANTENA 3";
            antenaNo3.Name = "3";
            settingLayer.Controls.Add(antenaNo3, 0, 1);

            CheckBox antenaNo4 = new CheckBox();
            antenaNo4.Text = "ANTENA 4";
            antenaNo4.Name = "4";
            settingLayer.Controls.Add(antenaNo4, 7, 1);

            CheckBox antenaNo5 = new CheckBox();
            antenaNo5.Text = "ANTENA 5";
            antenaNo5.Name = "5";
            settingLayer.Controls.Add(antenaNo5, 0, 2);

            CheckBox antenaNo6 = new CheckBox();
            antenaNo6.Text = "ANTENA 6";
            antenaNo6.Name = "6";
            settingLayer.Controls.Add(antenaNo6, 7, 2);

            CheckBox antenaNo7 = new CheckBox();
            antenaNo7.Text = "ANTENA 7";
            antenaNo7.Name = "7";
            settingLayer.Controls.Add(antenaNo7, 0, 3);

            CheckBox antenaNo8 = new CheckBox();
            antenaNo8.Text = "ANTENA 8";
            antenaNo8.Name = "8";
            settingLayer.Controls.Add(antenaNo8, 7, 3);

            foreach (CheckBox checkItem in settingLayer.Controls.OfType<CheckBox>())
            {
                //Handle checkbox to center
                checkItem.CheckedChanged += new System.EventHandler(checkBoxOnClick);
            }

            // Add button settingLayer - tableLayout
            Button btnSubmit = new Button();
            btnSubmit.Text = "SUBMIT";
            btnSubmit.Height = 50;
            btnSubmit.Width = 100;
            btnSubmit.Click += new System.EventHandler(btnSubmitOnClick);
            settingLayer.Controls.Add(btnSubmit, 8, 4);

            // Add pictureBox to settingLayer
            // Solution 1: Loop all settingLayer, find position not contain checkbox and insert picturebox
            // Solution 2: Manual insert
            // Solution 3: Loop with location

            PictureBox pictureBoxSetting_1_1 = new PictureBox();
            pictureBoxSetting_1_1.Name = "1_1";
            settingLayer.Controls.Add(pictureBoxSetting_1_1, 1, 0);

            PictureBox pictureBoxSetting_1_2 = new PictureBox();
            pictureBoxSetting_1_2.Name = "1_2";
            settingLayer.Controls.Add(pictureBoxSetting_1_2, 2, 0);

            PictureBox pictureBoxSetting_1_3 = new PictureBox();
            pictureBoxSetting_1_3.Name = "1_3";
            settingLayer.Controls.Add(pictureBoxSetting_1_3, 3, 0);

            PictureBox pictureBoxSetting_1_4 = new PictureBox();
            pictureBoxSetting_1_4.Name = "1_4";
            settingLayer.Controls.Add(pictureBoxSetting_1_4, 4, 0);

            PictureBox pictureBoxSetting_1_5 = new PictureBox();
            pictureBoxSetting_1_5.Name = "1_5";
            settingLayer.Controls.Add(pictureBoxSetting_1_5, 5, 0);

            PictureBox pictureBoxSetting_1_6 = new PictureBox();
            pictureBoxSetting_1_6.Name = "1_6";
            settingLayer.Controls.Add(pictureBoxSetting_1_6, 6, 0);

            PictureBox pictureBoxSetting_2_1 = new PictureBox();
            pictureBoxSetting_2_1.Name = "2_1";
            settingLayer.Controls.Add(pictureBoxSetting_2_1, 1, 1);

            PictureBox pictureBoxSetting_2_2 = new PictureBox();
            pictureBoxSetting_2_2.Name = "2_2";
            settingLayer.Controls.Add(pictureBoxSetting_2_2, 2, 1);

            PictureBox pictureBoxSetting_2_3 = new PictureBox();
            pictureBoxSetting_2_3.Name = "2_3";
            settingLayer.Controls.Add(pictureBoxSetting_2_3, 3, 1);

            PictureBox pictureBoxSetting_2_4 = new PictureBox();
            pictureBoxSetting_2_4.Name = "2_4";
            settingLayer.Controls.Add(pictureBoxSetting_2_4, 4, 1);

            PictureBox pictureBoxSetting_2_5 = new PictureBox();
            pictureBoxSetting_2_5.Name = "2_5";
            settingLayer.Controls.Add(pictureBoxSetting_2_5, 5, 1);

            PictureBox pictureBoxSetting_2_6 = new PictureBox();
            pictureBoxSetting_2_6.Name = "2_6";
            settingLayer.Controls.Add(pictureBoxSetting_2_6, 6, 1);

            PictureBox pictureBoxSetting_3_1 = new PictureBox();
            pictureBoxSetting_3_1.Name = "3_1";
            settingLayer.Controls.Add(pictureBoxSetting_3_1, 1, 2);

            PictureBox pictureBoxSetting_3_2 = new PictureBox();
            pictureBoxSetting_3_2.Name = "3_2";
            settingLayer.Controls.Add(pictureBoxSetting_3_2, 2, 2);

            PictureBox pictureBoxSetting_3_3 = new PictureBox();
            pictureBoxSetting_3_3.Name = "3_3";
            settingLayer.Controls.Add(pictureBoxSetting_3_3, 3, 2);

            PictureBox pictureBoxSetting_3_4 = new PictureBox();
            pictureBoxSetting_3_4.Name = "3_4";
            settingLayer.Controls.Add(pictureBoxSetting_3_4, 4, 2);

            PictureBox pictureBoxSetting_3_5 = new PictureBox();
            pictureBoxSetting_3_5.Name = "3_5";
            settingLayer.Controls.Add(pictureBoxSetting_3_5, 5, 2);

            PictureBox pictureBoxSetting_3_6 = new PictureBox();
            pictureBoxSetting_3_6.Name = "3_6";
            settingLayer.Controls.Add(pictureBoxSetting_3_6, 6, 2);

            PictureBox pictureBoxSetting_4_1 = new PictureBox();
            pictureBoxSetting_4_1.Name = "4_1";
            settingLayer.Controls.Add(pictureBoxSetting_4_1, 1, 3);

            PictureBox pictureBoxSetting_4_2 = new PictureBox();
            pictureBoxSetting_4_2.Name = "4_2";
            settingLayer.Controls.Add(pictureBoxSetting_4_2, 2, 3);

            PictureBox pictureBoxSetting_4_3 = new PictureBox();
            pictureBoxSetting_4_3.Name = "4_3";
            settingLayer.Controls.Add(pictureBoxSetting_4_3, 3, 3);

            PictureBox pictureBoxSetting_4_4 = new PictureBox();
            pictureBoxSetting_4_4.Name = "4_4";
            settingLayer.Controls.Add(pictureBoxSetting_4_4, 4, 3);

            PictureBox pictureBoxSetting_4_5 = new PictureBox();
            pictureBoxSetting_4_5.Name = "4_5";
            settingLayer.Controls.Add(pictureBoxSetting_4_5, 5, 3);

            PictureBox pictureBoxSetting_4_6 = new PictureBox();
            pictureBoxSetting_4_6.Name = "4_6";
            settingLayer.Controls.Add(pictureBoxSetting_4_6, 6, 3);

            //Set event onclick for all picture box
            foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
            {
                pic.Dock = DockStyle.Fill;
                pic.Size = MaximumSize;
                pic.SizeMode = PictureBoxSizeMode.StretchImage;
                pic.Load("blank_background.png");
            }

        }

    }
}
