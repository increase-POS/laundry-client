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


        public async Task<Coupon> getCouponByBarCode(string barcode)
        {
            Coupon item = new Coupon();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", barcode.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("coupons/GetCouponByBarcode", parameters);

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
        //public async Task<Boolean> deleteCoupon(int couponId, int userId, bool final)
        //{
        //    // ... Use HttpClient.
        //    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

        //    using (var client = new HttpClient())
        //    {
        //        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //        client.BaseAddress = new Uri(Global.APIUri);
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
        //        client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
        //        HttpRequestMessage request = new HttpRequestMessage();
        //        request.RequestUri = new Uri(Global.APIUri + "coupons/Delete?cId=" + couponId + "&userId=" + userId + "&final=" + final);

        //        request.Headers.Add("APIKey", Global.APIKey);

        //        request.Method = HttpMethod.Post;

        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var response = await client.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            return true;
        //        }
        //        return false;
        //    }
        //}

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

