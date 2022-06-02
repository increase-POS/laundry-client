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
    public class MembershipsOffers
    {
        public int membershipOfferId { get; set; }
        public Nullable<int> membershipId { get; set; }
        public Nullable<int> offerId { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }


        public async Task<List<MembershipsOffers>> GetAll()
        {
            List<MembershipsOffers> items = new List<MembershipsOffers>();
            IEnumerable<Claim> claims = await APIResult.getList("membershipsOffers/GetAll");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<MembershipsOffers>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<MembershipsOffers> GetById(int itemId)
        {
            MembershipsOffers item = new MembershipsOffers();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("membershipsOffers/GetById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<MembershipsOffers>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<int> save(MembershipsOffers item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "membershipsOffers/Save";
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
            string method = "membershipsOffers/Delete";
           return await APIResult.post(method, parameters);
        }

        public async Task<int> UpdateOffersByMembershipId(List<MembershipsOffers> newList, int membershipId, int updateUserId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "membershipsOffers/UpdateOffersByMembershipId";
            var newListParameter = JsonConvert.SerializeObject(newList);
            parameters.Add("newList", newListParameter);
            parameters.Add("membershipId", membershipId.ToString());
            parameters.Add("updateUserId", updateUserId.ToString());
            return await APIResult.post(method, parameters);
        }
    }
}
