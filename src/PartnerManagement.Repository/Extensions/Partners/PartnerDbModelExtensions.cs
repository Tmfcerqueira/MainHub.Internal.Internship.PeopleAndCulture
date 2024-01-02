using MainHub.Internal.PeopleAndCulture.Models;
using MainHub.Internal.PeopleAndCulture;
using PartnerManagement.DataBase.Models;
using PartnerManagement.Repository.Models;
using Microsoft.Extensions.Azure;

namespace PartnerManagement.Repository.Extensions.Partners
{
    internal static class PartnerDbModelExtensions
    {
        public static PartnerRepoModel ToPartnerRepoModel(this Partner model)
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
                IsDeleted = model.IsDeleted,
                DeletedBy = model.DeletedBy,
            };
        }
        public static PartnerHistory ToPartnerHistoryRepoModel(this PartnerRepoModel model)
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
            };
        }
        public static PartnerHistory ToPartnerHistoryModel(this Partner model)
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
            };
        }
        public static PartnerHistoryRepoModel ToPartnerHistory_RepoModel(this PartnerHistory model)
        {
            return new PartnerHistoryRepoModel
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
