using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;


using Newtonsoft.Json.Converters;
using System.Security.Claims;
using laundryApp.ApiClasses;

namespace laundryApp.Classes
{
    public class ItemUnitUser
    {

        public int id { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public Nullable<int> itemId { get; set; }
        public Nullable<int> unitId { get; set; }
        public Nullable<int> userId { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<byte> isActive { get; set; }
        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        /// 

        //public async Task<List<ItemUnitUser>> GetAll()
        //{

        //    List<ItemUnitUser> memberships = null;
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
        //        request.RequestUri = new Uri(Global.APIUri + "ItemUnitUser/GetAll");
        //        request.Headers.Add("APIKey", Global.APIKey);
        //        request.Method = HttpMethod.Get;
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        HttpResponseMessage response = await client.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonString = await response.Content.ReadAsStringAsync();

        //            memberships = JsonConvert.DeserializeObject<List<ItemUnitUser>>(jsonString);

        //            return memberships;
        //        }
        //        else //web api sent error response 
        //        {
        //            memberships = new List<ItemUnitUser>();
        //        }
        //        return memberships;
        //    }

        //}

        public async Task<List<ItemUnitUser>> GetByUserId(int userId)
        {
            List<ItemUnitUser> list = new List<ItemUnitUser>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemUnitUser/GetByUserId",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnitUser>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


            //List<ItemUnitUser> memberships = null;
            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //    client.BaseAddress = new Uri(Global.APIUri);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    request.RequestUri = new Uri(Global.APIUri + "ItemUnitUser/GetByUserId?userId=" + userId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();

            //        memberships = JsonConvert.DeserializeObject<List<ItemUnitUser>>(jsonString);

            //        return memberships;
            //    }
            //    else //web api sent error response 
            //    {
            //        memberships = new List<ItemUnitUser>();
            //    }
            //    return memberships;
            //}

        }

        //public async Task<string> Save(ItemUnitUser obj)
        //{
        //    // ... Use HttpClient.
        //    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        //    // 
        //    var myContent = JsonConvert.SerializeObject(obj);

        //    using (var client = new HttpClient())
        //    {
        //        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
        //        client.BaseAddress = new Uri(Global.APIUri);
        //        client.DefaultRequestHeaders.Clear();
        //        client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
        //        client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
        //        HttpRequestMessage request = new HttpRequestMessage();
        //        // encoding parameter to get special characters
        //        myContent = HttpUtility.UrlEncode(myContent);
        //        request.RequestUri = new Uri(Global.APIUri + "ItemUnitUser/Save?Object=" + myContent);
        //        request.Headers.Add("APIKey", Global.APIKey);
        //        request.Method = HttpMethod.Post;
        //        //set content type
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var response = await client.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var message = await response.Content.ReadAsStringAsync();
        //            message = JsonConvert.DeserializeObject<string>(message);
        //            return message;
        //        }
        //        return "";
        //    }
        //}

        //public async Task<ItemUnitUser> GetByID(int id)
        //{
        //    ItemUnitUser obj = new ItemUnitUser();

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
        //        request.RequestUri = new Uri(Global.APIUri + "ItemUnitUser/GetByID?id=" + id);
        //        request.Headers.Add("APIKey", Global.APIKey);
              
        //        request.Method = HttpMethod.Get;
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var response = await client.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var jsonString = await response.Content.ReadAsStringAsync();

        //            obj = JsonConvert.DeserializeObject<ItemUnitUser>(jsonString);

        //            return obj;
        //        }

        //        return obj;
        //    }
        //}

        //public async Task<string> Delete(int id)
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
        //        request.RequestUri = new Uri(Global.APIUri + "ItemUnitUser/Delete?id=" + id);
        //        request.Headers.Add("APIKey", Global.APIKey);

        //        request.Method = HttpMethod.Post;
        //        //set content type
        //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //        var response = await client.SendAsync(request);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var message = await response.Content.ReadAsStringAsync();
        //            message = JsonConvert.DeserializeObject<string>(message);
        //            return message;
        //        }
        //        return "";
        //    }
        //}


        public async Task<int> UpdateList(List<ItemUnitUser> newlist, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemUnitUser/UpdateList";

            var myContent = JsonConvert.SerializeObject(newlist);
            parameters.Add("Object", myContent);
            parameters.Add("userId", userId.ToString());

           return await APIResult.post(method, parameters);



            //string message = "";
            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //// 
            //var myContent = JsonConvert.SerializeObject(newlist);

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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemUnitUser/UpdateList?newlist=" + myContent);
            //    request.Headers.Add("APIKey", Global.APIKey);

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




    }
}
