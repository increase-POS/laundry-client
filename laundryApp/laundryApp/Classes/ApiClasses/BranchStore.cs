using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp;
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

    class BranchStoretable
    {
        public int id { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<int> storeId { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public int isActive { get; set; }
        public Boolean canDelete { get; set; } 


    }
    class BranchStore
    {
        public int id { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<int> storeId { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public int isActive { get; set; }


        public Boolean canDelete { get; set; }

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

        // store
        public int sbranchId { get; set; }
        public string scode { get; set; }
        public string sname { get; set; }
        public string saddress { get; set; }
        public string semail { get; set; }
        public string sphone { get; set; }
        public string smobile { get; set; }
        public Nullable<System.DateTime> screateDate { get; set; }
        public Nullable<System.DateTime> supdateDate { get; set; }
        public Nullable<int> screateUserId { get; set; }
        public Nullable<int> supdateUserId { get; set; }
        public string snotes { get; set; }
        public Nullable<int> sparentId { get; set; }
        public byte sisActive { get; set; }
        public string stype { get; set; }
         
       
         

        public async Task<List<BranchStore>> GetAll()
        {
            List<BranchStore> items = new List<BranchStore>();
            IEnumerable<Claim> claims = await APIResult.getList("BranchStore/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<BranchStore>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
       
           
        // get is exist
        public async Task<int> UpdateStoresById(List<BranchStoretable> newList, int branchId, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "BranchStore/UpdateStoresById";
            var newListParameter = JsonConvert.SerializeObject(newList);
            parameters.Add("newList", newListParameter);
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("userId", userId.ToString());
            return await APIResult.post(method, parameters);
        }
       

    }
}


