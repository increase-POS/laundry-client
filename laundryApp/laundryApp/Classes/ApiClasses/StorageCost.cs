using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace laundryApp.Classes
{
    public class StorageCost
    {
        public int storageCostId { get; set; }
        public string name { get; set; }
        public decimal cost { get; set; }
        public string notes { get; set; }
        public byte isActive { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }


        public bool canDelete { get; set; }
         

        public async Task<List<StorageCost>> Get()
        {
            List<StorageCost> items = new List<StorageCost>();
            IEnumerable<Claim> claims = await APIResult.getList("StorageCost/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<StorageCost>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<StorageCost> getById(int itemId)
        {
            StorageCost item = new StorageCost();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("StorageCost/GetByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<StorageCost>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<int> save(StorageCost item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "StorageCost/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<int> delete(int storageCostId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", storageCostId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "StorageCost/Delete";
           return await APIResult.post(method, parameters);
        }
        public async Task<int> setCostToUnits(List<int> itemUnitsIds, int storageCostId,int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "StorageCost/setCostToUnits";
            var myContent = JsonConvert.SerializeObject(itemUnitsIds);
            parameters.Add("itemUnitsIds", myContent);
            parameters.Add("storageCostId", storageCostId.ToString());
            parameters.Add("userId", userId.ToString());
            return await APIResult.post(method, parameters);
        }
        public async Task<List<ItemUnit>> GetStorageCostUnits(int storageCostId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("storageCostId", storageCostId.ToString());
            List<ItemUnit> items = new List<ItemUnit>();
            IEnumerable<Claim> claims = await APIResult.getList("StorageCost/GetStorageCostUnits",parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
    }
}
