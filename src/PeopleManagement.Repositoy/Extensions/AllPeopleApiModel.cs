using PeopleManagementDataBase;
using PeopleManagementRepository.Models;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class AllPeopleApiModel
    {
        public static ApiCollaboratorAllResponseModel ToAllApiResponseModel(this AllPeopleRepoModel model)
        {
            return new ApiCollaboratorAllResponseModel
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                BirthDate = model.BirthDate,
                EntryDate = model.EntryDate,
                ExitDate = model.ExitDate,
                PeopleGUID = model.PeopleGUID,
                Status = model.Status,
                Contact = model.Contact,
                EmergencyContact = model.EmergencyContact,
            };
        }
    }
}
