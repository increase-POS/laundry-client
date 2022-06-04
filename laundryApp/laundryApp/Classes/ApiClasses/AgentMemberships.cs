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
    public class AgentMemberships
    {
        public int agentMembershipsId { get; set; }
        public Nullable<int> membershipId { get; set; }
        public Nullable<int> agentId { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public byte isActive { get; set; }

        public decimal Amount { get; set; }


        public bool canDelete { get; set; }
        public Memberships memberShip { get; set; }

        //public async Task<List<AgentMemberships>> GetAll()
        //{
        //    List<AgentMemberships> items = new List<AgentMemberships>();
        //    IEnumerable<Claim> claims = await APIResult.getList("AgentMemberships/GetAll");
        //    foreach (Claim c in claims)
        //    {
        //        if (c.Type == "scopes")
        //        {
        //            items.Add(JsonConvert.DeserializeObject<AgentMemberships>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
        //        }
        //    }
        //    return items;
        //}

        //public async Task<AgentMemberships> GetById(int itemId)
        //{
        //    AgentMemberships item = new AgentMemberships();
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("itemId", itemId.ToString());
        //    //#################
        //    IEnumerable<Claim> claims = await APIResult.getList("AgentMemberships/GetById", parameters);

        //    foreach (Claim c in claims)
        //    {
        //        if (c.Type == "scopes")
        //        {
        //            item = JsonConvert.DeserializeObject<AgentMemberships>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        //            break;
        //        }
        //    }
        //    return item;
        //}
        //public async Task<int> save(AgentMemberships item)
        //{
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    string method = "AgentMemberships/Save";
        //    var myContent = JsonConvert.SerializeObject(item);
        //    parameters.Add("itemObject", myContent);
        //   return await APIResult.post(method, parameters);
        //}
        //public async Task<int> delete(int itemId, int userId, Boolean final)
        //{
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("itemId", itemId.ToString());
        //    parameters.Add("userId", userId.ToString());
        //    parameters.Add("final", final.ToString());
        //    string method = "AgentMemberships/Delete";
        //   return await APIResult.post(method, parameters);
        //}

        public async Task<int> UpdateAgentsByMembershipId(List<AgentMemberships> newList, int membershipId, int updateUserId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "AgentMemberships/UpdateAgentsByMembershipId";
            var newListParameter = JsonConvert.SerializeObject(newList);
            parameters.Add("newList", newListParameter);
            parameters.Add("membershipId", membershipId.ToString());
            parameters.Add("updateUserId", updateUserId.ToString());
            return await APIResult.post(method, parameters);
        }

        //public async Task<AgentMemberships> GetAgentMemberShip(int agentId)
        //{
        //    AgentMemberships item = new AgentMemberships();
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("agentId", agentId.ToString());
        //    //#################
        //    IEnumerable<Claim> claims = await APIResult.getList("AgentMemberships/GetAgentMemberShip", parameters);

        //    foreach (Claim c in claims)
        //    {
        //        if (c.Type == "scopes")
        //        {
        //            item = JsonConvert.DeserializeObject<AgentMemberships>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        //            break;
        //        }
        //    }
        //    return item;
        //}
    }
}
