using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp;
using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Windows;
using System.Security.Claims;
using laundryApp.ApiClasses;

namespace laundryApp.Classes
{

    public class PayedInvclass
    {
        public string processType { get; set; }
        public decimal cash { get; set; }
        public string cardName { get; set; }
        public int sequenc { get; set; }

    }
    public class CashTransfer
    {

        public int cashTransId { get; set; }
        public string transType { get; set; }
        public Nullable<int> posId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<int> agentId { get; set; }
        public Nullable<int> invId { get; set; }
        public string transNum { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public decimal cash { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> createUserId { get; set; }
        public string notes { get; set; }
        public Nullable<int> posIdCreator { get; set; }
        public byte isConfirm { get; set; }
        public Nullable<int> cashTransIdSource { get; set; }
        public string side { get; set; }
        public string docName { get; set; }
        public string docNum { get; set; }
        public string docImage { get; set; }
        public Nullable<int> bankId { get; set; }
        public string bankName { get; set; }
        public string agentName { get; set; }
        public string usersName { get; set; }// side=u
        public string posName { get; set; }
        public string pos2Name { get; set; }
        public Nullable<int> pos2Id { get; set; }
        public string posCreatorName { get; set; }
        public int cashTrans2Id { get; set; }
        public Nullable<byte> isConfirm2 { get; set; }
        public string processType { get; set; }
        public Nullable<int> cardId { get; set; }
        public string createUserName { get; set; }
        public string createUserJob { get; set; }
        public string createUserLName { get; set; }
        public string usersLName { get; set; } // side=u
        public string cardName { get; set; }// processType=card
        public string reciveName { get; set; }
        public Nullable<int> bondId { get; set; }
        public Nullable<System.DateTime> bondDeserveDate { get; set; }
        public Nullable<byte> bondIsRecieved { get; set; }
        public Nullable<int> shippingCompanyId { get; set; }
        public string shippingCompanyName { get; set; }
        public string userAcc { get; set; }

        //for reports
        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorname { get; set; }
        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }
        public Nullable<int> branch2Id { get; set; }
        public string branch2Name { get; set; }
        public string updateUserAcc { get; set; }
        public int points { get; set; }
        ////
        public async Task<List<CashTransfer>> GetCashTransferAsync(string type, string side)
        {
            // string type, string side
            List<CashTransfer> list = new List<CashTransfer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("type", type.ToString());
            parameters.Add("side", side.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Cashtransfer/GetBytypeandSide", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransfer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }

        
        public async Task<List<CashTransfer>> GetCashTransferForPosById(string type, string side, int posId)
        {
            // string type, string side
            List<CashTransfer> list = new List<CashTransfer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("type", type.ToString());
            parameters.Add("side", side.ToString());
            parameters.Add("posId", posId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Cashtransfer/GetCashTransferForPosById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransfer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


        }
        public async Task<List<CashTransfer>> GetCashBond(string type, string side)
        {
            // string type, string side
            List<CashTransfer> list = new List<CashTransfer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("type", type.ToString());
            parameters.Add("side", side.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Cashtransfer/GetCashBond", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransfer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }    

        //GetListByInvId
        public async Task<List<CashTransfer>> GetListByInvId(int invId)
        {
            List<CashTransfer> list = new List<CashTransfer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("invId", invId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Cashtransfer/GetListByInvId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransfer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
         
        }
       
        /// ///////////////////////////////////////

        public async Task<int> Save(CashTransfer cashTr)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Cashtransfer/Save";

            var myContent = JsonConvert.SerializeObject(cashTr);
            parameters.Add("Object", myContent);
            return await APIResult.post(method, parameters);
         
        }


        public async Task<List<PayedInvclass>> GetPayedByInvId(int invId)
        {

            List<PayedInvclass> list = new List<PayedInvclass>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("invId", invId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Cashtransfer/GetPayedByInvId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<PayedInvclass>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
        }


        public async Task<List<CashTransfer>> GetbySourcId(string side, int sourceId)
        {

            List<CashTransfer> list = new List<CashTransfer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("side", side);
            parameters.Add("sourceId", sourceId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Cashtransfer/GetbySourcId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransfer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
         
        }

        public async Task<int> deletePosTrans(int cashTransId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("cashTransId", cashTransId.ToString());

            string method = "Cashtransfer/Delete";
            return await APIResult.post(method, parameters);
          
        }

        public async Task<int> PayByAmmount(int agentId, decimal ammount, string payType, CashTransfer cashTr)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("agentId", agentId.ToString());
            parameters.Add("amount", ammount.ToString());
            parameters.Add("payType", payType.ToString());
            var myContent = JsonConvert.SerializeObject(cashTr);
            parameters.Add("cashTransfer", myContent);



            string method = "Cashtransfer/payByAmount";
            return await APIResult.post(method, parameters);
        
        }

        public async Task<int> PayUserByAmmount(int userId, decimal ammount, string payType, CashTransfer cashTr)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("userId", userId.ToString());
            parameters.Add("amount", ammount.ToString());
            parameters.Add("payType", payType.ToString());
            var myContent = JsonConvert.SerializeObject(cashTr);
            parameters.Add("cashTransfer", myContent);



            string method = "Cashtransfer/payUserByAmount";
            return await APIResult.post(method, parameters);

        }

        public async Task<int> payShippingCompanyByAmount(int shippingCompanyId, decimal ammount, string payType, CashTransfer cashTr)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("shippingCompanyId", shippingCompanyId.ToString());
            parameters.Add("amount", ammount.ToString());
            parameters.Add("payType", payType.ToString());
            var myContent = JsonConvert.SerializeObject(cashTr);
            parameters.Add("cashTransfer", myContent);



            string method = "Cashtransfer/payShippingCompanyByAmount";
            return await APIResult.post(method, parameters);

            #region
            //string message = "";
            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            //string myContent = JsonConvert.SerializeObject(cashTr);

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

            //    request.RequestUri = new Uri(Global.APIUri + "Cashtransfer/payShippingCompanyByAmount?shippingCompanyId=" + shippingCompanyId + "&amount=" + ammount + "&payType=" + payType + "&cashtransfer=" + myContent);

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
            #endregion
        }

        public async Task<int> PayListOfInvoices(int agentId, List<Invoice> invoicelst, string payType, CashTransfer cashTr)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("agentId", agentId.ToString());

            parameters.Add("payType", payType);
            var myContent = JsonConvert.SerializeObject(invoicelst);
            parameters.Add("invoices", myContent);
            var myContent2 = JsonConvert.SerializeObject(cashTr);
            parameters.Add("cashTransfer", myContent2);



            string method = "Cashtransfer/payListOfInvoices";
            return await APIResult.post(method, parameters);
        }

        public async Task<int> PayUserListOfInvoices(int userId, List<Invoice> invoicelst, string payType, CashTransfer cashTr)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("userId", userId.ToString());

            parameters.Add("payType", payType);
            var myContent = JsonConvert.SerializeObject(invoicelst);
            parameters.Add("invoices", myContent);
            var myContent2 = JsonConvert.SerializeObject(cashTr);
            parameters.Add("cashTransfer", myContent2);

            string method = "Cashtransfer/payUserListOfInvoices";
            return await APIResult.post(method, parameters);

        }

        public async Task<int> PayShippingCompanyListOfInvoices(int shippingCompanyId, List<Invoice> invoicelst, string payType, CashTransfer cashTr)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            parameters.Add("shippingCompanyId", shippingCompanyId.ToString());

            parameters.Add("payType", payType);
            var myContent = JsonConvert.SerializeObject(invoicelst);
            parameters.Add("invoices", myContent);
            var myContent2 = JsonConvert.SerializeObject(cashTr);
            parameters.Add("cashTransfer", myContent2);

            string method = "Cashtransfer/payShippingCompanyListOfInvoices";
            return await APIResult.post(method, parameters);

        }

        public async Task<string> generateCashNumber(string cashNum)
        {
            int sequence = await GetLastNumOfCash(cashNum);
            sequence++;
            string strSeq = sequence.ToString();
            if (sequence <= 999999)
                strSeq = sequence.ToString().PadLeft(6, '0');
            string transNum = cashNum + "-" + strSeq;
            return transNum;
        }

        public async Task<int> GetLastNumOfCash(string cashNum)
        {
            int message = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("cashCode", cashNum);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Cashtransfer/GetLastNumOfCash", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    message = int.Parse(c.Value); ;
                    break;
                }
            }
            return message;
        }
        public async Task<string> getLastOpenTransNum(int posId)
        {
            string message = "";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("posId", posId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Cashtransfer/getLastOpenTransNum", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    message = c.Value;
                    break;
                }
            }
            return message;
        }

        public async Task<string> generateDocNumber(string docNum)
        {
            int sequence = await GetLastNumOfDocNum(docNum);
            sequence++;
            string strSeq = sequence.ToString();
            if (sequence <= 999999)
                strSeq = sequence.ToString().PadLeft(6, '0');
            string transNum = docNum + "-" + strSeq;
            return transNum;
        }

        public async Task<int> GetLastNumOfDocNum(string docNum)
        {
            int message = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("docNum", docNum);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Cashtransfer/GetLastNumOfDocNum", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    message = int.Parse(c.Value); ;
                    break;
                }
            }
            return message;

            #region old
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
            //    request.RequestUri = new Uri(Global.APIUri + "Cashtransfer/GetLastNumOfDocNum?docNum=" + docNum);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        string message = await response.Content.ReadAsStringAsync();
            //        message = JsonConvert.DeserializeObject<string>(message);
            //        return int.Parse(message);
            //    }
            //    return 0;
            //}
            #endregion
        }

        public async Task<int> GetCashCount(int invoiceId)
        {
            int message = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("invoiceId", invoiceId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("cashTransfer/GetCountByInvId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    message = int.Parse(c.Value);
                    break;
                }
            }
            return message;

            #region old
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
            //    request.RequestUri = new Uri(Global.APIUri + "cashTransfer/GetCountByInvId?invoiceId=" + invoiceId );
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var count = await response.Content.ReadAsStringAsync();
            //        return int.Parse(count);
            //    }
            //    else //web api sent error response 
            //    {
            //        return 0;
            //    }
            //}
            #endregion
        }

    }

}

