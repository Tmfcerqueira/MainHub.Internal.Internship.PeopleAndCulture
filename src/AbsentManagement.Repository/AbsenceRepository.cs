using System;
using AbsentManagement.Api.Proxy.Client.Client;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Extensions;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.EntityFrameworkCore;
using AbsenceTypeRepoModel = MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models.AbsenceTypeRepoModel;
using AbsenceRepoModel = MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models.AbsenceRepoModel;
using PeopleManagementRepository.Data;
using System.Linq;
using PeopleManagementDataBase;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository
{
    public class AbsenceRepository : IAbsenceRepository
    {
        public readonly AbsenceManagementDbContext SkillHubDb;

        public AbsenceRepository(AbsenceManagementDbContext absencedbContext)
        {
            SkillHubDb = absencedbContext;
        }

        private static AbsenceRepoModel MapAbsenceToRepoModel(Absence absence, Collaborator person)
        {
            return new AbsenceRepoModel
            {
                PersonGuid = person.PeopleGUID,
                PersonName = $"{person.FirstName} {person.LastName}",
                AbsenceEnd = absence.AbsenceEnd,
                AbsenceGuid = absence.AbsenceGuid,
                AbsenceStart = absence.AbsenceStart,
                AbsenceTypeGuid = absence.AbsenceTypeGuid,
                ApprovalDate = absence.ApprovalDate,
                ApprovalStatus = absence.ApprovalStatus,
                ApprovedBy = $"{person.FirstName} {person.LastName}",
                CreatedBy = absence.CreatedBy,
                DeletedBy = absence.DeletedBy,
                IsDeleted = absence.IsDeleted,
                ModifiedBy = absence.ModifiedBy,
                Schedule = absence.Schedule,
                SubmissionDate = absence.SubmissionDate,
                Description = absence.Description
            };
        }

        //creates

        public async Task<AbsenceRepoModel> CreateAbsence(AbsenceRepoModel absenceRepoModel, Guid actionBy)
        {
            var absence = absenceRepoModel.ToAbsenceDbModel();

            var absenceHistory = absenceRepoModel.ToAbsenceHistoryModel();

            absence.AbsenceGuid = Guid.NewGuid();
            absence.CreatedBy = actionBy;

            absenceHistory.ActionText = "Create";
            absenceHistory.ActionDate = DateTime.Now;
            absenceHistory.AbsenceGuid = absence.AbsenceGuid;
            absenceHistory.UserId = actionBy;

            await SkillHubDb.Absence.AddAsync(absence);
            await SkillHubDb.AbsenceHistory.AddAsync(absenceHistory);
            await SkillHubDb.SaveChangesAsync();

            return absence.ToAbsenceRepoModel();
        }

        public async Task<AbsenceTypeRepoModel> CreateAbsenceType(AbsenceTypeRepoModel absenceTypeRepoModel, Guid actionBy)
        {

            var absenceType = absenceTypeRepoModel.ToAbsenceTypeDbModel();

            absenceType.CreatedBy = actionBy;

            var absenceTypeHistory = absenceTypeRepoModel.ToAbsenceTypeHistoryModel();

            absenceTypeHistory.ActionDate = DateTime.Now;
            absenceTypeHistory.ActionText = "Created";
            absenceTypeHistory.UserId = actionBy;

            SkillHubDb.AbsenceType.Add(absenceType);
            SkillHubDb.AbsenceTypeHistory.Add(absenceTypeHistory);
            await SkillHubDb.SaveChangesAsync();

            return absenceType.ToAbsenceTypeRepoModel();
        }


        //gets
        public async Task<(List<AbsenceRepoModel> absences, int count)> GetAbsencesByPerson(Guid person, int year, int page, int pageSize, Common.ApprovalStatus status, DateTime startDate, DateTime endDate)
        {
            int skip = (page - 1) * pageSize;
            int take = pageSize;
            var query = SkillHubDb.Absence.Where(a => a.PersonGuid == person && !a.IsDeleted);
            if (status != Common.ApprovalStatus.All)
            {
                query = query.Where(a => a.PersonGuid == person && a.ApprovalStatus == status); // filter by person and status
            }
            if (year != 0)
            {
                query = query.Where(a => a.AbsenceStart.Year == year); // filter by year
            }
            if (startDate != new DateTime())
            {
                query = query.Where(a => a.AbsenceStart >= startDate); // filter by start time
            }
            if (endDate != new DateTime())
            {
                query = query.Where(a => a.AbsenceEnd <= endDate); // filter by end date
            }

            var absencesAux = await query
                .Where(result => result.ApprovedBy != "None")
                .Join(
                    SkillHubDb.Person,
                    absence => absence.ApprovedBy,
                    person => person.PeopleGUID.ToString(),
                    (absence, person) => new { Absence = absence, Person = person }
                )
                .Select(result => MapAbsenceToRepoModel(result.Absence, result.Person))
                .ToListAsync();

            var totalCount = await query.CountAsync();

            var absences = query.Where(result => result.ApprovedBy == "None").Select(a => a.ToAbsenceRepoModel()).ToList();
            absences.AddRange(absencesAux);


            absences = absences.OrderBy(a => a.AbsenceStart) // order by StartDate in descending order
                         .Skip(skip)
                         .Take(take).ToList();

            return (absences.ToList(), totalCount);
        }



        public async Task<(List<AbsenceHistoryRepoModel> absences, int count)> GetAbsencesHistory(Guid id, int page, int pageSize)
        {
            var skip = (page - 1) * pageSize;
            var take = pageSize;

            var query = SkillHubDb.AbsenceHistory
                .Where(a => a.AbsenceGuid == id)
            .Join(
                SkillHubDb.Person,
                absenceHistory => absenceHistory.PersonGuid,
                person => person.PeopleGUID,
                (absenceHistory, person) => new { AbsenceHistory = absenceHistory, Person = person }
            )
            .Join(
                SkillHubDb.Person,
                result => result.AbsenceHistory.UserId,
                actionByPerson => actionByPerson.PeopleGUID,
                (result, actionByPerson) => new { Result = result, ActionByPerson = actionByPerson }
            )
            .Select(result => new AbsenceHistoryRepoModel
            {
                PersonGuid = result.Result.Person.PeopleGUID,
                PersonName = $"{result.Result.Person.FirstName} {result.Result.Person.LastName}",
                AbsenceEnd = result.Result.AbsenceHistory.AbsenceEnd,
                AbsenceGuid = result.Result.AbsenceHistory.AbsenceGuid,
                AbsenceStart = result.Result.AbsenceHistory.AbsenceStart,
                AbsenceTypeGuid = result.Result.AbsenceHistory.AbsenceTypeGuid,
                ApprovalDate = result.Result.AbsenceHistory.ApprovalDate,
                ApprovalStatus = result.Result.AbsenceHistory.ApprovalStatus,
                ApprovedBy = result.Result.AbsenceHistory.ApprovedBy,
                Schedule = result.Result.AbsenceHistory.Schedule,
                ActionDate = result.Result.AbsenceHistory.ActionDate,
                ActionText = result.Result.AbsenceHistory.ActionText,
                UserId = result.Result.AbsenceHistory.UserId,
                ActionBy = $"{result.ActionByPerson.FirstName} {result.ActionByPerson.LastName}",
                SubmissionDate = result.Result.AbsenceHistory.SubmissionDate,
            });

            var totalCount = await query.CountAsync();

            query = query.Skip(skip)
            .Take(take);

            var absenceHistoryList = await query.ToListAsync();
            return (absenceHistoryList, totalCount);
        }



        public async Task<AbsenceRepoModel?> GetAbsenceByPerson(Guid person, Guid absenceId)
        {
            var absence = await SkillHubDb.Absence
                .Where(a => a.PersonGuid == person && a.AbsenceGuid == absenceId)
                .FirstOrDefaultAsync();

            if (absence == null)
            {
                return null;
            }

            return absence.ToAbsenceRepoModel();
        }

        public async Task<List<AbsenceTypeRepoModel>> GetAllAbsenceTypes()
        {
            var absenceTypes = await SkillHubDb.AbsenceType
                .Where(a => !a.IsDeleted)
                .ToListAsync();
            return absenceTypes.Select(a => a.ToAbsenceTypeRepoModel()).ToList();
        }



        //delete
        public async Task<bool> DeleteAbsence(Guid absenceGuid, Guid actionBy)
        {
            var absence = await SkillHubDb.Absence.FirstOrDefaultAsync(a => a.AbsenceGuid == absenceGuid);
            if (absence == null)
            {
                return false; // absence not found
            }

            absence.DeletedBy = actionBy;
            absence.IsDeleted = true;

            var absenceHistory = absence.ToAbsenceRepoModel().ToAbsenceHistoryModel();

            absenceHistory.ActionText = "Delete";
            absenceHistory.ActionDate = DateTime.Now;
            absenceHistory.UserId = actionBy;

            await SkillHubDb.AbsenceHistory.AddAsync(absenceHistory);

            await SkillHubDb.SaveChangesAsync();

            return true; // absence deleted successfully
        }

        public async Task<bool> UpdateAbsence(AbsenceRepoModel updatedAbsence, Guid actionBy)
        {
            var absence = await SkillHubDb.Absence.FirstOrDefaultAsync(a => a.AbsenceGuid == updatedAbsence.AbsenceGuid);
            if (absence == null)
            {
                return false; // absence not found
            }

            absence.ApprovedBy = updatedAbsence.ApprovedBy;
            absence.ApprovalDate = updatedAbsence.ApprovalDate;
            absence.Description = updatedAbsence.Description;
            absence.ApprovalStatus = updatedAbsence.ApprovalStatus;
            absence.AbsenceStart = updatedAbsence.AbsenceStart;
            absence.AbsenceEnd = updatedAbsence.AbsenceEnd;
            absence.AbsenceTypeGuid = updatedAbsence.AbsenceTypeGuid;
            absence.Schedule = updatedAbsence.Schedule;
            absence.SubmissionDate = DateTime.Now;
            absence.ModifiedBy = actionBy;

            var absenceHistory = absence.ToAbsenceRepoModel().ToAbsenceHistoryModel();

            absenceHistory.ActionText = "Update";
            absenceHistory.ActionDate = DateTime.Now;
            absenceHistory.UserId = actionBy;

            await SkillHubDb.AbsenceHistory.AddAsync(absenceHistory);

            await SkillHubDb.SaveChangesAsync();

            return true; // absence updated successfully
        }


        //admin
        public async Task<(List<AbsenceRepoModel> absences, int totalCount)> GetAllAbsences(int page, int pageSize, Common.ApprovalStatus status)
        {
            int skip = (page - 1) * pageSize;
            int take = pageSize;
            var query = SkillHubDb.Absence
                .Where(a => !a.IsDeleted);

            if (status != Common.ApprovalStatus.All)
            {
                query = query.Where(a => a.ApprovalStatus == status);
            }

            var totalCount = await query.CountAsync();

            query = query.OrderBy(a => a.AbsenceStart) // order by StartDate in descending order
                         .Skip(skip)
                            .Take(take);

            var absences = await query
                .Join(
                    SkillHubDb.Person,
                    absence => absence.PersonGuid,
                    person => person.PeopleGUID,
                    (absence, person) => new { Absence = absence, Person = person }
                )
                .Select(result => MapAbsenceToRepoModel(result.Absence, result.Person))
                .ToListAsync();

            return (absences, totalCount);
        }






    }

}
