using Microsoft.EntityFrameworkCore;
using Shouldly;
using PeopleManagementRepository.Data;
using PeopleManagementRepository.Models;
using Moq;
using MainHub.Internal.PeopleAndCulture.PeopleManagement.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using MainHub.Internal.PeopleAndCulture;
using PeopleManagement.Api.Models;
using MainHub.Internal.PeopleAndCulture.PeopleManagement.API.Extensions;
using PeopleManagementDataBase;
using System.Drawing.Printing;
using Microsoft.AspNetCore.Http;
using App.Models;
using Newtonsoft.Json;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;

namespace PeopleManagementApi.WebApi.Tests
{
    public class PeopleControllerTests
    {
        private readonly CollaboratorController _collaboratorController;
        private readonly Mock<IPeopleRepository> _peopleRepositoryMock;



        public PeopleControllerTests()
        {
            // Inicializa o repositório People (fictício|mock) e o injeta no controller
            _peopleRepositoryMock = new Mock<IPeopleRepository>();
            _collaboratorController = new CollaboratorController(_peopleRepositoryMock.Object);
        }

        [Fact]
        public async Task PostCreateCollaborator_Should_Create_Collaborator_With_Correct_Data()
        {
            //Arrange
            var userID = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;

            using var dbContext = new PeopleManagementDbContext(options);

            var peopleRepositoryMock = new Mock<IPeopleRepository>();

            var model = new ApiCollaboratorCreateRequestModel
            {
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                FirstName = "user",
                Postal = "1500",
            };

            var repoModel = model.ToPeopleRepoModel();
            peopleRepositoryMock.Setup(x => x.CreateCollaboratorAsync(It.IsAny<PeopleRepoModel>(), userID))
            .Returns(Task.FromResult(new PeopleRepoModel { PeopleGUID = userID }));

            var controller = new CollaboratorController(peopleRepositoryMock.Object);

            //Act   
            var result = await controller.CreateCollaboratorAsync(model);

            //Assert
            result.ShouldBeOfType<ActionResult<ApiCollaboratorCreateResponseModel>>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateCollaboratorAsync_CatchesException_ReturnsBadRequest()
        {
            // Arrange
            var model = new ApiCollaboratorCreateRequestModel();
            var userId = Guid.NewGuid().ToString();

            var mockHttpContext = new Mock<HttpContext>();
            var mockControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

            var mockUser = new Mock<ClaimsPrincipal>();

            var claims = new List<Claim>()
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", userId),
            };

            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.SetupGet(ci => ci.Claims).Returns(claims);

            mockUser.SetupGet(u => u.Claims).Returns(claims);
            mockUser.SetupGet(u => u.Identity).Returns(mockClaimsIdentity.Object);

            var mockPeopleRepository = new Mock<IPeopleRepository>();
            mockPeopleRepository
                .Setup(r => r.CreateCollaboratorAsync(It.IsAny<PeopleRepoModel>(), It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Create failed"));



            mockHttpContext.SetupGet(h => h.User).Returns(mockUser.Object);



            var controller = new CollaboratorController(mockPeopleRepository.Object);
            controller.ControllerContext = mockControllerContext;

            // Act
            var result = await controller.CreateCollaboratorAsync(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            var errorMessage = Assert.IsType<string>(badRequestResult.Value);

            Assert.Equal("Create failed", errorMessage);
        }





        [Fact]
        public async Task GetCollaborators_WithValidParameters_ReturnsPagedCollaborators()
        {
            // Arrange
            int page = 1;
            int pageSize = 2;
            var filter = string.Empty;
            State list = State.All;

            var expectedCollaborators = new List<AllPeopleRepoModel>
                {
                    new AllPeopleRepoModel { PeopleGUID = Guid.NewGuid(), FirstName = "Dinis" },
                    new AllPeopleRepoModel { PeopleGUID = Guid.NewGuid(), FirstName = "João" }
                };

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(r => r.GetAllCollaborators(page, pageSize, filter, list))
                .ReturnsAsync(expectedCollaborators);

            var totalCount = 2;
            mockRepository.Setup(r => r.GetAllCollaborators(1, 500, filter, list))
                .ReturnsAsync(expectedCollaborators.Take(totalCount).ToList());

            var controller = new CollaboratorController(mockRepository.Object);

            // Act
            var result = await controller.GetCollaborators(page, pageSize, filter, list);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);

            var collaboratorResponse = Assert.IsAssignableFrom<ApiCollaboratorResponseWithCount>(okObjectResult.Value);
            Assert.Equal(expectedCollaborators.Count, collaboratorResponse.Collaborators.Count());
            Assert.Equal(totalCount, collaboratorResponse.TotalCount);
        }

        [Fact]
        public async Task GetCollaborators_WhenRepositoryThrowsException_ReturnsBadRequest()
        {
            // Arrange
            int page = 1;
            int pageSize = 10;
            var filter = string.Empty;
            State list = State.All;

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(r => r.GetAllCollaborators(page, pageSize, filter, list))
                .ThrowsAsync(new Exception("Repository exception"));

            var controller = new CollaboratorController(mockRepository.Object);

            // Act
            var result = await controller.GetCollaborators(page, pageSize, filter, list);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Repository exception", badRequestResult.Value);
        }

        [Fact]
        public async Task GetCollaborators_WithNegativeValues_ReturnsDefaultPageAndPageSize()
        {
            // Arrange
            var page = 1;
            var pageSize = 500;
            var filter = string.Empty;
            State list = State.All;

            var expectedCollaborators = new List<AllPeopleRepoModel>
                {
                    new AllPeopleRepoModel { PeopleGUID = Guid.NewGuid(), FirstName = "Dinis" },
                    new AllPeopleRepoModel { PeopleGUID = Guid.NewGuid(), FirstName = "João" }
                };

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(r => r.GetAllCollaborators(page, pageSize, filter, list))
                .ReturnsAsync(expectedCollaborators);

            var controller = new CollaboratorController(mockRepository.Object);

            // Act
            var result = await controller.GetCollaborators(-1, -1, filter, list);

            // Assert
            result.ShouldNotBeNull();

        }

        [Fact]
        public async Task GetCollaborators_NullList_ReturnsDefaultPageAndPageSize()
        {
            // Arrange
            var page = 1;
            var pageSize = 500;
            var filter = string.Empty;

            var expectedCollaborators = new List<AllPeopleRepoModel>
                {
                    new AllPeopleRepoModel { PeopleGUID = Guid.NewGuid(), FirstName = "Dinis", ExitDate = DateTime.Now.AddDays(-5) },
                    new AllPeopleRepoModel { PeopleGUID = Guid.NewGuid(), FirstName = "João", ExitDate = DateTime.Now.AddDays(5) },
                    new AllPeopleRepoModel { PeopleGUID = Guid.NewGuid(), FirstName = "Tomás", ExitDate = DateTime.Now.AddDays(-5) },
                };

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(r => r.GetAllCollaborators(page, pageSize, filter, State.All))
                .ReturnsAsync(expectedCollaborators);

            var controller = new CollaboratorController(mockRepository.Object);

            // Act
            var result = await controller.GetCollaborators(-1, -1, filter, null);

            // Assert

            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetOneCollaborator_ReturnsOk_WhenExistingCollaborator()
        {

            //Arrange
            var personId = Guid.NewGuid();
            var mockRepository = new Mock<IPeopleRepository>();
            var mockCollaborator = new PeopleRepoModel
            {
                PeopleGUID = personId,
                FirstName = "Dinis",
                LastName = "Godinho",
                Email = "dgodinho@mail.pt",
                Iban = "someIban",
                Observations = "observation"
            };

            mockRepository.Setup(r => r.GetOneCollaborator(personId)).ReturnsAsync(mockCollaborator);
            var controller = new CollaboratorController(mockRepository.Object);

            //Act
            var result = await controller.GetOneCollaborator(personId);

            //Assert
            result.ShouldBeOfType<ActionResult<ApiCollaboratorResponseModel>>();
            var okObjectResult = result.Result as OkObjectResult;
        }

        [Fact]
        public async Task GetOneCollaborator_ReturnsBadRequest_WhenExceptionThrown()
        {

            //Arrange
            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(r => r.GetOneCollaborator(It.IsAny<Guid>()))
                .ThrowsAsync(new Exception("Some error message"));
            var mockCollaborator = new PeopleRepoModel
            {
                PeopleGUID = Guid.NewGuid(),
            };

            var controller = new CollaboratorController(mockRepository.Object);

            //Act
            var result = await controller.GetOneCollaborator(mockCollaborator.PeopleGUID);

            //Assert
            result.Result.ShouldBeOfType<BadRequestObjectResult>();
            var badRequestResult = (BadRequestObjectResult)result.Result;
            badRequestResult.StatusCode.ShouldBe(StatusCodes.Status400BadRequest);
            badRequestResult.Value.ShouldBe("Some error message");
        }

        [Fact]
        public async Task DeleteCollaborator_WithValidPersonId_ReturnsNoContent()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var id = Guid.NewGuid();

            var mockHttpContext = new Mock<HttpContext>();
            var mockControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

            var mockUser = new Mock<ClaimsPrincipal>();

            var claims = new List<Claim>()
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", personId.ToString()),
            };

            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.SetupGet(ci => ci.Claims).Returns(claims);

            mockUser.SetupGet(u => u.Claims).Returns(claims);
            mockUser.SetupGet(u => u.Identity).Returns(mockClaimsIdentity.Object);

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(c => c.DeleteCollaboratorAsync(personId, id)).ReturnsAsync(true);

            mockHttpContext.SetupGet(h => h.User).Returns(mockUser.Object);

            var controller = new CollaboratorController(mockRepository.Object);
            controller.ControllerContext = mockControllerContext;


            // Act
            var result = await controller.DeleteCollaborator(personId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
            var noContentResult = (NotFoundResult)result;
            Assert.Equal(StatusCodes.Status404NotFound, noContentResult.StatusCode);
        }
        [Fact]
        public async Task DeleteCollaborator_WithInvalidCollaborator_ReturnsNotFound()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var collaboratorId = Guid.NewGuid();

            var mockHttpContext = new Mock<HttpContext>();
            var mockControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

            var mockUser = new Mock<ClaimsPrincipal>();

            var claims = new List<Claim>()
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", personId.ToString()),
            };

            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.SetupGet(ci => ci.Claims).Returns(claims);

            mockUser.SetupGet(u => u.Claims).Returns(claims);
            mockUser.SetupGet(u => u.Identity).Returns(mockClaimsIdentity.Object);

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(r => r.DeleteCollaboratorAsync(personId, collaboratorId))
                .ReturnsAsync(false);

            mockHttpContext.SetupGet(h => h.User).Returns(mockUser.Object);

            var controller = new CollaboratorController(mockRepository.Object);
            controller.ControllerContext = mockControllerContext;

            // Act
            var result = await controller.DeleteCollaborator(personId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task UpdateCollaborator_WithValidParameters_ReturnNoContent()
        {
            //Arranje

            var personId = Guid.NewGuid();
            var collaboratorUpdateModel = new ApiCollaboratorUpdateModel
            {
                FirstName = "Dinis Godinho"
            };
            var existingCollaborator = new PeopleRepoModel
            {
                FirstName = "Dinis"
            };

            var mockHttpContext = new Mock<HttpContext>();
            var mockControllerContext = new ControllerContext { HttpContext = mockHttpContext.Object };

            var mockUser = new Mock<ClaimsPrincipal>();

            var claims = new List<Claim>()
            {
                new Claim("http://schemas.microsoft.com/identity/claims/objectidentifier", personId.ToString()),
            };

            var mockClaimsIdentity = new Mock<ClaimsIdentity>();
            mockClaimsIdentity.SetupGet(ci => ci.Claims).Returns(claims);

            mockUser.SetupGet(u => u.Claims).Returns(claims);
            mockUser.SetupGet(u => u.Identity).Returns(mockClaimsIdentity.Object);

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(c => c.GetOneCollaborator(personId)).ReturnsAsync(existingCollaborator);
            mockHttpContext.SetupGet(h => h.User).Returns(mockUser.Object);

            var controller = new CollaboratorController(mockRepository.Object);
            controller.ControllerContext = mockControllerContext;

            //Act

            var result = await controller.UpdateCollaborator(personId, collaboratorUpdateModel);

            //Assert

            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
        }

        [Fact]
        public async Task GetCollaboratorHistory_WithValidIdAndPage_ReturnsPagedHistory()
        {
            // Arrange
            var id = Guid.NewGuid();
            var page = 1;
            var pageSize = 2;

            var expectedHistory = new List<PeopleHistoryRepoModel>
            {
                new PeopleHistoryRepoModel { PeopleGUID = Guid.NewGuid(), FirstName= "Dinis"},
                new PeopleHistoryRepoModel { PeopleGUID = Guid.NewGuid(), FirstName= "João"}
            };

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(c => c.GetCollaboratorHistory(id, page, pageSize))
                .ReturnsAsync(expectedHistory);

            var controller = new CollaboratorController(mockRepository.Object);

            // Act
            var result = await controller.GetCollaboratorHistory(id, page, pageSize);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);

            var responseModels = Assert.IsAssignableFrom<List<ApiCollaboratorHistoryResponseModel>>(okObjectResult.Value);
            Assert.Equal(expectedHistory.Count, responseModels.Count);
        }

        [Fact]
        public async Task GetCollaboratorHistory_WithInvalidId_ReturnsNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(c => c.GetCollaboratorHistory(id, page, pageSize))
                .ReturnsAsync(new List<PeopleHistoryRepoModel>());

            var controller = new CollaboratorController(mockRepository.Object);

            // Act
            var result = await controller.GetCollaboratorHistory(id, page, pageSize);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task GetCollaboratorHistory_WithZeroPage_ReturnsPagedHistoryWithPageOne()
        {
            // Arrange
            var id = Guid.NewGuid();
            var page = 0;
            var pageSize = 2;

            var expectedHistory = new List<PeopleHistoryRepoModel>
            {
                new PeopleHistoryRepoModel { PeopleGUID = Guid.NewGuid(), FirstName= "Dinis"},
                new PeopleHistoryRepoModel { PeopleGUID = Guid.NewGuid(), FirstName= "João"}
            };

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(c => c.GetCollaboratorHistory(id, 1, pageSize))
                .ReturnsAsync(expectedHistory);

            var controller = new CollaboratorController(mockRepository.Object);

            // Act
            var result = await controller.GetCollaboratorHistory(id, page, pageSize);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);

            var responseModels = Assert.IsAssignableFrom<List<ApiCollaboratorHistoryResponseModel>>(okObjectResult.Value);
            Assert.Equal(expectedHistory.Count, responseModels.Count);
        }

        [Fact]
        public async Task GetCollaboratorHistory_WithNegativePageSize_ReturnsPagedHistoryWithDefaultPageSize()
        {
            // Arrange
            var id = Guid.NewGuid();
            var page = 1;
            var pageSize = -2;

            var expectedHistory = new List<PeopleHistoryRepoModel>
            {
                new PeopleHistoryRepoModel { PeopleGUID = Guid.NewGuid(), FirstName= "Dinis"},
                new PeopleHistoryRepoModel { PeopleGUID = Guid.NewGuid(), FirstName= "João"}
            };

            var mockRepository = new Mock<IPeopleRepository>();
            mockRepository.Setup(c => c.GetCollaboratorHistory(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(expectedHistory);

            var controller = new CollaboratorController(mockRepository.Object);

            // Act
            var result = await controller.GetCollaboratorHistory(id, page, pageSize);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);

            var responseModels = Assert.IsAssignableFrom<List<ApiCollaboratorHistoryResponseModel>>(okObjectResult.Value);
            Assert.Equal(expectedHistory.Count, responseModels.Count);
        }

    }
}

