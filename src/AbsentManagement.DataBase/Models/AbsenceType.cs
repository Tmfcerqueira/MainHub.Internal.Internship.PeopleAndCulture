using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models
{
    public class AbsenceType
    {
        public int Id { get; set; }
        public Guid TypeGuid { get; set; }
        public string Type { get; set; }
        public bool IsDeleted { get; set; }

        public Guid CreatedBy { get; set; }
        public Guid ModifiedBy { get; set; }
        public Guid DeletedBy { get; set; }

        public virtual ICollection<Absence>? Absences { get; set; }

        //ctor with no params

        public AbsenceType()
        {
            Id = 0;
            TypeGuid = Guid.Empty;
            Type = "Has no Type";
            IsDeleted = false;
            CreatedBy = Guid.Empty;
            ModifiedBy = Guid.Empty;
            DeletedBy = Guid.Empty;
        }

    }
}
