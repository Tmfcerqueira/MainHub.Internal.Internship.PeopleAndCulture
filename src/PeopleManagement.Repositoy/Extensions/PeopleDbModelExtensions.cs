using MainHub.Internal.PeopleAndCulture;
using PeopleManagementDataBase;
using PeopleManagementRepository.Models;

namespace PeopleManagementRepository.Extensions
{
    public static class PeopleDbModelExtensions
    {
        public static PeopleRepoModel ToPeopleRepoModel(this Collaborator model)
        {
            return new PeopleRepoModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Adress = model.Adress,
                Postal = model.Postal,
                Locality = model.Locality,
                Country = model.Country,
                TaxNumber = model.TaxNumber,
                CCNumber = model.CCNumber,
                SSNumber = model.SSNumber,
                CCVal = model.CCVal,
                CivilState = model.CivilState,
                DependentNum = model.DependentNum,
                EntryDate = model.EntryDate,
                ExitDate = model.ExitDate,
                CreationDate = model.CreationDate,
                CreatedBy = model.CreatedBy,
                ChangeDate = model.ChangeDate,
                ChangedBy = model.ChangedBy,
                Status = model.Status,
                PeopleGUID = model.PeopleGUID,
                Iban = model.Iban,
                ContractType = (Models.Contract)model.ContractType,
                Observations = model.Observations,
                Employee_Id = model.Employee_Id,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
        public static CollaboratorHistory ToApiCollaboratorHistory(this Collaborator model)
        {
            return new CollaboratorHistory
            {
                PeopleGUID = model.PeopleGUID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                Adress = model.Adress,
                Postal = model.Postal,
                Locality = model.Locality,
                Country = model.Country,
                TaxNumber = model.TaxNumber,
                CCNumber = model.CCNumber,
                SSNumber = model.SSNumber,
                CCVal = model.CCVal,
                CivilState = model.CivilState,
                DependentNum = model.DependentNum,
                EntryDate = model.EntryDate,
                ExitDate = model.ExitDate,
                CreationDate = model.CreationDate,
                CreatedBy = model.CreatedBy,
                ChangedBy = model.ChangedBy,
                ChangeDate = model.ChangeDate,
                Status = model.Status,
                DeletedBy = model.DeletedBy,
                DeletedDate = model.DeletedDate,
                IsDeleted = model.IsDeleted,
                Iban = model.Iban,
                ContractType = model.ContractType,
                Observations = model.Observations,
                Employee_Id = model.Employee_Id,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
    }
}
