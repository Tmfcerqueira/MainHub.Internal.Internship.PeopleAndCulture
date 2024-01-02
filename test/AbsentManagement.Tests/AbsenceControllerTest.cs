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
    public class AbsenceControllerTest
    {



        //ctor test

        [Fact]
        public void Constructor_WithIAbsenceRepository_SetsAbsenceRepositoryProperty()
        {
            // Arrange
            var mockRepository = new Mock<IAbsenceRepository>();
            var controller = new AbsenceController(mockRepository.Object);

            // Act & Assert
            Assert.Equal(mockRepository.Object, controller.AbsenceRepository);
        }


        //creates

        [Fact]
        public async Task CreateAbsence_Returns_OkResult_With_AbsenceCreateResponseModel()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            var absenceRepositoryMock = new Mock<IAbsenceRepository>();

            var model = new AbsenceCreateRequestModel
            {
                AbsenceStart = DateTime.Now,
                AbsenceEnd = DateTime.Now,
                PersonGuid = Guid.NewGuid(),
                AbsenceTypeGuid = Guid.NewGuid()
            };

            var repoModel = model.ToAbsenceRepoModel();
            absenceRepositoryMock.Setup(x => x.CreateAbsence(It.IsAny<AbsenceRepoModel>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(new AbsenceRepoModel { AbsenceGuid = Guid.Empty }));

            var controller = new AbsenceController(absenceRepositoryMock.Object);

            // Act
            var result = await controller.CreateAbsence(model.PersonGuid, model, It.IsAny<Guid>());

            // Assert
            result.ShouldBeOfType<ActionResult<AbsenceCreateResponseModel>>();
            result.ShouldNotBeNull();
        }


        [Fact]
        public async Task CreateAbsence_Returns_BadRequest_When_PersonGuid_Is_Not_Equal_To_Model_PersonGuid()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
            .UseInMemoryDatabase(databaseName: "test_db")
            .Options;
            using var dbContext = new AbsenceManagementDbContext(options);

            var absenceRepositoryMock = new Mock<IAbsenceRepository>();

            var model = new AbsenceCreateRequestModel
            {
                AbsenceStart = DateTime.Now,
                AbsenceEnd = DateTime.Now,
                PersonGuid = Guid.NewGuid(),
                AbsenceTypeGuid = Guid.NewGuid()
            };

            var controller = new AbsenceController(absenceRepositoryMock.Object);

            // Act
            var result = await controller.CreateAbsence(Guid.NewGuid(), model, It.IsAny<Guid>());


            // Assert

            result.Result.ShouldBeOfType<BadRequestObjectResult>();
        }

        //gets

        [Fact]
        public async Task GetAbsencesByPerson_ReturnsOkResult_WithAbsenceResponseModels()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var year = 2023;
            var page = 1;
            var pageSize = 10;
            var status = ApprovalStatus.All;

            var absenceRepoMock = new Mock<IAbsenceRepository>();
            var absenceRepoModels = new List<AbsenceRepoModel>
             {
                new AbsenceRepoModel (),
                new AbsenceRepoModel ()
            };
            absenceRepoMock.Setup(repo => repo.GetAbsencesByPerson(personGuid, year, page, pageSize, status, new DateTime(), new DateTime()))
                           .ReturnsAsync(((absenceRepoModels), 2));

            var totalCount = absenceRepoModels.Count;
            absenceRepoMock.Setup(repo => repo.GetAbsencesByPerson(personGuid, year, page, pageSize, status, new DateTime(), new DateTime()))
                           .ReturnsAsync((new List<AbsenceRepoModel>(absenceRepoModels), 2));

            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.GetAbsencesByPerson(personGuid, year, page, pageSize, status, new DateTime(), new DateTime());

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var responseModels = Assert.IsType<AbsenceResponseModels>(okResult.Value);
            Assert.Equal(absenceRepoModels.Count, responseModels.Absences.Count);
            Assert.Equal(totalCount, responseModels.AllDataCount);

            absenceRepoMock.Verify(repo => repo.GetAbsencesByPerson(personGuid, year, page, pageSize, status, new DateTime(), new DateTime()), Times.Once);
            absenceRepoMock.Verify(repo => repo.GetAbsencesByPerson(personGuid, year, page, pageSize, status, new DateTime(), new DateTime()), Times.Once);
        }


        [Fact]
        public async Task GetAbsenceHistory_ReturnsOkResult_WithAbsenceHistoryResponseModels()
        {
            // Arrange
            var absenceGuid = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var absenceRepoMock = new Mock<IAbsenceRepository>();
            var absenceRepoModels = new List<AbsenceHistoryRepoModel>
            {
                new AbsenceHistoryRepoModel(),
                new AbsenceHistoryRepoModel()
            };
            absenceRepoMock.Setup(repo => repo.GetAbsencesHistory(absenceGuid, page, pageSize))
                           .ReturnsAsync((absenceRepoModels, 2));

            var totalCount = absenceRepoModels.Count;
            absenceRepoMock.Setup(repo => repo.GetAbsencesHistory(absenceGuid, page, pageSize))
                           .ReturnsAsync((absenceRepoModels, 2));

            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.GetAbsenceHistory(absenceGuid, page, pageSize);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);

            var responseModels = Assert.IsType<AbsenceHistoryResponseModels>(okResult.Value);
            Assert.Equal(absenceRepoModels.Count, responseModels.AbsenceHistory.Count);
            Assert.Equal(totalCount, responseModels.AllDataCount);

            absenceRepoMock.Verify(repo => repo.GetAbsencesHistory(absenceGuid, page, pageSize), Times.Once);
            absenceRepoMock.Verify(repo => repo.GetAbsencesHistory(absenceGuid, page, pageSize), Times.Once);
        }


        [Fact]
        public async Task GetAbsencesByPerson_ReturnsNotFound_WhenNoAbsencesFound()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var year = 2023;
            var page = 1;
            var pageSize = 10;
            var status = ApprovalStatus.All;

            var absenceRepoMock = new Mock<IAbsenceRepository>();
            absenceRepoMock.Setup(repo => repo.GetAbsencesByPerson(personGuid, year, page, pageSize, status, new DateTime(), new DateTime()))
                           .ReturnsAsync((new List<AbsenceRepoModel>(), 2));

            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.GetAbsencesByPerson(personGuid, year, page, pageSize, status, new DateTime(), new DateTime());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            absenceRepoMock.Verify(repo => repo.GetAbsencesByPerson(personGuid, year, page, pageSize, status, new DateTime(), new DateTime()), Times.Once);
        }



        [Fact]
        public async Task GetAbsenceHistoryByPerson_ReturnsNotFound_WhenNoAbsencesFound()
        {
            // Arrange
            var absenceGuid = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var absenceRepoMock = new Mock<IAbsenceRepository>();
            absenceRepoMock.Setup(repo => repo.GetAbsencesHistory(absenceGuid, page, pageSize))
                           .ReturnsAsync((new List<AbsenceHistoryRepoModel>(), 0));

            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.GetAbsenceHistory(absenceGuid, page, pageSize);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);

            absenceRepoMock.Verify(repo => repo.GetAbsencesHistory(absenceGuid, page, pageSize), Times.Once);
        }


        [Fact]
        public async Task GetAbsenceByPerson_ReturnsOk_WhenAbsenceFound()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceId = Guid.NewGuid();
            var absenceRepoMock = new Mock<IAbsenceRepository>();
            var absence = new AbsenceRepoModel() { AbsenceGuid = absenceId, PersonGuid = personGuid };
            absenceRepoMock.Setup(repo => repo.GetAbsenceByPerson(personGuid, absenceId))
                           .ReturnsAsync(absence);

            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.GetAbsenceByPerson(personGuid, absenceId);


            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            var model = Assert.IsType<AbsenceResponseModel>(okObjectResult.Value);
        }



        [Fact]
        public async Task GetAbsenceByPerson_ReturnsNotFound_WhenAbsenceNotFound()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceId = Guid.NewGuid();
            var absenceRepoMock = new Mock<IAbsenceRepository>();
            absenceRepoMock.Setup(repo => repo.GetAbsenceByPerson(personGuid, absenceId))
                           .Returns(Task.FromResult<AbsenceRepoModel>(null));


            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.GetAbsenceByPerson(personGuid, absenceId);

            // Assert
            result.Result.ShouldBeOfType<NotFoundObjectResult>();
        }



        //delete
        [Fact]
        public async Task DeleteAbsence_ReturnsNotFound_WhenAbsenceDoesNotExist()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceId = Guid.NewGuid();
            var absenceRepoMock = new Mock<IAbsenceRepository>();
            absenceRepoMock.Setup(repo => repo.DeleteAbsence(personGuid, absenceId))
                           .ReturnsAsync(false);

            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.DeleteAbsence(personGuid, absenceId, It.IsAny<Guid>());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }


        [Fact]
        public async Task DeleteAbsence_ReturnsNoContent_WhenAbsenceDeleted()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceId = Guid.NewGuid();
            var absence = new AbsenceRepoModel() { AbsenceGuid = absenceId, PersonGuid = personGuid };
            var absenceRepoMock = new Mock<IAbsenceRepository>();
            absenceRepoMock.Setup(repo => repo.DeleteAbsence(It.IsAny<Guid>(), It.IsAny<Guid>()))
                           .ReturnsAsync(true);

            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.DeleteAbsence(personGuid, absenceId, Guid.NewGuid());

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);

            absenceRepoMock.Verify(repo => repo.DeleteAbsence(absenceId, It.IsAny<Guid>()), Times.Once);
        }

        //update
        [Fact]
        public async Task UpdateAbsence_ReturnsNoContent_WhenAbsenceUpdatedSuccessfully()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceId = Guid.NewGuid();
            var updateModel = new AbsenceUpdateModel() { AbsenceTypeGuid = Guid.NewGuid(), AbsenceGuid = absenceId, PersonGuid = personGuid };
            var absenceRepoMock = new Mock<IAbsenceRepository>();
            var absence = new AbsenceRepoModel() { AbsenceGuid = absenceId, PersonGuid = personGuid };
            absenceRepoMock.Setup(repo => repo.GetAbsenceByPerson(personGuid, absenceId))
                           .ReturnsAsync(absence);
            absenceRepoMock.Setup(repo => repo.UpdateAbsence(It.IsAny<AbsenceRepoModel>(), It.IsAny<Guid>()))
                           .ReturnsAsync(true);

            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.UpdateAbsence(personGuid, absenceId, updateModel, It.IsAny<Guid>());

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            absenceRepoMock.Verify(repo => repo.UpdateAbsence(It.IsAny<AbsenceRepoModel>(), It.IsAny<Guid>()), Times.Once);
        }



        [Fact]
        public async Task UpdateAbsence_ReturnsNotFound_WhenUpdateFails()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceId = Guid.NewGuid();
            var updateModel = new AbsenceUpdateModel();
            var absenceRepoMock = new Mock<IAbsenceRepository>();
            absenceRepoMock.Setup(repo => repo.UpdateAbsence(It.IsAny<AbsenceRepoModel>(), It.IsAny<Guid>()))
                           .ReturnsAsync(false);

            var controller = new AbsenceController(absenceRepoMock.Object);

            // Act
            var result = await controller.UpdateAbsence(personGuid, absenceId, updateModel, It.IsAny<Guid>());

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }




    }
}
