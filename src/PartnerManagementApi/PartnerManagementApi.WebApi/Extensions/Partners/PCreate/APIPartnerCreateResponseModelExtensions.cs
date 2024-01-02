using PartnerManagement.Api.Models.API_Partners;
using PartnerManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Partners.PCreate
{
    public static class ApiPartnerCreateResponseModelExtensions
    {
        public static ApiPartnerCreateResponseModel ToPartnerCreateResponseModel(this PartnerRepoModel model)
        {
            return new ApiPartnerCreateResponseModel
            {
                PartnerGUID = model.PartnerGUID,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Locality = model.Locality,
                PostalCode = model.PostalCode,
                Country = model.Country,
                TaxNumber = model.TaxNumber,
                ServiceDescription = model.ServiceDescription,
                Observation = model.Observation,
                CreationDate = model.CreationDate,
                CreatedBy = model.CreatedBy,
                ChangedDate = model.ChangedDate,
                ModifiedBy = model.ModifiedBy,
                State = model.State,
                DeletedBy = model.DeletedBy,
                IsDeleted = model.IsDeleted,
            };
        }
    }
}
