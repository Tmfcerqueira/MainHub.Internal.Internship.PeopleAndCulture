using PartnerManagement.Api.Proxy.Client.Model;
using PartnerManagement.App.Models;

namespace PartnerManagement.App.Repository.Extensions.Contact
{
    public static class ContactModelExtension
    {
        public static ApiContactCreateRequestModel ToContactRequestModel(this ContactModel model)
        {
            return new ApiContactCreateRequestModel
            {
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                Observation = model.Observation,
            };
        }
        public static ContactUpdateModel ToContactUpdateModel(this ContactModel model)
        {
            return new ContactUpdateModel
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
