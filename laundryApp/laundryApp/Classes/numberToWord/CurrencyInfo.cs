using System;

namespace laundryApp.Classes
{
    public class CurrencyInfo
    {
        public enum Currencies {
            Kuwait=0,
            Saudi_Arabia,//1
            Oman,//2
            United_Arab_Emirates,//3
            Qatar,//4
            Bahrain,//5
            Iraq,//6
            Lebanon,//7
            Syria,//8
            Yemen,//9
            Jordan,//10
            Algeria,//11
            Egypt,//12
            Tunisia,//13
            Sudan,//14
            Morocco,//15
            Libya,//16
            Somalia,//17
            Turkey
        };

        #region Constructors

        public CurrencyInfo(Currencies currency)
        {
            switch (currency)
            {
                case Currencies.Kuwait:
                    CurrencyID = 0;
                    CurrencyCode = "KWD";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Kuwaiti Dinar";
                    EnglishPluralCurrencyName = "Kuwaiti Dinars";
                    EnglishCurrencyPartName = "Fils";
                    EnglishPluralCurrencyPartName = "Fils";
                    Arabic1CurrencyName = "دينار كويتي";
                    Arabic2CurrencyName = "ديناران كويتيان";
                    Arabic310CurrencyName = "دنانير كويتية";
                    Arabic1199CurrencyName = "ديناراً كويتياً";
                    Arabic1CurrencyPartName = "فلس";
                    Arabic2CurrencyPartName = "فلسان";
                    Arabic310CurrencyPartName = "فلوس";
                    Arabic1199CurrencyPartName = "فلساً";
                    PartPrecision = 3;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Syria:
                    CurrencyID = 8;
                    CurrencyCode = "SYP";
                    IsCurrencyNameFeminine = true;
                    EnglishCurrencyName = "Syrian Pound";
                    EnglishPluralCurrencyName = "Syrian Pounds";
                    EnglishCurrencyPartName = "Piaster";
                    EnglishPluralCurrencyPartName = "Piasteres";
                    Arabic1CurrencyName = "ليرة سورية";
                    Arabic2CurrencyName = "ليرتان سوريتان";
                    Arabic310CurrencyName = "ليرات سورية";
                    Arabic1199CurrencyName = "ليرة سورية";
                    Arabic1CurrencyPartName = "قرش";
                    Arabic2CurrencyPartName = "قرشان";
                    Arabic310CurrencyPartName = "قروش";
                    Arabic1199CurrencyPartName = "قرشاً";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.United_Arab_Emirates:
                    CurrencyID = 3;
                    CurrencyCode = "AED";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "UAE Dirham";
                    EnglishPluralCurrencyName = "UAE Dirhams";
                    EnglishCurrencyPartName = "Fils";
                    EnglishPluralCurrencyPartName = "Fils";
                    Arabic1CurrencyName = "درهم إماراتي";
                    Arabic2CurrencyName = "درهمان إماراتيان";
                    Arabic310CurrencyName = "دراهم إماراتية";
                    Arabic1199CurrencyName = "درهماً إماراتياً";
                    Arabic1CurrencyPartName = "فلس";
                    Arabic2CurrencyPartName = "فلسان";
                    Arabic310CurrencyPartName = "فلوس";
                    Arabic1199CurrencyPartName = "فلساً";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Saudi_Arabia:
                    CurrencyID = 1;
                    CurrencyCode = "SAR";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Saudi Riyal";
                    EnglishPluralCurrencyName = "Saudi Riyals";
                    EnglishCurrencyPartName = "Halala";
                    EnglishPluralCurrencyPartName = "Halalas";
                    Arabic1CurrencyName = "ريال سعودي";
                    Arabic2CurrencyName = "ريالان سعوديان";
                    Arabic310CurrencyName = "ريالات سعودية";
                    Arabic1199CurrencyName = "ريالاً سعودياً";
                    Arabic1CurrencyPartName = "هللة";
                    Arabic2CurrencyPartName = "هللتان";
                    Arabic310CurrencyPartName = "هللات";
                    Arabic1199CurrencyPartName = "هللة";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = true;
                    break;

                case Currencies.Tunisia:
                    CurrencyID = 13;
                    CurrencyCode = "TND";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Tunisian Dinar";
                    EnglishPluralCurrencyName = "Tunisian Dinars";
                    EnglishCurrencyPartName = "milim";
                    EnglishPluralCurrencyPartName = "millimes";
                    Arabic1CurrencyName = "دينار تونسي";
                    Arabic2CurrencyName = "ديناران تونسيان";
                    Arabic310CurrencyName = "دنانير تونسية";
                    Arabic1199CurrencyName = "ديناراً تونسياً";
                    Arabic1CurrencyPartName = "مليم";
                    Arabic2CurrencyPartName = "مليمان";
                    Arabic310CurrencyPartName = "ملاليم";
                    Arabic1199CurrencyPartName = "مليماً";
                    PartPrecision = 3;
                    IsCurrencyPartNameFeminine = false;
                    break;
                case Currencies.Oman:
                    CurrencyID = 2;
                    CurrencyCode = "OMR";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Omani Rial";
                    EnglishPluralCurrencyName = "Omani Rials";
                    EnglishCurrencyPartName = "baisa";
                    EnglishPluralCurrencyPartName = "baisas";
                    Arabic1CurrencyName = "ريال عماني";
                    Arabic2CurrencyName = "ريالان عمانيان";
                    Arabic310CurrencyName = "ريالات عمانية";
                    Arabic1199CurrencyName = "ريالاً عمانياً";
                    Arabic1CurrencyPartName = "بيسة";
                    Arabic2CurrencyPartName = "بيستان";
                    Arabic310CurrencyPartName = "بيسات";
                    Arabic1199CurrencyPartName = "بيسة";
                    PartPrecision = 3;
                    IsCurrencyPartNameFeminine = true;
                    break;
                case Currencies.Qatar:
                    CurrencyID = 4;
                    CurrencyCode = "QAR";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Qatari Riyal";
                    EnglishPluralCurrencyName = "Qatari Riyals";
                    EnglishCurrencyPartName = "Dirham";
                    EnglishPluralCurrencyPartName = "Dirhams ";
                    Arabic1CurrencyName = "ريال قطري";
                    Arabic2CurrencyName = "ريالان قطريان";
                    Arabic310CurrencyName = "ريالات قطرية";
                    Arabic1199CurrencyName = "ريالاً قطرياً";
                    Arabic1CurrencyPartName = "درهم";
                    Arabic2CurrencyPartName = "درهمان";
                    Arabic310CurrencyPartName = "دراهم";
                    Arabic1199CurrencyPartName = "درهم";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;
                case Currencies.Bahrain:
                    CurrencyID = 5;
                    CurrencyCode = "BHD";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Bahraini Dinar";
                    EnglishPluralCurrencyName = "Bahraini Dinars";
                    EnglishCurrencyPartName = "Fils";
                    EnglishPluralCurrencyPartName = "Fils";
                    Arabic1CurrencyName = "دينار بحريني";
                    Arabic2CurrencyName = "ديناران بحرينيان";
                    Arabic310CurrencyName = "دنانير بحرينية";
                    Arabic1199CurrencyName = "ديناراً بحرينياً";
                    Arabic1CurrencyPartName = "فلس";
                    Arabic2CurrencyPartName = "فلسان";
                    Arabic310CurrencyPartName = "فلوس";
                    Arabic1199CurrencyPartName = "فلساً";
                    PartPrecision = 3;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Iraq:
                    CurrencyID = 6;
                    CurrencyCode = "IQD";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Iraqi Dinar";
                    EnglishPluralCurrencyName = "Iraqi Dinars";
                    EnglishCurrencyPartName = "Fils";
                    EnglishPluralCurrencyPartName = "Fils";
                    Arabic1CurrencyName = "دينار عراقي";
                    Arabic2CurrencyName = "ديناران عراقيان";
                    Arabic310CurrencyName = "دنانير عراقية";
                    Arabic1199CurrencyName = "ديناراً عراقياً";
                    Arabic1CurrencyPartName = "فلس";
                    Arabic2CurrencyPartName = "فلسان";
                    Arabic310CurrencyPartName = "فلوس";
                    Arabic1199CurrencyPartName = "فلساً";
                    PartPrecision = 3;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Lebanon:
                    CurrencyID = 7;
                    CurrencyCode = "LBP";
                    IsCurrencyNameFeminine = true;
                    EnglishCurrencyName = "Lebanese Pound";
                    EnglishPluralCurrencyName = "Lebanese Pound";
                    EnglishCurrencyPartName = "";
                    EnglishPluralCurrencyPartName = "";
                    Arabic1CurrencyName = "ليرة لبنانية";
                    Arabic2CurrencyName = "ليرتان لبنانيتان";
                    Arabic310CurrencyName = "ليرات لبنانية";
                    Arabic1199CurrencyName = "ليرة لبنانية";
                    Arabic1CurrencyPartName = "";
                    Arabic2CurrencyPartName = "";
                    Arabic310CurrencyPartName = "";
                    Arabic1199CurrencyPartName = "";
                    PartPrecision = 0;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Yemen:
                    CurrencyID = 9;
                    CurrencyCode = "YER";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Yemeni Rial";
                    EnglishPluralCurrencyName = "Yemeni Rials";
                    EnglishCurrencyPartName = "Fils";
                    EnglishPluralCurrencyPartName = "Fils";
                    Arabic1CurrencyName = "ريال يمني";
                    Arabic2CurrencyName = "ريالان يمنيان";
                    Arabic310CurrencyName = "ريالات يمنية";
                    Arabic1199CurrencyName = "ريالاً يمنياً";
                    Arabic1CurrencyPartName = "فلس";
                    Arabic2CurrencyPartName = "فلسان";
                    Arabic310CurrencyPartName = "فلوس";
                    Arabic1199CurrencyPartName = "فلساً";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;


                case Currencies.Jordan:
                    CurrencyID = 10;
                    CurrencyCode = "JOD";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Jordanian Dinar";
                    EnglishPluralCurrencyName = "Jordanian Dinar";
                    EnglishCurrencyPartName = "Piaster";
                    EnglishPluralCurrencyPartName = "Piasteres";
                    Arabic1CurrencyName = "دينار أردني";
                    Arabic2CurrencyName = "ديناران أردنيان";
                    Arabic310CurrencyName = "دنانير أردنية";
                    Arabic1199CurrencyName = "ديناراً أردنياً";
                    Arabic1CurrencyPartName = "قرش";
                    Arabic2CurrencyPartName = "قرشان";
                    Arabic310CurrencyPartName = "قروش";
                    Arabic1199CurrencyPartName = "قرشاً";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Algeria:
                    CurrencyID = 11;
                    CurrencyCode = "DZD";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Algerian Dinar";
                    EnglishPluralCurrencyName = "Algerian Dinars";
                    EnglishCurrencyPartName = "Santeem";
                    EnglishPluralCurrencyPartName = "Santeems";
                    Arabic1CurrencyName = "دينار جزائري";
                    Arabic2CurrencyName = "ديناران جزائريان";
                    Arabic310CurrencyName = "دنانير جزائرية";
                    Arabic1199CurrencyName = "ديناراً جزائرياً";
                    Arabic1CurrencyPartName = "سنتيم";
                    Arabic2CurrencyPartName = "سنتيمان";
                    Arabic310CurrencyPartName = "سنتيمات";
                    Arabic1199CurrencyPartName = "سنتيم";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Egypt:
                    CurrencyID = 12;
                    CurrencyCode = "EGP";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Egyptian Pound";
                    EnglishPluralCurrencyName = "Egyptian Pounds";
                    EnglishCurrencyPartName = "Piastres";
                    EnglishPluralCurrencyPartName = "Piastres";
                    Arabic1CurrencyName = "جنيه مصري";
                    Arabic2CurrencyName = "جنيهان مصريان";
                    Arabic310CurrencyName = "جنيهات مصرية";
                    Arabic1199CurrencyName = "جنيهاً مصريا";
                    Arabic1CurrencyPartName = "قرش";
                    Arabic2CurrencyPartName = "قرشان";
                    Arabic310CurrencyPartName = "قروش";
                    Arabic1199CurrencyPartName = "قرش";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Sudan:
                    CurrencyID = 14;
                    CurrencyCode = "SDG";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Sudanese Pound";
                    EnglishPluralCurrencyName = "Sudanese Pounds";
                    EnglishCurrencyPartName = "Piastres";
                    EnglishPluralCurrencyPartName = "Piastres";
                    Arabic1CurrencyName = "جنيه سوداني";
                    Arabic2CurrencyName = "جنيهان سودانيان";
                    Arabic310CurrencyName = "جنيهات سودانية";
                    Arabic1199CurrencyName = "جنيهاً سودانياً";
                    Arabic1CurrencyPartName = "قرش";
                    Arabic2CurrencyPartName = "قرشان";
                    Arabic310CurrencyPartName = "قروش";
                    Arabic1199CurrencyPartName = "قرش";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Morocco:
                    CurrencyID = 15;
                    CurrencyCode = "MAD";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Moroccan Dirham";
                    EnglishPluralCurrencyName = "Moroccan Dirhams";
                    EnglishCurrencyPartName = "Centime";
                    EnglishPluralCurrencyPartName = "Centimes";
                    Arabic1CurrencyName = "درهم مغربي";
                    Arabic2CurrencyName = "درهمان مغربيان";
                    Arabic310CurrencyName = "دراهم مغربية";
                    Arabic1199CurrencyName = "درهماً مغربياً";
                    Arabic1CurrencyPartName = "سنتيم";
                    Arabic2CurrencyPartName = "سنتيمان";
                    Arabic310CurrencyPartName = "سنتيمات";
                    Arabic1199CurrencyPartName = "سنتيماً";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Libya:
                    CurrencyID = 16;
                    CurrencyCode = "LYD";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Libyan Dinar";
                    EnglishPluralCurrencyName = "Libyan Dinars";
                    EnglishCurrencyPartName = "Dirham";
                    EnglishPluralCurrencyPartName = "Dirhams";
                    Arabic1CurrencyName = "دينار ليبي";
                    Arabic2CurrencyName = "ديناران ليبيّان";
                    Arabic310CurrencyName = "دنانير ليبيّة";
                    Arabic1199CurrencyName = "ديناراً ليبيّاً";
                    Arabic1CurrencyPartName = "درهم";
                    Arabic2CurrencyPartName = "درهمان";
                    Arabic310CurrencyPartName = "دراهم";
                    Arabic1199CurrencyPartName = "درهم";
                    PartPrecision = 3;
                    IsCurrencyPartNameFeminine = false;
                    break;
                case Currencies.Somalia:
                    CurrencyID = 17;
                    CurrencyCode = "SOS";
                    IsCurrencyNameFeminine = false;
                    EnglishCurrencyName = "Somali Shilling";
                    EnglishPluralCurrencyName = "Somali Shillings";
                    EnglishCurrencyPartName = "Senti";
                    EnglishPluralCurrencyPartName = "Sentis";
                    Arabic1CurrencyName = "شلن صومالي";
                    Arabic2CurrencyName = "شلنان صوماليان";
                    Arabic310CurrencyName = "شلنات صومالية";
                    Arabic1199CurrencyName = "شلناَ صومالياً";
                    Arabic1CurrencyPartName = "سنت";
                    Arabic2CurrencyPartName = "سنتان";
                    Arabic310CurrencyPartName = "سنتات";
                    Arabic1199CurrencyPartName = "سنت";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;

                case Currencies.Turkey:
                    CurrencyID = 18;
                    CurrencyCode = "TRY";
                    IsCurrencyNameFeminine = true;
                    EnglishCurrencyName = " Turkish Lira";
                    EnglishPluralCurrencyName = " Turkish Liras";
                    EnglishCurrencyPartName = "Kuruş";
                    EnglishPluralCurrencyPartName = "Kuruş";
                    Arabic1CurrencyName = "ليرة تركية";
                    Arabic2CurrencyName = "ليرتان تركيتان";
                    Arabic310CurrencyName = "ليرات تركية";
                    Arabic1199CurrencyName = "ليرة تركية";
                    Arabic1CurrencyPartName = "قرش";
                    Arabic2CurrencyPartName = "قرشان";
                    Arabic310CurrencyPartName = "قروش";
                    Arabic1199CurrencyPartName = "قرش";
                    PartPrecision = 2;
                    IsCurrencyPartNameFeminine = false;
                    break;
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Currency ID
        /// </summary>
        public int CurrencyID { get; set; }

        /// <summary>
        /// Standard Code
        /// Syrian Pound: SYP
        /// UAE Dirham: AED
        /// </summary>
        public string CurrencyCode { get; set; }

        /// <summary>
        /// Is the currency name feminine ( Mua'anath مؤنث)
        /// ليرة سورية : مؤنث = true
        /// درهم : مذكر = false
        /// </summary>
        public Boolean IsCurrencyNameFeminine { get; set; }

        /// <summary>
        /// English Currency Name for single use
        /// Syrian Pound
        /// UAE Dirham
        /// </summary>
        public string EnglishCurrencyName { get; set; }

        /// <summary>
        /// English Plural Currency Name for Numbers over 1
        /// Syrian Pounds
        /// UAE Dirhams
        /// </summary>
        public string EnglishPluralCurrencyName { get; set; }

        /// <summary>
        /// Arabic Currency Name for 1 unit only
        /// ليرة سورية
        /// درهم إماراتي
        /// </summary>
        public string Arabic1CurrencyName { get; set; }

        /// <summary>
        /// Arabic Currency Name for 2 units only
        /// ليرتان سوريتان
        /// درهمان إماراتيان
        /// </summary>
        public string Arabic2CurrencyName { get; set; }

        /// <summary>
        /// Arabic Currency Name for 3 to 10 units
        /// خمس ليرات سورية
        /// خمسة دراهم إماراتية
        /// </summary>
        public string Arabic310CurrencyName { get; set; }

        /// <summary>
        /// Arabic Currency Name for 11 to 99 units
        /// خمس و سبعون ليرةً سوريةً
        /// خمسة و سبعون درهماً إماراتياً
        /// </summary>
        public string Arabic1199CurrencyName { get; set; }

        /// <summary>
        /// Decimal Part Precision
        /// for Syrian Pounds: 2 ( 1 SP = 100 parts)
        /// for Tunisian Dinars: 3 ( 1 TND = 1000 parts)
        /// </summary>
        public Byte PartPrecision { get; set; }

        /// <summary>
        /// Is the currency part name feminine ( Mua'anath مؤنث)
        /// هللة : مؤنث = true
        /// قرش : مذكر = false
        /// </summary>
        public Boolean IsCurrencyPartNameFeminine { get; set; }

        /// <summary>
        /// English Currency Part Name for single use
        /// Piaster
        /// Fils
        /// </summary>
        public string EnglishCurrencyPartName { get; set; }

        /// <summary>
        /// English Currency Part Name for Plural
        /// Piasters
        /// Fils
        /// </summary>
        public string EnglishPluralCurrencyPartName { get; set; }

        /// <summary>
        /// Arabic Currency Part Name for 1 unit only
        /// قرش
        /// هللة
        /// </summary>
        public string Arabic1CurrencyPartName { get; set; }

        /// <summary>
        /// Arabic Currency Part Name for 2 unit only
        /// قرشان
        /// هللتان
        /// </summary>
        public string Arabic2CurrencyPartName { get; set; }

        /// <summary>
        /// Arabic Currency Part Name for 3 to 10 units
        /// قروش
        /// هللات
        /// </summary>
        public string Arabic310CurrencyPartName { get; set; }

        /// <summary>
        /// Arabic Currency Part Name for 11 to 99 units
        /// قرشاً
        /// هللةً
        /// </summary>
        public string Arabic1199CurrencyPartName { get; set; }
        #endregion
    }
}
