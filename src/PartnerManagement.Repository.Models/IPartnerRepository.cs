using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture;
using PartnerManagement.DataBase.Models;

namespace PartnerManagement.Repository.Models
{
    public interface IPartnerRepository
    {
        //**************************************************
        //*               No History Models                *
        //**************************************************

        // Creates
        Task<PartnerRepoModel> Create_Partner_Async(PartnerRepoModel partner);
        Task<ContactRepoModel> Create_Contact_Async(ContactRepoModel contact);


        // Gets with Guids
        Task<PartnerRepoModel> Get_Partner_By_Guid_Async(Guid partnerGuid);
        Task<(List<ContactRepoModel>, int count)> Get_Partner_Contacts_Async(Guid partnerGuid, int num_page, int pageSize, string? name);
        Task<ContactRepoModel> Get_Partner_Contacts_By_Guid_Async(Guid partnerGuid, Guid contactGuid);

        // GETS all without Guids
        Task<(List<PartnerRepoModel>, int count)> Get_All_Partners_Async(int num_page, int pageSize, string? name);

        // UPDATES
        Task<Partner> Update_Partner_Async(PartnerRepoModel partnerModel);
        Task<Contact> Update_Contact_Async(ContactRepoModel contactRepoModel);

        //DELETES
        Task<bool> Delete_Partner_Async(Guid partnerGuid);
        Task<bool> Delete_Contact_Async(Guid partnerGuid, Guid contactGuid);
        Task<bool> Delete_All_Contacts_By_PartnerGuid_Async(Guid partnerGuid);


        //**************************************************
        //*                HISTORY METHODS                 *
        //**************************************************

        // Get All Historys

        Task<(List<PartnerHistoryRepoModel>, int count)> Get_All_Partners_History_Async(Guid partnerGuid, int num_page, int pageSize);
        Task<(List<ContactHistoryRepoModel>, int count)> Get_All_Contacts_History_Async(Guid partnerGuid, Guid contactGuid, int num_page, int pageSize);
    }
}
