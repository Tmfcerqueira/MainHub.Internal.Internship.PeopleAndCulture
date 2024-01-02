using App.Models;
using PeopleManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.App.Repository.Extensions
{
    public static class PeopleAppRepoModelExtensions
    {
        public static PeopleModel ToOnePeopleModel(this ApiCollaboratorResponseModel model)
        {
            return new PeopleModel
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
                CCNumber = model.CcNumber,
                SSNumber = model.SsNumber,
                CCVal = model.CcVal,
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
                ContractType = (PeopleModel.Contract)model.ContractType!,
                Observations = model.Observations,
                Employee_Id = model.EmployeeId,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
    }
}

