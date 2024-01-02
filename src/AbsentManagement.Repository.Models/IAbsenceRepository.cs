using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Mvc;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models
{
    public interface IAbsenceRepository
    {
        //creates

        Task<AbsenceRepoModel> CreateAbsence(AbsenceRepoModel absenceRepoModel, Guid actionBy);

        Task<AbsenceTypeRepoModel> CreateAbsenceType(AbsenceTypeRepoModel absenceTypeRepoModel, Guid actionBy);

        //gets
        Task<(List<AbsenceRepoModel> absences, int count)> GetAbsencesByPerson(Guid person, int year, int page, int pageSize, Common.ApprovalStatus status, DateTime startDate, DateTime endDate);

        Task<AbsenceRepoModel?> GetAbsenceByPerson(Guid person, Guid absenceId);

        Task<(List<AbsenceHistoryRepoModel> absences, int count)> GetAbsencesHistory(Guid id, int page, int pageSize);


        Task<List<AbsenceTypeRepoModel>> GetAllAbsenceTypes();

        //delete

        Task<bool> DeleteAbsence(Guid absenceGuid, Guid actionBy);

        //update

        Task<bool> UpdateAbsence(AbsenceRepoModel updatedAbsence, Guid actionBy);



        //admin
        Task<(List<AbsenceRepoModel> absences, int totalCount)> GetAllAbsences(int page, int pageSize, Common.ApprovalStatus status);


    }
}
