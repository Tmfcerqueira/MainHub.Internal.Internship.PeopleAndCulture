using Microsoft.EntityFrameworkCore;
using PeopleManagementRepository.Data;

namespace PeopleManagement.Tests
{
    public class PeopleManagementDbContextTest : PeopleManagementDbContext
    {
        public PeopleManagementDbContextTest(DbContextOptions<PeopleManagementDbContext> options) : base(options)
        { }
    }
}
