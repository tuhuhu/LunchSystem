using System;
using System.Collections.Generic;

namespace iEnterAsia.iseiQ.Models
{
    public interface IIndexViewModel
    {
        List<dynamic> FindAll();

        IViewModel Find();

        dynamic FindForSummary();

        dynamic FindForDetail();
    }
}
