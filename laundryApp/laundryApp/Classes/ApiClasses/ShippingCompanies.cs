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
    public class ShippingCompanies
    {

        public int shippingCompanyId { get; set; }
        public string name { get; set; }
        public decimal realDeliveryCost { get; set; }
        public decimal deliveryCost { get; set; }
        public string deliveryType { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public byte isActive { get; set; }
        public decimal balance { get; set; }
        public byte balanceType { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string fax { get; set; }
        public string address { get; set; }
        public bool canDelete { get; set; }



        public async Task<List<ShippingCompanies>> Get()
        {
            List<ShippingCompanies> items = new List<ShippingCompanies>();
            IEnumerable<Claim> claims = await APIResult.getList("ShippingCompanies/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ShippingCompanies>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<List<ShippingCompanies>> GetForAccount(string payType)
        {
            List<ShippingCompanies> items = new List<ShippingCompanies>();
            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("payType", payType.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("ShippingCompanies/GetForAccount", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ShippingCompanies>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<ShippingCompanies> GetByID(int itemId)
        {
            ShippingCompanies item = new ShippingCompanies();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ShippingCompanies/GetByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<ShippingCompanies>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<int> save(ShippingCompanies item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ShippingCompanies/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<int> delete(int posId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", posId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "ShippingCompanies/Delete";
           return await APIResult.post(method, parameters);
        }
    }
}
