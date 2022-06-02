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
    public class TablesReservation
    {
        public long reservationId { get; set; }
        public string code { get; set; }
        public Nullable<int> customerId { get; set; }
        public string customerName { get; set; }

        public Nullable<int> branchId { get; set; }
        public Nullable<System.DateTime> reservationDate { get; set; }
        public Nullable<System.DateTime> reservationTime { get; set; }
        public Nullable<System.DateTime> endTime { get; set; }
        public Nullable<int> personsCount { get; set; }
        public string status { get; set; }
        public Nullable<int> tableId { get; set; }
        public string notes { get; set; }
        public byte isActive { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }


        public string isExceed { get; set; }
        public List<Tables> tables { get; set; }

        /////////////////////////////////
        internal async Task<int> addReservation(TablesReservation reservation, List<Tables> tables)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Tables/addReservation";
            var myContent = JsonConvert.SerializeObject(reservation);
            parameters.Add("itemObject", myContent);

            myContent = JsonConvert.SerializeObject(tables);
            parameters.Add("tables", myContent);
            return await APIResult.post(method, parameters);
        }
        internal async Task<int> updateReservation(TablesReservation reservation, List<Tables> tables)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Tables/updateReservation";
            var myContent = JsonConvert.SerializeObject(reservation);
            parameters.Add("itemObject", myContent);

            myContent = JsonConvert.SerializeObject(tables);
            parameters.Add("tables", myContent);
            return await APIResult.post(method, parameters);
        }
        internal async Task<int> updateReservationStatus(long reservationId, string status, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Tables/updateReservationStatus";
            parameters.Add("reservationId", reservationId.ToString());
            parameters.Add("status", status);
            parameters.Add("userId", userId.ToString());

            return await APIResult.post(method, parameters);
        }
        internal async Task<IEnumerable<TablesReservation>> Get(int branchId = 0)
        {
            List<TablesReservation> items = new List<TablesReservation>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("Tables/GetReservations", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<TablesReservation>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<string> generateReserveCode(string reservCode, string branchCode, int branchId)
        {
            int sequence = await GetLastNumOfReserv(reservCode, branchId);
            sequence++;
            string strSeq = sequence.ToString();
            if (sequence <= 999999)
                strSeq = sequence.ToString().PadLeft(6, '0');
            string invoiceNum = reservCode + "-" + branchCode + "-" + strSeq;
            return invoiceNum;
        }
        public async Task<int> GetLastNumOfReserv(string reservCode, int branchId)
        {
            int count = 0;
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("reservCode", reservCode);
            parameters.Add("branchId", branchId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Tables/GetLastNumOfReserv", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    count = int.Parse(c.Value);
                    break;
                }
            }
            return count;
        }
    }
}
