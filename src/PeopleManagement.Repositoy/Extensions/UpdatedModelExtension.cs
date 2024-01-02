using PeopleManagement.Api.Models;
using PeopleManagementDataBase;
using PeopleManagementRepository.Models;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class UpdatedModelExtension
    {
        public static PeopleRepoModel ToPeopleRepoModel(this ApiCollaboratorUpdateModel model)
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
                ChangeDate = model.ChangeDate,
                ChangedBy = model.ChangedBy,
                Status = model.Status,
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
