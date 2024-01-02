using PartnerManagement.DataBase;
using PartnerManagement.DataBase.Models;
using PartnerManagement.Repository.Extensions.Partners;
using PartnerManagement.Repository.Models;
using PartnerManagement.Repository.Extensions.Contacts;
using Microsoft.EntityFrameworkCore;
using MainHub.Internal.PeopleAndCulture;
using System.Linq;

namespace PartnerManagement.Repository
{
    public class PartnerRepository : IPartnerRepository
    {
        public readonly PartnerManagementDBContext DbContext;

        public PartnerRepository(PartnerManagementDBContext dbContext)
        {
            DbContext = dbContext;
        }

        //**************************************************
        //*               No History Models                *
        //**************************************************

        //Create Partners | Contacts
        public async Task<PartnerRepoModel> Create_Partner_Async(PartnerRepoModel partner)
        {
            var partnerMapped = partner.ToPartnerDbModel();
            var partnerHistory = partner.ToPartnerHistoryRepoModel();

            partnerHistory.Action = "Create";
            partnerHistory.ActionDate = DateTime.Now;
            partnerHistory.UserGUID = Guid.Parse(partnerMapped.CreatedBy);

            await DbContext.Partner.AddAsync(partnerMapped);
            await DbContext.PartnerHistory.AddAsync(partnerHistory);

            await DbContext.SaveChangesAsync();

            return partnerMapped.ToPartnerRepoModel();
        }
        public async Task<ContactRepoModel> Create_Contact_Async(ContactRepoModel contact)
        {
            var contactMapped = contact.ToContactDbModel();
            var contactHistory = contact.ToContactHistoryModel();

            contactHistory.Action = "Create";
            contactHistory.ActionDate = DateTime.Now;

            await DbContext.Contact.AddAsync(contactMapped);
            await DbContext.ContactHistory.AddAsync(contactHistory);

            await DbContext.SaveChangesAsync();

            return contactMapped.ToContactRepoModel();
        }


        // GET Partners with Guids | Contacts with Guids
        public async Task<PartnerRepoModel> Get_Partner_By_Guid_Async(Guid partnerGuid)
        {
            var partners = await DbContext.Partner.FirstOrDefaultAsync(a => a.PartnerGUID == partnerGuid && !a.IsDeleted);

            // Retorna null ou Resultado suposto
            var partnerList = (partners == null) ? new PartnerRepoModel() : partners.ToPartnerRepoModel();

            return partnerList;
        }

        public async Task<ContactRepoModel> Get_Partner_Contacts_By_Guid_Async(Guid partnerGuid, Guid contactGuid)
        {
            var contact = await DbContext.Contact.FirstOrDefaultAsync(a => a.PartnerGUID == partnerGuid && a.ContactGUID == contactGuid && !a.IsDeleted);

            // Retorna null ou Resultado suposto
            var contactList = (contact == null) ? new ContactRepoModel() : contact.ToContactRepoModel();

            return contactList;
        }

        // GET all Partners | Contacts
        public async Task<(List<PartnerRepoModel>, int count)> Get_All_Partners_Async(int num_page, int pageSize, string? name)
        {
            // num_page começa 1 - 1 = 0 * 20 = 0 primeira row dá skip a 0 dados e take 20 
            var itemsToSkip = (num_page - 1) * pageSize;

            var itemsToTake = pageSize;

            try
            {
                var query = DbContext.Partner.Where(a => !a.IsDeleted);

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));

                }
                query = query
                    .OrderBy(pc => pc.Name)
                    .ThenBy(p => p.CreationDate);

                var count = await query.CountAsync();

                var partnerValidated = query.Skip(itemsToSkip).Take(itemsToTake).ToList();

                return (partnerValidated.Select(p => p.ToPartnerRepoModel()).ToList(), count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (new List<PartnerRepoModel>(), 0);
            }
        }

        public async Task<(List<ContactRepoModel>, int count)> Get_Partner_Contacts_Async(Guid partnerGuid, int num_page, int pageSize, string? name)
        {
            // num_page começa 1 - 1 = 0 * 20 = 0 primeira row dá skip a 0 dados e take 20 
            var itemsToSkip = (num_page - 1) * pageSize;

            var itemsToTake = pageSize;

            try
            {
                var query = DbContext.Contact.Where(a => a.PartnerGUID == partnerGuid && !a.IsDeleted);

                if (!string.IsNullOrEmpty(name))
                {
                    query = query.Where(p => p.Name.ToLower().Contains(name.ToLower()));
                }
                query = query.OrderBy(p => p.Name);

                var count = await query.CountAsync();

                var contactValidated = query.Skip(itemsToSkip).Take(itemsToTake).ToList();

                return (contactValidated.Select(p => p.ToContactRepoModel()).ToList(), count);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return (new List<ContactRepoModel>(), 0);
            }
        }

        // UPDATE Partner
        public async Task<Partner> Update_Partner_Async(PartnerRepoModel partnerModel)
        {
            var partner = await DbContext.Partner.FirstOrDefaultAsync(a => a.PartnerGUID == partnerModel.PartnerGUID && !a.IsDeleted);

            var partnerValidated = (partner == null) ? new Partner() : partner;

            partnerValidated.Name = partnerModel.Name;
            partnerValidated.PhoneNumber = partnerModel.PhoneNumber;
            partnerValidated.Address = partnerModel.Address;
            partnerValidated.Locality = partnerModel.Locality;
            partnerValidated.PostalCode = partnerModel.PostalCode;
            partnerValidated.Country = partnerModel.Country;
            partnerValidated.TaxNumber = partnerModel.TaxNumber;
            partnerValidated.ServiceDescription = partnerModel.ServiceDescription;
            partnerValidated.Observation = partnerModel.Observation;
            partnerValidated.IsDeleted = partnerModel.IsDeleted;
            partnerValidated.DeletedBy = partnerModel.DeletedBy;

            var partnerHistory = partnerValidated.ToPartnerHistoryModel();

            partnerHistory.Action = "Update";
            partnerHistory.ActionDate = DateTime.Now;
            partnerHistory.UserGUID = Guid.Parse(partnerValidated.CreatedBy);

            DbContext.Partner.Update(partnerValidated);


            await DbContext.PartnerHistory.AddAsync(partnerHistory);

            await DbContext.SaveChangesAsync();

            return partnerValidated;
        }


        //UPDATE Contact
        public async Task<Contact> Update_Contact_Async(ContactRepoModel contactRepoModel)
        {
            var contact = await DbContext.Contact.FirstOrDefaultAsync(a => a.ContactGUID == contactRepoModel.ContactGUID && !a.IsDeleted);

            var contactValidated = (contact == null) ? new Contact() : contact;

            contactValidated.Name = contactRepoModel.Name;
            contactValidated.Email = contactRepoModel.Email;
            contactValidated.Role = contactRepoModel.Role;
            contactValidated.PhoneNumber = contactRepoModel.PhoneNumber;
            contactValidated.Department = contactRepoModel.Department;
            contactValidated.Observation = contactRepoModel.Observation;
            contactValidated.IsDeleted = contactRepoModel.IsDeleted;
            contactValidated.DeletedBy = contactRepoModel.DeletedBy;

            var contactHistory = contactValidated.ToContactHistory_Model();

            contactHistory.Action = "Update";
            contactHistory.ActionDate = DateTime.Now;

            await DbContext.ContactHistory.AddAsync(contactHistory);

            DbContext.Contact.Update(contactValidated);

            await DbContext.SaveChangesAsync();

            return contactValidated;
        }

        //DELETE 
        public async Task<bool> Delete_Partner_Async(Guid partnerGuid)
        {
            var partner = await DbContext.Partner.FirstOrDefaultAsync(a => a.PartnerGUID == partnerGuid);

            if (partner == null)
            {
                return false;
            }
            partner.DeletedBy = Guid.Parse(partner.CreatedBy);
            partner.IsDeleted = true;

            DbContext.Partner.Update(partner);

            var partnerHistory = partner.ToPartnerHistoryModel();

            partnerHistory.Action = "Delete";
            partnerHistory.ActionDate = DateTime.Now;
            partnerHistory.UserGUID = Guid.Parse(partner.CreatedBy);

            await DbContext.PartnerHistory.AddAsync(partnerHistory);

            await DbContext.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Delete_Contact_Async(Guid partnerGuid, Guid contactGuid)
        {
            var contact = await DbContext.Contact.FirstOrDefaultAsync(a => a.ContactGUID == contactGuid && a.PartnerGUID == partnerGuid);

            if (contact == null)
            {
                return false;
            }
            contact.DeletedBy = contact.UserGUID;
            contact.IsDeleted = true;

            DbContext.Contact.Update(contact);

            var contactHistory = contact.ToContactHistory_Model();

            contactHistory.Action = "Delete";
            contactHistory.ActionDate = DateTime.Now;

            await DbContext.ContactHistory.AddAsync(contactHistory);

            await DbContext.SaveChangesAsync();

            return true;
        }
        public async Task<bool> Delete_All_Contacts_By_PartnerGuid_Async(Guid partnerGuid)
        {
            var contacts = await DbContext.Contact.Where(a => a.PartnerGUID == partnerGuid).ToListAsync();

            if (contacts == null || !contacts.Any())
            {
                return false;
            }

            foreach (var contact in contacts)
            {
                contact.IsDeleted = true;
            }

            var contactsHistory = contacts.Select(a => a.ToContactHistory_Model()).ToList();

            contactsHistory.ForEach(a =>
            {
                a.Action = "Delete";
                a.ActionDate = DateTime.Now;
            });

            foreach (var contactHistory in contactsHistory)
            {
                await DbContext.ContactHistory.AddAsync(contactHistory);
            }

            await DbContext.SaveChangesAsync();

            return true;
        }

        //**************************************************
        //*                HISTORY METHODS                 *
        //**************************************************

        public async Task<(List<PartnerHistoryRepoModel>, int count)> Get_All_Partners_History_Async(Guid partnerGuid, int num_page, int pageSize)
        {
            // num_page começa 1 - 1 = 0 * 10 = 0 primeira row dá skip a 0 dados e take 20 
            var itemsToSkip = (num_page - 1) * pageSize;

            var itemsToTake = pageSize;

            var query = DbContext.PartnerHistory.Where(p => p.PartnerGUID == partnerGuid);

            query = query.OrderBy(p => p.Name).
                        ThenBy(p => p.ActionDate);

            var count = await query.CountAsync();

            var partnerHistoryValidated = query.Skip(itemsToSkip).Take(itemsToTake).ToList();

            return (partnerHistoryValidated.Select(a => a.ToPartnerHistory_RepoModel()).ToList(), count);
        }

        public async Task<(List<ContactHistoryRepoModel>, int count)> Get_All_Contacts_History_Async(Guid partnerGuid, Guid contactGuid, int num_page, int pageSize)
        {
            // num_page começa 1 - 1 = 0 * 10 = 0 primeira row dá skip a 0 dados e take 20 
            var itemsToSkip = (num_page - 1) * pageSize;

            var itemsToTake = pageSize;

            var query = DbContext.ContactHistory.
                        Where(p => p.PartnerGUID == partnerGuid && p.ContactGUID == contactGuid);

            query = query.OrderBy(p => p.Name).ThenBy(p => p.ActionDate);

            var count = await query.CountAsync();

            var contactHistoryValidated = query.Skip(itemsToSkip).Take(itemsToTake).ToList();

            return (contactHistoryValidated.Select(a => a.ToContactHistoryRepoModel()).ToList(), count);
        }
    }
}
