using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture
{
    public class PartnerApiPagination
    {
        public int Start_Page_Number { get; set; }

        public int PageSize { get; set; }
        public PartnerApiPagination()
        {
            Start_Page_Number = 1;
            PageSize = 5;
        }
    }
}
