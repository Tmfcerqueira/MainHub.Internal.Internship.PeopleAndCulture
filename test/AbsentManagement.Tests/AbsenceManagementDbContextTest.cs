using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using Microsoft.EntityFrameworkCore;

namespace MainHub.Internal.PeopleAndCulture.Tests.AbsentManagementTests
{

    public class AbsenceManagementDbContextTest : AbsenceManagementDbContext
    {
        public AbsenceManagementDbContextTest(DbContextOptions<AbsenceManagementDbContext> options)
            : base(options)
        {

        }
    }
}
