using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using iEnterAsia.iseiQ.Models;
using iEnterAsia.iseiQ.Areas.PrototypeEstimate.ViewModels;

namespace iEnterAsia.iseiQ.Areas.PrototypeEstimate.Models
{
    /// <summary>
    /// �e�X�g�p�̃��|�W�g�� 
    /// </summary>
    public class EstimateRepository : IRepository
    {
        #region stock

        private static List<Estimate> stock = new List<Estimate>()
        {
        };
        
        #endregion

        #region function

        public object FindAll(IIndexViewModel model)
        {
            Thread.Sleep(300);
            
            List<object> list = new List<object>();
            EstimateRepository.stock.ForEach(dt => list.Add(dt));
            return list;
        }

        public IViewModel Find(IIndexViewModel model)
        {
            Thread.Sleep(300);
            var m = model as EstimateIndexViewModel;
            
            return null;
        }

        /// <summary>
        /// �T�}���[�o�͗p�̃f�[�^�������s���܂��B
        /// </summary>
        /// <param name="model">���f��</param>
        /// <returns>
        /// �T�}���[�o�͗p�̃f�[�^
        /// </returns>
        public object FindForSummary(IIndexViewModel model)
        {
            return null;
        }

        /// <summary>
        /// �ڍ׏o�͗p�̃f�[�^�������s���܂��B
        /// </summary>
        /// <param name="model">���f��</param>
        /// <returns>
        /// �ڍ׏o�͗p�̃f�[�^
        /// </returns>
        public object FindForDetail(IIndexViewModel model)
        {
            return null;
        }

        public void Add(IViewModel model)
        {
            Thread.Sleep(300);
            EstimateViewModel m = model as EstimateViewModel;
            
            EstimateRepository.stock.Add(m.Estimate);
        }

        public void Edit(IViewModel model)
        {
            Thread.Sleep(300);
            EstimateViewModel m = model as EstimateViewModel;
            
            this.Delete(model);
            EstimateRepository.stock.Add(m.Estimate);
        }

        public void Delete(IViewModel model)
        {
            Thread.Sleep(300);
            EstimateViewModel m = model as EstimateViewModel;
            
        }

        #endregion

    }
}

