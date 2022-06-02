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
    public class Notification
    {
        public int notUserId { get; set; }
        public int notId { get; set; }
        public Nullable<int> userId { get; set; }
        public bool isRead { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> posId { get; set; }



        public string title { get; set; }
        public string ncontent { get; set; }
        public string side { get; set; }
        public string msgType { get; set; }
        public string path { get; set; }

      
        public byte isActive { get; set; }

        //***********************************************
        public async Task<int> save(Notification obj, int branchId, string objectName, string prefix,
            int userId = 0, int posId = 0)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "notification/Save";
            var myContent = JsonConvert.SerializeObject(obj);
            parameters.Add("itemObject", myContent);
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("objectName", objectName);
            parameters.Add("prefix", prefix);
            parameters.Add("userId", userId.ToString());
            parameters.Add("posId", posId.ToString());


           return await APIResult.post(method, parameters);
        }
       
    }



    public  class NotificationUser
    {
        public int notUserId { get; set; }
        public Nullable<int> notId { get; set; }
        public Nullable<int> userId { get; set; }
        public bool isRead { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public string title { get; set; }
        public string ncontent { get; set; }
        public string side { get; set; }
        public string msgType { get; set; }
        public string path { get; set; }

        public async Task<List<NotificationUser>> GetByUserId(int userId, string type, int posId)
        {
            List<NotificationUser> items = new List<NotificationUser>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", userId.ToString());
            parameters.Add("type", type.ToString());
            parameters.Add("posId", posId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("notificationUser/GetByUserId", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<NotificationUser>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<int> GetCountByUserId(int userId, string type, int posId)
        {
            int count =0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", userId.ToString());
            parameters.Add("type", type.ToString());
            parameters.Add("posId", posId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("notificationUser/GetNotUserCount", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    count =int.Parse(c.Value);
                }
            }
            return count;
        }

        public async Task<int> save(NotificationUser item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "notificationUser/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<int> setAsRead(int notUserId, int posId, string type)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "notificationUser/setAsRead";
            parameters.Add("notUserId", notUserId.ToString());
            parameters.Add("posId", posId.ToString());
            parameters.Add("type", type.ToString());
           return await APIResult.post(method, parameters);
        }

    }
}
