using System.Net;
using PartnerManagement.Repository.Models;

namespace MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Miscellaneous
{
    public static class ApiPartnerUpdateExtensions
    {
        public static PartnerRepoModel ToPartnerRepoModel(this PartnerUpdateModel model)
        {
            return new PartnerRepoModel()
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
                State = model.State
            };
        }
    }
}
