using MainHub.Internal.PeopleAndCulture.Models;
using MainHub.Internal.PeopleAndCulture;
using PartnerManagement.DataBase.Models;
using PartnerManagement.Repository.Models; // Make sure to import the correct namespace for ContactRepoModel

namespace PartnerManagement.Repository.Extensions.Contacts
{
    internal static class ContactDbModelExtension
    {
        public static ContactRepoModel ToContactRepoModel(this Contact model)
        {
            return new ContactRepoModel
            {
                // Map properties from Contact model to ContactRepoModel
                ContactGUID = model.ContactGUID,
                PartnerGUID = model.PartnerGUID,
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                Observation = model.Observation,
                UserGUID = model.UserGUID,
            };
        }
        public static ContactHistory ToContactHistoryModel(this ContactRepoModel model)
        {
            return new ContactHistory
            {
                ContactGUID = model.ContactGUID,
                PartnerGUID = model.PartnerGUID,
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                Observation = model.Observation,
                UserGUID = model.UserGUID,
            };
        }
        public static ContactHistory ToContactHistory_Model(this Contact model)
        {
            return new ContactHistory
            {
                ContactGUID = model.ContactGUID,
                PartnerGUID = model.PartnerGUID,
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                Observation = model.Observation,
                UserGUID = model.UserGUID,
            };
        }
        public static ContactHistoryRepoModel ToContactHistoryRepoModel(this ContactHistory model)
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
