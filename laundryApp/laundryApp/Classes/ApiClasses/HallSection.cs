using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace laundryApp.Classes.ApiClasses
{
    public class HallSection
    {
        public int sectionId { get; set; }
        public string name { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> branchId { get; set; }
        public byte isActive { get; set; }
        public string notes { get; set; }
        public string details { get; set; }


        public string branchName { get; set; }

        public Boolean canDelete { get; set; }

        public async Task<List<HallSection>> Get()
        {
            List<HallSection> items = new List<HallSection>();
            IEnumerable<Claim> claims = await APIResult.getList("HallSection/GetAll");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<HallSection>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<HallSection>> getBranchSections(int branchId)
        {
            List<HallSection> items = new List<HallSection>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", branchId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("HallSection/getBranchSections", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<HallSection>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<int> save(HallSection item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "HallSection/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }

        public async Task<int> delete(int sectionId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("sectionId", sectionId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "HallSection/Delete";
            return await APIResult.post(method, parameters);
        }

        public async Task<HallSection> GetById(int itemId)
        {
            HallSection item = new HallSection();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("HallSection/GetById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<HallSection>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
    }
}
