using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Shelf_Register.Front;

namespace Shelf_Register
{
    public partial class Setting : Form
    {
        public static Dictionary<string, List<PictureBox>> dicItems = new Dictionary<string, List<PictureBox>>();

        public Setting()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
            this.CenterToScreen();
            init();


        }

        private void setPositionForRow(int row, int antena)
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

        private void unSetPositionForRow(int row, int antena)
        {
            foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
            {
                {
                    if (int.Parse(pic.Name.Substring(0, 1)) == row)
                    {
                        if (antena % 2 == 0)
                        {
                            pic.Click -= new System.EventHandler(pictureBoxOnClick_Right);
                        }
                        else
                        {
                            pic.Click -= new System.EventHandler(pictureBoxOnClick_Left);
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
                    switch (checkItem.Text)
                    {
                        case "ANTENA 1":
                            // Make picture box in row 1 cannot be clicked
                            unSetPositionForRow(1, 1);
                            break;
                        case "ANTENA 2":
                            unSetPositionForRow(1, 2);
                            break;
                        case "ANTENA 3":
                            unSetPositionForRow(2, 3);
                            break;
                        case "ANTENA 4":
                            unSetPositionForRow(2, 4);
                            break;
                        case "ANTENA 5":
                            unSetPositionForRow(3, 5);
                            break;
                        case "ANTENA 6":
                            unSetPositionForRow(3, 6);
                            break;
                        case "ANTENA 7":
                            unSetPositionForRow(4, 7);
                            break;
                        case "ANTENA 8":
                            unSetPositionForRow(4, 8);
                            break;
                        default:
                            break;
                    }
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

        private int getRowbyAntenName(string antena)
        {
            int row = 0 ;
            if (antena == "1" || antena == "2")
            {
                row = 1;
            }
            else if (antena == "3" || antena == "4")
            {
                row = 2;
            }
            else if (antena == "5" || antena == "6")
            {
                row = 3;
            }
            else if (antena == "7" || antena == "8")
            {
                row = 4;
            }

            return row;
        }
        private (int, string) getValueToInsertMST_Working(CheckBox antena)
        {
            int scan_col_start = 0;
            int right_col = 6;
            int CONST_MAX_RIGHT_COL = 6;
            int left_col = 1;
            int CONST_MIN_LEFT_COL = 1;
            string antenaNo = antena.Name;
            Boolean flgSelected = false;
            int antenna_row = getRowbyAntenName(antena.Name);
            foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
            {
                if (pic.ImageLocation == "selected.png")
                {
                    flgSelected = true;
                    if (int.Parse(pic.Name.Substring(0, 1)) == antenna_row){
                        // All antena in the right
                        if (int.Parse(antena.Name) % 2 == 0)
                        {
                            if (int.Parse(pic.Name.Substring(2, 1)) >= left_col)
                            {
                                left_col = int.Parse(pic.Name.Substring(2, 1));
                            }
                        }
                        // All antena in the left
                        else
                        {
                            if (int.Parse(pic.Name.Substring(2, 1)) <= right_col)
                            {
                                right_col = int.Parse(pic.Name.Substring(2, 1));
                            }
                        }


                    }
                }
            }

            if (int.Parse(antena.Name) % 2 == 0)
            {
               if (flgSelected == false )
               {
                    scan_col_start = CONST_MAX_RIGHT_COL;
               }
               else
                {
                    scan_col_start = left_col;

                }
                
            }
            else
            {
                if (flgSelected == false)
                {
                    scan_col_start = CONST_MIN_LEFT_COL;
                }
                else
                {
                    scan_col_start = right_col;
                }
                
            }
            return (scan_col_start, antenaNo);
        }


        private (int, int, string) getValueToInsertMST(CheckBox antena)
        {
            int rightAntena = 0;
            int leftAntena = 7;
            string antenaNo = "";
            foreach (CheckBox checkItem in settingLayer.Controls.OfType<CheckBox>())
            {
                if (checkItem.Checked)
                {
                    foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
                    {
                        if (pic.ImageLocation == "selected.png")
                        {
                            // All antena in the right
                            if (int.Parse(checkItem.Name) % 2 == 0)
                            {
                                // Lấy giá trị lớn nhất của row để làm start
                                for (var index = 0; index < 7; index++)
                                {
                                    // Xử lí chỉ trong 1 row
                                    if (int.Parse(pic.Name.Substring(2, 1)) == index)
                                    {
                                        //Lấy tên antena
                                        antenaNo = checkItem.Name;

                                        if (int.Parse(pic.Name.Substring(2, 1)) > rightAntena)
                                        {
                                            rightAntena = int.Parse(pic.Name.Substring(2, 1));
                                        }
                                    }
                                }
                            }
                            // All antena in the left
                            else
                            {
                                // Lấy giá trị nhỏ nhất của row để làm start
                                for (var index = 7; index > 0; index--)
                                {
                                    // Xử lí chỉ trong 1 row
                                    if (int.Parse(pic.Name.Substring(2, 1)) == index)
                                    {
                                        //Lấy tên antena
                                        antenaNo = checkItem.Name;
                                        if (int.Parse(pic.Name.Substring(2, 1)) < leftAntena)
                                        {
                                            leftAntena = int.Parse(pic.Name.Substring(2, 1));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return (leftAntena, rightAntena, antenaNo);
        }

        private int getScanColStartValue(int leftAntena, int rightAntena)
        {
            int value = 0;
            foreach (CheckBox checkItem in settingLayer.Controls.OfType<CheckBox>())
            {
                if (checkItem.Checked)
                {
                    // All antena in the right
                    if (int.Parse(checkItem.Name) % 2 == 0)
                    {
                        value = rightAntena;
                    }
                    // All antena in the left
                    else
                    {
                        value = leftAntena;
                    }
                }
            }
            return value;
        }


        private void btnSubmitOnClick(object sender, EventArgs e)
        {
            // Bug dự kiến: insert chỉ mỗi antena 2 nếu tick cả 2 antena
            // Get value of filed scan_col_start


            Task.Run(() => ApiClearPositionMSTAntena(Session.nameOfShelf).Wait());
            foreach (CheckBox checkItem in settingLayer.Controls.OfType<CheckBox>())
            {

                if (checkItem.Checked)
                {
                    //get thong tin antenna 
                    //var (leftAntena, rightAntena, antena) = getValueToInsertMST(checkItem);
                    //int value = getScanColStartValue(leftAntena, rightAntena);
                    var (scancolstart, antenaNo) = getValueToInsertMST_Working(checkItem);
                    Task.Run(() => ApiUpdatePositionMSTAntena(antenaNo, scancolstart)).Wait();


                }

            }
            DialogResult confirmResult = MessageBox.Show("Finish register MST Antena table", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);


        }

        private void LoadDataToScreen()
        {
            Wait wait = new Wait();
            wait.Visible = true;
            foreach (CheckBox antenaNo in settingLayer.Controls.OfType<CheckBox>())
            {
                foreach (CheckBox loadAntena in Session.antenaLoadList)
                {
                    if (loadAntena.Name == antenaNo.Name)
                    {
                        if (antenaNo.Checked == false)
                        {
                            antenaNo.Checked = true;

                        }
                    }
                }
            }

            foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
            {
                foreach(var item in Session.settingPosition)
                {
                    if (int.Parse(pic.Name.Substring(2, 1)) == item.col && int.Parse(pic.Name.Substring(0, 1)) == item.row)
                    {
                        pic.Load("selected.png");
                    }
                }
                
            }


                wait.Visible = false;

        }

        private async Task ApiClearPositionMSTAntena(string nameOfShelf)
        {
            try
            {

                HttpClient api_client = new HttpClient();
                api_client.BaseAddress = new Uri(Session.address_api);
                api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = "";

                json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    api_key = Session.api_key,
                    shelf_no = nameOfShelf
                }

                );

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await api_client.PostAsync(Session.clear_position_mst_antena, content);


                if (result.IsSuccessStatusCode)
                {

                    string resultContent = await result.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(resultContent);
                }
                else
                {
                    Console.WriteLine(result);
                }


            }
            catch (Exception)
            {
                Console.WriteLine("Failed to clear table MST Antena - ApiUpdatePositionMSTAntena");
            }
        }

        private async Task ApiLoadPositionMSTAntena(string nameOfShelf)
        {
            try
            {
                Session.antenaLoadList = new List<CheckBox>();
                Session.settingPosition = new List<SettingAntena>();
                HttpClient api_client = new HttpClient();
                api_client.BaseAddress = new Uri(Session.address_api);
                api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    api_key = Session.api_key,
                    shelf_no = nameOfShelf
                });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await api_client.PostAsync(Session.load_position_mst_antena, content);

                if (result.IsSuccessStatusCode)
                {

                    string resultContent = await result.Content.ReadAsStringAsync();
                    JObject JsonData = JObject.Parse(resultContent);

                    foreach (var item in JsonData["data"])
                    {
                        int row_api = Int32.Parse(item["row"].ToString());
                        int col_api = Int32.Parse(item["scan_col_start"].ToString());
                        string antenaNo = (string)item["antena_no"];

                        Session.settingPosition.Add(new SettingAntena { row = row_api, col = col_api });

                        foreach (CheckBox checkItem in settingLayer.Controls.OfType<CheckBox>())
                        {
                            if (checkItem.Name == antenaNo)
                            {
                                Session.antenaLoadList.Add(checkItem);
                            }

                        } 
                    }
                }
                else
                {
                    Console.WriteLine(result);
                }

            }
            catch (Exception)
            {
                Console.WriteLine("Failed to load table MST Antena - ApiLoadPositionMSTAntena \n");
            }
        }

        private async Task ApiUpdatePositionMSTAntena(string antena, int scan_col_start)
        {
            //Hanle shelfNo
            string shelfNo = Session.nameOfShelf;

            //Handle row 
            int row = 0;

            if (antena == "1" || antena == "2")
            {
                //antena = "1";
                row = 1;
            }
            else if (antena == "3" || antena == "4")
            {
                //antena = "2";
                row = 2;
            }
            else if (antena == "5" || antena == "6")
            {
                //antena = "3";
                row = 3;
            }
            else if (antena == "7" || antena == "8")
            {
                //antena = "4";
                row = 4;
            }

            //Handle col
            int col = 7;

            //Handle scan_col_end
            int scan_col_end = 7;


            try
            {

                HttpClient api_client = new HttpClient();
                api_client.BaseAddress = new Uri(Session.address_api);
                api_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                string json = "";

                json = System.Text.Json.JsonSerializer.Serialize(new
                {
                    api_key = Session.api_key,
                    shelf_no = shelfNo,
                    antena_no = antena,
                    row = row,
                    col = col,
                    scan_col_start = scan_col_start,
                    scan_col_end = scan_col_end
                }

                );
                   
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var result = await api_client.PostAsync(Session.update_position_mst_antena, content);


                if (result.IsSuccessStatusCode)
                {

                    string resultContent = await result.Content.ReadAsStringAsync();
                    JObject data = JObject.Parse(resultContent);
                    Console.WriteLine(resultContent);
                    //api_message = (string)data["message"];
                    //api_status = (string)data["code"];
                }
                else
                {
                    Console.WriteLine(result);
                }

                
            }
            catch (Exception)
            {
                Console.WriteLine("Failed to update table MST Antena - ApiUpdatePositionMSTAntena");
            }


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
            btnSubmit.Text = "REGISTER";
            btnSubmit.Height = 50;
            btnSubmit.Width = 100;
            btnSubmit.Click += new System.EventHandler(btnSubmitOnClick);
            settingLayer.Controls.Add(btnSubmit, 8, 4); 
            
            // Add button settingLayer - tableLayout
            Button btnLoad = new Button();
            btnLoad.Text = "LOAD";
            btnLoad.Height = 50;
            btnLoad.Width = 100;
            btnLoad.Click += new System.EventHandler(btnLoadOnClick);
            settingLayer.Controls.Add(btnLoad, 6, 4);

            // Add button settingLayer - tableLayout
            Button btnClear = new Button();
            btnClear.Text = "CLEAR";
            btnClear.Height = 50;
            btnClear.Width = 100;
            btnClear.Click += new System.EventHandler(btnClearOnClick);
            settingLayer.Controls.Add(btnClear, 5, 4);

            // Add pictureBox to settingLayer
            // Solution 1: Loop all settingLayer, find position not contain checkbox and insert picturebox
            // Solution 2: Manual insert
            // Solution 3: Loop with location


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

        private void btnClearOnClick(object sender, EventArgs e)
        {
            DialogResult warningPopUp = MessageBox.Show("All data on screen will be removed, are you sure?", "Confirm Diaglog", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
            if (warningPopUp == DialogResult.Yes)
            {
                foreach (PictureBox pic in settingLayer.Controls.OfType<PictureBox>())
                {
                    pic.Dock = DockStyle.Fill;
                    pic.Size = MaximumSize;
                    pic.SizeMode = PictureBoxSizeMode.StretchImage;
                    pic.Load("blank_background.png");
                    pic.Click -= new System.EventHandler(pictureBoxOnClick_Right);
                    pic.Click -= new System.EventHandler(pictureBoxOnClick_Left);

                }
                foreach (CheckBox antenaNo in settingLayer.Controls.OfType<CheckBox>())
                {
                    if (antenaNo.Checked == true)
                    {
                        antenaNo.Checked = false;
                    }
                }
            }
        }

        private void btnLoadOnClick(object sender, EventArgs e)
        {
            Task.Run(() => ApiLoadPositionMSTAntena(Session.nameOfShelf).Wait());
            LoadDataToScreen();
            DialogResult confirmResult = MessageBox.Show("Finish load MST Antena table", "Result", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);

        }

        private void settingLayer_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
