using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Api.Models;

namespace PartnerManagement.Api.Models
{
    public class ApiPartnerResponseWithCount
    {
        public List<ApiPartnerResponseModel> Partners { get; set; }

        public int TotalCount { get; set; }
        public ApiPartnerResponseWithCount()
        {
            Partners = new List<ApiPartnerResponseModel>();
            TotalCount = 0;
        }
    }
}
