using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp;
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
    public class Object
    {

        public int objectId { get; set; }
        public string name { get; set; }
        public Nullable<int> parentObjectId { get; set; }
        public string objectType { get; set; }
        public string translate { get; set; }
        public string translateHint { get; set; }
        public string icon { get; set; }


        public string note { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public byte isActive { get; set; }
        public Boolean canDelete { get; set; }

        List<Object> plist = new List<Object>();
        List<Object> clist = new List<Object>();

        public async Task<List<Object>> GetAll()
        {


            List<Object> list = new List<Object>();
            //  Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mainBranchId", mainBranchId.ToString());
            //parameters.Add("userId", userId.ToString());
            //parameters.Add("date", date.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Object/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<Object>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


            //List<Object> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Object/Get");
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
            //        list = JsonConvert.DeserializeObject<List<Object>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<Object>();
            //    }
            //    return list;
            //}
        }


        public List<Object> GetParents(List<Object> all, string objName)
        {
            plist = new List<Object>();
            List<Object> list = new List<Object>();
            Object tempob = null;
            tempob = all.Where(o => o.name == objName).FirstOrDefault();
            if (tempob != null)
            {
                int? firstpid = tempob.parentObjectId;
                plist.Add(tempob);

                list = findparentslist(firstpid, all);
                plist.Reverse();
                plist.Remove(plist.Where(x => x.parentObjectId == null).FirstOrDefault());
                return plist;
            }

            return plist;

        }

        private List<Object> findparentslist(int? parentid, List<Object> all)
        {


            // findparentslist(int ? parentid, List < Object > all)
            if (parentid != null)
            {
                Object Ptempob = null;
                Ptempob = findparents(parentid, all);
                plist.Add(Ptempob);
                return findparentslist(Ptempob.parentObjectId, all);
            }
            else
            {
                return plist;
            }


        }

        private Object findparents(int? parentid, List<Object> all)
        {
            Object ptempob = null;
            ptempob = all.Where(o => o.objectId == parentid).FirstOrDefault();

            return ptempob;
        }
        static public List<Object> findChildrenList(string objectName, List<Object> all)
        {
            try
            {
                int objectId = FillCombo.objectsList.Where(x => x.name == objectName.ToString()).FirstOrDefault().objectId;
                return all.Where(x => x.parentObjectId == objectId).ToList();
            }
           catch
            {
                return new List<Object>();
            }

        }
        public async Task<int> Save(Object newObject)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Object/Save";

            var myContent = JsonConvert.SerializeObject(newObject);
            parameters.Add("Object", myContent);
           return await APIResult.post(method, parameters);



            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //// 
            //var myContent = JsonConvert.SerializeObject(newObject);

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
            //    request.RequestUri = new Uri(Global.APIUri
            //                                 + "Object/Save?newObject="
            //                                 + myContent);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Post;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var message = await response.Content.ReadAsStringAsync();
            //        message = JsonConvert.DeserializeObject<string>(message);
            //        return message;
            //    }
            //    return "";
            //}
        }

       
        public async Task<Object> GetByID(int valId)
        {

            Object item = new Object();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", valId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Object/GetByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Object>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;

            //Object Object = new Object();

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
            //    request.RequestUri = new Uri(Global.APIUri + "Object/GetByID");
            //    request.Headers.Add("Id", valId.ToString());
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();

            //        Object = JsonConvert.DeserializeObject<Object>(jsonString);

            //        return Object;
            //    }

            //    return Object;
            //}
        }

      



        public async Task<int> Delete(int objectId, int userId, bool final)
        {
            //int objectId, int userId, bool final
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("objectId", objectId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "Object/Delete";
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
            //    request.RequestUri = new Uri(Global.APIUri + "Object/Delete?objectId=" + objectId + "&userId=" + userId + "&final=" + final);

            //    request.Headers.Add("APIKey", Global.APIKey);

            //    request.Method = HttpMethod.Post;

            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
        }


        // get is exist


    }
}

