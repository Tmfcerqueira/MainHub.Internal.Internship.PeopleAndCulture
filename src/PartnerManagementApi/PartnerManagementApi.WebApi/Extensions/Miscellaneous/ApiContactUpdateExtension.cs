using PartnerManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Miscellaneous
{
    public static class ApiContactUpdateExtension
    {
        public static ContactRepoModel ToContactRepoModel(this ContactUpdateModel model)
        {
            return new ContactRepoModel()
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
