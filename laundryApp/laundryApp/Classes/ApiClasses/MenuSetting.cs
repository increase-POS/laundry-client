using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp.ApiClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace laundryApp.Classes
{
   public class MenuSetting
    {
        #region basic attributes
        public Nullable<long> menuSettingId { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public Nullable<int> branchId { get; set; }
        public bool sat { get; set; }
        public bool sun { get; set; }
        public bool mon { get; set; }
        public bool tues { get; set; }
        public bool wed { get; set; }
        public bool thur { get; set; }
        public bool fri { get; set; }
        public Nullable<decimal> preparingTime { get; set; }
        public Nullable<byte> isActive { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> createUserId { get; set; }
        #endregion

        #region item attribute
        public int itemId { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string details { get; set; }
        public Nullable<int> categoryId { get; set; }
        public Nullable<int> tagId { get; set; }
        public string image { get; set; }
        public string type { get; set; }
        public decimal price { get; set; }
        public decimal priceWithService { get; set; }
        public Nullable<int> isNew { get; set; }
        #endregion

        #region methods
        public async Task<int> saveItemMenuSetting(MenuSetting item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "MenuSettings/Save";

            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
            return await APIResult.post(method, parameters);
        }
        public async Task<List<MenuSetting>> Get(int branchId)
        {
            List<MenuSetting> items = new List<MenuSetting>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());

            IEnumerable<Claim> claims = await APIResult.getList("items/GetItemsMenuSetting",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<MenuSetting>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        #endregion
    }
}
