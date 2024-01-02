using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using PeopleManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class PeopleUpdateModelExtensions
    {
        public static ApiCollaboratorUpdateModel ToUpdateModel(this PeopleModel model)
        {
            return new ApiCollaboratorUpdateModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                BirthDate = model.BirthDate,
                Email = model.Email,
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
                ChangeDate = DateTime.Now,
                ChangedBy = model.ChangedBy,
                Status = model.Status,
                Iban = model.Iban!,
                ContractType = (PeopleManagement.Api.Proxy.Client.Model.Contract?)model.ContractType!,
                Observations = model.Observations!,
                EmployeeId = model.Employee_Id,
                Contact = model.Contact!,
                EmergencyContact = model.EmergencyContact!,
            };
        }
    }
}
