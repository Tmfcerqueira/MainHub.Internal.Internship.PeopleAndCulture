using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Controllers;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Shouldly;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using Microsoft.AspNetCore.Builder;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.App.Models;

namespace MainHub.Internal.PeopleAndCulture.Tests.AbsentManagementTests
{
    public class AdminControllerTest
    {

        [Fact]
        public void Constructor_WithIAbsenceRepository_SetsAbsenceRepositoryProperty()
        {
            // Arrange
            var mockRepository = new Mock<IAbsenceRepository>();
            var controller = new AdminController(mockRepository.Object);

            // Act & Assert
            Assert.Equal(mockRepository.Object, controller.AbsenceRepository);
        }

        [Fact]
        public async Task GetAllAbsences_ReturnsOkResult_WithAbsenceResponseModels()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var status = ApprovalStatus.Approved;

            var absenceRepoMock = new Mock<IAbsenceRepository>();
            var absenceRepoModels = new List<AbsenceRepoModel>
            {
                new AbsenceRepoModel(),
                new AbsenceRepoModel()
            };
            var totalCount = absenceRepoModels.Count;

            absenceRepoMock.Setup(repo => repo.GetAllAbsences(page, pageSize, status))
                .ReturnsAsync((absenceRepoModels, totalCount));

            var controller = new AdminController(absenceRepoMock.Object);

            // Act
            var result = await controller.GetAllAbsences(page, pageSize, status);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var responseModel = Assert.IsType<AbsenceResponseModels>(okResult.Value);
            Assert.Equal(absenceRepoModels.Count, responseModel.Absences.Count);
            Assert.Equal(totalCount, responseModel.AllDataCount);

            absenceRepoMock.Verify(repo => repo.GetAllAbsences(page, pageSize, status), Times.Once);
        }

        [Fact]
        public async Task GetAllAbsences_ReturnsOkResult_WithWrongPages_WithAbsenceResponseModels()
        {
            // Arrange
            var page = 0;
            var pageSize = 21;
            var status = ApprovalStatus.Approved;

            var absenceRepoMock = new Mock<IAbsenceRepository>();
            var absenceRepoModels = new List<AbsenceRepoModel>
            {
                new AbsenceRepoModel(),
                new AbsenceRepoModel()
            };

            absenceRepoMock.Setup(repo => repo.GetAllAbsences(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ApprovalStatus>()))
                .ReturnsAsync((absenceRepoModels, absenceRepoModels.Count));

            var controller = new AdminController(absenceRepoMock.Object);

            // Act
            var result = await controller.GetAllAbsences(page, pageSize, status);

            // Assert
            var notFoundResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, notFoundResult.StatusCode);
        }


        [Fact]
        public async Task GetAllAbsences_ReturnsNotFound_WhenNoAbsences()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var status = ApprovalStatus.Approved;

            var absenceRepoMock = new Mock<IAbsenceRepository>();
            var absenceRepoModels = new List<AbsenceRepoModel>();
            var totalCount = absenceRepoModels.Count;

            absenceRepoMock.Setup(repo => repo.GetAllAbsences(page, pageSize, status))
                .ReturnsAsync((absenceRepoModels, totalCount));

            var controller = new AdminController(absenceRepoMock.Object);

            // Act
            var result = await controller.GetAllAbsences(page, pageSize, status);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            absenceRepoMock.Verify(repo => repo.GetAllAbsences(page, pageSize, status), Times.Once);
        }


    }
}
