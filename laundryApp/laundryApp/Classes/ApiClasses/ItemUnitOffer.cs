using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Security.Claims;
using laundryApp.ApiClasses;

namespace laundryApp.Classes
{
    public class ItemUnitOffer
    {
        public int ioId { get; set; }
        public Nullable<int> iuId { get; set; }
        public Nullable<int> offerId { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> quantity { get; set; }
        public string offerName { get; set; }
        public string unitName { get; set; }
        public string itemName { get; set; }
        public string code { get; set; }
        public Nullable<int> itemId { get; set; }
        public Nullable<int> unitId { get; set; }

        public async Task<int> updategroup(int offerId, List<ItemUnitOffer> newitoflist, int userId)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsOffers/UpdateItemsByOfferId";

            var myContent = JsonConvert.SerializeObject(newitoflist);
            parameters.Add("Object", myContent);
            parameters.Add("offerId", offerId.ToString());
            parameters.Add("userId", userId.ToString());



           return await APIResult.post(method, parameters);


            //string message = "";
            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //// 
            //var myContent = JsonConvert.SerializeObject(newitoflist);

            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //    client.BaseAddress = new Uri(Global.APIUri);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    // encoding parameter to get special characters
            //    myContent = HttpUtility.UrlEncode(myContent);
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsOffers/UpdateItemsByOfferId?newitoflist=" + myContent);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Headers.Add("offerId", offerId.ToString());
            //    request.Headers.Add("userId", userId.ToString());
            //    request.Method = HttpMethod.Post;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        message = await response.Content.ReadAsStringAsync();
            //        message = JsonConvert.DeserializeObject<string>(message);
            //    }
            //    return message;
            //}
        }
        public async Task<int> getRemain(int offerId, int itemUnitId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsOffers/getRemain";

            parameters.Add("offerId", offerId.ToString());
            parameters.Add("itemUnitId", itemUnitId.ToString());
            return await APIResult.post(method, parameters);  
        }







        //public async Task<List<ItemUnitOffer>> Getall()
        //{
        //    List <ItemUnitOffer> itemUnitOfferlist = null;

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
        //        request.RequestUri = new Uri(Global.APIUri + "ItemsOffers/Getall");
        //        request.Headers.Add("APIKey", Global.APIKey);
        //        request.Method = HttpMethod.Get;
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var response = await client.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonString = await response.Content.ReadAsStringAsync();

        //            itemUnitOfferlist = JsonConvert.DeserializeObject <List<ItemUnitOffer>>(jsonString);

        //            return itemUnitOfferlist;
        //        }

        //        return itemUnitOfferlist;
        //    }
        //}


        public async Task<List<ItemUnitOffer>> GetItemsByOfferId(int offerId)
        {
            List<ItemUnitOffer> list = new List<ItemUnitOffer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("offerId", offerId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsOffers/GetItemsByOfferId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnitOffer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
            //        List<ItemUnitOffer> itemUnitOfferlist = null;

            //        // ... Use HttpClient.
            //        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //        using (var client = new HttpClient())
            //        {
            //            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //            client.BaseAddress = new Uri(Global.APIUri);
            //            client.DefaultRequestHeaders.Clear();
            //            client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //            client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //            HttpRequestMessage request = new HttpRequestMessage();
            //            request.RequestUri = new Uri(Global.APIUri + "ItemsOffers/GetItemsByOfferId");
            //            request.Headers.Add("APIKey", Global.APIKey);
            //            request.Headers.Add("offerId", offerId.ToString());
            //            request.Method = HttpMethod.Get;
            //            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //            var response = await client.SendAsync(request);

            //            if (response.IsSuccessStatusCode)
            //            {
            //                var jsonString = await response.Content.ReadAsStringAsync();

            //                itemUnitOfferlist = JsonConvert.DeserializeObject<List<ItemUnitOffer>>(jsonString);

            //                return itemUnitOfferlist;
            //            }

            //            return itemUnitOfferlist;
            //        }
            //    }
            //    // get items in category and sub

        }
    }
}

