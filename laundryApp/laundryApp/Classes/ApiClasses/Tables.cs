using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace laundryApp.Classes.ApiClasses
{
    public class Tables
    {
        public int tableId { get; set; }
        public string name { get; set; }
        public int personsCount { get; set; }
        public Nullable<int> sectionId { get; set; }
        public Nullable<int> branchId { get; set; }
        public string notes { get; set; }
        public string status { get; set; }
        public byte isActive { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }

        public Boolean canDelete { get; set; }
        public string sectionName { get; set; }
        public string branchName { get; set; }

        internal async Task<int> save(Tables table)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Tables/Save";
            var myContent = JsonConvert.SerializeObject(table);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }

        internal async Task<int> delete(int tableId, int userId, bool final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("tableId", tableId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "Tables/Delete";
            return await APIResult.post(method, parameters);
        }

        internal async Task<IEnumerable<Tables>> Get(int branchId = 0, int sectionId = 0)
        {
            List<Tables> items = new List<Tables>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("sectionId", sectionId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("Tables/GetAll", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Tables>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        internal async Task<Invoice> GetTableInvoice(int tableId)
        {
            Invoice items = new Invoice();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("tableId", tableId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("Tables/GetTableInvoice", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items = JsonConvert.DeserializeObject<Invoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
            return items;
        }
        internal async Task<List<Tables>> getInvoiceTables(int invoiceId)
        {
            List<Tables> items = new List<Tables>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("invoiceId", invoiceId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("Tables/getInvoiceTables", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items = JsonConvert.DeserializeObject<List<Tables>>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
            return items;
        }

        internal async Task<List<Tables>> GetTablesStatusInfo(int branchId, string dateSearch, string startTimeSearch, string endTimeSearch)
        {
            List<Tables> items = new List<Tables>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("dateSearch", dateSearch);
            parameters.Add("startTimeSearch", startTimeSearch);
            parameters.Add("endTimeSearch", endTimeSearch);
            IEnumerable<Claim> claims = await APIResult.getList("Tables/GetTablesStatusInfo", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Tables>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        internal async Task<List<Tables>> GetTablesForDinning(int branchId, string dateSearch)
        {
            List<Tables> items = new List<Tables>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("dateSearch", dateSearch);
            //parameters.Add("startTimeSearch", startTimeSearch);

            IEnumerable<Claim> claims = await APIResult.getList("Tables/GetTablesForDinning", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Tables>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        internal async Task<List<TablesStatistics>> GetTablesStatistics(string dateSearch, int mainBranchId, int userId)
        {
            List<TablesStatistics> items = new List<TablesStatistics>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("dateSearch", dateSearch);
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("Tables/GetTablesStatistics", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<TablesStatistics>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));

                }
            }
            return items;
        }

        public async Task<int> AddTablesToSection(List<Tables> tablesList, int sectionId, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Tables/AddTablesToSection";
            var myContent = JsonConvert.SerializeObject(tablesList);
            parameters.Add("itemObject", myContent);
            parameters.Add("sectionId", sectionId.ToString());
            parameters.Add("userId", userId.ToString());
            return await APIResult.post(method, parameters);
        }

        public async Task<int> checkTableAvailabiltiy(int tableId, int branchId, string reservationDate, string startTime, string endTime, long reservationId = 0, int invoiceId = 0)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Tables/checkTableAvailabiltiy";
            parameters.Add("tableId", tableId.ToString());
            parameters.Add("branchId", branchId.ToString());
            parameters.Add("reservationId", reservationId.ToString());
            parameters.Add("invoiceId", invoiceId.ToString());
            parameters.Add("reservationDate", reservationDate);
            parameters.Add("startTime", startTime);
            parameters.Add("endTime", endTime);
            return await APIResult.post(method, parameters);
        }
        public async Task<int> checkOpenedTable(int tableId, int branchId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Tables/checkOpenedTable";
            parameters.Add("tableId", tableId.ToString());
            parameters.Add("branchId", branchId.ToString());
            return await APIResult.post(method, parameters);
        }
    }
}
