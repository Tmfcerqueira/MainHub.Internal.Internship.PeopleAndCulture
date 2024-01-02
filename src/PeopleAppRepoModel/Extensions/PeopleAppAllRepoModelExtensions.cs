using App.Models;
using Microsoft.Graph;
using PeopleManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture.Extensions
{
    public static class PeopleAppAllRepoModelExtensions
    {
        public static AllPeopleModel ToAllPeopleModel(this PeopleManagement.Api.Proxy.Client.Model.ApiCollaboratorAllResponseModel model)
        {
            return new AllPeopleModel
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
        public static AllPeopleModel ToAllPeopleModel(this User model)
        {
            return new AllPeopleModel
            {
                FirstName = model.GivenName,
                LastName = model.Surname,
                Email = model.Mail,
                BirthDate = model.Birthday != null ? model.Birthday.Value.DateTime : DateTime.MinValue,
                EntryDate = model.EmployeeHireDate != null ? model.EmployeeHireDate.Value.DateTime : DateTime.MinValue,
                ExitDate = DateTime.MinValue,
                PeopleGUID = Guid.Parse(model.Id),
            };
        }
    }
}
