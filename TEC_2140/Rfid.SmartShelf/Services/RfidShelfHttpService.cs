using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Vjp.Rfid.SmartShelf.Interface;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Services
{
    public class RfidShelfHttpService : IRfidShelfHttpService
    {
        //static readonly HttpClient client = new HttpClient();
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public async Task<JanToItemInfoResponse> GetItemInfoByJanCode(string janCode)
        {
            JanToItemInfoResponse res = new JanToItemInfoResponse();

            try
            {
                var reqData = new JanToItemInfoRequest
                {
                    Api_Key = ConfigFile.ApiKey,
                    Jancode = janCode,
                    Goods_Name =""
                };

                var jsonDataReq = JsonConvert.SerializeObject(reqData);
                logger.Info($"GetItemInfoByJanCode send params {jsonDataReq}");
                using (var client = new HttpClient())
                {
                    var data = new StringContent(jsonDataReq, System.Text.Encoding.UTF8, "application/json");

                    var resMsg = await client.PostAsync(ConfigFile.JanToItemInfoUrl, data);

                    string resultContent = await resMsg.Content.ReadAsStringAsync();


                    res = JsonConvert.DeserializeObject<JanToItemInfoResponse>(resultContent);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Call api {ConfigFile.RfidToJanUrl} error.{ex.Message}");
                throw;
            }
            return res;

        }

        public async Task<RfidToJanResponse> GetJanByRfid(string rfid)
        {
            RfidToJanResponse res = new RfidToJanResponse();

            try
            {
                var reqData = new RfidToJanRequest
                {
                    Api_Key = ConfigFile.ApiKey,
                    Rfid = rfid
                };

                var jsonDataReq = JsonConvert.SerializeObject(reqData);

                logger.Info($"GetJanByRfid send params {jsonDataReq}");

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigFile.RfidToJanUrl);
                    var data = new StringContent(jsonDataReq, System.Text.Encoding.UTF8, "application/json");

                    var resMsg = await client.PostAsync("/api/v1/rfid_to_jan", data);

                    string resultContent = await resMsg.Content.ReadAsStringAsync();


                    res = JsonConvert.DeserializeObject<RfidToJanResponse>(resultContent);
                }
            }
            catch (Exception ex)
            {
                logger.Error($"Call api {ConfigFile.RfidToJanUrl} error.{ex.Message}");
                throw;
            }
            return res;

        }

        public async Task ItemInOutShelfChangeNotify()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigFile.ItemShiftNotifyUrl);
                    //var content = new FormUrlEncodedContent(new[]
                    //{
                    //new KeyValuePair<string, string>("rfid", "ABC")
                    //});
                    client.Timeout = TimeSpan.FromMilliseconds(2000);
                    var result = await client.PostAsync("/notifys", null);
                    string resultContent = await result.Content.ReadAsStringAsync();
                    Console.WriteLine(resultContent);
                    logger.Info($"ItemInOutShelfChangeNotify result {resultContent}");
                }
            }catch(Exception ex)
            {
                logger.Error($"ItemInOutShelfChangeNotify error {ex.Message}");
            }
            
        }

        

    }
}
