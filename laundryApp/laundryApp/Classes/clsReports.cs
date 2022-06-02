using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using laundryApp.View.storage;
using laundryApp.Classes.ApiClasses;
using Newtonsoft.Json;
using System.Windows.Threading;
using System.Collections.Specialized;
using Microsoft.Win32;
//laundryApp.Classes
namespace laundryApp.Classes
{
    class clsReports
    {
        public static void setReportLanguage(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("lang", AppSettings.Reportlang));

        }

        public static void Header(List<ReportParameter> paramarr)
        {

            ReportCls rep = new ReportCls();
            paramarr.Add(new ReportParameter("companyName", AppSettings.companyName));
            paramarr.Add(new ReportParameter("Fax", AppSettings.Fax));
            paramarr.Add(new ReportParameter("Tel", AppSettings.Mobile));
            paramarr.Add(new ReportParameter("Address", AppSettings.Address));
            paramarr.Add(new ReportParameter("Email", AppSettings.Email));
            paramarr.Add(new ReportParameter("logoImage", "file:\\" + rep.GetLogoImagePath()));
            paramarr.Add(new ReportParameter("show_header", AppSettings.show_header));
        }
        public static void HeaderNoLogo(List<ReportParameter> paramarr)
        {

            ReportCls rep = new ReportCls();
            paramarr.Add(new ReportParameter("companyName", AppSettings.companyName));
            paramarr.Add(new ReportParameter("Fax", AppSettings.Fax));
            paramarr.Add(new ReportParameter("Tel", AppSettings.Mobile));
            paramarr.Add(new ReportParameter("Address", AppSettings.Address));
            paramarr.Add(new ReportParameter("Email", AppSettings.Email));


        }
        public static void bankdg(List<ReportParameter> paramarr)
        {


            paramarr.Add(new ReportParameter("trTransferNumber", AppSettings.resourcemanagerreport.GetString("trTransferNumberTooltip")));


        }
        public static void bondsDocReport(LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            DateFormConv(paramarr);

        }
        //public static void bondsReport(IEnumerable<Bonds> bondsQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();

        //    paramarr.Add(new ReportParameter("trDocNumTooltip", AppSettings.resourcemanagerreport.GetString("trDocNumTooltip")));
        //    paramarr.Add(new ReportParameter("trRecipientTooltip", AppSettings.resourcemanagerreport.GetString("trRecipientTooltip")));

        //    paramarr.Add(new ReportParameter("trPaymentTypeTooltip", AppSettings.resourcemanagerreport.GetString("trPaymentTypeTooltip")));

        //    paramarr.Add(new ReportParameter("trDocDateTooltip", AppSettings.resourcemanagerreport.GetString("trDocDateTooltip")));

        //    paramarr.Add(new ReportParameter("trPayDate", AppSettings.resourcemanagerreport.GetString("trPayDate")));
        //    paramarr.Add(new ReportParameter("trCashTooltip", AppSettings.resourcemanagerreport.GetString("trCashTooltip")));

        //    foreach (var c in bondsQuery)
        //    {

        //        c.amount = decimal.Parse(HelpClass.DecTostring(c.amount));
        //    }
        //    rep.DataSources.Add(new ReportDataSource("DataSetBond", bondsQuery));

        //    DateFormConv(paramarr);
        //    AccountSideConv(paramarr);
        //    cashTransTypeConv(paramarr);

        //}


        //public static void orderReport(IEnumerable<Invoice> invoiceQuery, LocalReport rep, string reppath)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    foreach(var o in invoiceQuery)
        //    {
        //        string status = "";
        //        switch (o.status)
        //        {
        //            case "tr":
        //                status = AppSettings.resourcemanager.GetString("trDelivered");
        //                break;
        //            case "rc":
        //                status = AppSettings.resourcemanager.GetString("trInDelivery");
        //                break;
        //            default:
        //                status = "";
        //                break;
        //        }
        //        o.status = status;
        //        o.deserved = decimal.Parse(HelpClass.DecTostring(o.deserved));
        //    }
        //    rep.DataSources.Add(new ReportDataSource("DataSetInvoice", invoiceQuery));
        //}
        public static void orderReport(IEnumerable<Invoice> invoiceQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            foreach (var o in invoiceQuery)
            {
                o.deserved = decimal.Parse(HelpClass.DecTostring(o.deserved));
                o.payStatus = invoicePayStatusConvert(o.payStatus);
            }
            DeliverStateConv(paramarr);

            paramarr.Add(new ReportParameter("trInvoiceNumber", AppSettings.resourcemanagerreport.GetString("trInvoiceNumber")));
            paramarr.Add(new ReportParameter("trSalesMan", AppSettings.resourcemanagerreport.GetString("trSalesMan")));
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trCashTooltip", AppSettings.resourcemanagerreport.GetString("trCashTooltip")));
            paramarr.Add(new ReportParameter("trState", AppSettings.resourcemanagerreport.GetString("trState")));

            DateFormConv(paramarr);


            rep.DataSources.Add(new ReportDataSource("DataSetInvoice", invoiceQuery));
        }

        public static string invoicePayStatusConvert(string payStatus)
        {

            switch (payStatus)
            {
                case "payed": return AppSettings.resourcemanagerreport.GetString("trPaid_");

                case "unpayed": return AppSettings.resourcemanagerreport.GetString("trUnPaid");

                case "partpayed": return AppSettings.resourcemanagerreport.GetString("trCredit");

                default: return "";

            }
        }
        public static void DeliverStateConv(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("trDelivered", AppSettings.resourcemanagerreport.GetString("trDelivered")));
            paramarr.Add(new ReportParameter("trInDelivery", AppSettings.resourcemanagerreport.GetString("trInDelivery")));

        }

        public static void bankAccReport(IEnumerable<CashTransfer> cash, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trBankAccounts")));
            paramarr.Add(new ReportParameter("trTransferNumberTooltip", AppSettings.resourcemanagerreport.GetString("trTransferNumberTooltip")));
            paramarr.Add(new ReportParameter("trBank", AppSettings.resourcemanagerreport.GetString("trBank")));
            paramarr.Add(new ReportParameter("trDepositeNumTooltip", AppSettings.resourcemanagerreport.GetString("trDepositeNumTooltip")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trCashTooltip", AppSettings.resourcemanagerreport.GetString("trCashTooltip")));
            DateFormConv(paramarr);
            foreach (var c in cash)
            {
                ///////////////////
                c.cash = decimal.Parse(HelpClass.DecTostring(c.cash));
                string s;
                switch (c.processType)
                {
                    case "cash":
                        s = AppSettings.resourcemanagerreport.GetString("trCash");
                        break;
                    case "doc":
                        s = AppSettings.resourcemanagerreport.GetString("trDocument");
                        break;
                    case "cheque":
                        s = AppSettings.resourcemanagerreport.GetString("trCheque");
                        break;
                    case "balance":
                        s = AppSettings.resourcemanagerreport.GetString("trCredit");
                        break;
                    case "card":
                        s = AppSettings.resourcemanagerreport.GetString("trCreditCard");
                        break;
                    default:
                        s = c.processType;
                        break;
                }
                ///////////////////
                c.processType = s;
                string name = "";
                switch (c.side)
                {
                    case "bnd": break;
                    case "v": name = AppSettings.resourcemanagerreport.GetString("trVendor"); break;
                    case "c": name = AppSettings.resourcemanagerreport.GetString("trCustomer"); break;
                    case "u": name = AppSettings.resourcemanagerreport.GetString("trUser"); break;
                    case "s": name = AppSettings.resourcemanagerreport.GetString("trSalary"); break;
                    case "e": name = AppSettings.resourcemanagerreport.GetString("trGeneralExpenses"); break;
                    case "m":
                        if (c.transType == "d")
                            name = AppSettings.resourcemanagerreport.GetString("trAdministrativeDeposit");
                        if (c.transType == "p")
                            name = AppSettings.resourcemanagerreport.GetString("trAdministrativePull");
                        break;
                    case "sh": name = AppSettings.resourcemanagerreport.GetString("trShippingCompany"); break;
                    default: break;
                }
                string fullName = "";
                if (!string.IsNullOrEmpty(c.agentName))
                    fullName = name + " " + c.agentName;
                else if (!string.IsNullOrEmpty(c.usersLName))
                    fullName = name + " " + c.usersLName;
                else if (!string.IsNullOrEmpty(c.shippingCompanyName))
                    fullName = name + " " + c.shippingCompanyName;
                else
                    fullName = name;
                /////////////////////
                c.side = fullName;

                string type;
                if (c.transType.Equals("p")) type = AppSettings.resourcemanagerreport.GetString("trPull");
                else type = AppSettings.resourcemanagerreport.GetString("trDeposit");
                ////////////////////
                c.transType = type;
            }
            rep.DataSources.Add(new ReportDataSource("DataSetBankAcc", cash));
        }
       
        public static string processTypeConvswitch(string processType)
        {
            
            switch (processType)
            {
                case "cash": return AppSettings.resourcemanagerreport.GetString("trCash");
                //break;
                case "doc": return AppSettings.resourcemanagerreport.GetString("trDocument");
                //break;
                case "cheque": return AppSettings.resourcemanagerreport.GetString("trCheque");
                //break;
                case "balance": return AppSettings.resourcemanagerreport.GetString("trCredit");
                //break;
                case "card": return AppSettings.resourcemanagerreport.GetString("trAnotherPaymentMethods");
                //break;
                case "inv": return AppSettings.resourcemanagerreport.GetString("trInv");
                //break;
                default: return processType;
                    //break;
            }
        }
        public static void paymentAccReport(IEnumerable<CashTransfer> cash, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            //foreach (var c in cash)
            //{
            //    ///////////////////
            //    c.cash = decimal.Parse(SectionData.DecTostring(c.cash));
            //    string s;
            //    switch (c.processType)
            //    {
            //        case "cash":
            //            s = AppSettings.resourcemanagerreport.GetString("trCash");
            //            break;
            //        case "doc":
            //            s = AppSettings.resourcemanagerreport.GetString("trDocument");
            //            break;
            //        case "cheque":
            //            s = AppSettings.resourcemanagerreport.GetString("trCheque");
            //            break;
            //        case "balance":
            //            s = AppSettings.resourcemanagerreport.GetString("trCredit");
            //            break;
            //        default:
            //            s = c.processType;
            //            break;
            //    }


            //}


            AccountSideConv(paramarr);

            cashTransTypeConv(paramarr);
            cashTransferProcessTypeConv(paramarr);
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trPayments")));

            paramarr.Add(new ReportParameter("trTransferNumberTooltip", AppSettings.resourcemanagerreport.GetString("trTransferNumberTooltip")));
            paramarr.Add(new ReportParameter("trRecepient", AppSettings.resourcemanagerreport.GetString("trRecepient")));
            paramarr.Add(new ReportParameter("trPaymentTypeTooltip", AppSettings.resourcemanagerreport.GetString("trPaymentTypeTooltip")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trCashTooltip", AppSettings.resourcemanagerreport.GetString("trCashTooltip")));
            paramarr.Add(new ReportParameter("accuracy", AppSettings.accuracy));
            paramarr.Add(new ReportParameter("trUnKnown", AppSettings.resourcemanagerreport.GetString("trUnKnown")));
            paramarr.Add(new ReportParameter("trCashCustomer", AppSettings.resourcemanagerreport.GetString("trCashCustomer")));

            DateFormConv(paramarr);


            foreach (var c in cash)
            {

                c.cash = decimal.Parse(HelpClass.DecTostring(c.cash));
                // c.notes = SectionData.DecTostring(c.cash);
                c.agentName = AgentUnKnownConvert(c.agentId, c.side, c.agentName);

            }
            rep.DataSources.Add(new ReportDataSource("DataSetBankAcc", cash));
        }
        public static string AgentUnKnownConvert(int? agentId, string side, string agentName)
        {

            if (agentId == null)
            {
                if (side == "v")
                {
                    agentName = AppSettings.resourcemanagerreport.GetString("trUnKnown");
                }
                else if (side == "c")
                {
                    agentName = AppSettings.resourcemanagerreport.GetString("trCashCustomer");
                }
            }
            return agentName;

        }
        public static string AgentCompanyUnKnownConvert(int? agentId, string side, string agentCompany)
        {
            if (agentId == null)
            {
                agentCompany = AppSettings.resourcemanagerreport.GetString("trUnKnown");

            }
            return agentCompany;
        }


        public static void receivedAccReport(IEnumerable<CashTransfer> cash, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            foreach (var c in cash)
            {

                c.cash = decimal.Parse(HelpClass.DecTostring(c.cash));
            }
            DateFormConv(paramarr);
            AccountSideConv(paramarr);

            cashTransTypeConv(paramarr);
            cashTransferProcessTypeConv(paramarr);
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trReceived")));
            paramarr.Add(new ReportParameter("trTransferNumberTooltip", AppSettings.resourcemanagerreport.GetString("trTransferNumberTooltip")));
            paramarr.Add(new ReportParameter("trDepositor", AppSettings.resourcemanagerreport.GetString("trDepositor")));
            paramarr.Add(new ReportParameter("trPaymentTypeTooltip", AppSettings.resourcemanagerreport.GetString("trPaymentTypeTooltip")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trCashTooltip", AppSettings.resourcemanagerreport.GetString("trCashTooltip")));
            paramarr.Add(new ReportParameter("accuracy", AppSettings.accuracy));
            paramarr.Add(new ReportParameter("trUnKnown", AppSettings.resourcemanagerreport.GetString("trUnKnown")));
            paramarr.Add(new ReportParameter("trCashCustomer", AppSettings.resourcemanagerreport.GetString("trCashCustomer")));

            rep.DataSources.Add(new ReportDataSource("DataSetBankAcc", cash));
        }

        public static void SubscriptionAcc(IEnumerable<AgentMembershipCash> cash, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            foreach (var c in cash)
            {

                c.total = decimal.Parse(HelpClass.DecTostring(c.total));
                c.subscriptionTypeconv = subscriptionTypeConverter(c.subscriptionType);
                c.EndDateconv = unlimitedEndDateConverter(c.subscriptionType, c.EndDate);

            }
            DateFormConv(paramarr);
            //   cashTransTypeConv(paramarr);
            //    cashTransferProcessTypeConv(paramarr);
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trSubscriptions")));
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
            paramarr.Add(new ReportParameter("trSubscriptionType", AppSettings.resourcemanagerreport.GetString("trSubscriptionType")));
            paramarr.Add(new ReportParameter("trTransferNumberTooltip", AppSettings.resourcemanagerreport.GetString("trTransferNumberTooltip")));
            paramarr.Add(new ReportParameter("trDepositor", AppSettings.resourcemanagerreport.GetString("trDepositor")));
            paramarr.Add(new ReportParameter("trPaymentTypeTooltip", AppSettings.resourcemanagerreport.GetString("trPaymentTypeTooltip")));
            paramarr.Add(new ReportParameter("trExpireDate", AppSettings.resourcemanagerreport.GetString("trExpireDate")));
            paramarr.Add(new ReportParameter("trAmount", AppSettings.resourcemanagerreport.GetString("trAmount")));
            paramarr.Add(new ReportParameter("accuracy", AppSettings.accuracy));

            rep.DataSources.Add(new ReportDataSource("DataSetBankAcc", cash));
        }
        public static string subscriptionTypeConverter(string subscriptionType)
        {

            switch (subscriptionType)
            {
                case "f": return AppSettings.resourcemanagerreport.GetString("trFree");

                case "m": return AppSettings.resourcemanagerreport.GetString("trMonthly");

                case "y": return AppSettings.resourcemanagerreport.GetString("trYearly");

                case "o": return AppSettings.resourcemanagerreport.GetString("trOnce");

                default: return AppSettings.resourcemanagerreport.GetString("");

            }
        }

        public static string unlimitedEndDateConverter(string subscriptionType, DateTime? EndDate)
        {
            if (subscriptionType != null && EndDate != null)
            {
                string sType = subscriptionType;
                DateTime sDate = (DateTime)EndDate;

                if (sType == "o" || sType == "f")
                    return AppSettings.resourcemanager.GetString("trUnlimited");
                else
                {


                    switch (AppSettings.dateFormat)
                    {
                        case "ShortDatePattern":
                            return sDate.ToString(@"dd/MM/yyyy");
                        case "LongDatePattern":
                            return sDate.ToString(@"dddd, MMMM d, yyyy");
                        case "MonthDayPattern":
                            return sDate.ToString(@"MMMM dd");
                        case "YearMonthPattern":
                            return sDate.ToString(@"MMMM yyyy");
                        default:
                            return sDate.ToString(@"dd/MM/yyyy");
                    }

                }
            }
            else if (subscriptionType == "o" || subscriptionType == "f")
            {
                return AppSettings.resourcemanager.GetString("trUnlimited");
            }
            else return "";
        }
        public static void posAccReport(IEnumerable<CashTransfer> cash, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var c in cash)
            {

                c.cash = decimal.Parse(HelpClass.DecTostring(c.cash));
            }

            paramarr.Add(new ReportParameter("trTransferNumberTooltip", AppSettings.resourcemanagerreport.GetString("trTransferNumberTooltip")));
            paramarr.Add(new ReportParameter("trCreator", AppSettings.resourcemanagerreport.GetString("trCreator")));
            paramarr.Add(new ReportParameter("trStatus", AppSettings.resourcemanagerreport.GetString("trStatus")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trCashTooltip", AppSettings.resourcemanagerreport.GetString("trCashTooltip")));
            paramarr.Add(new ReportParameter("trConfirmed", AppSettings.resourcemanagerreport.GetString("trConfirmed")));
            paramarr.Add(new ReportParameter("trCanceled", AppSettings.resourcemanagerreport.GetString("trCanceled")));
            paramarr.Add(new ReportParameter("trWaiting", AppSettings.resourcemanagerreport.GetString("trWaiting")));

            DateFormConv(paramarr);

            rep.DataSources.Add(new ReportDataSource("DataSetBankAcc", cash));
        }

        public static void posAccReportSTS(IEnumerable<CashTransfer> cash, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            posAccReport(cash, rep, reppath, paramarr);
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));

            paramarr.Add(new ReportParameter("trAccoutant", AppSettings.resourcemanagerreport.GetString("trAccoutant")));
            paramarr.Add(new ReportParameter("trAmount", AppSettings.resourcemanagerreport.GetString("trAmount")));



        }
        public string posTransfersStatusConverter(byte isConfirm1, byte isConfirm2)
        {

            if ((isConfirm1 == 1) && (isConfirm2 == 1))
                return AppSettings.resourcemanager.GetString("trConfirmed");
            else if ((isConfirm1 == 2) || (isConfirm2 == 2))
                return AppSettings.resourcemanager.GetString("trCanceled");
            else
                return AppSettings.resourcemanager.GetString("trWaiting");
        }



        public static void invItem(IEnumerable<InventoryItemLocation> itemLocations, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            rep.DataSources.Add(new ReportDataSource("DataSetInvItemLocation", itemLocations));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
        }
        public static void section(IEnumerable<Section> sections, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetSection", sections));
        }
        public static void location(IEnumerable<Location> locations, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetLocation", locations));
        }
        public static void itemLocation(IEnumerable<ItemLocation> itemLocations, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetItemLocation", itemLocations));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
        }
        public static void bankReport(IEnumerable<Bank> banksQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetBank", banksQuery));
        }
        public static void tablesReport(IEnumerable<Tables> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetTables", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trTheTables")));
            //table columns
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trPersonsCount", AppSettings.resourcemanagerreport.GetString("trPersonsCount")));
            paramarr.Add(new ReportParameter("trSection", AppSettings.resourcemanagerreport.GetString("trSection")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));
            paramarr.Add(new ReportParameter("trBranchStore", AppSettings.resourcemanagerreport.GetString("trBranch/Store")));

             
        }
    
               public static void reservationsUpdateReport(IEnumerable<TablesReservation> Query1, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<TablesReservation> Query = JsonConvert.DeserializeObject<List<TablesReservation>>(JsonConvert.SerializeObject(Query1));

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            foreach (TablesReservation r in Query)
            {
               // r.reservationTime = DateTime.Parse(timeFrameConv((DateTime)r.reservationTime));
                r.isExceed = ExceedConv(r.isExceed);
                r.notes = timeFrameConv((DateTime)r.reservationTime);


            }

            rep.DataSources.Add(new ReportDataSource("DataSetTables", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("tr_Sales")+" / "+ AppSettings.resourcemanagerreport.GetString("reservationsManagement")));
     
            //table columns
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trStartTime", AppSettings.resourcemanagerreport.GetString("trStartTime")));
            paramarr.Add(new ReportParameter("trCount", AppSettings.resourcemanagerreport.GetString("trCount")));
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
            paramarr.Add(new ReportParameter("trExceed", AppSettings.resourcemanagerreport.GetString("trExceed")));
            DateFormConv(paramarr);

        }
        public static string ExceedConv(string isExceed)
        {
            switch (isExceed)
            {
                // used in reservation update to know if reservation exceed warning Time For Late
                case "exceed":
                    isExceed = AppSettings.resourcemanagerreport.GetString("trExceed");
                    break;

                case "":
                    isExceed = "-";
                    break;
            }
            return isExceed;
        }
        // timeFrameConverter
        public static  string timeFrameConv(DateTime date)
        {


        //    DateTimeFormatInfo dtfi = DateTimeFormatInfo.CurrentInfo;
       
            if (!(date is  DateTime))
                return date.ToString();


            switch (AppSettings.timeFormat)
            {
                case "ShortTimePattern":
                    return date.ToShortTimeString();
                case "LongTimePattern":
                    return date.ToLongTimeString();
                default:
                    return date.ToShortTimeString();
            }

        }
        public static void hallSectionsReport(IEnumerable<HallSection> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetHallSections", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trSections")));
            //table columns
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trDetails", AppSettings.resourcemanagerreport.GetString("trDetails")));
            paramarr.Add(new ReportParameter("trBranchStore", AppSettings.resourcemanagerreport.GetString("trBranch/Store")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));

        }

        public static void CustomerReport(IEnumerable<Agent> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetAgent", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trCustomers")));
            //table columns
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trMobile", AppSettings.resourcemanagerreport.GetString("trMobile")));

        }

        public static void VendorReport(IEnumerable<Agent> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetAgent", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trVendors")));
            //table columns
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trMobile", AppSettings.resourcemanagerreport.GetString("trMobile")));

        }

        public static void UserReport(IEnumerable<User> Query1, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<User> Query = JsonConvert.DeserializeObject<List<User>>(JsonConvert.SerializeObject(Query1));

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trUsers")));
            //table columns
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trJob", AppSettings.resourcemanagerreport.GetString("trJob")));
            paramarr.Add(new ReportParameter("trWorkHours", AppSettings.resourcemanagerreport.GetString("trWorkHours")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));

            foreach (User row in Query)
            {
                row.job = UserJobConv(row.job);
            }
            rep.DataSources.Add(new ReportDataSource("DataSetUser", Query));
        }
        public static string UserJobConv(string job)
        {
            if (job != null)
            {
                string s = job.ToString();
                if (FillCombo.UserJobListReport is null)
                    FillCombo.RefreshUserJobsReport();
                keyValueString keyValueString = FillCombo.UserJobListReport.Where(x => x.key == s).FirstOrDefault();

                return keyValueString.value;
            }
            else return "";
        }


        public static void deliveryManagement(IEnumerable<Invoice> Query1, LocalReport rep, string reppath, List<ReportParameter> paramarr,int isdriver)
        {
            List<Invoice> Query = JsonConvert.DeserializeObject<List<Invoice>>(JsonConvert.SerializeObject(Query1));
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();


            //table columns
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trInvoiceCharp")));
            paramarr.Add(new ReportParameter("deliveryTime", AppSettings.resourcemanagerreport.GetString("deliveryTime")));
            paramarr.Add(new ReportParameter("trStatus", AppSettings.resourcemanagerreport.GetString("trStatus")));
            if (isdriver==1)
            {
                paramarr.Add(new ReportParameter("deliveryMan", AppSettings.resourcemanagerreport.GetString("deliveryMan")));
            }
            else
            {
                paramarr.Add(new ReportParameter("deliveryMan", AppSettings.resourcemanagerreport.GetString("trCompany")));
            }
            foreach (var row in Query)
            {
                row.status = preparingOrderStatusConvert(row.status);
                row.orderTimeConv = dateTimeToTimeConvert(row.orderTime);
                row.shipUserName = driverOrShipcompanyConvert(isdriver, row.shipUserName, row.shipUserLastName, row.shippingCompanyName);
            } 
               
            

            rep.DataSources.Add(new ReportDataSource("DataSet", Query));

            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trDeliveryManagement")));

        }
        public static string dateTimeToTimeConvert(DateTime? orderTime)
        {
            if (orderTime != null)
            {
                DateTime dt = (DateTime)orderTime;
                return dt.ToShortTimeString();
            }
            else
                return "-";
        }
        public static string driverOrShipcompanyConvert(int isDriver,string shipUserName,string shipUserLastName,string shippingCompanyName)
        {
            string name = "";
            if (isDriver == 1)
            {
                name = shipUserName + " " + shipUserLastName;
            }
            else
            {
                name = shippingCompanyName;
            }
              
            return name;
        }

        public static void driverManagement(List<Invoice> Query1, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            List<Invoice> Query = JsonConvert.DeserializeObject<List<Invoice>>(JsonConvert.SerializeObject(Query1));

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            //table columns
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trInvoiceCharp")));

            paramarr.Add(new ReportParameter("deliveryTime", AppSettings.resourcemanagerreport.GetString("deliveryTime")));
            //paramarr.Add(new ReportParameter("trStatus", AppSettings.resourcemanagerreport.GetString("trStatus")));

           
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
             paramarr.Add(new ReportParameter("trCustomerAddress", AppSettings.resourcemanagerreport.GetString("trAddress")));

            paramarr.Add(new ReportParameter("trCustomerMobile", AppSettings.resourcemanagerreport.GetString("trMobile")));


            foreach (var row in Query)
            {
                //row.status = preparingOrderStatusConvert(row.status);
                row.orderTimeConv = dateTimeToTimeConvert(row.orderTime);
                row.agentAddress = agentResSectorsAddressConv(row.agentResSectorsName, row.agentAddress);
            }
            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("deliveryList")));


        }
        public static string agentResSectorsAddressConv(string agentResSectorsName, string agentAddress)
        {
            if (agentResSectorsName == "" && agentAddress == "")
            {
                agentAddress = "-";

            }
            else if ((agentResSectorsName == "" || agentAddress == ""))
            {
                agentAddress = agentResSectorsName + agentAddress;
            }
            else
            {
                agentAddress = agentResSectorsName + "-" + agentAddress;
            }
            return agentAddress;
        }
        //public static void deliveryManagdata(IEnumerable<Invoice> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{


        //}
        public static void ShippingCompanies(IEnumerable<ShippingCompanies> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trShippingCompanies")));
            //table columns
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trRealDeliveryCost", AppSettings.resourcemanagerreport.GetString("trRealDeliveryCost")));
            paramarr.Add(new ReportParameter("trDeliveryCost", AppSettings.resourcemanagerreport.GetString("trDeliveryCost")));
            paramarr.Add(new ReportParameter("trDeliveryType", AppSettings.resourcemanagerreport.GetString("trDeliveryType")));
            foreach (var row in Query)
            {

                row.realDeliveryCost = decimal.Parse(HelpClass.DecTostring(row.realDeliveryCost));
                row.deliveryCost = decimal.Parse(HelpClass.DecTostring(row.deliveryCost));
                row.deliveryType = DeliveryTypeConvert(row.deliveryType);
                //deliveryTypeConverter
            }
            rep.DataSources.Add(new ReportDataSource("DataSet", Query));

        }
        public static string DeliveryTypeConvert(string deliveryType)
        {

            switch (deliveryType)
            {
                case "local": return AppSettings.resourcemanagerreport.GetString("trLocaly");
                //break;
                case "com": return AppSettings.resourcemanagerreport.GetString("trShippingCompany");
                //break;
                default: return AppSettings.resourcemanagerreport.GetString("");
                    //break;
            }
        }
        public static string preparingOrderStatusConvert(string status)
        {
            switch (status)
            {
                case "Listed": return AppSettings.resourcemanagerreport.GetString("trListed");
                case "Preparing": return AppSettings.resourcemanagerreport.GetString("trPreparing");
                case "Ready": return AppSettings.resourcemanagerreport.GetString("trReady");
                case "Collected": return AppSettings.resourcemanagerreport.GetString("withDeliveryMan");
                case "InTheWay": return AppSettings.resourcemanagerreport.GetString("onTheWay");
                case "Done": return AppSettings.resourcemanagerreport.GetString("trDone");// gived to customer
                default: return "";
            }
        }
        public static void BranchesReport(IEnumerable<Branch> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetBranch", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trBranches")));
            //table columns

            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trMobile", AppSettings.resourcemanagerreport.GetString("trMobile")));
            paramarr.Add(new ReportParameter("trBranchAddress", AppSettings.resourcemanagerreport.GetString("trAddress")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));

        }
        public static void PosReport(IEnumerable<Pos> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetPos", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trPOSs")));
            //table columns
            paramarr.Add(new ReportParameter("trPosCode", AppSettings.resourcemanagerreport.GetString("trPosCode")));
            paramarr.Add(new ReportParameter("trPosName", AppSettings.resourcemanagerreport.GetString("trPosName")));
            paramarr.Add(new ReportParameter("trBranchName", AppSettings.resourcemanagerreport.GetString("trBranchName")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));

        }
        public static void StoresReport(IEnumerable<Branch> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetBranch", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trStores")));
            //table columns
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trMobile", AppSettings.resourcemanagerreport.GetString("trMobile")));
            paramarr.Add(new ReportParameter("trBranchAddress", AppSettings.resourcemanagerreport.GetString("trAddress")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));

        }



        public static void BanksReport(IEnumerable<Bank> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetBank", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trBanks")));
            //table columns
            paramarr.Add(new ReportParameter("trBankName", AppSettings.resourcemanagerreport.GetString("trBankName")));
            paramarr.Add(new ReportParameter("trAccNumber", AppSettings.resourcemanagerreport.GetString("trAccNumber")));
            paramarr.Add(new ReportParameter("trBankAddress", AppSettings.resourcemanagerreport.GetString("trAddress")));
            paramarr.Add(new ReportParameter("trMobile", AppSettings.resourcemanagerreport.GetString("trMobile")));

        }

        public static void CardsReport(IEnumerable<Card> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetCard", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trCards")));
            //table columns
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));

        }

        // Catalog
        public static void categoryReport(IEnumerable<Category> categoryQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            //foreach (var r in categoryQuery)
            //{
            //    r.taxes = decimal.Parse(HelpClass.PercentageDecTostring(r.taxes));
            //}
            rep.DataSources.Add(new ReportDataSource("DataSetCategory", categoryQuery));
            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trCategories")));
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trDetails", AppSettings.resourcemanagerreport.GetString("trDetails")));
        }

        public static void itemReport(IEnumerable<Item> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(JsonConvert.SerializeObject(Query));

            itemdata(items, rep, reppath, paramarr);


            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trItems")));
        }
        public static void itemCosting(IEnumerable<Item> items, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            foreach (Item r in items)
            {
                r.avgPurchasePrice = decimal.Parse(HelpClass.DecTostring(r.avgPurchasePrice));

                r.price = decimal.Parse(HelpClass.DecTostring(r.price));

                r.priceWithService = decimal.Parse(HelpClass.DecTostring(r.priceWithService));

            }


            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trPrimeCost", AppSettings.resourcemanagerreport.GetString("trPrimeCost")));
            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));
            paramarr.Add(new ReportParameter("trPriceWithService", AppSettings.resourcemanagerreport.GetString("trPriceWithService")));

            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trItemsCosting")));
            rep.DataSources.Add(new ReportDataSource("DataSetItem", items));
        }
        public static void FoodReport(IEnumerable<Item> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr, string categoryName)
        {
            List<Item> items = JsonConvert.DeserializeObject<List<Item>>(JsonConvert.SerializeObject(Query));

            string title = AppSettings.resourcemanagerreport.GetString("trFoods");
            itemdata(items, rep, reppath, paramarr);

            title = title + " / " + CategoryConv(categoryName);

            paramarr.Add(new ReportParameter("Title", title));

        }
        public static string CategoryConv(string categoryName)
        {
            if (categoryName == "appetizers")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trAppetizers");
            }
            else if (categoryName == "beverages")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trBeverages");
            }
            else if (categoryName == "fastFood")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trFastFood");
            }
            else if (categoryName == "mainCourses")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trMainCourses"); ;
            }
            else if (categoryName == "desserts")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trDesserts");
            }
            else if (categoryName == "package")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trPackages");
            }
            else if (categoryName == "RawMaterials")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trRawMaterials");
            }
            else if (categoryName == "Vegetables")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trVegetables");
            }
            else if (categoryName == "Meat")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trMeat");
            }
            else if (categoryName == "Drinks")
            {
                categoryName = AppSettings.resourcemanagerreport.GetString("trDrinks");
            }
            return categoryName;
        }
        public static void itemdata(IEnumerable<Item> items, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (Item r in items)
            {
                r.categoryName = CategoryConv( r.categoryName);
            }
            rep.DataSources.Add(new ReportDataSource("DataSetItem", items));

            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trDetails", AppSettings.resourcemanagerreport.GetString("trDetails")));
            paramarr.Add(new ReportParameter("trCategory", AppSettings.resourcemanagerreport.GetString("trCategorie")));
        }
        public static void unitReport(IEnumerable<Unit> unitQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetUnit", unitQuery));
            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trUnits")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trUnitName")));
            paramarr.Add(new ReportParameter("trNotes", AppSettings.resourcemanagerreport.GetString("trNote")));

        }
        //
        public static void LocationsReport(IEnumerable<Location> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetLocation", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trLocations")));
            //table columns
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trSection", AppSettings.resourcemanagerreport.GetString("trSection")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));
            paramarr.Add(new ReportParameter("trBranchStore", AppSettings.resourcemanagerreport.GetString("trBranch/Store")));
        }

        public static void SectionReport(IEnumerable<Section> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetSection", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trSections")));
            //table columns
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trBranchStore", AppSettings.resourcemanagerreport.GetString("trBranch/Store")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));

        }


        public static void ItemsDestructive(IEnumerable<InventoryItemLocation> invoiceItems, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetItemsDestructive", invoiceItems));
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trDestructives")));// tt
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trSection", AppSettings.resourcemanagerreport.GetString("trSection") + "-" + AppSettings.resourcemanagerreport.GetString("trLocation")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem") + "-" + AppSettings.resourcemanagerreport.GetString("trUnit")));

            paramarr.Add(new ReportParameter("trQuantity", AppSettings.resourcemanagerreport.GetString("trAmount")));
            DateFormConv(paramarr);

        }

        public static void ItemsShortage(IEnumerable<InventoryItemLocation> invoiceItems, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetItemsShortage", invoiceItems));
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trShortages")));// tt
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trInventoryNum")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trSection", AppSettings.resourcemanagerreport.GetString("trSectionLocation")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItemUnit")));

            paramarr.Add(new ReportParameter("trQuantity", AppSettings.resourcemanagerreport.GetString("trAmount")));
            DateFormConv(paramarr);

        }

        public static void Stocktaking(IEnumerable<InventoryItemLocation> invoiceItems, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetStocktaking", invoiceItems));
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trStocktakingItems")));// tt
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));
            paramarr.Add(new ReportParameter("trSectionLocation", AppSettings.resourcemanagerreport.GetString("trSectionLocation")));
            paramarr.Add(new ReportParameter("trItemUnit", AppSettings.resourcemanagerreport.GetString("trItemUnit")));
            paramarr.Add(new ReportParameter("trRealAmount", AppSettings.resourcemanagerreport.GetString("trRealAmount")));
            paramarr.Add(new ReportParameter("trInventoryAmount", AppSettings.resourcemanagerreport.GetString("trInventoryAmount")));
            paramarr.Add(new ReportParameter("trDestoryCount", AppSettings.resourcemanagerreport.GetString("trDestoryCount")));
            paramarr.Add(new ReportParameter("trInventoryNum", AppSettings.resourcemanagerreport.GetString("trInventoryNum")));
            paramarr.Add(new ReportParameter("trInventoryDate", AppSettings.resourcemanagerreport.GetString("trInventoryDate")));

            DateFormConv(paramarr);

        }

        public static void ItemsStorage(IEnumerable<ItemLocation> invoiceItems, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetItemsStorage", invoiceItems));
             paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trItemUnit", AppSettings.resourcemanagerreport.GetString("trItemUnit")));
            paramarr.Add(new ReportParameter("trSectionLocation", AppSettings.resourcemanagerreport.GetString("trSectionLocation")));
            paramarr.Add(new ReportParameter("trQuantity", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trStartDate", AppSettings.resourcemanagerreport.GetString("trStartDate")));
            paramarr.Add(new ReportParameter("trEndDate", AppSettings.resourcemanagerreport.GetString("trEndDate")));
            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));
            paramarr.Add(new ReportParameter("trOrderNum", AppSettings.resourcemanagerreport.GetString("trOrderNum")));
            DateFormConv(paramarr);

        }

        public static void StorageInvoice(IEnumerable<ItemTransfer> invoiceItems, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetStorageInvoice", invoiceItems));
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trInvoice")));// tt
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));
            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));

            DateFormConv(paramarr);

        }

        public static void StorageCosts(IEnumerable<StorageCost> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<StorageCost> invoiceItems = JsonConvert.DeserializeObject<List<StorageCost>>(JsonConvert.SerializeObject(Query));

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (StorageCost r in invoiceItems)
            {
                r.cost =decimal.Parse( HelpClass.DecTostring(r.cost));
            }
            rep.DataSources.Add(new ReportDataSource("DataSetStorageCost", invoiceItems));
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trStorageCostPerDay")));// tt
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trStorageCost", AppSettings.resourcemanagerreport.GetString("trStorageCost")));


            DateFormConv(paramarr);

        }


        public static void SpendingOrder(IEnumerable<ItemTransfer> invoiceItems, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetSpendingOrder", invoiceItems));
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trInvoice")));
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trQuantity", AppSettings.resourcemanagerreport.GetString("trQuantity")));

       //   DateFormConv(paramarr);

        }


        public static void ResidentialSectorReport(IEnumerable<ResidentialSectors> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataResidentialSectors", Query));
            //title
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trResidentialSectors")));
            //table columns

            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));

            paramarr.Add(new ReportParameter("trNote", AppSettings.resourcemanagerreport.GetString("trNote")));

        }
        public static void ErrorsReport(IEnumerable<ErrorClass> Query, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetError", Query));
        }

        public static void couponReport(IEnumerable<Coupon> CouponQuery2, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var c in CouponQuery2)
            {
                c.discountValue = decimal.Parse(accuracyDiscountConvert(c.discountValue, c.discountType));

                //c.invMin = decimal.Parse(HelpClass.DecTostring(c.invMin));
                //c.invMax = decimal.Parse(HelpClass.DecTostring(c.invMax));

                string state = "";
                //(c.isActive == 1) && ((c.endDate > DateTime.Now)||(c.endDate == null)) && ((c.quantity == 0) || (c.quantity > 0 && c.remainQ != 0))
                if ((c.isActive == 1) && ((c.endDate > DateTime.Now) || (c.endDate == null)) && ((c.quantity == 0) || (c.quantity > 0 && c.remainQ != 0)))
                    state = AppSettings.resourcemanagerreport.GetString("trValid");
                else
                    state = AppSettings.resourcemanagerreport.GetString("trExpired");

                c.state = state;

            }

            rep.DataSources.Add(new ReportDataSource("DataSetCoupon", CouponQuery2));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
            // paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trCoupons")));
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trValue", AppSettings.resourcemanagerreport.GetString("trValue")));
            paramarr.Add(new ReportParameter("trQuantity", AppSettings.resourcemanagerreport.GetString("trQuantity")));
            paramarr.Add(new ReportParameter("trRemainQuantity", AppSettings.resourcemanagerreport.GetString("trRemainQuantity")));
            paramarr.Add(new ReportParameter("trvalidity", AppSettings.resourcemanagerreport.GetString("trvalidity")));
            paramarr.Add(new ReportParameter("trUnlimited", AppSettings.resourcemanagerreport.GetString("trUnlimited")));

        }

        public static void membershipReport(IEnumerable<Memberships> membershipsQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<Memberships> Query = JsonConvert.DeserializeObject<List<Memberships>>(JsonConvert.SerializeObject(membershipsQuery));

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var c in Query)
            {
                c.subscriptionType = subscriptionTypeConverter(c.subscriptionType);
            }

            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
                paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trSubscriptionType", AppSettings.resourcemanagerreport.GetString("trSubscriptionType")));


        }
        public static void InvClassReport(IEnumerable<InvoicesClass> invoicesClassesQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<InvoicesClass> Query = JsonConvert.DeserializeObject<List<InvoicesClass>>(JsonConvert.SerializeObject(invoicesClassesQuery));

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var c in Query)
            {
               // c.subscriptionType = subscriptionTypeConverter(c.subscriptionType);
            }

            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
              paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trMinimumInvoiceValueHint", AppSettings.resourcemanagerreport.GetString("trMinInvoice")));
            paramarr.Add(new ReportParameter("trMaximumInvoiceValueHint", AppSettings.resourcemanagerreport.GetString("trMaxInvoice")));
        }


        public static void OfferReport(IEnumerable<Offer> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var c in Query)
            {
                c.discountValue = decimal.Parse(accuracyDiscountConvert(c.discountValue, byte.Parse(c.discountType)));

            }

            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
            // paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trCoupons")));
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trValue", AppSettings.resourcemanagerreport.GetString("trValue")));
            paramarr.Add(new ReportParameter("trStartDate", AppSettings.resourcemanagerreport.GetString("trStartDate")));
            paramarr.Add(new ReportParameter("trEndDate", AppSettings.resourcemanagerreport.GetString("trEndDate")));


        }

        public string unlimitedCouponConv(decimal quantity)
        {

            if (quantity == 0)
                return AppSettings.resourcemanagerreport.GetString("trUnlimited");
            else
                return quantity.ToString();
        }
        public static void couponExportReport(LocalReport rep, string reppath, List<ReportParameter> paramarr, string barcode)
        {

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            ReportCls repcls = new ReportCls();


            paramarr.Add(new ReportParameter("invNumber", barcode));
            paramarr.Add(new ReportParameter("barcodeimage", "file:\\" + repcls.BarcodeToImage(barcode, "barcode")));

        }

        public static void packageReport(IEnumerable<Item> packageQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();


            rep.DataSources.Add(new ReportDataSource("DataSetItem", packageQuery));
            //    paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trPackageItems")));
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trPackage")));
            paramarr.Add(new ReportParameter("trDetails", AppSettings.resourcemanagerreport.GetString("trDetails")));
            paramarr.Add(new ReportParameter("trCategory", AppSettings.resourcemanagerreport.GetString("trCategorie")));

        }
        public static void serviceReport(IEnumerable<Item> serviceQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();


            rep.DataSources.Add(new ReportDataSource("DataSetItem", serviceQuery));
            //    paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));trTheService trTheServices
            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trTheServices")));
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trTheService")));
            paramarr.Add(new ReportParameter("trDetails", AppSettings.resourcemanagerreport.GetString("trDetails")));
            paramarr.Add(new ReportParameter("trCategory", AppSettings.resourcemanagerreport.GetString("trCategorie")));

        }
        public static void offerReport(IEnumerable<Offer> OfferQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var o in OfferQuery)
            {

                o.discountValue = decimal.Parse(HelpClass.DecTostring(o.discountValue));
            }

            rep.DataSources.Add(new ReportDataSource("DataSetOffer", OfferQuery));
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trValue")));
            paramarr.Add(new ReportParameter("trStartDate", AppSettings.resourcemanagerreport.GetString("trStartDate")));
            paramarr.Add(new ReportParameter("trEndDate", AppSettings.resourcemanagerreport.GetString("trEndDate")));



        }
        public static void cardReport(IEnumerable<Card> cardsQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetCard", cardsQuery));
        }
        public static void shippingReport(IEnumerable<ShippingCompanies> shippingCompanies, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetShipping", shippingCompanies));
        }
        public static string itemTypeConverter(string value)
        {
            string s = "";
            switch (value)
            {
                case "n": s = AppSettings.resourcemanagerreport.GetString("trNormals"); break;
                case "d": s = AppSettings.resourcemanagerreport.GetString("trHaveExpirationDates"); break;
                case "sn": s = AppSettings.resourcemanagerreport.GetString("trHaveSerialNumbers"); break;
                case "sr": s = AppSettings.resourcemanagerreport.GetString("trServices"); break;
                case "p": s = AppSettings.resourcemanagerreport.GetString("trPackages"); break;
            }

            return s;
        }
        public static string BranchStoreConverter(string type)
        {
            string s = "";
            switch (type)
            {
                case "b": s = AppSettings.resourcemanagerreport.GetString("tr_Branch"); break;
                case "s": s = AppSettings.resourcemanagerreport.GetString("tr_Store"); break;

            }

            return s;
        }
        public static void PurStsReport(IEnumerable<ItemTransferInvoice> tempquery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var r in tempquery)
            {
                r.invType= InvoiceTypeConv(r.invType);
                r.categoryName = CategoryConv(r.categoryName);
                if (r.CopdiscountValue!=null)
                {
                    r.CopdiscountValue = decimal.Parse(accuracyDiscountConvert(r.CopdiscountValue, r.CopdiscountType));
                    r.couponTotalValue = decimal.Parse(HelpClass.DecTostring(r.couponTotalValue));//

                }
                if (r.OdiscountValue!=null)
                {
                   
                    r.OdiscountValue = decimal.Parse(accuracyDiscountConvert(r.OdiscountValue, r.OdiscountType));
                    r.offerTotalValue = decimal.Parse(HelpClass.DecTostring(r.offerTotalValue));
                }
                r.price = decimal.Parse(HelpClass.DecTostring(r.price));
                r.ITprice = decimal.Parse(HelpClass.DecTostring(r.ITprice));

                r.subTotal = decimal.Parse(HelpClass.DecTostring(r.subTotal));
                r.totalNet = decimal.Parse(HelpClass.DecTostring(r.totalNet));
                r.discountValue = decimal.Parse(HelpClass.DecTostring(r.discountValue));
                r.tax = decimal.Parse(HelpClass.PercentageDecTostring(r.tax));
                if (r.itemAvg != null)
                {
                    r.itemAvg = double.Parse(HelpClass.DecTostring(decimal.Parse(r.itemAvg.ToString())));

                }
            }

            rep.DataSources.Add(new ReportDataSource("DataSetITinvoice", tempquery));


        }
        public static void PurItemStsReport(IEnumerable<ItemTransferInvoice> tempquery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            PurStsReport(tempquery, rep, reppath);
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));

            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trInvoices", AppSettings.resourcemanagerreport.GetString("trInvoices")));
            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));

            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("trI_nvoice", AppSettings.resourcemanager.GetString("trItem") + "/" + AppSettings.resourcemanager.GetString("trInvoices")));

        }

        public static void saleitemStsReport(IEnumerable<ItemTransferInvoice> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<ItemTransferInvoice> tempquery = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(JsonConvert.SerializeObject(Query));

            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trCategorie", AppSettings.resourcemanagerreport.GetString("trCategorie")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trInvoices", AppSettings.resourcemanagerreport.GetString("trInvoices")));
            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));

            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("tr_Invoice", AppSettings.resourcemanager.GetString("trItem") + "/" + AppSettings.resourcemanager.GetString("trInvoices")));

            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
         //   saleInvoiceConverterForReport
            PurStsReport(tempquery, rep, reppath);
          //  rep.DataSources.Add(new ReportDataSource("DataSetITinvoice", tempquery));
        }

        public static void SalePromoStsReport(IEnumerable<ItemTransferInvoice> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            List<ItemTransferInvoice> tempquery = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(JsonConvert.SerializeObject(Query));
    

            PurStsReport(tempquery, rep, reppath);
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trCoupon", AppSettings.resourcemanagerreport.GetString("trCoupon")));
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trValue", AppSettings.resourcemanagerreport.GetString("trValue")));
            paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trDiscount")));
            paramarr.Add(new ReportParameter("trOffer", AppSettings.resourcemanagerreport.GetString("trOffer")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trInvoiceClass", AppSettings.resourcemanagerreport.GetString("trInvoiceClass")));
         
            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));

            //  itemTransferDiscountTypeConv(paramarr);
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));


          
      

        }
        public static void invoicClassReport(IEnumerable<SalesMembership> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<SalesMembership> tempquery = JsonConvert.DeserializeObject<List<SalesMembership>>(JsonConvert.SerializeObject(Query));
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            //  PurStsReport(tempquery, rep, reppath);
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trCoupon", AppSettings.resourcemanagerreport.GetString("trCoupon")));
            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trValue", AppSettings.resourcemanagerreport.GetString("trValue")));
            paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trDiscount")));
            paramarr.Add(new ReportParameter("trOffer", AppSettings.resourcemanagerreport.GetString("trOffer")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trInvoiceClass", AppSettings.resourcemanagerreport.GetString("trInvoiceClass")));

            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));

            //  itemTransferDiscountTypeConv(paramarr);
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));


            foreach (var r in tempquery)
            {
                r.invClassdiscountValue = decimal.Parse(accuracyDiscountConvert(r.invClassdiscountValue, r.invClassdiscountType));
                r.finalDiscount = decimal.Parse(HelpClass.DecTostring(r.finalDiscount));
                r.total = decimal.Parse(HelpClass.DecTostring(r.total));

            }

            rep.DataSources.Add(new ReportDataSource("DataSetITinvoice", tempquery));
        }
        public static void itemTransferDiscountTypeConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("trValueDiscount", AppSettings.resourcemanagerreport.GetString("trValueDiscount")));
            paramarr.Add(new ReportParameter("trPercentageDiscount", AppSettings.resourcemanagerreport.GetString("trPercentageDiscount")));




        }

        public static void itemTypeConv(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("trNormal", AppSettings.resourcemanagerreport.GetString("trNormal")));
            paramarr.Add(new ReportParameter("trHaveExpirationDate", AppSettings.resourcemanagerreport.GetString("trHaveExpirationDate")));
            paramarr.Add(new ReportParameter("trHaveSerialNumber", AppSettings.resourcemanagerreport.GetString("trHaveSerialNumber")));
            paramarr.Add(new ReportParameter("trService", AppSettings.resourcemanagerreport.GetString("trService")));
            paramarr.Add(new ReportParameter("trPackage", AppSettings.resourcemanagerreport.GetString("trPackage")));
        }
        //clsReports.SaleInvoiceStsReport(itemTransfers, rep, reppath, paramarr);

        public static void SaleInvoiceStsReport(IEnumerable<ItemTransferInvoice> tempquery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            PurStsReport(tempquery, rep, reppath);
            paramarr.Add(new ReportParameter("isTax", AppSettings.invoiceTax_bool.ToString()));
            itemTransferInvTypeConv(paramarr);

        }
        public static void SaleInvoiceSTS(IEnumerable<ItemTransferInvoice> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<ItemTransferInvoice> tempquery = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(JsonConvert.SerializeObject(Query));

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var r in tempquery)
            {
                r.CopdiscountValue = decimal.Parse(HelpClass.DecTostring(r.CopdiscountValue));
                r.couponTotalValue = decimal.Parse(HelpClass.DecTostring(r.couponTotalValue));//
                r.OdiscountValue = decimal.Parse(HelpClass.DecTostring(r.OdiscountValue));
                r.offerTotalValue = decimal.Parse(HelpClass.DecTostring(r.offerTotalValue));
                r.ITprice = decimal.Parse(HelpClass.DecTostring(r.ITprice));
                r.subTotal = decimal.Parse(HelpClass.DecTostring(r.subTotal));
                r.totalNet = decimal.Parse(HelpClass.DecTostring(r.totalNet));
                r.discountValue = decimal.Parse(HelpClass.DecTostring(r.discountValue));
                r.tax = decimal.Parse(HelpClass.PercentageDecTostring(r.tax));

                r.invType = InvoiceTypeConv(r.invType);
                if (r.itemAvg != null)
                {
                    r.itemAvg = double.Parse(HelpClass.DecTostring(decimal.Parse(r.itemAvg.ToString())));

                }
            }
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trPOS", AppSettings.resourcemanagerreport.GetString("trPOS")));
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trUser", AppSettings.resourcemanagerreport.GetString("trUser")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trTax", AppSettings.resourcemanagerreport.GetString("trTax")));
            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trDiscount")));


            DateFormConv(paramarr);
            paramarr.Add(new ReportParameter("isTax", AppSettings.invoiceTax_bool.ToString()));

            rep.DataSources.Add(new ReportDataSource("DataSetITinvoice", tempquery));


            // itemTransferInvTypeConv(paramarr);

        }
        public static string InvoiceTypeConv(string invType)
        {
            switch (invType)
            {
                //مبيعات
                case "s":
                    invType = AppSettings.resourcemanagerreport.GetString("trDiningHallType");
                    break;
                // طلب خارجي
                case "ts":
                    invType = AppSettings.resourcemanagerreport.GetString("trTakeAway");
                    break;
                // خدمة ذاتية
                case "ss":
                    invType = AppSettings.resourcemanagerreport.GetString("trSelfService");
                    break;
                default: break;
            }
            return invType;
        }
        public static void SaledailyReport(IEnumerable<ItemTransferInvoice> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<ItemTransferInvoice> tempquery = JsonConvert.DeserializeObject<List<ItemTransferInvoice>>(JsonConvert.SerializeObject(Query));

            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            string date = "";
            //PurStsReport(tempquery, rep, reppath);
            if (tempquery == null || tempquery.Count() == 0)
            {
                date = "";
            }
            else
            {
                date = HelpClass.DateToString(tempquery.FirstOrDefault().updateDate);
            }
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("invDate", date));
            paramarr.Add(new ReportParameter("isTax", AppSettings.invoiceTax_bool.ToString()));
    
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trPOS", AppSettings.resourcemanagerreport.GetString("trPOS")));
            paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trDiscount")));
            paramarr.Add(new ReportParameter("trTax", AppSettings.resourcemanagerreport.GetString("trTax")));
            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("trPaymentMethods", AppSettings.resourcemanagerreport.GetString("trPaymentMethods")));

            foreach (var r in tempquery)
            {
                r.invType = InvoiceTypeConv(r.invType);
                r.discountValue = decimal.Parse(HelpClass.DecTostring(r.discountValue));
                r.tax = decimal.Parse(HelpClass.PercentageDecTostring(r.tax));
                r.totalNet = decimal.Parse(HelpClass.DecTostring(r.totalNet));
                r.processType = processTypeConvswitch(r.processType);
            }

            rep.DataSources.Add(new ReportDataSource("DataSetITinvoice", tempquery));

        }
        public static void ProfitReport(IEnumerable<ItemUnitInvoiceProfit> tempquery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var r in tempquery)
            {

                r.totalNet = decimal.Parse(HelpClass.DecTostring(r.totalNet));
                r.invoiceProfit = decimal.Parse(HelpClass.DecTostring(r.invoiceProfit));
                r.itemProfit = decimal.Parse(HelpClass.DecTostring(r.itemProfit));
                r.itemunitProfit = decimal.Parse(HelpClass.DecTostring(r.itemunitProfit));
            }
            rep.DataSources.Add(new ReportDataSource("DataSetProfit", tempquery));
            paramarr.Add(new ReportParameter("title", AppSettings.resourcemanagerreport.GetString("trProfits")));
            paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));
            itemTransferInvTypeConv(paramarr);
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trPOS", AppSettings.resourcemanagerreport.GetString("trPOS")));
            paramarr.Add(new ReportParameter("trProfits", AppSettings.resourcemanagerreport.GetString("trProfits")));

        }

        public static void AccTaxReport(IEnumerable<ItemTransferInvoiceTax> invoiceItems, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetITinvoice", invoiceItems));
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNo.")));// tt
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("trTaxValue", AppSettings.resourcemanagerreport.GetString("trTaxValue")));
            paramarr.Add(new ReportParameter("trTaxPercentage", AppSettings.resourcemanagerreport.GetString("trTaxPercentage")));
            paramarr.Add(new ReportParameter("trTotalInvoice", AppSettings.resourcemanagerreport.GetString("trTotalInvoice")));
            paramarr.Add(new ReportParameter("trItemUnit", AppSettings.resourcemanagerreport.GetString("trItemUnit")));
            paramarr.Add(new ReportParameter("trOnItem", AppSettings.resourcemanagerreport.GetString("trOnItem")));
            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));

            paramarr.Add(new ReportParameter("trSum", AppSettings.resourcemanagerreport.GetString("trTotalTax")));
            foreach (var r in invoiceItems)
            {
                r.OneItemPriceNoTax = decimal.Parse(HelpClass.DecTostring(r.OneItemPriceNoTax));
                r.subTotalNotax = decimal.Parse(HelpClass.DecTostring(r.subTotalNotax));//
                r.ItemTaxes = decimal.Parse(HelpClass.PercentageDecTostring(r.ItemTaxes));
                r.itemUnitTaxwithQTY = decimal.Parse(HelpClass.DecTostring(r.itemUnitTaxwithQTY));
                r.subTotalTax = decimal.Parse(HelpClass.DecTostring(r.subTotalTax));

                r.totalNoTax = decimal.Parse(HelpClass.DecTostring(r.totalNoTax));
                r.tax = decimal.Parse(HelpClass.PercentageDecTostring(r.tax));
                r.invTaxVal = decimal.Parse(HelpClass.DecTostring(r.invTaxVal));
                r.totalNet = decimal.Parse(HelpClass.DecTostring(r.totalNet));

            }
            paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));

        }

        public static string ReportTabTitle(string firstTitle, string secondTitle)
        {
            string trtext = "";
            //////////////////////////////////////////////////////////////////////////////
            if (firstTitle == "invoice")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trInvoices");
            else if (firstTitle == "quotation")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trQuotations");
            else if (firstTitle == "promotion")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trThePromotion");
            else if (firstTitle == "internal")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trInternal");
            else if (firstTitle == "external")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trExternal");
            else if (firstTitle == "banksReport")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trBanks");
            else if (firstTitle == "destroied")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trDestructives");
            else if (firstTitle == "usersReport")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trUsers");
            else if (firstTitle == "storageReports")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trStorage");
            else if (firstTitle == "stocktaking")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trStocktaking");
            else if (firstTitle == "stock")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trStock");
            else if (firstTitle == "purchaseOrders")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trPurchaseOrders");
            else if (firstTitle == "saleOrders")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trSalesOrders");

            else if (firstTitle == "saleItems" || firstTitle == "purchaseItem")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trItems");
            else if (firstTitle == "recipientReport")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trReceived");
            else if (firstTitle == "accountStatement")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trAccountStatement");
            else if (firstTitle == "paymentsReport")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trPayments");
            else if (firstTitle == "posReports")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trPOS");
            else if (firstTitle == "dailySalesStatistic")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trDailySales");
            else if (firstTitle == "accountProfits")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trProfits");
            else if (firstTitle == "accountFund")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trCashBalance");
            else if (firstTitle == "quotations")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trQTReport");
            else if (firstTitle == "transfers")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trTransfers");
            else if (firstTitle == "fund")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trCashBalance");
            else if (firstTitle == "DirectEntry")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trDirectEntry");
            else if (firstTitle == "tax")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trTax");
            else if (firstTitle == "closing")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trDailyClosing");
            else if (firstTitle == "orders")
                firstTitle = AppSettings.resourcemanagerreport.GetString("orderReport");
            else if (firstTitle == "PreparingOrders")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trPreparingOrders");
            else if (firstTitle == "SpendingRequests")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trSpendingRequests");
            else if (firstTitle == "Consumption")
                firstTitle = AppSettings.resourcemanagerreport.GetString("trConsumption");
            else if (firstTitle == "membership")
                firstTitle = AppSettings.resourcemanagerreport.GetString("membership");
            //trCashBalance trDirectEntry
            //membership 
            //////////////////////////////////////////////////////////////////////////////

            if (secondTitle == "branch")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trBranches");
            else if (secondTitle == "pos")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trPOS");
            else if (secondTitle == "vendors" || secondTitle == "vendor")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trVendors");
            else if (secondTitle == "customers" || secondTitle == "customer")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trCustomers");
            else if (secondTitle == "users" || secondTitle == "user")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trUsers");
            else if (secondTitle == "items" || secondTitle == "item")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trItems");
            else if (secondTitle == "coupon")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trCoupons");
            else if (secondTitle == "offers")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trOffers");
            else if (secondTitle == "invoice")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trInvoices");
            else if (secondTitle == "order")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trOrders");
            else if (secondTitle == "quotation")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trQTReport");
            else if (secondTitle == "operator")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trOperator");
            else if (secondTitle == "operations")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trOperations");//trOperations
            else if (secondTitle == "payments")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trPayments");
            else if (secondTitle == "recipient")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trReceived");
            else if (secondTitle == "destroied")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trDestructives");
            else if (secondTitle == "agent")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trCustomers");
            else if (secondTitle == "stock")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trStock");
            else if (secondTitle == "external")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trExternal");
            else if (secondTitle == "internal")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trInternal");
            else if (secondTitle == "stocktaking")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trStocktaking");
            else if (secondTitle == "archives")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trArchive");
            else if (secondTitle == "shortfalls")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trShortages");
            else if (secondTitle == "location")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trLocation");
            else if (secondTitle == "locations")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trLocations");
            else if (secondTitle == "collect")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trCollect");
            else if (secondTitle == "shipping")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trShipping");
            else if (secondTitle == "salary")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trSalary");
            else if (secondTitle == "generalExpenses")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trGeneralExpenses");
            else if (secondTitle == "administrativePull")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trAdministrativePull");
            else if (secondTitle == "AdministrativeDeposit")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trAdministrativeDeposit");
            else if (secondTitle == "BestSeller")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trBestSeller");
            else if (secondTitle == "MostPurchased")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trMostPurchased");
            else if (secondTitle == "pull")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trPull");
            else if (secondTitle == "deposit")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trDeposit");
            else if (secondTitle == "discounts")
                secondTitle = AppSettings.resourcemanagerreport.GetString("discounts");
            else if (secondTitle == "invClasses")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trInvoicesClasses");
            else if (secondTitle == "memberships")
                secondTitle = AppSettings.resourcemanagerreport.GetString("trMemberships");

            //memberships
            //////////////////////////////////////////////////////////////////////////////
            if (firstTitle == "" && secondTitle != "")
            {
                trtext = secondTitle;
            }
            else if (secondTitle == "" && firstTitle != "")
            {
                trtext = firstTitle;
            }
            else
            {
                trtext = firstTitle + " / " + secondTitle;
            }

            return trtext;
        }
        public static void PurInvStsReport(IEnumerable<ItemTransferInvoice> tempquery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            PurStsReport(tempquery, rep, reppath);
            itemTransferInvTypeConv(paramarr);
            paramarr.Add(new ReportParameter("isTax", AppSettings.invoiceTax_bool.ToString()));

            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trPOS", AppSettings.resourcemanagerreport.GetString("trPOS")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trUser", AppSettings.resourcemanagerreport.GetString("trUser")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trDiscount")));
            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));
            paramarr.Add(new ReportParameter("trTax", AppSettings.resourcemanagerreport.GetString("trTax")));
            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("trVendor", AppSettings.resourcemanagerreport.GetString("trVendor")));
        }


        public static void PurOrderStsReport(IEnumerable<ItemTransferInvoice> tempquery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            PurStsReport(tempquery, rep, reppath);
            itemTransferInvTypeConv(paramarr);

            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));

            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trPOS", AppSettings.resourcemanagerreport.GetString("trPOS")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trUser", AppSettings.resourcemanagerreport.GetString("trUser")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));

            paramarr.Add(new ReportParameter("trPrice", AppSettings.resourcemanagerreport.GetString("trPrice")));

            paramarr.Add(new ReportParameter("trVendor", AppSettings.resourcemanagerreport.GetString("trVendor")));

        }


        public static void posReport(IEnumerable<Pos> possQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetPos", possQuery));
        }

        public static void customerReport(IEnumerable<Agent> customersQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("AgentDataSet", customersQuery));
        }

        public static void branchReport(IEnumerable<Branch> branchQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetBranches", branchQuery));
        }

        public static void userReport(IEnumerable<User> usersQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetUser", usersQuery));
        }

        public static void vendorReport(IEnumerable<Agent> vendorsQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("AgentDataSet", vendorsQuery));
        }

        public static void storeReport(IEnumerable<Branch> storesQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetBranches", storesQuery));
        }
        public static void purchaseInvoiceReport(List<ItemTransfer> invoiceItems, LocalReport rep, string reppath)
        {
            foreach (var i in invoiceItems)
            {
                i.price = decimal.Parse(HelpClass.DecTostring(i.price));
            }
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetItemTransfer", invoiceItems));
            rep.EnableExternalImages = true;

        }
        public static void storage(IEnumerable<Storage> storageQuery, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var r in storageQuery)
            {
                if (r.startDate != null)
                    r.startDate = DateTime.Parse(HelpClass.DateToString(r.startDate));//
                if (r.endDate != null)
                    r.endDate = DateTime.Parse(HelpClass.DateToString(r.endDate));
                //r.inventoryDate = DateTime.Parse(HelpClass.DateToString(r.inventoryDate));
                //r.IupdateDate = DateTime.Parse(HelpClass.DateToString(r.IupdateDate));

                //r.diffPercentage = decimal.Parse(HelpClass.DecTostring(r.diffPercentage));
                r.storageCostValue = decimal.Parse(HelpClass.DecTostring(r.storageCostValue));
            }
            rep.DataSources.Add(new ReportDataSource("DataSetStorage", storageQuery));
        }

        /* free zone
         =iif((Fields!section.Value="FreeZone")And(Fields!location.Value="0-0-0"),
"-",Fields!section.Value+"-"+Fields!location.Value)
         * */
        public static void ClosingStsReport(IEnumerable<POSOpenCloseModel> query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var r in query)
            {
                r.cash = decimal.Parse(HelpClass.DecTostring(r.cash));
                r.openCash = decimal.Parse(HelpClass.DecTostring(r.openCash));

            }
            rep.DataSources.Add(new ReportDataSource("DataSetBalanceSTS", query));
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));
            paramarr.Add(new ReportParameter("trPOS", AppSettings.resourcemanagerreport.GetString("trPOS")));
            paramarr.Add(new ReportParameter("trOpenDate", AppSettings.resourcemanagerreport.GetString("trOpenDate")));
            paramarr.Add(new ReportParameter("trOpenCash", AppSettings.resourcemanagerreport.GetString("trOpenCash")));
            paramarr.Add(new ReportParameter("trCloseDate", AppSettings.resourcemanagerreport.GetString("trCloseDate")));
            paramarr.Add(new ReportParameter("trCloseCash", AppSettings.resourcemanagerreport.GetString("trCloseCash")));

            paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));


        }
        public static void ClosingOpStsReport(IEnumerable<OpenClosOperatinModel> query, LocalReport rep, string reppath, List<ReportParameter> paramarr, POSOpenCloseModel openclosrow)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var r in query)
            {
                r.cash = decimal.Parse(HelpClass.DecTostring(r.cash));
                //  r.openCash = decimal.Parse(SectionData.DecTostring(r.openCash));
                r.notes = closingDescriptonConverter(r);


            }
            rep.DataSources.Add(new ReportDataSource("DataSetBalanceSTS", query));
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));
            paramarr.Add(new ReportParameter("trPOS", AppSettings.resourcemanagerreport.GetString("trPOS")));
            paramarr.Add(new ReportParameter("trOpenDate", AppSettings.resourcemanagerreport.GetString("trOpenDate")));
            paramarr.Add(new ReportParameter("trOpenCash", AppSettings.resourcemanagerreport.GetString("trOpenCash")));
            paramarr.Add(new ReportParameter("trCloseDate", AppSettings.resourcemanagerreport.GetString("trCloseDate")));
            paramarr.Add(new ReportParameter("trCloseCash", AppSettings.resourcemanagerreport.GetString("trCloseCash")));

            paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));

            paramarr.Add(new ReportParameter("OpenDate", openclosrow.openDate.ToString()));
            paramarr.Add(new ReportParameter("OpenCash", HelpClass.DecTostring(openclosrow.openCash)));
            paramarr.Add(new ReportParameter("CloseDate", openclosrow.updateDate.ToString()));
            paramarr.Add(new ReportParameter("CloseCash", HelpClass.DecTostring(openclosrow.cash)));
            paramarr.Add(new ReportParameter("pos", openclosrow.branchName + " / " + openclosrow.posName));

            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trDescription", AppSettings.resourcemanagerreport.GetString("trDescription")));
            paramarr.Add(new ReportParameter("trCashTooltip", AppSettings.resourcemanagerreport.GetString("trCashTooltip")));



        }
        public static string closingDescriptonConverter(OpenClosOperatinModel s)
        {

            string name = "";
            switch (s.side)
            {
                case "bnd": break;
                case "v": name = AppSettings.resourcemanagerreport.GetString("trVendor"); break;
                case "c": name = AppSettings.resourcemanagerreport.GetString("trCustomer"); break;
                case "u": name = AppSettings.resourcemanagerreport.GetString("trUser"); break;
                case "s": name = AppSettings.resourcemanagerreport.GetString("trSalary"); break;
                case "e": name = AppSettings.resourcemanagerreport.GetString("trGeneralExpenses"); break;
                case "m":
                    if (s.transType == "d")
                        name = AppSettings.resourcemanagerreport.GetString("trAdministrativeDeposit");
                    if (s.transType == "p")
                        name = AppSettings.resourcemanagerreport.GetString("trAdministrativePull");
                    break;
                case "sh": name = AppSettings.resourcemanagerreport.GetString("trShippingCompany"); break;
                default: break;
            }

            if (!string.IsNullOrEmpty(s.agentName))
                name = name + " " + s.agentName;
            else if (!string.IsNullOrEmpty(s.usersName) && !string.IsNullOrEmpty(s.usersLName))
                name = name + " " + s.usersName + " " + s.usersLName;
            else if (!string.IsNullOrEmpty(s.shippingCompanyName))
                name = name + " " + s.shippingCompanyName;
            else if ((s.side != "e") && (s.side != "m"))
                name = name + " " + AppSettings.resourcemanagerreport.GetString("trUnKnown");

            if (s.transType.Equals("p"))
            {
                if ((s.side.Equals("bn")) || (s.side.Equals("p")))
                {
                    return AppSettings.resourcemanagerreport.GetString("trPull") + " " +
                           AppSettings.resourcemanagerreport.GetString("trFrom") + " " +
                           name;//receive
                }
                else if ((!s.side.Equals("bn")) || (!s.side.Equals("p")))
                {
                    return AppSettings.resourcemanagerreport.GetString("trPayment") + " " +
                           AppSettings.resourcemanagerreport.GetString("trTo") + " " +
                           name;//دفع
                }
                else return "";
            }
            else if (s.transType.Equals("d"))
            {
                if ((s.side.Equals("bn")) || (s.side.Equals("p")))
                {
                    return AppSettings.resourcemanagerreport.GetString("trDeposit") + " " +
                           AppSettings.resourcemanagerreport.GetString("trTo") + " " +
                           name;
                }
                else if ((!s.side.Equals("bn")) || (!s.side.Equals("p")))
                {
                    return AppSettings.resourcemanagerreport.GetString("trReceiptOperation") + " " +
                           AppSettings.resourcemanagerreport.GetString("trFrom") + " " +
                           name;//قبض
                }
                else return "";
            }
            else return "";

        }
        public static void storageStock(IEnumerable<Storage> storageQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            storage(storageQuery, rep, reppath);
            DateFormConv(paramarr);
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trSection", AppSettings.resourcemanagerreport.GetString("trSection")));
            paramarr.Add(new ReportParameter("trLocation", AppSettings.resourcemanagerreport.GetString("trLocation")));
            paramarr.Add(new ReportParameter("trItemUnit", AppSettings.resourcemanagerreport.GetString("trItemUnit")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trSectionLocation", AppSettings.resourcemanagerreport.GetString("trSectionLocation")));
            paramarr.Add(new ReportParameter("trStartDate", AppSettings.resourcemanagerreport.GetString("trStartDate")));
            paramarr.Add(new ReportParameter("trEndDate", AppSettings.resourcemanagerreport.GetString("trEndDate")));
            paramarr.Add(new ReportParameter("trMinCollect", AppSettings.resourcemanagerreport.GetString("trMinStock")));
            paramarr.Add(new ReportParameter("trMaxCollect", AppSettings.resourcemanagerreport.GetString("trMaxStock")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
        }
        // stocktaking 


        public static void Stocktakingparam(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trItemUnit", AppSettings.resourcemanagerreport.GetString("trItemUnit")));
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trDiffrencePercentage", AppSettings.resourcemanagerreport.GetString("trDiffrencePercentage")));
            paramarr.Add(new ReportParameter("trItemsCount", AppSettings.resourcemanagerreport.GetString("trItemsCount")));
            paramarr.Add(new ReportParameter("trDestroyedCount", AppSettings.resourcemanagerreport.GetString("trDestroyedCount")));
            paramarr.Add(new ReportParameter("trReason", AppSettings.resourcemanagerreport.GetString("trReason")));
        }

        public static void StocktakingArchivesReport(IEnumerable<InventoryClass> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            foreach (var r in Query)
            {

                //r.inventoryDate = DateTime.Parse(HelpClass.DateToString(r.inventoryDate));
                //r.IupdateDate = DateTime.Parse(HelpClass.DateToString(r.IupdateDate));

                r.diffPercentage = decimal.Parse(HelpClass.DecTostring(r.diffPercentage));
                //r.storageCostValue = decimal.Parse(HelpClass.DecTostring(r.storageCostValue));
            }


            rep.DataSources.Add(new ReportDataSource("DataSetInventoryClass", Query));
            DateFormConv(paramarr);
            InventoryTypeConv(paramarr);
            Stocktakingparam(paramarr);
        }

        public static void StocktakingShortfallsReport(IEnumerable<ItemTransferInvoice> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            //foreach (var r in Query)
            //{
            //    //if (r.startDate != null)
            //    //    r.startDate = DateTime.Parse(HelpClass.DateToString(r.startDate));//
            //    //if (r.endDate != null)
            //    //    r.endDate = DateTime.Parse(HelpClass.DateToString(r.endDate));

            //    //r.inventoryDate = DateTime.Parse(HelpClass.DateToString(r.inventoryDate));
            //    //r.IupdateDate = DateTime.Parse(HelpClass.DateToString(r.IupdateDate));

            //    //r.diffPercentage = decimal.Parse(HelpClass.DecTostring(r.diffPercentage));
            //    //r.storageCostValue = decimal.Parse(HelpClass.DecTostring(r.storageCostValue));
            //}


            rep.DataSources.Add(new ReportDataSource("DataSetItemTransferInvoice", Query));
            DateFormConv(paramarr);
            InventoryTypeConv(paramarr);
            Stocktakingparam(paramarr);

        }
        /*
        = Switch(Fields!inventoryType.Value="a",Parameters!trArchived.Value
,Fields!inventoryType.Value="n",Parameters!trSaved.Value
,Fields!inventoryType.Value="d",Parameters!trDraft.Value
)

         * */
        //

        public static void cashTransferStsBank(IEnumerable<CashTransferSts> cashTransfers, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            cashTransferSts(cashTransfers, rep, reppath);
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
            paramarr.Add(new ReportParameter("trPull", AppSettings.resourcemanagerreport.GetString("trPull")));
            paramarr.Add(new ReportParameter("trDeposit", AppSettings.resourcemanagerreport.GetString("trDeposit")));

            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));

            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trAccoutant", AppSettings.resourcemanagerreport.GetString("trAccoutant")));
            paramarr.Add(new ReportParameter("trBank", AppSettings.resourcemanagerreport.GetString("trBank")));
            paramarr.Add(new ReportParameter("trUser", AppSettings.resourcemanagerreport.GetString("trUser")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trAmount", AppSettings.resourcemanagerreport.GetString("trAmount")));

        }

        public static string StsStatementPaymentConvert(string value)
        {
            string s = "";
            switch (value)
            {
                case "cash":
                    s = AppSettings.resourcemanagerreport.GetString("trCash");
                    break;
                case "doc":
                    s = AppSettings.resourcemanagerreport.GetString("trDocument");
                    break;
                case "cheque":
                    s = AppSettings.resourcemanagerreport.GetString("trCheque");
                    break;
                case "balance":
                    s = AppSettings.resourcemanagerreport.GetString("trCredit");
                    break;
                case "card":
                    s = AppSettings.resourcemanagerreport.GetString("trAnotherPaymentMethods");
                    break;
                case "inv":
                    s = AppSettings.resourcemanagerreport.GetString("trInv");
                    break;
                default:
                    s = value;
                    break;


            }
            return s;
        }
        public static void cashTransferStsStatement(IEnumerable<CashTransferSts> cashTransfers, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            cashTransferStatSts(cashTransfers, rep, reppath);

            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo")));
            paramarr.Add(new ReportParameter("trTransferNumberTooltip", AppSettings.resourcemanagerreport.GetString("trTransferNumberTooltip")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trDescription", AppSettings.resourcemanagerreport.GetString("trDescription")));
            paramarr.Add(new ReportParameter("trPayment", AppSettings.resourcemanagerreport.GetString("trPayment")));
            paramarr.Add(new ReportParameter("trCashTooltip", AppSettings.resourcemanagerreport.GetString("trCashTooltip")));




        }
        public static void cashTransferStsPayment(IEnumerable<CashTransferSts> cashTransfers, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            cashTransferSts(cashTransfers, rep, reppath);

            cashTransferProcessTypeConv(paramarr);
            DateFormConv(paramarr);
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trPaymentTypeTooltip", AppSettings.resourcemanagerreport.GetString("trPaymentTypeTooltip")));
            paramarr.Add(new ReportParameter("trAccoutant", AppSettings.resourcemanagerreport.GetString("trAccoutant")));
            paramarr.Add(new ReportParameter("trRecipientTooltip", AppSettings.resourcemanagerreport.GetString("trRecipientTooltip")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trAmount", AppSettings.resourcemanagerreport.GetString("trAmount")));
            //trDepositor

        }
        public static void cashTransferStsPos(IEnumerable<CashTransferSts> cashTransfers, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            cashTransferSts(cashTransfers, rep, reppath);
            cashTransTypeConv(paramarr);
            DateFormConv(paramarr);

        }

        public static void cashTransferSts(IEnumerable<CashTransferSts> cashTransfers, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var r in cashTransfers)
            {
                r.updateDate = DateTime.Parse(HelpClass.DateToString(r.updateDate));
                r.cash = decimal.Parse(HelpClass.DecTostring(r.cash));
                r.agentName = AgentUnKnownConvert(r.agentId, r.side, r.agentName);
                r.agentCompany = AgentCompanyUnKnownConvert(r.agentId, r.side, r.agentCompany);

            }
            rep.DataSources.Add(new ReportDataSource("DataSetCashTransferSts", cashTransfers));
        }
        public static void cashTransferStatSts(IEnumerable<CashTransferSts> cashTransfers, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (CashTransferSts r in cashTransfers)
            {
                r.updateDate = DateTime.Parse(HelpClass.DateToString(r.updateDate));
                r.cash = decimal.Parse(HelpClass.DecTostring(r.cash));

                r.paymentreport = processTypeAndCardConverter(r.Description3, r.cardName);


            }
            rep.DataSources.Add(new ReportDataSource("DataSetCashTransferSts", cashTransfers));
        }
        public static string processTypeAndCardConverter(string processType, string cardName)
        {
            string pType = processType;
            string cName = cardName;

            switch (pType)
            {
                case "cash": return AppSettings.resourcemanagerreport.GetString("trCash");
                //break;
                case "doc": return AppSettings.resourcemanagerreport.GetString("trDocument");
                //break;
                case "cheque": return AppSettings.resourcemanagerreport.GetString("trCheque");
                //break;
                case "balance": return AppSettings.resourcemanagerreport.GetString("trCredit");
                //break;
                case "card": return cName;
                //break;
                case "inv": return "-";
                case "multiple": return AppSettings.resourcemanagerreport.GetString("trMultiplePayment");

                //break;
                default: return pType;
                    //break;
            }

        }
        public static void FundStsReport(IEnumerable<BalanceSTS> query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var r in query)
            {

                r.balance = decimal.Parse(HelpClass.DecTostring(r.balance));
            }
            rep.DataSources.Add(new ReportDataSource("DataSetBalanceSTS", query));
            paramarr.Add(new ReportParameter("title", AppSettings.resourcemanagerreport.GetString("trBalance")));
            paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trPOS", AppSettings.resourcemanagerreport.GetString("trPOS")));
            paramarr.Add(new ReportParameter("trBalance", AppSettings.resourcemanagerreport.GetString("trBalance")));


        }


        public static void cashTransferStsRecipient(IEnumerable<CashTransferSts> cashTransfers, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            cashTransferSts(cashTransfers, rep, reppath);

            cashTransferProcessTypeConv(paramarr);
            DateFormConv(paramarr);
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trPaymentTypeTooltip", AppSettings.resourcemanagerreport.GetString("trPaymentTypeTooltip")));
            paramarr.Add(new ReportParameter("trAccoutant", AppSettings.resourcemanagerreport.GetString("trAccoutant")));
            paramarr.Add(new ReportParameter("trRecipientTooltip", AppSettings.resourcemanagerreport.GetString("trDepositor")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trAmount", AppSettings.resourcemanagerreport.GetString("trAmount")));


        }
        public static void itemTransferInvoice(IEnumerable<ItemTransferInvoice> itemTransferInvoices, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetItemTransferInvoice", itemTransferInvoices));

        }
        public static void DateFormConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
        }

        public static void InventoryTypeConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("trArchived", AppSettings.resourcemanagerreport.GetString("trArchived")));
            paramarr.Add(new ReportParameter("trSaved", AppSettings.resourcemanagerreport.GetString("trSaved")));
            paramarr.Add(new ReportParameter("trDraft", AppSettings.resourcemanagerreport.GetString("trDraft")));
        }
        public static void cashTransTypeConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("trPull", AppSettings.resourcemanagerreport.GetString("trPull")));
            paramarr.Add(new ReportParameter("trDeposit", AppSettings.resourcemanagerreport.GetString("trDeposit")));

        }

        public static void cashTransferProcessTypeConv(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("trCash", AppSettings.resourcemanagerreport.GetString("trCash")));
            paramarr.Add(new ReportParameter("trDocument", AppSettings.resourcemanagerreport.GetString("trDocument")));
            paramarr.Add(new ReportParameter("trCheque", AppSettings.resourcemanagerreport.GetString("trCheque")));
            paramarr.Add(new ReportParameter("trCredit", AppSettings.resourcemanagerreport.GetString("trCredit")));
            paramarr.Add(new ReportParameter("trInv", AppSettings.resourcemanagerreport.GetString("trInv")));
            paramarr.Add(new ReportParameter("trCard", AppSettings.resourcemanagerreport.GetString("trCreditCard")));
        }
        public static void itemTransferInvTypeConv(List<ReportParameter> paramarr)
        {
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
            paramarr.Add(new ReportParameter("trPurchaseInvoice", AppSettings.resourcemanagerreport.GetString("trPurchaseInvoice")));
            paramarr.Add(new ReportParameter("trPurchaseInvoiceWaiting", AppSettings.resourcemanagerreport.GetString("trPurchaseInvoiceWaiting")));
            paramarr.Add(new ReportParameter("trSalesInvoice", AppSettings.resourcemanagerreport.GetString("trSalesInvoice")));
            paramarr.Add(new ReportParameter("trSalesReturnInvoice", AppSettings.resourcemanagerreport.GetString("trSalesReturnInvoice")));
            paramarr.Add(new ReportParameter("trPurchaseReturnInvoice", AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvoice")));
            paramarr.Add(new ReportParameter("trPurchaseReturnInvoiceWaiting", AppSettings.resourcemanagerreport.GetString("trPurchaseReturnInvoiceWaiting")));
            paramarr.Add(new ReportParameter("trDraftPurchaseBill", AppSettings.resourcemanagerreport.GetString("trDraftPurchaseBill")));
            paramarr.Add(new ReportParameter("trSalesDraft", AppSettings.resourcemanagerreport.GetString("trSalesDraft")));
            paramarr.Add(new ReportParameter("trSalesReturnDraft", AppSettings.resourcemanagerreport.GetString("trSalesReturnDraft")));

            paramarr.Add(new ReportParameter("trSaleOrderDraft", AppSettings.resourcemanagerreport.GetString("trSaleOrderDraft")));
            paramarr.Add(new ReportParameter("trSaleOrder", AppSettings.resourcemanagerreport.GetString("trSaleOrder")));
            paramarr.Add(new ReportParameter("trPurchaceOrderDraft", AppSettings.resourcemanagerreport.GetString("trPurchaceOrderDraft")));
            paramarr.Add(new ReportParameter("trPurchaceOrder", AppSettings.resourcemanagerreport.GetString("trPurchaceOrder")));
            paramarr.Add(new ReportParameter("trQuotationsDraft", AppSettings.resourcemanagerreport.GetString("trQuotationsDraft")));
            paramarr.Add(new ReportParameter("trQuotations", AppSettings.resourcemanagerreport.GetString("trQuotations")));
            paramarr.Add(new ReportParameter("trDestructive", AppSettings.resourcemanagerreport.GetString("trDestructive")));
            paramarr.Add(new ReportParameter("trShortage", AppSettings.resourcemanagerreport.GetString("trShortage")));
            paramarr.Add(new ReportParameter("trImportDraft", AppSettings.resourcemanagerreport.GetString("trImportDraft")));
            paramarr.Add(new ReportParameter("trImport", AppSettings.resourcemanagerreport.GetString("trImport")));
            paramarr.Add(new ReportParameter("trImportOrder", AppSettings.resourcemanagerreport.GetString("trImportOrder")));
            paramarr.Add(new ReportParameter("trExportDraft", AppSettings.resourcemanagerreport.GetString("trExportDraft")));

            paramarr.Add(new ReportParameter("trExport", AppSettings.resourcemanagerreport.GetString("trExport")));

            paramarr.Add(new ReportParameter("trExportOrder", AppSettings.resourcemanagerreport.GetString("trExportOrder")));

        }
        public static void invoiceSideConv(List<ReportParameter> paramarr)
        {


            paramarr.Add(new ReportParameter("trVendor", AppSettings.resourcemanagerreport.GetString("trVendor")));
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));


        }
        public static void AccountSideConv(List<ReportParameter> paramarr)
        {

            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));

            paramarr.Add(new ReportParameter("trVendor", AppSettings.resourcemanagerreport.GetString("trVendor")));
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
            paramarr.Add(new ReportParameter("trUser", AppSettings.resourcemanagerreport.GetString("trUser")));
            paramarr.Add(new ReportParameter("trSalary", AppSettings.resourcemanagerreport.GetString("trSalary")));
            paramarr.Add(new ReportParameter("trGeneralExpenses", AppSettings.resourcemanagerreport.GetString("trGeneralExpenses")));

            paramarr.Add(new ReportParameter("trAdministrativeDeposit", AppSettings.resourcemanagerreport.GetString("trAdministrativeDeposit")));

            paramarr.Add(new ReportParameter("trAdministrativePull", AppSettings.resourcemanagerreport.GetString("trAdministrativePull")));
            paramarr.Add(new ReportParameter("trShippingCompany", AppSettings.resourcemanagerreport.GetString("trShippingCompany")));


        }
        public static void itemTransferInvoiceExternal(IEnumerable<ItemTransferInvoice> itemTransferInvoices, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            itemTransferInvTypeConv(paramarr);
            invoiceSideConv(paramarr);

            itemTransferInvoice(itemTransferInvoices, rep, reppath);
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trAgentType", AppSettings.resourcemanagerreport.GetString("trAgentType")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trItemUnit", AppSettings.resourcemanagerreport.GetString("trItemUnit")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));

        }
        public static void itemTransferInvoiceDirect(IEnumerable<ItemTransferInvoice> itemTransferInvoices, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {

            itemTransferInvTypeConv(paramarr);
            invoiceSideConv(paramarr);

            itemTransferInvoice(itemTransferInvoices, rep, reppath);
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));

            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));

        }
        public static void itemTransferInvoiceInternal(IEnumerable<ItemTransferInvoice> itemTransferInvoices, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            itemTransferInvTypeConv(paramarr);
            itemTransferInvoice(itemTransferInvoices, rep, reppath);
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trType", AppSettings.resourcemanagerreport.GetString("trType")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));

            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));

            paramarr.Add(new ReportParameter("trFromBranch", AppSettings.resourcemanagerreport.GetString("trFromBranch")));
            paramarr.Add(new ReportParameter("trToBranch", AppSettings.resourcemanagerreport.GetString("trToBranch")));

        }
        public static void itemTransferInvoiceDestroied(IEnumerable<ItemTransferInvoice> itemTransferInvoices, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            itemTransferInvoice(itemTransferInvoices, rep, reppath);
            paramarr.Add(new ReportParameter("dateForm", AppSettings.dateFormat));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            paramarr.Add(new ReportParameter("trUser", AppSettings.resourcemanagerreport.GetString("trUser")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trItemUnit", AppSettings.resourcemanagerreport.GetString("trItemUnit")));
            paramarr.Add(new ReportParameter("trReason", AppSettings.resourcemanagerreport.GetString("trReason")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));


        }
        public static void PreparingOrdersReport(IEnumerable<OrderPreparingSTS> list, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<OrderPreparingSTS> Query = JsonConvert.DeserializeObject<List<OrderPreparingSTS>>(JsonConvert.SerializeObject(list));


            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));

            paramarr.Add(new ReportParameter("trInvoiceNumber", AppSettings.resourcemanagerreport.GetString("trInvoiceNumber")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));

            paramarr.Add(new ReportParameter("trCategorie", AppSettings.resourcemanagerreport.GetString("trCategorie")));
            paramarr.Add(new ReportParameter("trTag", AppSettings.resourcemanagerreport.GetString("trTag")));
            paramarr.Add(new ReportParameter("trStatus", AppSettings.resourcemanagerreport.GetString("trStatus")));
            paramarr.Add(new ReportParameter("duration", AppSettings.resourcemanagerreport.GetString("duration")));
            DateFormConv(paramarr);


            foreach (OrderPreparingSTS row in Query)
            {
                //row.statusConv = preparingOrderStatusConvert(row.status);
                row.status = preparingOrderStatusConvert(row.status);
            }


            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
        }
        //sale 
        public static reportsize PreparingOrdersPrint(IEnumerable<OrderPreparing> Query, List<ReportParameter> paramarr)
        {
            LocalReport rep = new LocalReport();
            reportsize rs = new reportsize();
            rs.rep = rep;
            List<OrderPreparing> list = JsonConvert.DeserializeObject<List<OrderPreparing>>(JsonConvert.SerializeObject(Query));
            List<OrderPreparing> reportOrderList = new List<OrderPreparing>();
            ReportCls reportclass = new ReportCls();

            rs.rep.EnableExternalImages = true;
            rs.rep.DataSources.Clear();
            //ReportCls.checkLang();
            paramarr.Add(new ReportParameter("invType", list.FirstOrDefault().invType));
            // string ss = AppSettings.resourcemanagerreport.GetString("trPreparingOrders");
            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trPreparingOrders")));
            paramarr.Add(new ReportParameter("invNumber", list.FirstOrDefault().invNum));

            paramarr.Add(new ReportParameter("invDate", reportclass.DateToString(list.FirstOrDefault().invDate) == null ? "-" : reportclass.DateToString(list.FirstOrDefault().createDate)));
            paramarr.Add(new ReportParameter("invTime", reportclass.TimeToString(list.FirstOrDefault().invTime)));
            paramarr.Add(new ReportParameter("branchName", list.FirstOrDefault().branchName));
            paramarr.Add(new ReportParameter("Notes", list.FirstOrDefault().notes));

            paramarr.Add(new ReportParameter("trNotes", AppSettings.resourcemanagerreport.GetString("trNotes")));

            paramarr.Add(new ReportParameter("trWaiter", AppSettings.resourcemanagerreport.GetString("trWaiter")));

            paramarr.Add(new ReportParameter("trTables", AppSettings.resourcemanagerreport.GetString("trTables")));

            paramarr.Add(new ReportParameter("Tables", list.FirstOrDefault().tables));
            paramarr.Add(new ReportParameter("trInvoice", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("trOrder", AppSettings.resourcemanagerreport.GetString("trBranch")));
            paramarr.Add(new ReportParameter("orderNum", list.FirstOrDefault().orderNum));
            paramarr.Add(new ReportParameter("userName", list.FirstOrDefault().waiter));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));

            paramarr.Add(new ReportParameter("isSaved", "y"));
            paramarr.Add(new ReportParameter("isOrginal", true.ToString()));

            DateFormConv(paramarr);
            if (Query.Count() > 0)
            {

                OrderPreparing tempObj = new OrderPreparing();

                //empty row
                tempObj.itemUnitId = -1;
                tempObj.orderNum = CategoryConv(list[0].categoryName);
                reportOrderList.Add(tempObj);
                //header row
                tempObj = addOrderHeader();
                reportOrderList.Add(tempObj);
                for (int i = 0; i < list.Count(); i++)
                {
                    tempObj = new OrderPreparing();
                    //list[i].categoryName = CategoryConv(list[i].categoryName);
                    //list[i].categoryCode = list[i].quantity.ToString();
                    tempObj = list[i];
                    //tempObj.categoryName= CategoryConv(list[i].categoryName);
                    tempObj.categoryCode = list[i].quantity.ToString();
                    reportOrderList.Add(tempObj);
                    tempObj = new OrderPreparing();

                    if (i + 1 < list.Count())
                    {
                        //&& i< list.Count()
                        //add headrer
                        if (list[i].categoryId != list[i + 1].categoryId)
                        {
                            tempObj = new OrderPreparing();
                            //empty row
                            tempObj.itemUnitId = -1;
                            tempObj.orderNum = CategoryConv(list[i + 1].categoryName);
                            reportOrderList.Add(tempObj);
                            //header row
                            tempObj = addOrderHeader();

                            reportOrderList.Add(tempObj);
                            //  trCategorie
                            //  trOrderSharp
                        }
                    }
                    //row.statusConv = preparingOrderStatusConvert(row.status);
                    //   row.status = preparingOrderStatusConvert(row.status);
                }
            }
            //rep.DataSources.Add(new ReportDataSource("DataSet", list)); 

            rs = reportclass.GetKitchenRdlcpath(AppSettings.kitchenPaperSize, reportOrderList.Count(), rs.rep);
            rs.rep.DataSources.Add(new ReportDataSource("DataSet", reportOrderList));
            return rs;

        }
        public static OrderPreparing addOrderHeader()
        {
            OrderPreparing tempObj = new OrderPreparing();
            tempObj.itemUnitId = -2;
            tempObj.orderNum = AppSettings.resourcemanagerreport.GetString("trOrderSharp");
            tempObj.itemName = AppSettings.resourcemanagerreport.GetString("trItem");
            tempObj.categoryCode = AppSettings.resourcemanagerreport.GetString("trQTR");
            return tempObj;
        }
        public static void DeliveryReport(IEnumerable<OrderPreparingSTS> list, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            List<OrderPreparingSTS> Query = JsonConvert.DeserializeObject<List<OrderPreparingSTS>>(JsonConvert.SerializeObject(list));


            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));

            paramarr.Add(new ReportParameter("trInvoiceNumber", AppSettings.resourcemanagerreport.GetString("trInvoiceNumber")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));

            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
            paramarr.Add(new ReportParameter("trCompany", AppSettings.resourcemanagerreport.GetString("trCompany")));
            paramarr.Add(new ReportParameter("trDriver", AppSettings.resourcemanagerreport.GetString("trDriver")));
            paramarr.Add(new ReportParameter("duration", AppSettings.resourcemanagerreport.GetString("duration")));
            DateFormConv(paramarr);


            foreach (OrderPreparingSTS row in Query)
            {
                //row.statusConv = preparingOrderStatusConvert(row.status);
                row.orderDurationConv = HelpClass.decimalToTime(row.orderDuration);
                row.shippingCompanyName = shippingCompanyNameConvert(row.shippingCompanyName);
            }


            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
        }


        public static string shippingCompanyNameConvert(string shippingCompanyName)
        {
            if (shippingCompanyName != null)
            {
                string s = shippingCompanyName;
                if (s.Equals("local ship"))
                    return "-";
                else
                    return s;
            }
            else return "-";
        }
        public static void spendingRequestReport(IEnumerable<ItemTransferInvoice> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));

            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("trDate", AppSettings.resourcemanagerreport.GetString("trDate")));
            paramarr.Add(new ReportParameter("trCount", AppSettings.resourcemanagerreport.GetString("trCount")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));


            DateFormConv(paramarr);


            //foreach (OrderPreparingSTS row in Query)
            //{
            //    row.statusConv = preparingOrderStatusConvert(row.status);
            //}


            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
        }

        public static void membershipsReport(IEnumerable<SalesMembership> Query, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();

            paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));

            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
            paramarr.Add(new ReportParameter("membership", AppSettings.resourcemanagerreport.GetString("membership")));
            paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trDiscount")));
            paramarr.Add(new ReportParameter("trBranch", AppSettings.resourcemanagerreport.GetString("trBranch")));


            //     DateFormConv(paramarr);


            foreach (SalesMembership row in Query)
            {
                row.totalDiscount = decimal.Parse(HelpClass.DecTostring(row.totalDiscount));
            }

            rep.DataSources.Add(new ReportDataSource("DataSet", Query));
        }

        public static void membershiptDiscountReport(SalesMembership salesMembership, LocalReport rep, string reppath, List<ReportParameter> paramarr, POSOpenCloseModel openclosrow)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var r in salesMembership.CouponInvoiceList.ToList())
            {
                r.finalDiscount = decimal.Parse(HelpClass.DecTostring(r.finalDiscount));
                r.discountValue = decimal.Parse(accuracyDiscountConvert(r.discountValue, r.discountType));



            }
            foreach (var r in salesMembership.itemsTransferList.ToList())
            {
                r.finalDiscount = decimal.Parse(HelpClass.DecTostring(r.finalDiscount));
                r.offerValue = decimal.Parse(accuracyDiscountConvert(r.offerValue, (byte?)r.offerType));



            }

            foreach (var r in salesMembership.invoiceClassDiscountList.ToList())
            {
                r.finalDiscount = decimal.Parse(HelpClass.DecTostring(r.finalDiscount));
                r.discountValue = decimal.Parse(accuracyDiscountConvert(r.discountValue, r.discountType));



            }

            paramarr.Add(new ReportParameter("trCode", AppSettings.resourcemanagerreport.GetString("trCode")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trValue", AppSettings.resourcemanagerreport.GetString("trValue")));
            paramarr.Add(new ReportParameter("trDiscount", AppSettings.resourcemanagerreport.GetString("trDiscount")));
            paramarr.Add(new ReportParameter("trCustomer", AppSettings.resourcemanagerreport.GetString("trCustomer")));
            // paramarr.Add(new ReportParameter("trNo", AppSettings.resourcemanagerreport.GetString("trNo.")));
            // paramarr.Add(new ReportParameter("membership", AppSettings.resourcemanagerreport.GetString("membership")));
            paramarr.Add(new ReportParameter("trExpire", AppSettings.resourcemanagerreport.GetString("trExpire")));
            paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("trInvoiceCode", AppSettings.resourcemanagerreport.GetString("trInvoiceCode")));

            paramarr.Add(new ReportParameter("trmembership", AppSettings.resourcemanagerreport.GetString("membership")));
            //  paramarr.Add(new ReportParameter("trTotal", AppSettings.resourcemanagerreport.GetString("trTotal")));
            paramarr.Add(new ReportParameter("trCoupons", AppSettings.resourcemanagerreport.GetString("trCoupons")));
            paramarr.Add(new ReportParameter("trOffer", AppSettings.resourcemanagerreport.GetString("trOffer")));
            paramarr.Add(new ReportParameter("trinvoicesClasses", AppSettings.resourcemanagerreport.GetString("invoicesClasses")));
            paramarr.Add(new ReportParameter("Customer", AgentUnKnownConvert(salesMembership.agentId, "c", salesMembership.agentName)));// unknown conv
            paramarr.Add(new ReportParameter("InvoiceCode", salesMembership.invNumber));

            paramarr.Add(new ReportParameter("membership", salesMembership.membershipsName));
            paramarr.Add(new ReportParameter("Expire", unlimitedEndDateConverter(salesMembership.subscriptionType, salesMembership.endDate)));// get datas
            paramarr.Add(new ReportParameter("total", HelpClass.DecTostring(salesMembership.totalDiscount)));
            paramarr.Add(new ReportParameter("Currency", AppSettings.Currency));
            paramarr.Add(new ReportParameter("trQTR", AppSettings.resourcemanagerreport.GetString("trQTR")));
            paramarr.Add(new ReportParameter("hidecop", salesMembership.CouponInvoiceList.ToList().Count() > 0 ? "0" : "1"));
            paramarr.Add(new ReportParameter("hideoff", salesMembership.itemsTransferList.ToList().Count() > 0 ? "0" : "1"));
            paramarr.Add(new ReportParameter("hideinvclass", salesMembership.invoiceClassDiscountList.ToList().Count() > 0 ? "0" : "1"));

            rep.DataSources.Add(new ReportDataSource("DataSetCoupon", salesMembership.CouponInvoiceList.ToList()));

            rep.DataSources.Add(new ReportDataSource("DataSetOffer", salesMembership.itemsTransferList.ToList()));

            rep.DataSources.Add(new ReportDataSource("DataSetinvoicesClasses", salesMembership.invoiceClassDiscountList.ToList()));

        }



        public static string accuracyDiscountConvert(decimal? discountValue, byte? discountType)
        {
            if (discountValue != null && discountType != null)
            {
                byte type = (byte)discountType;
                decimal value = (decimal)discountValue;

                decimal num = decimal.Parse(value.ToString());
                string s = num.ToString();

                switch (AppSettings.accuracy)
                {
                    case "0":
                        s = string.Format("{0:F0}", num);
                        break;
                    case "1":
                        s = string.Format("{0:F1}", num);
                        break;
                    case "2":
                        s = string.Format("{0:F2}", num);
                        break;
                    case "3":
                        s = string.Format("{0:F3}", num);
                        break;
                    default:
                        s = string.Format("{0:F1}", num);
                        break;
                }

                if (type == 2)
                {
                    string sdc = string.Format("{0:G29}", decimal.Parse(s));
                    //return sdc + "%";
                    return sdc;
                }
                else
                    return s;

            }
            else return "";
        }
        public static string accuracyDiscountConvert(decimal? discountValue,string discountType)
        {
            if (discountValue != null && discountType != null)
            {
               string type =  discountType;
                decimal value = (decimal)discountValue;

                decimal num = decimal.Parse(value.ToString());
                string s = num.ToString();

                switch (AppSettings.accuracy)
                {
                    case "0":
                        s = string.Format("{0:F0}", num);
                        break;
                    case "1":
                        s = string.Format("{0:F1}", num);
                        break;
                    case "2":
                        s = string.Format("{0:F2}", num);
                        break;
                    case "3":
                        s = string.Format("{0:F3}", num);
                        break;
                    default:
                        s = string.Format("{0:F1}", num);
                        break;
                }

                if (type == "2")
                {
                    string sdc = string.Format("{0:G29}", decimal.Parse(s));
                    //return sdc + "%";
                    return sdc;
                }
                else
                    return s;

            }
            else return "";
        }
        //public static void itemReport(IEnumerable<Item> itemQuery, LocalReport rep, string reppath)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    rep.DataSources.Add(new ReportDataSource("DataSetItem", itemQuery));

        //}

        //public static void properyReport(IEnumerable<Property> propertyQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        //{
        //    rep.ReportPath = reppath;
        //    rep.EnableExternalImages = true;
        //    rep.DataSources.Clear();
        //    rep.DataSources.Add(new ReportDataSource("DataSetProperty", propertyQuery));
        //    paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trProperties")));
        //    paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trProperty")));
        //    paramarr.Add(new ReportParameter("trValues", AppSettings.resourcemanagerreport.GetString("trValues")));
        //}

        public static void storageCostReport(IEnumerable<StorageCost> storageCostQuery, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            foreach (var s in storageCostQuery)
            {
                s.cost = decimal.Parse(HelpClass.DecTostring(s.cost));
            }
            rep.DataSources.Add(new ReportDataSource("DataSetStorageCost", storageCostQuery));
            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trStorageCost")));
            paramarr.Add(new ReportParameter("trName", AppSettings.resourcemanagerreport.GetString("trName")));
            paramarr.Add(new ReportParameter("trCost", AppSettings.resourcemanagerreport.GetString("trStorageCost")));

        }

        public static void inventoryReport(IEnumerable<InventoryItemLocation> invItemsLocations, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetInventory", invItemsLocations));
            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trStocktakingItems")));// tt
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));
            paramarr.Add(new ReportParameter("trSec_Loc", AppSettings.resourcemanagerreport.GetString("trSectionLocation")));//
            //paramarr.Add(new ReportParameter("trItem_UnitName", AppSettings.resourcemanagerreport.GetString("trUnitName")+"-" + AppSettings.resourcemanagerreport.GetString("")));
            paramarr.Add(new ReportParameter("trItem_UnitName", AppSettings.resourcemanagerreport.GetString("trItemUnit")));
            paramarr.Add(new ReportParameter("trRealAmount", AppSettings.resourcemanagerreport.GetString("trRealAmount")));
            paramarr.Add(new ReportParameter("trInventoryAmount", AppSettings.resourcemanagerreport.GetString("trInventoryAmount")));
            paramarr.Add(new ReportParameter("trDestroyCount", AppSettings.resourcemanagerreport.GetString("trDestoryCount")));
        }


        public static void ItemsExportReport(IEnumerable<ItemTransfer> invoiceItems, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetItemsExport", invoiceItems));
            paramarr.Add(new ReportParameter("trTitle", AppSettings.resourcemanagerreport.GetString("trItemsImport/Export")));// tt
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trQuantity", AppSettings.resourcemanagerreport.GetString("trQuantity")));
        }
        public static void ReceiptPurchaseReport(IEnumerable<ItemTransfer> invoiceItems, LocalReport rep, string reppath, List<ReportParameter> paramarr)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetItemsExport", invoiceItems));
            paramarr.Add(new ReportParameter("Title", AppSettings.resourcemanagerreport.GetString("trReceiptOfPurchasesBill")));// tt
            paramarr.Add(new ReportParameter("trNum", AppSettings.resourcemanagerreport.GetString("trNum")));
            paramarr.Add(new ReportParameter("trItem", AppSettings.resourcemanagerreport.GetString("trItem")));
            paramarr.Add(new ReportParameter("trUnit", AppSettings.resourcemanagerreport.GetString("trUnit")));
            paramarr.Add(new ReportParameter("trAmount", AppSettings.resourcemanagerreport.GetString("trQuantity")));
        }
        public static void itemLocation(IEnumerable<ItemLocation> itemLocations, LocalReport rep, string reppath)
        {
            rep.ReportPath = reppath;
            rep.EnableExternalImages = true;
            rep.DataSources.Clear();
            rep.DataSources.Add(new ReportDataSource("DataSetItemLocation", itemLocations));
        }


        // pdf print

        public async Task<resultmessage> pdfSaleInvoice(int invoiceId, string buttonSrc)
        {
            resultmessage resmsg = new resultmessage();
            resmsg.pdfpath = "";
            resmsg.result = "";
           // string result = "";
            try
            {
                Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
                Invoice prInvoice = new Invoice();
                List<ItemTransfer> invoiceItems = new List<ItemTransfer>();
                int itemscount;
                Invoice invoiceModel = new Invoice();
                ReportCls reportclass = new ReportCls();
                reportsize rs = new reportsize();
                LocalReport rep = new LocalReport();
                rs.rep = rep;
                //if (prinvoiceId != 0)
                //    prInvoice = await invoiceModel.GetByInvoiceId(prinvoiceId);
                //else
                prInvoice = await invoiceModel.GetByInvoiceId(invoiceId);

                ///
                if (int.Parse(AppSettings.Allow_print_inv_count) <= prInvoice.printedcount)
                {
                    resmsg.result = "trYouExceedLimit";
                    return resmsg;
                    //this.Dispatcher.Invoke(() =>
                    //{
                    //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);

                    //});

                }
                else
                {

                    ///////////////////////////
                    if (prInvoice.invType == "pd" || prInvoice.invType == "sd"
                             || prInvoice.invType == "sbd" || prInvoice.invType == "pbd" ||
                             prInvoice.invType == "ssd" || prInvoice.invType == "tsd")
                    {
                        //this.Dispatcher.Invoke(() =>
                        //{
                        //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trPrintDraftInvoice"), animation: ToasterAnimation.FadeIn);
                        //});
                        resmsg.result  = "trPrintDraftInvoice";
                        return resmsg;
                    }
                    else
                    {
                        //////////////////////////////////////////
                        List<ReportParameter> paramarr = new List<ReportParameter>();


                        if (prInvoice.invoiceId > 0)
                        {
                            #region fill invoice data

                            //items
                            invoiceItems = await invoiceModel.GetInvoicesItems(prInvoice.invoiceId);
                            itemscount = invoiceItems.Count();

                            rs = reportclass.GetreceiptInvoiceRdlcpath(prInvoice, 1, AppSettings.salePaperSize, itemscount, rs.rep);
                            //rs.rep;
                            //rs.width;
                            //rs.height;
                            //user


                            User employ = new User();
                            if (FillCombo.usersList != null)
                            {
                                employ = FillCombo.usersList.Where(X => X.userId == (int)prInvoice.updateUserId).FirstOrDefault();
                            }
                            else
                            {
                                employ = await employ.getUserById((int)prInvoice.updateUserId);
                            }

                            prInvoice.uuserName = employ.name;
                            prInvoice.uuserLast = employ.lastname;
                            //agent
                            if (prInvoice.agentId != null)
                            {
                                Agent agentinv = new Agent();

                                // agentinv = customers.Where(X => X.agentId == prInvoice.agentId).FirstOrDefault();

                                if (FillCombo.customersList != null)
                                {
                                    agentinv = FillCombo.customersList.Where(X => X.agentId == (int)prInvoice.agentId).FirstOrDefault();
                                }
                                else
                                {
                                    agentinv = await agentinv.getAgentById((int)prInvoice.agentId);
                                }

                                prInvoice.agentCode = agentinv.code;
                                //new lines
                                prInvoice.agentName = agentinv.name;
                                prInvoice.agentCompany = agentinv.company;

                            }
                            else
                            {
                                prInvoice.agentCode = "-";
                                prInvoice.agentName = "-";
                                prInvoice.agentCompany = "-";
                            }
                            //branch
                            Branch branch = new Branch();
                            if (FillCombo.branchsList != null)
                            {
                                branch = FillCombo.branchsList.Where(X => X.branchId == (int)prInvoice.branchCreatorId).FirstOrDefault();

                            }
                            else
                            {
                                branch = await branch.getBranchById((int)prInvoice.branchCreatorId);
                            }

                            if (branch.branchId > 0)
                            {
                                prInvoice.branchName = branch.name;
                            }

                            ReportCls.checkLang();
                            //shipping
                            ShippingCompanies shippingcom = new ShippingCompanies();

                            if (prInvoice.shippingCompanyId > 0)
                            {
                                if (FillCombo.shippingCompaniesList != null)
                                {
                                    shippingcom = FillCombo.shippingCompaniesList.Where(X => X.shippingCompanyId == (int)prInvoice.shippingCompanyId).FirstOrDefault();

                                }
                                else
                                {
                                    shippingcom = await shippingcom.GetByID((int)prInvoice.shippingCompanyId);
                                }

                            }
                            User shipuser = new User();
                            if (prInvoice.shipUserId > 0)
                            {
                                shipuser = await shipuser.getUserById((int)prInvoice.shipUserId);
                            }
                            prInvoice.shipUserName = shipuser.name + " " + shipuser.lastname;
                            //end shipping
                            //items subTotal & itemTax
                            decimal totaltax = 0;
                            foreach (var i in invoiceItems)
                            {
                                i.price = decimal.Parse(HelpClass.DecTostring(i.price));
                                if (i.itemTax != null)
                                {
                                    totaltax += (decimal)i.itemTax;

                                }
                                i.subTotal = decimal.Parse(HelpClass.DecTostring(i.price * i.quantity));

                            }

                            if (totaltax > 0 && prInvoice.invType != "sbd" && prInvoice.invType != "sb")
                            {
                                paramarr.Add(new ReportParameter("itemtax_note", AppSettings.itemtax_note.Trim()));
                                paramarr.Add(new ReportParameter("hasItemTax", "1"));

                            }
                            else
                            {
                                // paramarr.Add(new ReportParameter("itemtax_note", AppSettings.itemtax_note.Trim()));
                                paramarr.Add(new ReportParameter("hasItemTax", "0"));
                            }
                            //

                            clsReports.purchaseInvoiceReport(invoiceItems, rs.rep, rs.rep.ReportPath);

                            clsReports.setReportLanguage(paramarr);
                            clsReports.Header(paramarr);
                            paramarr.Add(new ReportParameter("isSaved", "y"));
                            paramarr = reportclass.fillSaleInvReport(prInvoice, paramarr, shippingcom);
                            //   multiplePaytable(paramarr);
                            // payment methods

                            if (prInvoice.invType == "s" || prInvoice.invType == "sd" || prInvoice.invType == "sbd" || prInvoice.invType == "sb"
                                || prInvoice.invType == "ts" || prInvoice.invType == "ss" || prInvoice.invType == "tsd" || prInvoice.invType == "ssd"
                                )
                            {
                                CashTransfer cachModel = new CashTransfer();
                                List<PayedInvclass> payedList = new List<PayedInvclass>();
                                payedList = await cachModel.GetPayedByInvId(prInvoice.invoiceId);
                                //mailpayedList = payedList;
                                decimal sump = payedList.Sum(x => x.cash);
                                decimal deservd = (decimal)prInvoice.totalNet - sump;
                                //convertter
                                foreach (var p in payedList)
                                {
                                    p.cash = decimal.Parse(reportclass.DecTostring(p.cash));
                                }
                                paramarr.Add(new ReportParameter("cashTr", AppSettings.resourcemanagerreport.GetString("trCashType")));

                                paramarr.Add(new ReportParameter("sumP", reportclass.DecTostring(sump)));
                                paramarr.Add(new ReportParameter("deserved", reportclass.DecTostring(deservd)));
                                rs.rep.DataSources.Add(new ReportDataSource("DataSetPayedInvclass", payedList));


                            }


                            rs.rep.SetParameters(paramarr);

                            rs.rep.Refresh();
                            #endregion
                            /////////////////////////////////////////////////////////
                            if (buttonSrc != "directprint")
                            {
                                if (buttonSrc == "pdf")
                                {
                                    resmsg.result = await savepdfinvoice(rs, prInvoice, paramarr);
                                    if (resmsg.result != "")
                                    {
                                        return resmsg;
                                    }
                                }
                                else if (buttonSrc == "print")
                                {
                                    resmsg.result = await printinvoice(rs, prInvoice, paramarr);
                                    if (resmsg.result != "")
                                    {
                                        return resmsg;
                                    }
                                }
                                else if (buttonSrc == "prev")
                                {
                                    resmsg = await previewinvoice(rs, prInvoice, paramarr);
                                    if (resmsg.result != "")
                                    {

                                        return resmsg;
                                    }
                                }
                                if (buttonSrc == "emailpdf")
                                {
                                    resmsg.rep = rs.rep;
                                    resmsg.width = rs.width;
                                    resmsg.height = rs.height;
                                    resmsg.paramarr = paramarr;
                                    resmsg = await saveEmailpdf(prInvoice, resmsg);
                                    //if (resmsg.result != "")
                                    //{
                                        return resmsg;
                                    //}
                                }
                            }
                            else
                            {
                                resmsg.rs = rs;
                                resmsg.prInvoice = prInvoice;
                                resmsg.paramarr = paramarr;
                                return resmsg;
                            }


                        }
                    }
                }

            }
            catch (Exception ex)
            {

                //this.Dispatcher.Invoke(() =>
                //{
                //    Toaster.ShowWarning(Window.GetWindow(this), message: "Not completed", animation: ToasterAnimation.FadeIn);

                //});
                resmsg.result = "notCompleted";
                return resmsg ;
            }
            return resmsg ;

        }
        public async Task<string> savepdfinvoice(reportsize rs, Invoice prInvoice, List<ReportParameter> paramarr)
        {
            string result = "";
            Microsoft.Win32.SaveFileDialog saveFileDialog = new Microsoft.Win32.SaveFileDialog();
            Invoice invoiceModel = new Invoice();
            //pdf
            saveFileDialog.Filter = "PDF|*.pdf;";
            bool? savdialog = false;
            //Dispatcher.Invoke(() =>
            // {
            savdialog = saveFileDialog.ShowDialog();

            //});
            if (savdialog == true)
            {
                string filepath = saveFileDialog.FileName;

                //copy count
                if (prInvoice.invType == "s" || prInvoice.invType == "sb" || prInvoice.invType == "p"
                    || prInvoice.invType == "pb"
                    || prInvoice.invType == "ss" || prInvoice.invType == "ts")
                {
                    paramarr.Add(new ReportParameter("isOrginal", prInvoice.isOrginal.ToString()));
                    //if (i > 1)
                    //{
                    //    // update paramarr->isOrginal
                    //    foreach (var item in paramarr.Where(x => x.Name == "isOrginal").ToList())
                    //    {
                    //        StringCollection myCol = new StringCollection();
                    //        myCol.Add(prInvoice.isOrginal.ToString());
                    //        item.Values = myCol;


                    //    }
                    //    //end update

                    //}
                    rs.rep.SetParameters(paramarr);

                    rs.rep.Refresh();


                    if (int.Parse(AppSettings.Allow_print_inv_count) > prInvoice.printedcount)
                    {

                        //Dispatcher.Invoke(() =>
                        //{
                        //LocalReportExtensions.ExportToPDF(rep, filepath);
                        if (AppSettings.salePaperSize != "A4")
                        {
                            LocalReportExtensions.customExportToPDF(rs.rep, filepath, rs.width, rs.height);
                        }
                        else
                        {
                            LocalReportExtensions.ExportToPDF(rs.rep, filepath);
                        }

                        //});


                        int res = 0;

                        res = await invoiceModel.updateprintstat(prInvoice.invoiceId, 1, false, true);



                        prInvoice.printedcount = prInvoice.printedcount + 1;

                        prInvoice.isOrginal = false;


                    }
                    else
                    {
                        //this.Dispatcher.Invoke(() =>
                        //{
                        //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);

                        //});
                        result = "trYouExceedLimit";
                        return result;


                    }


                }
                else
                {

                    //this.Dispatcher.Invoke(() =>
                    //{
                    //LocalReportExtensions.ExportToPDF(rep, filepath);
                    if (AppSettings.salePaperSize != "A4")
                    {
                        LocalReportExtensions.customExportToPDF(rs.rep, filepath, rs.width, rs.height);
                    }
                    else
                    {
                        LocalReportExtensions.ExportToPDF(rs.rep, filepath);
                    }

                    //});

                }
                // end copy count



            }
            //end pdf
            return result;
        }
        public async Task<resultmessage> saveEmailpdf( Invoice prInvoice, resultmessage resmsg)
        {
      
           // resmsg.result = "";
                  Invoice invoiceModel = new Invoice();
            //pdf
         
         
            //Dispatcher.Invoke(() =>
            // {
        

            //});
                //copy count
                if (prInvoice.invType == "s" || prInvoice.invType == "sb" || prInvoice.invType == "p"
                    || prInvoice.invType == "pb"
                    || prInvoice.invType == "ss" || prInvoice.invType == "ts")
                {
                   resmsg.paramarr.Add(new ReportParameter("isOrginal", false.ToString()));

                //if (i > 1)
                //{
                //    // update paramarr->isOrginal
                //    foreach (var item in paramarr.Where(x => x.Name == "isOrginal").ToList())
                //    {
                //        StringCollection myCol = new StringCollection();
                //        myCol.Add(prInvoice.isOrginal.ToString());
                //        item.Values = myCol;


                //    }
                //    //end update

                //}
                resmsg.rep.SetParameters(resmsg.paramarr);

                //    rs.rep.Refresh();


                    if (int.Parse(AppSettings.Allow_print_inv_count) > prInvoice.printedcount)
                    {

                        //Dispatcher.Invoke(() =>
                        //{
                        //LocalReportExtensions.ExportToPDF(rep, filepath);


                        //if (AppSettings.salePaperSize != "A4")
                        //{
                        //    LocalReportExtensions.customExportToPDF(rs.rep, filepath, rs.width, rs.height);
                        //}
                        //else
                        //{
                        //    LocalReportExtensions.ExportToPDF(rs.rep, filepath);
                        //}

                        //});


                        int res = 0;

                        res = await invoiceModel.updateprintstat(prInvoice.invoiceId, 1, false, true);



                        prInvoice.printedcount = prInvoice.printedcount + 1;

                        prInvoice.isOrginal = false;


                    }
                    else
                    {
                        //this.Dispatcher.Invoke(() =>
                        //{
                        //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);

                        //});
                  
                    resmsg.result = "trYouExceedLimit";
                     

                    }


                }
                else
                {
                resmsg.result = "trPrintDraftInvoice";
                    //this.Dispatcher.Invoke(() =>
                    //{
                    //LocalReportExtensions.ExportToPDF(rep, filepath);
                    //if (AppSettings.salePaperSize != "A4")
                    //{
                    //    LocalReportExtensions.customExportToPDF(rs.rep, filepath, rs.width, rs.height);
                    //}
                    //else
                    //{
                    //    LocalReportExtensions.ExportToPDF(rs.rep, filepath);
                    //}

                //});

            }
            // end copy count




            //end pdf
            resmsg.prInvoice = prInvoice;
            //resmsg.rep = rs.rep;
            //resmsg.height = rs.height;
            //resmsg.width = rs.width;
           
            return resmsg;
        }

        public async Task<string> printinvoice(reportsize rs, Invoice prInvoice, List<ReportParameter> paramarr)
        {
            string result = "";
            Invoice invoiceModel = new Invoice();
            // Start  print
            //copy count
            if (prInvoice.invType == "s" || prInvoice.invType == "sb" || prInvoice.invType == "p" || prInvoice.invType == "pb" || prInvoice.invType == "ts"
                || prInvoice.invType == "ss")
            {

                paramarr.Add(new ReportParameter("isOrginal", prInvoice.isOrginal.ToString()));

                for (int i = 1; i <= short.Parse(AppSettings.sale_copy_count); i++)
                {
                    if (i > 1)
                    {
                        // update paramarr->isOrginal
                        foreach (var item in paramarr.Where(x => x.Name == "isOrginal").ToList())
                        {
                            StringCollection myCol = new StringCollection();
                            myCol.Add(prInvoice.isOrginal.ToString());
                            item.Values = myCol;


                        }
                        //end update

                    }
                    rs.rep.SetParameters(paramarr);

                    rs.rep.Refresh();


                    if (int.Parse(AppSettings.Allow_print_inv_count) > prInvoice.printedcount)
                    {

                        //this.Dispatcher.Invoke(() =>
                        //{
                            if (AppSettings.salePaperSize == "A4")
                            {

                                LocalReportExtensions.PrintToPrinterbyNameAndCopy(rs.rep, AppSettings.sale_printer_name, 1);

                            }
                            else
                            {
                                LocalReportExtensions.customPrintToPrinter(rs.rep, AppSettings.sale_printer_name, 1, rs.width, rs.height);

                            }

                        //});


                        int res = 0;
                        res = await invoiceModel.updateprintstat(prInvoice.invoiceId, 1, false, true);
                        prInvoice.printedcount = prInvoice.printedcount + 1;

                        prInvoice.isOrginal = false;


                    }
                    else
                    {
                        //this.Dispatcher.Invoke(() =>
                        //{
                        //    Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);
                        //});
                        result = "trYouExceedLimit";
                        return result;

                    }

                }
            
            }
            else
            {

                //this.Dispatcher.Invoke(() =>
                //{

                    if (AppSettings.salePaperSize == "A4")
                    {

                        LocalReportExtensions.PrintToPrinterbyNameAndCopy(rs.rep, AppSettings.sale_printer_name, short.Parse(AppSettings.sale_copy_count));

                    }
                    else
                    {
                        LocalReportExtensions.customPrintToPrinter(rs.rep, AppSettings.sale_printer_name, short.Parse(AppSettings.sale_copy_count), rs.width, rs.height);

                    }


                //});

            }
            // end copy count
            //endprint
            return result;
        }
        public async Task<resultmessage> previewinvoice(reportsize rs, Invoice prInvoice, List<ReportParameter> paramarr)
        {
           // string result = "";
            resultmessage resmsg = new resultmessage();
            resmsg.pdfpath = "";
            resmsg.result = "";
            Invoice invoiceModel = new Invoice();
            ReportCls reportclass = new ReportCls();
            ////////////////////////
            string pdfpath = "";
            string folderpath = reportclass.PathUp(System.IO.Directory.GetCurrentDirectory(), 2, pdfpath) + @"\Thumb\report\";
            ReportCls.clearFolder(folderpath);

            pdfpath = @"\Thumb\report\Temp" + DateTime.Now.ToFileTime().ToString() + ".pdf";
            pdfpath = reportclass.PathUp(System.IO.Directory.GetCurrentDirectory(), 2, pdfpath);

            //////////////////////////////////
            
            // start preview
            //copy count
            if (prInvoice.invType == "s" || prInvoice.invType == "sb" || prInvoice.invType == "p" || prInvoice.invType == "pb" || prInvoice.invType == "ts" || prInvoice.invType == "ss")
            {
   
                paramarr.Add(new ReportParameter("isOrginal", prInvoice.isOrginal.ToString()));
                //// update paramarr->isOrginal
                //foreach (var item in paramarr.Where(x => x.Name == "isOrginal").ToList())
                //{
                //    StringCollection myCol = new StringCollection();
                //    myCol.Add(prInvoice.isOrginal.ToString());
                //    item.Values = myCol;


                //}
                ////end update
                //paramarr.Add(new ReportParameter("isOrginal", false.ToString()));

                rs.rep.SetParameters(paramarr);

                rs.rep.Refresh();
                /////////////////////////
                if (int.Parse(AppSettings.Allow_print_inv_count) > prInvoice.printedcount)
                {

                    if (prInvoice.invType == "s" && AppSettings.salePaperSize != "A4")
                    {
                        LocalReportExtensions.customExportToPDF(rs.rep, pdfpath, rs.width, rs.height);
                    }
                    else
                    {
                        LocalReportExtensions.ExportToPDF(rs.rep, pdfpath);
                    }


                    int res = 0;

                    res = await invoiceModel.updateprintstat(prInvoice.invoiceId, 1, false, true);



                    prInvoice.printedcount = prInvoice.printedcount + 1;

                    prInvoice.isOrginal = false;


                }
                else
                {
                    resmsg. result = "trYouExceedLimit";
                    return resmsg;
                    
                    //Toaster.ShowWarning(Window.GetWindow(this), message: AppSettings.resourcemanager.GetString("trYouExceedLimit"), animation: ToasterAnimation.FadeIn);
                }


            }
            else
            {

                if ( AppSettings.salePaperSize != "A4")
                {
                    LocalReportExtensions.customExportToPDF(rs.rep, pdfpath, rs.width, rs.height);
                }
                else
                {
                    LocalReportExtensions.ExportToPDF(rs.rep, pdfpath);
                }

            }
            // end copy count

            //end previw
            resmsg.pdfpath = pdfpath;
            return resmsg;
        }
        public List<OrderPreparing> newKitchenorderList(List<OrderPreparing> OrderListbeforesave, List<OrderPreparing> OrderListaftersave)
        {
            List<OrderPreparing> newOrderList = new List<OrderPreparing>();
            List<int> oldids = OrderListbeforesave.Select(X => (int)X.orderPreparingId).ToList();
            newOrderList = OrderListaftersave.Where(X => !oldids.Contains(X.orderPreparingId)).ToList().OrderBy(X => X.categoryId).ToList();
            return newOrderList;
        }
    }
}
