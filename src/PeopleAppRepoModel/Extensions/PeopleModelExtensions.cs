using App.Models;
using PeopleManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class PeopleModelExtensions
    {
        public static ApiCollaboratorCreateRequestModel ToPeopleCreateRequestModel(this PeopleModel model)
        {
            return new ApiCollaboratorCreateRequestModel
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
                CcNumber = model.CCNumber,
                SsNumber = model.SSNumber,
                CcVal = model.CCVal,
                CivilState = model.CivilState,
                DependentNum = model.DependentNum,
                EntryDate = model.EntryDate,
                ExitDate = model.ExitDate,
                ChangedBy = model.ChangedBy,
                CreatedBy = model.CreatedBy,
                PeopleGUID = model.PeopleGUID,
                Iban = model.Iban,
                ContractType = (PeopleManagement.Api.Proxy.Client.Model.Contract?)model.ContractType!,
                Observations = model.Observations,
                EmployeeId = model.Employee_Id,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
    }
}
