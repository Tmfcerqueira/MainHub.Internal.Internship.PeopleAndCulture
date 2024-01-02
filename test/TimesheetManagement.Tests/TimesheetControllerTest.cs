using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Controllers;
using TimesheetManagement.DataBase;
using MainHub.Internal.PeopleAndCulture.Properties;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions;
using TimesheetManagement.DataBase.Models;
using Proxy = TimesheetManagement.Api.Proxy.Client.Model;
using Microsoft.AspNetCore.Http;
using System.Drawing.Printing;
using System.Drawing;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;

namespace MainHub.Internal.PeopleAndCulture
{
    public class TimesheetControllerTest
    {
        //Constructor test
        [Fact]
        public void Constructor_WithITimesheetRepository_SetsTimesheetRepositoryProperty()
        {
            //Arrange
            var mockRepository = new Mock<ITimesheetRepository>();
            var controller = new TimesheetController(mockRepository.Object);

            //Act & Assert
            Assert.Equal(mockRepository.Object, controller.TimesheetRepository);
        }


        //Creates
        [Fact]
        public async Task CreateTimesheet_Returns_Ok_With_TimesheetCreateResponseModel()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new TimesheetManagementDbContext(options);

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();

            var model = new Properties.TimesheetCreateRequestModel
            {
                PersonGUID = Guid.Empty,
                Month = 0,
                Year = 0
            };

            var repoModel = model.ToTimesheetRepoModel();
            timesheetRepositoryMock.Setup(x => x.CreateTimesheet(It.IsAny<TimesheetRepoModel>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(new TimesheetRepoModel { TimesheetGUID = Guid.Empty }));

            var controller = new TimesheetController(timesheetRepositoryMock.Object);

            //Act
            var result = await controller.CreateTimesheet(model.PersonGUID, model, It.IsAny<Guid>());

            //Assert
            result.ShouldBeOfType<ActionResult<TimesheetManagement.API.Models.TimesheetCreateResponseModel>>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateTimesheet_Returns_BadRequest_When_PersonGUID_Is_Not_The_Same_As_Model_PersonGuid()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<TimesheetManagementDbContext>()
            .UseInMemoryDatabase(databaseName: "test_db")
            .Options;
            using var dbContext = new TimesheetManagementDbContext(options);

            var timesheetRepositoryMock = new Mock<ITimesheetRepository>();

            var model = new Properties.TimesheetCreateRequestModel
            {
                PersonGUID = Guid.Empty,
                Month = 0,
                Year = 0
            };

            var controller = new TimesheetController(timesheetRepositoryMock.Object);

            //Act
            var result = await controller.CreateTimesheet(Guid.NewGuid(), model, It.IsAny<Guid>());

            //Assert
            result.Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        //Gets
        [Fact]
        public async Task GetAllTimesheetsForPerson_ReturnsOk_WhenTimesheetsFound()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var year = 0;
            var month = 0;
            var status = ApprovalStatus.All;
            var page = -1;
            var pageSize = -1;

            var timesheetRepoMock = new Mock<ITimesheetRepository>();

            var timesheetRepoModels = new List<TimesheetRepoModel>
             {
                new TimesheetRepoModel (),
                new TimesheetRepoModel ()
            };

            timesheetRepoMock.Setup(repo => repo.GetTimesheetsForPerson(personGuid, year, month, status, It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync((timesheetRepoModels, 2));

            var totalCount = timesheetRepoModels.Count;
            timesheetRepoMock.Setup(repo => repo.GetTimesheetsForPerson(personGuid, year, month, status, It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync((new List<TimesheetRepoModel>(timesheetRepoModels), 2));

            var controller = new TimesheetController(timesheetRepoMock.Object);

            //Act
            var result = await controller.GetAllTimesheetsForPerson(personGuid, year, month, status, page, pageSize);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var responseModels = Assert.IsType<TimesheetResponseModels>(okResult.Value);
            Assert.Equal(timesheetRepoModels.Count, responseModels.Timesheets.Count);
            Assert.Equal(totalCount, responseModels.AllDataCount);

            timesheetRepoMock.Verify(repo => repo.GetTimesheetsForPerson(personGuid, year, month, status, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            timesheetRepoMock.Verify(repo => repo.GetTimesheetsForPerson(personGuid, year, month, status, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetAllTimesheetsForPerson_ReturnsNotFound_When_TimesheetsNotFound()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var year = 0;
            var month = 0;
            var approvalStatus = ApprovalStatus.All;
            var page = 1;
            var pageSize = 10;

            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            timesheetRepoMock.Setup(repo => repo.GetTimesheetsForPerson(personGuid, year, month, approvalStatus, page, pageSize))
                .ReturnsAsync((new List<TimesheetRepoModel>(), 2));

            var controller = new TimesheetController(timesheetRepoMock.Object);

            //Act
            var result = await controller.GetAllTimesheetsForPerson(personGuid, year, month, approvalStatus, page, pageSize);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            timesheetRepoMock.Verify(repo => repo.GetTimesheetsForPerson(personGuid, year, month, approvalStatus, page, pageSize), Times.Once);
        }

        [Fact]
        public async Task GetOneTimesheetFromOnePerson_ReturnsOk_WhenTimesheetFound()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            var timesheet = new TimesheetRepoModel() { TimesheetGUID = timesheetGuid, PersonGUID = personGuid };
            timesheetRepoMock.Setup(repo => repo.GetOneTimesheetForPerson(personGuid, timesheetGuid)).ReturnsAsync(timesheet);

            var controller = new TimesheetController(timesheetRepoMock.Object);

            //Act
            var result = await controller.GetTimesheetForPerson(personGuid, timesheetGuid);

            //Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            var model = Assert.IsType<TimesheetResponseModel>(okObjectResult.Value);
        }

        [Fact]
        public async Task GetOneTimesheetFromOnePerson_ReturnsNotFound_When_0()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            timesheetRepoMock.Setup(repo => repo.GetOneTimesheetForPerson(personGuid, timesheetGuid));

            var controller = new TimesheetController(timesheetRepoMock.Object);

            //Act
            var result = await controller.GetTimesheetForPerson(personGuid, timesheetGuid);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
        }

        //History
        [Fact]
        public async Task GetTimesheetHistory_ReturnsOkResult_WithTimesheetHistoryResponseModels()
        {
            //Arrange
            var timesheetGuid = Guid.NewGuid();
            var page = -1;
            var pageSize = -1;

            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            var timesheetRepoModels = new List<TimesheetHistoryRepoModel>
            {
                new TimesheetHistoryRepoModel(),
                new TimesheetHistoryRepoModel()
            };
            timesheetRepoMock.Setup(repo => repo.GetTimesheetHistory(timesheetGuid, It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync((timesheetRepoModels, 2));

            var totalCount = timesheetRepoModels.Count;
            timesheetRepoMock.Setup(repo => repo.GetTimesheetHistory(timesheetGuid, It.IsAny<int>(), It.IsAny<int>()))
                           .ReturnsAsync((timesheetRepoModels, 2));

            var controller = new TimesheetController(timesheetRepoMock.Object);

            //Act
            var result = await controller.GetTimesheetHistory(timesheetGuid, page, pageSize);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var responseModels = Assert.IsType<TimesheetHistoryResponseModels>(okResult.Value);
            Assert.Equal(timesheetRepoModels.Count, responseModels.TimesheetHistory.Count);
            Assert.Equal(totalCount, responseModels.AllDataCount);

            timesheetRepoMock.Verify(repo => repo.GetTimesheetHistory(timesheetGuid, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            timesheetRepoMock.Verify(repo => repo.GetTimesheetHistory(timesheetGuid, It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetTimesheetHistoryByPerson_ReturnsNotFound_WhenNoTimesheetsFound()
        {
            //Arrange
            var timesheetGuid = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            timesheetRepoMock.Setup(repo => repo.GetTimesheetHistory(timesheetGuid, page, pageSize))
                           .ReturnsAsync((new List<TimesheetHistoryRepoModel>(), 0));

            var controller = new TimesheetController(timesheetRepoMock.Object);

            //Act
            var result = await controller.GetTimesheetHistory(timesheetGuid, page, pageSize);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            timesheetRepoMock.Verify(repo => repo.GetTimesheetHistory(timesheetGuid, page, pageSize), Times.Once);
        }


        //Updates
        [Fact]
        public async Task UpdateTimesheet_ReturnsNoContent_WhenTimesheetUpdatedSuccessfully()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var updateModel = new TimesheetUpdateModel();
            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            var timesheet = new TimesheetRepoModel() { TimesheetGUID = timesheetGuid, PersonGUID = personGuid };
            timesheetRepoMock.Setup(repo => repo.GetOneTimesheetForPerson(personGuid, timesheetGuid))
                           .ReturnsAsync(timesheet);

            var controller = new TimesheetController(timesheetRepoMock.Object);

            //Act
            var result = await controller.UpdateTimesheet(personGuid, timesheetGuid, updateModel, It.IsAny<Guid>());

            //Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            timesheetRepoMock.Verify(repo => repo.UpdateTimesheet(It.IsAny<TimesheetRepoModel>(), It.IsAny<Guid>()));
        }

        [Fact]
        public async Task UpdateTimesheet_ReturnsNotFound_WhenTimesheetNull()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var updateModel = new TimesheetUpdateModel();
            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            var timesheet = new TimesheetRepoModel() { TimesheetGUID = timesheetGuid, PersonGUID = personGuid };

            timesheetRepoMock.Setup(repo => repo.GetOneTimesheetForPerson(personGuid, timesheetGuid))
                .Returns(Task.FromResult<TimesheetRepoModel>(null));

            var controller = new TimesheetController(timesheetRepoMock.Object);

            //Act
            var result = await controller.UpdateTimesheet(personGuid, timesheetGuid, updateModel, It.IsAny<Guid>());

            //Assert
            timesheetRepoMock.Verify(repo => repo.GetOneTimesheetForPerson(personGuid, timesheetGuid), Times.Once);
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            timesheetRepoMock.Verify(repo => repo.UpdateTimesheet(It.IsAny<TimesheetRepoModel>(), It.IsAny<Guid>()), Times.Never);
        }
    }
}
