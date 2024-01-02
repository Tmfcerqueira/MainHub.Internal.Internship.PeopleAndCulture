using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsentManagement.Api.Proxy.Client.Api;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Controllers;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Extensions;
using Moq;

namespace MainHub.Internal.PeopleAndCulture
{
    public class AbsenceTypeControllerTest
    {

        //ctor test

        [Fact]
        public void Constructor_WithIAbsenceRepository_SetsAbsenceRepositoryProperty()
        {
            // Arrange
            var mockRepository = new Mock<IAbsenceRepository>();
            var controller = new AbsenceTypeController(mockRepository.Object);

            // Act & Assert
            Assert.Equal(mockRepository.Object, controller.AbsenceRepository);
        }


        [Fact]
        public async Task CreateAbsenceType_ValidModel_ReturnsOkResultWithResponseModel()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            var absenceRepositoryMock = new Mock<IAbsenceRepository>();

            var model = new AbsentManagement.API.Models.AbsenceTypeCreateRequestModel
            {
                Type = "Other"
            };

            var repoModel = model.ToAbsenceTypeRepoModel();
            absenceRepositoryMock.Setup(x => x.CreateAbsenceType(It.IsAny<AbsentManagement.Repository.Models.AbsenceTypeRepoModel>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(new AbsentManagement.Repository.Models.AbsenceTypeRepoModel()));

            var controller = new AbsenceTypeController(absenceRepositoryMock.Object);

            // Act
            var result = await controller.CreateAbsenceType(model, It.IsAny<Guid>());

            // Assert
            result.ShouldBeOfType<ActionResult<AbsentManagement.API.Models.AbsenceTypeCreateResponseModel>>();
            result.ShouldNotBeNull();
        }


        [Fact]
        public async Task GetAllAbsenceTypes_ReturnsOkResult_WithListOfAbsenceTypeRepoModels()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            var absenceRepositoryMock = new Mock<IAbsenceRepository>();

            var absenceTypes = new List<AbsentManagement.Repository.Models.AbsenceTypeRepoModel>
            {
                new AbsentManagement.Repository.Models.AbsenceTypeRepoModel()
            };

            absenceRepositoryMock.Setup(repo => repo.GetAllAbsenceTypes()).ReturnsAsync(absenceTypes);

            var controller = new AbsenceTypeController(absenceRepositoryMock.Object);

            // Act
            var result = await controller.GetAllAbsenceTypes();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var model = Assert.IsAssignableFrom<List<AbsentManagement.API.Models.AbsenceTypeResponseModel>>(okResult.Value);
        }

    }
}
