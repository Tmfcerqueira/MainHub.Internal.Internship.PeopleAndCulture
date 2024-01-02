using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Controllers;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using TimesheetManagement.DataBase;
using TimesheetManagement.DataBase.Models;

namespace MainHub.Internal.PeopleAndCulture
{
    public class TimesheetActivityControllerTest
    {
        //Constructor test
        [Fact]
        public void Constructor_WithITimesheetRepository_SetsTimesheetRepositoryProperty()
        {
            //Arrange
            var mockRepository = new Mock<ITimesheetRepository>();
            var controller = new TimesheetActivityController(mockRepository.Object);

            //Act & Assert
            Assert.Equal(mockRepository.Object, controller.TimesheetActivityRepository);
        }

        //Creates
        [Fact]
        public async Task CreateTimesheetActivity_Returns_Ok_With_TimesheetActivityCreateResponseModel()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            var personGuid = Guid.NewGuid();

            using var dbContext = new TimesheetManagementDbContext(options);

            var timesheetActivityRepositoryMock = new Mock<ITimesheetRepository>();

            var model = new TimesheetActivityCreateRequestModel
            {
                ActivityGUID = Guid.NewGuid(),
                TimesheetGUID = Guid.NewGuid(),
                ProjectGUID = Guid.NewGuid(),
                ActivityDate = new DateTime(2999 - 12 - 31),
                TypeOfWork = Common.TypeOfWork.Regular,
                Hours = 0
            };

            var repoModel = model.ToTimesheetActivityRepoModel();
            timesheetActivityRepositoryMock.Setup(x => x.CreateTimesheetActivity(It.IsAny<TimesheetActivityRepoModel>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(new TimesheetActivityRepoModel { TimesheetActivityGUID = Guid.Empty }));

            var controller = new TimesheetActivityController(timesheetActivityRepositoryMock.Object);

            //Act
            var result = await controller.CreateTimesheetActivity(personGuid, model.TimesheetGUID, model, It.IsAny<Guid>());

            //Assert
            result.ShouldBeOfType<ActionResult<TimesheetActivityCreateResponseModel>>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateTimesheetActivity_Returns_BadRequest_When_TimesheetGUID_Is_Not_The_Same_As_Model_TimesheetGuid()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
            .UseInMemoryDatabase(databaseName: "test_db")
            .Options;
            using var dbContext = new TimesheetManagementDbContext(options);

            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();

            var timesheetActivityRepositoryMock = new Mock<ITimesheetRepository>();

            var model = new TimesheetActivityCreateRequestModel
            {
                ActivityGUID = Guid.NewGuid(),
                TimesheetGUID = Guid.NewGuid(),
                ProjectGUID = Guid.NewGuid(),
                ActivityDate = new DateTime(2999 - 12 - 31),
                TypeOfWork = Common.TypeOfWork.Regular,
                Hours = 0
            };

            var controller = new TimesheetActivityController(timesheetActivityRepositoryMock.Object);

            //Act
            var result = await controller.CreateTimesheetActivity(personGuid, timesheetGuid, model, It.IsAny<Guid>());


            //Assert
            result.Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        //Gets
        [Fact]
        public async Task GetTimesheetActivitiesForTimesheet_ReturnsOk_WhenTimesheetActivitiesFound()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();

            var timesheetActivityRepoModels = new List<TimesheetActivityRepoModel>
             {
                new TimesheetActivityRepoModel (),
                new TimesheetActivityRepoModel ()
            };
            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();

            timesheetActivityRepoMock.Setup(repo => repo.GetTimesheetActivitiesForTimesheet(timesheetGuid))
                             .ReturnsAsync(timesheetActivityRepoModels);

            var totalCount = timesheetActivityRepoModels.Count;
            timesheetActivityRepoMock.Setup(repo => repo.GetTimesheetActivitiesForTimesheet(timesheetGuid))
                           .ReturnsAsync(timesheetActivityRepoModels);

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.GetAllTimesheetActivities(personGuid, timesheetGuid);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var responseModels = Assert.IsType<TimesheetActivityResponseModels>(okResult.Value);
            Assert.Equal(timesheetActivityRepoModels.Count, responseModels.Activities.Count);
            Assert.Equal(totalCount, responseModels.AllDataCount);
        }

        [Fact]
        public async Task GetAllTimesheetActivitiesFromTimesheet_ReturnsNotFound_When_0()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();
            timesheetActivityRepoMock.Setup(repo => repo.GetTimesheetActivitiesForTimesheet(timesheetGuid))
                        .ReturnsAsync(new List<TimesheetActivityRepoModel>());

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.GetAllTimesheetActivities(personGuid, timesheetGuid);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task GetOneTimesheetActivityFromOneTimesheet_ReturnsOk_WhenTimesheetActivityFound()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();
            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();
            var timesheetActivity = new TimesheetActivityRepoModel() { TimesheetGUID = timesheetGuid, TimesheetActivityGUID = timesheetActivityGuid };
            timesheetActivityRepoMock.Setup(repo => repo.GetOneTimesheetActivityForTimesheet(timesheetGuid, timesheetActivityGuid)).ReturnsAsync(timesheetActivity);

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.GetTimesheetActivityForTimesheet(personGuid, timesheetGuid, timesheetActivityGuid);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            var model = Assert.IsType<TimesheetActivityResponseModel>(okObjectResult.Value);
        }

        [Fact]
        public async Task GetOneTimesheetActivityFromOneTimesheet_ReturnsNotFound_When_Null()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();
            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();
            timesheetActivityRepoMock.Setup(repo => repo.GetOneTimesheetActivityForTimesheet(timesheetGuid, timesheetActivityGuid));

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.GetTimesheetActivityForTimesheet(personGuid, timesheetGuid, timesheetActivityGuid);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }


        //Deletes
        [Fact]
        public async Task DeleteTimesheetActivity_ReturnsNoContent_WhenTimesheetActivityDeleted()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();
            var timesheetActivity = new TimesheetActivityRepoModel() { TimesheetGUID = timesheetGuid, TimesheetActivityGUID = timesheetActivityGuid };
            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();
            timesheetActivityRepoMock.Setup(repo => repo.DeleteTimesheetActivity(It.IsAny<Guid>(), It.IsAny<Guid>()))
                           .ReturnsAsync(true);

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.DeleteTimesheetActivity(personGuid, timesheetGuid, timesheetActivityGuid, Guid.NewGuid());

            //Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);

            timesheetActivityRepoMock.Verify(repo => repo.DeleteTimesheetActivity(timesheetActivityGuid, It.IsAny<Guid>()), Times.Once);
        }

        [Fact]
        public async Task DeleteTimesheetActivity_ReturnsNotFound_WhenTimesheetActivityDoesNotExist()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();
            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();
            timesheetActivityRepoMock.Setup(repo => repo.DeleteTimesheetActivity(timesheetActivityGuid, personGuid))
                           .ReturnsAsync(false);

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.DeleteTimesheetActivity(personGuid, timesheetGuid, timesheetActivityGuid, It.IsAny<Guid>());

            //Assert
            Assert.IsType<NotFoundResult>(result);
        }


        //Updates
        [Fact]
        public async Task UpdateTimesheetActivity_ReturnsNoContent_WhenTimesheetActivityUpdatedSuccessfully()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();
            var updateModel = new TimesheetActivityUpdateModel();
            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();
            var timesheetActivity = new TimesheetActivityRepoModel() { TimesheetActivityGUID = timesheetActivityGuid, TimesheetGUID = timesheetGuid };
            timesheetActivityRepoMock.Setup(repo => repo.GetOneTimesheetActivityForTimesheet(timesheetGuid, timesheetActivityGuid))
                           .ReturnsAsync(timesheetActivity);

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.UpdateTimesheetActivity(personGuid, timesheetGuid, timesheetActivityGuid, updateModel, It.IsAny<Guid>());

            //Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            timesheetActivityRepoMock.Verify(repo => repo.UpdateTimesheetActivity(It.IsAny<TimesheetActivityRepoModel>(), It.IsAny<Guid>()));
        }

        [Fact]
        public async Task UpdateTimesheetActivity_ReturnsNotFound_WhenTimesheetActivityNull()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();
            var updateModel = new TimesheetActivityUpdateModel();
            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();
            var timesheetActivity = new TimesheetActivityRepoModel() { TimesheetActivityGUID = timesheetActivityGuid, TimesheetGUID = timesheetGuid };

            timesheetActivityRepoMock.Setup(repo => repo.GetOneTimesheetActivityForTimesheet(timesheetGuid, timesheetActivityGuid))
                .Returns(Task.FromResult<TimesheetActivityRepoModel>(null));

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.UpdateTimesheetActivity(personGuid, timesheetGuid, timesheetActivityGuid, updateModel, It.IsAny<Guid>());

            //Assert
            timesheetActivityRepoMock.Verify(repo => repo.GetOneTimesheetActivityForTimesheet(timesheetGuid, timesheetActivityGuid), Times.Once);
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            timesheetActivityRepoMock.Verify(repo => repo.UpdateTimesheetActivity(It.IsAny<TimesheetActivityRepoModel>(), It.IsAny<Guid>()), Times.Never);
        }


        //History
        [Fact]
        public async Task GetTimesheetActivityHistory_ReturnsOkResult_WithTimesheetActivityHistoryResponseModels()
        {
            //Arrange
            var timesheetActivityGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();
            var timesheetActivityRepoModels = new List<TimesheetActivityHistoryRepoModel>
            {
                new TimesheetActivityHistoryRepoModel(),
                new TimesheetActivityHistoryRepoModel()
            };
            timesheetActivityRepoMock.Setup(repo => repo.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize))
                           .ReturnsAsync(timesheetActivityRepoModels);

            var totalCount = timesheetActivityRepoModels.Count;
            timesheetActivityRepoMock.Setup(repo => repo.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize))
                           .ReturnsAsync(timesheetActivityRepoModels);

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var responseModels = Assert.IsType<TimesheetActivityHistoryResponseModels>(okResult.Value);
            Assert.Equal(timesheetActivityRepoModels.Count, responseModels.TimesheetActivityHistory.Count);
            Assert.Equal(totalCount, responseModels.AllDataCount);

            timesheetActivityRepoMock.Verify(repo => repo.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize), Times.Once);
            timesheetActivityRepoMock.Verify(repo => repo.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize), Times.Once);
        }

        [Fact]
        public async Task GetTimesheetActivityHistoryByPerson_ReturnsNotFound_WhenNoTimesheetActivitiesFound()
        {
            //Arrange
            var timesheetActivityGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var timesheetActivityRepoMock = new Mock<ITimesheetRepository>();
            timesheetActivityRepoMock.Setup(repo => repo.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize))
                           .ReturnsAsync(new List<TimesheetActivityHistoryRepoModel>());

            var controller = new TimesheetActivityController(timesheetActivityRepoMock.Object);

            //Act
            var result = await controller.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            timesheetActivityRepoMock.Verify(repo => repo.GetTimesheetActivityHistory(timesheetGuid, timesheetActivityGuid, page, pageSize), Times.Once);
        }
    }
}
