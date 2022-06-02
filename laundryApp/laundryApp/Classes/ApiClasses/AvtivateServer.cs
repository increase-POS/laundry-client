using Newtonsoft.Json;
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

using Newtonsoft.Json.Converters;
using laundryApp.ApiClasses;

namespace laundryApp.Classes
{

    public class PosSerialSend
    {

        public string serial { get; set; }
        public string posDeviceCode { get; set; }

        public Nullable<bool> isBooked { get; set; }
        public int isActive { get; set; }
        public string posName { get; set; }
        public string branchName { get; set; }
        public Nullable<int> posSettingId { get; set; }
        public Nullable<int> posId { get; set; }
        public bool unLimited { get; set; }
    }


    public class packagesSend
    {
        public Nullable<int> packageUserId { get; set; }

        public string packageName { get; set; }

        public int branchCount { get; set; }
        public int posCount { get; set; }
        public int userCount { get; set; }
        public int vendorCount { get; set; }
        public int customerCount { get; set; }
        public int itemCount { get; set; }
        public int salesInvCount { get; set; }

        public string programName { get; set; }

        public string verName { get; set; }

        public int isActive { get; set; }

        public string packageCode { get; set; }

        public int storeCount { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }
        public bool islimitDate { get; set; }
        public Nullable<bool> isOnlineServer { get; set; }
        public string customerServerCode { get; set; }
        public string packageSaleCode { get; set; }
        public int monthCount { get; set; }
        public bool canRenew { get; set; }
        public bool isBooked { get; set; }


        public Nullable<System.DateTime> bookDate { get; set; }

        public Nullable<System.DateTime> expireDate { get; set; }


        public string type { get; set; }
        public bool isPayed { get; set; }

        public Nullable<System.DateTime> activatedate { get; set; }
        public bool isServerActivated { get; set; }
        public int totalsalesInvCount { get; set; }
        public int result { get; set; }

        public string packageNumber { get; set; }



        public Nullable<int> pId { get; set; }
        public Nullable<int> pcdId { get; set; }

        public string activeState { get; set; }
        public string activeres { get; set; }

        public string customerName { get; set; }// 6- customer Name
        public string customerLastName { get; set; }// 6- customer LastName
        public string agentName { get; set; }// 5- Agent name 
        public string agentAccountName { get; set; }//5- Agent AccountName
        public string agentLastName { get; set; }//5- Agent LastName

        public Nullable<System.DateTime> pocrDate { get; set; }
        public Nullable<int> poId { get; set; }
        public string notes { get; set; }
        public string upnum { get; set; }
        public string activeApp { get; set; }
        public string confirmStat { get; set; }
    }
    public class SendDetail
    {
        public List<PosSerialSend> PosSerialSendList;

        public packagesSend packageSend;
    }

    public class AvtivateServer
    {
      
        public string serverUri { get; set; }
        public string activateCode { get; set; }
        /// <summary>
        /// ///////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        /// 





        public async Task<int> checkconn()
        {
            int id = 1;
            int item = 0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("id", id.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Activate/checkconn", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = int.Parse(c.Value) ;
                    break;
                }
            }
            return item;


         
        }

        public async Task<int> Sendserverkey(string skey)
        {
            int item = 0;
           // int res = 0;
          //  SendDetail item = new SendDetail();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("skey", skey);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Activate/Sendserverkey", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = int.Parse(c.Value);
                  // item = JsonConvert.DeserializeObject<SendDetail>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                    break;
                }
            }
            return item;

        }


        public async Task<int> StatSendserverkey(string skey, string activeState)
        {
            int item = 0;
            // int res = 0;
            //  SendDetail item = new SendDetail();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("skey", skey);
            parameters.Add("activeState", activeState);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Activate/StatSendserverkey", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = int.Parse(c.Value);
                    // item = JsonConvert.DeserializeObject<SendDetail>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                    break;
                }
            }
            return item;

        }

        public async Task<int> updatesalecode(string skey)
        {
            int item = 0;
            // int res = 0;
            //  SendDetail item = new SendDetail();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("skey", skey);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Activate/updatesalecode", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = int.Parse(c.Value);
                    // item = JsonConvert.DeserializeObject<SendDetail>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                    break;
                }
            }
            return item;

        }


        public async Task<SetValues> getactivesite()
        {

            SetValues item = new SetValues();


            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Activate/getactivesite");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<SetValues>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;


        }

        public async Task<SendDetail> OfflineActivate(SendDetail SendDetaildata, string activeState)
        {
            SendDetail item = new SendDetail();
            Dictionary<string, string> parameters = new Dictionary<string, string>();

            var myContent3 = JsonConvert.SerializeObject(SendDetaildata);
            parameters.Add("object", myContent3);
            parameters.Add("activeState", activeState);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Activate/OfflineActivate", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<SendDetail>(c.Value, new JsonSerializerSettings { DateParseHandling = DateParseHandling.None });

                }
            }
            return item;
        }




        //AvtivateServer ac = new AvtivateServer();
        ////get
        //SetValues sv = new SetValues();
        //SetValues svModel = new SetValues();

        //sv = await ac.getactivesite();

        //// save
        //sv.value =  @"https://localhost:443";//from textbox
        //        int res = await svModel.Save(sv);


        // extend button start:
        //private async void next_Click(object sender, RoutedEventArgs e)
        //{

        //    int chk = 0;
        //    string activationkey = "";//get from info 
        //    try
        //    {
        //        if (activationkey.Trim() != "".Trim())
        //        {
        //            AvtivateServer ac = new AvtivateServer();

        //            chk = await ac.checkconn();

        //            chk = await ac.Sendserverkey(tb_activationkey.Text);



        //            if (chk <= 0)
        //            {
        //                string message = "inc(" + chk + ")";

        //                string messagecode = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(message));
        //                tb_activationkey.Text = messagecode;


        //                string msg = "The Activation not complete (Error code:" + messagecode + ")";


        //                Toaster.ShowWarning(Window.GetWindow(this), message: msg, animation: ToasterAnimation.FadeIn);
        //            }

        //            else
        //            {
        //                Toaster.ShowSuccess(Window.GetWindow(this), message: "The Activation done successfuly", animation: ToasterAnimation.FadeIn);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        Toaster.ShowWarning(Window.GetWindow(this), message: "The server Not Found", animation: ToasterAnimation.FadeIn);
        //    }
        //}
        // extend buttonend



    }
}
