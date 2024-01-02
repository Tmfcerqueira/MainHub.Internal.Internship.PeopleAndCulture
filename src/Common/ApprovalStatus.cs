using System.ComponentModel.DataAnnotations;

namespace MainHub.Internal.PeopleAndCulture.Common
{
    public enum ApprovalStatus
    {
        [Display(Description = "Draft")]
        Draft = 1,
        [Display(Description = "Submitted")]
        Submitted,
        [Display(Description = "Approved")]
        Approved,
        [Display(Description = "Rejected")]
        Rejected,
        All
    }

}
