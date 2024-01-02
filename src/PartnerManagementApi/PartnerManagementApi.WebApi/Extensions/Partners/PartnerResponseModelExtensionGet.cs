using MainHub.Internal.PeopleAndCulture.PartnerManagement.Api.Models;
using PartnerManagement.Api.Models.API_Partners;
using PartnerManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Partners
{
    public static class PartnerResponseModelExtensionGet
    {
        public static PartnerRepoModel ToPartnerRepoModel(this ApiPartnerResponseModel model)
        {
            return new PartnerRepoModel
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
            };
        }
    }
}
