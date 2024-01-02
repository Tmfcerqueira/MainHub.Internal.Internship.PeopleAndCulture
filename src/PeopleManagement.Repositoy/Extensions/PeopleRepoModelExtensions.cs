using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture;
using PeopleManagement.Api.Models;
using PeopleManagementDataBase;
using PeopleManagementRepository.Models;


namespace PeopleManagementRepository.Extensions
{
    internal static class PeopleRepoModelExtensions
    {
        public static Collaborator ToPeopleDbModel(this PeopleRepoModel model)
        {
            return new Collaborator
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
                ChangeDate = model.ChangeDate,
                ChangedBy = model.ChangedBy,
                Status = model.Status,
                Iban = model.Iban,
                ContractType = (MainHub.Internal.PeopleAndCulture.Contract)model.ContractType,
                Observations = model.Observations,
                Employee_Id = model.Employee_Id,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
        public static CollaboratorHistory ToApiCollaboratorHistory(this PeopleRepoModel model)
        {
            return new CollaboratorHistory
            {
                PeopleGUID = model.PeopleGUID,
                FirstName = model.FirstName,
                LastName = model.LastName,
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
                Email = model.Email,
                Iban = model.Iban,
                ContractType = (MainHub.Internal.PeopleAndCulture.Contract)model.ContractType,
                Observations = model.Observations,
                Employee_Id = model.Employee_Id,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
    }
}
