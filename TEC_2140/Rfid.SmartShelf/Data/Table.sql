CREATE TABLE `drfid_log_move` (
  `dlm_id` MEDIUMINT NOT NULL AUTO_INCREMENT,
  `dlm_date` date DEFAULT '0000-00-00',
  `dlm_rfid_cd` varchar(45) COLLATE utf8mb3_unicode_ci DEFAULT NULL,
  `dlm_cnt` int(11) DEFAULT NULL,
  `dlm_outdate` timestamp NULL DEFAULT NULL,
  `dlm_indate` timestamp NULL DEFAULT NULL,
   PRIMARY KEY (`dlm_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_unicode_ci;
--index
create index dlm_rfid_cd_index  on `drfid_log_move`(`dlm_rfid_cd`);

--棚番号、列番号、RFIDタグ情報、ISBNコード、商品名
CREATE TABLE `drfid_product_pos` (
  `dpp_id` MEDIUMINT NOT NULL AUTO_INCREMENT,
  `dpp_shelf_pos` int(7) DEFAULT NULL,
  `dpp_shelf_col_pos` int(7) DEFAULT NULL,
  `dpp_rfid_cd` varchar(45) COLLATE utf8mb3_unicode_ci DEFAULT NULL,
  `dpp_isbn` varchar(13) COLLATE utf8mb3_unicode_ci DEFAULT NULL,
  `dpp_product_name` varchar(200) COLLATE utf8mb3_unicode_ci DEFAULT NULL,
  PRIMARY KEY (`dpp_id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3 COLLATE=utf8mb3_unicode_ci;
--index
create index dpp_rfid_cd_index  on `drfid_product_pos`(`dpp_rfid_cd`);