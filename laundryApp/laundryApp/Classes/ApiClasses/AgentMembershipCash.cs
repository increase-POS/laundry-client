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
    public class AgenttoPayCash
    {

        public Nullable<int> agentMembershipCashId { get; set; }
        public Nullable<int> subscriptionFeesId { get; set; }
        public Nullable<int> cashTransId { get; set; }
        public Nullable<int> membershipId { get; set; }
        public Nullable<int> agentId { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        //public Nullable<int> updateUserId { get; set; }
        //public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        //public Nullable<int> createUserId { get; set; }
        public decimal Amount { get; set; }
        public Nullable<int> pointId { get; set; }
        public string agentName { get; set; }
        public string agentcode { get; set; }
        public string agentcompany { get; set; }
        public string agentaddress { get; set; }
        public string agentemail { get; set; }
        public string agentphone { get; set; }
        public string agentmobile { get; set; }

        public string agenttype { get; set; }
        public string agentaccType { get; set; }
        public decimal agentbalance { get; set; }
        public byte agentbalanceType { get; set; }

        public string agentfax { get; set; }
        public decimal agentmaxDeserve { get; set; }
        public bool agentisLimited { get; set; }
        public string agentpayType { get; set; }
        public bool agentcanReserve { get; set; }
        public string agentdisallowReason { get; set; }
        public Nullable<int> agentresidentSecId { get; set; }
        public string agentGPSAddress { get; set; }

        public string membershipName { get; set; }

        public byte membershipisActive { get; set; }
        public string subscriptionType { get; set; }
        public string cashsubscriptionType { get; set; }
        public string membershipcode { get; set; }
        public bool isFreeDelivery { get; set; }
        public decimal deliveryDiscountPercent { get; set; }
        public Nullable<decimal> subscriptionFee { get; set; }
        public Nullable<int> monthsCount { get; set; }
        public string transType { get; set; }
        public string transNum { get; set; }
        public decimal discountValue { get; set; }
        public decimal total { get; set; }

        public string membershipStatus { get; set; }
        public int couponsCount { get; set; }
        public int invoicesClassesCount { get; set; }
        public int offersCount { get; set; }

    }
    public class AgentMembershipCash
    {
        public int agentMembershipsId { get; set; }
        public Nullable<int> subscriptionFeesId { get; set; }
        public Nullable<int> cashTransId { get; set; }
        public Nullable<int> membershipId { get; set; }
        public Nullable<int> agentId { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public byte isActive { get; set; }
        public decimal Amount { get; set; }
        public bool canDelete { get; set; }
        public Nullable<int> monthsCount { get; set; }
        public string agentName { get; set; }
        public string agentcode { get; set; }
        public string agentcompany { get; set; }
        public string agenttype { get; set; }
        public string membershipName { get; set; }
        public string membershipcode { get; set; }
        public string transType { get; set; }
        public string transNum { get; set; }
        public Nullable<System.DateTime> payDate { get; set; }
        public byte membershipisActive { get; set; }
        public int agentMembershipCashId { get; set; }
        public string subscriptionType { get; set; }
        public decimal discountValue { get; set; }
        public decimal total { get; set; }
        public string processType { get; set; }
        public Nullable<int> cardId { get; set; }
        public string cardName { get; set; }
        public string docNum { get; set; }
        public string subscriptionTypeconv { get; set; }
        public string EndDateconv { get; set; }

        public async Task<List<AgentMembershipCash>> GetAll()
        {
            List<AgentMembershipCash> items = new List<AgentMembershipCash>();
            IEnumerable<Claim> claims = await APIResult.getList("AgentMembershipCash/GetAll");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<AgentMembershipCash>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        //public async Task<AgentMembershipCash> GetById(int itemId)
        //{
        //    AgentMembershipCash item = new AgentMembershipCash();
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("itemId", itemId.ToString());
        //    //#################
        //    IEnumerable<Claim> claims = await APIResult.getList("AgentMembershipCash/GetById", parameters);

        //    foreach (Claim c in claims)
        //    {
        //        if (c.Type == "scopes")
        //        {
        //            item = JsonConvert.DeserializeObject<AgentMembershipCash>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        //            break;
        //        }
        //    }
        //    return item;
        //}
        //public async Task<int> save(AgentMembershipCash item)
        //{
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    string method = "AgentMembershipCash/Save";
        //    var myContent = JsonConvert.SerializeObject(item);
        //    parameters.Add("itemObject", myContent);
        //   return await APIResult.post(method, parameters);
        //}
        public async Task<int> Savepay(AgentMembershipCash item,CashTransfer cashtransferobject)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "AgentMembershipCash/Savepay";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
            var myContent2 = JsonConvert.SerializeObject(cashtransferobject);
            parameters.Add("cashtransferobject", myContent2);
            //item.agentMembershipCashId
            return await APIResult.post(method, parameters);
            //return  item.agentMembershipCashId
        }
        //public async Task<int> delete(int itemId, int userId, Boolean final)
        //{
        //    Dictionary<string, string> parameters = new Dictionary<string, string>();
        //    parameters.Add("itemId", itemId.ToString());
        //    parameters.Add("userId", userId.ToString());
        //    parameters.Add("final", final.ToString());
        //    string method = "AgentMembershipCash/Delete";
        //   return await APIResult.post(method, parameters);
        //}

        public async Task<List<AgenttoPayCash>> GetAgentToPay()
        {
            List<AgenttoPayCash> items = new List<AgenttoPayCash>();
            IEnumerable<Claim> claims = await APIResult.getList("AgentMembershipCash/GetAgentToPay");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<AgenttoPayCash>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
    }
}
