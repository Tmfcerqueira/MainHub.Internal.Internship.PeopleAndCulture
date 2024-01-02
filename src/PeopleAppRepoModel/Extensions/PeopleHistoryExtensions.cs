using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Models;
using PeopleManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class PeopleHistoryExtensions
    {
        public static PeopleHistory ToPeopleHistory(this PeopleManagement.Api.Proxy.Client.Model.ApiCollaboratorHistoryResponseModel model)
        {
            return new PeopleHistory
            {
                PeopleGuid = model.PeopleGUID,
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
                DeletedBy = model.DeletedBy,
                DeletedDate = model.DeletedDate,
                IsDeleted = model.IsDeleted,
                Action = model.Action,
                ActionDate = model.ActionDate,
                UserID = model.UserID,
                Iban = model.Iban,
                ContractType = (PeopleHistory.Contract)model.ContractType!,
                Observations = model.Observations,
                Employee_Id = model.EmployeeId,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
    }
}
