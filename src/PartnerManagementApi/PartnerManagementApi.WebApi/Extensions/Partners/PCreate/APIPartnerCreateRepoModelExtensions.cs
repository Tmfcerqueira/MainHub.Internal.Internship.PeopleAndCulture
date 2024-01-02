using PartnerManagement.Api.Models.API_Partners;
using PartnerManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Partners.PCreate
{
    public static class ApiPartnerCreateRepoModelExtensions
    {
        public static PartnerRepoModel ToPartnerRepoModel(this ApiPartnerCreateRequestModel model)
        {
            return new PartnerRepoModel
            {
                PartnerGUID = Guid.NewGuid(),
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Locality = model.Locality,
                PostalCode = model.PostalCode,
                Country = model.Country,
                TaxNumber = model.TaxNumber,
                ServiceDescription = model.ServiceDescription,
                Observation = model.Observation,
                CreationDate = DateTime.Now,
                CreatedBy = model.CreatedBy,
                ChangedDate = DateTime.Now,
                ModifiedBy = "System",
                State = "Active",
                DeletedBy = Guid.Empty,
                IsDeleted = false,
            };
        }
    }
}
