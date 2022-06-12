using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace laundryApp.Classes
{
    public class AppSettings
    {
        public static ResourceManager resourcemanager;
        public static ResourceManager resourcemanagerreport;

        public static string lang;
        public static string Reportlang;

        public static string defaultPath;

        internal static int? itemCost;
        public static CountryCode Region;
        public static string Currency;
        public static int CurrencyId;

        public static string logoImage;
        static public int nameId, addressId, emailId, mobileId, phoneId, faxId, logoId, taxId;
        public static string companyName;
        public static string Email;
        public static string Fax;
        public static string Mobile;
        public static string Address;
        public static string Phone;

        public static string sale_copy_count;
        public static string pur_copy_count;
        public static string print_on_save_sale;
        public static string print_on_save_pur;
        public static string email_on_save_sale;
        public static string email_on_save_pur;
        public static string rep_printer_name;
        public static string sale_printer_name;
        public static string kitchen_printer_name;

        public static string kitchenPaperSize;
        public static string salePaperSize;
        public static string rep_print_count;
        public static string docPapersize;
        public static string Allow_print_inv_count;
        public static string show_header;
        public static string print_on_save_directentry;
        public static string directentry_copy_count;
        public static string print_kitchen_on_sale;
        public static string print_kitchen_on_preparing;
        public static string kitchen_copy_count;
        internal static int? isInvTax;
        internal static decimal? tax;
        //tax
        internal static bool? invoiceTax_bool = true;
        internal static decimal? invoiceTax_decimal;
        internal static bool? itemsTax_bool = true;
        internal static decimal? itemsTax_decimal;
        internal static string itemtax_note;
        internal static string sales_invoice_note;
        //invoice
        internal static string invType;
        

        internal static string statusesOfPreparingOrder;
        internal static string dateFormat;
        internal static string accuracy;
        internal static decimal maxDiscount;
        internal static decimal? StorageCost;
        internal static string timeFormat = "ShortTimePattern";
        // hour
        static public double time_staying;
        // hour
        static public double maximumTimeToKeepReservation;
        // minutes
        static public int warningTimeForLateReservation;

        //typesOfService
        public static string typesOfService_clothes;
        public static string typesOfService_carpets;
        public static string typesOfService_cars;

        //points
        internal static int cashForPoint;
        internal static int PointsForInvoice;

        static public PosSetting posSetting = new PosSetting();
        static public List<Pos> posList = new List<Pos>();
        static public SettingCls setModel = new SettingCls();
        static public SetValues valueModel = new SetValues();
        static public List<SettingCls> settingsList = new List<SettingCls>();

    }
}
