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
    public class itransIUServices
    {
        public int itemUnitServiceId { get; set; }
        public decimal normalPrice { get; set; }
        public decimal instantPrice { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> serviceId { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public decimal cost { get; set; }

        ////////////////////////////////////////
        ///

        public async Task<List<itransIUServices>> Get()
        {
            List<itransIUServices> items = new List<itransIUServices>();
            IEnumerable<Claim> claims = await APIResult.getList("itransIUServices/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<itransIUServices>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }


        public async Task<itransIUServices> GetById(int itemId)
        {
            itransIUServices item = new itransIUServices();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("itransIUServices/GetById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<itransIUServices>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }


        
        public async Task<int> Save(itransIUServices item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "itransIUServices/Save";
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
            string method = "itransIUServices/Delete";
            return await APIResult.post(method, parameters);
        }

         


    }
}
