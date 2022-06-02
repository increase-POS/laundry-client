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
    public class Location
    {
        public int locationId { get; set; }
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public byte isActive { get; set; }
        public Nullable<int> sectionId { get; set; }
        public string notes { get; set; }
        public Nullable<int> branchId { get; set; }
        public byte isFreeZone { get; set; }
        public byte isKitchen { get; set; }



        public Boolean canDelete { get; set; }

        public string sectionName { get; set; }
        public string branchName { get; set; }



        public string name
        {
            get
            {
                return $"{x}{y}{z}";
            }
            set
            {
                name = value;
            }
        }
       

        public async Task<List<Location>> Get()
        {
            List<Location> items = new List<Location>();
            IEnumerable<Claim> claims = await APIResult.getList("Locations/Get");
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Location>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<Location> getById(int itemId)
        {
            Location item = new Location();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Locations/GetLocationByID", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item = JsonConvert.DeserializeObject<Location>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                    break;
                }
            }
            return item;
        }
        public async Task<List<Location>> GetLocsByBranchId(int itemId)
        {
            List<Location> items = new List<Location>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Locations/GetLocsByBranchId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Location>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        public async Task<List<Location>> getLocsBySectionId(int itemId)
        {
            List<Location> items = new List<Location>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Locations/GetLocsBySectionId", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<Location>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        public async Task<int> save(Location item)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Locations/Save";
            var myContent = JsonConvert.SerializeObject(item);
            parameters.Add("itemObject", myContent);
           return await APIResult.post(method, parameters);
        }

        public async Task<int> saveLocationsSection(List<Location> locations, int sectionId, int userId)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "Locations/AddLocationsToSection";
            var myContent = JsonConvert.SerializeObject(locations);
            parameters.Add("itemObject", myContent);
            parameters.Add("sectionId", sectionId.ToString());
            parameters.Add("userId", userId.ToString());
           return await APIResult.post(method, parameters);
        }
        public async Task<int> delete(int locationId, int userId, Boolean final)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", locationId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "Locations/Delete";
           return await APIResult.post(method, parameters);
        }

    }
}
