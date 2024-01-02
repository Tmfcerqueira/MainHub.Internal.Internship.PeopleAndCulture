using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using proxy = PartnerManagement.Api.Proxy.Client.Model;
using PartnerManagement.App.Models;

namespace MainHub.Internal.PeopleAndCulture.Extensions.Partner.Get
{
    public static class PartnerResponseModelExtensionGet
    {
        public static PartnerModel ToPartnerModel(this proxy.ApiPartnerResponseModel model)
        {
            return new PartnerModel
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
        public static PartnerHistoryModel ToPartnerHistoryModel(this proxy.ApiPartnerHistoryResponseModel model)
        {
            return new PartnerHistoryModel
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
