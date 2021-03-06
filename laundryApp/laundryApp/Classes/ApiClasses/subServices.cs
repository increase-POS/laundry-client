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
    public class subServices
    {
        public int subServiceId { get; set; }
        public Nullable<int> agentSubId { get; set; }
        public Nullable<int> subId { get; set; }
        public Nullable<int> serviceId { get; set; }
        public string serviceType { get; set; }
        public int count { get; set; }
        public int remain { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        ////////////////////////////////////////
        ///

        public async Task<List<subServices>> Get()
        {
            List<subServices> items = new List<subServices>();
            IEnumerable<Claim> claims = await APIResult.getList("subServices/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<subServices>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }


        public async Task<subServices> GetById(int itemId)
        {
            subServices item = new subServices();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("subServices/GetById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<subServices>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }


        
        public async Task<int> Save(subServices item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "subServices/Save";
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
            string method = "subServices/Delete";
            return await APIResult.post(method, parameters);
        }

         


    }
}
