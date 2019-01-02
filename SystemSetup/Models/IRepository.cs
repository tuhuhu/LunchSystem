using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace iEnterAsia.iseiQ.Models
{
    public interface IRepository
    {
        object FindAll(IIndexViewModel model);

        IViewModel Find(IIndexViewModel model);

        object FindForSummary(IIndexViewModel model);

        object FindForDetail(IIndexViewModel model);

        //void Add(IViewModel model, LoginInfo info);

        //void Edit(IViewModel model, LoginInfo info);

        //void Delete(IViewModel model, LoginInfo info);

        void Add(IViewModel model);

        void Edit(IViewModel model);

        void Delete(IViewModel model);

    }
}
