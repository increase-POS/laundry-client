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
using System.IO;
using System.Security.Policy;
using laundryApp.ApiClasses;

namespace laundryApp.Classes
{

    class BackupCls
    {
        public int logId { get; set; }

        public string result { get; set; }
        public string fileName { get; set; }


        public async Task<string> getbackup()
        {
            string messsage = "";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mainBranchId", mainBranchId.ToString());
            //parameters.Add("userId", userId.ToString());
            //parameters.Add("date", date.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Backup/getbackup", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    messsage = c.Value.ToString();
                    break;
                }
            }
            return messsage;



        }


        public async Task<string> getrestore()
        {
            string messsage = "";

            //newlog.userId = MainWindow.userID;
            //newlog.posId = MainWindow.posID;

            int logId = (int)MainWindow.userLogin.userId;
            BackupCls item = new BackupCls();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("logId", logId.ToString());
            parameters.Add("fileName", fileName);
            //parameters.Add("userId", userId.ToString());fileName
            //parameters.Add("date", date.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Backup/getrestore", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    if (c.Type == "scopes")
                    {
                        item = JsonConvert.DeserializeObject<BackupCls>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                        break;
                    }
                }
            }
            if (item.result == "1")
            {
                MainWindow.userLogin.userId = item.logId;
            }


            return item.result;



        }

        public async Task<string> GetFile(string dest)
        {
            //  = "back.bak";
            //  dest = @"D:\temp\back.bak";

            Stream jsonString = null;
            //  byte[] byteImg = null;


            // ... Use HttpClient.
            ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            try
            {
                using (var client = new HttpClient())
                {
                    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                    client.BaseAddress = new Uri(Global.APIUri);
                    client.DefaultRequestHeaders.Clear();
                    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
                    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
                    HttpRequestMessage request = new HttpRequestMessage();
                    request.RequestUri = new Uri(Global.APIUri + "Backup/GetFile");
                    //request.Headers.Add("APIKey", Global.APIKey);

                    request.Method = HttpMethod.Get;
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = await client.SendAsync(request);

                    if (response.IsSuccessStatusCode)
                    {
                        jsonString = await response.Content.ReadAsStreamAsync();


                        //  img = Bitmap.FromStream(jsonString);

                        //  byteImg = await response.Content.ReadAsByteArrayAsync();

                        // configure trmporery path
                        //   string dir = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.FullName;
                        // string tmpPath = Path.Combine(dir, Global.TMPAgentsFolder);

                        //  if (!Directory.Exists(dest))
                        //    Directory.CreateDirectory(dest);

                        // tmpPath = Path.Combine(tmpPath, imageName);

                        //if (System.IO.File.Exists(dest))
                        //{
                        //    System.IO.File.Delete(dest);
                        //}
                        CopyStream(jsonString, dest);
                    }

                }
                return "1";
            }
            catch
            {
                return "0";
            }


        }
        public void CopyStream(Stream stream, string destPath)
        {
            using (var fileStream = new FileStream(destPath, FileMode.Create, FileAccess.Write))
            {
                stream.CopyTo(fileStream);
            }
        }
        public async Task<string> Getpath(string fileName)

        {
            string message = "";
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
                request.RequestUri = new Uri(Global.APIUri + "Backup/Getpath?fileName=" + fileName);
                //request.Headers.Add("APIKey", Global.APIKey);

                request.Method = HttpMethod.Get;
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.SendAsync(request);
                string baseUrl = request.RequestUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped);
                if (response.IsSuccessStatusCode)
                {
                    message = await response.Content.ReadAsStringAsync();
                    message = JsonConvert.DeserializeObject<string>(message);

                }
                return baseUrl;
            }
        }



        public void ProcessDirectory(string targetDirectory)
        {
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                bool inuse = false;

                inuse = IsFileInUse(fileName);
                if (inuse == false)
                {
                    File.Delete(fileName);
                }

                //ProcessFile(fileName);
            }




        }

        private bool IsFileInUse(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                //throw new ArgumentException("'path' cannot be null or empty.", "path");
                return true;
            }


            try
            {
                using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite)) { }
            }
            catch (IOException)
            {
                return true;
            }

            return false;
        }



        public async Task<string> uploadFile(string source)
        {
            string message = "";
            if (source != "" && System.IO.File.Exists(source))
            {

                string fName = Path.GetFileNameWithoutExtension(source);
                fileName = Path.GetFileName(source);
                //string fileName = agentId.ToString();
                MultipartFormDataContent form = new MultipartFormDataContent();
                // get file extension


                try
                {


                    // read file
                    var stream = new FileStream(source, FileMode.Open, FileAccess.Read);

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
                            Name = fName,
                            FileName = fileName
                        };
                        form.Add(content, "fileToUpload");

                        var response = await client.PostAsync(@"Backup/uploadfile", form);
                        if (response.IsSuccessStatusCode)
                        {
                            message = await response.Content.ReadAsStringAsync();
                            message = JsonConvert.DeserializeObject<string>(message);
                            //await save();
                            // return message;
                        }
                    }
                    stream.Dispose();
                    if (message == "1")
                    {
                        message = await getrestore();
                    }
                    return message;
                    //delete tmp image

                }
                catch
                { return "0"; }
            }
            else
            {
                return "-3";
            }

        }


    }
}

