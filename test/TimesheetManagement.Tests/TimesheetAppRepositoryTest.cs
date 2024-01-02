using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.API.Extensions;
using MainHub.Internal.PeopleAndCulture.TimesheetManagement.Repository;
using Microsoft.Graph;
using Moq;
using Shouldly;
using TimesheetManagement.Api.Proxy.Client.Api;
using TimesheetManagement.Api.Proxy.Client.Client;
using TimesheetManagement.Api.Proxy.Client.Model;
using ProxyModel = TimesheetManagement.Api.Proxy.Client.Model;
using MainHub.Internal.PeopleAndCulture.Extensions;
using TimesheetActivityUpdateModel = TimesheetManagement.Api.Proxy.Client.Model.TimesheetActivityUpdateModel;
using System.Drawing;
using ProjectManagement.Api.Proxy.Client.Api;
using ProjectProxyModel = ProjectManagement.Api.Proxy.Client.Model;

namespace MainHub.Internal.PeopleAndCulture
{
    public class TimesheetAppRepositoryTest
    {
        [Fact]
        public void Constructor_InjectsProxies()
        {
            //Arrange
            var mockTimesheetProxy = new Mock<ITimesheetApi>();
            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            //Act
            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Assert
            Assert.Equal(mockTimesheetProxy.Object, repository.TimesheetProxy);
            Assert.Equal(mockTimesheetActivityProxy.Object, repository.TimesheetActivityProxy);
        }


        //Creates
        [Fact]
        public void CreatedTimesheet_Successfully()
        {
            //Arrange
            var timesheetModel = new TimesheetModel();

            var mockTimesheetProxy = new Mock<ITimesheetApi>();
            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            var responseModel = new TimesheetCreateResponseModel()
            {
                TimesheetGUID = Guid.Empty,
                PersonGUID = Guid.Empty,
                Month = 0,
                Year = 0,
                ApprovalStatus = ProxyModel.ApprovalStatus.Draft,
                ApprovedBy = "None",
                DateOfSubmission = new DateTime(2999, 12, 31, 23, 59, 59),
                DateOfApproval = DateTime.Now
            };

            mockTimesheetProxy.Setup(p => p.ApiPeoplePersonGuidTimesheetPostAsync(responseModel.PersonGUID, It.IsAny<Guid>(), It.IsAny<TimesheetCreateRequestModel>(), 0, default))
                     .ReturnsAsync(responseModel);

            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //ActCreateTimesheet
            var result = repository.CreateTimesheet(timesheetModel, It.IsAny<Guid>());

            //Assert
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task CreatedTimesheetActivities_Successfully()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();
            var actionBy = Guid.NewGuid();

            var tarm = new TimesheetActivityRecordsModel();

            var tams = tarm.ToTimesheetActivitiesModel();

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            var repository = new TimesheetAppRepository(new Mock<ITimesheetApi>().Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            foreach (var model in tams)
            {
                if (model.TimesheetActivityGUID == Guid.Empty && model.Hours > 0)
                {
                    var timesheetActivityRequest = model.ToTimesheetActivityCreateRequestModel();

                    var responseModel = new TimesheetActivityCreateResponseModel
                    {
                        TimesheetActivityGUID = model.TimesheetActivityGUID,
                        ActivityGUID = model.ActivityGUID,
                        TimesheetGUID = model.TimesheetGUID,
                        ProjectGUID = model.ProjectGUID,
                        TypeOfWork = (TypeOfWork?)model.TypeOfWork,
                        ActivityDate = model.ActivityDate,
                        Hours = model.Hours
                    };

                    //Mock create
                    mockTimesheetActivityProxy
                        .Setup(p => p.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityPostAsync(personGuid, model.TimesheetGUID, personGuid, timesheetActivityRequest, 0, default))
                        .ReturnsAsync(responseModel);
                }
                else if (model.TimesheetActivityGUID != Guid.Empty && model.Hours > 0)
                {
                    //Mock update
                    var updatedModel = model.ToTimesheetActivityUpdateModel();
                    mockTimesheetActivityProxy
                        .Setup(p => p.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidPutAsync(personGuid, model.TimesheetGUID, model.TimesheetActivityGUID, actionBy, updatedModel, 0, default));
                }
                else if (model.TimesheetActivityGUID != Guid.Empty && model.Hours == 0)
                {
                    //Mock delete
                    mockTimesheetActivityProxy
                        .Setup(p => p.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidDeleteAsync(personGuid, model.TimesheetGUID, model.TimesheetActivityGUID, actionBy, 0, default));
                }
            }

            //Act
            var result = await repository.CreateActivities(personGuid, timesheetActivityGuid, actionBy, tarm);

            result.ShouldNotBeNull();
        }



        //Gets
        [Fact]
        public async Task GetAllTimesheetsFromOnePerson_ReturnsTimesheetModels_WhenApiCallSucceeds()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var year = 0;
            var month = 0;
            var status = ApprovalStatus.All;

            var timesheetModels = new List<TimesheetModel>
            {
                new TimesheetModel()
            };

            var timesheetResponseMock = new TimesheetResponseModels()
            {
                Timesheets = new List<ProxyModel.TimesheetResponseModel>()
                {
                    new ProxyModel.TimesheetResponseModel()
                },
                AllDataCount = 1
            };

            var timesheetActivityResponseMock = new List<ProxyModel.TimesheetActivityResponseModel>();

            var mockTimesheetProxy = new Mock<ITimesheetApi>();

            mockTimesheetProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetGetAsync(personGuid, year, month, status, 1, 20, 0, default))
                .ReturnsAsync(timesheetResponseMock);

            var mockRepo = new Mock<ITimesheetAppRepository>();
            mockRepo.Setup(m => m.GetAllTimesheetsFromOnePerson(personGuid, year, month, status, 1, 15))
                .ReturnsAsync((timesheetModels, 200, 1));

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            //Inject the mocked ITimesheetApi
            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var (models, error, count) = await repository.GetAllTimesheetsFromOnePerson(personGuid, year, month, status, 1, 20);

            //Assert
            models.ShouldNotBeNull();
            error.ShouldBe(200);
        }

        [Fact]
        public async Task GetAllTimesheetsFromOnePerson_ReturnsErrorCode_WhenApiCallFails()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var year = 0;
            var month = 0;
            var status = ApprovalStatus.All;

            var timesheetModels = new List<TimesheetModel>
            {
                new TimesheetModel()
            };

            var mockTimesheetProxy = new Mock<ITimesheetApi>();
            mockTimesheetProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetGetAsync(personGuid, year, month, status, 1, 20, 0, default))
                .ThrowsAsync(new ApiException(404, "error"));

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            var mockRepo = new Mock<ITimesheetAppRepository>();
            mockRepo.Setup(m => m.GetAllTimesheetsFromOnePerson(personGuid, year, month, status, 1, 20))
                .ReturnsAsync((timesheetModels, 200, 1));

            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var (models, error, count) = await repository.GetAllTimesheetsFromOnePerson(personGuid, year, month, status, 1, 20);

            //Assert
            Assert.Equal(404, error);
        }

        [Fact]
        public async Task GetOneTimesheetFromOnePerson_ReturnsTimesheetModel_WhenApiCallSucceeds()
        {
            //Arrange
            var personGuid = Guid.NewGuid();

            var timesheetModel = new TimesheetModel();

            var timesheetResponseMock = new ProxyModel.TimesheetResponseModel();

            var timesheetActivityResponseMock = new List<ProxyModel.TimesheetActivityResponseModel>();

            var mockTimesheetProxy = new Mock<ITimesheetApi>();

            mockTimesheetProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidGetAsync(personGuid, Guid.Empty, 0, default))
                .ReturnsAsync(timesheetResponseMock);

            var mockRepo = new Mock<ITimesheetAppRepository>();
            mockRepo.Setup(m => m.GetOneTimesheetFromOnePerson(personGuid, timesheetModel.TimesheetGUID))
                .ReturnsAsync(timesheetModel);

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            //Inject the mocked ITimesheetApi
            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var models = await repository.GetOneTimesheetFromOnePerson(personGuid, timesheetModel.TimesheetGUID);

            //Assert
            models.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetAllTimesheetActivitiesForOneTimesheet_ReturnsTimesheetActivityModels_WhenApiCallSucceeds()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();

            var timesheetActivityModels = new List<TimesheetActivityModel>
            {
                new TimesheetActivityModel()
            };

            var timesheetActivityResponseMock = new TimesheetActivityResponseModels()
            {
                Activities = new List<ProxyModel.TimesheetActivityResponseModel>()
                {
                    new ProxyModel.TimesheetActivityResponseModel()
                },
                AllDataCount = 1
            };

            var timesheetResponseMock = new List<ProxyModel.TimesheetResponseModel>();

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            mockTimesheetActivityProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityGetAsync(personGuid, timesheetGuid, 0, default))
                .ReturnsAsync(timesheetActivityResponseMock);

            var mockRepo = new Mock<ITimesheetAppRepository>();
            mockRepo.Setup(m => m.GetAllTimesheetActivitiesForOneTimesheet(personGuid, timesheetGuid))
                .ReturnsAsync((timesheetActivityModels, 200, 1));

            var mockTimesheetProxy = new Mock<ITimesheetApi>();

            //Inject the mocked ITimesheetActivityApi
            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var (models, error, count) = await repository.GetAllTimesheetActivitiesForOneTimesheet(personGuid, timesheetGuid);

            //Assert
            models.ShouldNotBeNull();
            error.ShouldBe(200);
        }

        [Fact]
        public async Task GetAllTimesheetActivitiesForOneTimesheet_ReturnsErrorCode_WhenApiCallFails()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();

            var timesheetActivityModels = new List<TimesheetActivityModel>
            {
                new TimesheetActivityModel()
            };

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();
            mockTimesheetActivityProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityGetAsync(personGuid, timesheetGuid, 0, default))
                .ThrowsAsync(new ApiException(404, "error"));

            var mockTimesheetProxy = new Mock<ITimesheetApi>();

            var mockRepo = new Mock<ITimesheetAppRepository>();
            mockRepo.Setup(m => m.GetAllTimesheetActivitiesForOneTimesheet(personGuid, timesheetGuid))
                .ReturnsAsync((timesheetActivityModels, 200, 1));

            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var (models, error, count) = await repository.GetAllTimesheetActivitiesForOneTimesheet(personGuid, timesheetGuid);

            //Assert
            Assert.Equal(404, error);
        }

        [Fact]
        public async Task GetOneTimesheetActivityForOneTimesheet_ReturnsTimesheetModel_WhenApiCallSucceeds()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();

            var timesheetActivityModel = new TimesheetActivityModel();

            var timesheetActivityResponseMock = new ProxyModel.TimesheetActivityResponseModel();

            var timesheetResponseMock = new TimesheetResponseModels()
            {
                Timesheets = new List<ProxyModel.TimesheetResponseModel>()
                {
                    new ProxyModel.TimesheetResponseModel()
                },
                AllDataCount = 1
            };

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            mockTimesheetActivityProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidGetAsync(personGuid, timesheetGuid, timesheetActivityGuid, 0, default))
                .ReturnsAsync(timesheetActivityResponseMock);

            var mockRepo = new Mock<ITimesheetAppRepository>();
            mockRepo.Setup(m => m.GetOneTimesheetActivityForOneTimesheet(personGuid, timesheetGuid, timesheetActivityGuid))
                .ReturnsAsync(timesheetActivityModel);

            var mockTimesheetProxy = new Mock<ITimesheetApi>();
            mockTimesheetProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetGetAsync(personGuid, DateTime.Now.Year, DateTime.Now.Month, ProxyModel.ApprovalStatus.Draft, 1, 20, 0, default))
                .ReturnsAsync(timesheetResponseMock);

            //Inject the mocked ITimesheetApi
            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var models = await repository.GetOneTimesheetActivityForOneTimesheet(personGuid, timesheetGuid, timesheetActivityGuid);

            //Assert
            models.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetProjectsForUser_ReturnsListOfProjects()
        {
            //Arrange
            var personGuid = Guid.NewGuid();

            var projectModels = new List<ProjectProxyModel.Project>
            {
                new ProjectProxyModel.Project { }
            };

            var mockProjectProxy = new Mock<IProjectApi>();
            mockProjectProxy.Setup(p => p.ProjectGetAsync(0, default))
                .ReturnsAsync(projectModels);

            //Inject the mocked ITimesheetActivityApi
            var repository = new TimesheetAppRepository(new Mock<ITimesheetApi>().Object, new Mock<ITimesheetActivityApi>().Object, new Mock<IAdminApi>().Object, mockProjectProxy.Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var models = await repository.GetProjectsForUser(personGuid);

            //Assert
            models.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetProjectActivities_ReturnsListOfActivities()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var projectGuid = Guid.NewGuid();

            var projectActivityModels = new List<ProjectProxyModel.ProjectActivity>
            {
                new ProjectProxyModel.ProjectActivity { }
            };

            var mockProjectActivityProxy = new Mock<IProjectActivityApi>();
            mockProjectActivityProxy.Setup(p => p.ProjectActivityGetAsync(0, default))
                .ReturnsAsync(projectActivityModels);

            //Inject the mocked ITimesheetActivityApi
            var repository = new TimesheetAppRepository(new Mock<ITimesheetApi>().Object, new Mock<ITimesheetActivityApi>().Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, mockProjectActivityProxy.Object);

            //Act
            var models = await repository.GetProjectActivities(personGuid, projectGuid);

            //Assert
            models.ShouldNotBeNull();
        }

        [Fact]
        public async Task GetTimesheetActivityRecords_ReturnsTimesheetActivityRecords_WhenApiCallSucceeds()
        {
            // Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();

            var timesheetActivityModels = new List<TimesheetActivityModel>
            {
                new TimesheetActivityModel()
            };

            var timesheetActivityResponseMock = new TimesheetActivityResponseModels()
            {
                Activities = new List<TimesheetActivityResponseModel>()
                {
                    new TimesheetActivityResponseModel()
                },
                AllDataCount = 1
            };

            var timesheetActivityRecords = new List<TimesheetActivityRecordsModel>
            {
                new TimesheetActivityRecordsModel()
            };

            var timesheetResponseMock = new List<TimesheetResponseModel>();

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            mockTimesheetActivityProxy
                    .Setup(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityGetAsync(personGuid, timesheetGuid, 0, default))
                    .ReturnsAsync(timesheetActivityResponseMock);

            var mockRepo = new Mock<ITimesheetAppRepository>();
            mockRepo.Setup(m => m.GetTimesheetActivityRecords(personGuid, timesheetGuid))
                .ReturnsAsync((timesheetActivityRecords, 200, 1));

            var mockTimesheetProxy = new Mock<ITimesheetApi>();

            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var (models, error, count) = await repository.GetTimesheetActivityRecords(personGuid, timesheetGuid);

            //Assert
            models.ShouldNotBeNull();
            error.ShouldBe(200);
        }

        [Fact]
        public async Task GetTimesheetActivityRecords_ReturnsErrorCode_WhenApiCallFails()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();
            mockTimesheetActivityProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityGetAsync(personGuid, timesheetGuid, 0, default))
                .ThrowsAsync(new ApiException(404, "error"));

            var repository = new TimesheetAppRepository(new Mock<ITimesheetApi>().Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var (activityRecords, error, count) = await repository.GetTimesheetActivityRecords(personGuid, timesheetGuid);

            //Assert
            Assert.Equal(404, error);
            Assert.Empty(activityRecords);
            Assert.Equal(0, count);
        }



        //Deletes
        [Fact]
        public async Task DeleteTimesheetActivity_ReturnsTrue_When_ApiCallSucceeds()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();
            mockTimesheetActivityProxy
                .Setup(t => t.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidDeleteAsync(personGuid, timesheetGuid, timesheetActivityGuid, It.IsAny<Guid>(), 0, default))
                .Returns(Task.CompletedTask);

            var mockTimesheetProxy = new Mock<ITimesheetApi>();

            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var result = await repository.DeleteTimesheetActivityAsync(personGuid, timesheetGuid, timesheetActivityGuid, It.IsAny<Guid>());

            //Assert
            Assert.True(result);
            mockTimesheetActivityProxy.Verify(t => t.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidDeleteAsync(personGuid, timesheetGuid, timesheetActivityGuid, It.IsAny<Guid>(), 0, default), Times.Once);
        }


        [Fact]
        public async Task DeleteTimesheetActivity_ReturnsFalse_When_ApiException()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetActivityGuid = Guid.NewGuid();

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();
            mockTimesheetActivityProxy
                .Setup(t => t.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidDeleteAsync(personGuid, timesheetGuid, timesheetActivityGuid, It.IsAny<Guid>(), 0, default))
                .ThrowsAsync(new ApiException());

            var mockTimesheetProxy = new Mock<ITimesheetApi>();

            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var result = await repository.DeleteTimesheetActivityAsync(personGuid, timesheetGuid, timesheetActivityGuid, It.IsAny<Guid>());

            //Assert
            Assert.False(result);
            mockTimesheetActivityProxy.Verify(t => t.ApiPeoplePersonGuidTimesheetTimesheetGuidActivityTimesheetActivityGuidDeleteAsync(personGuid, timesheetGuid, timesheetActivityGuid, It.IsAny<Guid>(), 0, default), Times.Once);
        }



        //Updates
        [Fact]
        public async Task UpdateTimesheet_ReturnsTrue_When_ApiCallSucceeds()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var updatedModel = new TimesheetModel();

            var mockTimesheetProxy = new Mock<ITimesheetApi>();
            mockTimesheetProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidPutAsync(personGuid, timesheetGuid, It.IsAny<Guid>(), It.IsAny<TimesheetUpdateModel>(), 0, default))
                .Returns(Task.CompletedTask);

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var result = await repository.UpdateTimesheetAsync(personGuid, timesheetGuid, updatedModel, It.IsAny<Guid>());

            //Assert
            Assert.True(result);
            mockTimesheetProxy.Verify(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidPutAsync(personGuid, timesheetGuid, It.IsAny<Guid>(), It.IsAny<TimesheetUpdateModel>(), 0, default), Times.Once);
        }

        [Fact]
        public async Task UpdateTimesheet_ReturnsFalse_When_ApiCallThrowsException()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var updatedModel = new TimesheetModel();

            var mockTimesheetProxy = new Mock<ITimesheetApi>();
            mockTimesheetProxy
                .Setup(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidPutAsync(personGuid, timesheetGuid, It.IsAny<Guid>(), It.IsAny<TimesheetUpdateModel>(), 0, default))
                .ThrowsAsync(new ApiException());

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            var repository = new TimesheetAppRepository(mockTimesheetProxy.Object, mockTimesheetActivityProxy.Object, new Mock<IAdminApi>().Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var result = await repository.UpdateTimesheetAsync(personGuid, timesheetGuid, updatedModel, It.IsAny<Guid>());

            //Assert
            Assert.False(result);
            mockTimesheetProxy.Verify(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidPutAsync(personGuid, timesheetGuid, It.IsAny<Guid>(), It.IsAny<TimesheetUpdateModel>(), 0, default), Times.Once);
        }


        //History
        //[Fact]
        //public async Task GetAbsenceHistory_ReturnsAbsenceHistory_WhenApiCallSucceeds()
        //{
        //    //Arrange
        //    var timesheetGuid = Guid.NewGuid();
        //    var personGuid = Guid.NewGuid();
        //    var page = 1;
        //    var pageSize = 10;

        //    var responseModels = new TimesheetHistoryResponseModels()
        //    {
        //        TimesheetHistory = new List<TimesheetHistoryResponseModel>()
        //        {
        //            //Create sample response models as needed for testing
        //            new TimesheetHistoryResponseModel(),
        //            new TimesheetHistoryResponseModel()
        //        },
        //        AllDataCount = 20 //Sample count value
        //    };

        //    var timesheetProxyMock = new Mock<ITimesheetApi>();
        //    timesheetProxyMock.Setup(m => m.ApiPeoplePersonGuidTimesheetTimesheetGuidHistoryGetAsync(timesheetGuid, personGuid.ToString(), page, pageSize, 0, default))
        //        .ReturnsAsync(responseModels);



        //    var repository = new TimesheetAppRepository(timesheetProxyMock.Object, new Mock<ITimesheetActivityApi>().Object, new Mock<IAdminApi>().Object, new Mock<ICollaboratorApi>().Object);

        //    //Act
        //    var (models, errorCode, count) = await repository.GetTimesheetHistory(timesheetGuid, personGuid.ToString(), page, pageSize);

        //    //Assert
        //    Assert.Equal(responseModels.TimesheetHistory.Count, models.Count);
        //    Assert.Equal(200, errorCode);
        //    Assert.Equal(responseModels.AllDataCount, count);
        //}


        //Admin
        [Fact]
        public async Task GetAllTimesheets_ReturnsTimesheetModel_WhenApiCallSucceeds()
        {
            //Arrange
            var personGuid = Guid.NewGuid();
            var timesheetGuid = Guid.NewGuid();
            var timesheetModel = new TimesheetModel();
            var timesheetResponseMock = new TimesheetResponseModels()
            {
                Timesheets = new List<TimesheetResponseModel>() { new TimesheetResponseModel() }
            };

            var mockTimesheetActivityProxy = new Mock<ITimesheetActivityApi>();

            var mockAdminProxy = new Mock<IAdminApi>();
            mockAdminProxy
                .Setup(m => m.ApiTimesheetGetAsync(1, 15, ApprovalStatus.All, 0, default))
                .ReturnsAsync(timesheetResponseMock);

            var mockRepo = new Mock<ITimesheetAppRepository>();
            mockRepo.Setup(m => m.GetOneTimesheetFromOnePerson(personGuid, timesheetModel.TimesheetGUID))
                .ReturnsAsync(timesheetModel);

            //Inject the mocked IAdminApi
            var repository = new TimesheetAppRepository(new Mock<ITimesheetApi>().Object, mockTimesheetActivityProxy.Object, mockAdminProxy.Object, new Mock<IProjectApi>().Object, new Mock<IProjectActivityApi>().Object);

            //Act
            var (models, errorCode, count) = await repository.GetAllTimesheetsAsync(1, 15, ApprovalStatus.All);

            //Assert
            models.ShouldNotBeNull();
            errorCode.ShouldBe(200);
            count.ShouldBe(timesheetResponseMock.AllDataCount);
        }
    }
}
