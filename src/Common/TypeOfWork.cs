using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.Common
{
    public enum TypeOfWork
    {
        [Display(Description = "Regular")]
        Regular = 1,
        [Display(Description = "Extra")]
        Extra = 2
    }
}
