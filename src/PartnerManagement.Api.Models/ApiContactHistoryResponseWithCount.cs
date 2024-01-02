using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Api.Models;

namespace MainHub.Internal.PeopleAndCulture
{
    public class ApiContactHistoryResponseWithCount
    {
        public List<ApiContactHistoryResponseModel> Contacts { get; set; }

        public int TotalCount { get; set; }

        public ApiContactHistoryResponseWithCount()
        {
            Contacts = new List<ApiContactHistoryResponseModel>();
            TotalCount = 0;
        }
    }
}
