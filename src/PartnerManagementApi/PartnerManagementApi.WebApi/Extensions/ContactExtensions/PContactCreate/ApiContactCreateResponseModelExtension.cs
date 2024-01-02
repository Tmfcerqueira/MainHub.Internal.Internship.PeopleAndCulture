using PartnerManagement.Api.Models.API_Contact;
using PartnerManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.ContactExtensions.PContactCreate
{
    public static class ApiContactCreateResponseModelExtension
    {
        public static ApiContactCreateResponseModel ToContactCreateResponseModel(this ContactRepoModel model)
        {
            return new ApiContactCreateResponseModel
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
