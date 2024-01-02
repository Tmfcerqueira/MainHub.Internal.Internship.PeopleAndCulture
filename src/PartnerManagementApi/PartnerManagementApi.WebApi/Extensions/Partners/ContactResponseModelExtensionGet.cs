using System.Data;
using PartnerManagement.Api.Proxy.Client.Model;
using PartnerManagement.App.Models;

namespace MainHub.Internal.PeopleAndCulture.Extensions.Partner.Get
{
    public static class ContactResponseModelExtensionGet
    {
        public static ContactModel ToContactModel(this ApiContactResponseModel model)
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
            };
        }
    }
}
