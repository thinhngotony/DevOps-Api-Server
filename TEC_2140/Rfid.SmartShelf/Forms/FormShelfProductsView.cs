using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Vjp.Rfid.SmartShelf.Helper;
using Vjp.Rfid.SmartShelf.Models;
using Vjp.Rfid.SmartShelf.Services;

namespace Vjp.Rfid.SmartShelf.Forms
{
    public partial class FormShelfProductsView : Form
    {
        private ListView myListView;
        private ImageList imageList = new ImageList();
        private readonly static RfidShelfProductService rfidShelfProductService = new RfidShelfProductService();
        private List<RfidShelfProduct> rfidCurrentShelfProducts;
        private List<RfidShelfProductView> rfidCurrentShelfProductsView;
        private static string appDirNamePath = Path.Combine(Path.GetTempPath(), Assembly.GetExecutingAssembly().GetName().Name + "\\" + Assembly.GetExecutingAssembly().GetName().Version.ToString());

        private int MAX_COL_PER_SHELF = 4;
        private int MAX_ROW_PER_SHELF = 4;
        private string[,] shelftProductsView;
        private int borderSize = 2;

        private readonly ImageService imageService = new ImageService();

        #region"constructor"
        public FormShelfProductsView()
        {
            InitializeComponent();

            this.Padding = new Padding(borderSize);//Border size
            //this.BackColor = Color.FromArgb(98, 102, 244);//Border color

        }

        #endregion

        #region"event"
        private void FormReOrderShelfProducts_Load(object sender, EventArgs e)
        {

            DeleteTempDirectory(appDirNamePath);

            imageList.ImageSize = new Size(128, 128);

            //get shelf product list
            rfidCurrentShelfProducts = rfidShelfProductService.GetRfidShelfProducts();

            rfidCurrentShelfProductsView = RfidShelfProductToView(rfidCurrentShelfProducts);

            LoadImagesToListView();

            InitShelfItems(rfidCurrentShelfProducts);

            ListViewInsertionMark();

        }
        private void FormReOrderShelfProducts_FormClosed(object sender, FormClosedEventArgs e)
        {
            imageList.Images.Clear();
            imageList.Dispose();
        }
        public List<RfidShelfProductView> RfidShelfProductToView(List<RfidShelfProduct> list)
        {
            List<RfidShelfProductView> newProductList = new List<RfidShelfProductView>();

            foreach (RfidShelfProduct item in list)
            {
                RfidShelfProductView pv = new RfidShelfProductView();
                pv.ShelfNo = item.ShelfNo;
                pv.ShelfColIndex = item.ShelfColIndex;
                pv.Rfid = item.Rfid;
                pv.IsbnCode = item.IsbnCode;
                pv.ProductName = item.ProductName;

                pv.ImgUrl = imageService.GetResourceFromOpenDb(item.IsbnCode).Summary.Cover  ;

                if (pv.ImgUrl != null)
                {
                    string imgPath = SaveImageFromUrl(pv.ImgUrl , pv.Rfid);
                    if(!string.IsNullOrEmpty(imgPath))
                    {
                        pv.ImgLocalPath = imgPath;
                    }
                }
                
                newProductList.Add(pv);
            }
            

            return newProductList;
        }

        private void DeleteTempDirectory(string directoryPathName)
        {
            try
            {
                if (!Directory.Exists(directoryPathName))
                {
                    Directory.CreateDirectory(directoryPathName);
                }
                else
                {
                    Directory.Delete(directoryPathName, true);
                    Directory.CreateDirectory(directoryPathName);
                }

            }
            catch (Exception ex)
            {

            }
        }

        private string SaveImageFromUrl(string imgUrl , string tagName)
        {
            string imgPath = "";
            try
            {
                imgPath = Path.Combine(appDirNamePath, tagName );

                imgPath = Ultil.SaveImage(imgUrl, imgPath, ImageFormat.Png);
            }
            catch(Exception e)
            {
                return "";
            }
            

            return imgPath;

        }

        private void LoadImagesToListView()
        {

            String[] imageFiles = Directory.GetFiles(appDirNamePath);
            foreach (var file in imageFiles)
            {
                var item = rfidCurrentShelfProductsView.SingleOrDefault(x => x.ImgLocalPath == file);
                //Add images to Imagelist
                if (item != null)
                {
                    imageList.Images.Add( item.Rfid, Image.FromStream(new MemoryStream(File.ReadAllBytes(file))));
                    //byte[] imageBytes = File.ReadAllBytes(file);

                    //using (var ms = new MemoryStream(imageBytes))
                    //{
                    //    var image = Image.FromStream(ms);
                    //    imageList.Images.Add(product.Rfid, image);
                    //}
                    //Image.FromStream(new MemoryStream(File.ReadAllBytes(file)));

                }
                
            }
        }

        // Starts the drag-and-drop operation when an item is dragged.
        private void myListView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            myListView.DoDragDrop(e.Item, DragDropEffects.Move);
        }

        // Sets the target drop effect.
        private void myListView_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        // Moves the insertion mark as the item is dragged.
        private void myListView_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the mouse pointer.
            Point targetPoint =
                myListView.PointToClient(new Point(e.X, e.Y));

            // Retrieve the index of the item closest to the mouse pointer.
            int targetIndex = myListView.InsertionMark.NearestIndex(targetPoint);

            // Confirm that the mouse pointer is not over the dragged item.
            if (targetIndex > -1)
            {
                // Determine whether the mouse pointer is to the left or
                // the right of the midpoint of the closest item and set
                // the InsertionMark.AppearsAfterItem property accordingly.
                Rectangle itemBounds = myListView.GetItemRect(targetIndex);
                if (targetPoint.X > itemBounds.Left + (itemBounds.Width / 2))
                {
                    myListView.InsertionMark.AppearsAfterItem = true;
                }
                else
                {
                    myListView.InsertionMark.AppearsAfterItem = false;
                }
            }

            // Set the location of the insertion mark. If the mouse is
            // over the dragged item, the targetIndex value is -1 and
            // the insertion mark disappears.
            myListView.InsertionMark.Index = targetIndex;
        }

        // Removes the insertion mark when the mouse leaves the control.
        private void myListView_DragLeave(object sender, EventArgs e)
        {
            myListView.InsertionMark.Index = -1;
        }

        // Moves the item to the location of the insertion mark.
        private void myListView_DragDrop(object sender, DragEventArgs e)
        {
            // Retrieve the index of the insertion mark;
            int targetIndex = myListView.InsertionMark.Index;

            // If the insertion mark is not visible, exit the method.
            if (targetIndex == -1)
            {
                return;
            }

            // If the insertion mark is to the right of the item with
            // the corresponding index, increment the target index.
            if (myListView.InsertionMark.AppearsAfterItem)
            {
                targetIndex++;
            }

            //Console.WriteLine($"targetIndex {targetIndex}");

            // Retrieve the dragged item.
            ListViewItem draggedItem =
                (ListViewItem)e.Data.GetData(typeof(ListViewItem));

            //Console.WriteLine($"draggedItem {draggedItem.Text}");
            // Insert a copy of the dragged item at the target index.
            // A copy must be inserted before the original item is removed
            // to preserve item index values. 
            myListView.Items.Insert(
                targetIndex, (ListViewItem)draggedItem.Clone());


            // Remove the original copy of the dragged item.
            myListView.Items.Remove(draggedItem);


        }

        private void listView_DrawItem(object sender, DrawListViewItemEventArgs e)
        {
            Color textColor = SystemColors.WindowText;
            if (e.Item.Selected)
            {
                if (myListView.Focused)
                {
                    textColor = SystemColors.HighlightText;
                    e.Graphics.FillRectangle(SystemBrushes.Highlight, e.Bounds);
                }
                else if (!myListView.HideSelection)
                {
                    textColor = SystemColors.ControlText;
                    e.Graphics.FillRectangle(SystemBrushes.Control, e.Bounds);
                }
            }
            else
            {
                using (SolidBrush br = new SolidBrush(myListView.BackColor))
                {
                    e.Graphics.FillRectangle(br, e.Bounds);
                }
            }

            e.Graphics.DrawRectangle(Pens.Red, e.Bounds);
            TextRenderer.DrawText(e.Graphics, e.Item.Text, myListView.Font, e.Bounds,
                                  textColor, Color.Empty,
                                  TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        #endregion

        #region"methods"
        private void InitShelfItems(List<RfidShelfProduct> rfidShelfProducts)
        {
            shelftProductsView = new string[MAX_ROW_PER_SHELF, MAX_COL_PER_SHELF];

            for (int i = 0; i < MAX_ROW_PER_SHELF; i++)
            {
                for (int j = 0; j < MAX_COL_PER_SHELF; j++)
                {

                    shelftProductsView[i, j] = GetShelfProductInfoByCell(rfidShelfProducts, i + 1, j + 1);
                }
            }
        }

        private string GetShelfProductInfoByCell(List<RfidShelfProduct> rfidShelfProducts, int row, int col)
        {
            string result = "";
            if (row >= 0 && col >= 0)
            {
                //find row
                //var shelfProduct = rfidShelfProducts.Find(x => x.ShelfColIndex.ToString() == row.ToString() + col.ToString());
                var shelfProduct = rfidShelfProducts.Find(x => ( x.ShelfNo.ToString() == row.ToString() && x.ShelfColIndex.ToString() == col.ToString()));
                if (shelfProduct != null)
                {
                    result = $"{shelfProduct.ProductName} {shelfProduct.Rfid}";

                    
                }

            }
            return result;
        }

        public void ListViewInsertionMark()
        {
            // Initialize myListView.
            myListView = new ListView();
            myListView.Dock = DockStyle.Fill;
            myListView.View = View.LargeIcon;
            myListView.MultiSelect = false;
            myListView.LargeImageList = imageList;
            myListView.SmallImageList  = imageList;

            //myListView.TileSize = new Size(128, 128);
            //myListView.View = View.Tile;
            //myListView.OwnerDraw = true;
            //myListView.DrawItem += listView_DrawItem;

            //myListView.ListViewItemSorter = new ListViewIndexComparer();

            // Initialize the insertion mark.
            myListView.InsertionMark.Color = Color.Green;

            // Add items to myListView.
            for (int i = 0; i < MAX_ROW_PER_SHELF; i++)
            {
                for (int j = 0; j < MAX_COL_PER_SHELF; j++)
                {
                    //var item = rfidCurrentShelfProductsView.FirstOrDefault(p => p.ShelfColIndex.ToString() == (i+1).ToString() + (j+1).ToString());
                    var item = rfidCurrentShelfProductsView.FirstOrDefault(p => p.ShelfNo.ToString() == (i + 1).ToString() && p.ShelfColIndex.ToString() == (j + 1).ToString());
                    if (item != null)
                    {
                        myListView.Items.Add(shelftProductsView[i, j] , item.Rfid);
                    }
                    else
                    {
                        myListView.Items.Add(shelftProductsView[i, j]);
                    }
                    
                }
            }

            // Initialize the drag-and-drop operation when running
            // under Windows XP or a later operating system.
            if (OSFeature.Feature.IsPresent(OSFeature.Themes))
            {
                myListView.AllowDrop = true;
                myListView.ItemDrag += new ItemDragEventHandler(myListView_ItemDrag);
                myListView.DragEnter += new DragEventHandler(myListView_DragEnter);
                myListView.DragOver += new DragEventHandler(myListView_DragOver);
                myListView.DragLeave += new EventHandler(myListView_DragLeave);
                myListView.DragDrop += new DragEventHandler(myListView_DragDrop);
            }

            // Initialize the form.
            panelMain.Controls.Add(myListView);

        }

        #endregion

        #region"other class"
        // Sorts ListViewItem objects by index.
        private class ListViewIndexComparer : System.Collections.IComparer
        {
            public int Compare(object x, object y)
            {
                return ((ListViewItem)x).Index - ((ListViewItem)y).Index;
            }
        }
        #endregion


    }
}
