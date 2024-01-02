using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Extensions
{
    internal static class AbsenceRepoModelExtensions
    {
        public static Absence ToAbsenceDbModel(this AbsenceRepoModel model)
        {
            return new Absence
            {
                PersonGuid = model.PersonGuid,
                AbsenceGuid = model.AbsenceGuid,
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                ApprovalDate = model.ApprovalDate,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                SubmissionDate = model.SubmissionDate,
                Schedule = model.Schedule,
                CreatedBy = model.CreatedBy,
                ModifiedBy = model.ModifiedBy,
                DeletedBy = model.DeletedBy,
                Description = model.Description
            };
        }

        public static AbsenceHistory ToAbsenceHistoryModel(this AbsenceRepoModel model)
        {
            return new AbsenceHistory
            {
                PersonGuid = model.PersonGuid,
                AbsenceGuid = model.AbsenceGuid,
                AbsenceEnd = model.AbsenceEnd,
                AbsenceStart = model.AbsenceStart,
                AbsenceTypeGuid = model.AbsenceTypeGuid,
                ApprovalDate = model.ApprovalDate,
                ApprovalStatus = model.ApprovalStatus,
                ApprovedBy = model.ApprovedBy,
                SubmissionDate = model.SubmissionDate,
                Schedule = model.Schedule,
                Description = model.Description
            };
        }


    }
}
