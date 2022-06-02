using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace laundryApp.Classes
{
    public class BillDetailsSales
    {
        public int index { get; set; }
        public string image { get; set; }
        public int itemId { get; set; }
        public int itemUnitId { get; set; }
        public string itemName { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal basicPrice { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public int? offerId { get; set; }
        public decimal OfferValue { get; set; }
        public string OfferType { get; set; }
        public string forAgents { get; set; }

    }


    public class BillDetailsPurchase
    {
        public int ID { get; set; }
        public int itemId { get; set; }
        public int itemUnitId { get; set; }
        public string Product { get; set; }
        public string Unit { get; set; }
        public string UnitName { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public int OrderId { get; set; }
        
    }
}
