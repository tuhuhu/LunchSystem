using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemSetup.Models
{
    public class ContractFirmEntity : BaseEntity
    {
        //契約企業コード
        public string COMPANY_CD { get; set; }
        //契約企業名
        public string COMPANY_NAME { get; set; }
        //契約郵便番号
        public string COMPANY_ZIP_CD { get; set; }
        //契約住所１
        public string COMPANY_ADDR_1 { get; set; }
        //契約住所２
        public string COMPANY_ADDR_2 { get; set; }
        //契約担当部署
        public string COMPANY_GROUP_NAME { get; set; }
        //契約担当者名
        public string COMPANY_STAFF_NAME { get; set; }
        //契約担当者カナ
        public string COMPANY_STAFF_NAME_KANA { get; set; }
        //契約担当者電話番号
        public string COMPANY_STAFF_TEL { get; set; }
        //契約担当者電話番号内線
        public string COMPANY_STAFF_TEL_EXT { get; set; }
        //契約担当者メールアドレス
        public string COMPANY_STAFF_EMAIL_ADDR { get; set; }
        //端数処理区分
        public string ROUNDING_TYPE { get; set; }
        //契約階層区分
        public string CONTRACT_LEVEL_TYPE { get; set; }
        //見積番号接頭辞有無
        public string EST_NO_USE_PREFIX { get; set; }
        //見積番号年月有無
        public string EST_NO_USE_YM { get; set; }

        public string EST_COLLECTION_NO_USE_PREFIX { get; set; }//集約見積書番号接頭辞有無
        public string EST_COLLECTION_NO_PREFIX { get; set; }//集約見積書番号接頭辞
        public string EST_COLLECTION_NO_USE_YM { get; set; }//集約見積書番号年月有無

        public string EST_COLLECTION_FORMAT_PATH { get; set; }//集約見積書フォーマットパス
        public int EST_COLLECTION_FORMAT_DETAIL_LINE { get; set; }//集約見積書明細行数
        public int EST_COLLECTION_FORMAT_NOMEN_LINE { get; set; }//集約見積書品目行数

        public string EST_COLLECTION_FORMAT_SES_PATH { get; set; }//集約見積書SESフォーマットパス
        public int EST_COLLECTION_FORMAT_SES_DETAIL_LINE { get; set; }//集約見積書SES明細行数
        public int EST_COLLECTION_FORMAT_SES_NOMEN_LINE { get; set; }//集約見積書SES品目行数

        public string ORD_COLLECTION_FORMAT_PATH { get; set; }//集約注文書フォーマットパス
        public int ORD_COLLECTION_FORMAT_DETAIL_LINE { get; set; }//集約注文書明細行数
        public int ORD_COLLECTION_FORMAT_NOMEN_LINE { get; set; }//集約注文書品目行数

        public string ORD_COLLECTION_FORMAT_SES_PATH { get; set; }//集約注文書SESフォーマットパス
        public int ORD_COLLECTION_FORMAT_SES_DETAIL_LINE { get; set; }//集約注文書SES明細行数
        public int ORD_COLLECTION_FORMAT_SES_NOMEN_LINE { get; set; }//集約注文書SES品目行数

        //請求書番号接頭辞有無
        public string BILL_NO_USE_PREFIX { get; set; }
        //請求書番号接頭辞
        public string BILL_NO_PREFIX { get; set; }
        //請求書番号年月有無
        public string BILL_NO_USE_YM { get; set; }
        //請求書フォーマットパス
        public string BILL_FORMAT_PATH { get; set; }
        //請求書明細行数
        public int BILL_FORMAT_DETAIL_LINE { get; set; }
        public string BILL_FORMAT_SES_PATH { get; set; }
        public int BILL_FORMAT_SES_DETAIL_LINE { get; set; }

        /// <summary>
        /// 請求書派遣フォーマットパス
        /// </summary>
        public string BILL_FORMAT_TEMP_PATH { get; set; }

        /// <summary>
        /// 請求書派遣明細行数
        /// </summary>
        public int BILL_FORMAT_TEMP_DETAIL_LINE { get; set; }

        public string BILL_PAYEE_TITLE { get; set; }
        //請求書振込先注釈
        public string BILL_PAYEE_NOTE { get; set; }
        public string BILL_DIRECT_TITLE { get; set; }
        //請求書引落先注釈
        public string BILL_DIRECT_NOTE { get; set; }
        //支払先注文書フォーマットパス
        public string PLC_ORDER_FORMAT_PATH { get; set; }
        //支払先注文書書明細行数
        public int PLC_ORDER_FORMAT_DETAIL_LINE { get; set; }
        //支払通知書フォーマットパス
        public string PLC_PAYMENT_NOTICE_FORMAT_PATH { get; set; }
        //書類送付状フォーマットパス
        public string DOC_COVER_LETTER_FORMAT_PATH { get; set; }
        //FAX送付状フォーマットパス
        public string FAX_COVER_LETTER_FORMAT_PATH { get; set; }
        //封筒住所フォーマットパス
        public string ENVELOPE_DESTINATION_FORMAT_PATH { get; set; }
        //契約期間（開始）
        public DateTime AVAILABLE_PERIOD_FROM { get; set; }
        //契約期間（終了）
        public DateTime? AVAILABLE_PERIOD_TO { get; set; }
        //契約状態
        public string COMPANY_CONTRACT_STATUS { get; set; }
        //契約企業ロゴ画像パス
        public string COMPANY_LOGO_IMAGE_PATH { get; set; }

        public string UPLOAD_FILE_PASSWORD { get; set; }

        public int? UPLOAD_FILE_SIZE_LIMIT { get; set; }
        //決算月
        public string ACCOUNT_CLOSING_MONTH { get; set; }
        //納品番号接頭辞有無
        public string DELIVERY_NO_USE_PREFIX { get; set; }
        //納品番号年月有無
        public string DELIVERY_NO_USE_YM { get; set; }
        //支払先注文番号接頭辞有無
        public string PLC_ORDER_NO_USE_PREFIX { get; set; }
        //支払先注文番号年月有無
        public string PLC_ORDER_NO_USE_YM { get; set; }
        //契約プラン
        public long PLAN_SEQ_NO { get; set; }

        public long PLAN_REL_SEQ_NO { get; set; }
    }

    public class ContractFirmEntityPlus : ContractFirmEntity
    {
        public string INS_USERNAME { get; set; }
        public string UPD_USERNAME { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [DataType(DataType.Date)]
        new public DateTime? INS_DATE { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [DataType(DataType.Date)]
        new public DateTime? UPD_DATE { get; set; }
    }
}
