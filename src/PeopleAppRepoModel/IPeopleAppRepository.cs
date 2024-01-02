using App.Models;
using MainHub.Internal.PeopleAndCulture;

namespace App.Repository
{
    public interface IPeopleAppRepository
    {
        Task<PeopleModel> CreateCollaboratorAsync(PeopleModel peopleModel);
        Task<(List<AllPeopleModel>, int count)> GetAllCollaborators(int page, int pageSize, string filter, State list);
        Task<PeopleModel> GetOneCollaborator(Guid peoplemodelId);
        Task<bool> DeleteCollaboratorAsync(Guid personId);
        Task<bool> UpdateCollaboratorAsync(Guid personId, PeopleModel collaborator);
        Task<List<PeopleHistory>> GetCollaboratorHistory(Guid id, int page, int pageSize);
        Task<(List<AllPeopleModel>, int count)> GetAzure(int page, int pageSize, string filter);
        Task<PeopleModel> ImportAzure(Guid id, AllPeopleModel allPeopleModel);
    }
    public enum State
    {
        All = 1,
        Inactive = 2,
        Active = 3
    }
}

