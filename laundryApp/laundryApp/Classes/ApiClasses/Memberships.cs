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
    public class Memberships
    {
        public int membershipId { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string subscriptionType { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public byte isActive { get; set; }
        public Nullable<decimal> subscriptionFee { get; set; }
        public bool canDelete { get; set; }
        public bool isFreeDelivery { get; set; }
        public decimal deliveryDiscountPercent { get; set; }

        public int customersCount { get; set; }
        public int couponsCount { get; set; }
        public int offersCount { get; set; }
        public int invoicesClassesCount { get; set; }


        public async Task<List<Memberships>> GetAll()
        {
            List<Memberships> items = new List<Memberships>();
            IEnumerable<Claim> claims = await APIResult.getList("memberships/GetAll");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Memberships>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<Memberships> GetById(int itemId)
        {
            Memberships item = new Memberships();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("memberships/GetById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Memberships>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<int> save(Memberships item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "memberships/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<int> delete(int itemId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "memberships/Delete";
           return await APIResult.post(method, parameters);
        }
        public async Task<int> SaveMemberAndSub(Memberships item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "memberships/SaveMemberAndSub";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }

        public async Task<AgenttoPayCash> GetmembershipByAgentId(int itemId)
        {
            AgenttoPayCash item = new AgenttoPayCash();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("memberships/GetmembershipByAgentId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<AgenttoPayCash>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }

        public async Task<AgenttoPayCash> GetmembershipStateByAgentId(int itemId)
        {
            AgenttoPayCash item = new AgenttoPayCash();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("memberships/GetmembershipStateByAgentId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<AgenttoPayCash>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }

    }
}
