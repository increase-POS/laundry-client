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
    public class InventoryItemLocation
    {
        public int sequence { get; set; }
        public int id { get; set; }
        public Nullable<bool> isDestroyed { get; set; }
        public Nullable<bool> isFalls { get; set; }
        public Nullable<int> amount { get; set; }
        public Nullable<int> amountDestroyed { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<int> itemLocationId { get; set; }
        public Nullable<int> inventoryId { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<byte> isActive { get; set; }
        public string notes { get; set; }
        public Boolean canDelete { get; set; }
        public string itemName { get; set; }
        public string location { get; set; }
        public string section { get; set; }
        public string unitName { get; set; }
        public int itemId { get; set; }
        public int itemUnitId { get; set; }
        public int unitId { get; set; }
        public string inventoryNum { get; set; }
        public Nullable<System.DateTime> inventoryDate { get; set; }
        public string itemType { get; set; }
        public string cause { get; set; }
        public string fallCause { get; set; }
        public Nullable<decimal> avgPurchasePrice { get; set; }

        public async Task<List<InventoryItemLocation>> GetAll(int itemId)
        {
            List<InventoryItemLocation> items = new List<InventoryItemLocation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("InventoryItemLocation/Get", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<InventoryItemLocation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
       
        public async Task<List<InventoryItemLocation>> GetItemToDestroy(int branchId)
        {
            List<InventoryItemLocation> items = new List<InventoryItemLocation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", branchId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("InventoryItemLocation/GetItemToDestroy", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<InventoryItemLocation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<InventoryItemLocation>> GetShortageItem(int branchId)
        {
            List<InventoryItemLocation> items = new List<InventoryItemLocation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", branchId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("InventoryItemLocation/GetShortageItem", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<InventoryItemLocation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<int> save(List<InventoryItemLocation> newObject, int inventoryId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "InventoryItemLocation/Save";
            var myContent = JsonConvert.SerializeObject(newObject);
            parameters.Add("itemObject", myContent);
            parameters.Add("inventoryId", inventoryId.ToString());

            return await APIResult.post(method, parameters);
        }
        public async Task<int> distroyItem(InventoryItemLocation item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "InventoryItemLocation/distroyItem";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }
        public async Task<int> fallItem(InventoryItemLocation item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "InventoryItemLocation/fallItem";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }
       
        public async Task ShortageRecordCash(Invoice invoice, int userId)
        {
            User user = new User();
            decimal newBalance = 0;
            //user = await user.getUserById(userId);
            user = FillCombo.usersList.Where(x => x.userId == userId).FirstOrDefault();
            CashTransfer cashTrasnfer = new CashTransfer();
            cashTrasnfer.posId = MainWindow.posLogin.posId;
            cashTrasnfer.userId = userId;
            cashTrasnfer.invId = invoice.invoiceId;
            cashTrasnfer.createUserId = invoice.createUserId;
            cashTrasnfer.processType = "balance";
            cashTrasnfer.side = "u"; // user
            cashTrasnfer.transType = "d"; //deposit
            cashTrasnfer.transNum = await cashTrasnfer.generateCashNumber("du");

            if (user.balanceType == 0)
            {
                if (invoice.totalNet <= (decimal)user.balance)
                {
                    invoice.paid = invoice.totalNet;
                    invoice.deserved = 0;
                    newBalance = user.balance - (decimal)invoice.totalNet;
                    user.balance = newBalance;
                }
                else
                {
                    invoice.paid = (decimal)user.balance;
                    invoice.deserved = invoice.totalNet - (decimal)user.balance;
                    newBalance = (decimal)invoice.totalNet - user.balance;
                    user.balance = newBalance;
                    user.balanceType = 1;
                }

                cashTrasnfer.cash = invoice.paid;

                await invoice.saveInvoice(invoice);
                await cashTrasnfer.Save(cashTrasnfer); //add cash transfer
                await user.save(user);
            }
            else if (user.balanceType == 1)
            {
                newBalance = user.balance + (decimal)invoice.totalNet;
                user.balance = newBalance;
                await user.save(user);
            }
            await FillCombo.RefreshUsers();
        }
    }
}

