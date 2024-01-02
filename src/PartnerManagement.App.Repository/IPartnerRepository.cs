using MainHub.Internal.PeopleAndCulture;
using Microsoft.AspNetCore.Mvc;
using PartnerManagement.App.Models;

namespace PartnerManagement.App.Repository
{
    public interface IPartnerRepository
    {
        //CREATES
        Task<PartnerModel> App_Create_Partner_By_Form(PartnerModel partnerRepoModel);
        Task<ContactModel> App_Create_Contact_By_Form(Guid partnerGuid, ContactModel contactRepoModel);


        //GETS
        Task<PartnerModel> Get_Partner_By_Guid_Async(Guid partnerGuid);
        Task<(List<PartnerModel>, int count)> Get_All_Partners_Async(int num_page, int pageSize, string name);
        Task<(List<ContactModel>, int count)> Get_All_Contacts_Async(Guid partnerGuid, int num_page, int pageSize, string name);

        //UPDATES
        Task<bool> Update_Partner_Async(Guid partnerGuid, PartnerModel modelNew);
        Task<bool> Update_Contact_Async(Guid partnerGuid, Guid contactGuid, ContactModel modelNew);


        //DELETES
        Task<bool> Delete_Partner_Async(Guid partnerGuid);
        Task<bool> Delete_Contact_Async(Guid partnerGuid, Guid contactGuid);
        Task<bool> Delete_All_Contacts_Async(Guid partnerGuid);

        // History Gets
        Task<(List<PartnerHistoryModel>, int count)> Get_All_Partner_History_From_Specific_Partner(Guid partnerGuid, int num_page, int pageSize);
        Task<(List<ContactHistoryModel>, int count)> Get_All_Contact_History_From_Specific_Partner_And_Contact(Guid partnerGuid, Guid contactGuid, int num_page, int pageSize);
    }
}
