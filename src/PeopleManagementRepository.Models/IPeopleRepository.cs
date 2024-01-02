using MainHub.Internal.PeopleAndCulture;

namespace PeopleManagementRepository.Models
{
    public interface IPeopleRepository
    {
        Task<PeopleRepoModel> CreateCollaboratorAsync(PeopleRepoModel collaborator, Guid userID);
        Task<List<AllPeopleRepoModel>> GetAllCollaborators(int page, int pageSize, string filter, State? list);
        Task<PeopleRepoModel?> GetOneCollaborator(Guid personId);
        Task<List<PeopleHistoryRepoModel>> GetCollaboratorHistory(Guid id, int page, int pageSize);
        Task<bool> DeleteCollaboratorAsync(Guid personId, Guid id);
        Task<bool> UpdateCollaboratorAsync(Guid personID, PeopleRepoModel updatedCollaborator, Guid userId);
    }
    public enum State
    {
        All = 1,
        Inactive = 2,
        Active = 3
    }
    public enum Contract
    {
        Certo,
        Incerto,
        NoTerm,
        Curta,
        Parcial,
    }
}
