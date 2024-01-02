using PeopleManagementRepository.Data;
using PeopleManagementRepository.Models;
using PeopleManagementRepository.Extensions;
using Microsoft.EntityFrameworkCore;
using MainHub.Internal.PeopleAndCulture;
using MainHub.Internal.PeopleAndCulture.Extensions;
using App.Models;
using Microsoft.IdentityModel.Tokens;
using PeopleManagementDataBase;
using Microsoft.Graph;
using System;

namespace PeopleManagementRepository
{
    public class PeopleRepository : IPeopleRepository
    {
        public readonly PeopleManagementDbContext DbContext;
        public PeopleRepository(PeopleManagementDbContext dbContext)
        {
            DbContext = dbContext;
        }
        public async Task<PeopleRepoModel> CreateCollaboratorAsync(PeopleRepoModel collaborator, Guid userID)
        {
            var collaboratorMapped = collaborator.ToPeopleDbModel();
            var collaboratorHistory = collaboratorMapped.ToApiCollaboratorHistory();
            IQueryable<Collaborator> query = DbContext.Person;
            if (collaborator.ExitDate == DateTime.MinValue)
            {
                collaboratorMapped.ExitDate = DateTime.Parse("2999-12-31");
            }
            collaboratorMapped.Employee_Id = query.Max(c => c.Employee_Id) + 1;
            collaboratorHistory.Action = "Create";
            collaboratorHistory.ActionDate = DateTime.Now;
            collaboratorHistory.UserID = userID.ToString();
            collaboratorHistory.Employee_Id = collaboratorMapped.Employee_Id;

            await DbContext.Person.AddAsync(collaboratorMapped);
            collaboratorHistory.PeopleGUID = collaboratorMapped.PeopleGUID;
            await DbContext.PersonHistory.AddAsync(collaboratorHistory);
            await DbContext.SaveChangesAsync();

            return collaboratorMapped.ToPeopleRepoModel();
        }

        public async Task<List<AllPeopleRepoModel>> GetAllCollaborators(int page, int pageSize, string filter, State? list)
        {
            int skip = (page - 1) * pageSize;
            int take = pageSize;

            IQueryable<Collaborator> query = DbContext.Person.OrderBy(c => c.FirstName).ThenBy(c => c.LastName);

            try
            {
                if (!filter.IsNullOrEmpty())
                {
                    query = query.Where(c => c.FirstName!.ToLower().Contains(filter.ToLower()) || c.LastName!.ToLower().Contains(filter.ToLower()));
                }

                if (list == State.Inactive)
                {
                    query = query.Where(c => c.ExitDate < DateTime.Now);
                }
                else if (list == State.Active)
                {
                    query = query.Where(c => c.ExitDate > DateTime.Now);
                }

                var result = await query.Skip(skip).Take(take).Select(c => c.ToAllPeopleRepoModel()).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return null!;
            }
        }


        public async Task<List<PeopleHistoryRepoModel>> GetCollaboratorHistory(Guid id, int page, int pageSize)
        {
            var skip = (page - 1) * (pageSize);
            var take = pageSize;

            var query = DbContext.PersonHistory.Where(a => a.PeopleGUID == id);

            //pagination
            query = query.OrderByDescending(c => c.ActionDate).Skip(skip).Take(take);

            var collaboratorHistory = await query.Select(h => h.ToHistoryRepoModel()).ToListAsync();
            return collaboratorHistory;
        }

        public async Task<PeopleRepoModel?> GetOneCollaborator(Guid personId)
        {
            var collaborator = await DbContext.Person
                .FirstOrDefaultAsync(c => c.PeopleGUID == personId);
            return collaborator?.ToPeopleRepoModel();
        }

        public async Task<bool> DeleteCollaboratorAsync(Guid personId, Guid id)
        {
            var collaborator = await DbContext.Person.FirstOrDefaultAsync(c => c.PeopleGUID == personId);
            if (collaborator == null)
            {
                return false; // Collaborator not found
            }

            var collaboratorMapped = collaborator.ToPeopleRepoModel();
            collaboratorMapped.DeletedBy = id.ToString();
            collaboratorMapped.IsDeleted = true;
            collaboratorMapped.DeletedDate = DateTime.Now;

            var collaboratorHistory = collaboratorMapped.ToApiCollaboratorHistory();
            collaboratorHistory.Action = "Deleted";
            collaboratorHistory.ActionDate = DateTime.Now;
            collaboratorHistory.UserID = id.ToString();

            await DbContext.PersonHistory.AddAsync(collaboratorHistory);
            DbContext.Person.Remove(collaborator);
            await DbContext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateCollaboratorAsync(Guid personID, PeopleRepoModel updatedCollaborator, Guid userId)
        {
            var existingCollaborator = await DbContext.Person
                .FirstOrDefaultAsync(c => c.PeopleGUID == personID);

            if (existingCollaborator != null)
            {
                existingCollaborator.FirstName = updatedCollaborator.FirstName;
                existingCollaborator.LastName = updatedCollaborator.LastName;
                existingCollaborator.Adress = updatedCollaborator.Adress;
                existingCollaborator.BirthDate = updatedCollaborator.BirthDate;
                existingCollaborator.DependentNum = updatedCollaborator.DependentNum;
                existingCollaborator.BirthDate = updatedCollaborator.BirthDate;
                existingCollaborator.EntryDate = updatedCollaborator.EntryDate;
                existingCollaborator.ExitDate = updatedCollaborator.ExitDate;
                existingCollaborator.ChangeDate = DateTime.Now;
                existingCollaborator.CCNumber = updatedCollaborator.CCNumber;
                existingCollaborator.CCVal = updatedCollaborator.CCVal;
                existingCollaborator.Postal = updatedCollaborator.Postal;
                existingCollaborator.Locality = updatedCollaborator.Locality;
                existingCollaborator.CivilState = updatedCollaborator.CivilState;
                existingCollaborator.Country = updatedCollaborator.Country;
                existingCollaborator.ExitDate = updatedCollaborator.ExitDate;
                existingCollaborator.Status = updatedCollaborator.Status;
                existingCollaborator.SSNumber = updatedCollaborator.SSNumber;
                existingCollaborator.TaxNumber = updatedCollaborator.TaxNumber;
                existingCollaborator.Iban = updatedCollaborator.Iban;
                existingCollaborator.Email = updatedCollaborator.Email;
                existingCollaborator.Observations = updatedCollaborator.Observations;
                existingCollaborator.ContractType = (MainHub.Internal.PeopleAndCulture.Contract)updatedCollaborator.ContractType;
                existingCollaborator.Contact = updatedCollaborator.Contact;
                existingCollaborator.EmergencyContact = updatedCollaborator.EmergencyContact;

                var collaboratorHistory = existingCollaborator.ToApiCollaboratorHistory();
                collaboratorHistory.Action = "Update";
                collaboratorHistory.ActionDate = DateTime.Now;
                collaboratorHistory.UserID = userId.ToString();

                await DbContext.PersonHistory.AddAsync(collaboratorHistory);
                await DbContext.SaveChangesAsync();

                return true;
            }
            return false; // Collaborator not found, return false
        }
    }
}
