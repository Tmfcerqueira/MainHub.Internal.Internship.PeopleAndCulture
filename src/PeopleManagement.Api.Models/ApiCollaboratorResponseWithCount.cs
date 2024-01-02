using MainHub.Internal.PeopleAndCulture;

namespace PeopleManagement.Api.Models
{
    public class ApiCollaboratorResponseWithCount
    {
        public List<ApiCollaboratorAllResponseModel> Collaborators { get; set; } = new List<ApiCollaboratorAllResponseModel>();

        public int TotalCount { get; set; }
    }
}
