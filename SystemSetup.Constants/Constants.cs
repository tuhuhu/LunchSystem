//------------------------------------------------------------------------
// Version		: 001
// Designer		: GiangVT
// Programmer	: GiangVT
// Date			: 2014/09/03
// Comment		: Create new
//------------------------------------------------------------------------

using System.Collections.Specialized;

namespace SystemSetup.Constants
{

    public enum ReturnCode
    {
        Success = 0,
        Failure = -1,
    }

    public enum ContractRegistSource
    {
        CreateNewContract = 0,
        FromEstimate = 1,
        CopyContract = 2,
        ExpandContract = 3,
        ChangeContract = 4,
        FinishContract = 5
    }

    public enum SendMailFromSource
    {
        Extend = 1,
        ContractView = 2,
        ContractFinishNormal = 3,
        ContractFinishInMiddle = 4,
        ContractFinishMiddleWithSpecialPayment = 5,
        ContractEdit = 6,
    }

    /// <summary>
    /// Mst_BusinessPartnerTradeInfo.BP_TRADE_TYPE
    /// </summary>
    public class BusinessPartnerTradeType {
        public const string Billing = "1";
        public const string Payment = "2";
        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            {Billing, "請求"},
            {Payment, "支払"}
        }.AsReadOnly();
    }

    /// <summary>
    /// Mst_BusinessPartnerStaff.BP_BILL_STAFF_FLG
    /// </summary>
    public class BusinessPartnerBillStaffFlag
    {
        public const string On = "1";
        public const string Off = "2";
    }

    public class CompanyId
    {
        public const string PUBLIC = "0";
    }

    public class CorporationCd
    {
        public const string IENTER = "IENTER";
    }

    /// <summary>
    /// Mst_ContractFirmBankAccountInfo.DEFAULT_ACCOUNT_FLG
    /// </summary>
    public class DefaultBankAccountFlag
    {
        public const string On = "1";
        public const string Off = "0";
    }

    public class Flag
    {
        public const string ON = "1";
        public const string OFF = "0";
    }

    public class Authority_Type
    {
        public const string All_company = "0";
        public const string Organization = "1";
        public const string Person = "2";
    }

    public class TextKey
    {
        public const string PRE_EXCEPTION_MSG = "IENTER_SYS_Exception_Message";
    }

    /// <summary>
    /// Common constants definition
    /// </summary>
    public class Constant
    {
        /// <summary>Key name for login user in session object</summary>
        public const string SESSION_LOGIN_USER = "SESSION_LOGIN_USER";

        /// <summary> Menu state cookie name </summary>
        public const string MENU_STATE_COOKIE = "MENU_STATE_COOKIE";

        // Set password out of date
        public const string PASSWORD_OUT_OF_DATE = "PASSWORD_OUT_OF_DATE";

        // Set display password
        public const string DISPLAY_PASSWORD = "••••••••••••••••••••••••••••••••";

        // Set string validate password
        public const string REGEX_PASSWORD = @"^[a-zA-Z0-9_\!\""\#\$\%\&\'\(\)\=\~\|\-\^\@\[\;\:\]\,\.\/\`\{\+\*\}\>\?]*$";

        /// <summary>Key name for invalid login user in session object</summary>
        public const string SESSION_INVALID_LOGIN_USER = "SESSION_INVALID_LOGIN_USER";

        /// <summary>Key name for  in session object</summary>
        public const string SESSION_TRANSITION_DESTINATION = "SESSION_TRANSITION_DESTINATION";

        /// <summary>Max size image</summary>
        public const int MaxContentLength = 1024 * 500; //500Kb;

        /// <summary>Input image</summary>
        public const string Input_Image = "/Content/img/input_image.jpg";

        /// <summary>No Image</summary>
        public const string No_Image = "/Content/img/no_image.jpg";

        /// <summary>Default value </summary>
        public const string DEFAULT_VALUE = "0";

        /// <summary>Project No max length</summary>
        public const int PROJECT_NO_MAX_LENGTH = 10;

        //Define TOTAL ACTUAL WORK TIME
        public const int TOTAL_ACTUAL_WORK_TIME = 744;

        /// <summary>max month change pass</summary>
        public const int MAX_MONTH_EXPIRED_PASSWORD = 3;

        /// <summary>Deletation value </summary>
        public const string DELEGATION = "2";

        /// <summary>Package value </summary>
        public const string PACKAGE = "1";

        /// <summary>Maintain value </summary>
        public const string MAINTAINANCE = "3";

        /// <summary>min input account</summary>
        public const int MIN_INPUT_ACCOUNT = 6;

        /// <summary>max input account</summary>
        public const int MAX_INPUT_ACCOUNT = 100;

        /// <summary>min input password</summary>
        public const int MIN_INPUT_PASS = 6;

        /// <summary>max input password</summary>
        public const int MAX_INPUT_PASS = 32;

        /// <summary>strength input password</summary>
        public const int STRENGTH_INPUT_PASS = 3;

        /// <summary>Copied estimation</summary>
        public const int COPIED = 1;

        /// <summary> NO Copy estimation</summary>
        public const int NO_COPY = 0;

        /// <summary> default</summary>
        public const int DEFAULT_USER_ID = 0;

        /// <summary></summary>
        public const int PERCENTAGE = 100;

        /// <summary></summary>
        public const int TOKEN_ERROR_CODE = -2147467259;

        public const int TIME_OUT = 419;
        public const int NOT_FOUND = 404;

        // request has been fulfilled and resulted in a new resource being created
        public const int CREATED = 201;

        // response for successful HTTP requests
        public const int SUCCESSFUL = 200;
        public const int INTERNAL_SERVER_ERROR = 500;
        public const int EXPECTATION_FAILED = 417;

        /// <summary> Default unit time: 30 mins</summary>
        public const int DEFAULT_UNIT_TIME = 30;
        /// <summary> Default unit quantity: 1</summary>
        public const int DEFAULT_UNIT_QTY = 1;
        /// <summary> Default unit price: 0</summary>
        public const int DEFAULT_UNIT_PRICE = 0;

        /// <summary> Default tax rate</summary>
        public const int DEFAULT_TAX_RATE = 8;
        /// <summary> Maximum TAX_RATE field's length</summary>
        public const int TAX_RATE_MAX_LENGTH = 5;
        /// <summary> Minimum tax rate</summary>
        public const int MIN_TAX_RATE = 0;
        /// <summary> Maximum tax rate</summary>
        public const decimal MAX_TAX_RATE = 99.99m;

        /// <summary> Maximum BP_NAME_DISP's length</summary>
        public const int BP_NAME_DISP_MAX_LENGTH = 40;

        /// <summary> Maximum PRJ_NAME_MAX's length</summary>
        public const int MAX_PRJ_NAME = 100;

        /// <summary> Maximum TITLE_MAX_LENGTH's length</summary>
        public const int TITLE_MAX_LENGTH = 100;

        /// <summary> Maximum negative-money-max's length</summary>
        public const int NEGATIVE_MONEY_MAX_MAX_LENGTH = 14;
        /// <summary> Maximum negative-money-max field's min value</summary>
        public const long NEGATIVE_MONEY_MAX_MIN_VALUE = -9999999999999;
        /// <summary> Maximum negative-money-max field's max value</summary>
        public const long NEGATIVE_MONEY_MAX_MAX_VALUE = 9999999999999;

        /// <summary> Maximum negative-money field's length</summary>
        public const int NEGATIVE_MONEY_MAX_LENGTH = 10;
        /// <summary> Maximum negative-money field's min value</summary>
        public const long NEGATIVE_MONEY_MIN_VALUE = -999999999;
        /// <summary> Maximum negative-money field's max value</summary>
        public const long NEGATIVE_MONEY_MAX_VALUE = 999999999;

        /// <summary> Maximum money field's length</summary>
        public const int MONEY_MAX_LENGTH = 9;
        /// <summary> Maximum money field's min value</summary>
        public const long MONEY_MIN_VALUE = 0;
        /// <summary> Maximum money field's max value</summary>
        public const long MONEY_MAX_VALUE = 999999999;

        /// <summary> Maximum PRJ_NAME, PRJ_DSP_NAME and PRJ_DESC_NAME field's length</summary>
        public const int PROJECT_NAME_MAX_LENGTH = 100;
        public const int WORKING_CONTENT_MAX_LENGTH = 50;

        /// <summary> Maximum DELIVERY_LOCATION and DELIVERABLES field's length</summary>
        public const int DELIVER_DETAILED_MAX_LENGTH = 25;
        public const int WORKER_NAME_MAX_LENGTH = 25;

        /// <summary> Maximum EST_NO_PREFIX field's length</summary>
        public const int EST_NO_PREFIX_MAX_LENGTH = 3;

        /// <summary> Maximum EST_NO_YM field's length</summary>
        public const int EST_NO_YM_MAX_LENGTH = 6;

        /// <summary> Maximum NOMEN field's length</summary>
        public const int NOMEN_MAX_LENGTH = 40;

        /// <summary> Maximum MAX_AREA_LENGTH field's length</summary>
        public const int MAX_AREA_LENGTH = 4000;

        /// <summary> Maximum QTY field's length</summary>
        public const int QTY_MAX_LENGTH = 8;

        /// <summary> Maximum TEL_NO field's length</summary>
        public const int TEL_NO_MAX_LENGTH = 13;

        /// <summary> Maximum FAX_NO field's length</summary>
        public const int FAX_NO_MAX_LENGTH = 13;

        /// <summary> Maximum BP_ZIP_CD field's length</summary>
        public const int ZIP_CD_MAX_LENGTH = 8;

        /// <summary> Maximum STAFF_NAME field's length</summary>
        public const int STAFF_NAME_MAX_LENGTH = 50;

        /// <summary> Maximum STAFF_GROUP_NAME field's length</summary>
        public const int STAFF_GROUP_NAME_MAX_LENGTH = 30;
        /// <summary> Maximum EMAIL_ADDR field's length</summary>
        public const int EMAIL_ADDR_MAX_LENGTH = 100;
        /// <summary> Maximum STAFF_TEL field's length</summary>
        public const int STAFF_TEL_MAX_LENGTH = 3;
        /// <summary> Maximum STAFF_FAX field's length</summary>
        public const int STAFF_FAX_MAX_LENGTH = 3;
        /// <summary> Maximum URL field's length</summary>
        public const int URL_MAX_LENGTH = 100;
        /// <summary> Maximum Adress field's length</summary>
        public const int ADRESS_MAX_LENGTH = 50;

        /// <summary> Minimum QTY</summary>
        public const int MIN_QTY = 0;
        /// <summary> Maximum QTY</summary>
        public const decimal MAX_QTY = 99999.99m;

        /// <summary> Maximum UNIT_TYPE field's length</summary>
        public const int UNIT_TYPE_MAX_LENGTH = 2;

        /// <summary> Maximum BASE_TIME field's length</summary>
        public const int BASE_TIME_MAX_LENGTH = 6;
        public const decimal MAX_BASE_TIME = 999.99m;
        public const decimal MIN_BASE_TIME = -999.99m;
        /// <summary> Maximum UNIT_TIME field's length</summary>
        public const int UNIT_TIME_MAX_LENGTH = 5;
        /// <summary> Minimum UNIT_TIME</summary>
        public const int UNIT_TIME_MIN = 1;
        /// <summary> Maximum UNIT_TIME</summary>
        public const int UNIT_TIME_MAX = 60;
        public const int MAX_FORMAT_PATH = 255;
        public const int MAX_FORMAT_NAME = 15;
        public const int MAX_DETAIL_LINE = 2;

        /// <summary> Maximum NVARCHAR(MAX)'s length</summary>
        public const int NVARCHAR_MAX_MAX_LENGTH = 4000;
        /// <summary> Maximum NVARCHAR's length</summary>
        public const int NVARCHAR_MAX_LENGTH = 255;

        /// <summary> Maximum for Deposit y_m field's length</summary>
        public const int PAY_DEPOSIT_YM_MAX_LENGTH = 6;

        /// <summary> Maximum for Account CD field's length</summary>
        public const int PAY_ACCOUNT_CD_MAX_LENGTH = 4;

        public class UpdateSuccessScreen
        {
            public const string EstimateRegist = "EstimateRegist";

            public const string ContractRegist = "ContractRegist";
        }

        /// <summary>File not Found</summary>
        public const string FILE_NOT_FOUND = "The remote server returned an error: (404) Not Found.";

        /// <summary>Close last day in month</summary>
        public const string CLOSE_LASTDAY_MONTH = "末日締";

        /// <summary>Pay last day in month</summary>
        public const string PAY_LASTDAY_MONTH = "末日払";

        /// <summary>Close last day in month</summary>
        public const string CLOSE_DAY = "日締";

        /// <summary>Pay last day in month</summary>
        public const string PAY_DAY = "日払";

        /// <summary>Key name for scroll top in session object</summary>
        public const string SESSION_SCROLL_TOP = "SESSION_SCROLL_TOP";

        public class SqlDateRange
        {
            public const string MIN = "1/1/1753";

            public const string MAX = "12/31/9999";

        }

        public const int DEFAULT_SUB_CONTRACT_TYPE_VALUE = 1;

        public const int MAX_MONEY_VALUE = 999999999;

        public const int YEAR_LENGTH = 4;

        // Fixed working hour in month
        public const int WORKING_HOUR_IN_MONTH = 165;

        public class BillingType
        {
            public const string TYPE_ENTRUSTMENT = "1";

            public const string TYPE_SES = "2";
        }
        //環境依存文字
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS1 = "<>&\" ©@®〒%";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS2 = "㈷㈪㈬㈮㊗㊊㊌㊎㋀㋂㋄㋆㋈㋊㏠㏢㏤㏦㏨㏪㏬㏮㏰㏲㏴㏶㏸㏺㏼㏾㈰㈫㈭㈯㊐㊋㊍㊏㋁㋃㋅㋇㋉㋋㏡㏣㏥㏧㏩㏫㏭㏯㏱㏳㏵㏷㏹㏻㏽";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS3 = "←↑↔↖↘⇒⇔⇐⇑⇓⇕⇖⇗⇘⇙→↓↕↗↙";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS4 = "㋐㋒㋔㋖㋘㊑㊓㊕㊚㊜㊡㊫㊭㊯㊞㊩㊘㊪㋑㋓㋕㋗㋙㊒㊔㊟㊛㊣㊢㊬㊮㊰㊖㊝㊙";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS5 = "㈳㈵㈸㈻㈽㈿㉃㈴㈶㈺㈼㈾㉀";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS6 = "†━┌┏┒┕┘┛┞┡┤┧┪┭┰┳┶┹┼┿╂╅╈╋‡│┍┐┓┖┙├┟┢┥┨┫┮┱┴┷┺┽╀╃╆╉═─┃┎┑└┗┚┝┠┣┦┩┬┯┲┵┸┻┾╁╄╇╊║";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS7 = "╌╍╒╕╘╛╞╡╤╧╪▀▃▆▉▌▏▕╓╖╙╜╟╢╥╨╫▁▄▇▊▍▐╔╗╚╝╠╣╦╩╬▂▅█▋▎░▒▓";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS8 = "♭►♀☆☺〇♯◄♂★☻‼";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS9 = "㍱㍴㎀㎃㎉㎌㎒㎕㎛㎤㎧㎪㎭㎰㎳㎶㎹㎼㎿㏂㏅㏈㏎㏑㏔㏗㏚㎡㎇㎖㏝βΦ㍲㍵㎁㎄㎊㎍㎓㎙㎟㎥㎨㎫㎮㎱㎴㎷㎺㎽㏀㏃㏆㏉㏏㏒㏕㏘㏛㎅㏋㎢㎗℃α㍳㍶㎂㎈㎋㎑㎔㎚㎠㎦㎩㎬㎯㎲㎵㎸㎻㎾㏁㏌㏇㏊㏐㏓㏖㏙㏜㎆㎐㎣㎘₧";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS10 = "¥$¢£";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS11 = "㌀㌂㌄㌈㌊㌋㌌㌏㌐㌕㌓㌚㌜㌞㌟㌡㌤㌩㌭㌰㌳㌷㌺㌾㍁㍄㍃㍆㍋㍏㍓㍇㍌㍐㍔㌁㌅㌆㌇㌉㍿㌎㌒㌖㌗㌙㌛㌝㌠㌥㌨㌪㌮㌱㌴㌸㌼㌿㍂㍅㍈㍎㍒㌬㌯㌲㌵㌹㌽㍀";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS12 = "①③⑤⑦⑨⑪⑬⑮⑰⑲②④⑥⑧⑩⑫⑭⑯⑱⑳㊤㊨㊥㊦㊧";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS13 = "ⅠⅡⅢⅣⅤⅥⅦⅧⅨⅩ";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS14 = "ⅰⅱⅲⅳⅴⅵⅶⅷⅸⅹ";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS15 = "㌃㌔㌢㌦㌫㌻㍊㍑㌍㌘㌣㌧㌶㍉㍍㍗";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS16 = "№㏍℡〠㍻㍽㎜㎎㏄㈱㈹♠♥♤♧♡㍼㍾㎝㎏㎞㈲♣";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS17 = "“”≒≡∫∮Σ√⊥∠∟⊿∵∩∪￢￤＇＂";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS18 = "纊褜鍈銈蓜俉炻昱棈鋹曻彅丨仡仼伀伃伹佖侒侊侚侔俍偀倢俿倞偆偰偂傔僴僘兊兤冝冾凬刕劜劦勀勛匀匇匤卲厓厲叝﨎咜咊咩哿喆坙坥垬埈埇﨏塚增墲夋奓奛奝奣妤妺孖寀甯寘寬尞岦岺峵崧嵓﨑嵂嵭嶸嶹巐弡弴彧德忞恝悅悊惞惕愠惲愑愷愰憘戓抦揵摠撝擎敎昀昕昻昉昮昞昤晥晗晙晴晳暙暠暲暿曺朎朗杦枻桒柀栁桄棏﨓楨﨔榘槢樰橫橆橳橾櫢櫤毖氿汜沆汯泚洄涇浯涖涬淏淸淲淼渹湜渧渼溿澈澵濵瀅瀇瀨炅炫焏焄煜煆煇凞燁燾犱犾猤猪獷玽珉珖珣珒琇珵琦琪琩琮瑢璉璟甁畯皂皜皞皛皦益睆劯砡硎硤硺礰礼神祥禔福禛竑竧靖竫箞精絈絜綷綠緖繒罇羡羽茁荢荿菇菶葈蒴蕓蕙蕫﨟薰蘒﨡蠇裵訒訷詹誧誾諟諸諶譓譿賰賴贒赶﨣軏﨤逸遧郞都鄕鄧釚釗釞釭釮釤釥鈆鈐鈊鈺鉀鈼鉎鉙鉑鈹鉧銧鉷鉸鋧鋗鋙鋐﨧鋕鋠鋓錥錡鋻﨨錞鋿錝錂鍰鍗鎤鏆鏞鏸鐱鑅鑈閒隆﨩隝隯霳霻靃靍靏靑靕顗顥飯飼餧館馞驎髙髜魵魲鮏鮱鮻鰀鵰鵫鶴鸙黑";
        public const string ENVIRONMENT_DEPENDENT_CHARACTERS19 = "纊褜鍈銈蓜俉炻昱棈鋹曻彅丨仡仼伀伃伹佖侒侊侚侔俍偀倢俿倞偆偰偂傔僴僘兊兤冝冾凬刕劜劦勀勛匀匇匤卲厓厲叝﨎咜咊咩哿喆坙坥垬埈埇﨏塚增墲夋奓奛奝奣妤妺孖寀甯寘寬尞岦岺峵崧嵓﨑嵂嵭嶸嶹巐弡弴彧德忞恝悅悊惞惕愠惲愑愷愰憘戓抦揵摠撝擎敎昀昕昻昉昮昞昤晥晗晙晴晳暙暠暲暿曺朎朗杦枻桒柀栁桄棏﨓楨﨔榘槢樰橫橆橳橾櫢櫤毖氿汜沆汯泚洄涇浯涖涬淏淸淲淼渹湜渧渼溿澈澵濵瀅瀇瀨炅炫焏焄煜煆煇凞燁燾犱犾猤猪獷玽珉珖珣珒琇珵琦琪琩琮瑢璉璟甁畯皂皜皞皛皦益睆劯砡硎硤硺礰礼神祥禔福禛竑竧靖竫箞精絈絜綷綠緖繒罇羡羽茁荢荿菇菶葈蒴蕓蕙蕫﨟薰蘒﨡蠇裵訒訷詹誧誾諟諸諶譓譿賰賴贒赶﨣軏﨤逸遧郞都鄕鄧釚釗釞釭釮釤釥鈆鈐鈊鈺鉀鈼鉎鉙鉑鈹鉧銧鉷鉸鋧鋗鋙鋐﨧鋕鋠鋓錥錡鋻﨨錞鋿錝錂鍰鍗鎤鏆鏞鏸鐱鑅鑈閒隆﨩隝隯霳霻靃靍靏靑靕顗顥飯飼餧館馞驎髙髜魵魲鮏鮱鮻鰀鵰鵫鶴鸙黑";

    }

    public class WindowName
    {

        public const string COOKIE_NAME = "WindowName";

        public const string MAIN = "Main";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { "Login", MAIN }
        }.AsReadOnly();
    }

    public class MailSourceType
    {
        public const string EXTEND = "1";
        public const string CONTRACT_VIEW = "2";
        public const string CONTRACT_FINISH_NORMAL = "3";
        public const string CONTRACT_FINISH_MIDDLE = "4";
        public const string CONTRACT_FINISH_MIDDLE_WITH_SPECIAL_PAYMENT = "5";
        public const string CONTRACT_EDIT = "6";
        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            {EXTEND, "延長"},
            {CONTRACT_VIEW, "新規決定"},
            {CONTRACT_FINISH_NORMAL, "完全終了"},
            {CONTRACT_FINISH_MIDDLE, "途中終了"},
            {CONTRACT_FINISH_MIDDLE_WITH_SPECIAL_PAYMENT, "途中終了＋特殊精算"},
            {CONTRACT_EDIT, "新規決定"},
        }.AsReadOnly();
    }

    public class ContractInputType
    {
        public const string OUTSOURCING = "1";
        public const string STAFF = "2";
        public const string CONTRACT = "3";
        public const string GENERAL_DISPATCH = "4";
        public const string SPECIFIC_DISPATCH = "5";
        public const string TRUSTEE = "6";
        public const string PRODUCT_SALE = "7";
        public const string DELEGATION = "8";
        public const string MAINTAINANCE = "9";
        public const string PRODUCT_SALE_MONTH = "A";

        public static string[] SESTypes = { OUTSOURCING, STAFF, CONTRACT, GENERAL_DISPATCH, SPECIFIC_DISPATCH, DELEGATION };
        public static string[] TrusteeTypes = { TRUSTEE, PRODUCT_SALE, MAINTAINANCE, PRODUCT_SALE_MONTH };
        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { OUTSOURCING, "外注" },
            { STAFF, "社員" },
            { CONTRACT, "契約" },
            { DELEGATION, "委任" },
            { GENERAL_DISPATCH, "派遣" },
            //{ SPECIFIC_DISPATCH, "特定派遣" },
            { TRUSTEE, "受託" },
            { MAINTAINANCE, "保守" },
            { PRODUCT_SALE, "物販" },                        
            { PRODUCT_SALE_MONTH, "月額" }
        }.AsReadOnly();
    }

    public class ContractState
    {
        public const string CREATE_NEW = "00";

        public const string CREATE_ESTIMATE = "10";

        public const string EDIT_ESTIMATE = "17";

        public const string ISSUE_ESTIMATE = "15";

        public const string REISSUE_ESTIMATE = "19";

        public const string DELETE_ESTIMATE = "91";

        public const string CREATE_CONTRACT = "30";
        //契約決定
        public const string IMPLEMENT_CONTRACT = "35";
        //契約変更
        public const string CHANGE_CONTRACT = "37";

        public const string CONTRACTED = "90";
        //途中終了
        public const string FINISH_IN_MIDDLE = "95";
        //延長終了
        public const string FINISH_EXPAND = "97";
        //完全終了
        public const string FINISH_COMPLETE = "99";

        //DUMMY
        public const string ISSUE_BILLING = "999";
    }

    public class ContractConditionFlag
    {
        public const int FinishNormal = 0;
        public const int FinishInMiddle = 1;
        public const int FinishMiddleWithSpecialPayment = 2;
    }

    public class ProjectState
    {
        public const string CREATE_NEW = "00";
        public const string DELETE_ESTIMATE = "91";
        public const string DELETE_CONTRACT = "93";
        public const string FINISH_PROJECT = "99";
        public const int PRJ_STATE_SELECT_LIMIT = 90;
    }

    public class BillingState
    {
        public const string CREATE_NEW = "00";

        public const string FINISH_BILLING = "99";
    }

    public class DeleteFlag
    {
        public const string NON_DELETE = "0";

        public const string DELETE = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { NON_DELETE, "" },
            { DELETE, "削除" }
        }.AsReadOnly();
    }

    public class StatusMeal
    {
        public const int CREATED = 1;

        public const int BOOKED = 2;

        public const int DELETED = 3;
       
    }

    public class UnitMeal
    {
        public const long NORMAL = 35000;
        

    }

    public class RedInvoiceType
    {

        public const string BLACK_INVOICE = "0";

        public const string RED_INVOICE = "1";

    }

    public class IssueFlag
    {
        public const string NON_ISSUE = "0";
        public const string ISSUED = "1";
    }

    public class EstimateEditFlag
    {

        public const string Normal = "0";

        public const string Edit = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Normal, "通常" },
                { Edit, "編集中" }                
            }.AsReadOnly();

    }

    public class CollectContractMonths
    {
        public const string None = "0";

        public const string Month_02 = "2";

        public const string Month_03 = "3";

        public const string Month_04 = "4";

        public const string Month_05 = "5";

        public const string Month_06 = "6";

        public const string Month_07 = "7";

        public const string Month_08 = "8";

        public const string Month_09 = "9";

        public const string Month_10 = "10";

        public const string Month_11 = "11";

        public const string Month_12 = "12";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { None, "" },
            { Month_02, "2月" },
            { Month_03, "3月" },
            { Month_04, "4月" },
            { Month_05, "5月" },
            { Month_06, "6月" },
            { Month_07, "7月" },
            { Month_08, "8月" },
            { Month_09, "9月" },
            { Month_10, "10月" },
            { Month_11, "11月" },
            { Month_12, "12月" }
        }.AsReadOnly();
    }

    public class CollectContractMonthsProductSale
    {
        public const string None = "0";

        public const string Month_02 = "2";

        public const string Month_03 = "3";

        public const string Month_04 = "4";

        public const string Month_05 = "5";

        public const string Month_06 = "6";

        public const string Month_07 = "7";

        public const string Month_08 = "8";

        public const string Month_09 = "9";

        public const string Month_10 = "10";

        public const string Month_11 = "11";

        public const string Month_12 = "12";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { None, "" },
            { Month_02, "2" },
            { Month_03, "3" },
            { Month_04, "4" },
            { Month_05, "5" },
            { Month_06, "6" },
            { Month_07, "7" },
            { Month_08, "8" },
            { Month_09, "9" },
            { Month_10, "10" },
            { Month_11, "11" },
            { Month_12, "12" }
        }.AsReadOnly();
    }

    public class ClosingMonthType
    {
        public const string None = "0";

        public const string Month_01 = "1";

        public const string Month_02 = "2";

        public const string Month_03 = "3";

        public const string Month_04 = "4";

        public const string Month_05 = "5";

        public const string Month_06 = "6";

        public const string Month_07 = "7";

        public const string Month_08 = "8";

        public const string Month_09 = "9";

        public const string Month_10 = "10";

        public const string Month_11 = "11";

        public const string Month_12 = "12";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { None, "" },
            { Month_01, "1月" },
            { Month_02, "2月" },
            { Month_03, "3月" },
            { Month_04, "4月" },
            { Month_05, "5月" },
            { Month_06, "6月" },
            { Month_07, "7月" },
            { Month_08, "8月" },
            { Month_09, "9月" },
            { Month_10, "10月" },
            { Month_11, "11月" },
            { Month_12, "12月" }
        }.AsReadOnly();
    }

    public class ClosingDayType
    {
        public const string None = "0";

        public const string Day1 = "1";

        public const string Day2 = "2";

        public const string Day3 = "3";

        public const string Day4 = "4";

        public const string Day5 = "5";

        public const string Day6 = "6";

        public const string Day7 = "7";

        public const string Day8 = "8";

        public const string Day9 = "9";

        public const string Day10 = "10";

        public const string Day11 = "11";

        public const string Day12 = "12";

        public const string Day13 = "13";

        public const string Day14 = "14";

        public const string Day15 = "15";

        public const string Day16 = "16";

        public const string Day17 = "17";

        public const string Day18 = "18";

        public const string Day19 = "19";

        public const string Day20 = "20";

        public const string Day21 = "21";

        public const string Day22 = "22";

        public const string Day23 = "23";

        public const string Day24 = "24";

        public const string Day25 = "25";

        public const string Day26 = "26";

        public const string Day27 = "27";

        public const string Day31 = "31";

        public const int StandardValue = 30;

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            //{ None, "" },
            { Day1, "1日" },
            { Day2, "2日" },
            { Day3, "3日" },
            { Day4, "4日" },
            { Day5, "5日" },
            { Day6, "6日" },
            { Day7, "7日" },
            { Day8, "8日" },
            { Day9, "9日" },
            { Day10, "10日" },
            { Day11, "11日" },
            { Day12, "12日" },
            { Day13, "13日" },
            { Day14, "14日" },
            { Day15, "15日" },
            { Day16, "16日" },
            { Day17, "17日" },
            { Day18, "18日" },
            { Day19, "19日" },
            { Day20, "20日" },
            { Day21, "21日" },
            { Day22, "22日" },
            { Day23, "23日" },
            { Day24, "24日" },
            { Day25, "25日" },
            { Day26, "26日" },
            { Day27, "27日" },
            { Day31, "末日" }
        }.AsReadOnly();
    }

    public class ClosingDayTypeContractRegister
    {
        public const string None = "0";

        public const string Day1 = "1";

        public const string Day2 = "2";

        public const string Day3 = "3";

        public const string Day4 = "4";

        public const string Day5 = "5";

        public const string Day6 = "6";

        public const string Day7 = "7";

        public const string Day8 = "8";

        public const string Day9 = "9";

        public const string Day10 = "10";

        public const string Day11 = "11";

        public const string Day12 = "12";

        public const string Day13 = "13";

        public const string Day14 = "14";

        public const string Day15 = "15";

        public const string Day16 = "16";

        public const string Day17 = "17";

        public const string Day18 = "18";

        public const string Day19 = "19";

        public const string Day20 = "20";

        public const string Day21 = "21";

        public const string Day22 = "22";

        public const string Day23 = "23";

        public const string Day24 = "24";

        public const string Day25 = "25";

        public const string Day26 = "26";

        public const string Day27 = "27";

        public const string Day31 = "31";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                //{ None, "" },
                { Day1, "1" },
                { Day2, "2" },
                { Day3, "3" },
                { Day4, "4" },
                { Day5, "5" },
                { Day6, "6" },
                { Day7, "7" },
                { Day8, "8" },
                { Day9, "9" },
                { Day10, "10" },
                { Day11, "11" },
                { Day12, "12" },
                { Day13, "13" },
                { Day14, "14" },
                { Day15, "15" },
                { Day16, "16" },
                { Day17, "17" },
                { Day18, "18" },
                { Day19, "19" },
                { Day20, "20" },
                { Day21, "21" },
                { Day22, "22" },
                { Day23, "23" },
                { Day24, "24" },
                { Day25, "25" },
                { Day26, "26" },
                { Day27, "27" },
                { Day31, "末" }
            }.AsReadOnly();
    }

    public class PaymentMonths
    {
        public const string Jan = "01";

        public const string Feb = "02";

        public const string Mar = "03";

        public const string Apr = "04";

        public const string May = "05";

        public const string Jun = "06";

        public const string Jul = "07";

        public const string Aug = "08";

        public const string Sep = "09";

        public const string Oct = "10";

        public const string Nov = "11";

        public const string Dec = "12";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Jan, "01" },
                { Feb, "02" },
                { Mar, "03" },
                { Apr, "04" },
                { May, "05" },
                { Jun, "06" },
                { Jul, "07" },
                { Aug, "08" },
                { Sep, "09" },
                { Oct, "10" },
                { Nov, "11" },
                { Dec, "12" }
            }.AsReadOnly();
    }

    public class Month
    {
        public const string None = "0";

        public const string Jan = "1";

        public const string Feb = "2";

        public const string Mar = "3";

        public const string Apr = "4";

        public const string May = "5";

        public const string Jun = "6";

        public const string Jul = "7";

        public const string Aug = "8";

        public const string Sep = "9";

        public const string Oct = "10";

        public const string Nov = "11";

        public const string Dec = "12";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { None, "" },
                { Jan, "1" },
                { Feb, "2" },
                { Mar, "3" },
                { Apr, "4" },
                { May, "5" },
                { Jun, "6" },
                { Jul, "7" },
                { Aug, "8" },
                { Sep, "9" },
                { Oct, "10" },
                { Nov, "11" },
                { Dec, "12" }
            }.AsReadOnly();
    }

    public class ClosingDay
    {
        public const string None = "0";

        public const string Day1 = "1";

        public const string Day2 = "2";

        public const string Day3 = "3";

        public const string Day4 = "4";

        public const string Day5 = "5";

        public const string Day6 = "6";

        public const string Day7 = "7";

        public const string Day8 = "8";

        public const string Day9 = "9";

        public const string Day10 = "10";

        public const string Day11 = "11";

        public const string Day12 = "12";

        public const string Day13 = "13";

        public const string Day14 = "14";

        public const string Day15 = "15";

        public const string Day16 = "16";

        public const string Day17 = "17";

        public const string Day18 = "18";

        public const string Day19 = "19";

        public const string Day20 = "20";

        public const string Day21 = "21";

        public const string Day22 = "22";

        public const string Day23 = "23";

        public const string Day24 = "24";

        public const string Day25 = "25";

        public const string Day26 = "26";

        public const string Day27 = "27";

        public const string Day31 = "31";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {                
                { None, "" },
                { Day1, "1" },
                { Day2, "2" },
                { Day3, "3" },
                { Day4, "4" },
                { Day5, "5" },
                { Day6, "6" },
                { Day7, "7" },
                { Day8, "8" },
                { Day9, "9" },
                { Day10, "10" },
                { Day11, "11" },
                { Day12, "12" },
                { Day13, "13" },
                { Day14, "14" },
                { Day15, "15" },
                { Day16, "16" },
                { Day17, "17" },
                { Day18, "18" },
                { Day19, "19" },
                { Day20, "20" },
                { Day21, "21" },
                { Day22, "22" },
                { Day23, "23" },
                { Day24, "24" },
                { Day25, "25" },
                { Day26, "26" },
                { Day27, "27" },
                { Day31, "末" }
            }.AsReadOnly();

    }

    public class PaymentDayType
    {
        public const string None = "0";

        public const string Day1 = "1";

        public const string Day2 = "2";

        public const string Day3 = "3";

        public const string Day4 = "4";

        public const string Day5 = "5";

        public const string Day6 = "6";

        public const string Day7 = "7";

        public const string Day8 = "8";

        public const string Day9 = "9";

        public const string Day10 = "10";

        public const string Day11 = "11";

        public const string Day12 = "12";

        public const string Day13 = "13";

        public const string Day14 = "14";

        public const string Day15 = "15";

        public const string Day16 = "16";

        public const string Day17 = "17";

        public const string Day18 = "18";

        public const string Day19 = "19";

        public const string Day20 = "20";

        public const string Day21 = "21";

        public const string Day22 = "22";

        public const string Day23 = "23";

        public const string Day24 = "24";

        public const string Day25 = "25";

        public const string Day26 = "26";

        public const string Day27 = "27";

        public const string Day31 = "31";

        public const int StandardValue = 30;

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { None, "" },
            { Day1, "1日" },
            { Day2, "2日" },
            { Day3, "3日" },
            { Day4, "4日" },
            { Day5, "5日" },
            { Day6, "6日" },
            { Day7, "7日" },
            { Day8, "8日" },
            { Day9, "9日" },
            { Day10, "10日" },
            { Day11, "11日" },
            { Day12, "12日" },
            { Day13, "13日" },
            { Day14, "14日" },
            { Day15, "15日" },
            { Day16, "16日" },
            { Day17, "17日" },
            { Day18, "18日" },
            { Day19, "19日" },
            { Day20, "20日" },
            { Day21, "21日" },
            { Day22, "22日" },
            { Day23, "23日" },
            { Day24, "24日" },
            { Day25, "25日" },
            { Day26, "26日" },
            { Day27, "27日" },
            { Day31, "末日" }
        }.AsReadOnly();
    }

    public class PaymentDayTypeContractRegister
    {
        public const string None = "0";

        public const string Day1 = "1";

        public const string Day2 = "2";

        public const string Day3 = "3";

        public const string Day4 = "4";

        public const string Day5 = "5";

        public const string Day6 = "6";

        public const string Day7 = "7";

        public const string Day8 = "8";

        public const string Day9 = "9";

        public const string Day10 = "10";

        public const string Day11 = "11";

        public const string Day12 = "12";

        public const string Day13 = "13";

        public const string Day14 = "14";

        public const string Day15 = "15";

        public const string Day16 = "16";

        public const string Day17 = "17";

        public const string Day18 = "18";

        public const string Day19 = "19";

        public const string Day20 = "20";

        public const string Day21 = "21";

        public const string Day22 = "22";

        public const string Day23 = "23";

        public const string Day24 = "24";

        public const string Day25 = "25";

        public const string Day26 = "26";

        public const string Day27 = "27";

        public const string Day31 = "31";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
                { None, "" },
                { Day1, "1" },
                { Day2, "2" },
                { Day3, "3" },
                { Day4, "4" },
                { Day5, "5" },
                { Day6, "6" },
                { Day7, "7" },
                { Day8, "8" },
                { Day9, "9" },
                { Day10, "10" },
                { Day11, "11" },
                { Day12, "12" },
                { Day13, "13" },
                { Day14, "14" },
                { Day15, "15" },
                { Day16, "16" },
                { Day17, "17" },
                { Day18, "18" },
                { Day19, "19" },
                { Day20, "20" },
                { Day21, "21" },
                { Day22, "22" },
                { Day23, "23" },
                { Day24, "24" },
                { Day25, "25" },
                { Day26, "26" },
                { Day27, "27" },
                { Day31, "末" }
            }.AsReadOnly();
    }

    public class ContractStatus
    {
        public const string Create = "0";
        public const string Implementation = "1";
        public const string Finish = "2";
        public const string ExitMiddle = "3";
        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { Create, "作成" },
            { Implementation, "履行" },
            { Finish, "終了" },
            { ExitMiddle, "途中終了" }
        }.AsReadOnly();
    }

    public class MailPriorityType
    {
        public const string HIGH = "0";

        public const string NORMAL = "1";

        public const string LOW = "2";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { HIGH, "高い" },
                { NORMAL, "通常" },
                { LOW,"低い"}
            }.AsReadOnly();
    }

    public class PaymentSiteType
    {
        public const string None = "0";

        public const string ThisMonth = "1";

        public const string NextMonth = "2";

        public const string TwoMonthsAfter = "3";

        public const string ThreeMonthsAfter = "4";

        public const string FourMonthsAfter = "5";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { None, "" },
            { ThisMonth, "当月" },
            { NextMonth, "翌月" },
            { TwoMonthsAfter, "翌々月" },
            { ThreeMonthsAfter, "翌々々月" },
            { FourMonthsAfter, "翌々々々月" }
        }.AsReadOnly();

        public static int GetMonths(int payment_site_type)
        {
            if (payment_site_type == 0)
                return payment_site_type;
            else return payment_site_type - 1;
        }
    }

    public class EffectiveType
    {

        public const string Day = "0";

        public const string Month = "1";

        public const string Year = "2";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Day, "日" },
                { Month, "月" },
                { Year, "年" }                
            }.AsReadOnly();

    }

    public class QuotationType
    {

        public const string Estimate = "0";

        public const string Fixed = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Estimate, "概算" },
                { Fixed, "確定" }                
            }.AsReadOnly();
    }

    public class ModeExport
    {
        public const int EstimateFile = 0;

        public const int OrderFile = 1;

        public const int DebitNote = 2;

        public const int BillingList = 3;

        public const int PaymentOrderFile = 4;

        public const int CoverLetterByDocument = 5;

        public const int CoverLetterByFax = 6;

        public const int CoverLetterByEnvelopeAddress = 7;
    }

    public class EstimateIssueFlag
    {

        public const string None = "0";

        public const string Printing = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { None, "未発行" },
                { Printing, "発行済" }                
            }.AsReadOnly();

    }

    public class EstimateSendFlag
    {

        public const string None = "0";

        public const string Sending = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { None, "未送付" },
                { Sending, "送付済" }                
            }.AsReadOnly();

    }

    public class ProjectStatus
    {

        public const string PROPOSED = "1";

        public const string ISSUED_ESTIMATE = "2";

        public const string CONTRACTED = "3";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { PROPOSED, "提案中" },
                { ISSUED_ESTIMATE, "見積書発行済み" },
                { CONTRACTED, "契約済み" }
            }.AsReadOnly();

    }

    public class ContentType
    {
        public const string General = "0";

        public const string Maintenance = "1";

        public const string Release = "2";

        public const string Event = "3";

        public const string Other = "4";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { General, "一般情報" },
            { Maintenance, "メンテナンス情報" } ,
            { Release, "リリース情報" },
            { Event, "イベント情報" },
            { Other, "その他" }
        }.AsReadOnly();
    }

    public class BankAccountType
    {
        public const string Savings = "1";
        public const string Cheque = "2";
        public const string Deposit = "3";
        public const string Other = "4";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { Savings, "普通預金" },
            { Cheque, "当座預金" },
            { Deposit, "貯蓄預金" },
            { Other, "その他" }
        }.AsReadOnly();

        public static string GetBankAccountTypeName(string accTypeId)
        {
            string result = string.Empty;
            switch (accTypeId)
            {
                case Savings:
                    result = Items[Savings].ToString();
                    break;
                case Cheque:
                    result = Items[Cheque].ToString();
                    break;
                case Deposit:
                    result = Items[Deposit].ToString();
                    break;
                case Other:
                    result = Items[Other].ToString();
                    break;
                default:
                    result = Items[Other].ToString();
                    break;
            }
            return result;
        }

        public static string GetBankAccountTypeID(string bankAccType)
        {
            string result = string.Empty;
            switch (bankAccType)
            {
                case "普通預金":
                    result = Savings;
                    break;
                case "当座預金":
                    result = Cheque;
                    break;
                case "貯蓄預金":
                    result = Deposit;
                    break;
                case "その他":
                    result = Other;
                    break;
                default:
                    result = Other;
                    break;
            }
            return result;
        }
    }

    public class ContractListStatus
    {

        public const string CREATING_CONTRACTED = "1";

        public const string CREATING = "2";

        public const string CONTRACTING = "3";

        public const string CONTRACTED = "4";

        public const string ALL = "5";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { CREATING_CONTRACTED, "作成中・契約済" },
                { CREATING, "作成中" },
                { CONTRACTING, "契約履行中" },
                { CONTRACTED, "終了済み" },
                { ALL, "全て" }
            }.AsReadOnly();

    }

    public class BillIssueFlg
    {
        public const string Unissued = "0";
        public const string Published = "1";
        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { Unissued, "未入力" },
            { Published, "入力済み" }
        }.AsReadOnly();
    }

    public class ActualInputFlg
    {
        public const string NotInput = "0";
        public const string Entered = "1";
        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { NotInput, "未入力" },
            { Entered, "入力済み" }
        }.AsReadOnly();
    }

    public class DepositType
    {
        public const string Payee = "0";
        public const string DirectDebit = "1";
        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Payee, "請求" },
                { DirectDebit , "引き落とし" }
            }.AsReadOnly();
    }

    public class BillingInfoSetupFlg
    {
        public const string NoSetting = "0";
        public const string Setting = "1";
    }

    public class BaseTimeType
    {
        public const string Normal = "0";

        public const string Special = "1";

        public const string NormalValue = "通常";

        public const string SpecialValue = "特殊精算";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { Normal, "通常" },
            { Special, "特殊精算" }
        }.AsReadOnly();
    }

    public class TaxableFlag
    {
        public const string TaxFree = "0";

        public const string Taxable = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { Taxable, "課税" },
            { TaxFree, "非課税" }
        }.AsReadOnly();
    }

    public class RoundingType
    {
        public const string RoundDown = "0";

        public const string RoundUp = "1";

        public const string RoundOff = "2";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { RoundDown, "切り捨て" },
            { RoundUp, "切り上げ" },
            { RoundOff, "四捨五入" }
        }.AsReadOnly();
    }

    public class DepositStateType
    {
        public const string NotPayment = "0";

        public const string NotRecievedPart = "1";

        public const string Received = "2";

        public const string AllType = "3";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { NotPayment, "未入金" },
                { NotRecievedPart, "未入金／一部未入金" },
                { Received, "入金済" },
                { AllType, "すべて" }
            }.AsReadOnly();

    }

    public class ContractLevelType
    {

        public const string Level2 = "0";

        public const string Level3 = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Level2, "２階層" },
                { Level3, "３階層" }                
            }.AsReadOnly();

    }

    public class UsePrefix
    {

        public const string None = "0";

        public const string Useprefix = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { None, "接頭辞無し" },
                { Useprefix, "接頭辞有り" }                
            }.AsReadOnly();

    }

    public class DisableFlag
    {

        public const string Enable = "0";

        public const string Disable = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Enable, "有効" },
                { Disable, "無効" }                
            }.AsReadOnly();

    }

    public class LoginLockFlag
    {
        public const string unLock = "0";

        public const string Lock = "1";

        public static readonly OrderedDictionary Item = new OrderedDictionary
        {
            {unLock,""},
            {Lock,"制限"}

        }.AsReadOnly();
    }

    public class AutoExpandFlag
    {
        public const string None = "0";

        public const string AutoExpand = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { None, "展開無し" },
            { AutoExpand, "展開有り" }
        }.AsReadOnly();

    }

    public class ChargeType
    {
        public const int LumpSum = 0;

        public const int MonthlySum = 1;

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { LumpSum, "一括" },
            { MonthlySum, "月額" }
        }.AsReadOnly();
    }

    public class PaymentMethodType
    {
        public const string Duration = "1";

        public const string Fixed = "2";

        public const string Hour = "3";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { Duration, "時間幅精算" },
            { Fixed, "固定精算" } ,
            { Hour, "時間精算" } 
        }.AsReadOnly();
    }

    public class PaymentStateType
    {
        public const string State0 = "0";

        public const string State1 = "1";

        public const string State2 = "2";

        public const string State3 = "3";

        public const string State4 = "4";

        public const string State5 = "5";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { State0, "　" },
            { State1, "△" },
            { State2, "▲" },
            { State3, "○" },
            { State4, "●" },
            { State5, "※" }
        }.AsReadOnly();
    }

    public class OuterTimeCalcUnitType
    {
        public const string Normal = "1";

        public const string Upper = "2";

        public const string Middle = "3";

        public const string Lower = "4";

        public const string Other = "5";

        public const string NormalValue = "上下割";

        public const string UpperValue = "上割";

        public const string MiddleValue = "中割";

        public const string LowerValue = "下割";

        public const string OtherValue = "その他";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Normal, "上下割" },
                { Upper, "上割" } ,
                { Middle, "中割" },
                { Lower, "下割" },
                { Other, "その他" }
            }.AsReadOnly();
    }

    public class ReportTemplate
    {

        public const string NOMEN = "**NOMEN_";

        public const string NOMEN_COUNT = "**NOMEN_COUNT_";

        public const string BASE_TIME_TYPE = "**BASE_TIME_TYPE_";

        public const string UNIT_PRICE = "**UNIT_PRICE_";

        public const string QTY = "**QTY_";

        public const string UNIT = "**UNIT_";

        public const string PAYMENT_METHOD = "**PAYMENT_METHOD_";

        public const string BASE_TIME_LOWER = "**BASE_TIME_LOWER_";

        public const string BASE_TIME_LOWER_TANI = "**BASE_TIME_LOWER_TANI_";

        public const string BASE_TIME_UPPER = "**BASE_TIME_UPPER_";

        public const string BASE_TIME_UPPER_TANI = "**BASE_TIME_UPPER_TANI_";

        public const string BASE_TIME = "**BASE_TIME_";

        public const string OUTER_TIME_CALC_UNIT = "**OUTER_TIME_CALC_UNIT_";

        public const string EXCEED_UNIT_PRICE = "**EXCEED_UNIT_PRICE_";

        public const string DEDUCTION_UNIT_PRICE = "**DEDUCTION_UNIT_PRICE_";

        public const string UNIT_TIME = "**UNIT_TIME_";

        public const string UNIT_TIME_TANI = "**UNIT_TIME_TANI_";

        public const string AMOUNT_INCLUDETAX = "**AMOUNT_INCLUDETAX";

        public const string AMOUNT = "**AMOUNT_";

        public const string EST_NO_FULL = "**EST_NO_FULL";

        public const string EST_NO = "**EST_NO";

        public const string EST_NO_SEQ = "**EST_NO_SEQ";

        public const string ORDER_NO_FULL = "**ORDER_NO_FULL";

        public const string ORDER_NO = "**ORDER_NO";

        public const string ORDER_NO_SEQ = "**ORDER_NO_SEQ";

        public const string ISSUE_DATE = "**ISSUE_DATE";

        public const string ISSUE_DATE_KANJI = "**ISSUE_DATE_KANJI";

        public const string EFFECTIVE_DATE = "**EFFECTIVE_DATE";

        public const string EFFECTIVE_DATE_KANJI = "**EFFECTIVE_DATE_KANJI";

        public const string EFFECTIVE_INTERVAL = "**EFFECTIVE_INTERVAL";

        public const string PRJ_NAME = "**PRJ_NAME";

        public const string PRJ_DESC_NAME = "**PRJ_DESC_NAME";

        public const string PRJ_DSP_NAME = "**PRJ_DSP_NAME";

        public const string PRJ_PERIOD = "**PRJ_PERIOD";

        public const string PRJ_PERIOD_KANJI = "**PRJ_PERIOD_KANJI";

        public const string PRJ_PERIOD_START = "**PRJ_PERIOD_START";

        public const string PRJ_PERIOD_START_KANJI = "**PRJ_PERIOD_START_KANJI";

        public const string PRJ_PERIOD_END = "**PRJ_PERIOD_END";

        public const string PRJ_PERIOD_END_KANJI = "**PRJ_PERIOD_END_KANJI";

        public const string ACCEPTANCE_DATE = "**ACCEPTANCE_DATE";

        public const string ACCEPTANCE_DATE_KANJI = "**ACCEPTANCE_DATE_KANJI";

        public const string DELIVERY_LOCATION = "**DELIVERY_LOCATION";

        public const string CLOSING_DAY = "**CLOSING_DAY";

        public const string PAYMENT_DAY = "**PAYMENT_DAY";

        public const string CLOSING_PAYMENT_DAY = "**CLOSING_PAYMENT_DAY";

        public const string DELIVERABLES = "**DELIVERABLES";

        public const string DELIVERY_DATE = "**DELIVERY_DATE";

        public const string DELIVERY_DATE_KANJI = "**DELIVERY_DATE_KANJI";

        public const string MEMO = "**MEMO";

        public const string EST_CONDITION = "**EST_CONDITION";

        public const string BP_NAME = "**BP_NAME";

        public const string BP_NAME_HONORIFIC = "**BP_NAME_HONORIFIC";

        public const string BP_STAFF_NAME = "**BP_STAFF_NAME";

        public const string BP_STAFF_NAME_HONORIFIC = "**BP_STAFF_NAME_HONORIFIC";

        public const string ADDRESS_1 = "**ADDRESS_1";

        public const string ADDRESS_2 = "**ADDRESS_2";

        public const string ADDRESS = "**ADDRESS";

        public const string ZIP_CODE = "**ZIP_CODE";

        public const string GROUP_NAME = "**GROUP_NAME";

        public const string STAFF_POSITION_NAME = "**STAFF_POSITION_NAME";

        public const string BP_ADDR_1 = "**BP_ADDR_1";

        public const string EST_AMOUNT_TOTAL = "**EST_AMOUNT_TOTAL";

        public const string EST_AMOUNT_TOTAL_ON_TAX = "**EST_AMOUNT_TOTAL_ON_TAX";

        public const string EST_TAXABLE_AMOUNT_TOTAL = "**EST_TAXABLE_AMOUNT_TOTAL";

        public const string ORDER_AMOUNT_TOTAL = "**ORDER_AMOUNT_TOTAL";

        public const string ORDER_AMOUNT_TOTAL_ON_TAX = "**ORDER_AMOUNT_TOTAL_ON_TAX";

        public const string ORDER_TAXABLE_AMOUNT_TOTAL = "**ORDER_TAXABLE_AMOUNT_TOTAL";

        public const string ORDER_CONDITION = "**ORDER_CONDITION";

        public const string TAXABLE = "**TAXABLE_";

        public const string TAX_RATE = "**TAX_RATE";

        public const string TAX = "**TAX";

        public const string TAX_TYPE = "**TAX_TYPE";

        public const string QUOTATION_TYPE = "**QUOTATION_TYPE";

        public const string ORD_FORMAT_PATH = "**ORD_FORMAT_PATH";

        public const string ORD_FORMAT_DETAIL_LINE = "**ORD_FORMAT_DETAIL_LINE";

        public const string ORD_DETAIL_LINE = "**ORD_DETAIL_LINE";

        public const string EST_FORMAT_PATH = "**EST_FORMAT_PATH";

        public const string EST_FORMAT_DETAIL_LINE = "**EST_FORMAT_DETAIL_LINE";

        public const string EST_DETAIL_LINE = "**EST_DETAIL_LINE";
    }

    public class EditDebitNoteTemplate
    {
        public const string BILLING_NO_DSP = "**BILLING_NO_DSP";
        public const string BILLING_YEAR = "**BILLING_YEAR";
        public const string BILLING_MONTH = "**BILLING_MONTH";
        public const string ISSUE_DATE = "**ISSUE_DATE";
        public const string ISSUE_DATE_KANJI = "**ISSUE_DATE_KANJI";
        public const string ZIP_CODE = "**ZIP_CODE";
        public const string ADDRESS_1 = "**ADDRESS_1";
        public const string ADDRESS_2 = "**ADDRESS_2";
        public const string COMPANY_NAME = "**COMPANY_NAME";
        public const string GROUP_NAME = "**GROUP_NAME";
        public const string STAFF_POSITION_NAME = "**STAFF_POSITION_NAME";
        public const string STAFF_NAME = "**STAFF_NAME";
        public const string AMOUNT_TOTAL = "**AMOUNT_TOTAL";
        public const string BILLING_AMOUNT = "**BILLING_AMOUNT";
        public const string BILLING_TAX = "**BILLING_TAX";
        public const string BILL_TYPE = "**BILL_TYPE";
        public const string BILLING_TITLE = "**BILLING_TITLE";
        public const string DEPOSIT_DATE = "**DEPOSIT_DATE";
        public const string DEPOSIT_DATE_KANJI = "**DEPOSIT_DATE_KANJI";
        public const string DEPOSIT_DATE_TITLE = "**DEPOSIT_DATE_TITLE";
        public const string BANK_NAME = "**BANK_NAME";
        public const string BANK_BRANCH_NAME = "**BANK_BRANCH_NAME";
        public const string BANK_ACCOUNT_TYPE = "**BANK_ACCOUNT_TYPE";
        public const string BANK_ACCOUNT_NO = "**BANK_ACCOUNT_NO";
        public const string BANK_ACCOUNT_HOLDER = "**BANK_ACCOUNT_HOLDER";
        public const string DEPOSIT_NOTE = "**DEPOSIT_NOTE";
        public const string NOTE = "**NOTE";
        public const string DETAIL_ITEM_NO = "**DETAIL_ITEM_";
        public const string DETAIL_ITEM_DESC_NO = "**DETAIL_ITEM_DESC_";

        public const string DETAIL_BASE_BILLING_AMOUNT_NO = "**DETAIL_BASE_BILLING_AMOUNT_";
        public const string DETAIL_WORKING_TIME_NO = "**DETAIL_WORKING_TIME_";
        public const string DETAIL_WORKING_TIME_UNIT_NO = "**DETAIL_WORKING_TIME_UNIT_";
        public const string DETAIL_BILLING_WORKING_TIME_NO = "**DETAIL_BILLING_WORKING_TIME_";
        public const string DETAIL_BILLING_WORKING_TIME_UNIT_NO = "**DETAIL_BILLING_WORKING_TIME_UNIT_";
        public const string DETAIL_EXPENSES_ADJUSTMENT_TIME_NO = "**DETAIL_EXPENSES_ADJUSTMENT_TIME_";
        public const string DETAIL_EXPENSES_ADJUSTMENT_TIME_UNIT_NO = "**DETAIL_EXPENSES_ADJUSTMENT_TIME_UNIT_";
        public const string DETAIL_EXPENSES_ADJUSTMENT_AMOUNT_NO = "**DETAIL_EXPENSES_ADJUSTMENT_AMOUNT_";
        public const string DETAIL_TAXFREE_OVERHEAD_COSTS_NO = "**DETAIL_TAXFREE_OVERHEAD_COSTS_";
        public const string DETAIL_TAXABLE_OVERHEAD_COSTS_NO = "**DETAIL_TAXABLE_OVERHEAD_COSTS_";
        public const string DETAIL_ADJUSTMENT_AMOUNT_NO = "**DETAIL_ADJUSTMENT_AMOUNT_";
        public const string DETAIL_TAXABLE_AMOUNT_NO = "**DETAIL_TAXABLE_AMOUNT_";
        public const string DETAIL_EXCEED_UNIT_PRICE_NO = "**DETAIL_EXCEED_UNIT_PRICE_";
        public const string DETAIL_DEDUCTION_UNIT_PRICE_NO = "**DETAIL_DEDUCTION_UNIT_PRICE_";

        public const string DETAIL_AMOUNT_NO = "**DETAIL_AMOUNT_";
        public const string DETAIL_TAX_NO = "**DETAIL_TAX_";
        public const string DETAIL_AMOUNT_ON_TAX_NO = "**DETAIL_AMOUNT_ON_TAX_";
    }

    public class ContractType
    {
        public const int NO_SELECT = 0;

        public const int SES = 1;

        public const int STAFF = 2;

        public const int CONTRACT = 3;

        public const int PACKAGE = 6;

        public const int DELEGATION = 8;

        public const int MAINTAIN = 9;
    }

    public class EstimationCreationFlag
    {
        public const string NoNeed = "0";

        public const string NeedEstimation = "1";

    }

    public class InvoiceCreationFlag
    {
        public const string NoNeed = "0";

        public const string Needed = "1";

    }

    public class CautionNeededFlag
    {
        public const string NoNeed = "0";

        public const string Needed = "1";

    }

    public class BillingCreateStatus
    {

        public const string All = "1";

        public const string NotCreated = "2";

        public const string Created = "3";

        public const string AllText = "全て";

        public const string NotCreatedText = "未作成";

        public const string CreatedText = "作成済み";

    }

    public class BillingAutoUpdate
    {

        public const string All = "2";

        public const string NotAuto = "0";

        public const string OnlyAuto = "1";

        public const string AllText = "全て";

        public const string NotAutoText = "NOT自動更新";

        public const string OnlyAutoText = "自動更新のみ";
    }

    public class BillingIssueFlag
    {

        public const string None = "0";

        public const string Printing = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { None, "未発行" },
                { Printing, "発行済" }                
            }.AsReadOnly();

    }

    public class BillingSendFlag
    {

        public const string None = "0";

        public const string Sending = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { None, "未送付" },
                { Sending, "送付済" }                
            }.AsReadOnly();

    }

    public class BillingIssueDayType
    {
        public const string ContractExpectedDate = "0";
        public const string BillingCloseDate = "1";
        public const string TodayDate = "2";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { ContractExpectedDate, "契約どおりの日付を表示" },
            { BillingCloseDate, "請求締日を表示" },
            { TodayDate, "本日の日付を表示" }
        }.AsReadOnly();
    }

    public class EvergreenContractFlag
    {
        public const string Off = "0";
        public const string AutoUpdate = "1";
        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Off, "OFF" },
                { AutoUpdate, "自動更新" }
            }.AsReadOnly();
    }

    public class CollectContractFlag
    {
        public const string Off = "0";
        public const string Collect = "1";
        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { Off, "OFF" },
                { Collect, "纏める" }
            }.AsReadOnly();
    }

    public class AssetsLiabilitiesType
    {
        public const string None = "";
        public const string debtor = "0";
        public const string creditor = "1";
        public static readonly OrderedDictionary Items = new OrderedDictionary
            {
                { None, "" },
                { debtor, "借方" },
                { creditor, "貸方"}
            }.AsReadOnly();
    }

    /// <summary>
    /// 更新プログラムＩＤ(UPD_PROG_ID)用ID　LIST
    /// </summary>
    public class UpdateProgramId
    {
        // 
        /// <summary>
        /// 請求一覧 BILLING LIST
        /// </summary>
        public const string BILLING_LIST = "BILLING_LIST";
    }

    public class BillingInputOperateTimeOrderCondition
    {
        public const string ByBilling = "0";
        public const string ByDesName = "1";

        public static readonly OrderedDictionary Items = new OrderedDictionary
        {
            { ByBilling, "請求先カナの昇順" },
            { ByDesName, "作業者名／品名の昇順" }
        }.AsReadOnly();
    }

    public class TimeIndex
    {
        public const int ContractPeriodStart = 1;
        public const int ContractPeriodEnd = 2;
        public const int DeliveryDate = 3;
        public const int AcceptanceDate = 4;
        public const int ProjectJoiningDate = 5;
    }

    public class CaseFinish
    {
        public const int FinishNormal = 1;
        public const int FinishInMiddle = 2;
        public const int FinishInMiddleWithSpecialPayment = 3;
    }

    public class ClosingSiteType
    {
        public const int None = 0;
        public const int January = 1;
        public const int February = 2;
        public const int March = 3;
        public const int April = 4;
        public const int May = 5;
        public const int June = 6;
        public const int July = 7;
        public const int August = 8;
        public const int September = 9;
        public const int October = 10;
        public const int November = 11;
        public const int December = 12;
    }

    // Number of collumn in billing detail
    public class BillingDetailNoCol
    {
        public const int FirstCol = 1;
        public const int SixthCol = 6;
    }

    public class ConditionChangeFlag
    {
        public const string No = "0";
        public const string Yes = "1";
    }

    public class BillConfirmFlag
    {
        public const string UN_ISSUED = "0";
        public const string ISSUED = "1";
    }

    public class PaymentType
    {
        public const string Billing = "0";
        public const string PullingDown = "1";
    }

    public class ExtendContractFlag
    {
        public const string NoExtend = "0";
        public const string Extended = "1";
    }

    public class ContractCreateProcess
    {
        public const string CreateNewOrCopy = "0";
        public const string Extend = "1";
        public const string FromEstimate = "2";
        public const string None = "99";
    }

    public class CoverLetterCreateType
    {
        public const string Document = "0";

        public const string Fax = "1";

        public const string EnvelopeAddress = "2";
    }

    public class CoverLetterTemplate
    {
        public const string ISSUE_DATE = "**ISSUE_DATE";
        public const string BP_NAME = "**BP_NAME";
        public const string BP_STAFF_NAME = "**BP_STAFF_NAME";
        public const string BP_STAFF_GROUP_NAME = "**BP_STAFF_GROUP_NAME";
        public const string BP_STAFF_POSITION_NAME = "**BP_STAFF_POSITION_NAME";
        public const string BP_STAFF_TEL = "**BP_STAFF_TEL";
        public const string BP_STAFF_FAX = "**BP_STAFF_FAX";
        public const string BP_STAFF_ZIP_CD = "**BP_STAFF_ZIP_CD";
        public const string BP_ADDR = "**BP_ADDR";
        public const string BP_ADDR_1 = "**BP_ADDR_1";
        public const string BP_ADDR_2 = "**BP_ADDR_2";
        public const string BP_TEL = "**BP_TEL";
        public const string BP_FAX = "**BP_FAX";
    }

    public class NoticeFlg
    {
        public const string OFF = "0";
        public const string ON = "1";

    }

    public class FormatType
    {
        public const string DispatchType = "1";
        public const int FORMAT_DETAIL_LINE = 0;
        public const int MAX_FORMAT_NAME = 15;
        public const int MAX_FORMAT_PATH = 255;
        public const int UPLOAD_FILE_PASSWORD = 128;
        public const int UPLOAD_FILE_SIZE_LIMIT = 9999;
    }

    public class FormatSubType
    {
        public const string Billing = "1";

        public const string Payment = "2";

    }
}