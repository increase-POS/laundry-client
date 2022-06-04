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

    public class BranchesUserstable
    {
        public int branchsUsersId { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
         
    }

    public class BranchesUsers
    {
        public int branchsUsersId { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }




        // branch
        public int bbranchId { get; set; }
        public string bcode { get; set; }
        public string bname { get; set; }
        public string baddress { get; set; }
        public string bemail { get; set; }
        public string bphone { get; set; }
        public string bmobile { get; set; }
        public Nullable<System.DateTime> bcreateDate { get; set; }
        public Nullable<System.DateTime> bupdateDate { get; set; }
        public Nullable<int> bcreateUserId { get; set; }
        public Nullable<int> bupdateUserId { get; set; }
        public string bnotes { get; set; }
        public Nullable<int> bparentId { get; set; }
        public byte bisActive { get; set; }
        public string btype { get; set; }

        // user
        public int uuserId { get; set; }
        public string uusername { get; set; }
        public string upassword { get; set; }
        public string uname { get; set; }
        public string ulastname { get; set; }
        public string ujob { get; set; }
        public string uworkHours { get; set; }
        public DateTime? ucreateDate { get; set; }
        public DateTime? uupdateDate { get; set; }
        public int? ucreateUserId { get; set; }
        public int? uupdateUserId { get; set; }
        public string uphone { get; set; }
        public string umobile { get; set; }
        public string uemail { get; set; }
        public string unotes { get; set; }
        public string uaddress { get; set; }
        public byte uisActive { get; set; }
        public byte uisOnline { get; set; }
        public Boolean ucanDelete { get; set; }
        public string uimage { get; set; }
      
       
        public async Task<List<BranchesUsers>> GetBranchesByUserId(int userId)
        {
            List<BranchesUsers> items = new List<BranchesUsers>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", userId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("BranchesUsers/GetBranchesByUserId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add( JsonConvert.DeserializeObject<BranchesUsers>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                   
                }
            }
            return items;
        }
        
        public async Task<int> UpdateBranchByUserId(List<BranchesUserstable> newList, int userId, int updateUserId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "BranchesUsers/UpdateBranchByUserId";
            var newListParameter = JsonConvert.SerializeObject(newList);
            parameters.Add("newList", newListParameter);
            parameters.Add("userId", userId.ToString());
            parameters.Add("updateUserId", updateUserId.ToString());
           return await APIResult.post(method, parameters);
        }

    }
}

