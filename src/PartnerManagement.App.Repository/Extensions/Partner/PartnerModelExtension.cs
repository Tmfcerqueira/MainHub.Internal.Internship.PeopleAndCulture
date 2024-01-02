using PartnerManagement.Api.Proxy.Client.Model;
using PartnerManagement.App.Models;

namespace PartnerManagement.App.Repository.Extensions.Partner
{
    public static class PartnerModelExtension
    {
        public static ApiPartnerCreateRequestModel ToPartnerRequestModel(this PartnerModel model)
        {
            return new ApiPartnerCreateRequestModel
            {
                Name = model.Name,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                Locality = model.Locality,
                PostalCode = model.PostalCode,
                Country = model.Country,
                TaxNumber = model.TaxNumber,
                ServiceDescription = model.ServiceDescription,
                Observation = model.Observation,
                CreatedBy = model.CreatedBy,
            };
        }
        public static PartnerUpdateModel ToPartnerUpdateModel(this PartnerModel model)
        {
            return new PartnerUpdateModel
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
