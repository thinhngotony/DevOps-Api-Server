package main

import (
	"main/controllers"

	"github.com/gin-gonic/gin"
	_ "github.com/go-sql-driver/mysql"
)

func main() {
	gin.SetMode(gin.ReleaseMode)
	r := gin.Default()
	r.POST("/api/v1/rfid_to_jan", controllers.GetJanCodefromRFID)
	r.POST("/api/v1/rfids_to_jans", controllers.GetJanCodefromRFIDList)
	r.POST("/api/v1/rfids_to_status", controllers.GetStatusfromRFIDList)
	r.POST("/api/v1/jan_to_rfid", controllers.GetRFIDfromJanCode)
	r.POST("/api/v1/insert_rfid_master", controllers.InsertDataToMasterTable)
	r.POST("/api/v1/delete_rfids_master", controllers.DeleteDataFromMasterTable)
	r.POST("/api/v1/insert_rfid_log", controllers.InsertDataToLogTable)
	r.POST("/api/v1/insert_rfid_logs", controllers.InsertMultiDataToLogTable)
	r.POST("/api/v1/search", controllers.SearchfromJan)
	r.POST("/api/v1/search_fromBQ", controllers.SearchFromJan_BQ)
	r.POST("/api/v1/get_gate_setting", controllers.GetGateSetting)
	r.POST("/api/v1/set_gate_setting", controllers.SetGateSetting)
	r.POST("/api/v1/get_smart_self_setting", controllers.GetSmartSelfSetting)
	r.POST("/api/v1/get_smart_self_names", controllers.GetShelfNames)
	r.POST("/api/v1/set_smart_self_setting", controllers.SetSmartSelfSetting)
	r.POST("/api/v1/rfid_to_info", controllers.GetInfofromRFID)
	r.POST("/api/v1/rfid_to_status_smartself", controllers.GetSmartSelfLogSetting)

	// New Transactions
	r.POST("/api/v1/insert_transaction", controllers.InsertDataToTransactionTable)
	r.POST("/api/v1/set_is_saved_transaction", controllers.Set_is_saved_transaction)
	r.POST("/api/v1/get_not_saved_transaction", controllers.Get_not_saved_transaction)

	// Test API
	r.POST("/api/auth/signin", controllers.GetTokenByAuth)
	r.POST("/api/v1/unload", controllers.UnloadInventory)
	r.POST("/api/v1/load", controllers.Inventory)
	r.POST("/api/v1/inventory", controllers.Inventory_2)

	r.POST("/api/v1/get_blacklist", controllers.Get_blacklist_OLD)

	r.GET("/api/v1/inventory", controllers.Get_blacklist)

	//New SmartShelf
	r.POST("/api/v1/reset_smartself", controllers.SetSmartSelfLocation)
	r.POST("/api/v1/insert_more_info_smartself", controllers.SetSmartSelfMoreInfo)
	r.POST("/api/v1/get_smartself_location", controllers.GetSmartSelfLocation)
	r.POST("/api/v1/get_smartself_location_by_col", controllers.GetSmartSelfLocationByCol)
	if err := r.Run("0.0.0.0:8027"); err != nil {
		panic(err.Error())
	}
}
