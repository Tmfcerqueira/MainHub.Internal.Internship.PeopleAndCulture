using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TimesheetManagement.DataBase;

namespace MainHub.Internal.PeopleAndCulture
{
    public class TimesheetManagementDbContextTest : TimesheetManagementDbContext
    {
        public TimesheetManagementDbContextTest(DbContextOptions<TimesheetManagementDbContext> options)
            : base(options)
        {

        }
    }
}
