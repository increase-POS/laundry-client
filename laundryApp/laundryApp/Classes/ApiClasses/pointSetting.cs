using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp;
using laundryApp.ApiClasses;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace laundryApp.Classes.ApiClasses
{
    public class pointSetting
    {
        public int pointSettingId { get; set; }
        public int points { get; set; }
        public int pointsAccum { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public string notes { get; set; }
        public byte isActive { get; set; }
        public bool canDelete { get; set; }
        ////////////////////////////////////////
        ///

        public async Task<List<pointSetting>> Get()
        {
            List<pointSetting> items = new List<pointSetting>();
            IEnumerable<Claim> claims = await APIResult.getList("pointSetting/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<pointSetting>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }


        public async Task<pointSetting> GetById(int itemId)
        {
            pointSetting item = new pointSetting();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("pointSetting/GetById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<pointSetting>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }


        
        public async Task<int> Save(pointSetting item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "pointSetting/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }
        public async Task<int> Delete(int itemId, int userId, bool final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "pointSetting/Delete";
            return await APIResult.post(method, parameters);
        }

         


    }
}
