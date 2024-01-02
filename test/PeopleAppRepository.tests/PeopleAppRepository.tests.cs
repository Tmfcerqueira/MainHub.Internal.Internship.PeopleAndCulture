using App.Models;
using App.Repository;
using Moq;
using PeopleManagement.Api.Proxy.Client.Api;
using ProxyModel = PeopleManagement.Api.Proxy.Client.Model;
using Shouldly;
using Microsoft.Extensions.DependencyInjection;
using PeopleManagement.Api.Proxy.Client.Client;
using PeopleManagement.Api.Proxy.Client.Model;
using Microsoft.EntityFrameworkCore;
using PeopleManagementRepository.Data;
using Microsoft.Graph;
using System.Net;
using System.Runtime.InteropServices;
using MainHub.Internal.PeopleAndCulture.App.Repository.Extensions;
using MainHub.Internal.PeopleAndCulture.Extensions;
using System.Drawing.Printing;

namespace PeopleAppRepositoryTests
{
    public class PeopleAppRepositoryTests
    {
        [Fact]
        public void Constructor_Injects_Proxies()
        {
            // Arrange
            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            var mockAuthProvider = new Mock<IAuthenticationProvider>();
            var mockHttpProvider = new Mock<IHttpProvider>();
            var mockGraphClient = new Mock<GraphServiceClient>(mockAuthProvider.Object, mockHttpProvider.Object);

            // Act
            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, mockGraphClient.Object);
            // Assert
            Assert.Equal(mockCollaboratorProxy.Object, repository.CollaboratorProxy);
        }
        [Fact]
        public void CreateCollaborator_Is_Success()
        {
            // Arrange
            var peopleModel = new PeopleModel();


            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            var mockAuthProvider = new Mock<IAuthenticationProvider>();
            var mockHttpProvider = new Mock<IHttpProvider>();
            var mockGraphClient = new Mock<GraphServiceClient>(mockAuthProvider.Object, mockHttpProvider.Object);

            var responseModel = new ProxyModel.ApiCollaboratorCreateResponseModel
            {
                CcNumber = "123456789",
                TaxNumber = "123456789",
                SsNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                CreatedBy = "None",
                FirstName = "user",
                LastName = "lastName",
                Email = "userlastName@mainhub.pt",
                Locality = "Lisboa",
                Postal = "1500",
                Status = "Active",
            };

            mockCollaboratorProxy.Setup(p => p.CreateCollaboratorAsyncAsync(It.IsAny<ProxyModel.ApiCollaboratorCreateRequestModel>(), 0, default))
                     .ReturnsAsync(responseModel);

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, mockGraphClient.Object);

            // Act
            var result = repository.CreateCollaboratorAsync(peopleModel);

            // Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreateCollaboratorAsync_InvalidData_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var invalidPeopleModel = new PeopleModel
            {
                FirstName = "Dinis",
                LastName = "Godinho",
                Email = ""
            };

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, null!);

            // Act
            var result = await repository.CreateCollaboratorAsync(invalidPeopleModel);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetAllCollaborators_ReturnsCollaboratorsAndCount()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = string.Empty;
            App.Repository.State list = App.Repository.State.All;

            var expectedCollaborators = new List<ApiCollaboratorAllResponseModel>
            {
                new ApiCollaboratorAllResponseModel
                {
                    FirstName = "Dinis", LastName = "Godinho", Email = "dgodinho@mail.com", Status="Active"
                },
                new ApiCollaboratorAllResponseModel
                {
                    FirstName = "Joao", LastName = "Monteiro", Email = "jmonteiro@mail.com", Status="Inactive"
                }
            };

            var expectedTotalCount = expectedCollaborators.Count;

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(page, pageSize, filter, ProxyModel.State.All, 0, default))
                .ReturnsAsync(new ApiCollaboratorResponseWithCount
                {
                    Collaborators = expectedCollaborators,
                    TotalCount = expectedTotalCount
                });

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, null!);

            // Act
            var (collaborators, count) = await repository.GetAllCollaborators(page, pageSize, filter, list);

            // Assert
            Assert.Equal(expectedCollaborators.Count, collaborators.Count);
            Assert.Equal(expectedTotalCount, count);
        }

        [Fact]
        public async Task GetAllCollaborators_ReturnsEmptyListAndZeroCount()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = string.Empty;
            App.Repository.State list = App.Repository.State.All;

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(page, pageSize, filter, ProxyModel.State.All, 0, default))
                .ReturnsAsync(new ApiCollaboratorResponseWithCount
                {
                    Collaborators = new List<ApiCollaboratorAllResponseModel>(),
                    TotalCount = 0
                });

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, null!);

            // Act
            var (collaborators, count) = await repository.GetAllCollaborators(page, pageSize, filter, list);

            // Assert
            Assert.Empty(collaborators);
            Assert.Equal(0, count);
        }


        [Fact]
        public async Task GetOneCollaborator_Returns_Correct_Collaborator()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var collaboratorResponseModel = new ApiCollaboratorResponseModel
            {
                PeopleGUID = personGuid,
                FirstName = "Dinis",
                LastName = "Godinho",
                Email = "dgodinho@mail.pt",
                ContractType = ProxyModel.Contract.NoTerm,
            };

            var mockPeopleProxy = new Mock<ICollaboratorApi>();
            mockPeopleProxy
                .Setup(c => c.ApiPeopleIdGetAsync(personGuid, 0, default))
                .ReturnsAsync(collaboratorResponseModel);

            var repository = new PeopleAppRepository(mockPeopleProxy.Object, null!);

            // Act
            var result = await repository.GetOneCollaborator(personGuid);

            // Assert
            result.ShouldNotBeNull();
            result.ShouldBeOfType<PeopleModel>();
            result.FirstName.ShouldBe("Dinis");
        }


        [Fact]
        public async Task GetOneCollaborator_CatchesException_ReturnsNull()
        {
            // Arrange
            var peoplemodelId = Guid.NewGuid();

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy
                .Setup(p => p.ApiPeopleIdGetAsync(peoplemodelId, 0, default))
                .ThrowsAsync(new Exception("Failed to get collaborator"));

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, null!);

            // Act
            var result = await repository.GetOneCollaborator(peoplemodelId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteCollaboratorAsync_WithValidPersonId_DeletesCollaboratorAndReturnsTrue()
        {
            // Arrange
            var personId = Guid.NewGuid();

            var mockpeopleProxy = new Mock<ICollaboratorApi>();
            var mockAuthProvider = new Mock<IAuthenticationProvider>();
            var mockHttpProvider = new Mock<IHttpProvider>();
            var mockGraphClient = new Mock<GraphServiceClient>(mockAuthProvider.Object, mockHttpProvider.Object);
            mockpeopleProxy
                .Setup(m => m.ApiPeoplePersonIdDeleteAsync(personId, 0, default))
                .Returns(Task.CompletedTask);


            var repository = new PeopleAppRepository(mockpeopleProxy.Object, mockGraphClient.Object);

            // Act
            var result = await repository.DeleteCollaboratorAsync(personId);

            // Assert
            Assert.True(result);

        }
        [Fact]
        public async Task UpdateCollaboratorAsync_WithValidParameters_UpdatesCollaboratorAndReturnsTrue()
        {
            // Arranje
            var personId = Guid.NewGuid();
            var updatedCollaborator = new PeopleModel
            {
                PeopleGUID = personId,
                FirstName = "Dinis Godinho",
            };
            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            var mockAuthProvider = new Mock<IAuthenticationProvider>();
            var mockHttpProvider = new Mock<IHttpProvider>();
            var mockGraphClient = new Mock<GraphServiceClient>(mockAuthProvider.Object, mockHttpProvider.Object);
            mockCollaboratorProxy.Setup(c => c.ApiPeoplePersonIdPutAsync(updatedCollaborator.PeopleGUID, It.IsAny<ApiCollaboratorUpdateModel>(), 0, default))
                .Returns(Task.CompletedTask);

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, mockGraphClient.Object);

            //Act

            var result = await repository.UpdateCollaboratorAsync(personId, updatedCollaborator);

            // Assert

            Assert.True(result);
        }

        [Fact]
        public async Task UpdateCollaboratorAsync_CatchesException_ReturnsFalse()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var collaborator = new PeopleModel();

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy
                .Setup(m => m.ApiPeoplePersonIdPutAsync(personId, It.IsAny<ApiCollaboratorUpdateModel>(), 0, default))
                .ThrowsAsync(new Exception("Update failed"));

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, null!);

            // Act
            var result = await repository.UpdateCollaboratorAsync(personId, collaborator);

            // Assert
            Assert.False(result);
            mockCollaboratorProxy.Verify(
                m => m.ApiPeoplePersonIdPutAsync(personId, It.IsAny<ApiCollaboratorUpdateModel>(), 0, default),
                Times.Once,
                $"Expected ApiPeoplePersonIdPutAsync to be called once with personId: {personId}");
        }

        [Fact]
        public async Task GetBirthdays_ReturnsFilteredCollaborators()
        {
            // Arrange
            var currentDate = DateTime.Now;
            var endDate = currentDate.AddDays(7);
            var collaborators = new List<ApiCollaboratorAllResponseModel>
            {
                new ApiCollaboratorAllResponseModel { FirstName = "Dinis", BirthDate = currentDate.AddDays(1), Status = "Active", LastName = "Godinho", Email = "dgodinho@mail.com" },
                new ApiCollaboratorAllResponseModel { FirstName = "João", BirthDate = currentDate.AddDays(5), Status = "Active", LastName = "Monteiro", Email = "jmonteiro@mail.com" },
                new ApiCollaboratorAllResponseModel { FirstName = "Tomás", BirthDate = currentDate.AddDays(10), Status = "Active", LastName = "Bota", Email = "tbota@mail.com" },
            };

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(It.IsAny<int>(), It.IsAny<int>(), string.Empty, PeopleManagement.Api.Proxy.Client.Model.State.All, 0, default))
                .ReturnsAsync(new ApiCollaboratorResponseWithCount
                {
                    Collaborators = collaborators,
                    TotalCount = collaborators.Count
                });

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, null!);

            // Act
            var result = await repository.GetBirthdays();

            // Assert
            result.Count.ShouldBe(2);
            Assert.Contains(result, c => c.FirstName == "Dinis");
            Assert.Contains(result, c => c.FirstName == "João");
            Assert.DoesNotContain(result, c => c.FirstName == "Tomás");
        }




        [Fact]
        public async Task GetAzure_UsersExistNoMatchingCollaborators_ReturnsFilteredUsers()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = string.Empty;
            ProxyModel.State list = ProxyModel.State.All;

            var collaborators = new List<ApiCollaboratorAllResponseModel>
            {
                new ApiCollaboratorAllResponseModel { Email = "othermail@example.com", FirstName="Dinis", LastName="Godinho",Status="Active"},
                new ApiCollaboratorAllResponseModel { Email = "othermail2@example.com", FirstName="Joao", LastName="Monteiro",Status="Inactive" }
            };

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(1, 500, filter, list, 0, default))
                .ReturnsAsync(new ApiCollaboratorResponseWithCount
                {
                    Collaborators = collaborators
                });

            var graphUsers = new List<User>
            {
                new User { GivenName = "Dinis", Surname="Godinho", Mail ="dgodinho@mainhub.pt", Id = Guid.NewGuid().ToString() },
                new User { GivenName = "Joao", Surname="Monteiro", Mail ="jmonteiro@mainhub.pt", Id = Guid.NewGuid().ToString() }
            };

            var mockAuthProvider = new Mock<IAuthenticationProvider>();
            var mockHttpProvider = new Mock<IHttpProvider>();
            var mockGraphServiceClient = new Mock<GraphServiceClient>(mockAuthProvider.Object, mockHttpProvider.Object);
            mockGraphServiceClient
                .Setup(c => c.Users
                    .Request()
                    .GetAsync(default))
                .ReturnsAsync(() => new MainHub.Internal.PeopleAndCulture.MockGraphServiceUsersCollectionPage(graphUsers));

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, mockGraphServiceClient.Object);

            // Act
            var (result, count) = await repository.GetAzure(page, pageSize, null!);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(2, count);
        }

        [Fact]
        public async Task GetAzure_AllUsersAreMatchingCollaborators_ReturnsEmptyList()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = string.Empty;
            ProxyModel.State list = ProxyModel.State.All;

            var collaborators = new List<ApiCollaboratorAllResponseModel>
            {
                new ApiCollaboratorAllResponseModel { Email = "user1@example.com" },
                new ApiCollaboratorAllResponseModel { Email = "user2@example.com" }
            };

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(1, 500, filter, list, 0, default))
                .ReturnsAsync(new ApiCollaboratorResponseWithCount
                {
                    Collaborators = collaborators
                });

            var graphUsers = new List<User>
            {
                new User { Mail = "user1@example.com" },
                new User { Mail = "user2@example.com" }
            };

            var mockAuthProvider = new Mock<IAuthenticationProvider>();
            var mockHttpProvider = new Mock<IHttpProvider>();
            var mockGraphServiceClient = new Mock<GraphServiceClient>(mockAuthProvider.Object, mockHttpProvider.Object);
            mockGraphServiceClient
                .Setup(c => c.Users
                    .Request()
                    .GetAsync(default))
                .ReturnsAsync(() => new MainHub.Internal.PeopleAndCulture.MockGraphServiceUsersCollectionPage(graphUsers));

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, mockGraphServiceClient.Object);

            // Act
            var (result, count) = await repository.GetAzure(page, pageSize, null!);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task GetAzure_NoUsersReturned_ReturnsEmptyList()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = string.Empty;
            ProxyModel.State list = ProxyModel.State.All;

            var collaborators = new List<ApiCollaboratorAllResponseModel>
            {
                new ApiCollaboratorAllResponseModel { Email = "user1@example.com" },
                new ApiCollaboratorAllResponseModel { Email = "user2@example.com" }
            };

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(1, 500, filter, list, 0, default))
                .ReturnsAsync(new ApiCollaboratorResponseWithCount
                {
                    Collaborators = collaborators
                });

            var graphUsers = new List<User>();

            var mockAuthProvider = new Mock<IAuthenticationProvider>();
            var mockHttpProvider = new Mock<IHttpProvider>();
            var mockGraphServiceClient = new Mock<GraphServiceClient>(mockAuthProvider.Object, mockHttpProvider.Object);
            mockGraphServiceClient
                .Setup(c => c.Users
                    .Request()
                    .GetAsync(default))
                .ReturnsAsync(() => new MainHub.Internal.PeopleAndCulture.MockGraphServiceUsersCollectionPage(graphUsers));

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, mockGraphServiceClient.Object);

            // Act
            var (result, count) = await repository.GetAzure(page, pageSize, null!);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task GetAzure_PageAndPageSizeNotIncludingUsers_ReturnsEmptyList()
        {
            // Arrange
            var page = 3;
            var pageSize = 10;
            var filter = string.Empty;
            ProxyModel.State list = ProxyModel.State.All;


            var collaborators = new List<ApiCollaboratorAllResponseModel>
            {
                new ApiCollaboratorAllResponseModel { Email = "user1@example.com" },
                new ApiCollaboratorAllResponseModel { Email = "user2@example.com" }
            };

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(1, 500, filter, list, 0, default))
                .ReturnsAsync(new ApiCollaboratorResponseWithCount
                {
                    Collaborators = collaborators
                });

            var graphUsers = new List<User>
            {
                new User { GivenName = "Dinis", Surname="Godinho", Mail ="dgodinho@mainhub.pt", Id = Guid.NewGuid().ToString() },
                new User { GivenName = "Joao", Surname="Monteiro", Mail ="jmonteiro@mainhub.pt", Id = Guid.NewGuid().ToString() }
            };

            var mockAuthProvider = new Mock<IAuthenticationProvider>();
            var mockHttpProvider = new Mock<IHttpProvider>();
            var mockGraphServiceClient = new Mock<GraphServiceClient>(mockAuthProvider.Object, mockHttpProvider.Object);
            mockGraphServiceClient
                .Setup(c => c.Users
                    .Request()
                    .GetAsync(default))
                .ReturnsAsync(() => new MainHub.Internal.PeopleAndCulture.MockGraphServiceUsersCollectionPage(graphUsers));

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, mockGraphServiceClient.Object);

            // Act
            var (result, count) = await repository.GetAzure(page, pageSize, null!);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAzure_UsersExistNoMatchingCollaborators_Filtered_ReturnsMatchingCollaborators()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = "D";

            var collaborators = new ApiCollaboratorResponseWithCount
            {
                Collaborators = new List<ApiCollaboratorAllResponseModel>
                {
                    new ApiCollaboratorAllResponseModel { Email = "othermail@example.com" },
                    new ApiCollaboratorAllResponseModel { Email = "othermail2@example.com" }
                }
            };

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(1, 500, string.Empty, PeopleManagement.Api.Proxy.Client.Model.State.All, 0, default))
                .ReturnsAsync(collaborators);

            var graphUsers = new List<User>
            {
                new User { GivenName = "Dinis", Surname="Godinho", Mail ="dgodinho@mainhub.pt", Id = Guid.NewGuid().ToString() },
                new User { GivenName = "Joao", Surname="Monteiro", Mail ="jmonteiro@mainhub.pt", Id = Guid.NewGuid().ToString() }
            };

            var mockAuthProvider = new Mock<IAuthenticationProvider>();
            var mockHttpProvider = new Mock<IHttpProvider>();
            var mockGraphServiceClient = new Mock<GraphServiceClient>(mockAuthProvider.Object, mockHttpProvider.Object);
            mockGraphServiceClient
                .Setup(c => c.Users
                    .Request()
                    .GetAsync(default))
                .ReturnsAsync(() => new MainHub.Internal.PeopleAndCulture.MockGraphServiceUsersCollectionPage(graphUsers));

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, mockGraphServiceClient.Object);

            // Act
            var (result, count) = await repository.GetAzure(page, pageSize, filter);

            // Assert
            Assert.NotNull(result);
            result.Count.ShouldBe(1);
            Assert.Equal(1, count);
            Assert.Equal("Dinis", result[0].FirstName);
        }


        [Fact]
        public async Task ImportAzure_ValidData_ReturnsPeopleModel()
        {
            // Arrange
            var id = Guid.NewGuid();
            var allPeopleModel = new AllPeopleModel
            {
                PeopleGUID = id,
                FirstName = "João",
                LastName = "Monteiro",
                Email = "jmonteiro@mail.com",
            };

            var collaboratorModel = allPeopleModel.ToPeopleModel();
            var createRequestModel = collaboratorModel.ToPeopleCreateRequestModel();
            var expectedResponse = new ApiCollaboratorCreateResponseModel
            {
                FirstName = "João",
                LastName = "Monteiro",
                Email = "jmonteiro@mail.com",
                ContractType = ProxyModel.Contract.NoTerm,
            };

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.CreateCollaboratorAsyncAsync(createRequestModel, 0, default))
                .ReturnsAsync(expectedResponse);

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, null!);

            // Act
            var result = await repository.ImportAzure(id, allPeopleModel);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(allPeopleModel.FirstName, result.FirstName);
            Assert.Equal(allPeopleModel.LastName, result.LastName);
            Assert.Equal(allPeopleModel.Email, result.Email);
        }

        [Fact]
        public async Task ImportAzure_ExceptionThrown_ReturnsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var allPeopleModel = new AllPeopleModel
            {
                FirstName = "John",
                LastName = "Doe",
                Email = ""
            };

            var collaboratorModel = allPeopleModel.ToPeopleModel();
            var createRequestModel = collaboratorModel.ToPeopleCreateRequestModel();
            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.CreateCollaboratorAsyncAsync(createRequestModel, 0, default))
                .ThrowsAsync(new Exception("Simulated exception"));

            var repository = new PeopleAppRepository(mockCollaboratorProxy.Object, null!);

            // Act
            var result = await repository.ImportAzure(id, allPeopleModel);

            // Assert
            Assert.Null(result);
        }



    }

}
