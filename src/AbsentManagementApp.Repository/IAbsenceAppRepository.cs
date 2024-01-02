using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsentManagement.Api.Proxy.Client.Model;
using App.Models;
using App.Repository;
using MainHub.Internal.PeopleAndCulture.App.Models;
using MainHub.Internal.PeopleAndCulture.App.Repository;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models
{
    public interface IAbsenceAppRepository
    {

        //creates
        Task<(AbsenceModel, int)> CreateAbsenceAsync(AbsenceModel absenceModel, Guid actionBy);


        //gets

        Task<(List<AbsenceModel>, int errorCode, int count)> GetAbsencesByPersonAsync(Guid person, int year, int page, int pageSize, ApprovalStatus status, DateTime startDate, DateTime endDate);

        Task<(AbsenceModel, int)> GetAbsenceByPersonAsync(Guid personGuid, Guid absenceId);

        Task<List<AbsenceTypeModel>> GetAllAbsenceTypesAsync();

        Task<(List<AbsenceHistoryModel>, int errorCode, int count)> GetAbsenceHistory(Guid id, string personGuid, int page, int pageSize);

        //delete

        Task<bool> DeleteAbsenceAsync(Guid personGuid, Guid absenceGuid, Guid actionBy);

        //update
        Task<bool> UpdateAbsenceAsync(Guid personGuid, Guid absenceId, AbsenceModel updatedModel, Guid actionBy);

        //methods
        void SetTypeTextByGuidAsync(AbsenceModel model);

        void SetHistoryTypeTextByGuidAsync(AbsenceHistoryModel model);

        Task<bool> SubmitAllDraftAsync(Guid personGuid, Guid actionBy);

        //admin

        Task<(List<AbsenceModel>, int errorCode, int count)> GetAllAbsencesAsync(int page, int pageSize, ApprovalStatus status);

        Task<(List<AllPeopleModel>, int count)> GetAllPeople(int page, int pageSize, string filter, State list);

    }
}
