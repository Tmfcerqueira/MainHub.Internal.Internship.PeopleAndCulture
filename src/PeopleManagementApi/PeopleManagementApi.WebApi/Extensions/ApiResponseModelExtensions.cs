using PeopleManagement.Api.Models;
using PeopleManagementRepository.Models;

namespace MainHub.Internal.PeopleAndCulture.PeopleManagement.API.Extensions
{
    public static class ApiResponseModelExtensions
    {
        public static ApiCollaboratorResponseModel ToApiResponseModel(this PeopleRepoModel model)
        {
            return new ApiCollaboratorResponseModel
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
                Email = model.Email,
                Iban = model.Iban,
                ContractType = (ApiCollaboratorResponseModel.Contract)model.ContractType,
                Observations = model.Observations,
                Employee_Id = model.Employee_Id,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
    }
}
