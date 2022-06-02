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
    public class ResidentialSectors
    {
        public int residentSecId { get; set; }
        public string name { get; set; }
        public string notes { get; set; }
        public byte isActive { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Boolean canDelete { get; set; }
        public decimal deliveryTime { get; set; }

        public async Task<List<ResidentialSectors>> Get()
        {
            List<ResidentialSectors> items = new List<ResidentialSectors>();
            IEnumerable<Claim> claims = await APIResult.getList("ResidentialSectors/get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ResidentialSectors>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<ResidentialSectors> GeById(int itemId)
        {
            ResidentialSectors item = new ResidentialSectors();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ResidentialSectors/GeById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<ResidentialSectors>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<int> Save(ResidentialSectors item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ResidentialSectors/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<int> Delete(int itemId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "ResidentialSectors/Delete";
           return await APIResult.post(method, parameters);
        }

        public async Task<List<ResidentialSectors>> GetResSectorsByUserId(int membershipId)
        {
            List<ResidentialSectors> items = new List<ResidentialSectors>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", membershipId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ResidentialSectors/GetResSectorsByUserId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<ResidentialSectors>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));

                }
            }
            return items;
        }
    }
}
