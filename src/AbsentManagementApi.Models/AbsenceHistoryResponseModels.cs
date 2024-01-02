using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models
{
    public class AbsenceHistoryResponseModels
    {

        public List<AbsenceHistoryResponseModel> AbsenceHistory { get; set; }

        public int AllDataCount { get; set; }


        public AbsenceHistoryResponseModels()
        {
            AbsenceHistory = new List<AbsenceHistoryResponseModel>();
            AllDataCount = 0;
        }

    }
}
