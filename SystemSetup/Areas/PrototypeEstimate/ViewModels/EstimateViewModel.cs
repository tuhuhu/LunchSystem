using iEnterAsia.iseiQ.Areas.PrototypeEstimate.Models;
using iEnterAsia.iseiQ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iEnterAsia.iseiQ.Areas.PrototypeEstimate.ViewModels
{
    public partial class EstimateViewModel : IViewModel
    {
        public Estimate Estimate { get; set; }
        public IList<SelectListItem> closing_site_type_list { get; set; }
        public IList<EstimateDetail> ESTIMATE_DETAIL_LIST { get; set; }

        #region constructor

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EstimateViewModel()
        {
            var model = new Estimate();
            model.acceptance_date = DateTime.Now;
            this.Estimate = model;

            this.closing_site_type_list = new List<SelectListItem>();
            this.closing_site_type_list.Add(new SelectListItem
            {
                Text = "選択肢1",
                Value = "01"
            });

            this.ESTIMATE_DETAIL_LIST = new List<EstimateDetail>();
            this.ESTIMATE_DETAIL_LIST.Add(new EstimateDetail());
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="Estimate"></param>
        public EstimateViewModel(Estimate Estimate)
        {
            this.Estimate = Estimate;
        }

        #endregion

        #region function

        /// <summary>
        /// 新規登録します。
        /// </summary>
        /// <param name="db">DbContext</param>
        /// <param name="info">ログイン情報</param>
        public void Add()
        {
            //this.OnAdding(db);
            //this.Estimate.RegistDate = MyUtility.Now;
            //this.Estimate.RegistEmpNo = info.RegisterEmpNo;
            //this.UpdateHistoryInfo(info);
            //this.Estimate.Regist(db);
            //this.OnAdded(db);
        }

        partial void OnAdding();
        partial void OnAdded();

        /// <summary>
        /// 編集します。
        /// </summary>
        /// <param name="db">DbContext</param>
        /// <param name="info">ログイン情報</param>
        public void Edit()
        {
            //this.OnEditing(db);
            //this.UpdateHistoryInfo(info);
            //this.Estimate.Regist(db);
            //this.OnEdited(db);
        }

        partial void OnEditing();
        partial void OnEdited();

        /// <summary>
        /// 削除します。
        /// </summary>
        /// <param name="db">DbContext</param>
        /// <param name="info">ログイン情報</param>
        public void Delete()
        {
            //this.OnDeleting(db);
            //this.UpdateHistoryInfo(info);
            //this.Estimate.Delete(db);
            //this.OnDeleted(db);
        }

        partial void OnDeleting();
        partial void OnDeleted();

        /// <summary>
        /// 履歴情報を更新します。
        /// </summary>
        /// <param name="info">ログイン情報</param>
        public void UpdateHistoryInfo()
        {
            //this.Estimate.UpdateDate = MyUtility.Now;
            //this.Estimate.UpdateEmpNo = info.RegisterEmpNo;
            
            //if (this.Estimate.DeleteFlg == CommonModel.FlgOn)
            //{
            //    this.Estimate.DeleteDate = MyUtility.Now;
            //    this.Estimate.DeleteEmpNo = info.RegisterEmpNo;
            //}
            //else
            //{
            //    this.Estimate.DeleteDate = null;
            //    this.Estimate.DeleteEmpNo = null;
            //}
        }

        #endregion

    }
}

