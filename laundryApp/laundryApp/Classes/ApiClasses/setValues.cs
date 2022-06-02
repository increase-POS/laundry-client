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
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

using System.Security.Claims;
using laundryApp.ApiClasses;
namespace laundryApp.Classes
{
    public class SetValues
    {
        public int valId { get; set; }
        public string value { get; set; }
        public Nullable<int> isDefault { get; set; }
        public Nullable<int> isSystem { get; set; }
        public string notes { get; set; }
        public Nullable<int> settingId { get; set; }
        //setting
        public string name { get; set; }
        public async Task<List<SetValues>> GetAll()
        {

            List<SetValues> list = new List<SetValues>();
            //  Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mainBranchId", mainBranchId.ToString());
            //parameters.Add("userId", userId.ToString());
            //parameters.Add("date", date.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("setValues/Get");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<SetValues>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<SetValues> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "setValues/Get");
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
            //        list = JsonConvert.DeserializeObject<List<SetValues>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<SetValues>();
            //    }
            //    return list;
            //}
        }


        //print
       
        public async Task<List<SetValues>> GetBySetvalNote(string setvalnote)
        {
            List<SetValues> list = new List<SetValues>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("setvalnote", setvalnote);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("setValues/GetBySetvalNote", parameters);



            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<SetValues>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;



            //List<SetValues> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "setValues/GetBySetvalNote?setvalnote=" + setvalnote);
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
            //        list = JsonConvert.DeserializeObject<List<SetValues>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        // SetValues sv = new SetValues();
            //        //    sv.notes =  await response.Content.ReadAsStringAsync();
            //        list = new List<SetValues>();
            //        //  list.Add(sv);

            //    }
            //    return list;
            //}
        }
        // email
        public async Task<List<SetValues>> GetBySetName(string name)
        {

            List<SetValues> list = new List<SetValues>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", name);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("setValues/GetBySetName", parameters);



            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<SetValues>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


            //List<SetValues> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "setValues/GetBySetName?name=" + name);
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
            //        list = JsonConvert.DeserializeObject<List<SetValues>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //       // SetValues sv = new SetValues();
            //    //    sv.notes =  await response.Content.ReadAsStringAsync();
            //       list = new List<SetValues>();
            //      //  list.Add(sv);

            //    }
            //    return list;
            //}
        }
        public async Task<SetValues> GetBySetNameAndUserId(string name, int userId)
        {

            SetValues list = new SetValues();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("name", name);
            parameters.Add("userId", userId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("setValues/GetBySetNameAndUserId", parameters);



            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list = JsonConvert.DeserializeObject<SetValues>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
            return list;


         
        }
        // email
        public async Task<int> SaveValueByNotes(SetValues obj)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "setValues/SaveValueByNotes";

            var myContent = JsonConvert.SerializeObject(obj);
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
            //                                 + "setValues/SaveValueByNotes?newObject="
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

        public async Task<int> Save(SetValues obj)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "setValues/Save";

            var myContent = JsonConvert.SerializeObject(obj);
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
            //                                 + "setValues/Save?newObject="
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


        public async Task<SetValues> GetByID(int valId)
        {

            SetValues item = new SetValues();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", valId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("setValues/GetByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<SetValues>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;


            //SetValues Object = new SetValues();

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
            //    request.RequestUri = new Uri(Global.APIUri + "setValues/GetByID");
            //    request.Headers.Add("Id", valId.ToString());
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();

            //        Object = JsonConvert.DeserializeObject<SetValues>(jsonString);

            //        return Object;
            //    }

            //    return Object;
            //}
        }

      



        public async Task<int> Delete(int Id, int userId)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("Id", Id.ToString());
            parameters.Add("userId", userId.ToString());

            string method = "setValue/Delete";
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
            //    request.RequestUri = new Uri(Global.APIUri + "setValue/Delete?Id=" + Id + "&userId=" + userId );

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

        //image part
        #region image


        public async Task<string> uploadImage(string imagePath, string imageName, int valId)
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
                    //string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = Path.Combine(dir, Global.TMPSettingFolder);
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

                        var response = await client.PostAsync(@"setValues/PostImage", form);
                        if (response.IsSuccessStatusCode)
                        {
                            // save image name in DB
                            SetValues setValues = new SetValues();
                            setValues.valId = valId;
                            setValues.value = fileName;
                            await updateImage(setValues);
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
        public async Task<int> updateImage(SetValues setValues)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "setValues/UpdateImage";

            var myContent = JsonConvert.SerializeObject(setValues);
            parameters.Add("Object", myContent);
            return await APIResult.post(method, parameters);

            //string message = "";

            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            //string myContent = JsonConvert.SerializeObject(setValues);

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

            //    request.RequestUri = new Uri(Global.APIUri + "setValues/UpdateImage?SetValuesObject=" + myContent);
            //    request.Headers.Add("APIKey", Global.APIKey);
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
                request.RequestUri = new Uri(Global.APIUri + "setValues/GetImage?imageName=" + imageName);
                //request.Headers.Add("APIKey", Global.APIKey);
                request.Method = HttpMethod.Get;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    jsonString = await response.Content.ReadAsStreamAsync();
                    img = Bitmap.FromStream(jsonString);
                    byteImg = await response.Content.ReadAsByteArrayAsync();

                    // configure trmporery path
                    //string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string dir = Directory.GetCurrentDirectory();
                    string tmpPath = Path.Combine(dir, Global.TMPSettingFolder);
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

        public async Task getImg(string imageName)
        {
           
            try
            {
                if (string.IsNullOrEmpty(imageName))
                {
                   // SectionData.clearImg(img_customer);
                }
                else
                {
                    byte[] imageBuffer = await this.downloadImage(imageName); // read this as BLOB from your DB

                   var bitmapImage = new BitmapImage();
                 
                    if (imageBuffer != null)
                    {
                        using (var memoryStream = new MemoryStream(imageBuffer))
                        {
                            bitmapImage.BeginInit();
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.EndInit();
                         Bitmap serial_bitmap = new Bitmap(memoryStream);
                         //string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                            string dir = Directory.GetCurrentDirectory();
                            string tmpPath = System.IO.Path.Combine(dir, Global.TMPSettingFolder);
                         tmpPath = System.IO.Path.Combine(tmpPath, imageName);
                         serial_bitmap.Save(tmpPath);
                        }
                      
                       
                    }
                
                }
            }
            catch { }
        }

        #endregion

        //


    }
}

