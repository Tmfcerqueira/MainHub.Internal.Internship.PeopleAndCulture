using PartnerManagement.Api.Models.API_Contact;
using PartnerManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.ContactExtensions.PContactCreate
{
    public static class ApiContactCreateRepoModelExtension
    {
        public static ContactRepoModel ToContactRepoModel(this ApiContactCreateRequestModel model)
        {
            return new ContactRepoModel
            {
                ContactGUID = Guid.NewGuid(),
                Name = model.Name,
                Email = model.Email,
                Role = model.Role,
                PhoneNumber = model.PhoneNumber,
                Department = model.Department,
                Observation = model.Observation,
                DeletedBy = Guid.Empty,
                IsDeleted = false,
                UserGUID = model.UserGUID,
            };
        }
    }
}
