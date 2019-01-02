using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemSetup.Constants.Resources;

namespace SystemSetup.Models
{
    public class BaseEntity
    {
        // 削除フラグ
        public string DEL_FLG { get; set; }
        // 作成日
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [DataType(DataType.DateTime, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "IncorrectFormat")]
        public DateTime INS_DATE { get; set; }
        // 作成者ID
        public long INS_USER_ID { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd HH:mm:ss}")]
        [DataType(DataType.DateTime, ErrorMessageResourceType = typeof(Messages), ErrorMessageResourceName = "IncorrectFormat")]
        // 更新日
        public DateTime UPD_DATE { get; set; }
        // 更新者ID
        public long UPD_USER_ID { get; set; }
        // 更新プログラムID
        public string UPD_PROG_ID { get; set; }
    }

    public class CountResult
    {
        public int CNT { get; set; }
    }
    public class DepositStateRelation
    {
        public const int NoRelationship = 0;
        public const int RelationNotPaid = 1;
        public const int RelationPaid = 2;
    }
}
