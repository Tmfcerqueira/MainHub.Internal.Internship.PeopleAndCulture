﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models
{
    public class AbsenceHistory
    {

        public int Id { get; set; }
        public Guid AbsenceGuid { get; set; }
        public Guid PersonGuid { get; set; }
        public Guid AbsenceTypeGuid { get; set; }
        public DateTime AbsenceStart { get; set; }
        public DateTime AbsenceEnd { get; set; }
        public ApprovalStatus ApprovalStatus { get; set; }
        public string ApprovedBy { get; set; }
        public DateTime ApprovalDate { get; set; }
        public DateTime SubmissionDate { get; set; }
        public string Schedule { get; set; }
        public string ActionText { get; set; }
        public DateTime ActionDate { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }

        //ctor with no params

        public AbsenceHistory()
        {
            Id = 0;
            AbsenceGuid = Guid.Empty;
            PersonGuid = Guid.Empty;
            AbsenceTypeGuid = Guid.Empty;
            AbsenceStart = new DateTime(2999, 12, 31, 23, 59, 59);
            AbsenceEnd = new DateTime(2999, 12, 31, 23, 59, 59);
            ApprovalStatus = ApprovalStatus.Draft;
            ApprovedBy = "None";
            ApprovalDate = new DateTime(2999, 12, 31, 23, 59, 59);
            SubmissionDate = new DateTime(2999, 12, 31, 23, 59, 59);
            Schedule = "Full Day";
            ActionText = "None";
            ActionDate = DateTime.Now;
            UserId = Guid.Empty;
            Description = "None";
        }

    }
}
