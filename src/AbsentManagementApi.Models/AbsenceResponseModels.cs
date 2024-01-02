using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models
{
    public class AbsenceResponseModels
    {

        public List<AbsenceResponseModel> Absences { get; set; }

        public int AllDataCount { get; set; }


        public AbsenceResponseModels()
        {
            Absences = new List<AbsenceResponseModel>();
            AllDataCount = 0;
        }
    }
}

