using MainHub.Internal.PeopleAndCulture.Models;
using MainHub.Internal.PeopleAndCulture;
using PartnerManagement.DataBase.Models;
using PartnerManagement.Repository.Models;

namespace PartnerManagement.Repository.Extensions.Contacts
{
    internal static class ContactRepoModelExtension
    {
        public static Contact ToContactDbModel(this ContactRepoModel model)
        {
            return new Contact
            {
                ContactGUID = model.ContactGUID,
                PartnerGUID = model.PartnerGUID,
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                Observation = model.Observation,
            };
        }
        public static ContactHistoryRepoModel ToContactHistoryDbModel(this ContactHistory model)
        {
            return new ContactHistoryRepoModel
            {
                ContactGUID = model.ContactGUID,
                PartnerGUID = model.PartnerGUID,
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                Observation = model.Observation,
                Action = model.Action,
                ActionDate = model.ActionDate,
                UserGUID = model.UserGUID,
            };
        }
    }
}
