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
    public class Inventory
    {
        public int inventoryId { get; set; }
        public string num { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public byte isActive { get; set; }
        public string notes { get; set; }
        public string inventoryType { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<int> posId { get; set; }
        public Nullable<int> mainInventoryId { get; set; }



        public Boolean canDelete { get; set; }
       
        //*******************************************************
       
        public async Task<Inventory> getByBranch(string inventoryType, int branchId)
        {
            Inventory item = new Inventory();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("inventoryType", inventoryType.ToString());
            parameters.Add("branchId", branchId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Inventory/getByBranch", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Inventory>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            if (item == null)
                item = new Inventory();
            return item;
        }
        public async Task<int> GetLastNumOfInv(string invCode, int branchId)
        {
            int LastNumOfInv = 0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("invCode", invCode.ToString());
            parameters.Add("branchId", branchId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Inventory/GetLastNumOfInv", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    LastNumOfInv =int.Parse( c.Value);
                    break;
                }
            }
            return LastNumOfInv;
        }
        public async Task<bool> shortageIsManipulated(int inventoryId)
        {
            bool IsManipulated = false;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", inventoryId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Inventory/shortageIsManipulated", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    IsManipulated = bool.Parse( c.Value);
                    break;
                }
            }
            return IsManipulated;
        }
        public async Task<int> save(Inventory item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Inventory/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }
       
        public async Task<int> delete(int itemId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "Inventory/Delete";
           return await APIResult.post(method, parameters);
        }
        public async Task<string> generateInvNumber(string invCode, int branchId)
        {         
            int sequence = await GetLastNumOfInv(invCode, branchId); // in : inventory
            sequence++;
            string strSeq = sequence.ToString();
            if (sequence <= 999999)
                strSeq = sequence.ToString().PadLeft(6, '0');
            string inventoryNum = invCode + "-" + strSeq;
            return inventoryNum;
        }

        // get is exist


    }
}

