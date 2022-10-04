using System.Collections.Generic;

namespace Vjp.Rfid.SmartShelf.Models
{
    public class JanToItemInfoResponse
    {
        public string Code { get; set; }
        public List<JanToItemInfoDataResponse> Data { get; set; }
        public string Message { get; set; }

    }

    public class JanToItemInfoDataResponse
    {
        public JanToItemInfoDataResponse()
        {
        }

        public string drgm_create { get; set; }
        public string drgm_pos_shop_cd { get; set; }
        public string drgm_com_shop_cd { get; set; }

        public string rf_goods_type { get; set; }
        public string rf_goods_cd_type { get; set; }
        public string drgm_rfid_cd { get; set; }
        public string drgm_jan { get; set; }
        public string drgm_jan2 { get; set; }
        public string drgm_goods_name { get; set; }
        public string drgm_goods_name_kana { get; set; }
        public string drgm_artist { get; set; }
        public string drgm_artist_kana { get; set; }
        public string drgm_maker_cd { get; set; }
        public string drgm_maker_name { get; set; }
        public string drgm_genre_cd { get; set; }
        public string drgm_maker_name_kana { get; set; }
        public string drgm_c_code { get; set; }
        public string drgm_selling_date { get; set; }
        public int drgm_price_tax_off { get; set; }
        public decimal? drgm_cost_rate { get; set; }
        public int? drgm_cost_price { get; set; }
        public string drgm_media_cd { get; set; }
        public int? bqsq_shop_goods_price { get; set; }
        public int? bqsq_shop_goods_price_intax { get; set; }

    }

}
