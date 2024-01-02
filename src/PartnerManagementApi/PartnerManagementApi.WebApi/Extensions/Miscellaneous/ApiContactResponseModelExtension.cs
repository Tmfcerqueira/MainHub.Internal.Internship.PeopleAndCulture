using MainHub.Internal.PeopleAndCulture.PartnerManagement.Api.Models;
using PartnerManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Miscellaneous
{
    public static class ApiContactResponseModelExtension
    {
        public static ApiContactResponseModel ToContactResponseModel(this ContactRepoModel model)
        {
            return new ApiContactResponseModel
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
        public static ApiContactHistoryResponseModel ToContactHistoryResponseModel(this ContactHistoryRepoModel model)
        {
            return new ApiContactHistoryResponseModel
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
