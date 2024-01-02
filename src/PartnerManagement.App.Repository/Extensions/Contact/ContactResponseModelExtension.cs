using PartnerManagement.Api.Proxy.Client.Model;
using PartnerManagement.App.Models;

namespace PartnerManagement.App.Repository.Extensions.Contact
{
    public static class ContactResponseModelExtension
    {
        public static ContactModel ToContactModel(this ApiContactCreateResponseModel model)
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
                UserGUID = model.UserGUID,
            };
        }
    }
}
