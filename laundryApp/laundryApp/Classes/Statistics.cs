using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using laundryApp;
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
using laundryApp.Classes.ApiClasses;

namespace laundryApp.Classes
{
    public class OpenClosOperatinModel
    {
        public int cashTransId { get; set; }
        public string transType { get; set; }
        public Nullable<int> posId { get; set; }
        public Nullable<int> userId { get; set; }
        public Nullable<int> agentId { get; set; }
        public Nullable<int> invId { get; set; }
        public string transNum { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<decimal> cash { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> createUserId { get; set; }
        public string notes { get; set; }
        public Nullable<int> posIdCreator { get; set; }
        public Nullable<byte> isConfirm { get; set; }
        public Nullable<int> cashTransIdSource { get; set; }
        public string side { get; set; }
        public string opSideNum { get; set; }
        public string docName { get; set; }
        public string docNum { get; set; }
        public string docImage { get; set; }
        public Nullable<int> bankId { get; set; }
        public string bankName { get; set; }
        public string agentName { get; set; }
        public string usersName { get; set; }
        public string usersLName { get; set; }
        public string posName { get; set; }
        public string posCreatorName { get; set; }
        public Nullable<byte> isConfirm2 { get; set; }
        public int cashTrans2Id { get; set; }
        public Nullable<int> pos2Id { get; set; }

        public string pos2Name { get; set; }
        public string processType { get; set; }
        public Nullable<int> cardId { get; set; }
        public Nullable<int> bondId { get; set; }
        public string createUserName { get; set; }
        public string updateUserName { get; set; }
        public string updateUserJob { get; set; }
        public string updateUserAcc { get; set; }
        public string createUserJob { get; set; }
        public string createUserLName { get; set; }
        public string updateUserLName { get; set; }
        public string cardName { get; set; }
        public Nullable<System.DateTime> bondDeserveDate { get; set; }
        public Nullable<byte> bondIsRecieved { get; set; }
        public string agentCompany { get; set; }
        public Nullable<int> shippingCompanyId { get; set; }
        public string shippingCompanyName { get; set; }
        public string userAcc { get; set; }

        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorname { get; set; }
        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }
        public Nullable<int> branch2Id { get; set; }
        public string branch2Name { get; set; }




    }
    public class POSOpenCloseModel
    {
        public int cashTransId { get; set; }
        public string transType { get; set; }
        public Nullable<int> posId { get; set; }

        public string transNum { get; set; }

        public Nullable<decimal> cash { get; set; }//close

        public string notes { get; set; }

        public Nullable<byte> isConfirm { get; set; }
        public Nullable<int> cashTransIdSource { get; set; }
        public string side { get; set; }

        public string posName { get; set; }



        public string processType { get; set; }


        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }

        public Nullable<System.DateTime> updateDate { get; set; }//close
        public Nullable<System.DateTime> openDate { get; set; }
        public Nullable<decimal> openCash { get; set; }
        public Nullable<int> openCashTransId { get; set; }



    }
    public class ItemTransferInvoiceTax
    {// new properties
        public Nullable<System.DateTime> updateDate { get; set; }




        public string agentCompany { get; set; }




        // ItemTransfer
        public int ITitemsTransId { get; set; }
        public Nullable<int> ITitemUnitId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> ITitemId { get; set; }
        public Nullable<int> ITunitId { get; set; }
        public string ITitemName { get; set; }
        public string ITunitName { get; set; }

        public Nullable<long> ITquantity { get; set; }
        public Nullable<decimal> ITprice { get; set; }




        public string ITnotes { get; set; }

        public string ITbarcode { get; set; }

        //invoice
        public int invoiceId { get; set; }
        public string invNumber { get; set; }
        public string invBarcode { get; set; }
        public Nullable<int> agentId { get; set; }

        public string invType { get; set; }
        public string discountType { get; set; }

        public Nullable<decimal> discountValue { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> totalNet { get; set; }
        public Nullable<decimal> paid { get; set; }
        public Nullable<decimal> deserved { get; set; }
        public Nullable<System.DateTime> deservedDate { get; set; }
        public Nullable<System.DateTime> invDate { get; set; }
        public Nullable<System.DateTime> IupdateDate { get; set; }
        public Nullable<int> IupdateUserId { get; set; }

        public string invCase { get; set; }

        public string Inotes { get; set; }
        public string vendorInvNum { get; set; }

        public string branchName { get; set; }
        public string posName { get; set; }
        public Nullable<System.DateTime> vendorInvDate { get; set; }
        public Nullable<int> branchId { get; set; }

        public Nullable<decimal> tax { get; set; }
        public Nullable<int> taxtype { get; set; }
        public Nullable<int> posId { get; set; }

        public string ITtype { get; set; }

        public string branchType { get; set; }

        public string posCode { get; set; }
        public string agentName { get; set; }

        public string agentType { get; set; }
        public string agentCode { get; set; }

        public string uuserName { get; set; }
        public string uuserLast { get; set; }
        public string uUserAccName { get; set; }
        public Nullable<decimal> itemUnitPrice { get; set; }

        public Nullable<decimal> totalwithTax { get; set; }
        public Nullable<decimal> totalNoTax { get; set; }
        public Nullable<decimal> subTotalTax { get; set; }
        public Nullable<decimal> subTotalNotax { get; set; }

        public Nullable<decimal> OneitemUnitTax { get; set; }
        public Nullable<decimal> itemUnitTaxwithQTY { get; set; }
        public Nullable<decimal> invTaxVal { get; set; }
        public Nullable<decimal> OneItemOfferVal { get; set; }
        public Nullable<decimal> OneItemPriceNoTax { get; set; }
        public Nullable<decimal> ItemTaxes { get; set; }
        public Nullable<decimal> OneItemPricewithTax { get; set; }

        public Nullable<int> itemsRowsCount { get; set; }
        // public Nullable<decimal> totalNet { get; set; }

    }
    public class ItemUnitInvoiceProfit
    {

        /////////////// الارباح
        public decimal shippingCost { get; set; }
        public decimal realShippingCost { get; set; }
        public decimal shippingProfit { get; set; }
        public decimal totalNoShip { get; set; }
        public decimal totalNetNoShip { get; set; }
        public string ITitemName { get; set; }
        public string ITunitName { get; set; }
        //public int ITitemsTransId { get; set; }*
        public Nullable<int> ITitemUnitId { get; set; }

        public Nullable<int> ITitemId { get; set; }
        public Nullable<int> ITunitId { get; set; }
        public Nullable<long> ITquantity { get; set; }

        //public Nullable<System.DateTime> ITupdateDate { get; set; }*
        //  public Nullable<int> IT.createUserId { get; set; } 
        //public Nullable<int> ITupdateUserId { get; set; }*

        public Nullable<decimal> ITprice { get; set; }
        //public string ITbarcode { get; set; }*

        //public string ITUpdateuserNam { get; set; }*
        //public string ITUpdateuserLNam { get; set; }*
        //public string ITUpdateuserAccNam { get; set; }*
        public int invoiceId { get; set; }
        public string invNumber { get; set; }
        public string invBarcode { get; set; }
        //public Nullable<int> agentId { get; set; }*
        public Nullable<int> posId { get; set; }
        public string invType { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> totalNet { get; set; }

        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<System.DateTime> invDate { get; set; }
        //public Nullable<int> updateUserId { get; set; }*
        //public Nullable<int> branchId { get; set; }*
        public Nullable<decimal> discountValue { get; set; }
        public string discountType { get; set; }
        public Nullable<decimal> tax { get; set; }
        // public string name { get; set; }
        //  isApproved { get; set; }


        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorName { get; set; }


        public string posName { get; set; }
        public string posCode { get; set; }
        //public string agentName { get; set; }*
        //public string agentCode { get; set; }*
        //public string agentType { get; set; }*

        //public string uuserName { get; set; }*
        //public string uuserLast { get; set; }*
        //public string uUserAccName { get; set; }*
        //public string agentCompany { get; set; }*
        public Nullable<decimal> subTotal { get; set; }
        public decimal purchasePrice { get; set; }
        public decimal totalwithTax { get; set; }
        public decimal subTotalNet { get; set; } // with invoice discount 
        public decimal itemunitProfit { get; set; }
        public decimal invoiceProfit { get; set; }
        public decimal itemProfit { get; set; }

    }
    public class BalanceSTS
    {

        public int posId { get; set; }
        public string posName { get; set; }
        public Nullable<byte> posIsActive { get; set; }
        public Nullable<decimal> balance { get; set; }
        public string posCode { get; set; }
        public int branchId { get; set; }
        public string branchName { get; set; }
        public string branchCode { get; set; }

        public string branchType { get; set; }
        public Nullable<byte> banchIsActive { get; set; }

    }
    public class CashTransferSts
    {
        public Nullable<int> invShippingCompanyId { get; set; }
        public Nullable<int> shipUserId { get; set; }
        public Nullable<int> invAgentId { get; set; }
        public Nullable<decimal> agentBalance { get; set; }
        public Nullable<byte> agentBType { get; set; }
        public Nullable<decimal> userBalance { get; set; }
        public Nullable<byte> userBType { get; set; }
        public Nullable<decimal> shippingBalance { get; set; }
        public Nullable<byte> shippingCompaniesBType { get; set; }
        private string description;
        private string description1;
        private string description3;
        private string bIsReceived;
        public string bondNumber { get; set; }
        public Nullable<int> fromposId { get; set; }
        public string fromposName { get; set; }
        public Nullable<int> frombranchId { get; set; }
        public string frombranchName { get; set; }
        public Nullable<int> toposId { get; set; }
        public string toposName { get; set; }
        public Nullable<int> tobranchId { get; set; }
        public string tobranchName { get; set; }

        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }
        public Nullable<int> branch2Id { get; set; }
        public string branch2Name { get; set; }
        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreator { get; set; }
        public int depositCount { get; set; }
        public decimal depositSum { get; set; }
        public int pullCount { get; set; }
        public decimal pullSum { get; set; }


        public Nullable<int> posId { get; set; }//
        public Nullable<int> userId { get; set; }
        public Nullable<int> agentId { get; set; }//

        public string transNum { get; set; }//
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }//

        public Nullable<int> updateUserId { get; set; }//
        public Nullable<int> createUserId { get; set; }
        public string notes { get; set; }
        public Nullable<int> posIdCreator { get; set; }
        public Nullable<byte> isConfirm { get; set; }
        public Nullable<int> cashTransIdSource { get; set; }

        public string opSideNum { get; set; }
        public string docName { get; set; }

        public string docImage { get; set; }

        public string bankName { get; set; }//
        public string agentName { get; set; }//
        public string usersName { get; set; }
        public string usersLName { get; set; }
        public string posName { get; set; }
        public string posCreatorName { get; set; }
        public Nullable<byte> isConfirm2 { get; set; }
        public int cashTrans2Id { get; set; }
        public Nullable<int> pos2Id { get; set; }

        public string pos2Name { get; set; }

        public int processTypeCount { get; set; }

        public decimal cardTotal { get; set; }
        public decimal docTotal { get; set; }
        public decimal chequeTotal { get; set; }
        public decimal balanceTotal { get; set; }
        public decimal invoiceTotal { get; set; }

        public string createUserName { get; set; }
        public string updateUserName { get; set; }//
        public string updateUserJob { get; set; }
        public string updateUserAcc { get; set; }
        public string createUserJob { get; set; }
        public string createUserLName { get; set; }
        public string cardName { get; set; }//
        public Nullable<System.DateTime> bondDeserveDate { get; set; }
        public Nullable<byte> bondIsRecieved { get; set; }
        public string agentCompany { get; set; }//

        public Nullable<int> shippingCompanyId { get; set; }//
        public string shippingCompanyName { get; set; }//
        public string invAgentName { get; set; }
        public string invShippingCompanyName { get; set; }
        public string userAcc { get; set; }


        //invoice



        public Nullable<decimal> deserved { get; set; }
        public Nullable<System.DateTime> deservedDate { get; set; }

        public int cashTransId { get; set; }
        public string transType { get; set; }//
        public string desc { get; set; }//
        public Nullable<int> invId { get; set; }//
        public Nullable<decimal> cash { get; set; }//
        public decimal cashTotal { get; set; }//
        public string side { get; set; }//
        public string docNum { get; set; }//
        public Nullable<int> bankId { get; set; }//
        public string processType { get; set; }//
        public string paymentreport { get; set; }//
        public Nullable<int> cardId { get; set; }//
        public Nullable<int> bondId { get; set; }//
        public string invNumber { get; set; }//
        public string invBarcode { get; set; }
        public string invType { get; set; }//
        public Nullable<decimal> totalNet { get; set; }//
        public string Description
        {
            get => processType == "cash" ? description = AppSettings.resourcemanager.GetString("trCash")//
                 : processType == "card" ? description = cardName + " " + AppSettings.resourcemanager.GetString("trNum:") + " : " + docNum
                 : processType == "doc" ? description = AppSettings.resourcemanager.GetString("trBond") + " " + AppSettings.resourcemanager.GetString("trNum:") + " : " + bondNumber
                 : processType == "cheque" ? description = AppSettings.resourcemanager.GetString("trCheque") + " " + AppSettings.resourcemanager.GetString("trNum:") + " : " + docNum
                 : processType == "inv" ? description = AppSettings.resourcemanager.GetString("trInv")//yasmine
                 : AppSettings.resourcemanager.GetString("trCredit");

            set => description = value;
        }
        public string Description1
        {//
            get =>
                //description1 = (transType == "p" && processType != "inv") ? description1 = AppSettings.resourcemanager.GetString("trPaymentReceipt")
                description1 = (transType == "p" && processType != "inv") ? description1 = AppSettings.resourcemanager.GetString("trPayment")
                : description1 = (transType == "d" && processType != "inv") ? description1 = AppSettings.resourcemanager.GetString("trReceipt")
                : invId > 0 && processType == "inv" ? description1 = AppSettings.resourcemanager.GetString("tr_Invoice") + " " + AppSettings.resourcemanager.GetString("trNum:") + " : " + invNumber
                : ""
                ; set => description1 = value;
        }
        public string Description2
        {
            get; set;
        }
        public string Description3
        {
            get => bondId > 0 ?
                description3 = AppSettings.resourcemanager.GetString("trBond") + " " + AppSettings.resourcemanager.GetString("trNum:") + " : " + bondNumber
                 :
                processType;

            set => description3 = value;
        }

        public string BIsReceived
        {
            get; set;
        }
    }
    public class Storage
    {
        public string itemType { get; set; }
        //storagecost
        public Nullable<int> storageCostId { get; set; }
        public string storageCostName { get; set; }
        public decimal storageCostValue { get; set; }


        //
        public int min { get; set; }
        public int max { get; set; }

        public Nullable<int> minUnitId { get; set; }
        public Nullable<int> maxUnitId { get; set; }
        public string minUnitName { get; set; }
        public string maxUnitName { get; set; }
        private string minAll;
        private string maxAll;
        public string MinAll { get => minAll = minUnitName + " " + min.ToString(); set => minAll = value; }
        public string MaxAll { get => maxAll = maxUnitName + " " + max.ToString(); set => maxAll = value; }
        // item unit
        public string itemName { get; set; }
        public string unitName { get; set; }
        public int itemUnitId { get; set; }

        public int itemId { get; set; }
        public int unitId { get; set; }

        public string barcode { get; set; }
        //item location
        public string CreateuserName { get; set; }
        public string CreateuserLName { get; set; }
        public string CreateuserAccName { get; set; }
        public string UuserName { get; set; }
        public string UuserLName { get; set; }
        public string UuserAccName { get; set; }

        //
        public string branchName { get; set; }

        public string branchType { get; set; }
        //itemslocations

        public int itemsLocId { get; set; }
        public int locationId { get; set; }
        public Nullable<decimal> quantity { get; set; }

        public Nullable<System.DateTime> startDate { get; set; }
        public Nullable<System.DateTime> endDate { get; set; }

        public string IULnote { get; set; }
        public Nullable<decimal> storeCost { get; set; }

        public string cuserName { get; set; }
        public string cuserLast { get; set; }
        public string cUserAccName { get; set; }
        public string uuserName { get; set; }
        public string uuserLast { get; set; }
        public string uUserAccName { get; set; }
        // Location
        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
        private string sectionLoactionName;

        public Nullable<byte> LocisActive { get; set; }
        public int sectionId { get; set; }
        public string Locnote { get; set; }
        public int branchId { get; set; }
        public Nullable<byte> LocisFreeZone { get; set; }


        // section

        public string Secname { get; set; }
        public Nullable<byte> SecisActive { get; set; }
        public string Secnote { get; set; }
        public Nullable<byte> SecisFreeZone { get; set; }
        public string SectionLoactionName { get => sectionLoactionName = Secname + " - " + x + y + z; set => sectionLoactionName = value; }


        private string itemUnits;
        private string loactionName;
        public string LoactionName { get => loactionName = x + y + z; set => loactionName = value; }
        public string ItemUnits { get => itemUnits = itemName + " - " + unitName; set => itemUnits = value; }

    }
    public class InventoryClass

    {
        public string userFalls { get; set; }
        private string itemUnits;
        public string ItemUnits { get => itemUnits = itemName + " - " + unitName; set => itemUnits = value; }
        public int shortfalls { get; set; }
        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }
        public int inventoryILId { get; set; }
        public Nullable<bool> isDestroyed { get; set; }
        public Nullable<int> amount { get; set; }
        public Nullable<int> amountDestroyed { get; set; }
        public Nullable<int> realAmount { get; set; }
        public Nullable<int> itemLocationId { get; set; }
        public Nullable<byte> isActive { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public int itemId { get; set; }
        public string itemName { get; set; }

        public int unitId { get; set; }
        public int itemUnitId { get; set; }
        public string unitName { get; set; }
        public int sectionId { get; set; }
        public string Secname { get; set; }

        public string x { get; set; }
        public string y { get; set; }
        public string z { get; set; }
        public string itemType { get; set; }
        public Nullable<System.DateTime> inventoryDate { get; set; }
        public string inventoryNum { get; set; }
        public string inventoryType { get; set; }
        public int inventoryId { get; set; }
        public decimal diffPercentage { get; set; }
        public int nCount { get; set; }
        public int dCount { get; set; }
        public int aCount { get; set; }
        public int itemCount { get; set; }
        public int DestroyedCount { get; set; }

    }
    public class ItemUnitCombo
    {

        public int itemUnitId { get; set; }
        public string itemUnitName { get; set; }

    }
    public class CouponCombo
    {

        public int Copcid { get; set; }
        public string Copname { get; set; }
    }

    public class OfferCombo
    {

        public int OofferId { get; set; }
        public string Oname { get; set; }
    }

    public class InvoiceClassCombo
    {

        public int invoiceId { get; set; }
        public string invNumber { get; set; }
        public string invBarcode { get; set; }
    }

    public class ItemTransferInvoice
    {// new properties
        public string shippingCompanyName { get; set; }
        public string shipUserName { get; set; }
        public string shipUserLastName { get; set; }
        public string categoryName { get; set; }
        public Nullable<int> categoryId { get; set; }
        public int archived { get; set; }
        public double? itemAvg { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public string causeFalls { get; set; }
        public string causeDestroy { get; set; }
        public string userdestroy { get; set; }
        public string userFalls { get; set; }
        public Nullable<int> userId { get; set; }
        public string inventoryNum { get; set; }
        public string inventoryType { get; set; }
        public Nullable<DateTime> inventoryDate { get; set; }
        public int itemCount { get; set; }
        public Nullable<decimal> subTotal { get; set; }
        public string agentCompany { get; set; }
        public string itemName { get; set; }
        public string unitName { get; set; }
        public int itemsTransId { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public Nullable<int> itemId { get; set; }
        public Nullable<int> unitId { get; set; }
        public Nullable<long> quantity { get; set; }
        public Nullable<decimal> price { get; set; }
        public string barcode { get; set; }

        // ItemTransfer
        public int ITitemsTransId { get; set; }
        public Nullable<int> ITitemUnitId { get; set; }
        public Nullable<int> updateUserId { get; set; }
        public Nullable<int> ITitemId { get; set; }
        public Nullable<int> ITunitId { get; set; }
        public string ITitemName { get; set; }
        public string ITunitName { get; set; }
        private string ITitemUnitName;
        public Nullable<long> ITquantity { get; set; }
        public Nullable<decimal> ITprice { get; set; }


        public Nullable<System.DateTime> ITcreateDate { get; set; }
        public Nullable<System.DateTime> ITupdateDate { get; set; }
        public Nullable<int> ITcreateUserId { get; set; }
        public Nullable<int> ITupdateUserId { get; set; }
        public string ITnotes { get; set; }

        public string ITbarcode { get; set; }
        public string ITCreateuserName { get; set; }
        public string ITCreateuserLName { get; set; }
        public string ITCreateuserAccName { get; set; }

        public string ITUpdateuserName { get; set; }
        public string ITUpdateuserLName { get; set; }
        public string ITUpdateuserAccName { get; set; }
        //invoice
        public int invoiceId { get; set; }
        public string invNumber { get; set; }
        public string invBarcode { get; set; }
        public Nullable<int> agentId { get; set; }
        public Nullable<int> createUserId { get; set; }
        public string invType { get; set; }
        public string discountType { get; set; }
        public Nullable<decimal> ITdiscountValue { get; set; }
        public Nullable<decimal> discountValue { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> totalNet { get; set; }
        public Nullable<decimal> paid { get; set; }
        public Nullable<decimal> deserved { get; set; }
        public Nullable<System.DateTime> deservedDate { get; set; }
        public Nullable<System.DateTime> invDate { get; set; }
        public Nullable<System.DateTime> IupdateDate { get; set; }
        public Nullable<int> IupdateUserId { get; set; }
        public Nullable<int> invoiceMainId { get; set; }
        public string invCase { get; set; }
        public Nullable<System.TimeSpan> invTime { get; set; }
        public string Inotes { get; set; }
        public string vendorInvNum { get; set; }
        public string name { get; set; }
        public string branchName { get; set; }
        public Nullable<System.DateTime> vendorInvDate { get; set; }
        public Nullable<int> branchId { get; set; }
        public Nullable<int> itemsCount { get; set; }
        public Nullable<decimal> tax { get; set; }
        public Nullable<int> taxtype { get; set; }
        public Nullable<int> posId { get; set; }
        public Nullable<byte> isApproved { get; set; }
        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorName { get; set; }
        public string ITtype { get; set; }
        private string invTypeNumber;//number
        //public string InvTypeNumber { get => invTypeNumber = invType + "-" + invNumber; set => invTypeNumber = value; }
        public string InvTypeNumber
        {
            get => invType == "ex" ? invTypeNumber = AppSettings.resourcemanager.GetString("trExport") + "-" + invNumber
                 : invType == "im" ? invTypeNumber = AppSettings.resourcemanager.GetString("trImport") + "-" + invNumber
                 : invType == "p" ? invTypeNumber = AppSettings.resourcemanager.GetString("trPurchaseInvoice") + "-" + invNumber
                 : invType == "pw" ? invTypeNumber = AppSettings.resourcemanager.GetString("trPurchaseInvoiceWaiting") + "-" + invNumber
                 : invType == "s" ? invTypeNumber = AppSettings.resourcemanager.GetString("trSalesInvoice") + "-" + invNumber
                 : invType == "sb" ? invTypeNumber = AppSettings.resourcemanager.GetString("trSalesReturnInvoice") + "-" + invNumber
                 : invType == "pb" ? invTypeNumber = AppSettings.resourcemanager.GetString("trPurchaseReturnInvoice") + "-" + invNumber
                 : invType == "pbw" ? invTypeNumber = AppSettings.resourcemanager.GetString("trPurchaseReturnInvoiceWaiting") + "-" + invNumber
                 : invType == "pd" ? invTypeNumber = AppSettings.resourcemanager.GetString("trDraftPurchaseBill") + "-" + invNumber
                 : invType == "sd" ? invTypeNumber = AppSettings.resourcemanager.GetString("trSalesDraft") + "-" + invNumber
                 : invType == "sbd" ? invTypeNumber = AppSettings.resourcemanager.GetString("trSalesReturnDraft") + "-" + invNumber
                 : invType == "pbd" ? invTypeNumber = AppSettings.resourcemanager.GetString("trPurchaseReturnDraft") + "-" + invNumber
                 : invType == "ord" ? invTypeNumber = AppSettings.resourcemanager.GetString("trSaleOrderDraft") + "-" + invNumber
                 : invType == "or" ? invTypeNumber = AppSettings.resourcemanager.GetString("trSaleOrder") + "-" + invNumber
                 : invType == "ors" ? invTypeNumber = AppSettings.resourcemanager.GetString("trSaleOrder") + "-" + invNumber

                : invType == "pod" ? invTypeNumber = AppSettings.resourcemanager.GetString("trPurchaceOrderDraft") + "-" + invNumber
                 : invType == "po" ? invTypeNumber = AppSettings.resourcemanager.GetString("trPurchaceOrder") + "-" + invNumber
                  : invType == "pos" ? invTypeNumber = AppSettings.resourcemanager.GetString("trPurchaceOrder") + "-" + invNumber

                : invType == "qd" ? invTypeNumber = AppSettings.resourcemanager.GetString("trQuotationsDraft") + "-" + invNumber
                 : invType == "q" ? invTypeNumber = AppSettings.resourcemanager.GetString("trQuotations") + "-" + invNumber
                 : invType == "qs" ? invTypeNumber = AppSettings.resourcemanager.GetString("trQuotations") + "-" + invNumber

                : invType == "d" ? invTypeNumber = AppSettings.resourcemanager.GetString("trDestructive") + "-" + invNumber
                 : invType == "sh" ? invTypeNumber = AppSettings.resourcemanager.GetString("trShortage") + "-" + invNumber
                 : invType == "imd" ? invTypeNumber = AppSettings.resourcemanager.GetString("trImportDraft") + "-" + invNumber
                 : invType == "imw" ? invTypeNumber = AppSettings.resourcemanager.GetString("trImportOrder") + "-" + invNumber
                 : invType == "exd" ? invTypeNumber = AppSettings.resourcemanager.GetString("trExportDraft") + "-" + invNumber
                 : invType == "exw" ? invTypeNumber = AppSettings.resourcemanager.GetString("trExportOrder") + "-" + invNumber

                 : "";
            set => invTypeNumber = value;
        }

        // for report
        public int countP { get; set; }
        public int countS { get; set; }
        public int count { get; set; }

        public Nullable<decimal> totalS { get; set; }
        public Nullable<decimal> totalNetS { get; set; }
        public Nullable<decimal> totalP { get; set; }
        public Nullable<decimal> totalNetP { get; set; }
        public string branchType { get; set; }
        public string posName { get; set; }
        public string posCode { get; set; }
        public string agentName { get; set; }


        public string agentType { get; set; }
        public string agentCode { get; set; }
        public string cuserName { get; set; }
        public string cuserLast { get; set; }
        public string cUserAccName { get; set; }
        public string uuserName { get; set; }
        public string uuserLast { get; set; }
        public string uUserAccName { get; set; }
        private string agentTypeAgent;
        public string AgentTypeAgent
        {
            get => agentType == "v" ? agentTypeAgent = AppSettings.resourcemanager.GetString("trVendor") + "-"
                                      :
                                       agentTypeAgent = AppSettings.resourcemanager.GetString("trCustomer") + "-"
                                       ;
            set => agentTypeAgent = value;
        }
        private string agentNameAgent;
        public string AgentNameAgent
        {
            get => agentName == "unknown" ? agentNameAgent = AppSettings.resourcemanager.GetString("trUnKnown")
                                      :
                                       agentTypeAgent = agentName
                                       ;
            set => agentTypeAgent = value;
        }

        public int countPb { get; set; }
        public int countD { get; set; }
        public Nullable<decimal> totalPb { get; set; }
        public Nullable<decimal> totalD { get; set; }
        public Nullable<decimal> totalNetPb { get; set; }
        public Nullable<decimal> totalNetD { get; set; }


        public Nullable<decimal> paidPb { get; set; }
        public Nullable<decimal> deservedPb { get; set; }
        public Nullable<decimal> discountValuePb { get; set; }
        public Nullable<decimal> paidD { get; set; }
        public Nullable<decimal> deservedD { get; set; }
        public Nullable<decimal> discountValueD { get; set; }
        // coupon


        public int CopcId { get; set; }
        public string Copname { get; set; }
        public string Copcode { get; set; }
        public Nullable<byte> CopisActive { get; set; }
        public Nullable<byte> CopdiscountType { get; set; }
        public Nullable<decimal> CopdiscountValue { get; set; }
        public Nullable<System.DateTime> CopstartDate { get; set; }
        public Nullable<System.DateTime> CopendDate { get; set; }
        public string Copnotes { get; set; }
        public Nullable<int> Copquantity { get; set; }
        public Nullable<int> CopremainQ { get; set; }
        public Nullable<decimal> CopinvMin { get; set; }
        public Nullable<decimal> CopinvMax { get; set; }
        public Nullable<System.DateTime> CopcreateDate { get; set; }
        public Nullable<System.DateTime> CopupdateDate { get; set; }
        public Nullable<int> CopcreateUserId { get; set; }
        public Nullable<int> CopupdateUserId { get; set; }
        public string Copbarcode { get; set; }
        public Nullable<decimal> couponTotalValue { get; set; }
        // offer

        public int OofferId { get; set; }
        public string Oname { get; set; }
        public string Ocode { get; set; }
        public Nullable<byte> OisActive { get; set; }
        public string OdiscountType { get; set; }
        public Nullable<decimal> OdiscountValue { get; set; }
        public Nullable<System.DateTime> OstartDate { get; set; }
        public Nullable<System.DateTime> OendDate { get; set; }
        public Nullable<System.DateTime> OcreateDate { get; set; }
        public Nullable<System.DateTime> OupdateDate { get; set; }
        public Nullable<int> OcreateUserId { get; set; }
        public Nullable<int> OupdateUserId { get; set; }
        public string Onotes { get; set; }
        public Nullable<int> Oquantity { get; set; }
        public int Oitemofferid { get; set; }
        public Nullable<decimal> offerTotalValue { get; set; }

        //external
        public int movbranchid { get; set; }
        public string movbranchname { get; set; }
        // internal
        public string exportBranch { get; set; }
        public string importBranch { get; set; }
        public int exportBranchId { get; set; }
        public int importBranchId { get; set; }
        private string itemUnits;
        private int cusCount;
        private int venCount;
        private int pCount;
        private int sCount;
        private int pbCount;
        private int sbCount;
        public string ItemUnits { get => itemUnits = itemName + " - " + unitName; set => itemUnits = value; }
        public int CusCount { get => cusCount; set => cusCount = value; }
        public int VenCount { get => venCount; set => venCount = value; }

        public int PCount { get => pCount; set => pCount = value; }
        public int SCount { get => sCount; set => sCount = value; }
        public int PbCount { get => pbCount; set => pbCount = value; }
        public int SbCount { get => sbCount; set => sbCount = value; }
        private int importCount;
        private int exportCount;
        public string ITitemUnitName1 { get => ITitemUnitName = ITitemName + " - " + ITunitName; set => ITitemUnitName = value; }
        public int ImportCount { get => importCount; set => importCount = value; }
        public int ExportCount { get => exportCount; set => exportCount = value; }
        public string processType { get; set; }

 
    }

    public class OrderPreparingSTS
    {
        public int orderPreparingId { get; set; }
        public string orderNum { get; set; }
        public Nullable<System.DateTime> orderTime { get; set; }
        public Nullable<int> invoiceId { get; set; }
        public string notes { get; set; }
        public Nullable<decimal> preparingTime { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }
        public Nullable<System.DateTime> updateDate { get; set; }
        public Nullable<int> createUserId { get; set; }
        public Nullable<int> updateUserId { get; set; }


        // item
        public string itemName { get; set; }
        public Nullable<int> itemUnitId { get; set; }
        public int quantity { get; set; }
        //order
        public string status { get; set; }
        public int num { get; set; }
        public decimal remainingTime { get; set; }
        public string tables { get; set; }
        public string waiter { get; set; }
        //invoice

        public string invType { get; set; }
        public Nullable<int> shippingCompanyId { get; set; }
        public string branchName { get; set; }
        public Nullable<int> branchId { get; set; }

      //  public List<itemOrderPreparingModel> items { get; set; }
        //
        public Nullable<int> categoryId { get; set; }
        public string categoryName { get; set; }
        public Nullable<decimal> realDuration { get; set; }
        public string invNumber { get; set; }
        public string invBarcode { get; set; }
        public Nullable<int> tagId { get; set; }
        public string tagName { get; set; }
        public Nullable<System.DateTime> listedDate { get; set; }

        public string shipUserName { get; set; }
        public string shipUserLastName { get; set; }
        public string shippingCompanyName { get; set; }
        public Nullable<int> shipUserId { get; set; }
     
        public Nullable<int> agentId { get; set; }
        public string agentName { get; set; }
        public string agentCompany { get; set; }
        public string agentType { get; set; }
        public string agentCode { get; set; }
        public List<orderPreparingStatus> orderStatusList { get; set; }
        public decimal orderDuration { get; set; }
        public string orderDurationConv { get; set; }
        public string statusConv { get; set; }
        public string categoryNameConv { get; set; }
  
    }
   

    public class SalesMembership
    { 
        
        //invoice
        public int invoiceId { get; set; }
        public string invNumber { get; set; }
        public string invBarcode { get; set; }
        public string invType { get; set; }
        public string discountType { get; set; }
        public Nullable<decimal> discountValue { get; set; }
        public Nullable<decimal> total { get; set; }
        public Nullable<decimal> totalNet { get; set; }
        public Nullable<decimal> paid { get; set; }
        public Nullable<decimal> deserved { get; set; }
        public Nullable<System.DateTime> deservedDate { get; set; }
        public Nullable<System.DateTime> invDate { get; set; }
   
        public Nullable<int> invoiceMainId { get; set; }
        public string invCase { get; set; }
        public Nullable<System.TimeSpan> invTime { get; set; }
        public string notes { get; set; }
        public Nullable<System.DateTime> createDate { get; set; }//
        public Nullable<System.DateTime> updateDate { get; set; }

        public Nullable<decimal> tax { get; set; }
 
        public Nullable<byte> isApproved { get; set; }
        public int count { get; set; }
 
       

        //pos
        public Nullable<int> posId { get; set; }
        public string posName { get; set; }
        public string posCode { get; set; }
        //branch

        public Nullable<int> branchCreatorId { get; set; }
        public string branchCreatorName { get; set; }
        public Nullable<int> branchId { get; set; }
        public string branchName { get; set; }
        public string branchType { get; set; }


        //agent
        public Nullable<int> agentId { get; set; }
        public string agentCompany { get; set; }
        public string agentName { get; set; }

        public string agentType { get; set; }
        public string agentCode { get; set; }
        public string vendorInvNum { get; set; }


        public Nullable<System.DateTime> vendorInvDate { get; set; }
        //user
        public Nullable<int> createUserId { get; set; }
        public string cuserName { get; set; }
        public string cuserLast { get; set; }
        public string cUserAccName { get; set; }
        public string uuserName { get; set; }
        public string uuserLast { get; set; }
        public string uUserAccName { get; set; }
        public Nullable<int> userId { get; set; }
        //membership

        public Nullable<int> membershipId { get; set; }
        public string membershipsCode { get; set; }
        public string membershipsName { get; set; }

        public List<CouponInvoice> CouponInvoiceList { get; set; }
        public List<ItemTransfer> itemsTransferList { get; set; }
        public List<InvoicesClass> invoiceClassDiscountList { get; set; }

        public decimal invclassDiscount { get; set; }
        public decimal couponDiscount { get; set; }
        public decimal offerDiscount { get; set; }
        public decimal totalDiscount { get; set; }

        public Nullable<System.DateTime> endDate { get; set; }
        public string subscriptionType { get; set; }
        //invClass
        public string invoicesClassName { get; set; }
        public Nullable<int> invClassDiscountId { get; set; }

        public Nullable<int> invClassId { get; set; }
        public byte invClassdiscountType { get; set; }
        public decimal invClassdiscountValue { get; set; }
        public decimal finalDiscount { get; set; }

    }

    class Statistics
    {

        //****************************************************
        public async Task<List<ItemUnitInvoiceProfit>> GetInvoiceProfit(int mainBranchId, int userId)
        {

            List<ItemUnitInvoiceProfit> list = new List<ItemUnitInvoiceProfit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetInvoiceProfit", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnitInvoiceProfit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
        }

        public async Task<List<ItemUnitInvoiceProfit>> GetItemProfit(int mainBranchId, int userId)
        {
            List<ItemUnitInvoiceProfit> list = new List<ItemUnitInvoiceProfit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetItemProfit", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnitInvoiceProfit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
        }

        #region old
        //public async Task<List<ItemLocation>> GetItemQtyInBranches(int itemId, int UnitId, string BranchType)
        //{
        //    List<ItemLocation> list = null;
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
        //        request.RequestUri = new Uri(Global.APIUri + "Statistics/GetItemQtyInBranches?itemId=" + itemId + "&UnitId=" + UnitId + "&BranchType=" + BranchType);
        //        request.Headers.Add("APIKey", Global.APIKey);
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
        //            list = JsonConvert.DeserializeObject<List<ItemLocation>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        //            return list;
        //        }
        //        else //web api sent error response 
        //        {
        //            list = new List<ItemLocation>();
        //        }
        //        return list;
        //    }
        //}
        #endregion

        // المشتريات
        #region Purchases
        public async Task<List<Invoice>> GetPurinv(int mainBranchId, int userId)
        {


            List<Invoice> list = new List<Invoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPurinv", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<Invoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
            
            #region old
            //List<Invoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetPurinv?mainBranchId=" + mainBranchId + "&userId=" + userId );
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
            //        list = JsonConvert.DeserializeObject<List<Invoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<Invoice>();
            //    }
            //    return list;
            //}

            #endregion

        }

        // الاصناف في الفواتير

        public async Task<List<ItemTransferInvoice>> GetPuritem(int mainBranchId, int userId)
        {
            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPuritem", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region old
            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetPuritem?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
            #endregion
        }

        // عدد الاصناف في الفواتير
        public async Task<List<ItemTransferInvoice>> GetPuritemcount(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPuritemcount", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region old
            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetPuritemcount?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
            #endregion
        }
        //    الشراء عدد العناصر في فواتير طلبات
        public async Task<List<ItemTransferInvoice>> GetPurorderitemcount(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPurorderitemcount", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
 
        }
        #endregion

       
        // مبيعات
        #region Sales 
        // الفواتير مع العناصر
        public async Task<List<ItemTransferInvoice>> GetSaleitem(int mainBranchId, int userId)
        {
            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetSaleitem", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region
            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetSaleitem?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
            #endregion
        }

        //  عدد العناصر في فواتير عرض السعر
        public async Task<List<ItemTransferInvoice>> GetQtitemcount(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetQtitemcount", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
            #region
            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetQtitemcount?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
            #endregion
        }


        //  للمبيعات عدد العناصر في فواتير الطلبات
        public async Task<List<ItemTransferInvoice>> Getorderitemcount(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/Getorderitemcount", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region
            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/Getorderitemcount?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
            #endregion
        }


      
        public async Task<List<ItemTransferInvoice>> GetSaleitemcount(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetSaleitemcount", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
 
        }
        // الفواتير  مع الكوبون
        public async Task<List<ItemTransferInvoice>> GetSalecoupon(int mainBranchId, int userId)
        {
            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetSalecoupon", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetSalecoupon?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
        }

        // الفواتير مع العناصر مع الاوفر
        public async Task<List<ItemTransferInvoice>> GetSaleOffer(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetSaleOffer", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
            #region
            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetSaleOffer?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
            #endregion
        }

        // الفواتير مع العناصر التي لديها اوفر
        public async Task<List<ItemTransferInvoice>> GetPromoOffer(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPromoOffer", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }

        // الاشتراكات
       
        public async Task<List<SalesMembership>> GetSaleMembership(int mainBranchId, int userId)
        {

            List<SalesMembership> list = new List<SalesMembership>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetSaleMembership", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<SalesMembership>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }
        //Inv class
        public async Task<List<SalesMembership>> GetInvoiceClass(int mainBranchId, int userId)
        {

            List<SalesMembership> list = new List<SalesMembership>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetInvoiceClass", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<SalesMembership>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }
        #endregion 
        //
        #region combo

        public List<ItemUnitCombo> GetIUComboList(List<ItemTransferInvoice> ITInvoice)
        {
            List<ItemUnitCombo> iulist = new List<ItemUnitCombo>();

            iulist = ITInvoice.GroupBy(x => x.ITitemUnitId)
                   .Select(g => new ItemUnitCombo { itemUnitId = (int)g.FirstOrDefault().ITitemUnitId, itemUnitName = g.FirstOrDefault().ITitemName + " - " + g.FirstOrDefault().ITunitName }).ToList();
            return iulist;

        }
        public List<CouponCombo> GetCopComboList(List<ItemTransferInvoice> ITInvoice)
        {
            List<CouponCombo> iulist = new List<CouponCombo>();

            iulist = ITInvoice.GroupBy(x => x.CopcId)
                   .Select(g => new CouponCombo { Copcid = g.FirstOrDefault().CopcId, Copname = g.FirstOrDefault().Copname }).ToList();
            return iulist;

        }
        public List<OfferCombo> GetOfferComboList(List<ItemTransferInvoice> ITInvoice)
        {
            List<OfferCombo> iulist = new List<OfferCombo>();

            iulist = ITInvoice.GroupBy(x => x.OofferId)
                   .Select(g => new OfferCombo { OofferId = g.FirstOrDefault().OofferId, Oname = g.FirstOrDefault().Oname }).ToList();
            return iulist;

        }
        public List<InvoiceClassCombo> GetInvoiceClassComboList(List<SalesMembership> ITInvoice)
        {
            List<InvoiceClassCombo> iulist = new List<InvoiceClassCombo>();

            iulist = ITInvoice.GroupBy(x => x.invoiceId)
                   .Select(g => new InvoiceClassCombo { invoiceId = g.FirstOrDefault().invoiceId, invNumber = g.FirstOrDefault().invNumber }).ToList();
            return iulist;

        }
        #endregion
        //OfferCombo





        // المخزون 
        #region Storage

        public async Task<List<Storage>> GetStorage(int mainBranchId, int userId)
        {

            List<Storage> list = new List<Storage>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetStorage", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<Storage>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region
            //List<Storage> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetStorage?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<Storage>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<Storage>();
            //    }
            //    return list;
            //}
            #endregion
        }

        #region
        // حركة الاصناف التي دخلت الى الفرع
        //public async Task<List<ItemTransferInvoice>> GetInItems()
        //{
        //    List<ItemTransferInvoice> list = null;
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
        //        request.RequestUri = new Uri(Global.APIUri + "Statistics/GetInItems");
        //        request.Headers.Add("APIKey", Global.APIKey);
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
        //            list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
        //            return list;
        //        }
        //        else //web api sent error response 
        //        {
        //            list = new List<ItemTransferInvoice>();
        //        }
        //        return list;
        //    }
        //}
        #endregion
        // حركة الاصناف الخارجية (مع الزبائن والموردين)
        public async Task<List<ItemTransferInvoice>> GetExternalMov(int mainBranchId, int userId)
        {
            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetExternalMov", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region
            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetExternalMov?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
            #endregion
        }
        //حركة الإدخال المباشر
        public async Task<List<ItemTransferInvoice>> GetDirectInMov(int mainBranchId, int userId)
        {
            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetDirectInMov", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
        }
        //حركة الاصناف الداخلية بين الفروع
        public async Task<List<ItemTransferInvoice>> GetInternalMov(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetInternalMov", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;
            #region
            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetInternalMov?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
            #endregion
        }


        //GetSpendingStorage
        //فواتير الصرف المنفذة من المخزن الى المطبخ 
        public async Task<List<ItemTransferInvoice>> GetSpendingStorage(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetSpendingStorage", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }

        #endregion


        // الجرد
        #region
        // عناصر الجرد

        public async Task<List<InventoryClass>> GetInventory(int mainBranchId, int userId)
        {

            List<InventoryClass> list = new List<InventoryClass>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetInventory", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<InventoryClass>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<InventoryClass> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetInventory?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<InventoryClass>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<InventoryClass>();
            //    }
            //    return list;
            //}
        }

        public async Task<List<InventoryClass>> GetInventoryItems(int mainBranchId, int userId)
        {

            List<InventoryClass> list = new List<InventoryClass>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetInventoryItems", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<InventoryClass>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


            //List<InventoryClass> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetInventoryItems?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<InventoryClass>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<InventoryClass>();
            //    }
            //    return list;
            //}
        }


        // العناصر التالفة
        public async Task<List<ItemTransferInvoice>> GetDesItems(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetDesItems", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetDesItems?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
        }

        // العناصر الناقصة
        public async Task<List<ItemTransferInvoice>> GetFallsItems(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetFallsItems", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetFallsItems?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
        }

        #endregion


        // المحاسبة
        #region Accountant
        // المدفوعات
        public async Task<List<CashTransferSts>> GetPayments()
        {

            List<CashTransferSts> list = new List<CashTransferSts>();
            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mainBranchId", mainBranchId.ToString());
            //parameters.Add("userId", userId.ToString());
            //IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPayments", parameters);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPayments");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransferSts>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region old
            //List<CashTransferSts> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetPayments");
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
            //        list = JsonConvert.DeserializeObject<List<CashTransferSts>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<CashTransferSts>();
            //    }
            //    return list;
            //}
            #endregion
        }

        //المقبوضات
        public async Task<List<CashTransferSts>> GetReceipt()
        {

            List<CashTransferSts> list = new List<CashTransferSts>();
            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mainBranchId", mainBranchId.ToString());
            //parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetReceipt");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransferSts>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region old
            //List<CashTransferSts> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetReceipt?mainBranchId");
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
            //        list = JsonConvert.DeserializeObject<List<CashTransferSts>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<CashTransferSts>();
            //    }
            //    return list;
            //}
            #endregion
        }


        // bank

        public async Task<List<CashTransferSts>> GetBankTrans()
        {

            List<CashTransferSts> list = new List<CashTransferSts>();
            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mainBranchId", mainBranchId.ToString());
            //parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetBankTrans");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransferSts>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region old
            //List<CashTransferSts> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetBankTrans?mainBranchId");
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
            //        list = JsonConvert.DeserializeObject<List<CashTransferSts>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<CashTransferSts>();
            //    }
            //    return list;
            //}
            #endregion

        }

        //posTrans
        public async Task<List<CashTransferSts>> GetPosTrans()
        {

            List<CashTransferSts> list = new List<CashTransferSts>();
            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mainBranchId", mainBranchId.ToString());
            //parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPosTrans");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransferSts>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


            #region old
            //List<CashTransferSts> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetPosTrans");
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
            //        list = JsonConvert.DeserializeObject<List<CashTransferSts>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<CashTransferSts>();
            //    }
            //    return list;
            //}
            #endregion
        }

        //كشف حساب
        public async Task<List<CashTransferSts>> GetStatement()
        {

            List<CashTransferSts> list = new List<CashTransferSts>();
            //Dictionary<string, string> parameters = new Dictionary<string, string>();
            //parameters.Add("mainBranchId", mainBranchId.ToString());
            //parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetStatement");

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransferSts>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region old
            //List<CashTransferSts> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetStatement");
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
            //        list = JsonConvert.DeserializeObject<List<CashTransferSts>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<CashTransferSts>();
            //    }
            //    return list;
            //}
            #endregion
        }

        // رصيد نقاط البيع والفروع

        public async Task<List<BalanceSTS>> GetBalance(int mainBranchId, int userId)
        {

            List<BalanceSTS> list = new List<BalanceSTS>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetBalance", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<BalanceSTS>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region old
            //List<BalanceSTS> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetBalance?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<BalanceSTS>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<BalanceSTS>();
            //    }
            //    return list;
            //}
            #endregion
        }

        // الارباح
        public async Task<List<ItemUnitInvoiceProfit>> GetProfit(int mainBranchId, int userId)
        {

            List<ItemUnitInvoiceProfit> list = new List<ItemUnitInvoiceProfit>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetProfit", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemUnitInvoiceProfit>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            #region old
            //List<ItemUnitInvoiceProfit> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetProfit?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemUnitInvoiceProfit>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemUnitInvoiceProfit>();
            //    }
            //    return list;
            //}
            #endregion
        }

        // الضرائب
        public async Task<List<ItemTransferInvoiceTax>> GetInvItemTax(int mainBranchId, int userId)
        {

            List<ItemTransferInvoiceTax> list = new List<ItemTransferInvoiceTax>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetInvItemTax", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoiceTax>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


        }

        public async Task<List<CashTransfer>> GetBytypeAndSideForPos(string type, string side)
        {
            // string type, string side
            List<CashTransfer> list = new List<CashTransfer>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("type", type.ToString());
            parameters.Add("side", side.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetBytypeAndSideForPos", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransfer>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


        }

        // الفتح والاغلاق
        public async Task<List<POSOpenCloseModel>> GetPosCashOpenClose(int mainBranchId, int userId)
        {

            List<POSOpenCloseModel> list = new List<POSOpenCloseModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPosCashOpenClose", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<POSOpenCloseModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


        }


        //العمليات المنفذة بين تاريخ الفتح والاغلاق
        public async Task<List<OpenClosOperatinModel>> GetTransBetweenOpenClose(int openCashTransId, int closeCashTransId)
        {

            List<OpenClosOperatinModel> list = new List<OpenClosOperatinModel>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("openCashTransId", openCashTransId.ToString());
            parameters.Add("closeCashTransId", closeCashTransId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetTransBetweenOpenClose", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<OpenClosOperatinModel>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


        }


        // combo
        #region
        public class VendorCombo
        {
            private int? vendorId;
            private string vendorName;
            private string side;
            private string userAcc;
            private int? userId;

            public int? VendorId { get => vendorId; set => vendorId = value; }
            //public string VendorName { get => vendorName; set => vendorName = value; }
            public string VendorName
            {
                get => vendorName == null ? (side == "c" ? vendorName = AppSettings.resourcemanager.GetString("trCashCustomer")
                                                        : vendorName = AppSettings.resourcemanager.GetString("trUnKnown"))
                   : vendorName;
                set => vendorName = value;
            }
            public string Side { get => side; set => side = value; }
            public string UserAcc { get => userAcc; set => userAcc = value; }
            public int? UserId { get => userId; set => userId = value; }
        }
        public List<VendorCombo> getVendorCombo(List<CashTransferSts> ITInvoice, string x)
        {
            List<VendorCombo> iulist = new List<VendorCombo>();

            //iulist = ITInvoice.Where(g => g.side == x).GroupBy(g => g.agentId).Select(g => new VendorCombo { VendorId = g.FirstOrDefault().agentId, VendorName = g.FirstOrDefault().agentName }).ToList();
            //iulist = ITInvoice.Where(g => g.side == x).GroupBy(g => g.agentId).Select(g => new VendorCombo { VendorId = g.FirstOrDefault().agentId, VendorName = g.FirstOrDefault().agentName, Side = g.FirstOrDefault().side }).ToList();
            iulist = ITInvoice.Where(g => g.side == x && (g.invShippingCompanyId == null || (g.invShippingCompanyId != null && g.shipUserId != null))).GroupBy(g => g.agentId).Select(g => new VendorCombo { VendorId = g.FirstOrDefault().agentId, VendorName = g.FirstOrDefault().agentName, Side = g.FirstOrDefault().side }).ToList();
            return iulist;

        }
        public List<VendorCombo> getCustomerForStatementCombo(List<CashTransferSts> ITInvoice, string x)
        {
            List<VendorCombo> iulist = new List<VendorCombo>();

            iulist = ITInvoice.Where(g => g.side == x).GroupBy(g => g.invAgentId).Select(g => new VendorCombo { VendorId = g.FirstOrDefault().invAgentId, VendorName = g.FirstOrDefault().invAgentName }).ToList();
            return iulist;

        }
        public List<VendorCombo> getUserAcc(List<CashTransferSts> ITInvoice, string x)
        {
            List<VendorCombo> iulist = new List<VendorCombo>();

            iulist = ITInvoice.Where(g => g.side == x).GroupBy(g => g.userId).Select(g => new VendorCombo { UserId = g.FirstOrDefault().userId, UserAcc = g.FirstOrDefault().userAcc }).ToList();
            return iulist;

        }
        public class PaymentsTypeCombo
        {
            private string paymentsTypeName;
            private string paymentsTypeText;

            public string PaymentsTypeName { get => paymentsTypeName; set => paymentsTypeName = value; }
            public string PaymentsTypeText
            {
                get => paymentsTypeName == "cash" ? paymentsTypeText = AppSettings.resourcemanager.GetString("trCash")
                    : paymentsTypeName == "doc" ? paymentsTypeText = AppSettings.resourcemanager.GetString("trDocument")
                    : paymentsTypeName == "cheque" ? paymentsTypeText = AppSettings.resourcemanager.GetString("trCheque")
                    : paymentsTypeName == "balance" ? paymentsTypeText = AppSettings.resourcemanager.GetString("trCredit")
                    : paymentsTypeName == "card" ? paymentsTypeText = AppSettings.resourcemanager.GetString("trAnotherPaymentMethods")
                    : paymentsTypeName == "inv" ? paymentsTypeText = AppSettings.resourcemanager.GetString("trInv")
                    : "";
                set => paymentsTypeText = value;
            }

        }
        public List<PaymentsTypeCombo> getPaymentsTypeCombo(List<CashTransferSts> ITInvoice)
        {
            List<PaymentsTypeCombo> iulist = new List<PaymentsTypeCombo>();

            iulist = ITInvoice.Where(g => g.processType != null).GroupBy(g => g.processType).Select(g => new PaymentsTypeCombo { PaymentsTypeName = g.FirstOrDefault().processType }).ToList();
            return iulist;

        }

        public List<PaymentsTypeCombo> getPaymentsTypeComboBySide(List<CashTransferSts> ITInvoice, string side)
        {
            List<PaymentsTypeCombo> iulist = new List<PaymentsTypeCombo>();

            iulist = ITInvoice.Where(g => g.processType != null && g.side == side).GroupBy(g => g.processType).Select(g => new PaymentsTypeCombo { PaymentsTypeName = g.FirstOrDefault().processType }).ToList();
            return iulist;

        }

        public class AccountantCombo
        {
            private string accountant;

            public string Accountant { get => accountant; set => accountant = value; }
        }
        public List<AccountantCombo> getAccounantCombo(List<CashTransferSts> ITInvoice, string x)
        {
            List<AccountantCombo> iulist = new List<AccountantCombo>();

            iulist = ITInvoice.Where(g => g.side == x).GroupBy(g => g.updateUserAcc).Select(g => new AccountantCombo { Accountant = g.FirstOrDefault().updateUserAcc }).ToList();
            return iulist;

        }
        public class ShippingCombo
        {
            private string shippingName;
            private int? shippingId;

            public string ShippingName { get => shippingName; set => shippingName = value; }
            public int? ShippingId { get => shippingId; set => shippingId = value; }
        }
        public List<ShippingCombo> getShippingCombo(List<CashTransferSts> ITInvoice)
        {
            List<ShippingCombo> iulist = new List<ShippingCombo>();

            iulist = ITInvoice.Where(g => g.shippingCompanyId != null).GroupBy(g => g.shippingCompanyId).Select(g => new ShippingCombo { ShippingId = g.FirstOrDefault().shippingCompanyId, ShippingName = g.FirstOrDefault().shippingCompanyName }).ToList();
            return iulist;

        }
        public List<ShippingCombo> getShippingForStatementCombo(List<CashTransferSts> ITInvoice)
        {
            List<ShippingCombo> iulist = new List<ShippingCombo>();

            iulist = ITInvoice.Where(g => g.invShippingCompanyId != null && g.shipUserId == null).GroupBy(g => g.invShippingCompanyId).Select(g => new ShippingCombo { ShippingId = g.FirstOrDefault().invShippingCompanyId, ShippingName = g.FirstOrDefault().invShippingCompanyName }).ToList();
            return iulist;

        }
        public class branchFromCombo
        {
            private string branchFromName;
            private int? branchFromId;

            public string BranchFromName { get => branchFromName; set => branchFromName = value; }
            public int? BranchFromId { get => branchFromId; set => branchFromId = value; }
        }
        //public List<branchFromCombo> getFromCombo(List<CashTransferSts> ITInvoice)
        //{
        //    List<branchFromCombo> iulist = new List<branchFromCombo>();

        //    iulist = ITInvoice.GroupBy(g => g.frombranchId).Select(g => new branchFromCombo { BranchFromId = g.FirstOrDefault().frombranchId, BranchFromName = g.FirstOrDefault().frombranchName }).ToList();
        //    return iulist;

        //}

        public List<branchFromCombo> getFromCombo(List<CashTransfer> ITInvoice)
        {
            List<branchFromCombo> iulist = new List<branchFromCombo>();

            iulist = ITInvoice.GroupBy(g => g.branchId).Select(g => new branchFromCombo { BranchFromId = g.FirstOrDefault().branchId, BranchFromName = g.FirstOrDefault().branchName }).ToList();
            return iulist;

        }

        public class branchToCombo
        {
            private string branchToName;
            private int? branchToId;

            public string BranchToName { get => branchToName; set => branchToName = value; }
            public int? BranchToId { get => branchToId; set => branchToId = value; }
        }
        //public List<branchToCombo> getToCombo(List<CashTransferSts> ITInvoice)
        //{
        //    List<branchToCombo> iulist = new List<branchToCombo>();

        //    iulist = ITInvoice.GroupBy(g => g.tobranchId).Select(g => new branchToCombo { BranchToId = g.FirstOrDefault().tobranchId, BranchToName = g.FirstOrDefault().tobranchName }).ToList();
        //    return iulist;

        //}
        public List<branchToCombo> getToCombo(List<CashTransfer> ITInvoice)
        {
            List<branchToCombo> iulist = new List<branchToCombo>();

            iulist = ITInvoice.GroupBy(g => g.branch2Id).Select(g => new branchToCombo { BranchToId = g.FirstOrDefault().branch2Id, BranchToName = g.FirstOrDefault().branch2Name }).ToList();
            return iulist;

        }
        public class posFromCombo
        {
            private string posFromName;
            private int? posFromId;
            private int? branchId;

            public string PosFromName { get => posFromName; set => posFromName = value; }
            public int? PosFromId { get => posFromId; set => posFromId = value; }
            public int? BranchId { get => branchId; set => branchId = value; }
        }
        //public List<posFromCombo> getFromPosCombo(List<CashTransferSts> ITInvoice)
        //{
        //    List<posFromCombo> iulist = new List<posFromCombo>();

        //    iulist = ITInvoice.GroupBy(g => g.fromposId).Select(g => new posFromCombo { PosFromId = g.FirstOrDefault().fromposId, PosFromName = g.FirstOrDefault().fromposName, BranchId = g.FirstOrDefault().frombranchId }).ToList();
        //    return iulist;

        //}

        public List<posFromCombo> getFromPosCombo(List<CashTransfer> ITInvoice)
        {
            List<posFromCombo> iulist = new List<posFromCombo>();

            iulist = ITInvoice.GroupBy(g => g.posId).Select(g => new posFromCombo { PosFromId = g.FirstOrDefault().posId, PosFromName = g.FirstOrDefault().posName, BranchId = g.FirstOrDefault().branchId }).ToList();
            return iulist;
        }
        public class posToCombo
        {
            private string posToName;
            private int? posToId;
            private int? branchId;

            public string PosToName { get => posToName; set => posToName = value; }
            public int? PosToId { get => posToId; set => posToId = value; }
            public int? BranchId { get => branchId; set => branchId = value; }
        }
        public List<posToCombo> getToPosCombo(List<CashTransfer> ITInvoice)
        {
            List<posToCombo> iulist = new List<posToCombo>();

            iulist = ITInvoice.GroupBy(g => g.pos2Id).Select(g => new posToCombo { PosToId = g.FirstOrDefault().pos2Id, PosToName = g.FirstOrDefault().pos2Name, BranchId = g.FirstOrDefault().branch2Id }).ToList();
            //iulist = ITInvoice.Where(g => g.toposId != posFromId).GroupBy(g => g.toposId).Select(g => new posToCombo { PosToId = g.FirstOrDefault().toposId, PosToName = g.FirstOrDefault().toposName, BranchId = g.FirstOrDefault().tobranchId }).ToList();
            return iulist;

        }
        #endregion
        #endregion

        // اليومية
        #region Daily

        // فواتير اليومية العامة في قسم التقارير
        public async Task<List<ItemTransferInvoice>> Getdailyinvoice(int mainBranchId, int userId, string date)
        {


            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("date", date);
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/Getdailyinvoice", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


        }
        // يومية فواتير المشتريات العامة في قسم التقارير
        public async Task<List<ItemTransferInvoice>> GetPurdailyinvoice(int mainBranchId, int userId, DateTime? date)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("date", date.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPurdailyinvoice", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetPurdailyinvoice?mainBranchId=" + mainBranchId + "&userId=" + userId + "&date=" + date);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
        }

        // فواتير اليوميةالخاصة بمستخدم
        public async Task<List<ItemTransferInvoice>> GetUserdailyinvoice(int mainBranchId, int userId)
        {


            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetUserdailyinvoice", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

            //List<ItemTransferInvoice> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetUserdailyinvoice?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<ItemTransferInvoice>();
            //    }
            //    return list;
            //}
        }

        // يومية الصندوق

        public async Task<List<CashTransferSts>> GetDailyStatement(int mainBranchId, int userId, DateTime? date)
        {


            List<CashTransferSts> list = new List<CashTransferSts>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            parameters.Add("date", date.ToString());
            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetDailyStatement", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransferSts>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;



            //List<CashTransferSts> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetDailyStatement?mainBranchId=" + mainBranchId + "&userId=" + userId + "&date=" + date);
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
            //        list = JsonConvert.DeserializeObject<List<CashTransferSts>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<CashTransferSts>();
            //    }
            //    return list;
            //}

        }

        // يومية الصندوق الخاصة بالمستخدم
        public async Task<List<CashTransferSts>> GetUserDailyStatement(int mainBranchId, int userId)
        {
            List<CashTransferSts> list = new List<CashTransferSts>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetUserDailyStatement", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<CashTransferSts>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;


            //List<CashTransferSts> list = null;
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
            //    request.RequestUri = new Uri(Global.APIUri + "Statistics/GetUserDailyStatement?mainBranchId=" + mainBranchId + "&userId=" + userId);
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
            //        list = JsonConvert.DeserializeObject<List<CashTransferSts>>(jsonString, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" });
            //        return list;
            //    }
            //    else //web api sent error response 
            //    {
            //        list = new List<CashTransferSts>();
            //    }
            //    return list;
            //}

        }

        #endregion

        #region Kitchen //المطبخ
        // preparing order
        public async Task<List<OrderPreparingSTS>> GetPreparingOrders(int mainBranchId, int userId)
        {
            List<OrderPreparingSTS> items = new List<OrderPreparingSTS>();
          
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetPreparingOrders", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<OrderPreparingSTS>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }

        // فواتير الصرف 
        public async Task<List<ItemTransferInvoice>> GetSpendingRequest(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetSpendingRequest", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }
        // فواتير الصرف عناصر  
        public async Task<List<ItemTransferInvoice>> GetSpendingItems(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetSpendingItems", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }

        // الاستهلاك
        //فواتير الاستهلاك 
        public async Task<List<ItemTransferInvoice>> GetConsumption(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetConsumption", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }
        //عناصر فواتير الاستهلاك
        public async Task<List<ItemTransferInvoice>> GetConsumptionItems(int mainBranchId, int userId)
        {

            List<ItemTransferInvoice> list = new List<ItemTransferInvoice>();
            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());

            //#################
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetConsumptionItems", parameters);

            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    list.Add(JsonConvert.DeserializeObject<ItemTransferInvoice>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return list;

        }
        #endregion

        //التوصيل
        #region Delivery
        public async Task<List<OrderPreparingSTS>> GetDelivery(int mainBranchId, int userId)
        {
            List<OrderPreparingSTS> items = new List<OrderPreparingSTS>();

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("mainBranchId", mainBranchId.ToString());
            parameters.Add("userId", userId.ToString());
            IEnumerable<Claim> claims = await APIResult.getList("Statistics/GetDelivery", parameters);
            foreach (Claim c in claims)
            {
                if (c.Type == "scopes")
                {
                    items.Add(JsonConvert.DeserializeObject<OrderPreparingSTS>(c.Value, new IsoDateTimeConverter { DateTimeFormat = "dd/MM/yyyy" }));
                }
            }
            return items;
        }
        #endregion
        // Combo
        #region combo

        public class itemCombo
        {
            private int itemId;
            private string itemName;
            private int branchId;
            public int ItemId { get => itemId; set => itemId = value; }
            public string ItemName { get => itemName; set => itemName = value; }
            public int BranchId { get => branchId; set => branchId = value; }
        }
        public class ExternalitemCombo
        {
            private int? itemId;
            private string itemName;
            private int? branchId;

            public int? ItemId { get => itemId; set => itemId = value; }
            public string ItemName { get => itemName; set => itemName = value; }
            public int? BranchId { get => branchId; set => branchId = value; }
        }
        public List<itemCombo> getItemCombo(List<Storage> ITInvoice)
        {
            List<itemCombo> iulist = new List<itemCombo>();

            iulist = ITInvoice.Select(g => new itemCombo { ItemId = g.itemId, ItemName = g.itemName, BranchId = g.branchId }).ToList();
            return iulist;

        }

        public List<ExternalitemCombo> getExternalItemCombo(List<ItemTransferInvoice> ITInvoice)
        {
            List<ExternalitemCombo> iulist = new List<ExternalitemCombo>();

            iulist = ITInvoice.Select(g => new ExternalitemCombo { ItemId = g.itemId, ItemName = g.itemName, BranchId = g.branchId }).ToList();
            return iulist;

        }

        public class unitCombo
        {
            private int unitId;
            private string unitName;
            private int itemId;
            private int branchId;
            public int UnitId { get => unitId; set => unitId = value; }
            public string UnitName { get => unitName; set => unitName = value; }
            public int ItemId { get => itemId; set => itemId = value; }
            public int BranchId { get => branchId; set => branchId = value; }
        }
        public class ExternalUnitCombo
        {
            private int? branchId;
            private int? unitId;
            private string unitName;
            private int? itemId;

            public int? UnitId { get => unitId; set => unitId = value; }
            public string UnitName { get => unitName; set => unitName = value; }
            public int? ItemId { get => itemId; set => itemId = value; }
            public int? BranchId { get => branchId; set => branchId = value; }
        }


        public List<unitCombo> getUnitCombo(List<Storage> ITInvoice)
        {
            List<unitCombo> iulist = new List<unitCombo>();
            iulist = ITInvoice.Select(g => new unitCombo { BranchId = g.branchId, UnitId = g.unitId, UnitName = g.unitName, ItemId = g.itemId }).ToList();
            return iulist;
        }

        public List<ExternalUnitCombo> getExternalUnitCombo(List<ItemTransferInvoice> ITInvoice)
        {
            List<ExternalUnitCombo> iulist = new List<ExternalUnitCombo>();
            iulist = ITInvoice.Select(g => new ExternalUnitCombo { BranchId = g.branchId, UnitId = g.unitId, UnitName = g.unitName, ItemId = g.itemId }).ToList();
            return iulist;
        }
        public class sectionCombo
        {
            private int sectionId;
            private string sectionName;
            private int branchId;
            public int SectionId { get => sectionId; set => sectionId = value; }
            public string SectionName { get => sectionName; set => sectionName = value; }
            public int BranchId { get => branchId; set => branchId = value; }
        }

        public List<sectionCombo> getSectionCombo(List<Storage> ITInvoice)
        {
            List<sectionCombo> iulist = new List<sectionCombo>();

            iulist = ITInvoice.Select(g => new sectionCombo { SectionId = (int)g.sectionId, SectionName = g.Secname, BranchId = g.branchId }).ToList();
            return iulist;

        }

        public class locationCombo
        {
            private int locationId;
            private string locationName;
            private int sectionId;
            private int branchId;
            public int LocationId { get => locationId; set => locationId = value; }
            public string LocationName { get => locationName; set => locationName = value; }
            public int SectionId { get => sectionId; set => sectionId = value; }
            public int BranchId { get => branchId; set => branchId = value; }
        }

        public List<locationCombo> getLocationCombo(List<Storage> ITInvoice)
        {
            List<locationCombo> iulist = new List<locationCombo>();

            iulist = ITInvoice.Select(g => new locationCombo { BranchId = g.branchId, LocationId = g.locationId, LocationName = g.LoactionName, SectionId = g.sectionId }).ToList();
            return iulist;

        }


        public class AgentTypeCombo
        {
            private string agentType;
            private int? branchId;

            public int? BranchId { get => branchId; set => branchId = value; }
            public string AgentType { get => agentType; set => agentType = value; }
        }

        public List<AgentTypeCombo> GetExternalAgentTypeCombos(List<ItemTransferInvoice> ITInvoice)
        {
            List<AgentTypeCombo> iulist = new List<AgentTypeCombo>();

            iulist = ITInvoice.Select(g => new AgentTypeCombo { AgentType = g.agentType, BranchId = g.branchId }).ToList();
            return iulist;

        }

        public class AgentCombo
        {
            private int? agentId;
            private string agentName;
            private int? branchId;
            private string agentType;

            public int? AgentId { get => agentId; set => agentId = value; }
            public int? BranchId { get => branchId; set => branchId = value; }
            public string AgentName { get => agentName; set => agentName = value; }
            public string AgentType { get => agentType; set => agentType = value; }
        }

        public List<AgentCombo> GetExternalAgentCombos(List<ItemTransferInvoice> ITInvoice)
        {
            List<AgentCombo> iulist = new List<AgentCombo>();

            iulist = ITInvoice.Select(g => new AgentCombo { AgentId = g.agentId, AgentName = g.agentName, BranchId = g.branchId, AgentType = g.agentType }).ToList();
            return iulist;

        }

        public class InvTypeCombo
        {
            private string invoiceType;
            private int? branchId;

            public string InvoiceType { get => invoiceType; set => invoiceType = value; }
            public int? BranchId { get => branchId; set => branchId = value; }
        }

        public List<InvTypeCombo> GetExternalInvoiceTypeCombos(List<ItemTransferInvoice> ITInvoice)
        {
            List<InvTypeCombo> iulist = new List<InvTypeCombo>();

            iulist = ITInvoice.Select(g => new InvTypeCombo { InvoiceType = g.invType, BranchId = g.branchId }).ToList();
            return iulist;

        }
        public class InvCombo
        {
            private int invoiceId;
            private string invoiceNumber;
            private int? branchId;
            private string invoiceType;

            public int InvoiceId { get => invoiceId; set => invoiceId = value; }
            public string InvoiceNumber { get => invoiceNumber; set => invoiceNumber = value; }
            public int? BranchId { get => branchId; set => branchId = value; }
            public string InvoiceType { get => invoiceType; set => invoiceType = value; }
        }

        public List<InvCombo> GetExternalInvoiceCombos(List<ItemTransferInvoice> ITInvoice)
        {
            List<InvCombo> iulist = new List<InvCombo>();

            iulist = ITInvoice.Select(g => new InvCombo { InvoiceId = g.invoiceId, InvoiceNumber = g.invNumber, BranchId = g.branchId, InvoiceType = g.invType }).ToList();
            return iulist;

        }
        public class internalTypeCombo
        {//type
            private int? branchId;
            private string invType;

            public int? BranchId { get => branchId; set => branchId = value; }
            //  public string InvType { get => invType; set => invType = value; }
            public string InvType { get => invType; set => invType = value; }
            public string trInvType { get; set; }
            //public string InvType
            //{
            //    get => invType == "ex" ? invType = AppSettings.resourcemanager.GetString("trExport")
            //         : invType == "im" ? invType = AppSettings.resourcemanager.GetString("trImport")
            //         : "";

            //    set => invType = value;
            //}
        }


        public List<internalTypeCombo> getTypeCompo(List<ItemTransferInvoice> ITInvoice)
        {
            List<internalTypeCombo> iulist = new List<internalTypeCombo>();
            iulist = ITInvoice.Select(g => new internalTypeCombo { BranchId = g.branchId, InvType = g.invType }).ToList();
            return iulist;
        }
        public class internalOperatorCombo
        {
            private int? branchId;
            private string invNum;

            public int? BranchId { get => branchId; set => branchId = value; }
            public string InvNum { get => invNum; set => invNum = value; }
        }


        public List<internalOperatorCombo> getOperatroCompo(List<ItemTransferInvoice> ITInvoice)
        {
            List<internalOperatorCombo> iulist = new List<internalOperatorCombo>();
            iulist = ITInvoice.Select(g => new internalOperatorCombo { BranchId = g.branchId, InvNum = g.invNumber }).ToList();
            return iulist;
        }
        public class StocktakingArchivesTypeCombo
        {//stocktype
            private int? branchId;
            private string inventoryType;
            private string inventoryTypeText;

            public int? BranchId { get => branchId; set => branchId = value; }
            public string InventoryType { get => inventoryType; set => inventoryType = value; }
            public string InventoryTypeText
            {
                get => inventoryType == "a" ? inventoryTypeText = AppSettings.resourcemanager.GetString("trArchived")
                     : inventoryType == "n" ? inventoryTypeText = AppSettings.resourcemanager.GetString("trSaved")
                     : inventoryType == "d" ? inventoryTypeText = AppSettings.resourcemanager.GetString("trDraft")
                     : "";

                set => inventoryTypeText = value;
            }
        }


        public List<StocktakingArchivesTypeCombo> getStocktakingArchivesTypeCombo(List<InventoryClass> ITInvoice)
        {
            List<StocktakingArchivesTypeCombo> iulist = new List<StocktakingArchivesTypeCombo>();
            iulist = ITInvoice.Select(g => new StocktakingArchivesTypeCombo { BranchId = g.branchId, InventoryType = g.inventoryType }).ToList();
            return iulist;
        }
        public class DestroiedCombo
        {
            private int? branchId;
            private string itemsUnits;
            private int? itemsUnitsId;

            public int? BranchId { get => branchId; set => branchId = value; }
            public string ItemsUnits { get => itemsUnits; set => itemsUnits = value; }
            public int? ItemsUnitsId { get => itemsUnitsId; set => itemsUnitsId = value; }
        }


        public List<DestroiedCombo> getDestroiedCombo(List<ItemTransferInvoice> ITInvoice)
        {
            List<DestroiedCombo> iulist = new List<DestroiedCombo>();
            iulist = ITInvoice.Select(g => new DestroiedCombo { BranchId = g.branchId, ItemsUnitsId = g.itemUnitId, ItemsUnits = g.ItemUnits }).ToList();
            return iulist;
        }
        public class ShortFalls
        {
            private int? branchId;
            private string itemsUnits;
            private int? itemsUnitsId;

            public int? BranchId { get => branchId; set => branchId = value; }
            public string ItemsUnits { get => itemsUnits; set => itemsUnits = value; }
            public int? ItemsUnitsId { get => itemsUnitsId; set => itemsUnitsId = value; }
        }


        public List<ShortFalls> getshortFalls(List<ItemTransferInvoice> ITInvoice)
        {
            List<ShortFalls> iulist = new List<ShortFalls>();
            iulist = ITInvoice.Select(g => new ShortFalls { BranchId = g.branchId, ItemsUnitsId = g.itemUnitId, ItemsUnits = g.ItemUnits }).ToList();
            return iulist;
        }


        #endregion



        public List<CashTransferSts> getstate(List<CashTransferSts> list, int tab, List<CashTransferSts> listAll)
        {
            List<CashTransferSts> list2 = new List<CashTransferSts>();
            IEnumerable<CashTransferSts> temp = list;
            //if (tab == 1)
            //{
            //    temp = list.Where(t => (t.shippingCompanyId == null && t.shipUserId == null && t.invAgentId != null) ||
            //                              (t.shippingCompanyId != null && t.shipUserId != null && t.invAgentId != null));
            //}
            //else if (tab == 3)
            //{
            //    temp = list.Where(t => (t.shippingCompanyId != null && t.shipUserId == null && t.invAgentId != null)
            //                         ||
            //                         (t.shippingCompanyId != null && t.shipUserId == null && t.invAgentId == null)
            //    );
            //}
            if (tab == 1)
            {
                temp = list.Where(t => (t.invShippingCompanyId == null && t.shipUserId == null && t.invAgentId != null) ||
                                          (t.invShippingCompanyId != null && t.shipUserId != null && t.invAgentId != null));
            }
            else if (tab == 3)
            {
                temp = list.Where(t => (t.shippingCompanyId != null && t.shipUserId == null && t.invAgentId != null)
                                     ||
                                     (t.shippingCompanyId != null && t.shipUserId == null && t.invAgentId == null)
                );
            }

            list2 = temp.OrderBy(X => X.updateDate).GroupBy(obj => obj.transNum).Select(obj => new CashTransferSts
            {
                bondIsRecieved = obj.FirstOrDefault().bondIsRecieved,
                //processType = obj.FirstOrDefault().processType,
                processType = (obj.FirstOrDefault().processType == "doc" && obj.FirstOrDefault().bondIsRecieved == 1)
                ? (listAll.Where(x => x.bondId == obj.FirstOrDefault().bondId && x.side == "bnd").ToList().Count > 0
                ? listAll.Where(x => x.bondId == obj.FirstOrDefault().bondId && x.side == "bnd").FirstOrDefault().processType : "-")
                : obj.FirstOrDefault().processType,
                bondNumber = obj.FirstOrDefault().bondNumber,
                userId = obj.FirstOrDefault().userId,
                agentId = obj.FirstOrDefault().agentId,
                bondId = obj.FirstOrDefault().bondId,
                transNum = obj.FirstOrDefault().transNum,
                updateDate = obj.FirstOrDefault().updateDate,

                bankName = obj.FirstOrDefault().bankName,
                agentName = obj.FirstOrDefault().agentName,
                usersName = obj.FirstOrDefault().usersName,
                usersLName = obj.FirstOrDefault().usersLName,
                posName = obj.FirstOrDefault().posName,

                updateUserName = obj.FirstOrDefault().updateUserName,
                updateUserAcc = obj.FirstOrDefault().updateUserAcc,
                //cardName = obj.FirstOrDefault().cardName,
                // get pay type from other trans row of bond
                cardName = (obj.FirstOrDefault().processType == "doc" && obj.FirstOrDefault().bondIsRecieved == 1)
                ? (listAll.Where(x => x.bondId == obj.FirstOrDefault().bondId && x.side == "bnd").ToList().Count > 0
                ? listAll.Where(x => x.bondId == obj.FirstOrDefault().bondId && x.side == "bnd").FirstOrDefault().cardName : "-")
                : obj.FirstOrDefault().cardName,

                bondDeserveDate = obj.FirstOrDefault().bondDeserveDate,
                docNum = obj.FirstOrDefault().docNum,
                shippingCompanyId = obj.FirstOrDefault().shippingCompanyId,
                shippingCompanyName = obj.FirstOrDefault().shippingCompanyName,
                userAcc = obj.FirstOrDefault().userAcc,

                cashTransId = obj.FirstOrDefault().cashTransId,
                transType = obj.FirstOrDefault().transType,
                desc = obj.FirstOrDefault().desc,
                invId = obj.FirstOrDefault().invId,
                cash = obj.Sum(x => x.cash),
                cashTotal = 0,
                side = obj.FirstOrDefault().side,

                //invNumber = "",
                invNumber = obj.FirstOrDefault().invNumber,
                invType = obj.FirstOrDefault().invType,
                totalNet = obj.FirstOrDefault().totalNet,

                invShippingCompanyId = obj.FirstOrDefault().invShippingCompanyId,
                invShippingCompanyName = obj.FirstOrDefault().invShippingCompanyName,
                shipUserId = obj.FirstOrDefault().shipUserId,
                invAgentId = obj.FirstOrDefault().invAgentId,
                invAgentName = obj.FirstOrDefault().invAgentName,

                Description = obj.FirstOrDefault().Description,

                Description1 = obj.FirstOrDefault().Description1,

                Description3 = obj.FirstOrDefault().Description3,

            }).Where(t => !(t.side == "bnd" && t.bondIsRecieved == 1)).ToList();
            decimal rowtotal = 0;

            foreach (CashTransferSts row in list2)
            {
                row.Description2 = row.bondId > 0
                ?
               (row.bondIsRecieved == 0 ?
                   AppSettings.resourcemanager.GetString("trBondNotRecieved") :
                  AppSettings.resourcemanager.GetString("trBondRecieved") + "-" + getProcessType(row.processType, row.cardName))
                 :
                  ((row.side == "c") && (row.invShippingCompanyId != null) && (row.processType == "inv") ?
                                                                  row.Description1 + " + " + AppSettings.resourcemanager.GetString("trDeliveryCost")
                                                                : row.Description1);

                row.BIsReceived = row.bondId > 0
               ? ((row.bondIsRecieved == 0 && row.transType == "d") || (row.bondIsRecieved == 0 && row.transType == "p") ?
                   "0" :
                   "1")
                  :
                  "2";

                if (row.transType == "d" && !(row.processType == "doc" && row.bondIsRecieved != 1) && row.side != "mb")
                {
                    rowtotal += (decimal)row.cash;
                }
                else if (row.transType == "p" && !(row.processType == "doc" && row.bondIsRecieved != 1) && row.side != "mb")
                {// p
                    rowtotal -= (decimal)row.cash;
                }
                row.cashTotal = rowtotal;


            }

            return list2;


        }

        private string getProcessType(string value, string name)
        {
            switch (value)
            {
                case "cash": return AppSettings.resourcemanager.GetString("trCash");
                case "doc": return AppSettings.resourcemanager.GetString("trDocument");
                case "cheque": return AppSettings.resourcemanager.GetString("trCheque");
                case "balance": return AppSettings.resourcemanager.GetString("trCredit");
                //case "card": return AppSettings.resourcemanager.GetString("trAnotherPaymentMethods");
                case "card": return name;
                case "inv": return AppSettings.resourcemanager.GetString("trInv");
                default: return value;
            }
        }

        private string getProcessType(string value)
        {
            switch (value)
            {
                case "cash": return AppSettings.resourcemanager.GetString("trCash");
                case "doc": return AppSettings.resourcemanager.GetString("trDocument");
                case "cheque": return AppSettings.resourcemanager.GetString("trCheque");
                case "balance": return AppSettings.resourcemanager.GetString("trCredit");
                case "card": return AppSettings.resourcemanager.GetString("trAnotherPaymentMethods");
                case "inv": return AppSettings.resourcemanager.GetString("trInv");
                default: return value;
            }
        }

    }
}
