using LiveCharts;
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
    public class InvoiceCount
    {
        public string invType { get; set; }
        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorName { get; set; }
        public int purCount { get; set; }
        public int saleCount { get; set; }
        public int purBackCount { get; set; }
        public int saleBackCount { get; set; }

        // new
        public int diningHallCount { get; set; }
        public int takeAwayCount { get; set; }
        public int selfServiceCount { get; set; }

        



    }
    //public class AgentsCount
    //{


    //    public int vendorCount { get; set; }
    //    public int customerCount { get; set; }


    //}
    public class UserOnlineCount
    {

        public int branchId { get; set; }
        public string branchName { get; set; }
        public int userOnlineCount { get; set; }
        public int allPos { get; set; }
        // public int allUsers { get; set; }
        public int offlineUsers { get; set; }

    }
    public class userOnlineInfo
    { 
        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }
        public Nullable<byte> branchisActive { get; set; }
        public Nullable<int> posId { get; set; }
        public string posName { get; set; }
        public Nullable<byte> posisActive { get; set; }
        public Nullable<int> userId { get; set; }
        public string usernameAccount { get; set; }
        public string userName { get; set; }
        public string lastname { get; set; }
        public string job { get; set; }
        public string phone { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string address { get; set; }
        public Nullable<byte> userisActive { get; set; }
        public Nullable<byte> isOnline { get; set; }
        public string image { get; set; }

    }
    public class BranchOnlineCount
    {

        public int branchOnline { get; set; }
        public int branchAll { get; set; }
        public int branchOffline { get; set; }


    }
    public class BestSeller
    {
        public string itemName { get; set; }
        public string unitName { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public Nullable<int> itemId { get; set; }
        public Nullable<int> unitId { get; set; }
        public Nullable<long> quantity { get; set; }
        public Nullable<decimal> price { get; set; }
        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorName { get; set; }
        public Nullable<decimal> subTotal { get; set; }
    }
   
    // storage
    public class IUStorage
    {

        public string itemName { get; set; }
        public string unitName { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public Nullable<int> itemId { get; set; }
        public Nullable<int> unitId { get; set; }

        public string branchName { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<long> quantity { get; set; }


    }
    public class TotalPurSale
    {
        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorName { get; set; }
        public Nullable<decimal> totalPur { get; set; }
        public Nullable<decimal> totalSale { get; set; }
        public int countPur { get; set; }
        public int countSale { get; set; }
        public int day { get; set; }

    }

    /*
      public class CashAndTablesCount
    {
        public Nullable<int> branchCreatorId { get; set; }
        public int totalCash { get; set; }
        public int emptyTablesCount { get; set; }
        public int openTablesCount { get; set; }
        public int reservationsCount { get; set; }

    }
    */
    //bestOf
    public class BranchInvoiceCount
    {


        public DateTime fromDate { get; set; }
        public DateTime toDate { get; set; }
        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }
        public int count { get; set; }
        public int dateindex { get; set; }
        public string duration { get; set; }


    }
    public class BestOfCount
    {
        public List<BranchInvoiceCount> CountinMonthsList { get; set; }
        public List<BranchInvoiceCount> CountinDaysList   { get; set; }
        public List<BranchInvoiceCount> CountinHoursList  { get; set; }


        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }



    }

    //branches balance


    public class BranchBalance
    {
        public string branchName { get; set; }
        public  decimal   balance { get; set; }
        public int branchId{ get; set; }
        public string branchType{ get; set; }
        public string branchCode{ get; set; }
        public byte banchIsActive{ get; set; }

    }

    // عدد الفواتير بكل فرع حسب نوع الفاتورة
    public class CountByInvType
    {
        public string invType { get; set; }
        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }
        public int dhallCount { get; set; }
        public int selfCount { get; set; }
        public int tawayCount { get; set; }

    }
    public class Dash
    {

        //public string countAllPurchase { get; set; }
        //public string countAllSalesValue { get; set; }
        //public string customerCount { get; set; }
        //public string vendorCount { get; set; }

        // new
        // branch balance
        public string balance { get; set; }
        public string emptyCount { get; set; }
        public string openedCount { get; set; }
        public string reservedCount { get; set; }
        // new
        //public string diningHallCount { get; set; }
        //public string takeAwayCount { get; set; }
        //public string selfServiceCount { get; set; }

        // count by inv type
        public string dhallCount { get; set; }
        public string selfCount { get; set; }
        public string tawayCount { get; set; }

        


        public string userOnline { get; set; }
        public string userOffline { get; set; }

        public string branchOnline { get; set; }
        public string branchOffline { get; set; }

        public string countDailyPurchase { get; set; }
        public string countDailySales { get; set; }

        public string countMonthlyPurchase { get; set; }
        public string countMonthlySales { get; set; }

        public List<userOnlineInfo> listUserOnline;

        public List<BestOfCount> listBestOfCount;

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> YFormatter { get; set; }

        // bestOff
        public class BranchInvoicesCount
        {
            public List<BranchInvoiceCount> CountinMonthsList { get; set; }
            public List<BranchInvoiceCount> CountinDaysList { get; set; }
            public List<BranchInvoiceCount> CountinHoursList { get; set; }
            public Nullable<int> branchId { get; set; }
            public string branchName { get; set; }

        }
        public class BranchInvoiceCount
        {
            public DateTime fromDate { get; set; }
            public DateTime toDate { get; set; }
            public Nullable<int> branchId { get; set; }
            public string branchName { get; set; }
            public int count { get; set; }
            public int dateindex { get; set; }

        }
      
       
        //عدد المستخدمين المتصلين والغير متصلين  حاليا في كل فرع 
        public async Task<List<UserOnlineCount>> Getuseronline(int mainBranchId, int userId)
        {

            List<UserOnlineCount> list = new List<UserOnlineCount>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("dash/Getuseronline", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<UserOnlineCount>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<UserOnlineCount> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "dash/Getuseronline");
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
            //        list = JsonConvert.DeserializeObject<List<UserOnlineCount>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<UserOnlineCount>();
            //    }
            //    return list;
            //}
        }
        //بيانات المستخدمين المتصلين
        public async Task<List<userOnlineInfo>> GetuseronlineInfo(int mainBranchId, int userId)
        {
            List<userOnlineInfo> list = new List<userOnlineInfo>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("dash/GetuseronlineInfo", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<userOnlineInfo>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


            //List<userOnlineInfo> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "dash/GetuseronlineInfo");
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
            //        list = JsonConvert.DeserializeObject<List<userOnlineInfo>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<userOnlineInfo>();
            //    }
            //    return list;
            //}
        }
        // عدد الفروع المتصلة وغير المتصلة
        public async Task<List<BranchOnlineCount>> GetBrachonline(int mainBranchId, int userId)
        {
            List<BranchOnlineCount> list = new List<BranchOnlineCount>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("dash/GetBrachonline", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<BranchOnlineCount>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }
        // عدد فواتير المبيعات ومرتجع المبيعات والمشتريات ومرتجع المشتريات حسب الفرع في اليوم الحالي
        public async Task<List<InvoiceCount>> GetdashsalpurDay(int mainBranchId, int userId)
        {
            List<InvoiceCount> list = new List<InvoiceCount>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("dash/GetdashsalpurDay", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<InvoiceCount>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

           
        }
        // اكثر 10 اصناف مبيعا
        public async Task<List<BestSeller>> Getbestseller(int mainBranchId, int userId)
        {
            List<BestSeller> list = new List<BestSeller>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("dash/Getbestseller", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<BestSeller>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
          return list;
         
        }
        //كمية قائمة من 10 اصناف في كل فرع 
        public async Task<List<IUStorage>> GetIUStorage(List<ItemUnit> IUList, int mainBranchId, int userId)
        {

            List<IUStorage> list = new List<IUStorage>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            var myContent = JsonConvert.SerializeObject(IUList);
            parameters.Add("IUList", myContent);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("dash/GetIUStorage", parameters);



            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<IUStorage>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;          
        }
        // مجموع مبالغ المشتريات والمبيعات اليومي خلال الشهر الحالي لكل فرع
        public async Task<List<TotalPurSale>> GetTotalPurSale(int mainBranchId, int userId)
        {
            List<TotalPurSale> list = new List<TotalPurSale>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("dash/GetTotalPurSale", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<TotalPurSale>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        
        }

        // عدد الفواتير في كل فرع وحسب التاريخ BestOf
        public async Task<List<BestOfCount>> GetBestOf(int mainBranchId, int userId)
        {

            List<BestOfCount> list = new List<BestOfCount>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("dash/GetBestOf", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<BestOfCount>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
        }

        // الكاش في كل فرع
        public async Task<List<BranchBalance>> GetCashBalance(int mainBranchId, int userId)
        {

            List<BranchBalance> list = new List<BranchBalance>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("dash/GetCashBalance", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<BranchBalance>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
        }

        // عدد الفواتير بكل فرع حسب نوع الفاتورة
        public async Task<List<CountByInvType>> GetCountByInvType(int mainBranchId, int userId)
        {

            List<CountByInvType> list = new List<CountByInvType>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("dash/GetCountByInvType", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CountByInvType>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
        }
    }
}
