using MainHub.Internal.PeopleAndCulture.Extensions;
using Microsoft.AspNetCore.Identity;
using PeopleManagement.Api.Models;
using PeopleManagementRepository.Models;

namespace MainHub.Internal.PeopleAndCulture.PeopleManagement.API.Extensions
{
    public static class ApiPeopleCreateRequestModelExtensions
    {
        public static PeopleRepoModel ToPeopleRepoModel(this ApiCollaboratorCreateRequestModel model)
        {
            return new PeopleRepoModel
            {
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
                CreationDate = DateTime.Now,
                CreatedBy = model.CreatedBy,
                ChangeDate = DateTime.Now,
                ChangedBy = model.ChangedBy,
                Status = "Active",
                PeopleGUID = model.PeopleGUID,
                Email = model.Email,
                Iban = model.Iban,
                ContractType = (PeopleManagementRepository.Models.Contract)model.ContractType,
                Observations = model.Observations,
                Employee_Id = model.Employee_Id,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
    }
}
