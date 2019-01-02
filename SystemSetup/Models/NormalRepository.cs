using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace iEnterAsia.iseiQ.Models
{
    public class NormalRepository : IRepository
    {
        public dynamic FindAll(IIndexViewModel model)
        {
            return model.FindAll();
        }

        public IViewModel Find(IIndexViewModel model)
        {
            return model.Find();
        }

        public dynamic FindForSummary(IIndexViewModel model)
        {
            return model.FindForSummary();
        }

        public dynamic FindForDetail(IIndexViewModel model)
        {
            return model.FindForDetail();
        }

        public void Add(IViewModel model)
        {
            model.Add();
        }

        public void Edit(IViewModel model)
        {
            model.Edit();
        }

        public void Delete(IViewModel model)
        {
            model.Delete();
        }

    }
}
