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

    public class ProgramDetails
    {
        public int id { get; set; }
        public string programName { get; set; }
        public int branchCount { get; set; }
        public int posCount { get; set; }
        public int userCount { get; set; }
        public int vendorCount { get; set; }
        public int customerCount { get; set; }
        public int itemCount { get; set; }
        public int saleinvCount { get; set; }
        public Nullable<int> programIncId { get; set; }
        public Nullable<int> versionIncId { get; set; }
        public string versionName { get; set; }
        public int storeCount { get; set; }
        public string packageSaleCode { get; set; }
        public string customerServerCode { get; set; }
        public Nullable<System.DateTime> expireDate { get; set; }
        public Nullable<bool> isOnlineServer { get; set; }
        public string packageNumber { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<bool> isLimitDate { get; set; }
        public Nullable<bool> isLimitCount { get; set; }
        public bool isActive { get; set; }
        public string packageName { get; set; }

        // current info

        public int branchCountNow { get; set; }
        public int posCountNow { get; set; }
        public int userCountNow { get; set; }
        public int vendorCountNow { get; set; }
        public int customerCountNow { get; set; }
        public int itemCountNow { get; set; }
        public int saleinvCountNow { get; set; }

        public int storeCountNow { get; set; }

        public Nullable<System.DateTime> serverDateNow { get; set; }

        public string customerName { get; set; }
        public string customerLastName { get; set; }
        public string agentName { get; set; }
        public string agentLastName { get; set; }
        public string agentAccountName { get; set; }

        public Nullable<System.DateTime> pocrDate { get; set; }
        public Nullable<int> poId { get; set; }
        public string notes { get; set; }
        public string upnum { get; set; }
        public string packuserType { get; set; }


        public async Task<ProgramDetails> getCurrentInfo()
        {
            ProgramDetails item = new ProgramDetails();
            IEnumerable<Claim> claims = await APIResult.getList("ProgramDetails/getCurrentInfo");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<ProgramDetails>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }

        public async Task<int> updateIsonline(bool isOnlineServer)
        {
            int item = 0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("isOnlineServer", isOnlineServer.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ProgramDetails/updateIsonline", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = int.Parse(c.Value);

                }
            }
            return item;
        }
    }
}
