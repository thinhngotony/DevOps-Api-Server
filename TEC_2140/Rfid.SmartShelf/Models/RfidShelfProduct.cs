namespace Vjp.Rfid.SmartShelf.Models
{
    /// <summary>
    /// 棚番号、列番号、RFIDタグ情報、ISBNコード(ISBNcode)、商品名
    /// </summary>
    public class RfidShelfProduct
    {
        /// <summary>
        /// 棚番号
        /// </summary>
        public int ShelfNo { get; set; }
        /// <summary>
        /// 列番号
        /// </summary>
        public int ShelfColIndex { get; set; }
        /// <summary>
        /// RFIDタグ情報
        /// </summary>
        public string Rfid{ get; set; }
        /// <summary>
        /// ISBNコード
        /// </summary>
        public string IsbnCode { get; set; }
        /// <summary>
        /// 商品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// Scanner Device Name
        /// </summary>
        public string ScannerName { get; set; }

        /// <summary>
        /// Scanner Name Map with Shelf Name
        /// </summary>
        public string ShelfName { get; set; }

        public string JanCd { get; set; }

    }
}
