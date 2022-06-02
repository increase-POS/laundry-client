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
    public class Section
    {
        public int sectionId { get; set; }
        public string name { get; set; }
        public string details { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> branchId { get; set; }
        public byte isActive { get; set; }
        public string notes { get; set; }
        public byte isFreeZone { get; set; }
        public string type { get; set; }


        public string branchName { get; set; }

        public Boolean canDelete { get; set; }

         
        public async Task<List<Section>> Get()
        {
            List<Section> items = new List<Section>();
            IEnumerable<Claim> claims = await APIResult.getList("Sections/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Section>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<Section> getById(int itemId)
        {
            Section item = new Section();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Sections/GetSectionByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Section>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<List<Section>> getBranchSections(int branchId)
        {
            List<Section> items = new List<Section>();
            Section item = new Section();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", branchId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Sections/getBranchSections", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Section>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<int> save(Section item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Sections/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<int> delete(int sectionId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", sectionId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "Sections/Delete";
           return await APIResult.post(method, parameters);
        }
    }
}
