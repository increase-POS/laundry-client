using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp.ApiClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
    public class User
    {
        public int userId { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string lastname { get; set; }
        //public string fullName { get; set; }
        public string fullName
        { get { return name + " " + lastname; } }
        public string job { get; set; }
        public string workHours { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public byte isActive { get; set; }
        public string notes { get; set; }
        public byte isOnline { get; set; }

        public string image { get; set; }
        public Nullable<int> groupId { get; set; }
        public decimal balance { get; set; }
        public byte balanceType { get; set; }
        public bool isAdmin { get; set; }

        public byte driverIsAvailable { get; set; }
        public Boolean canDelete { get; set; }

         

        public async Task<List<User>> Get()
        {
            List<User> users = new List<User>();

            IEnumerable<Claim> claims = await APIResult.getList("Users/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    users.Add(JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return users;
        }
        public async Task<List<User>> getBranchSalesMan(int branchId, string objectName)
        {
            List<User> items = new List<User>();

            //########### to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("objectName", objectName);
            IEnumerable<Claim> claims = await APIResult.getList("Users/GetSalesMan", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<User>> GetUsersActive()
        {
            List<User> items = new List<User>();
            IEnumerable<Claim> claims = await APIResult.getList("Users/GetActive");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<User>> GetActiveForAccount(string payType)
        {
            List<User> items = new List<User>();
            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("payType", payType.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("Users/GetActiveForAccount", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<List<User>> getUsersForDelivery(string job, int customerId)
        {
            List<User> items = new List<User>();
            //  to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("job", job);
            parameters.Add("customerId", customerId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("Users/getUsersForDelivery", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<User> Getloginuser(string userName, string password)
        {
            User user = new User();

            //########### to pass parameters (optional)
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("userName", userName);
            parameters.Add("password", password);
            IEnumerable<Claim> claims = await APIResult.getList("Users/Getloginuser", parameters);
            //#################

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    user = JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return user;
        }
        public async Task<User> getUserById(int userId)
        {
            User user = new User();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", userId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Users/GetUserByID", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    user = JsonConvert.DeserializeObject<User>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return user;
        }
         
        public async Task<int> save(User user)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Users/Save";

            var myContent = JsonConvert.SerializeObject(user);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }

        public async Task<int> delete(int delUserId, int userId, bool final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("delUserId", delUserId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());

            string method = "Users/Delete";
           return await APIResult.post(method, parameters);
        }
        public async Task<int> updateImage(User user)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var myContent = JsonConvert.SerializeObject(user);
            parameters.Add("itemObject", myContent);

            string method = "Users/UpdateImage";
           return await APIResult.post(method, parameters);
        }
        public async Task<string> uploadImage(string imagePath, string imageName, int userId)
        //public async Task<Boolean> uploadImage(string imagePath, int userId)
        {
            if (imagePath != "")
            {
                //string imageName = userId.ToString();
                MultipartFormDataContent form = new MultipartFormDataContent();
                // get file extension
                var ext = imagePath.Substring(imagePath.LastIndexOf('.'));
                var extension = ext.ToLower();
                try
                {
                    // configure trmporery path
                    string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string tmpPath = Path.Combine(dir, Global.TMPUsersFolder);
                    tmpPath = Path.Combine(tmpPath, imageName + extension);
                    if (System.IO.File.Exists(tmpPath))
                    {
                        System.IO.File.Delete(tmpPath);
                    }
                    // resize image
                    ImageProcess imageP = new ImageProcess(150, imagePath);
                    imageP.ScaleImage(tmpPath);

                    // read image file
                    var stream = new FileStream(tmpPath, FileMode.Open, FileAccess.Read);

                    // create http client request
                    using (var client = new HttpClient())
                    {
                        client.BaseAddress = new Uri(Global.APIUri);
                        client.Timeout = System.TimeSpan.FromSeconds(3600);
                        string boundary = string.Format("----WebKitFormBoundary{0}", DateTime.Now.Ticks.ToString("x"));
                        HttpContent content = new StreamContent(stream);
                        content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                        content.Headers.Add("client", "true");

                        string fileName = imageName + extension;
                        content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                        {
                            Name = imageName,
                            FileName = fileName
                        };
                        form.Add(content, "fileToUpload");

                        var response = await client.PostAsync(@"users/PostUserImage", form);
                        if (response.IsSuccessStatusCode)
                        {
                            // save image name in DB
                            User user = new User();
                            user.userId = userId;
                            user.image = fileName;
                            await updateImage(user);
                            return fileName;
                        }
                    }
                    stream.Dispose();
                    //delete tmp image
                    if (System.IO.File.Exists(tmpPath))
                    {
                        System.IO.File.Delete(tmpPath);
                    }
                }
                catch
                { return ""; }
            }
            return "";
        }
        public async Task<byte[]> downloadImage(string imageName)

        {
            Stream jsonString = null;
            byte[] byteImg = null;
            Image img = null;
            // ... Use HttpClient.
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            using (var client = new HttpClient())
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                client.BaseAddress = new Uri(Global.APIUri);
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
                HttpRequestMessage request = new HttpRequestMessage();
                request.RequestUri = new Uri(Global.APIUri + "Users/GetImage?imageName=" + imageName);
                request.Headers.Add("APIKey", Global.APIKey);
                request.Method = HttpMethod.Get;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    jsonString = await response.Content.ReadAsStreamAsync();
                    img = Bitmap.FromStream(jsonString);
                    byteImg = await response.Content.ReadAsByteArrayAsync();

                    // configure trmporery path
                    string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string tmpPath = Path.Combine(dir, Global.TMPUsersFolder);
                    if (!Directory.Exists(tmpPath))
                        Directory.CreateDirectory(tmpPath);
                    tmpPath = Path.Combine(tmpPath, imageName);
                    if (System.IO.File.Exists(tmpPath))
                    {
                        System.IO.File.Delete(tmpPath);
                    }
                    using (FileStream fs = new FileStream(tmpPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fs.Write(byteImg, 0, byteImg.Length);
                    }
                }
                return byteImg;
            }
        }

        public async Task<int> checkLoginAvalability(int posId, string userName, string password)
        {
            string motherCode = setupConfiguration.GetMotherBoardID();
            string hardCode = setupConfiguration.GetHDDSerialNo();
            string deviceCode = motherCode + "-" + hardCode;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Users/checkLoginAvalability";

            parameters.Add("deviceCode", deviceCode);
            parameters.Add("posId", posId.ToString());
            parameters.Add("userName", userName);
            parameters.Add("password", password);
            return await APIResult.post(method, parameters);
        }

    }
}
