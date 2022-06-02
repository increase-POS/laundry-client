using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp.ApiClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    public class Tag
    {
        public int tagId { get; set; }
        public string tagName { get; set; }
        public Nullable<int> categoryId { get; set; }
        public string notes { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public byte isActive { get; set; }


        public bool canDelete { get; set; }
         

        public async Task<List<Tag>> Get(int categoryId =0)
        {
            List<Tag> items = new List<Tag>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("categoryId", categoryId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("Tags/Get", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Tag>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<Tag> GetByID(int itemId)
        {
            Tag item = new Tag();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
           
            IEnumerable<Claim> claims = await APIResult.getList("Tags/GetByID", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Tag>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<Tag> GetByisActive(byte isActive)
        {
            Tag item = new Tag();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("isActive", isActive.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Tags/GetByisActive", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Tag>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<int> Save(Tag item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Tags/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<int> Delete(int TagId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", TagId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "Tags/Delete";
           return await APIResult.post(method, parameters);
        }

    }
}

