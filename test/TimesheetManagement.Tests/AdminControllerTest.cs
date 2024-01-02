using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.Common;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Controllers;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace MainHub.Internal.PeopleAndCulture
{
    public class AdminControllerTest
    {
        [Fact]
        public void Constructor_WithITimesheetRepository_SetsTimesheetRepositoryProperty()
        {
            //Arrange
            var mockRepository = new Mock<ITimesheetRepository>();
            var controller = new AdminController(mockRepository.Object);

            //Act & Assert
            Assert.Equal(mockRepository.Object, controller.TimesheetRepository);
        }

        [Fact]
        public async Task GetAllTimesheets_ReturnsOkResult_WithTimesheetResponseModels()
        {
            //Arrange
            var page = -1;
            var pageSize = -1;
            var status = ApprovalStatus.Approved;

            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            var timesheetRepoModels = new List<TimesheetRepoModel>
            {
                new TimesheetRepoModel(),
                new TimesheetRepoModel()
            };
            var totalCount = timesheetRepoModels.Count;

            timesheetRepoMock.Setup(repo => repo.GetAllTimesheets(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ApprovalStatus>()))
                           .ReturnsAsync((timesheetRepoModels, totalCount));

            var controller = new AdminController(timesheetRepoMock.Object);

            //Act
            var result = await controller.GetAllTimesheets(page, pageSize, status);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var responseModel = Assert.IsType<TimesheetResponseModels>(okResult.Value);
            Assert.Equal(timesheetRepoModels.Count, responseModel.Timesheets.Count);
            Assert.Equal(totalCount, responseModel.AllDataCount);

            timesheetRepoMock.Verify(repo => repo.GetAllTimesheets(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ApprovalStatus>()), Times.Once);
        }

        [Fact]
        public async Task GetAllTimesheets_ReturnsNotFound_WhenNoTimesheets()
        {
            //Arrange
            var page = 1;
            var pageSize = 10;
            var status = ApprovalStatus.Approved;

            var timesheetRepoMock = new Mock<ITimesheetRepository>();
            var timesheetRepoModels = new List<TimesheetRepoModel>();
            var totalCount = timesheetRepoModels.Count;

            timesheetRepoMock.Setup(repo => repo.GetAllTimesheets(page, pageSize, status))
                .ReturnsAsync((timesheetRepoModels, totalCount));

            var controller = new AdminController(timesheetRepoMock.Object);

            //Act
            var result = await controller.GetAllTimesheets(page, pageSize, status);

            //Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            timesheetRepoMock.Verify(repo => repo.GetAllTimesheets(page, pageSize, status), Times.Once);
        }
    }
}
