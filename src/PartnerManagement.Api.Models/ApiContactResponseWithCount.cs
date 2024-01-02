using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Api.Models;

namespace PartnerManagement.Api.Models
{
    public class ApiContactResponseWithCount
    {
        public List<ApiContactResponseModel> Contacts { get; set; }
        public int TotalCount { get; set; }

        public ApiContactResponseWithCount()
        {
            Contacts = new List<ApiContactResponseModel>();
            TotalCount = 0;
        }
    }
}
