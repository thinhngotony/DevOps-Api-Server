using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Vjp.Rfid.SmartShelf.Helper;
using Vjp.Rfid.SmartShelf.Models;

namespace Vjp.Rfid.SmartShelf.Services
{
    public class ImageService
    {
        //https://api.openbd.jp/v1/get?isbn=978-4-7808-0204-7&pretty
        private readonly static string openDbApiUrl = "https://api.openbd.jp/v1/";
        private readonly static HttpClient client = new HttpClient();

        public ImageService()
        {
            
        }

        public OpenDbInfo GetResourceFromOpenDb(string isbn)
        {
            //https://api.openbd.jp/v1/get?isbn=978-4-7808-0204-7&pretty
            OpenDbInfo openDbInfo = new OpenDbInfo();

            try
            {
                var response = Task.Run(async () => {

                    if (client.BaseAddress == null)
                    {
                        client.BaseAddress = new Uri(openDbApiUrl);
                    }

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    //return await client.GetStringAsync("get?isbn=@isbn&pretty".Replace("@isbn", long.Parse(isbn).ToString("000-0-0000-0000-0")));
                    return await client.GetStringAsync("get?isbn=@isbn".Replace("@isbn", isbn));
                });

                var result = response.Result;

                var data = JsonConvert.DeserializeObject<List<ExpandoObject>>(result);

                if (data != null)
                {

                    var summary = data[0].FirstOrDefault<ExpandoObject>("summary");
                    if (summary != null)
                    {
                        openDbInfo.Summary.Cover = summary.FirstOrDefault<string>("cover");
                    }

                }
            }
            catch(Exception ex)
            {

            }
            return openDbInfo;

        }


    }
}
