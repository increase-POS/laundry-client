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
    public class InvoicesClassMemberships
    {
        public int invClassMemberId { get; set; }
        public Nullable<int> membershipId { get; set; }
        public Nullable<int> invClassId { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }


        public async Task<List<InvoicesClassMemberships>> GetAll()
        {
            List<InvoicesClassMemberships> items = new List<InvoicesClassMemberships>();
            IEnumerable<Claim> claims = await APIResult.getList("invoicesClassMemberships/GetAll");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<InvoicesClassMemberships>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<InvoicesClassMemberships> GetById(int itemId)
        {
            InvoicesClassMemberships item = new InvoicesClassMemberships();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("invoicesClassMemberships/GetById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<InvoicesClassMemberships>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
       
      
        public async Task<int> UpdateInvclassByMembershipId(List<InvoicesClassMemberships> newList, int membershipId, int updateUserId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "invoicesClassMemberships/UpdateInvclassByMembershipId";
            var newListParameter = JsonConvert.SerializeObject(newList);
            parameters.Add("newList", newListParameter);
            parameters.Add("membershipId", membershipId.ToString());
            parameters.Add("updateUserId", updateUserId.ToString());
            return await APIResult.post(method, parameters);
        }
    }
}
