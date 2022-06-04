using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp.ApiClasses;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;

namespace laundryApp.Classes
{
    public class Category
    {
        public int categoryId { get; set; }
        public string categoryCode { get; set; }
        public string name { get; set; }
        public string details { get; set; }
        public string type { get; set; }
        public string image { get; set; }
        public byte isActive { get; set; }
        public decimal taxes { get; set; }
        //public Nullable<int> parentId { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public string notes { get; set; }


        public Boolean canDelete { get; set; }
        public Nullable<int> sequence { get; set; }
        public Nullable<int> id { get; set; }

     
      
        public async Task<List<Category>> Get(string type = "")
        {
            List<Category> items = new List<Category>();
            //#################
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("type", type);
            IEnumerable<Claim> claims = await APIResult.getList("Categories/Get", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Category>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<Category>> GetAllCategories( int userId)
        {
            List<Category> items = new List<Category>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", userId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Categories/GetAllCategories", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Category>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        
      
        // adding or editing  category by calling API metod "save"
        // if categoryId = 0 will call save else call edit
        public async Task<int> save(Category item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Categories/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<int> delete(int categoryId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", categoryId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "Categories/Delete";
           return await APIResult.post(method, parameters);
        }
        public async Task<int> updateImage(Category card)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var myContent = JsonConvert.SerializeObject(card);
            parameters.Add("itemObject", myContent);
            string method = "Categories/UpdateImage";
           return await APIResult.post(method, parameters);
        }
        public async Task<Boolean> uploadImage(string imagePath, string imageName, int categoryId)
        {
            if (imagePath != "")
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                // get file extension
                var ext = imagePath.Substring(imagePath.LastIndexOf('.'));
                var extension = ext.ToLower();
                string fileName = imageName + extension;
                try
                {
                    // configure trmporery path
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = Path.Combine(dir, Global.TMPFolder);
                    string[] files = System.IO.Directory.GetFiles(tmpPath, imageName + ".*");
                    foreach (string f in files)
                    {
                        System.IO.File.Delete(f);
                    }
                    tmpPath = Path.Combine(tmpPath, imageName + extension);

                    if (imagePath != tmpPath) // edit mode
                    {
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

                            content.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                            {
                                Name = imageName,
                                FileName = fileName
                            };
                            form.Add(content, "fileToUpload");

                            var response = await client.PostAsync(@"Categories/PostCategoryImage", form);
                        }
                        stream.Dispose();
                    }
                    // save image name in DB
                    Category category = new Category();
                    category.categoryId = categoryId;
                    category.image = fileName;
                    await updateImage(category);

                    return true;
                }
                catch
                { return false; }
            }
            return false;
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
                request.RequestUri = new Uri(Global.APIUri + "Categories/GetImage?imageName=" + imageName);
                request.Method = HttpMethod.Get;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    jsonString = await response.Content.ReadAsStreamAsync();
                    img = Bitmap.FromStream(jsonString);
                    byteImg = await response.Content.ReadAsByteArrayAsync();

                    // configure trmporery path
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = Path.Combine(dir, Global.TMPFolder);
                    if (!Directory.Exists(tmpPath))
                        Directory.CreateDirectory(tmpPath);
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(imageName);
                    string[] files = System.IO.Directory.GetFiles(tmpPath, fileName+".*");
                    foreach (string f in files)
                    {
                        System.IO.File.Delete(f);
                    }
                    tmpPath = Path.Combine(tmpPath, imageName);

                    using (FileStream fs = new FileStream(tmpPath, FileMode.Create, FileAccess.ReadWrite))
                    {
                        fs.Write(byteImg, 0, byteImg.Length);
                    }
                }
                return byteImg;
            }
        }
       
    }
}
