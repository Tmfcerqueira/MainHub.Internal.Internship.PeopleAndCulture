using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Api.Models;

namespace MainHub.Internal.PeopleAndCulture
{
    public class ApiPartnerHistoryResponseWithCount
    {
        public List<ApiPartnerHistoryResponseModel> Partners { get; set; }

        public int TotalCount { get; set; }

        public ApiPartnerHistoryResponseWithCount()
        {
            Partners = new List<ApiPartnerHistoryResponseModel>();
            TotalCount = 0;
        }
    }
}
