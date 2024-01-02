using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Controllers;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Repository;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using Xunit;
using TimesheetManagement.DataBase;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using TimesheetManagement.DataBase.Models;
using PeopleManagementDataBase;
using MainHub.Internal.PeopleAndCulture.Models;
using MainHub.Internal.PeopleAndCulture;
using TimesheetManagement.Repository.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TimesheetManagement.Api.Proxy.Client.Api;

namespace TimesheetManagement.Tests
{
    public class TimesheetRepositoryTest
    {
        [Fact]
        public void Constructor_Should_Inject_TimhesheetManagementDbContext()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Act
            //Instance of TimesheetRepository with the mock context as a parameter
            var repository = new TimesheetRepository(dbContext);

            //Assert
            //Verify that the repository instance is not null and that its _dbContext property is set to the mock context object
            repository.ShouldNotBeNull();
            repository.DbContext.ShouldBe(dbContext);
        }


        //Creates
        [Fact]
        public async Task CreateTimesheet_Should_Create_Timesheet_With_Correct_Properties()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);
            var repository = new TimesheetRepository(dbContext);

            var timesheetRepoModel = new TimesheetRepoModel
            {
                TimesheetGUID = Guid.Empty,
                PersonGUID = Guid.Empty,
                Month = 0,
                Year = 0,
                ApprovedBy = "None",
                ApprovalStatus = ApprovalStatus.Draft,
                DateOfSubmission = new DateTime(2999, 12, 31, 23, 59, 59),
                DateOfApproval = new DateTime(2999, 12, 31, 23, 59, 59)
            };

            //Act
            var createdTimesheet = await repository.CreateTimesheet(timesheetRepoModel, It.IsAny<Guid>());

            //Assert
            createdTimesheet.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateTimesheetActivity_Should_Create_TimesheetActivity_With_Correct_Properties()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);
            var repository = new TimesheetRepository(dbContext);

            var timesheetActivityRepoModel = new TimesheetActivityRepoModel
            {
                TimesheetActivityGUID = Guid.Empty,
                ActivityGUID = Guid.Empty,
                TimesheetGUID = Guid.Empty,
                ProjectGUID = Guid.Empty,
                TypeOfWork = TypeOfWork.Regular,
                ActivityDate = new DateTime(2999, 12, 31, 23, 59, 59),
                Hours = 0
            };

            //Act
            var createdTimesheetActivity = await repository.CreateTimesheetActivity(timesheetActivityRepoModel, It.IsAny<Guid>());

            //Assert
            createdTimesheetActivity.ShouldNotBeNull();
            // assert other properties as needed
        }


        //Gets
        [Fact]
        public async Task GetTimesheetsForPerson_ReturnsExpectedTimesheets()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
               .UseInMemoryDatabase(databaseName: "test_db")
               .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var personGuid = Guid.NewGuid();
            dbContext.Timesheet.AddRange(
                new Timesheet(),
                new Timesheet()
            );

            await dbContext.SaveChangesAsync();

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.GetTimesheetsForPerson(personGuid, DateTime.Now.Year, DateTime.Now.Month, ApprovalStatus.Draft, 1, 20);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetTimesheetsForPerson_ReturnsNotFound_WhenNoTimesheetsFound()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            int year = 2023;
            int month = 1;
            var page = 1;
            var pageSize = 10;
            var status = ApprovalStatus.All;

            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            timesheetRepoMock.Setup(repo => repo.GetTimesheetsForPerson(personGuid, year, month, status, page, pageSize))
                           .ReturnsAsync((new List<TimesheetRepoModel>(), 2));

            var controller = new TimesheetController(timesheetRepoMock.Object);

            //Act
            var result = await controller.GetAllTimesheetsForPerson(personGuid, year, month, status, page, pageSize);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            timesheetRepoMock.Verify(repo => repo.GetTimesheetsForPerson(personGuid, year, month, status, page, pageSize), Times.Once);
        }

        [Fact]
        public async Task GetOneTimesheetForPerson_ReturnsExpectedTimesheet()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            dbContext.Timesheet.Add(new Timesheet { PersonGUID = personGuid, TimesheetGUID = timesheetGuid });
            await dbContext.SaveChangesAsync();

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.GetOneTimesheetForPerson(personGuid, timesheetGuid);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(timesheetGuid, result.TimesheetGUID);
            Assert.Equal(personGuid, result.PersonGUID);
        }

        [Fact]
        public async Task GetOneTimesheetForPerson_Returns_Null_When_Timesheet_Null()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.GetOneTimesheetForPerson(personGuid, timesheetGuid);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetTimesheetActivitiesForTimesheet_ReturnsExpectedTimesheetActivities()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
               .UseInMemoryDatabase(databaseName: "test_db")
               .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            dbContext.TimesheetActivity.AddRange(
                new TimesheetActivity(),
                new TimesheetActivity()
            );

            await dbContext.SaveChangesAsync();

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.GetTimesheetActivitiesForTimesheet(timesheetGuid);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetOneTimesheetActivityForTimesheet_ReturnsExpectedTimesheetActivity()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();
            dbContext.TimesheetActivity.Add(new TimesheetActivity { TimesheetGUID = timesheetGuid, TimesheetActivityGUID = timesheetActivityGuid });
            await dbContext.SaveChangesAsync();

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.GetOneTimesheetActivityForTimesheet(timesheetGuid, timesheetActivityGuid);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(timesheetGuid, result.TimesheetGUID);
            Assert.Equal(timesheetActivityGuid, result.TimesheetActivityGUID);
        }

        [Fact]
        public async Task GetOneTimesheetActivityForTimesheet_Returns_Null_When_Timesheet_Null()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.GetOneTimesheetActivityForTimesheet(timesheetGuid, timesheetActivityGuid);

            //Assert
            Assert.Null(result);
        }


        //Deletes
        [Fact]
        public async Task DeleteTimesheetActivity_ReturnsTrue_When_TimesheetActivityExists()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var timesheetActivityGuid = Guid.NewGuid();
            dbContext.TimesheetActivity.Add(new TimesheetActivity { TimesheetActivityGUID = timesheetActivityGuid });
            await dbContext.SaveChangesAsync();

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.DeleteTimesheetActivity(timesheetActivityGuid, It.IsAny<Guid>());

            //Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteTimesheetActivity_ReturnsFalse_When_TimesheetActivityDoesNotExist()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.DeleteTimesheetActivity(Guid.NewGuid(), It.IsAny<Guid>());

            //Assert
            Assert.False(result);
        }


        //Updates
        [Fact]
        public async Task UpdateTimesheet_ReturnsTrue_When_TimesheetExistsAndIsUpdated()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
            .UseInMemoryDatabase(databaseName: "test_db")
            .Options;
            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var timesheetGuid = Guid.NewGuid();
            var personGuid = Guid.NewGuid();
            var timesheet = new Timesheet { TimesheetGUID = Guid.NewGuid() };
            dbContext.Timesheet.Add(timesheet);
            await dbContext.SaveChangesAsync();
            dbContext.Timesheet.Add(new Timesheet
            {
                TimesheetGUID = timesheetGuid,
                PersonGUID = personGuid,
                Year = 2023,
                Month = 1,
                ApprovalStatus = ApprovalStatus.Draft,
                DateOfSubmission = new DateTime(2999, 12, 31, 23, 59, 59),
                DateOfApproval = new DateTime(2999, 12, 31, 23, 59, 59),
                ApprovedBy = "None"
            });
            await dbContext.SaveChangesAsync();

            var updatedTimesheet = new TimesheetRepoModel
            {
                TimesheetGUID = timesheetGuid,
                PersonGUID = personGuid,
                Year = 2023,
                Month = 1,
                ApprovalStatus = ApprovalStatus.Submitted,
                DateOfSubmission = DateTime.Now,
                DateOfApproval = new DateTime(2999, 12, 31, 23, 59, 59),
                ApprovedBy = "None"
            };

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.UpdateTimesheet(updatedTimesheet, It.IsAny<Guid>());

            //Assert
            Assert.True(result);

            var updatedTimesheetEntity = await dbContext.Timesheet.FirstOrDefaultAsync(a => a.TimesheetGUID == timesheetGuid);
            Assert.NotNull(updatedTimesheetEntity);
            Assert.Equal(updatedTimesheet.TimesheetGUID, updatedTimesheetEntity.TimesheetGUID);
            Assert.Equal(updatedTimesheet.PersonGUID, updatedTimesheetEntity.PersonGUID);
            Assert.Equal(updatedTimesheet.Year, updatedTimesheetEntity.Year);
            Assert.Equal(updatedTimesheet.Month, updatedTimesheetEntity.Month);
            Assert.Equal(updatedTimesheet.ApprovalStatus, updatedTimesheetEntity.ApprovalStatus);
            Assert.NotEqual(updatedTimesheet.DateOfSubmission, updatedTimesheetEntity.DateOfSubmission);
            Assert.Equal(updatedTimesheet.DateOfApproval, updatedTimesheetEntity.DateOfApproval);
            Assert.Equal(updatedTimesheet.ApprovedBy, updatedTimesheetEntity.ApprovedBy);
        }

        [Fact]
        public async Task UpdateTimesheet_ReturnsFalse_When_TimesheetDoesNotExist()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data

            var updatedTimesheet = new TimesheetRepoModel
            {
                TimesheetGUID = Guid.NewGuid(),
                PersonGUID = Guid.NewGuid(),
                Year = 2023,
                Month = 1,
                ApprovalStatus = ApprovalStatus.Submitted,
                DateOfSubmission = DateTime.Now,
                DateOfApproval = new DateTime(2999, 12, 31, 23, 59, 59),
                ApprovedBy = "None"
            };

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.UpdateTimesheet(updatedTimesheet, It.IsAny<Guid>());

            //Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateTimesheetActivity_ReturnsTrue_When_TimesheetActivityExistsAndIsUpdated()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
            .UseInMemoryDatabase(databaseName: "test_db")
            .Options;
            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var timesheetActivityGuid = Guid.NewGuid();
            var activityGuid = Guid.NewGuid();
            var projectGuid = Guid.NewGuid();
            var timesheet = new Timesheet { TimesheetGUID = Guid.NewGuid() };
            dbContext.Timesheet.Add(timesheet);
            await dbContext.SaveChangesAsync();
            dbContext.TimesheetActivity.Add(new TimesheetActivity
            {
                TimesheetActivityGUID = timesheetActivityGuid,
                ActivityGUID = activityGuid,
                ProjectGUID = projectGuid,
                TimesheetGUID = timesheet.TimesheetGUID,
                TypeOfWork = TypeOfWork.Regular,
                ActivityDate = new DateTime(2999, 12, 31, 23, 59, 59),
                Hours = 8
            });
            await dbContext.SaveChangesAsync();

            var updatedTimesheetActivity = new TimesheetActivityRepoModel
            {
                TimesheetActivityGUID = timesheetActivityGuid,
                ActivityGUID = activityGuid,
                ProjectGUID = projectGuid,
                TimesheetGUID = timesheet.TimesheetGUID,
                TypeOfWork = TypeOfWork.Regular,
                ActivityDate = DateTime.Now,
                Hours = 4
            };

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.UpdateTimesheetActivity(updatedTimesheetActivity, It.IsAny<Guid>());

            //Assert
            Assert.True(result);

            var updatedTimesheetActivityEntity = await dbContext.TimesheetActivity.FirstOrDefaultAsync(a => a.TimesheetActivityGUID == timesheetActivityGuid);
            Assert.NotNull(updatedTimesheetActivityEntity);
            Assert.Equal(updatedTimesheetActivity.ActivityGUID, updatedTimesheetActivityEntity.ActivityGUID);
            Assert.Equal(updatedTimesheetActivity.ProjectGUID, updatedTimesheetActivityEntity.ProjectGUID);
            Assert.Equal(updatedTimesheetActivity.TimesheetGUID, updatedTimesheetActivityEntity.TimesheetGUID);
            Assert.Equal(updatedTimesheetActivity.TypeOfWork, updatedTimesheetActivityEntity.TypeOfWork);
            Assert.Equal(updatedTimesheetActivity.ActivityDate, updatedTimesheetActivityEntity.ActivityDate);
            Assert.Equal(updatedTimesheetActivity.Hours, updatedTimesheetActivityEntity.Hours);
        }

        [Fact]
        public async Task UpdateTimesheetActivity_ReturnsFalse_When_TimesheetActivityDoesNotExist()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            //Add test data
            var timesheet = new Timesheet { TimesheetGUID = Guid.NewGuid() };
            dbContext.Timesheet.Add(timesheet);
            await dbContext.SaveChangesAsync();

            var updatedTimesheetActivity = new TimesheetActivityRepoModel
            {
                TimesheetActivityGUID = Guid.NewGuid(),
                ActivityGUID = Guid.NewGuid(),
                ProjectGUID = Guid.NewGuid(),
                TimesheetGUID = timesheet.TimesheetGUID,
                TypeOfWork = TypeOfWork.Regular,
                ActivityDate = DateTime.Now,
                Hours = 4
            };

            //Act
            var repo = new TimesheetRepository(dbContext);
            var result = await repo.UpdateTimesheetActivity(updatedTimesheetActivity, It.IsAny<Guid>());

            //Assert
            Assert.False(result);
        }


        //Admin
        [Fact]
        public async Task GetAllTimesheets_ReturnsListOfTimesheetRepoModel()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;
            var status = ApprovalStatus.Approved;

            var timesheets = new List<Timesheet>
            {
                new Timesheet { IsDeleted = false, ApprovalStatus = status, TimesheetGUID = new Guid()},
                new Timesheet { IsDeleted = false, ApprovalStatus = status, TimesheetGUID =  new Guid() },
                new Timesheet { IsDeleted = false, ApprovalStatus = status, TimesheetGUID =  new Guid() }
            };

            var expectedTimesheetRepoModels = timesheets.Select(a => a.ToTimesheetRepoModel()).ToList();

            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using (var dbContext = new TimesheetManagementDbContext(options))
            {
                dbContext.Timesheet.AddRange(timesheets);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new TimesheetManagementDbContext(options))
            {
                var repository = new TimesheetRepository(dbContext);

                //Act
                var result = await repository.GetAllTimesheets(page, pageSize, status);

                //Assert
                Assert.NotNull(result);
            }
        }


        //History
        [Fact]
        public async Task GetTimesheetsHistory_ReturnsCorrectTimesheets()
        {
            //Arrange
            var timesheetGuid = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using (var dbContext = new TimesheetManagementDbContext(options))
            {
                //Add test data to the in-memory database
                var timesheetHistories = new List<TimesheetHistory>
            {
                new TimesheetHistory
                {
                    TimesheetGUID = timesheetGuid,
                    PersonGUID = Guid.NewGuid(),
                },
            };
                await dbContext.TimesheetHistory.AddRangeAsync(timesheetHistories);
                await dbContext.SaveChangesAsync();

                var repository = new TimesheetRepository(dbContext);

                //Act
                var result = await repository.GetTimesheetHistory(timesheetGuid, page, pageSize);

                //Assert
                Assert.NotNull(result);
                Assert.IsType<List<TimesheetHistoryRepoModel>>(result.timesheets);
            }
        }

        [Fact]
        public async Task GetTimesheetActivitiesHistory_ReturnsCorrectTimesheetActivities()
        {
            //Arrange
            var timesheetActivityGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using (var dbContext = new TimesheetManagementDbContext(options))
            {
                //Add test data to the in-memory database
                var timesheetActivityHistories = new List<TimesheetActivityHistory>
            {
                new TimesheetActivityHistory
                {
                    TimesheetActivityGUID = timesheetActivityGuid,
                    TimesheetGUID = timesheetGuid,
                },
            };
                await dbContext.TimesheetActivityHistory.AddRangeAsync(timesheetActivityHistories);
                await dbContext.SaveChangesAsync();

                var repository = new TimesheetRepository(dbContext);

                //Act
                var result = await repository.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize);

                //Assert
                Assert.NotNull(result);
                Assert.IsType<List<TimesheetActivityHistoryRepoModel>>(result);
            }
        }
    }
}
