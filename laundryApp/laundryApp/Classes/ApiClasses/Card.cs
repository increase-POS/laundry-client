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
    public class Card
    {
        public int cardId { get; set; }
        public string name { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public byte isActive { get; set; }
        public string image { get; set; }
        public bool hasProcessNum { get; set; }
        public Boolean canDelete { get; set; }

       

        public async Task<List<Card>> GetAll()
        {
            List<Card> items = new List<Card>();
            IEnumerable<Claim> claims = await APIResult.getList("cards/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Card>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
              
        public async Task<int> save(Card item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "cards/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
        public async Task<int> delete(int cardId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", cardId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "cards/Delete";
           return await APIResult.post(method, parameters);
        }
        public async Task<int> updateImage(Card card)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            var myContent = JsonConvert.SerializeObject(card);
            parameters.Add("itemObject", myContent);
            string method = "cards/UpdateImage";
           return await APIResult.post(method, parameters);
        }
        public async Task<string> uploadImage(string imagePath, string imageName, int cardId)
        {
            if (imagePath != "")
            {
                MultipartFormDataContent form = new MultipartFormDataContent();
                // get file extension
                var ext = imagePath.Substring(imagePath.LastIndexOf('.'));
                var extension = ext.ToLower();
                try
                {
                    // configure trmporery path
                    string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                    string tmpPath = Path.Combine(dir, Global.TMPFolder);
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

                        var response = await client.PostAsync(@"cards/PostCardImage", form);
                        if (response.IsSuccessStatusCode)
                        {
                            // save image name in DB
                            Card card = new Card();
                            card.cardId = cardId;
                            card.image = fileName;
                            await updateImage(card);
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
                request.RequestUri = new Uri(Global.APIUri + "cards/GetImage?imageName=" + imageName);
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
                    string tmpPath = Path.Combine(dir, Global.TMPCardsFolder);
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
    }
}

