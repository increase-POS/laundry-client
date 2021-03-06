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
    public class ItemsUnitsServices
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
        public string unitName { get; set; }
        public string itemName { get; set; }
        public string ServiceName { get; set; }
        public Nullable<int> itemId { get; set; }
        public Nullable<int> unitId { get; set; }

        ////////////////////////////////////////
        ///

        public async Task<List<ItemsUnitsServices>> Get()
        {
            List<ItemsUnitsServices> items = new List<ItemsUnitsServices>();
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnitsServices/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ItemsUnitsServices>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }


        public async Task<ItemsUnitsServices> GetById(int itemId)
        {
            ItemsUnitsServices item = new ItemsUnitsServices();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnitsServices/GetById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<ItemsUnitsServices>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }


        
        public async Task<int> Save(ItemsUnitsServices item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsUnitsServices/Save";
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
            string method = "ItemsUnitsServices/Delete";
            return await APIResult.post(method, parameters);
        }

        public async Task<List<ItemsUnitsServices>> GetIUServicesByServiceId(int serviceId)
        {
            List<ItemsUnitsServices> items = new List<ItemsUnitsServices>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", serviceId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnitsServices/GetIUServicesByServiceId", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ItemsUnitsServices>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }


        public async Task<int> UpdateIUServiceList(List<ItemsUnitsServices> newList, int serviceId, int updateUserId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsUnitsServices/UpdateIUServiceList";
            var newListParameter = JsonConvert.SerializeObject(newList);
            parameters.Add("newList", newListParameter);
            parameters.Add("serviceId", serviceId.ToString());
            parameters.Add("updateUserId", updateUserId.ToString());
            return await APIResult.post(method, parameters);
        }

        public async Task<int> UpdateCostByServiceId(  int serviceId, int updateUserId,decimal cost)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsUnitsServices/UpdateCostByServiceId";
         
            parameters.Add("cost", cost.ToString());
            parameters.Add("serviceId", serviceId.ToString());
            parameters.Add("updateUserId", updateUserId.ToString());
            return await APIResult.post(method, parameters);
        }
        public async Task<int> UpdateInstantByServiceId(int serviceId, int updateUserId, decimal instant)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsUnitsServices/UpdateInstantByServiceId";

            parameters.Add("instant", instant.ToString());
            parameters.Add("serviceId", serviceId.ToString());
            parameters.Add("updateUserId", updateUserId.ToString());
            return await APIResult.post(method, parameters);
        }
        public async Task<int> UpdateNormalByServiceId(int serviceId, int updateUserId, decimal normal)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsUnitsServices/UpdateNormalByServiceId";

            parameters.Add("normal", normal.ToString());
            parameters.Add("serviceId", serviceId.ToString());
            parameters.Add("updateUserId", updateUserId.ToString());
            return await APIResult.post(method, parameters);
        }
    }
}
