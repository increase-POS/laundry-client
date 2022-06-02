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
    public class ItemLocation
    {

        public int itemsLocId { get; set; }
        public Nullable<int> locationId { get; set; }
        public long quantity { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public string notes { get; set; }
        public Nullable<int> invoiceId { get; set; }

        public int sequence { get; set; }

        public Nullable<int> sectionId { get; set; }


        public string itemName { get; set; }
        public string location { get; set; }
        public string section { get; set; }
        public string unitName { get; set; }
        public string itemType { get; set; }
        public Nullable<decimal> storeCost { get; set; }
        public Nullable<byte> isFreeZone { get; set; }

        public string invNumber { get; set; }

       
        public Nullable<bool> isSelected { get; set; }

        //****************************************************
        public async Task<List<ItemLocation>> get(int branchId)
        {
            List<ItemLocation> list = new List<ItemLocation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());
           
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsLocations/Get",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemLocation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemLocation> items = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/Get?branchId=" + branchId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace("\\", string.Empty);
            //        jsonString = jsonString.Trim('"');
            //        // fix date format
            //        JsonSerializerSettings settings = new JsonSerializerSettings
            //        {
            //            Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //            DateParseHandling = DateParseHandling.None
            //        };
            //        items = JsonConvert.DeserializeObject<List<ItemLocation>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return items;
            //    }
            //    else //web api sent error response 
            //    {
            //        items = new List<ItemLocation>();
            //    }
            //    return items;
            //}
        }
        public async Task<List<ItemLocation>> getAll(int branchId)
        {

            List<ItemLocation> list = new List<ItemLocation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsLocations/getAll",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemLocation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemLocation> items = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/getAll?branchId=" + branchId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace("\\", string.Empty);
            //        jsonString = jsonString.Trim('"');
            //        // fix date format
            //        JsonSerializerSettings settings = new JsonSerializerSettings
            //        {
            //            Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //            DateParseHandling = DateParseHandling.None
            //        };
            //        items = JsonConvert.DeserializeObject<List<ItemLocation>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return items;
            //    }
            //    else //web api sent error response 
            //    {
            //        items = new List<ItemLocation>();
            //    }
            //    return items;
            //}
        }
     //public async Task<List<ItemLocation>> getWithSequence(int branchId)
     //   {
     //       List<ItemLocation> items = null;
     //       // ... Use HttpClient.
     //       ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
     //       using (var client = new HttpClient())
     //       {
     //           ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
     //           client.BaseAddress = new Uri(Global.APIUri);
     //           client.DefaultRequestHeaders.Clear();
     //           client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
     //           client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
     //           HttpRequestMessage request = new HttpRequestMessage();
     //           request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/getWithSequence?branchId=" + branchId);
     //           request.Headers.Add("APIKey", Global.APIKey);
     //           request.Method = HttpMethod.Get;
     //           client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
     //           HttpResponseMessage response = await client.SendAsync(request);

     //           if (response.IsSuccessStatusCode)
     //           {
     //               var jsonString = await response.Content.ReadAsStringAsync();
     //               jsonString = jsonString.Replace("\\", string.Empty);
     //               jsonString = jsonString.Trim('"');
     //               // fix date format
     //               JsonSerializerSettings settings = new JsonSerializerSettings
     //               {
     //                   Converters = new List<JsonConverter> { new BadDateFixingConverter() },
     //                   DateParseHandling = DateParseHandling.None
     //               };
     //               items = JsonConvert.DeserializeObject<List<ItemLocation>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
     //               return items;
     //           }
     //           else //web api sent error response 
     //           {
     //               items = new List<ItemLocation>();
     //           }
     //           return items;
     //       }
     //   }
    
        public async Task<List<ItemLocation>> GetFreeZoneItems(int branchId)
        {


            List<ItemLocation> list = new List<ItemLocation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsLocations/GetFreeZoneItems",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemLocation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
            //List<ItemLocation> items = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/GetFreeZoneItems?branchId=" + branchId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace("\\", string.Empty);
            //        jsonString = jsonString.Trim('"');
            //        // fix date format
            //        JsonSerializerSettings settings = new JsonSerializerSettings
            //        {
            //            Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //            DateParseHandling = DateParseHandling.None
            //        };
            //        items = JsonConvert.DeserializeObject<List<ItemLocation>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return items;
            //    }
            //    else //web api sent error response 
            //    {
            //        items = new List<ItemLocation>();
            //    }
            //    return items;
            //}
        }
        public async Task<List<ItemLocation>> GetLockedItems(int branchId)
        {
            List<ItemLocation> list = new List<ItemLocation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsLocations/GetLockedItems",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemLocation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemLocation> items = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/GetLockedItems?branchId=" + branchId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace("\\", string.Empty);
            //        jsonString = jsonString.Trim('"');
            //        // fix date format
            //        JsonSerializerSettings settings = new JsonSerializerSettings
            //        {
            //            Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //            DateParseHandling = DateParseHandling.None
            //        };
            //        items = JsonConvert.DeserializeObject<List<ItemLocation>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return items;
            //    }
            //    else //web api sent error response 
            //    {
            //        items = new List<ItemLocation>();
            //    }
            //    return items;
            //}
        }
        public async Task<int> decreaseAmounts(List<ItemTransfer> invoiceItems, int branchId, int userId, string objectName, Notification not)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/decraseAmounts";

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("objectName", objectName);
            var myContent1 = JsonConvert.SerializeObject(not);
            parameters.Add("notificationObj", myContent1);

           return await APIResult.post(method, parameters);
        }
        public async Task<int> decreaseAmountsInKitchen(List<ItemTransfer> invoiceItems, int branchId, int userId)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/decreaseAmountsInKitchen";

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("userId", userId.ToString());

           return await APIResult.post(method, parameters);
        }
        public async Task<int> unlockItem(ItemLocation il, int branchId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/unlockItem";

            var myContent = JsonConvert.SerializeObject(il);
            parameters.Add("Object", myContent);
            parameters.Add("branchId", branchId.ToString());

            return await APIResult.post(method, parameters);
        }
        public async Task<int> unitsConversion(int branchId,int fromItemUnit , int toItemUnt, int fromQuantity,int toQuantity, int userId, ItemUnit smallUnit)
        {


            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/unitsConversion";

        
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("fromItemUnit", fromItemUnit.ToString());
            parameters.Add("toItemUnt", toItemUnt.ToString());
            parameters.Add("fromQuantity", fromQuantity.ToString());
            parameters.Add("toQuantity", toQuantity.ToString());



            var myContent = JsonConvert.SerializeObject(smallUnit);
            parameters.Add("Object", myContent);

           return await APIResult.post(method, parameters);



            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //if (smallUnit == null)
            //    smallUnit = new ItemUnit();
            //var myContent = JsonConvert.SerializeObject(smallUnit);
            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //    client.BaseAddress = new Uri(Global.APIUri);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/unitsConversion?branchId=" + branchId+ "&fromItemUnit="+ fromItemUnit + "&toItemUnit="+toItemUnt+
            //                                                    "&fromQuantity="+fromQuantity + "&toQuantity="+ toQuantity + "&userId=" +userId + "&smallUnit=" + myContent);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Post;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
        }
        public async Task<int> decreaseItemLocationQuantity(int itemLocId ,int quantity, int userId, string objectName, Notification notification)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/decreaseItemLocationQuantity";


            parameters.Add("itemLocId", itemLocId.ToString());
            parameters.Add("quantity", quantity.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("objectName", objectName);
        



            var myContent = JsonConvert.SerializeObject(notification);
            parameters.Add("Object", myContent);

           return await APIResult.post(method, parameters);



            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //var myContent = JsonConvert.SerializeObject(notification);
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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/decreaseItemLocationQuantity?itemLocId=" + itemLocId+ "&quantity="+ quantity +
            //                                        "&userId=" + userId +"&objectName=" + objectName +"&notificationObj=" + myContent);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Post;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
        }
        public async Task<int> trasnferItem( int itemLocId ,ItemLocation itemLocation)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/trasnferItem";


            parameters.Add("itemLocId", itemLocId.ToString());
   

            var myContent = JsonConvert.SerializeObject(itemLocation);
            parameters.Add("Object", myContent);

           return await APIResult.post(method, parameters);


            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //var myContent = JsonConvert.SerializeObject(itemLocation);
            //// 
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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/trasnferItem?itemLocId=" + itemLocId + "&itemLocation=" + myContent);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Post;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
        }
        public async Task<int> reserveItems(List<ItemTransfer> invoiceItems,int invoiceId, int branchId, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/reserveItems";


            parameters.Add("invoiceId", invoiceId.ToString());

            parameters.Add("branchId", branchId.ToString());
            parameters.Add("userId", userId.ToString());
           

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);

           return await APIResult.post(method, parameters);


            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //// 
            //var myContent = JsonConvert.SerializeObject(invoiceItems);
            ////var myContent1 = JsonConvert.SerializeObject(not);

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
            //    //myContent1 = HttpUtility.UrlEncode(myContent1);
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/reserveItems?itemLocationObject=" + myContent+"&invoiceId="+invoiceId + "&branchId=" + branchId +
            //                            "&userId=" + userId );
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Post;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
        }
        public async Task<int> reReserveItems(List<ItemTransfer> invoiceItems, int invoiceId, int branchId, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/reReserveItems";

            parameters.Add("invoiceId", invoiceId.ToString());
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("userId", userId.ToString());

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);

            return await APIResult.post(method, parameters);
        }
        public async Task<int> getAmountInBranch(int itemUnitId, int branchId,int isKitchen = 0)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/getAmountInBranch";

            parameters.Add("branchId", branchId.ToString());
            parameters.Add("itemUnitId", itemUnitId.ToString());
            parameters.Add("isKitchen", isKitchen.ToString());
           return await APIResult.post(method, parameters);
        }
        public async Task<int> getAmountInLocation(int itemUnitId, int branchId , int locationId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/getAmountInLocation";


            parameters.Add("locationId", locationId.ToString());

            parameters.Add("branchId", branchId.ToString());
            parameters.Add("itemUnitId", itemUnitId.ToString());



           return await APIResult.post(method, parameters);


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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/getAmountInLocation?itemUnitId=" + itemUnitId + "&branchId=" + branchId+ "&locationId="+ locationId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var AvailableAmount = await response.Content.ReadAsStringAsync();
            //        return int.Parse(AvailableAmount);
            //    }
            //    return 0;
            //}
        }
        public async Task<int> getUnitAmount(int itemUnitId, int branchId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/getUnitAmount";


            parameters.Add("itemUnitId", itemUnitId.ToString());

            parameters.Add("branchId", branchId.ToString());
   



           return await APIResult.post(method, parameters);


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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/getUnitAmount?itemUnitId=" + itemUnitId + "&branchId=" + branchId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var AvailableAmount = await response.Content.ReadAsStringAsync();
            //        return int.Parse(AvailableAmount);
            //    }
            //    return 0;
            //}
        }
        public async Task<int> getAmountByItemLocId(int itemLocId)
        {

            int item = 0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemLocId", itemLocId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsLocations/getAmountByItemLocId",parameters);


            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = int.Parse(c.Value);
                    break;
                }
            }
            return item;


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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/getAmountByItemLocId?itemLocId=" + itemLocId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var AvailableAmount = await response.Content.ReadAsStringAsync();
            //        return int.Parse(AvailableAmount);
            //    }
            //    return 0;
            //}
        }
        public async Task recieptInvoice(List<ItemTransfer> invoiceItems, int branchId,int userId, string objectName, Notification notificationObj)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/receiptInvoice";

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);

            var myContent1 = JsonConvert.SerializeObject(notificationObj);
            parameters.Add("notificationObj", myContent1);

            parameters.Add("userId", userId.ToString());

            parameters.Add("branchId", branchId.ToString());
            parameters.Add("objectName", objectName);

           await APIResult.post(method, parameters);


            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //// 
            //var myContent = JsonConvert.SerializeObject(invoiceItems);
            //var myContent1 = JsonConvert.SerializeObject(notificationObj);

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
            //    myContent1 = HttpUtility.UrlEncode(myContent1);
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/receiptInvoice?itemLocationObject=" + myContent + "&branchId=" + branchId 
            //                                            + "&userId=" + userId
            //                                            +"&objectName=" + objectName + "&notificationObj="+myContent1);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Post;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
        }
        public async Task<int> generatePackage(int packageParentId, int quantity,int locationId,int branchId,int userId)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/generatePackage";

    

            parameters.Add("packageParentId", packageParentId.ToString());

            parameters.Add("quantity", quantity.ToString());
            parameters.Add("locationId", locationId.ToString());
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("userId", userId.ToString());

           return await APIResult.post(method, parameters);


            //  // ... Use HttpClient.
            //  ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //  // 
            ////  var myContent = JsonConvert.SerializeObject(invoiceItems);

            //  using (var client = new HttpClient())
            //  {
            //      ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //      client.BaseAddress = new Uri(Global.APIUri);
            //      client.DefaultRequestHeaders.Clear();
            //      client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //      client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //      HttpRequestMessage request = new HttpRequestMessage();
            //      // encoding parameter to get special characters
            //      //myContent = HttpUtility.UrlEncode(myContent);
            //      request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/generatePackage?packageParentId=" + packageParentId + "&quantity=" + quantity+
            //                                          "&locationId="+ locationId +"&branchId="+branchId+ "&userId=" + userId);
            //      request.Headers.Add("APIKey", Global.APIKey);
            //      request.Method = HttpMethod.Post;
            //      //set content type
            //      client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //      var response = await client.SendAsync(request);

            //      if (response.IsSuccessStatusCode)
            //      {
            //          return true;
            //      }
            //      return false;
            //  }
        }
        public async Task<int> recieptOrder(List<ItemLocation> invoiceItems,List<ItemTransfer> orderList,int toBranch,int userId, string objectName, Notification notificationObj)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/receiptOrder";

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);

            var myContent2 = JsonConvert.SerializeObject(orderList);
            parameters.Add("orderList", myContent2);

            var myContent1 = JsonConvert.SerializeObject(notificationObj);
            parameters.Add("notificationObj", myContent1);

            parameters.Add("userId", userId.ToString());

            parameters.Add("toBranch", toBranch.ToString());
            parameters.Add("objectName", objectName);

           return await APIResult.post(method, parameters);
        }
        public async Task<int> transferToKitchen(List<ItemLocation> invoiceItems,List<ItemTransfer> orderList,int branchId,int userId)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/transferToKitchen";

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);

            var myContent2 = JsonConvert.SerializeObject(orderList);
            parameters.Add("orderList", myContent2);

            parameters.Add("userId", userId.ToString());

            parameters.Add("branchId", branchId.ToString());

           return await APIResult.post(method, parameters);
        }
        public async Task<int> destroyItem(List<ItemLocation> invoiceItems, int userId)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/destroyItem";

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);

      
            parameters.Add("userId", userId.ToString());


           return await APIResult.post(method, parameters);


            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //// 
            //var myContent = JsonConvert.SerializeObject(invoiceItems);

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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/destroyItem?itemLocationObject=" + myContent+ "&userId=" + userId );
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Post;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
        }
        public async Task<int> returnInvoice(List<ItemTransfer> invoiceItems, int branchId, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/returnInvoice";

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);

            parameters.Add("userId", userId.ToString());

            parameters.Add("branchId", branchId.ToString());
          
           return await APIResult.post(method, parameters);
        }
        public async Task<int> returnSpendingOrder(List<ItemTransfer> invoiceItems, int branchId, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/returnSpendingOrder";

            var myContent = JsonConvert.SerializeObject(invoiceItems);
            parameters.Add("Object", myContent);

            parameters.Add("userId", userId.ToString());

            parameters.Add("branchId", branchId.ToString());
          
           return await APIResult.post(method, parameters);
        }
        public async Task<int> transferAmountbetweenUnits(int locationId,int itemLocId,int toItemUnitId,int fromQuantity, int toQuantity, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsLocations/transferAmountbetweenUnits";        

            parameters.Add("locationId", locationId.ToString());
            parameters.Add("itemLocId", itemLocId.ToString());
            parameters.Add("toItemUnitId", toItemUnitId.ToString());
            parameters.Add("fromQuantity", fromQuantity.ToString());
            parameters.Add("toQuantity", toQuantity.ToString());
            parameters.Add("userId", userId.ToString());


           return await APIResult.post(method, parameters);
        }
        public async Task<List<ItemLocation>> getSpecificItemLocation(string itemUnitsIds, int branchId)
        {

            List<ItemLocation> list = new List<ItemLocation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemUnitsIds", itemUnitsIds.ToString());
            parameters.Add("branchId", branchId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsLocations/getSpecificItemLocation", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemLocation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
         


            //List<ItemLocation> items = null;
            //var myContent = JsonConvert.SerializeObject(itemUnitsIds);
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
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsLocations/getSpecificItemLocation?itemUnitsIds=" + myContent+"&branchId= " + branchId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace("\\", string.Empty);
            //        jsonString = jsonString.Trim('"');
            //        // fix date format
            //        JsonSerializerSettings settings = new JsonSerializerSettings
            //        {
            //            Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //            DateParseHandling = DateParseHandling.None
            //        };
            //        items = JsonConvert.DeserializeObject<List<ItemLocation>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return items;
            //    }
            //    else //web api sent error response 
            //    {
            //        items = new List<ItemLocation>();
            //    }
            //    return items;
            //}
        }
    }
}
