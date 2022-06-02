using laundryApp.Classes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace laundryApp.converters
{
     
    public class invoiceTypeConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            switch (value)
            {
                //مشتريات 
                case "p":  value = AppSettings.resourcemanager.GetString("trPurchaseInvoice");
                    break;
                //فاتورة مشتريات بانتظار الادخال
                case "pw":
                    value = AppSettings.resourcemanager.GetString("trPurchaseInvoiceWaiting");
                    break;
                //مبيعات
                case "s": value = AppSettings.resourcemanager.GetString("trSalesInvoice");
                    break;
                //مرتجع مبيعات
                case "sb": value = AppSettings.resourcemanager.GetString("trSalesReturnInvoice");
                    break;
                //مرتجع مشتريات
                case "pb": value = AppSettings.resourcemanager.GetString("trPurchaseReturnInvoice");
                    break;
                //فاتورة مرتجع مشتريات بانتظار الاخراج
                case "pbw":
                    value = AppSettings.resourcemanager.GetString("trPurchaseReturnInvoiceWaiting");
                    break;
                //مسودة مشتريات 
                case "pd": value = AppSettings.resourcemanager.GetString("trDraftPurchaseBill");
                    break;
                //مسودة مبيعات
                case "sd": value = AppSettings.resourcemanager.GetString("trSalesDraft");
                    break;
                //مسودة مرتجع مبيعات
                case "sbd": value = AppSettings.resourcemanager.GetString("trSalesReturnDraft");
                    break;
                //مسودة مرتجع مشتريات
                case "pbd": value = AppSettings.resourcemanager.GetString("trPurchaseReturnDraft");
                    break;
                // مسودة طلبية مبيعا 
                case "ord":
                    value = AppSettings.resourcemanager.GetString("trDraft");
                    break;
                //طلبية مبيعات 
                case "or":
                    value = AppSettings.resourcemanager.GetString("trSaleOrder");
                    break;
                //مسودة طلبية شراء 
                case "pod":
                    value = AppSettings.resourcemanager.GetString("trDraft");
                    break;
                //طلبية شراء 
                case "po":
                    value = AppSettings.resourcemanager.GetString("trPurchaceOrder");
                    break;
                // طلبية شراء أو بيع محفوظة
                case "pos": case "ors":
                    value = AppSettings.resourcemanager.GetString("trSaved");
                    break;
                //مسودة عرض 
                case "qd":
                    value = AppSettings.resourcemanager.GetString("trQuotationsDraft");
                    break; 
                //عرض سعر محفوظ
                case "qs":
                    value = AppSettings.resourcemanager.GetString("trSaved");
                    break;
                //فاتورة عرض اسعار
                case "q":
                    value = AppSettings.resourcemanager.GetString("trQuotations");
                    break;
                //الإتلاف
                case "d":
                    value = AppSettings.resourcemanager.GetString("trDestructive");
                    break;
                //النواقص
                case "sh":
                    value = AppSettings.resourcemanager.GetString("trShortage");
                    break;
                //مسودة  استراد
                case "imd":
                    value = AppSettings.resourcemanager.GetString("trImportDraft");
                    break; 
                // استراد
                case "im":
                    value = AppSettings.resourcemanager.GetString("trImport");
                    break;  
              // طلب استيراد
                case "imw":
                    value = AppSettings.resourcemanager.GetString("trImportOrder");
                    break; 
                //مسودة تصدير
                case "exd":
                    value = AppSettings.resourcemanager.GetString("trExportDraft");
                    break; 
                // تصدير
                case "ex":
                    value = AppSettings.resourcemanager.GetString("trExport");
                    break;
               // طلب تصدير
                case "exw":
                    value = AppSettings.resourcemanager.GetString("trExportOrder");
                    break;
                // إدخال مباشر
                case "is":
                    value = AppSettings.resourcemanager.GetString("trDirectEntry");
                    break;
                // مسودة إدخال مباشر
                case "isd":
                    value = AppSettings.resourcemanager.GetString("trDirectEntryDraft");
                    break;
                // مسودة طلب خارجي
                case "tsd":
                    value = AppSettings.resourcemanager.GetString("trTakeAwayDraft");
                    break;
               // طلب خارجي
                case "ts":
                    value = AppSettings.resourcemanager.GetString("trTakeAway");
                    break;
               // خدمة ذاتية
                case "ss":
                    value = AppSettings.resourcemanager.GetString("trSelfService");
                    break;
               // خدمة ذاتية مسودة
                case "ssd":
                    value = AppSettings.resourcemanager.GetString("trSelfServiceDraft");
                    break;
                // فاتورة استهلاك
                case "fbc":
                    value = AppSettings.resourcemanager.GetString("consumptionInvoice");
                    break;
                default: break;
            }
            return value;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
   

}
