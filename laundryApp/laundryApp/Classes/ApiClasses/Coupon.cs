using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp;
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
    public class Coupon
    {
        public int cId { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public byte isActive { get; set; }
        public byte discountType { get; set; }
        public decimal discountValue { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public string notes { get; set; }
        public int quantity { get; set; }
        public int remainQ { get; set; }
        public decimal invMin { get; set; }
        public decimal invMax { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public string barcode { get; set; }
        public Nullable<int> membershipId { get; set; }
        public Boolean canDelete { get; set; }
        public string state { get; set; }
        public string forAgents { get; set; }
        public int couponMembershipId { get; set; }
     

        public async Task<List<Coupon>> Get()
        {
            List<Coupon> items = new List<Coupon>();
            IEnumerable<Claim> claims = await APIResult.getList("coupons/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Coupon>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }


        public async Task<List<Coupon>> GetEffictiveCoupons()
        {
            List<Coupon> items = new List<Coupon>();
            IEnumerable<Claim> claims = await APIResult.getList("coupons/GetEffictive");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Coupon>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }


        public async Task<List<Coupon>> GetEffictiveByMemberShipID(int memberShipId)
        {
            List<Coupon> items = new List<Coupon>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("memberShipId", memberShipId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("coupons/GetEffictiveByMemberShipID",parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Coupon>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }


        public async Task<Coupon> getById(int itemId)
        {
            Coupon item = new Coupon();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("coupons/GetCouponByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Coupon>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }


         // get is exist
        public async Task<Coupon> Existcode(string code)
        {
            Coupon item = new Coupon();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", code.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("coupons/IsExistcode", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Coupon>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
      
        public async Task<int> save(Coupon item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "coupons/Save";
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
            string method = "coupons/Delete";
           return await APIResult.post(method, parameters);
        }
       
        public async Task<List<Coupon>> GetCouponsByMembershipId(int membershipId)
        {
            List<Coupon> items = new List<Coupon>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", membershipId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("coupons/GetCouponsByMembershipId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Coupon>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));

                }
            }
            return items;
        }

    }
}

