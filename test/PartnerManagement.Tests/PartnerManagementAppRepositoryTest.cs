using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Moq;
using _Proxy = PartnerManagement.Api.Proxy.Client.Api;
using _ProxyModel = PartnerManagement.Api.Proxy.Client.Model;
using PartnerManagement.Api.Proxy.Client.Model;
using PartnerManagement.App.Repository;
using PartnerManagement.App.Models;
using _Repository = PartnerManagement.App.Repository.IPartnerRepository;
using System.Net;
using Shouldly;
using System.Data;
using Microsoft.Identity.Web;
using Microsoft.Extensions.Configuration;
using PartnerManagement.Api.Proxy.Client.Api;
using MainHub.Internal.PeopleAndCulture;
using PartnerManagement.Api.Proxy.Client.Client;
using PartnerManagement.App.Repository.Extensions.Partner;
using PartnerUpdateModel = PartnerManagement.Api.Proxy.Client.Model.PartnerUpdateModel;
using PartnerManagement.App.Repository.Extensions.Contact;
using ContactUpdateModel = PartnerManagement.Api.Proxy.Client.Model.ContactUpdateModel;
using System.Runtime.CompilerServices;
using PartnerManagement.Repository;
using IdentityModel.OidcClient;
using MainHub.Internal.PeopleAndCulture.Extensions.Partner.Get;
using PartnerManagement.Repository.Models;

namespace PartnerManagement.Tests
{
    public class PartnerManagementAppRepositoryTest
    {
        // CREATE TESTS
        [Fact]
        public void Create_Partner_Success()
        {
            // Arrange
            // criar uma nova instância da classe PartnerModel
            var partnerModel = new PartnerModel();

            // Criar um objeto mock da interface PartnerApi | criar um objeto mock da classe ContactApi
            var mockPartnerProxy = new Mock<_Proxy.IPartnerApi>();

            // criar uma nova instância da classe ApiPartnerCreateResponseModel com valores fictícios para fins de teste
            var responseModel = new ApiPartnerCreateResponseModel()
            {
                PartnerGUID = Guid.Empty,
                Name = string.Empty,
                Address = string.Empty,
                PostalCode = string.Empty,
                Country = string.Empty,
                TaxNumber = string.Empty,
                ServiceDescription = string.Empty,
                CreationDate = new DateTime(2999, 12, 31, 23, 59, 54),
                CreatedBy = string.Empty,
                ChangedDate = new DateTime(2999, 12, 31, 23, 59, 54),
                ModifiedBy = string.Empty,
                State = "Ativo",
            };

            // configurar o objeto mock da classe PartnerApi para retornar o responseModel quando o método ApiPartnerPostAsync for chamado com qualquer objeto ApiPartnerCreateRequestModel como primeiro parâmetro, 0 como segundo parâmetro e o token de cancelamento padrão como terceiro parâmetro
            mockPartnerProxy.Setup(p => p.ApiPartnerPostAsync(It.IsAny<ApiPartnerCreateRequestModel>(), 0, default))
               .ReturnsAsync(responseModel);

            // criar uma nova instância da classe PartnerAppRepository usando os objetos mock da classe PartnerApi e ContactApi
            var repository = new PartnerAppRepository(mockPartnerProxy.Object);

            // Act
            // chamar o método Create_Partner do objeto repository com o objeto partnerModel como parâmetro
            var result = repository.App_Create_Partner_By_Form(partnerModel);

            // Assert
            // verificar se o resultado não é nulo
            result.ShouldNotBeNull();
        }

        [Fact]
        public void Create_Contact_Success()
        {
            // Arrange
            var contactModel = new ContactModel();

            var mockPartnerProxy = new Mock<_Proxy.IPartnerApi>();

            var responseModel = new ApiContactCreateResponseModel()
            {
                ContactGUID = Guid.Empty,
                PartnerGUID = Guid.Empty,
                Name = string.Empty,
                Email = string.Empty,
                Role = string.Empty,
                PhoneNumber = string.Empty,
                Department = string.Empty
            };

            mockPartnerProxy.Setup(p => p.ApiPartnerPartnerGuidContactPostAsync(responseModel.PartnerGUID, It.IsAny<ApiContactCreateRequestModel>(), 0, default))
                     .ReturnsAsync(responseModel);

            var repository = new PartnerAppRepository(mockPartnerProxy.Object);

            // ActCreateAbsence
            var result = repository.App_Create_Contact_By_Form(responseModel.PartnerGUID, contactModel);

            // Assert
            result.ShouldNotBeNull();
        }

        //GETS TESTS
        [Fact]
        public async Task GetAllPartners_ReturnsSuccessfulPartnersAndCount()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = "unspecifiedname";

            var expectedPartners = new ApiPartnerResponseWithCount
            {

                Partners = new List<ApiPartnerResponseModel> { new ApiPartnerResponseModel { } },
                TotalCount = 1
            };

            var expectedTotalCount = 1;

            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy.Setup(p => p.ApiPartnerGetAsync(page, pageSize, filter, 0, default))
                    .ReturnsAsync(expectedPartners);

            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            var (partners, count) = await repository.Get_All_Partners_Async(page, pageSize, filter);

            // Assert

            Assert.Equal(1, partners.Count);
            Assert.Equal(expectedTotalCount, count);
        }

        [Fact]
        public async Task GetAllContact_ReturnsSuccessfulContactAndCount()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = "unspecifiedname";
            var partnerGuid = Guid.NewGuid();
            var expectedContacts = new ApiContactResponseWithCount
            {

                Contacts = new List<ApiContactResponseModel> { new ApiContactResponseModel { } },
                TotalCount = 1
            };

            var expectedTotalCount = 1;

            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy.Setup(p => p.ApiPartnerPartnerGuidContactGetAsync(partnerGuid, page, pageSize, filter, 0, default))
                    .ReturnsAsync(expectedContacts);

            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            var (contacts, count) = await repository.Get_All_Contacts_Async(partnerGuid, page, pageSize, filter);

            // Assert

            Assert.Equal(1, contacts.Count);
            Assert.Equal(expectedTotalCount, count);
        }
        [Fact]
        public async Task Get_Partner_By_Guid_Async_Returns_PartnerModel()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var partnerResponseModel = new ApiPartnerResponseModel
            {
                PartnerGUID = partnerGuid,
                Name = "Example Partner",
                Address = "123 Main Street",
                Locality = "City",
                PostalCode = "12345",
                Country = "Country",
                TaxNumber = "123456789",
                ServiceDescription = "Service Description",
                CreationDate = DateTime.Now,
                CreatedBy = "John Doe",
                ChangedDate = DateTime.Now.AddDays(-1),
                ModifiedBy = "Jane Smith",
                State = "Active"
            };
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(m => m.ApiPartnerPartnerGuidGetAsync(partnerGuid, 0, default(CancellationToken)))
                .ReturnsAsync(partnerResponseModel);

            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            var result = await repository.Get_Partner_By_Guid_Async(partnerGuid);

            // Assert
            result.ShouldNotBeNull();
            result.PartnerGUID.ShouldBe(partnerResponseModel.PartnerGUID);
        }

        [Fact]
        public async Task Get_Specific_Contact_From_Specific_Partner_Returns_ContactModel()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            var contactResponseModel = new ApiContactResponseModel
            {
                ContactGUID = contactGuid,
                PartnerGUID = partnerGuid,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Role = "Manager",
                PhoneNumber = "1234567890",
                Department = "Sales"
            };
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(m => m.ApiPartnerPartnerGuidContactContactGuidGetAsync(partnerGuid, contactGuid, 0, default(CancellationToken)))
                .ReturnsAsync(contactResponseModel);

            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            var result = await repository.Get_Specific_Contact_From_One_Partner(partnerGuid, contactGuid);

            // Assert
            result.ShouldNotBeNull();
            result.ContactGUID.ShouldBe(contactResponseModel.ContactGUID);
            result.PartnerGUID.ShouldBe(contactResponseModel.PartnerGUID);
            result.Name.ShouldBe(contactResponseModel.Name);
            result.Email.ShouldBe(contactResponseModel.Email);
            result.Role.ShouldBe(contactResponseModel.Role);
            result.PhoneNumber.ShouldBe(contactResponseModel.PhoneNumber);
            result.Department.ShouldBe(contactResponseModel.Department);
        }

        [Fact]
        public async Task Update_Partner_Async_Returns_True_On_Success()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            PartnerModel modelNew = new PartnerModel { /* Initialize properties */ };
            PartnerUpdateModel expectedUpdatedModel = modelNew.ToPartnerUpdateModel();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidPutAsync(partnerGuid, expectedUpdatedModel, 0, default(CancellationToken)))
                .Returns(Task.CompletedTask);
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Update_Partner_Async(partnerGuid, modelNew);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public async Task Update_Partner_Async_Returns_False_On_ApiException()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            PartnerModel modelNew = new PartnerModel { /* Initialize properties */ };
            PartnerUpdateModel expectedUpdatedModel = modelNew.ToPartnerUpdateModel();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidPutAsync(partnerGuid, expectedUpdatedModel, 0, default(CancellationToken)))
                .Throws(new ApiException());
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Update_Partner_Async(partnerGuid, modelNew);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public async Task Update_Contact_Async_Returns_True_On_Success()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            Guid contactGuid = Guid.NewGuid();
            ContactModel modelNew = new ContactModel { /* Initialize properties */ };
            ContactUpdateModel expectedUpdatedModel = modelNew.ToContactUpdateModel();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidContactContactGuidPutAsync(partnerGuid, contactGuid, expectedUpdatedModel, 0, default(CancellationToken)))
                .Returns(Task.CompletedTask);
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Update_Contact_Async(partnerGuid, contactGuid, modelNew);

            // Assert
            Assert.True(result);
        }
        [Fact]
        public async Task Update_Contact_Async_Returns_False_On_ApiException()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            Guid contactGuid = Guid.NewGuid();
            ContactModel modelNew = new ContactModel { /* Initialize properties */ };
            ContactUpdateModel expectedUpdatedModel = modelNew.ToContactUpdateModel();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidContactContactGuidPutAsync(partnerGuid, contactGuid, expectedUpdatedModel, 0, default(CancellationToken)))
                .Throws(new ApiException());
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Update_Contact_Async(partnerGuid, contactGuid, modelNew);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Delete_Partner_Async_Returns_True_On_Success()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidDeleteAsync(partnerGuid, 0, default(CancellationToken)))
                .Returns(Task.CompletedTask);
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Delete_Partner_Async(partnerGuid);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Delete_Partner_Async_Returns_False_On_ApiException()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidDeleteAsync(partnerGuid, 0, default(CancellationToken)))
                .Throws(new ApiException());
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Delete_Partner_Async(partnerGuid);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Delete_Contact_Async_Returns_True_On_Success()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            Guid contactGuid = Guid.NewGuid();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidContactContactGuidDeleteAsync(partnerGuid, contactGuid, 0, default(CancellationToken)))
                .Returns(Task.CompletedTask);
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Delete_Contact_Async(partnerGuid, contactGuid);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Delete_Contact_Async_Returns_False_On_ApiException()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            Guid contactGuid = Guid.NewGuid();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidContactContactGuidDeleteAsync(partnerGuid, contactGuid, 0, default(CancellationToken)))
                .Throws(new ApiException());
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Delete_Contact_Async(partnerGuid, contactGuid);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Delete_All_Contacts_Async_Returns_True_On_Success()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidContactDeleteAsync(partnerGuid, 0, default(CancellationToken)))
                .Returns(Task.CompletedTask);
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Delete_All_Contacts_Async(partnerGuid);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Delete_All_Contacts_Async_Returns_False_On_ApiException()
        {
            // Arrange
            Guid partnerGuid = Guid.NewGuid();
            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy
                .Setup(mock => mock.ApiPartnerPartnerGuidContactDeleteAsync(partnerGuid, 0, default(CancellationToken)))
                .Throws(new ApiException());
            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            bool result = await repository.Delete_All_Contacts_Async(partnerGuid);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public async Task Get_All_Partner_History_From_Specific_Partner_Should_Return_Partner_History()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var filter = "unspecifiedname";
            var partnerGUID = Guid.NewGuid();
            var expectedPartners = new _ProxyModel.ApiPartnerHistoryResponseWithCount
            {

                Partners = new List<_ProxyModel.ApiPartnerHistoryResponseModel> { new _ProxyModel.ApiPartnerHistoryResponseModel { } },
                TotalCount = 1
            };

            var expectedTotalCount = 1;

            var mockPartnerApiProxy = new Mock<IPartnerApi>();

            mockPartnerApiProxy.Setup(p => p.ApiPartnerPartnerGUIDPartnerhistoryGetAsync(partnerGUID, page, pageSize, 0, default(CancellationToken)))
                    .ReturnsAsync(expectedPartners);

            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            var (partners, count) = await repository.Get_All_Partner_History_From_Specific_Partner(partnerGUID, page, pageSize);

            // Assert
            Assert.Equal(1, partners.Count);
            Assert.Equal(expectedTotalCount, count);
        }

        [Fact]
        public async Task Get_All_Contact_History_From_Specific_Partner_And_Contact_Should_Return_Contact_History()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            int numPage = 1;
            int pageSize = 20;
            var expectedPartners = new _ProxyModel.ApiContactHistoryResponseWithCount
            {

                Contacts = new List<_ProxyModel.ApiContactHistoryResponseModel> { new _ProxyModel.ApiContactHistoryResponseModel { } },
                TotalCount = 1
            };
            var expectedTotalCount = 1;

            var mockPartnerApiProxy = new Mock<IPartnerApi>();
            mockPartnerApiProxy
                .Setup(m => m.ApiPartnerPartnerGUIDContactContactGuidContacthistoryGetAsync(partnerGuid, contactGuid, numPage, pageSize, 0, default(CancellationToken)))
                .ReturnsAsync(expectedPartners);

            var repository = new PartnerAppRepository(mockPartnerApiProxy.Object);

            // Act
            var (contacts, count) = await repository.Get_All_Contact_History_From_Specific_Partner_And_Contact(partnerGuid, contactGuid, numPage, pageSize);

            // Assert
            Assert.Equal(1, contacts.Count);
            Assert.Equal(expectedTotalCount, count);
        }
        //OTHER TESTS
        [Fact]
        public void Proxy_Injection()
        {
            // Arrange
            var partnerProxyMock = new Mock<IPartnerApi>();

            // Act
            var repository = new PartnerAppRepository(partnerProxyMock.Object);

            // Assert
            Assert.Equal(partnerProxyMock.Object, repository.PartnerApiProxy);
        }
    }
}
