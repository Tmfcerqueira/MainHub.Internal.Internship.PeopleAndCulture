using System.Data;
using proxy = PartnerManagement.Api.Proxy.Client.Model;
using PartnerManagement.App.Models;

namespace MainHub.Internal.PeopleAndCulture.Extensions.Partner.Get
{
    public static class ContactResponseModelExtensionGet
    {
        public static ContactModel ToContactModel(this proxy.ApiContactResponseModel model)
        {
            return new ContactModel
            {
                ContactGUID = model.ContactGUID,
                PartnerGUID = model.PartnerGUID,
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                Observation = model.Observation,
                IsDeleted = model.IsDeleted,
                DeletedBy = model.DeletedBy,
                UserGUID = model.UserGUID,
            };
        }
        public static ContactHistoryModel ToContactHistoryModel(this proxy.ApiContactHistoryResponseModel model)
        {
            return new ContactHistoryModel
            {
                ContactGUID = model.ContactGUID,
                PartnerGUID = model.PartnerGUID,
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                Action = model.Action,
                ActionDate = model.ActionDate,
                UserGUID = model.UserGUID,
            };
        }
    }
}
