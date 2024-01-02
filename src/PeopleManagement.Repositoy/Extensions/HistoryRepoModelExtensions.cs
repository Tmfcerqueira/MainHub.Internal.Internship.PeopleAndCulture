using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class HistoryRepoModelExtensions
    {
        public static ApiCollaboratorHistoryResponseModel ToHistoryResponseModel(this PeopleHistoryRepoModel model)
        {
            return new ApiCollaboratorHistoryResponseModel
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
                Action = model.Action,
                ActionDate = model.ActionDate,
                UserID = model.UserID,
                Email = model.Email,
                Iban = model.Iban,
                ContractType = (PeopleManagement.Api.Models.ApiCollaboratorResponseModel.Contract)model.ContractType,
                Observations = model.Observations,
                Employee_Id = model.Employee_Id,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
    }
}
