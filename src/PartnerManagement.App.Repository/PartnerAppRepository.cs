using PartnerManagement.Api.Proxy.Client.Api;
using PartnerManagement.Api.Proxy.Client.Client;
using PartnerManagement.App.Models;
using PartnerManagement.App.Repository.Extensions.Partner;
using PartnerManagement.App.Repository.Extensions.Contact;
using Microsoft.AspNetCore.Mvc;
using proxy = PartnerManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.Extensions.Partner.Get;
using MainHub.Internal.PeopleAndCulture;
using PartnerUpdateModel = PartnerManagement.Api.Proxy.Client.Model.PartnerUpdateModel;
using ContactUpdateModel = PartnerManagement.Api.Proxy.Client.Model.ContactUpdateModel;

namespace PartnerManagement.App.Repository
{
    public class PartnerAppRepository : IPartnerRepository
    {
        public PartnerAppRepository(IPartnerApi partnerProxy)
        {
            PartnerApiProxy = partnerProxy;
        }
        public IPartnerApi PartnerApiProxy { get; set; }


        //  CREATE
        public async Task<PartnerModel> App_Create_Partner_By_Form(PartnerModel partnerRepoModel)
        {
            //convert from normal to request
            var partnerMapped = partnerRepoModel.ToPartnerRequestModel(); // metodo extensão

            //use the proxy to call the api
            var response = await PartnerApiProxy.ApiPartnerPostAsync(partnerMapped);

            //convert from response to partnerModel
            var finalPartnerModel = response.ToPartnerResponseModel(); // metodo extensão

            //return normal model
            return finalPartnerModel;
        }
        public async Task<ContactModel> App_Create_Contact_By_Form(Guid partnerGuid, ContactModel contactRepoModel)
        {
            //convert from normal to request
            var contactRequest = contactRepoModel.ToContactRequestModel();

            //use the proxy to call the api
            var response = await PartnerApiProxy.ApiPartnerPartnerGuidContactPostAsync(partnerGuid, contactRequest);

            //convert from response to partnerconctactlistModel
            var finalContactModel = ContactResponseModelExtension.ToContactModel(response);

            //return normal model
            return finalContactModel;
        }


        // GETS
        public async Task<(List<PartnerModel>, int count)> Get_All_Partners_Async(int num_page, int pageSize, string name)
        {
            var responseModelAPI = await PartnerApiProxy.ApiPartnerGetAsync(num_page, pageSize, name);

            List<PartnerModel> model = responseModelAPI.Partners.Select(p => p.ToPartnerModel()).ToList();

            return (model, responseModelAPI.TotalCount);
        }

        // GETS

        public async Task<(List<ContactModel>, int)> Get_All_Contacts_Async(Guid partnerGuid, int num_page, int pageSize, string name)
        {
            var responseModelAPI = await PartnerApiProxy.ApiPartnerPartnerGuidContactGetAsync(partnerGuid, num_page, pageSize, name);

            List<ContactModel> model = responseModelAPI.Contacts.Select(c => c.ToContactModel()).ToList();

            return (model, responseModelAPI.TotalCount);
        }


        public async Task<ContactModel> Get_Specific_Contact_From_One_Partner(Guid partnerGuid, Guid contactGuid)
        {
            proxy.ApiContactResponseModel contactResponseModel = await PartnerApiProxy.ApiPartnerPartnerGuidContactContactGuidGetAsync(partnerGuid, contactGuid);

            ContactModel contactModel = contactResponseModel.ToContactModel();

            return contactModel;
        }
        public async Task<PartnerModel> Get_Partner_By_Guid_Async(Guid partnerGuid)
        {
            proxy.ApiPartnerResponseModel partnerResponseModel = await PartnerApiProxy.ApiPartnerPartnerGuidGetAsync(partnerGuid);

            PartnerModel partnerModel = partnerResponseModel.ToPartnerModel();

            return partnerModel;
        }


        //UPDATES
        public async Task<bool> Update_Partner_Async(Guid partnerGuid, PartnerModel modelNew)
        {
            try
            {
                PartnerUpdateModel updatedModel = modelNew.ToPartnerUpdateModel();
                await PartnerApiProxy.ApiPartnerPartnerGuidPutAsync(partnerGuid, updatedModel);
                return true;
            }
            catch (ApiException)
            {
                return false;
            }


        }
        public async Task<bool> Update_Contact_Async(Guid partnerGuid, Guid contactGuid, ContactModel modelNew)
        {
            try
            {
                ContactUpdateModel updatedModel = modelNew.ToContactUpdateModel();
                await PartnerApiProxy.ApiPartnerPartnerGuidContactContactGuidPutAsync(partnerGuid, contactGuid, updatedModel);
                return true;
            }
            catch (ApiException)
            {
                return false;
            }
        }


        //DELETES
        public async Task<bool> Delete_Partner_Async(Guid partnerGuid)
        {
            try
            {
                await PartnerApiProxy.ApiPartnerPartnerGuidDeleteAsync(partnerGuid);
                return true;
            }
            catch (ApiException)
            {
                return false;

            }
        }
        public async Task<bool> Delete_Contact_Async(Guid partnerGuid, Guid contactGuid)
        {
            try
            {
                await PartnerApiProxy.ApiPartnerPartnerGuidContactContactGuidDeleteAsync(partnerGuid, contactGuid);
                return true;
            }
            catch (ApiException)
            {
                return false;

            }
        }
        public async Task<bool> Delete_All_Contacts_Async(Guid partnerGuid)
        {
            try
            {
                await PartnerApiProxy.ApiPartnerPartnerGuidContactDeleteAsync(partnerGuid);
                return true;
            }
            catch (ApiException)
            {
                return false;
            }
        }


        // History Gets
        public async Task<(List<PartnerHistoryModel>, int count)> Get_All_Partner_History_From_Specific_Partner(Guid partnerGuid, int num_page, int pageSize)
        {
            var partnerHistoryResponseModel = await PartnerApiProxy.ApiPartnerPartnerGUIDPartnerhistoryGetAsync(partnerGuid, num_page, pageSize);

            List<PartnerHistoryModel> partnerHistoryModel = partnerHistoryResponseModel.Partners.Select(a => a.ToPartnerHistoryResponseModel()).ToList();

            return (partnerHistoryModel, partnerHistoryResponseModel.TotalCount);

        }

        public async Task<(List<ContactHistoryModel>, int count)> Get_All_Contact_History_From_Specific_Partner_And_Contact(Guid partnerGuid, Guid contactGuid, int num_page, int pageSize)
        {
            var contactHistoryResponseModel = await PartnerApiProxy.ApiPartnerPartnerGUIDContactContactGuidContacthistoryGetAsync(partnerGuid, contactGuid, num_page, pageSize);

            List<ContactHistoryModel> contactHistoryModel = contactHistoryResponseModel.Contacts.Select(a => a.ToContactHistoryModel()).ToList();

            return (contactHistoryModel, contactHistoryResponseModel.TotalCount);
        }
    }
}
