
using MainHub.Internal.PeopleAndCulture.Models;
using MainHub.Internal.PeopleAndCulture;
using PartnerManagement.DataBase.Models;
using PartnerManagement.Repository.Models;

namespace PartnerManagement.Repository.Extensions.Partners
{
    public static class PartnerRepoModelExtensions
    {
        public static Partner ToPartnerDbModel(this PartnerRepoModel model)
        {
            return new Partner
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
                IsDeleted = model.IsDeleted,
                DeletedBy = model.DeletedBy,
            };
        }
        public static PartnerHistory ToPartnerHistoryDbModel(this PartnerHistoryRepoModel model)
        {
            return new PartnerHistory
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
                Action = model.Action,
                ActionDate = model.ActionDate,
                UserGUID = model.UserGUID,
            };
        }
    }
}
