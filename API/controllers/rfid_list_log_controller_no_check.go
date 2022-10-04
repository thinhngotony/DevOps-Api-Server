package controllers

import (
	"main/db_client"
	"main/utils"
	"net/http"

	"github.com/gin-gonic/gin"
	"github.com/go-playground/validator"
)

func InsertMultiDataToLogTableNoCheck(c *gin.Context) {
	var RFID_list []string
	var Mode_list []string
	var reqBody InsertLogList
	var respBody respond

	if err := c.ShouldBindJSON(&reqBody); err != nil {
		c.JSON(http.StatusOK, gin.H{
			"code":    utils.INVALID_FORMAT_CODE,
			"data":    nil,
			"error":   nil,
			"message": "jsonリクエストの本文の形式が正しくありません。",
		})
		return

	} else if err := validator.New().Struct(reqBody); err != nil {
		c.JSON(http.StatusOK, gin.H{
			"code":    utils.MISSING_DATA_CODE,
			"data":    nil,
			"error":   nil,
			"message": "リクエストパラメータが不足しています。",
		})
		return

	} else if err := utils.VerifyApiKey(reqBody.ApiKey); !err {
		c.JSON(http.StatusOK, gin.H{
			"code":    utils.INVALID_API_KEY_CODE,
			"data":    nil,
			"error":   nil,
			"message": "APIキーが正しくありません。",
		})
		return
	}

	db, _ := db_client.DbConnection()
	defer db.Close()
	for i := 0; i < len(reqBody.Data); i++ {

		RFID_list = append(RFID_list, reqBody.Data[i].RFID)
		Mode_list = append(Mode_list, reqBody.Data[i].Mode)

	}
	if valid_rfid, empty_rfid, _ := db_client.CheckExistRFID_Master(db, RFID_list); len(empty_rfid) != 0 {
		for i := 0; i < len(valid_rfid); i++ {
			if success, create_date, shop_code, _ := db_client.InsertSingleRowToLogTable(db, RFID_list[i], Mode_list[i]); success {
				respBody = append(respBody, InsertLogRespond{create_date, reqBody.Data[i].RFID, reqBody.Data[i].Mode, shop_code})
			}
		}
		c.JSON(http.StatusOK, gin.H{
			"code":    utils.DATA_NOT_EXIST_CODE,
			"data":    respBody,
			"error":   empty_rfid,
			"message": "不正なRFIDコードが存在しています。",
		})
	} else {
		for i := 0; i < len(valid_rfid); i++ {
			if success, create_date, shop_code, _ := db_client.InsertSingleRowToLogTable(db, RFID_list[i], Mode_list[i]); success {
				respBody = append(respBody, InsertLogRespond{create_date, reqBody.Data[i].RFID, reqBody.Data[i].Mode, shop_code})
			}
		}
		if len(reqBody.Data) == 0 {
			c.JSON(http.StatusOK, gin.H{
				"code":    utils.MISSING_DATA_CODE,
				"data":    nil,
				"error":   nil,
				"message": "リクエストパラメータが不足しています。",
			})
		}

		if len(respBody) != 0 {
			c.JSON(http.StatusOK, gin.H{
				"code":    utils.SUCCESSFULLY_CODE,
				"data":    respBody,
				"error":   nil,
				"message": "登録に成功しました。",
			})
		} else {
			c.JSON(http.StatusOK, gin.H{
				"code":    utils.UNKNOWN_ERROR_CODE,
				"data":    nil,
				"error":   nil,
				"message": "システムの不明なエラーです。",
			})
		}

	}
}
