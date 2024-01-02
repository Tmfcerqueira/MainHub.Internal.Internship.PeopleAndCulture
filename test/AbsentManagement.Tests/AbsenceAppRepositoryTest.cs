using AbsentManagement.Api.Proxy.Client.Api;
using ProxyModel = AbsentManagement.Api.Proxy.Client.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Moq;
using AbsentManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using AbsentManagement.Api.Proxy.Client.Client;
using System.Linq.Expressions;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.AppRepository.Models;
using MainHub.Internal.PeopleAndCulture.App.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository;
using MainHub.Internal.PeopleAndCulture.App.Repository.Extensions;
using Microsoft.AspNetCore.Routing;
using Microsoft.OpenApi.Any;
using PeopleManagement.Api.Proxy.Client.Api;
using App.Repository;
using PeopleProxyModel = PeopleManagement.Api.Proxy.Client.Model;
using System;
using Microsoft.Graph;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.DataProtection;

namespace MainHub.Internal.PeopleAndCulture.Tests.AbsentManagementTests
{
    public class AbsenceAppRepositoryTest : Bunit.TestContext
    {
        [Fact]
        public void Constructor_InjectsProxies()
        {
            // Arrange
            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();

            // Act
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);


            var repository = new AbsenceAppRepository(
                mockAbsenceProxy.Object,
                mockAbsenceTypeProxy.Object,
                new Mock<IAdminApi>().Object,
                new Mock<ICollaboratorApi>().Object);

            // Assert
            Assert.Equal(mockAbsenceProxy.Object, repository.AbsenceProxy);
            Assert.Equal(mockAbsenceTypeProxy.Object, repository.AbsenceTypeProxy);
        }


        //Absence


        [Fact]
        public async Task CreateAbsence_Success()
        {
            // Arrange

            var personGuid = Guid.NewGuid();
            var absenceModel = new AbsenceModel
            {
                PersonGuid = Guid.NewGuid(),
                AbsenceStart = new DateTime(2023, 5, 20),
                AbsenceEnd = new DateTime(2023, 5, 25)
            };

            var absenceModels = new List<AbsenceModel>
            {
                new AbsenceModel()
            };

            var absenceTypeResponseMock = new List<ProxyModel.AbsenceTypeResponseModel>();


            var absenceResponseMock = new AbsenceResponseModels()
            {
                Absences = new List<ProxyModel.AbsenceResponseModel>()
                {
                     new AbsenceResponseModel { AbsenceStart = new DateTime(2023, 4, 30), AbsenceEnd = new DateTime(2023, 5, 2), PersonGuid =  personGuid, ApprovalStatus= ApprovalStatus.Draft},
                    new AbsenceResponseModel { AbsenceStart = new DateTime(2023, 5, 5), AbsenceEnd = new DateTime(2023, 5, 8), PersonGuid = personGuid, ApprovalStatus= ApprovalStatus.Draft}
                },
                AllDataCount = 2
            };

            var expectedResponse = new AbsenceCreateResponseModel
            {
                AbsenceGuid = Guid.NewGuid(),
                PersonGuid = absenceModel.PersonGuid,
                AbsenceTypeGuid = Guid.NewGuid(),
                AbsenceStart = absenceModel.AbsenceStart,
                AbsenceEnd = absenceModel.AbsenceEnd,
                ApprovalStatus = ApprovalStatus.Draft,
                ApprovedBy = "None",
                ApprovalDate = new DateTime(2999, 12, 31, 23, 59, 59),
                SubmissionDate = DateTime.Now,
                Schedule = "Full Day"
            };

            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            mockAbsenceProxy
                .Setup(p => p.ApiPeoplePersonGuidAbsencePostAsync(absenceModel.PersonGuid, It.IsAny<Guid>(), It.IsAny<AbsenceCreateRequestModel>(), 0, default))
                .ReturnsAsync(expectedResponse);

            mockAbsenceProxy
              .Setup(m => m.ApiPeoplePersonGuidAbsenceGetAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), ApprovalStatus.All, It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0, default))
              .Returns(Task.FromResult(absenceResponseMock));


            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy
                .Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default))
                .ReturnsAsync(absenceTypeResponseMock);

            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var result = await repository.CreateAbsenceAsync(absenceModel, It.IsAny<Guid>());

            // Assert
            mockAbsenceProxy.Verify(p => p.ApiPeoplePersonGuidAbsencePostAsync(absenceModel.PersonGuid, It.IsAny<Guid>(), It.IsAny<AbsenceCreateRequestModel>(), 0, default), Times.Once);

            Assert.Equal(200, result.Item2);
        }


        [Fact]
        public async Task CreateAbsence_Returns_EmptyList_And_ErrorCode_WhenExceptionOccurs()
        {
            // Arrange

            var personGuid = Guid.NewGuid();
            var absenceModel = new AbsenceModel
            {
                PersonGuid = Guid.NewGuid(),
                AbsenceStart = new DateTime(2023, 5, 20),
                AbsenceEnd = new DateTime(2023, 5, 25)
            };

            var absenceModels = new List<AbsenceModel>
            {
                new AbsenceModel()
            };

            var absenceTypeResponseMock = new List<ProxyModel.AbsenceTypeResponseModel>();


            var absenceResponseMock = new AbsenceResponseModels()
            {
                Absences = new List<ProxyModel.AbsenceResponseModel>()
                {
                     new AbsenceResponseModel { AbsenceStart = new DateTime(2023, 4, 30), AbsenceEnd = new DateTime(2023, 5, 2), PersonGuid =  personGuid, ApprovalStatus= ApprovalStatus.Draft},
                    new AbsenceResponseModel { AbsenceStart = new DateTime(2023, 5, 5), AbsenceEnd = new DateTime(2023, 5, 8), PersonGuid = personGuid, ApprovalStatus= ApprovalStatus.Draft}
                },
                AllDataCount = 2
            };

            var expectedResponse = new AbsenceCreateResponseModel
            {
                AbsenceGuid = Guid.NewGuid(),
                PersonGuid = absenceModel.PersonGuid,
                AbsenceTypeGuid = Guid.NewGuid(),
                AbsenceStart = absenceModel.AbsenceStart,
                AbsenceEnd = absenceModel.AbsenceEnd,
                ApprovalStatus = ApprovalStatus.Draft,
                ApprovedBy = "None",
                ApprovalDate = new DateTime(2999, 12, 31, 23, 59, 59),
                SubmissionDate = DateTime.Now,
                Schedule = "Full Day"
            };

            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            mockAbsenceProxy
                .Setup(p => p.ApiPeoplePersonGuidAbsencePostAsync(absenceModel.PersonGuid, It.IsAny<Guid>(), It.IsAny<AbsenceCreateRequestModel>(), 0, default))
                .ThrowsAsync(new ApiException(404, "error"));

            mockAbsenceProxy
              .Setup(m => m.ApiPeoplePersonGuidAbsenceGetAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), ApprovalStatus.All, It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0, default))
              .Returns(Task.FromResult(absenceResponseMock));


            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy
                .Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default))
                .ReturnsAsync(absenceTypeResponseMock);

            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var result = await repository.CreateAbsenceAsync(absenceModel, It.IsAny<Guid>());

            // Assert
            Assert.Equal(404, result.Item2);
        }

        //gets
        [Fact]
        public async Task GetAbsenceByPerson_ReturnsAbsenceModel_WhenApiCallSucceeds()
        {

            // Arrange
            var personGuid = Guid.NewGuid();

            var absenceModel = new AbsenceModel();

            var absenceResponseMock = new ProxyModel.AbsenceResponseModel();

            var absenceTypeResponseMock = new List<ProxyModel.AbsenceTypeResponseModel>();

            var mockAbsenceProxy = new Mock<IAbsenceApi>();

            mockAbsenceProxy
                .Setup(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdGetAsync(personGuid, Guid.Empty, 0, default))
                .ReturnsAsync(absenceResponseMock);

            var mockRepo = new Mock<IAbsenceAppRepository>();
            mockRepo.Setup(m => m.GetAbsenceByPersonAsync(personGuid, absenceModel.AbsenceGuid))
                .ReturnsAsync((absenceModel, 200));

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy
                .Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default))
                .ReturnsAsync(absenceTypeResponseMock);


            // Inject the mocked IAbsenceApi
            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var (models, error) = await repository.GetAbsenceByPersonAsync(personGuid, absenceModel.AbsenceGuid);


            // Assert
            models.ShouldNotBeNull();
            error.ShouldBe(200); // Check to ensure error is not null
        }

        [Fact]
        public async Task GetAbsenceByPerson_ReturnsErrorCode_WhenApiCallFails()
        {
            // Arrange
            var personGuid = Guid.NewGuid();

            var absenceModel = new AbsenceModel();


            var absenceResponseMock = new ProxyModel.AbsenceResponseModel();

            var absenceTypeResponseMock = new List<ProxyModel.AbsenceTypeResponseModel>();

            var mockAbsenceProxy = new Mock<IAbsenceApi>();

            mockAbsenceProxy
                .Setup(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdGetAsync(personGuid, Guid.Empty, 0, default))
                .ThrowsAsync(new ApiException(404, "error"));

            var mockRepo = new Mock<IAbsenceAppRepository>();
            mockRepo.Setup(m => m.GetAbsenceByPersonAsync(personGuid, absenceModel.AbsenceGuid))
                .ReturnsAsync((absenceModel, 200));

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy
                .Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default))
                .ReturnsAsync(absenceTypeResponseMock);


            // Inject the mocked IAbsenceApi
            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var (models, error) = await repository.GetAbsenceByPersonAsync(personGuid, absenceModel.AbsenceGuid);


            // Assert
            Assert.Equal(404, error);
        }


        [Fact]
        public async Task GetAbsencesByPerson_ReturnsAbsenceModels_WhenApiCallSucceeds()
        {
            // Arrange
            var personGuid = Guid.NewGuid();

            var absenceModels = new List<AbsenceModel>
            {
                new AbsenceModel()
            };


            var absenceResponseMock = new AbsenceResponseModels()
            {
                Absences = new List<ProxyModel.AbsenceResponseModel>()
                {
                    new ProxyModel.AbsenceResponseModel()
                },
                AllDataCount = 1
            };

            var absenceTypeResponseMock = new List<ProxyModel.AbsenceTypeResponseModel>();

            var mockAbsenceProxy = new Mock<IAbsenceApi>();

            mockAbsenceProxy
                .Setup(m => m.ApiPeoplePersonGuidAbsenceGetAsync(personGuid, DateTime.Now.Year, 1, 15, ApprovalStatus.All, new DateTime(), new DateTime(), 0, default))
                .ReturnsAsync(absenceResponseMock);

            var mockRepo = new Mock<IAbsenceAppRepository>();
            mockRepo.Setup(m => m.GetAbsencesByPersonAsync(personGuid, DateTime.Now.Year, 1, 15, ApprovalStatus.All, new DateTime(), new DateTime()))
                .ReturnsAsync((absenceModels, 200, 1));

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy
                .Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default))
                .ReturnsAsync(absenceTypeResponseMock);



            // Inject the mocked IAbsenceApi
            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var (models, error, count) = await repository.GetAbsencesByPersonAsync(personGuid, DateTime.Now.Year, 1, 15, ApprovalStatus.All, new DateTime(), new DateTime());


            // Assert
            models.ShouldNotBeNull();
            error.ShouldBe(200); // Check to ensure error is not null
        }

        [Fact]
        public async Task GetAbsencesByPerson_ReturnsErrorCode_WhenApiCallFails()
        {
            // Arrange
            var personGuid = Guid.NewGuid();

            var absenceModels = new List<AbsenceModel>
            {
                new AbsenceModel()
            };

            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            mockAbsenceProxy
                .Setup(m => m.ApiPeoplePersonGuidAbsenceGetAsync(personGuid, DateTime.Now.Year, 1, 1000, ApprovalStatus.All, new DateTime(), new DateTime(), 0, default))
                .ThrowsAsync(new ApiException(404, "error"));

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);

            var mockRepo = new Mock<IAbsenceAppRepository>();
            mockRepo.Setup(m => m.GetAbsencesByPersonAsync(personGuid, DateTime.Now.Year, 1, 1000, ApprovalStatus.All, new DateTime(), new DateTime()))
                .ReturnsAsync((absenceModels, 200, 1));

            // Inject the mocked IAbsenceApi
            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var (models, error, count) = await repository.GetAbsencesByPersonAsync(personGuid, DateTime.Now.Year, 1, 1000, ApprovalStatus.All, new DateTime(), new DateTime());

            // Assert
            Assert.Equal(404, error);
        }


        // AbsenceType

        [Fact]
        public void SetTypesTextByGuid_SetsAbsenceType_WhenMatchingTypeGuidExists()
        {
            // Arrange
            var model = new AbsenceModel() { AbsenceTypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c") };
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);
            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            repository.SetTypeTextByGuidAsync(model);

            // Assert
            model.ShouldNotBeNull();
            model.AbsenceType.ShouldBe("Other");
        }

        [Fact]
        public async Task SetTypesTextByGuidOvveride_SetsAbsenceType_WhenMatchingTypeGuidExists()
        {
            // Arrange
            var model = new AbsenceAdminModel() { AbsenceTypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c") };
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);
            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            repository.SetTypeTextByGuidAsync(model);

            // Assert
            model.ShouldNotBeNull();
            model.AbsenceType.ShouldBe("Other");
        }

        [Fact]
        public void SetHistoryTypesTextByGuid_SetsAbsenceType_WhenMatchingTypeGuidExists()
        {
            // Arrange
            var model = new AbsenceHistoryModel() { AbsenceTypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c") };
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);
            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            repository.SetHistoryTypeTextByGuidAsync(model);

            // Assert
            model.ShouldNotBeNull();
            model.AbsenceType.ShouldBe("Other");
        }



        //delete
        [Fact]
        public async Task DeleteAbsence_ReturnsTrue_When_ApiCallSucceeds()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceGuid = Guid.NewGuid();

            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            mockAbsenceProxy
                .Setup(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdDeleteAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), 0, default))
                .Returns(Task.CompletedTask);

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);


            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var result = await repository.DeleteAbsenceAsync(personGuid, absenceGuid, It.IsAny<Guid>());

            // Assert
            Assert.True(result);
            mockAbsenceProxy.Verify(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdDeleteAsync(personGuid, absenceGuid, It.IsAny<Guid>(), 0, default), Times.Once);
        }


        [Fact]
        public async Task DeleteAbsence_ReturnsFalse_When_ApiException()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceGuid = Guid.NewGuid();

            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            mockAbsenceProxy
                .Setup(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdDeleteAsync(personGuid, absenceGuid, It.IsAny<Guid>(), 0, default))
                .ThrowsAsync(new ApiException());


            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);


            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);


            // Act
            var result = await repository.DeleteAbsenceAsync(personGuid, absenceGuid, It.IsAny<Guid>());

            // Assert
            Assert.False(result);
            mockAbsenceProxy.Verify(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdDeleteAsync(personGuid, absenceGuid, It.IsAny<Guid>(), 0, default), Times.Once);
        }

        //update
        [Fact]
        public async Task UpdateAbsence_ReturnsTrue_When_ApiCallSucceeds()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceId = Guid.NewGuid();
            var updatedModel = new AbsenceModel();

            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            mockAbsenceProxy
                .Setup(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdPutAsync(personGuid, absenceId, It.IsAny<Guid>(), It.IsAny<AbsenceUpdateModel>(), 0, default))
                .Returns(Task.CompletedTask);

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);

            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var result = await repository.UpdateAbsenceAsync(personGuid, absenceId, updatedModel, It.IsAny<Guid>());

            // Assert
            Assert.True(result);
            mockAbsenceProxy.Verify(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdPutAsync(personGuid, absenceId, It.IsAny<Guid>(), It.IsAny<AbsenceUpdateModel>(), 0, default), Times.Once);
        }

        [Fact]
        public async Task UpdateAbsence_ReturnsFalse_When_ApiCallThrowsException()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceId = Guid.NewGuid();
            var updatedModel = new AbsenceModel();

            var mockAbsenceProxy = new Mock<IAbsenceApi>();
            mockAbsenceProxy
                .Setup(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdPutAsync(personGuid, absenceId, It.IsAny<Guid>(), It.IsAny<AbsenceUpdateModel>(), 0, default))
                .ThrowsAsync(new ApiException());

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);

            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var result = await repository.UpdateAbsenceAsync(personGuid, absenceId, updatedModel, It.IsAny<Guid>());

            // Assert
            Assert.False(result);
            mockAbsenceProxy.Verify(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdPutAsync(personGuid, absenceId, It.IsAny<Guid>(), It.IsAny<AbsenceUpdateModel>(), 0, default), Times.Once);
        }

        // Test case
        public class AbsenceAppRepositoryTests
        {
            [Fact]
            public async Task SubmitAllDraft_ReturnsTrue_When_ExistingDraftAbsencesSubmitted()
            {
                // Arrange
                var personGuid = Guid.NewGuid();
                var actionBy = Guid.NewGuid();

                // Mock existing draft absences
                var existingAbsences = new List<AbsenceResponseModel>()
        {
            new AbsenceResponseModel() { AbsenceGuid = Guid.NewGuid(), PersonGuid = personGuid, ApprovalStatus = ApprovalStatus.Draft },
            new AbsenceResponseModel() { AbsenceGuid = Guid.NewGuid(), PersonGuid = personGuid, ApprovalStatus = ApprovalStatus.Draft }
        };

                var mockAbsenceProxy = new Mock<IAbsenceApi>();

                // Mock the Get method
                mockAbsenceProxy
                    .Setup(m => m.ApiPeoplePersonGuidAbsenceGetAsync(personGuid, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ApprovalStatus>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0, default))
                    .Returns(Task.FromResult(new AbsenceResponseModels()
                    {
                        AllDataCount = existingAbsences.Count,
                        Absences = existingAbsences
                    }));

                // Mock the Put method
                foreach (var absence in existingAbsences)
                {
                    var updatedAbsence = new AbsenceUpdateModel()
                    {
                        ApprovalStatus = ApprovalStatus.Submitted,
                        SubmissionDate = DateTime.Now
                    };

                    mockAbsenceProxy
                        .Setup(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdPutAsync(personGuid, absence.AbsenceGuid, actionBy, updatedAbsence, 0, default))
                        .Returns(Task.CompletedTask);
                }


                var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
                var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
                mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);


                var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

                // Act
                var result = await repository.SubmitAllDraftAsync(personGuid, actionBy);

                // Assert
                Assert.True(result);
                foreach (var absence in existingAbsences)
                {
                    mockAbsenceProxy.Verify(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdPutAsync(personGuid, absence.AbsenceGuid, actionBy, It.IsAny<AbsenceUpdateModel>(), 0, default), Times.Once);
                }
            }

            [Fact]
            public async Task SubmitAllDraft_ReturnsFalse_When_GetMethodThrowsApiException()
            {
                // Arrange
                var personGuid = Guid.NewGuid();
                var actionBy = Guid.NewGuid();

                var mockAbsenceProxy = new Mock<IAbsenceApi>();

                // Mock the Get method to throw an ApiException
                mockAbsenceProxy
                    .Setup(m => m.ApiPeoplePersonGuidAbsenceGetAsync(personGuid, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ApprovalStatus>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0, default))
                    .Throws(new ApiException(404, "Error"));


                var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
                var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
                mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);

                var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

                // Act
                var result = await repository.SubmitAllDraftAsync(personGuid, actionBy);

                // Assert
                Assert.False(result);
            }

            [Fact]
            public async Task SubmitAllDraft_ReturnsTrue_When_NoExistingDraftAbsences()
            {
                // Arrange
                var personGuid = Guid.NewGuid();
                var actionBy = Guid.NewGuid();

                var existingAbsences = new List<AbsenceResponseModel>(); // Empty list

                var mockAbsenceProxy = new Mock<IAbsenceApi>();

                // Mock the Get method
                mockAbsenceProxy
                    .Setup(m => m.ApiPeoplePersonGuidAbsenceGetAsync(personGuid, It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<ApprovalStatus>(), It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0, default))
                    .Returns(Task.FromResult(new AbsenceResponseModels()
                    {
                        AllDataCount = existingAbsences.Count,
                        Absences = existingAbsences
                    }));

                var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
                var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
                mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);


                var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

                // Act
                var result = await repository.SubmitAllDraftAsync(personGuid, actionBy);

                // Assert
                Assert.True(result);
                mockAbsenceProxy.Verify(m => m.ApiPeoplePersonGuidAbsenceAbsenceIdPutAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<AbsenceUpdateModel>(), It.IsAny<int>(), default), Times.Never);
            }
        }






        [Fact]
        public async Task CheckAbsenceOverlapAsync_ReturnsTrue_WhenOverlapDetected()
        {
            // Arrange

            var personGuid = Guid.NewGuid();
            var startDate = new DateTime(2023, 5, 1);
            var endDate = new DateTime(2023, 5, 10);

            var absenceModels = new List<AbsenceModel>
            {
                new AbsenceModel()
            };


            var absenceResponseMock = new AbsenceResponseModels()
            {
                Absences = new List<ProxyModel.AbsenceResponseModel>()
                {
                     new AbsenceResponseModel { AbsenceStart = new DateTime(2023, 4, 30), AbsenceEnd = new DateTime(2023, 5, 2), PersonGuid =  personGuid, ApprovalStatus= ApprovalStatus.Draft},
                    new AbsenceResponseModel { AbsenceStart = new DateTime(2023, 5, 5), AbsenceEnd = new DateTime(2023, 5, 8), PersonGuid = personGuid, ApprovalStatus= ApprovalStatus.Draft}
                },
                AllDataCount = 2
            };

            var absenceTypeResponseMock = new List<ProxyModel.AbsenceTypeResponseModel>();

            var mockAbsenceProxy = new Mock<IAbsenceApi>();


            mockAbsenceProxy
               .Setup(m => m.ApiPeoplePersonGuidAbsenceGetAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), ApprovalStatus.All, It.IsAny<DateTime>(), It.IsAny<DateTime>(), 0, default))
               .Returns(Task.FromResult(absenceResponseMock));

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy
                .Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default))
                .ReturnsAsync(absenceTypeResponseMock);


            var repository = new AbsenceAppRepository(mockAbsenceProxy.Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var result = await repository.CheckAbsenceOverlapAsync(personGuid, startDate, endDate);


            // Assert
            Assert.True(result);
        }


        //Admin

        [Fact]
        public async Task GetAllAbsences_ReturnsAbsenceModel_WhenApiCallSucceeds()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var absenceModel = new AbsenceModel();
            var absenceResponseMock = new AbsenceResponseModels()
            {
                Absences = new List<AbsenceResponseModel>() { new AbsenceResponseModel() }
            };
            var absenceTypeResponseMock = new List<ProxyModel.AbsenceTypeResponseModel>();

            var mockAdminProxy = new Mock<IAdminApi>();
            mockAdminProxy
                .Setup(m => m.ApiAbsenceGetAsync(1, 15, ApprovalStatus.All, 0, default))
                .ReturnsAsync(absenceResponseMock);

            var mockRepo = new Mock<IAbsenceAppRepository>();
            mockRepo.Setup(m => m.GetAbsenceByPersonAsync(personGuid, absenceModel.AbsenceGuid))
                .ReturnsAsync((absenceModel, 200));

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            mockAbsenceTypeProxy
                .Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default))
                .ReturnsAsync(absenceTypeResponseMock);

            var repository = new AbsenceAppRepository(new Mock<IAbsenceApi>().Object, mockAbsenceTypeProxy.Object, mockAdminProxy.Object, new Mock<ICollaboratorApi>().Object);

            // Act
            var (models, errorCode, count) = await repository.GetAllAbsencesAsync(1, 15, ApprovalStatus.All);

            // Assert
            models.ShouldNotBeNull();
            errorCode.ShouldBe(200);
            count.ShouldBe(absenceResponseMock.AllDataCount);
        }

        [Fact]
        public async Task GetAllPeople_ReturnsCollaboratorsAndCount()
        {
            // Arrange
            var filter = string.Empty;

            var expectedCollaborators = new List<PeopleProxyModel.ApiCollaboratorAllResponseModel>
            {
                 new PeopleProxyModel.ApiCollaboratorAllResponseModel
                {
                    FirstName = "Dinis", LastName = "Godinho", Email = "dgodinho@mail.com", Status="Active"
                },
                new PeopleProxyModel.ApiCollaboratorAllResponseModel
                {
                    FirstName = "Joao", LastName = "Monteiro", Email = "jmonteiro@mail.com", Status="Inactive"
                }
            };

            var expectedTotalCount = expectedCollaborators.Count;

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(It.IsAny<int>(), It.IsAny<int>(), filter, PeopleProxyModel.State.All, 0, default))
                .ReturnsAsync(new PeopleProxyModel.ApiCollaboratorResponseWithCount
                {
                    Collaborators = expectedCollaborators,
                    TotalCount = expectedTotalCount
                });

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);


            var repository = new AbsenceAppRepository(new Mock<IAbsenceApi>().Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, mockCollaboratorProxy.Object);

            // Act
            var (collaborators, count) = await repository.GetAllPeople(It.IsAny<int>(), It.IsAny<int>(), filter, State.All);

            // Assert
            Assert.Equal(expectedCollaborators.Count, collaborators.Count);
            Assert.Equal(expectedTotalCount, count);
        }


        [Fact]
        public async Task GetAllPeople_ReturnsEmptyListAndZeroCount()
        {
            // Arrange
            var filter = string.Empty;

            var mockCollaboratorProxy = new Mock<ICollaboratorApi>();
            mockCollaboratorProxy.Setup(p => p.GetAllCollaboratorsAsync(It.IsAny<int>(), It.IsAny<int>(), filter, PeopleProxyModel.State.All, 0, default))
                .ReturnsAsync(new PeopleProxyModel.ApiCollaboratorResponseWithCount
                {
                    Collaborators = new List<PeopleProxyModel.ApiCollaboratorAllResponseModel>(),
                    TotalCount = 0
                });

            var mockAbsenceTypeProxy = new Mock<IAbsenceTypeApi>();
            var types = new List<ProxyModel.AbsenceTypeResponseModel>() { new ProxyModel.AbsenceTypeResponseModel() { TypeGuid = new Guid("997c9937-6f20-4572-5f9d-08db47ddff9c"), Type = "Other" } };
            mockAbsenceTypeProxy.Setup(m => m.ApiAbsenceTypeTypesGetAsync(0, default)).ReturnsAsync(types);


            var repository = new AbsenceAppRepository(new Mock<IAbsenceApi>().Object, mockAbsenceTypeProxy.Object, new Mock<IAdminApi>().Object, mockCollaboratorProxy.Object);

            // Act
            var (collaborators, count) = await repository.GetAllPeople(It.IsAny<int>(), It.IsAny<int>(), filter, State.All);

            // Assert
            Assert.Empty(collaborators);
            Assert.Equal(0, count);
        }

    }

}
