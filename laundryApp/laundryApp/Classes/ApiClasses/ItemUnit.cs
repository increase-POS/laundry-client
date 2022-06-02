using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
using laundryApp.ApiClasses;

namespace laundryApp.Classes
{
    public class ItemUnit
    {
        public int itemUnitId { get; set; }
        public Nullable<int> itemId { get; set; }
        public Nullable<int> unitId { get; set; }
        public Nullable<int> unitValue { get; set; }
        public short defaultSale { get; set; }
        public short defaultPurchase { get; set; }
        public decimal price { get; set; }
        public decimal priceWithService { get; set; }
        public string barcode { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> subUnitId { get; set; }
        public decimal purchasePrice { get; set; }
        public Nullable<int> storageCostId { get; set; }
        public byte isActive { get; set; }


        public string mainUnit { get; set; }
        public string smallUnit { get; set; }
        public string countSmallUnit { get; set; }

        public string itemName { get; set; }
        public string itemCode { get; set; }
        public string unitName { get; set; }

        public Boolean canDelete { get; set; }

        public Nullable<decimal> taxes { get; set; }
        public Nullable<decimal> priceTax { get; set; }

        public string type { get; set; }
        public Nullable<int> categoryId { get; set; }
        //**************************************************
        //*************** item unit methods *********************
        public async Task<List<ItemUnit>> GetAllItemUnits(int itemId)
        {


            List<ItemUnit> list = new List<ItemUnit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnits/GetAll", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
            //List<ItemUnit> items = null;
            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //    client.BaseAddress = new Uri(Global.APIUri);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsUnits/GetAll?itemId=" + itemId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace("\\", string.Empty);
            //        jsonString = jsonString.Trim('"');
            //        // fix date format
            //        JsonSerializerSettings settings = new JsonSerializerSettings
            //        {
            //            Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //            DateParseHandling = DateParseHandling.None
            //        };
            //        items = JsonConvert.DeserializeObject<List<ItemUnit>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return items;
            //    }
            //    else //web api sent error response 
            //    {
            //        items = new List<ItemUnit>();
            //    }
            //    return items;
            //}
        }


        public async Task<ItemUnit> GetById(int itemUnitId)
        {
            ItemUnit item = new ItemUnit();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemUnitId", itemUnitId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnits/GetById", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    item =JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
                }
            }
            return item;

            //ItemUnit items = null;
            //... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //    client.BaseAddress = new Uri(Global.APIUri);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsUnits/GetById?itemUnitId=" + itemUnitId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace("\\", string.Empty);
            //        jsonString = jsonString.Trim('"');
            //       // fix date format
            //       JsonSerializerSettings settings = new JsonSerializerSettings
            //       {
            //           Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //           DateParseHandling = DateParseHandling.None
            //       };
            //        items = JsonConvert.DeserializeObject<ItemUnit>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return items;
            //    }
            //    else //web api sent error response 
            //    {
            //        items = new ItemUnit();
            //    }
            //    return items;
            //}
        }
        public async Task<List<ItemUnit>> GetItemUnits(int itemId = 0)
        {

            List<ItemUnit> list = new List<ItemUnit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnits/Get",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;         
        }

        public List<ItemUnit> GetIUbyItem(int itemId, List<ItemUnit>AllIU,List<Unit>AllUnits)
        {

            if (AllIU is null)
                FillCombo.RefreshItemUnit();
            if (AllUnits is null)
                FillCombo.RefreshUnit();

            //AllIU = FillCombo.itemUnitList;
            //AllUnits = FillCombo.unitsList;

            List<ItemUnit> itemUnitsList = new List<ItemUnit>();
            try
            {
                itemUnitsList = ( from IU in AllIU
                            where (IU.itemId == itemId && IU.isActive == 1)
                            join U in AllUnits on IU.unitId equals U.unitId into lj

                            from v in lj.DefaultIfEmpty()
                            join u1 in AllUnits on IU.subUnitId equals u1.unitId into tj
                            from v1 in tj.DefaultIfEmpty()
                            select new ItemUnit()
                            {
                                itemUnitId = IU.itemUnitId,
                                unitId = IU.unitId,
                                mainUnit = v.name,
                                createDate = IU.createDate,
                                createUserId = IU.createUserId,
                                defaultPurchase = IU.defaultPurchase,
                                defaultSale = IU.defaultSale,
                                price = IU.price,
                                subUnitId = IU.subUnitId,
                                smallUnit = v1.name,
                                unitValue = IU.unitValue,
                                barcode = IU.barcode,
                                updateDate = IU.updateDate,
                                updateUserId = IU.updateUserId,
                                storageCostId = IU.storageCostId,

                            }).ToList();

                    return itemUnitsList;
              
            }
            catch
            {
                return itemUnitsList;
            }
        }


        //***************************************
        // get all barcodes from DB , return list of string represent barcodes
        //***************************************
        public async Task<List<ItemUnit>> getAllBarcodes()
        {
            List<ItemUnit> list = new List<ItemUnit>();
            //  Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mainBranchId", mainBranchId.ToString());
            //parameters.Add("userId", userId.ToString());
            //parameters.Add("date", date.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnits/GetAllBarcodes");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemUnit> barcodes = null;
            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //    client.BaseAddress = new Uri(Global.APIUri);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsUnits/GetAllBarcodes");
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace("\\", string.Empty);
            //        jsonString = jsonString.Trim('"');
            //        // fix date format
            //        JsonSerializerSettings settings = new JsonSerializerSettings
            //        {
            //            Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //            DateParseHandling = DateParseHandling.None
            //        };
            //        barcodes = JsonConvert.DeserializeObject<List<ItemUnit>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return barcodes;
            //    }
            //    else //web api sent error response 
            //    {
            //        barcodes = new List<ItemUnit>();
            //    }
            //    return barcodes;
            //}
        }
        //***************************************
        // add or update item unit
        //***************************************
        public async Task<int> saveItemUnit(ItemUnit itemUnit)
        {
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            string method = "ItemsUnits/Save";

            var myContent = JsonConvert.SerializeObject(itemUnit);
            parameters.Add("Object", myContent);
           return await APIResult.post(method, parameters);    
        }
        //***************************************
        // delete item unit (barcode)
        //***************************************
        public async Task<int> Delete(int ItemUnitId, int userId, bool final)
        {

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("ItemUnitId", ItemUnitId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("final", final.ToString());
            string method = "ItemsUnits/Delete";
           return await APIResult.post(method, parameters);

            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //    client.BaseAddress = new Uri(Global.APIUri);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsUnits/Delete?ItemUnitId=" + ItemUnitId+ "&userId="+userId+"&final="+isFinal);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Post;
            //    //set content type
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    var response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        return true;
            //    }
            //    return false;
            //}
        }

        public async Task<List<ItemUnit>> Getall()
        {
            List<ItemUnit> list = new List<ItemUnit>();
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnits/GetallItemsUnits");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemUnit> items = null;
            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //    client.BaseAddress = new Uri(Global.APIUri);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    request.RequestUri = new Uri(Global.APIUri + "ItemsUnits/Getall");
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();
            //        jsonString = jsonString.Replace("\\", string.Empty);
            //        jsonString = jsonString.Trim('"');
            //        // fix date format
            //        JsonSerializerSettings settings = new JsonSerializerSettings
            //        {
            //            Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //            DateParseHandling = DateParseHandling.None
            //        };
            //        items = JsonConvert.DeserializeObject<List<ItemUnit>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return items;
            //    }
            //    else //web api sent error response 
            //    {
            //        items = new List<ItemUnit>();
            //    }
            //    return items;
            //}
        }
       
        public async Task<List<Item>> GetForSale()
        {
            List<Item> list = new List<Item>();
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnits/GetForSale");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<Item>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list; 
        }

        public async Task<List<ItemUnit>> GetIU()
        {
            List<ItemUnit> list = new List<ItemUnit>();
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnits/GetIU");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
        }

        public async Task<List<ItemUnit>> GetActiveItemsUnits()
        {
            List<ItemUnit> list = new List<ItemUnit>();
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnits/GetActiveItemsUnits");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;   
        }
        public async Task<List<ItemUnit>> GetUnitsForSales(int branchId)
        {
            List<ItemUnit> list = new List<ItemUnit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("branchId", branchId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("ItemsUnits/GetUnitsForSales",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
        }
            //***************************************

            //public async Task<List<ItemUnit>> GetbyOfferId(int offerId)
            //{


            //    List<ItemUnit> items = null;
            //    // ... Use HttpClient.
            //    ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //    using (var client = new HttpClient())
            //    {
            //        ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //        client.BaseAddress = new Uri(Global.APIUri);
            //        client.DefaultRequestHeaders.Clear();
            //        client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //        client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //        HttpRequestMessage request = new HttpRequestMessage();
            //        request.RequestUri = new Uri(Global.APIUri + "ItemsUnits/GetbyOfferId");
            //        request.Headers.Add("APIKey", Global.APIKey);
            //        request.Headers.Add("offerId", offerId.ToString());
            //        request.Method = HttpMethod.Get;
            //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //        HttpResponseMessage response = await client.SendAsync(request);

            //        if (response.IsSuccessStatusCode)
            //        {
            //            var jsonString = await response.Content.ReadAsStringAsync();
            //            jsonString = jsonString.Replace("\\", string.Empty);
            //            jsonString = jsonString.Trim('"');
            //            // fix date format
            //            JsonSerializerSettings settings = new JsonSerializerSettings
            //            {
            //                Converters = new List<JsonConverter> { new BadDateFixingConverter() },
            //                DateParseHandling = DateParseHandling.None
            //            };
            //            items = JsonConvert.DeserializeObject<List<ItemUnit>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //            return items;
            //        }
            //        else //web api sent error response 
            //        {
            //            items = new List<ItemUnit>();
            //        }
            //        return items;
            //    }
            //}


            //*******************************
            public async Task<List<ItemUnit>> getSmallItemUnits(int itemId, int itemUnitId)
        {

            List<ItemUnit> list = new List<ItemUnit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("itemId", itemId.ToString());
            parameters.Add("itemUnitId", itemUnitId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("itemsUnits/getSmallItemUnits", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
            //List<ItemUnit> units = null;
            //// ... Use HttpClient.
            //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
            //using (var client = new HttpClient())
            //{
            //    ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //    client.BaseAddress = new Uri(Global.APIUri);
            //    client.DefaultRequestHeaders.Clear();
            //    client.DefaultRequestHeaders.Add("Connection", "Keep-Alive");
            //    client.DefaultRequestHeaders.Add("Keep-Alive", "3600");
            //    HttpRequestMessage request = new HttpRequestMessage();
            //    request.RequestUri = new Uri(Global.APIUri + "itemsUnits/getSmallItemUnits?itemId=" + itemId + "&itemUnitId=" + itemUnitId);
            //    request.Headers.Add("APIKey", Global.APIKey);
            //    request.Method = HttpMethod.Get;
            //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //    HttpResponseMessage response = await client.SendAsync(request);

            //    if (response.IsSuccessStatusCode)
            //    {
            //        var jsonString = await response.Content.ReadAsStringAsync();

            //        units = JsonConvert.DeserializeObject<List<ItemUnit>>(jsonString);

            //        return units;
            //    }
            //    else //web api sent error response 
            //    {
            //        units = new List<ItemUnit>();
            //    }
            //    return units;
            //}

        }
        public async Task<int> largeToSmallUnitQuan(int fromItemUnit, int toItemUnit)
        {
            int AvailableAmount = 0;

           
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("fromItemUnit", fromItemUnit.ToString());
            parameters.Add("toItemUnit", toItemUnit.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("itemsUnits/largeToSmallUnitQuan", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    AvailableAmount = int.Parse(c.Value);
                }
            }
            return AvailableAmount;

        }
        public async Task<int> smallToLargeUnit(int fromItemUnit, int toItemUnit)
        {
            int AvailableAmount = 0;

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("fromItemUnit", fromItemUnit.ToString());
            parameters.Add("toItemUnit", toItemUnit.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("itemsUnits/smallToLargeUnitQuan",parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    AvailableAmount = int.Parse(c.Value);
                }
            }
            return AvailableAmount;

        }
        public async Task<int> fromUnitToUnitQuantity(int quantity, int itemId, int fromItemUnitId, int toItemUnitId)
        {
            int remain = 0;
            int _ConversionQuantity;
            int _ToQuantity = 0;

            if (quantity != 0)
            {
                List<ItemUnit> smallUnits = await getSmallItemUnits(itemId, fromItemUnitId);

                var isSmall = smallUnits.Find(x => x.itemUnitId == toItemUnitId);
                if (isSmall != null) // from-unit is bigger than to-unit
                {
                    _ConversionQuantity = await largeToSmallUnitQuan(fromItemUnitId, toItemUnitId);
                    _ToQuantity = quantity * _ConversionQuantity;

                }
                else
                {
                    _ConversionQuantity = await smallToLargeUnit(fromItemUnitId, toItemUnitId);

                    if (_ConversionQuantity != 0)
                    {
                        _ToQuantity = quantity / _ConversionQuantity;
                        remain = quantity - (_ToQuantity * _ConversionQuantity); // get remain quantity which cannot be changeed
                    }
                }
            }

            return _ToQuantity;
        }
    }
}
