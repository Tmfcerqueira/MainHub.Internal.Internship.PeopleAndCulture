using PartnerManagement.Api.Models.API_Contact;
using PartnerManagement.Api.Models.API_Partners;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Controllers;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Partners.PCreate;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.ContactExtensions.PContactCreate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using PartnerManagement.DataBase;
using PartnerManagement.Repository.Models;
using Shouldly;
using MainHub.Internal.PeopleAndCulture;
using Microsoft.AspNetCore.Http;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.Api.Models;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Miscellaneous;
using PartnerManagement.Api.Models;
using MainHub.Internal.PeopleAndCulture.PartnerManagement.API.Extensions.Partners;
using AngleSharp.Dom;
using System;
using MainHub.Internal.PeopleAndCulture.Models;

namespace PartnerManagement.Tests
{
    public class PartnerControllerTests
    {
        private readonly PartnerController _partnerController;
        private readonly Mock<IPartnerRepository> _partnerRepositoryMock;

        public PartnerControllerTests()
        {
            // Inicializa o repositório Partner (fictício|mock) e o injeta no controller
            _partnerRepositoryMock = new Mock<IPartnerRepository>();
            _partnerController = new PartnerController(_partnerRepositoryMock.Object);
        }

        //CREATE TESTS
        [Fact]
        public async Task PostCreatePartner_Should_Create_Partner_With_Correct_Data()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;

            using var dbContext = new PartnerManagementDBContext(options);

            var partnerRepositoryMock = new Mock<IPartnerRepository>();

            var model = new ApiPartnerCreateRequestModel
            {
                Name = "Test Partner",
                Address = "Test Address",
                PostalCode = "12345",
                Country = "Test Country",
                TaxNumber = "123456789",
                ServiceDescription = "Test Service",
            };

            var repoModel = model.ToPartnerRepoModel();
            partnerRepositoryMock.Setup(x => x.Create_Partner_Async(It.IsAny<PartnerRepoModel>()))
                .Returns(Task.FromResult(new PartnerRepoModel { PartnerGUID = Guid.Empty }));

            var controller = new PartnerController(partnerRepositoryMock.Object);

            //Act   
            var result = await controller.Post_Create_Partner(model);

            //Assert
            result.ShouldBeOfType<ActionResult<ApiPartnerCreateResponseModel>>();
            result.ShouldNotBeNull();
        }

        [Fact]
        public async Task PostCreateContact_Should_Create_Contact_With_Correct_Data()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;

            using var dbContext = new PartnerManagementDBContext(options);

            var contactRepositoryMock = new Mock<IPartnerRepository>();

            var model = new ApiContactCreateRequestModel
            {
                Name = "John Doe",
                Email = "johndoe@example.com",
                Role = "Manager",
                PhoneNumber = "1234567890",
                Department = "Sales"
            };

            var repoModel = model.ToContactRepoModel();
            contactRepositoryMock.Setup(x => x.Create_Contact_Async(It.IsAny<ContactRepoModel>()))
                .Returns(Task.FromResult(new ContactRepoModel { ContactGUID = Guid.Empty }));

            var controller = new PartnerController(contactRepositoryMock.Object);

            //Act   
            var result = await controller.Post_Create_Contact(repoModel.PartnerGUID, model);

            //Assert
            result.ShouldBeOfType<ActionResult<ApiContactCreateResponseModel>>();
            result.ShouldNotBeNull();
        }
        [Fact]
        public async Task PostCreatePartner_Should_Return_BadRequest_When_Model_Is_Invalid()
        {
            // Arrange
            var controller = new PartnerController(_partnerRepositoryMock.Object);
            controller.ModelState.AddModelError("name", "The Name field is required.");

            var model = new ApiPartnerCreateRequestModel();

            // Act
            var result = await controller.Post_Create_Partner(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }

        [Fact]
        public async Task PostCreateContact_Should_Return_BadRequest_When_Model_Is_Invalid()
        {
            // Arrange
            var controller = new PartnerController(_partnerRepositoryMock.Object);
            controller.ModelState.AddModelError("email", "The Email field is required.");

            var model = new ApiContactCreateRequestModel();

            // Act
            var result = await controller.Post_Create_Contact(Guid.Empty, model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
        }
        //GET TESTS

        [Fact]
        public async Task Get_Specific_Contact_With_Specific_Contact_Guid_From_A_Specific_Partner_Guid_Returns_Ok_If_Found()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();

            var contact = new ContactRepoModel()
            {
                PartnerGUID = partnerGuid,
                ContactGUID = contactGuid
            };

            partnerMockRepository
                .Setup(r => r.Get_Partner_Contacts_By_Guid_Async(partnerGuid, contactGuid))
                .ReturnsAsync(contact);

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Get_Specific_Contact_From_One_Partner(partnerGuid, contactGuid);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiContactResponseModel = Assert.IsType<ApiContactResponseModel>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.Equal(contactGuid, apiContactResponseModel.ContactGUID);
            Assert.Equal(partnerGuid, apiContactResponseModel.PartnerGUID);
        }
        [Fact]
        public async Task Get_Specific_Contact_With_Specific_Contact_Guid_From_A_Specific_Partner_Guid_Returns_NotFound_If_NotFound()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();

            partnerMockRepository
                .Setup(r => r.Get_Partner_Contacts_By_Guid_Async(partnerGuid, contactGuid))
                .Returns(Task.FromResult<ContactRepoModel>(null));

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Get_Specific_Contact_From_One_Partner(partnerGuid, contactGuid);

            // Assert
            var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundObjectResult.StatusCode);
            Assert.Equal("Contact or Partner doesn't exist", notFoundObjectResult.Value);
        }

        [Fact]
        public async Task Get_Partner_By_Guid_Async_Returns_Ok_If_Found()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var partnerGuid = Guid.NewGuid();
            var partner = new PartnerRepoModel { PartnerGUID = partnerGuid, Name = "Test Partner" };

            partnerMockRepository
                .Setup(r => r.Get_Partner_By_Guid_Async(partnerGuid))
                .ReturnsAsync(partner);

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Get_Partner_By_Guid_Async(partnerGuid);

            // Assert
            var okObjectResult = Assert.IsType<OkObjectResult>(result.Result);
            var apiPartnerResponseModel = Assert.IsType<ApiPartnerResponseModel>(okObjectResult.Value);
            Assert.Equal(StatusCodes.Status200OK, okObjectResult.StatusCode);
            Assert.Equal(partnerGuid, apiPartnerResponseModel.PartnerGUID);
            Assert.Equal("Test Partner", apiPartnerResponseModel.Name);
        }
        [Fact]
        public async Task Get_Partner_By_Guid_Async_Returns_NotFound_If_NotFound()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var partnerGuid = Guid.NewGuid();

            partnerMockRepository
                .Setup(r => r.Get_Partner_By_Guid_Async(partnerGuid))
                .Returns(Task.FromResult<PartnerRepoModel>(null));

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Get_Partner_By_Guid_Async(partnerGuid);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Get_All_Partners_Async_ExistingPartners_ReturnsOkResult()
        {
            // Arrange
            var numPage = 1;
            var pageSize = 5;
            var name = "John Doe";

            var partners = new List<PartnerRepoModel>
        {
            new PartnerRepoModel { Name = "John Doe", ServiceDescription = "john@example.com" },
            new PartnerRepoModel { Name = "Jane Smith", ServiceDescription = "jane@example.com" }
        };
            var totalCount = 2;

            _partnerRepositoryMock
                .Setup(r => r.Get_All_Partners_Async(numPage, pageSize, name))
                .ReturnsAsync((partners, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Partners_Async(numPage, pageSize, name);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            var responseModel = okResult.Value as ApiPartnerResponseWithCount;

            Assert.NotNull(responseModel);
            Assert.Equal(partners.Count, responseModel.Partners.Count);
            Assert.Equal(totalCount, responseModel.TotalCount);
        }

        [Fact]
        public async Task Get_All_Partners_Async_NonExistingPartners_ReturnsNotFoundResult()
        {
            // Arrange
            var numPage = 1;
            var pageSize = 5;
            var name = "John Doe";

            var partners = new List<PartnerRepoModel>();
            var totalCount = 0;

            _partnerRepositoryMock
                .Setup(r => r.Get_All_Partners_Async(numPage, pageSize, name))
                .ReturnsAsync((partners, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Partners_Async(numPage, pageSize, name);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }

        [Fact]
        public async Task Get_All_Partners_Async_ExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var numPage = 1;
            var pageSize = 5;
            var name = "John Doe";

            _partnerRepositoryMock
                .Setup(r => r.Get_All_Partners_Async(numPage, pageSize, name))
                .ThrowsAsync(new Exception("Something went wrong."));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Partners_Async(numPage, pageSize, name);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("Something went wrong.", badRequestResult.Value);
        }
        [Fact]
        public async Task Get_All_Partners_Async_NoPartners_ReturnsNotFoundResult()
        {
            // Arrange
            var numPage = 1;
            var pageSize = 5;
            var name = "John Doe";

            var partners = new List<PartnerRepoModel>();
            var totalCount = 0;

            _partnerRepositoryMock
                .Setup(r => r.Get_All_Partners_Async(numPage, pageSize, name))
                .ReturnsAsync((partners, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Partners_Async(numPage, pageSize, name);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task Get_All_Contacts_From_One_Partner_ExistingContact_ReturnsOkResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 5;
            var name = "John Doe";

            var contacts = new List<ContactRepoModel>
        {
            new ContactRepoModel { Name = "John Doe", Email = "john@example.com" },
            new ContactRepoModel { Name = "Jane Smith", Email = "jane@example.com" }
        };
            var totalCount = 2;

            _partnerRepositoryMock
                .Setup(r => r.Get_Partner_Contacts_Async(partnerGuid, numPage, pageSize, name))
                .ReturnsAsync((contacts, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);


            var result = await controller.Get_All_Contacts_From_One_Partner(partnerGuid, numPage, pageSize, name);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            var responseModel = okResult.Value as ApiContactResponseWithCount;

            Assert.NotNull(responseModel);
            Assert.Equal(contacts.Count, responseModel.Contacts.Count);
            Assert.Equal(totalCount, responseModel.TotalCount);
        }

        [Fact]
        public async Task Get_All_Contacts_From_One_Partner_NonExistingContact_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 5;
            var name = "John Doe";

            var contacts = new List<ContactRepoModel>();
            var totalCount = 0;

            _partnerRepositoryMock
                .Setup(r => r.Get_Partner_Contacts_Async(partnerGuid, numPage, pageSize, name))
                .ReturnsAsync((contacts, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);


            var result = await controller.Get_All_Contacts_From_One_Partner(partnerGuid, numPage, pageSize, name);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task Get_All_Contacts_From_One_Partner_NonExistingPartner_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 5;
            var name = "John Doe";

            var contacts = new List<ContactRepoModel>();
            var totalCount = 0;

            _partnerRepositoryMock
                .Setup(r => r.Get_Partner_Contacts_Async(partnerGuid, numPage, pageSize, name))
                .ReturnsAsync((contacts, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Contacts_From_One_Partner(partnerGuid, numPage, pageSize, name);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task Get_All_Contacts_From_One_Partner_ExceptionThrown_ReturnsBadRequestResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 5;
            var name = "John Doe";

            _partnerRepositoryMock
                .Setup(r => r.Get_Partner_Contacts_Async(partnerGuid, numPage, pageSize, name))
                .ThrowsAsync(new Exception("Something went wrong."));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Contacts_From_One_Partner(partnerGuid, numPage, pageSize, name);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);

            var badRequestResult = result.Result as BadRequestObjectResult;
            Assert.Equal("Something went wrong.", badRequestResult.Value);
        }

        [Fact]
        public async Task Update_Partner_Async_Returns_NoContent_When_Successful()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var partnerGuid = Guid.NewGuid();
            var partnerUpdateModel = new PartnerUpdateModel
            {
                Name = "Updated Test Partner",
                Address = "Updated Address",
                Locality = "Updated Locality",
                PostalCode = "Updated PostalCode",
                Country = "Updated Country",
                TaxNumber = "Updated TaxNumber",
                ServiceDescription = "Updated ServiceDescription"
            };
            var partnerRepoModel = partnerUpdateModel.ToPartnerRepoModel();
            partnerRepoModel.PartnerGUID = partnerGuid;

            var existingPartner = new PartnerRepoModel { PartnerGUID = partnerGuid, Name = "Test Partner" };

            partnerMockRepository
                .Setup(r => r.Get_Partner_By_Guid_Async(partnerGuid))
                .ReturnsAsync(existingPartner);

            partnerMockRepository
                .Setup(r => r.Update_Partner_Async(It.IsAny<PartnerRepoModel>()))
                .ReturnsAsync(new DataBase.Models.Partner());

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Update_Partner(partnerGuid, partnerUpdateModel);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            partnerMockRepository.Verify(r => r.Update_Partner_Async(It.IsAny<PartnerRepoModel>()), Times.Once);
        }
        [Fact]
        public async Task Update_Contact_Returns_NoContent_When_Successful()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            var contactUpdateModel = new ContactUpdateModel
            {
                Name = "Updated Test Contact",
                Email = "updated@test.com",
                PhoneNumber = "1234567890"
            };
            var contactRepoModel = contactUpdateModel.ToContactRepoModel();

            var existingContact = new ContactRepoModel { PartnerGUID = partnerGuid, ContactGUID = contactGuid, Name = "Test Contact" };

            partnerMockRepository
                .Setup(r => r.Get_Partner_Contacts_By_Guid_Async(partnerGuid, contactGuid))
                .ReturnsAsync(existingContact);

            partnerMockRepository
                .Setup(r => r.Update_Contact_Async(It.IsAny<ContactRepoModel>()))
                .ReturnsAsync(new DataBase.Models.Contact());

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Update_Contact(partnerGuid, contactGuid, contactUpdateModel);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            partnerMockRepository.Verify(r => r.Update_Contact_Async(It.IsAny<ContactRepoModel>()), Times.Once);
        }
        [Fact]
        public async Task Delete_Partner_Returns_NoContent_When_Successful()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var partnerGuid = Guid.NewGuid();
            var existingPartner = new PartnerRepoModel { PartnerGUID = partnerGuid, Name = "Test Partner" };

            partnerMockRepository
                .Setup(r => r.Get_Partner_By_Guid_Async(partnerGuid))
                .ReturnsAsync(existingPartner);

            partnerMockRepository
                .Setup(r => r.Delete_Partner_Async(partnerGuid))
                .ReturnsAsync(true);

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Delete_Partner(partnerGuid);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            partnerMockRepository.Verify(r => r.Delete_Partner_Async(partnerGuid), Times.Once);
        }

        [Fact]
        public async Task Delete_Contact_Returns_NoContent_When_Successful()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            var existingContact = new ContactRepoModel { PartnerGUID = partnerGuid, ContactGUID = contactGuid, Name = "Test Contact" };

            partnerMockRepository
                .Setup(r => r.Get_Partner_Contacts_By_Guid_Async(partnerGuid, contactGuid))
                .ReturnsAsync(existingContact);

            partnerMockRepository
                .Setup(r => r.Delete_Contact_Async(partnerGuid, contactGuid))
                .ReturnsAsync(true);

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Delete_Contact(partnerGuid, contactGuid);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            partnerMockRepository.Verify(r => r.Delete_Contact_Async(partnerGuid, contactGuid), Times.Once);
        }
        [Fact]
        public async Task Delete_All_Contacts_By_PartnerGuid_Returns_NoContent_When_Successful()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var partnerGuid = Guid.NewGuid();
            var existingContacts = new List<ContactRepoModel>
    {
        new ContactRepoModel { ContactGUID = Guid.NewGuid(), PartnerGUID = partnerGuid, Name = "Test Contact 1" },
        new ContactRepoModel { ContactGUID = Guid.NewGuid(), PartnerGUID = partnerGuid, Name = "Test Contact 2" }
    };
            partnerMockRepository
                .Setup(r => r.Get_Partner_Contacts_Async(partnerGuid, 1, int.MaxValue, string.Empty))
                .ReturnsAsync((existingContacts, existingContacts.Count));

            partnerMockRepository
                .Setup(r => r.Delete_All_Contacts_By_PartnerGuid_Async(partnerGuid))
                .ReturnsAsync(true);

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Delete_All_Contacts(partnerGuid);

            // Assert
            var noContentResult = Assert.IsType<NoContentResult>(result);
            Assert.Equal(StatusCodes.Status204NoContent, noContentResult.StatusCode);
            partnerMockRepository.Verify(r => r.Delete_All_Contacts_By_PartnerGuid_Async(partnerGuid), Times.Once);
        }

        [Fact]
        public async Task Get_All_Partners_History_Should_Return_NotFound_If_History_NotFound()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var pagination = new PartnerApiPagination()
            {
                PageSize = 20,
                Start_Page_Number = 1
            };

            var partnerGuid = Guid.NewGuid();
            int total = 1;

            partnerMockRepository
                .Setup(r => r.Get_All_Partners_History_Async(partnerGuid, pagination.Start_Page_Number, pagination.PageSize))
                .ReturnsAsync((new List<PartnerHistoryRepoModel>(), total));

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Get_All_Partners_History_Async(partnerGuid, pagination.Start_Page_Number, pagination.PageSize);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Get_All_Contacts_History_Should_Return_NotFound_If_History_NotFound()
        {
            // Arrange
            var partnerMockRepository = new Mock<IPartnerRepository>();
            var pagination = new PartnerApiPagination()
            {
                PageSize = 20,
                Start_Page_Number = 1
            };

            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();

            partnerMockRepository
                .Setup(r => r.Get_All_Contacts_History_Async(partnerGuid, contactGuid, pagination.Start_Page_Number, pagination.PageSize))
                .ReturnsAsync((new List<ContactHistoryRepoModel>(), 1));

            var pController = new PartnerController(partnerMockRepository.Object);

            // Act
            var result = await pController.Get_All_Contacts_History_Async(partnerGuid, contactGuid, pagination.Start_Page_Number, pagination.PageSize);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        [Fact]
        public async Task Get_All_Partners_History_Async_ExistingHistory_ReturnsOkResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 5;

            var partnerHistory = new List<PartnerHistoryRepoModel>
        {
            new PartnerHistoryRepoModel { Action = "Action 1", ServiceDescription = "Description 1" },
            new PartnerHistoryRepoModel { Action = "Action 2", ServiceDescription = "Description 2" }
        };
            var totalCount = 2;

            _partnerRepositoryMock
                .Setup(r => r.Get_All_Partners_History_Async(partnerGuid, numPage, pageSize))
                .ReturnsAsync((partnerHistory, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Partners_History_Async(partnerGuid, numPage, pageSize);

            // Assert
            Assert.IsType<OkObjectResult>(result.Result);

            var okResult = result.Result as OkObjectResult;
            var responseModel = okResult.Value as ApiPartnerHistoryResponseWithCount;

            Assert.NotNull(responseModel);
            Assert.Equal(partnerHistory.Count, responseModel.Partners.Count);
            Assert.Equal(totalCount, responseModel.TotalCount);
        }

        [Fact]
        public async Task Get_All_Partners_History_Async_NonExistingHistory_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 5;

            var partnerHistory = new List<PartnerHistoryRepoModel>();
            var totalCount = 0;

            _partnerRepositoryMock
                .Setup(r => r.Get_All_Partners_History_Async(partnerGuid, numPage, pageSize))
                .ReturnsAsync((partnerHistory, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Partners_History_Async(partnerGuid, numPage, pageSize);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task Get_All_Partners_History_Async_EmptyHistory_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 5;

            var partnerHistory = new List<PartnerHistoryRepoModel>();
            var totalCount = 0;

            _partnerRepositoryMock
                .Setup(r => r.Get_All_Partners_History_Async(partnerGuid, numPage, pageSize))
                .ReturnsAsync((partnerHistory, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Partners_History_Async(partnerGuid, numPage, pageSize);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task Get_All_Contacts_History_Async_EmptyHistory_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 5;

            var contactHistory = new List<ContactHistoryRepoModel>();
            var totalCount = 0;

            _partnerRepositoryMock
                .Setup(r => r.Get_All_Contacts_History_Async(partnerGuid, contactGuid, numPage, pageSize))
                .ReturnsAsync((contactHistory, totalCount));

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Get_All_Contacts_History_Async(partnerGuid, contactGuid, numPage, pageSize);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task Update_Partner_NonExistingPartner_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var partnerUpdateModel = new PartnerUpdateModel();

            _partnerRepositoryMock
                .Setup(r => r.Get_Partner_By_Guid_Async(partnerGuid))
                .ReturnsAsync((PartnerRepoModel)null);

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);


            var result = await controller.Update_Partner(partnerGuid, partnerUpdateModel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Update_Contact_NonExistingContact_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            var contactUpdateModel = new ContactUpdateModel();

            _partnerRepositoryMock
                .Setup(r => r.Get_Partner_Contacts_By_Guid_Async(partnerGuid, contactGuid))
                .ReturnsAsync((ContactRepoModel)null);

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Update_Contact(partnerGuid, contactGuid, contactUpdateModel);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Partner_NonExistingPartner_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();

            _partnerRepositoryMock
                .Setup(r => r.Delete_Partner_Async(partnerGuid))
                .ReturnsAsync(false);

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Delete_Partner(partnerGuid);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_Contact_NonExistingContact_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();

            _partnerRepositoryMock
                .Setup(r => r.Delete_Contact_Async(partnerGuid, contactGuid))
                .ReturnsAsync(false);

            // Act
            var controller = new PartnerController(_partnerRepositoryMock.Object);

            var result = await controller.Delete_Contact(partnerGuid, contactGuid);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Delete_All_Contacts_NonExistingContacts_ReturnsNotFoundResult()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var partnerRepositoryMock = new Mock<IPartnerRepository>();

            partnerRepositoryMock
                .Setup(repo => repo.Delete_All_Contacts_By_PartnerGuid_Async(partnerGuid))
                .ReturnsAsync(false);

            var controller = new PartnerController(partnerRepositoryMock.Object);

            // Act
            var result = await controller.Delete_All_Contacts(partnerGuid);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
        [Fact]
        public async Task Get_All_Contacts_History_Async_ContactNotFound_ReturnsNotFound()
        {
            // Arrange
            var partnerGUID = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            var num_page = 1;
            var pageSize = 10;
            var partnerRepositoryMock = new Mock<IPartnerRepository>();
            partnerRepositoryMock.Setup(repo => repo.Get_All_Contacts_History_Async(partnerGUID, contactGuid, num_page, pageSize))
                .ReturnsAsync((null, 0));

            var controller = new PartnerController(partnerRepositoryMock.Object);

            // Act
            var result = await controller.Get_All_Contacts_History_Async(partnerGUID, contactGuid, num_page, pageSize);

            // Assert
            Assert.IsType<NotFoundResult>(result.Result);
        }
        [Fact]
        public async Task Get_All_Contacts_History_Async_ContactFound_ReturnsOk()
        {
            // Arrange
            var partnerGUID = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            var num_page = 1;
            var pageSize = 10;

            var contactHistoryList = new List<ContactHistoryRepoModel>
    {
        // Add mock data if needed
        new ContactHistoryRepoModel
        {
            // Initialize properties with sample values
            ContactGUID = contactGuid,
            PartnerGUID = partnerGUID,
            Name = "John Doe",
            Email = "johndoe@example.com",
            Role = "Manager",
            PhoneNumber = "1234567890",
            Department = "HR",
            Observation = "Observation",
            Action = "Action",
            ActionDate = DateTime.Now,
            UserGUID = Guid.NewGuid()
        }
    };

            var totalCount = contactHistoryList.Count;

            var partnerRepositoryMock = new Mock<IPartnerRepository>();
            partnerRepositoryMock
                .Setup(repo => repo.Get_All_Contacts_History_Async(partnerGUID, contactGuid, num_page, pageSize))
                .ReturnsAsync((contactHistoryList, totalCount));

            var controller = new PartnerController(partnerRepositoryMock.Object);

            // Act
            var result = await controller.Get_All_Contacts_History_Async(partnerGUID, contactGuid, num_page, pageSize);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result.Result);
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var contactPagingModel = Assert.IsType<ApiContactHistoryResponseWithCount>(okResult.Value);
            Assert.Equal(totalCount, contactPagingModel.TotalCount);

            // Verify the returned contact data if needed
            Assert.Single(contactPagingModel.Contacts);
            var contactResponseModel = Assert.Single(contactPagingModel.Contacts);
            // Add additional assertions for the contact response model properties
            Assert.Equal(contactGuid, contactResponseModel.ContactGUID);
            Assert.Equal(partnerGUID, contactResponseModel.PartnerGUID);
            Assert.Equal("John Doe", contactResponseModel.Name);
            // ...
        }
        //OTHER TESTS
        [Fact]
        public void Constructor_PartnerPropertys()
        {
            // Arrange
            var mockPartnerRepository = new Mock<IPartnerRepository>();

            // Act
            var controller = new PartnerController(mockPartnerRepository.Object);

            //Assert
            Assert.Equal(mockPartnerRepository.Object, controller.PartnerRepository);
        }
    }
}
