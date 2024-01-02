using Microsoft.EntityFrameworkCore;
using PartnerManagement.DataBase;
using PartnerManagement.Repository;
using PartnerManagement.Repository.Models;
using Moq;
using Shouldly;
using Xunit;
using PartnerManagement.DataBase.Models;
using MainHub.Internal.PeopleAndCulture;
using Microsoft.EntityFrameworkCore.Metadata;
using PartnerManagement.Api.Proxy.Client.Api;
using PartnerManagement.Api.Proxy.Client.Client;
using PartnerManagement.App.Models;
using PartnerManagement.App.Repository;
using MainHub.Internal.PeopleAndCulture.Models;

namespace PartnerManagement.Tests
{
    public class PartnerRepositoryTest
    {
        //CREATE TESTS
        [Fact]
        public async Task CreatePartner_Should_Create_Partner_With_Correct_Properties()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;
            using var dbContext = new PartnerManagementDBContext(options);
            var repository = new PartnerRepository(dbContext);

            var partnerRepoModel = new PartnerRepoModel
            {
                PartnerGUID = Guid.Empty,
                Name = string.Empty,
                Address = string.Empty,
                Locality = string.Empty,
                PostalCode = string.Empty,
                Country = string.Empty,
                TaxNumber = string.Empty,
                ServiceDescription = string.Empty,
                CreationDate = new DateTime(2999, 12, 31, 23, 59, 54),
                CreatedBy = Guid.NewGuid().ToString(),
                ChangedDate = new DateTime(2999, 12, 31, 23, 59, 54),
                ModifiedBy = string.Empty,
                State = "Active",
                IsDeleted = false,
                DeletedBy = Guid.NewGuid(),
            };

            //Act
            var createdPartner = await repository.Create_Partner_Async(partnerRepoModel);

            //Assert
            createdPartner.ShouldNotBeNull();
        }

        [Fact]
        public async Task Create_Contact_Should_Create_Contact_With_Correct_Properties()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;
            using var dbContext = new PartnerManagementDBContext(options);
            var repository = new PartnerRepository(dbContext);

            var contactRepoModel = new ContactRepoModel
            {
                ContactGUID = Guid.Empty,
                PartnerGUID = Guid.Empty,
                Name = string.Empty,
                Email = string.Empty,
                Role = string.Empty,
                PhoneNumber = string.Empty,
                Department = string.Empty
            };

            //Act
            var createdContact = await repository.Create_Contact_Async(contactRepoModel);

            //Assert
            createdContact.ShouldNotBeNull();
        }

        [Fact]
        public void Constructor_Should_Inject_PartnerManagementDbContext()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;

            using var dbContext = new PartnerManagementDBContext(options);

            //Act
            var repository = new PartnerRepository(dbContext);

            //Assert
            repository.ShouldNotBeNull();
            repository.DbContext.ShouldBe(dbContext);
        }

        //GET TESTS
        // GET TESTS
        [Fact]
        public async Task Get_All_Partners_Async_WithunspecifiednameName_ShouldReturnNonDeletedPartners()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var dbContext = new PartnerManagementDBContext(options);
            var paginated = new PartnerApiPagination();
            var expectedTotalCount = 2;

            var numPage = 1;
            var pageSize = 20;
            var name = string.Empty;
            var partnerGUID1 = Guid.NewGuid();
            var partnerGUID2 = Guid.NewGuid();
            var partnerGUID3 = Guid.NewGuid();

            var partner1 = new Partner { PartnerGUID = partnerGUID1, Name = "Partner 1", IsDeleted = false };
            var partner2 = new Partner { PartnerGUID = partnerGUID2, Name = "Partner 2", IsDeleted = true };
            var partner3 = new Partner { PartnerGUID = partnerGUID3, Name = "Partner 3", IsDeleted = false };

            await dbContext.Partner.AddRangeAsync(partner1, partner2, partner3);
            await dbContext.SaveChangesAsync();

            // Act
            var repo = new PartnerRepository(dbContext);

            var (result, count) = await repo.Get_All_Partners_Async(numPage, pageSize, name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal(expectedTotalCount, count);

            Assert.Equal(partner1.PartnerGUID, result.First().PartnerGUID);
        }
        [Fact]
        public async Task Get_All_Partners_Async_DeletedPartner_ReturnsEmptyList()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new PartnerManagementDBContext(options);

            var numPage = 1;
            var pageSize = 20;
            var name = "unspecifiedname";
            var partnerGUID = Guid.NewGuid();

            var deletedPartner = new Partner { PartnerGUID = partnerGUID, Name = "Deleted Partner", IsDeleted = true };

            await dbContext.Partner.AddAsync(deletedPartner);
            await dbContext.SaveChangesAsync();

            var repository = new PartnerRepository(dbContext);

            // Act
            var (partners, count) = await repository.Get_All_Partners_Async(numPage, pageSize, name);

            // Assert
            Assert.NotNull(partners);
            Assert.Empty(partners);
            Assert.Equal(0, count);
        }
        [Fact]
        public async Task Get_All_Partners_Async_WithNonUnspecifiedNameName_ShouldReturnFilteredPartners()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new PartnerManagementDBContext(options);

            var numPage = 1;
            var pageSize = 20;
            var name = "filter";
            var partnerGUID1 = Guid.NewGuid();
            var partnerGUID2 = Guid.NewGuid();

            var partner1 = new Partner { PartnerGUID = partnerGUID1, Name = "Filtered Partner 1", IsDeleted = false };
            var partner2 = new Partner { PartnerGUID = partnerGUID2, Name = "Filtered Partner 2", IsDeleted = false };
            var partner3 = new Partner { PartnerGUID = Guid.NewGuid(), Name = "Not Filtered Partner", IsDeleted = false };

            await dbContext.Partner.AddRangeAsync(partner1, partner2, partner3);
            await dbContext.SaveChangesAsync();

            var repository = new PartnerRepository(dbContext);

            // Act
            var (partners, count) = await repository.Get_All_Partners_Async(numPage, pageSize, name);

            // Assert
            Assert.NotNull(partners);
            Assert.Equal(3, partners.Count);
            Assert.Equal(3, count);
            Assert.All(partners, p => Assert.Contains(name, p.Name, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public async Task Get_All_Partners_Async_WithUnspecifiedNameName_ShouldReturnBadNonDeletedPartners()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            using var dbContext = new PartnerManagementDBContext(options);

            var numPage = 1;
            var pageSize = 20;
            var name = "unspecifiedname";
            var partnerGUID1 = Guid.NewGuid();
            var partnerGUID2 = Guid.NewGuid();
            var partnerGUID3 = Guid.NewGuid();

            var partner1 = new Partner { PartnerGUID = partnerGUID1, Name = "Partner 1", IsDeleted = true };
            var partner2 = new Partner { PartnerGUID = partnerGUID2, Name = "Partner 2", IsDeleted = true };
            var partner3 = new Partner { PartnerGUID = partnerGUID3, Name = "Partner 3", IsDeleted = true };

            await dbContext.Partner.AddRangeAsync(partner1, partner2, partner3);
            await dbContext.SaveChangesAsync();

            var repository = new PartnerRepository(dbContext);

            // Act
            var (partners, count) = await repository.Get_All_Partners_Async(numPage, pageSize, name);

            // Assert
            Assert.NotNull(partners);
            Assert.Empty(partners);
            Assert.Equal(0, count);
        }

        [Fact]
        public async Task Get_All_Contacts_With_Specific_Partner_Guid_With_Successful_Return()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;

            var filter = string.Empty;

            using var dbContext = new PartnerManagementDBContext(options);
            var paginated = new PartnerApiPagination();

            //Add test data
            var contact1 = new Contact { PartnerGUID = Guid.NewGuid(), ContactGUID = Guid.NewGuid() };

            paginated.PageSize = 10;
            paginated.Start_Page_Number = 1;
            dbContext.Contact.AddRange(contact1);
            await dbContext.SaveChangesAsync();

            //Act
            var repo = new PartnerRepository(dbContext);
            var (result, count) = await repo.Get_Partner_Contacts_Async(contact1.PartnerGUID, 1, 5, filter);

            //Assert
            Assert.NotNull(result);

            //coleção e o predicado/filtro 
            Assert.Contains(result, c => c.PartnerGUID == contact1.PartnerGUID && c.ContactGUID == contact1.ContactGUID);
        }
        [Fact]
        public async Task Get_Partner_Contacts_Async_WithNullOrEmptyName_ShouldReturnNonDeletedContacts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var dbContext = new PartnerManagementDBContext(options);
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 20;
            string name = string.Empty;

            var contactGUID1 = Guid.NewGuid();
            var contactGUID2 = Guid.NewGuid();
            var contactGUID3 = Guid.NewGuid();

            var partnerGUID1 = Guid.NewGuid();
            var partnerGUID2 = Guid.NewGuid();

            var contact1 = new Contact { ContactGUID = contactGUID1, PartnerGUID = partnerGUID1, Name = "Contact 1", IsDeleted = false };
            var contact2 = new Contact { ContactGUID = contactGUID2, PartnerGUID = partnerGUID2, Name = "Contact 2", IsDeleted = true };
            var contact3 = new Contact { ContactGUID = contactGUID3, PartnerGUID = partnerGUID1, Name = "Contact 3", IsDeleted = false };

            await dbContext.Contact.AddRangeAsync(contact1, contact2, contact3);
            await dbContext.SaveChangesAsync();

            // Act
            var repo = new PartnerRepository(dbContext);

            var (result, count) = await repo.Get_Partner_Contacts_Async(partnerGUID1, numPage, pageSize, name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.ContactGUID == contact1.ContactGUID);
            Assert.Contains(result, c => c.ContactGUID == contact3.ContactGUID);
        }

        [Fact]
        public async Task Get_Partner_Contacts_Async_WithunspecifiednameName_ShouldReturnNonDeletedContacts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var dbContext = new PartnerManagementDBContext(options);
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 20;
            string name = string.Empty;

            var contactGUID1 = Guid.NewGuid();
            var contactGUID2 = Guid.NewGuid();
            var contactGUID3 = Guid.NewGuid();

            var partnerGUID1 = Guid.NewGuid();
            var partnerGUID2 = Guid.NewGuid();

            var contact1 = new Contact { ContactGUID = contactGUID1, PartnerGUID = partnerGUID1, Name = "Contact 1", IsDeleted = false };
            var contact2 = new Contact { ContactGUID = contactGUID2, PartnerGUID = partnerGUID2, Name = "Contact 2", IsDeleted = true };
            var contact3 = new Contact { ContactGUID = contactGUID3, PartnerGUID = partnerGUID1, Name = "Contact 3", IsDeleted = false };

            await dbContext.Contact.AddRangeAsync(contact1, contact2, contact3);
            await dbContext.SaveChangesAsync();

            // Act
            var repo = new PartnerRepository(dbContext);

            var (result, count) = await repo.Get_Partner_Contacts_Async(partnerGUID1, numPage, pageSize, name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.ContactGUID == contact1.ContactGUID);
            Assert.Contains(result, c => c.ContactGUID == contact3.ContactGUID);
        }

        [Fact]
        public async Task Get_Partner_Contacts_Async_WithNonunspecifiednameName_ShouldReturnFilteredContacts()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var dbContext = new PartnerManagementDBContext(options);
            var partnerGuid = Guid.NewGuid();
            var numPage = 1;
            var pageSize = 20;
            string name = "example";

            var contactGUID1 = Guid.NewGuid();
            var contactGUID2 = Guid.NewGuid();
            var contactGUID3 = Guid.NewGuid();

            var partnerGUID1 = Guid.NewGuid();
            var partnerGUID2 = Guid.NewGuid();

            var contact1 = new Contact { ContactGUID = contactGUID1, PartnerGUID = partnerGUID1, Name = "Contact 1", IsDeleted = false };
            var contact2 = new Contact { ContactGUID = contactGUID2, PartnerGUID = partnerGUID2, Name = "Contact 2", IsDeleted = true };
            var contact3 = new Contact { ContactGUID = contactGUID3, PartnerGUID = partnerGUID1, Name = "Contact 3", IsDeleted = false };

            await dbContext.Contact.AddRangeAsync(contact1, contact2, contact3);
            await dbContext.SaveChangesAsync();

            // Act
            var repo = new PartnerRepository(dbContext);

            var (result, count) = await repo.Get_Partner_Contacts_Async(partnerGUID1, numPage, pageSize, name);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(0, result.Count);
        }
        [Fact]
        public async Task Get_One_Contact_With_Specific_Contact_Guid_With_Specific_Partner_Guid_With_Successful_Return()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;

            using var dbContext = new PartnerManagementDBContext(options);

            //Add test data
            var contact1 = new Contact { PartnerGUID = Guid.NewGuid(), ContactGUID = Guid.NewGuid() };

            dbContext.Contact.AddRange(contact1);
            await dbContext.SaveChangesAsync();

            //Act
            var repo = new PartnerRepository(dbContext);
            var result = await repo.Get_Partner_Contacts_By_Guid_Async(contact1.PartnerGUID, contact1.ContactGUID);

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Get_One_Partner_With_Specific_Partner_Guid_With_Successful_Return()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;

            using var dbContext = new PartnerManagementDBContext(options);

            //Add test data
            var partner1 = new Partner { PartnerGUID = Guid.NewGuid() };

            dbContext.Partner.AddRange(partner1);
            await dbContext.SaveChangesAsync();

            //Act
            var repo = new PartnerRepository(dbContext);
            var result = await repo.Get_Partner_By_Guid_Async(partner1.PartnerGUID);

            //Assert
            Assert.NotNull(result);
        }
        [Fact]
        public async Task Update_Partner_Async_Should_Return_True_On_Successful_Update()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var modelNew = new PartnerRepoModel
            {
                PartnerGUID = partnerGuid,
                Name = "New Name",
                Address = "New Address",
                Locality = "New Locality",
                PostalCode = "New Postal Code",
                Country = "New Country",
                TaxNumber = "New Tax Number",
                ServiceDescription = "New Service Description",
                IsDeleted = false,
                CreatedBy = Guid.NewGuid().ToString(),
                DeletedBy = Guid.NewGuid(),
            };

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;

            using var dbContext = new PartnerManagementDBContext(options);

            // Add a test partner
            var partner = new Partner { PartnerGUID = partnerGuid, CreatedBy = Guid.NewGuid().ToString() };
            dbContext.Partner.Add(partner);
            await dbContext.SaveChangesAsync();

            var partnerService = new PartnerRepository(dbContext);

            // Act
            var result = await partnerService.Update_Partner_Async(modelNew);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(modelNew.Name, result.Name);
            Assert.Equal(modelNew.Address, result.Address);
            Assert.Equal(modelNew.Locality, result.Locality);
            Assert.Equal(modelNew.PostalCode, result.PostalCode);
            Assert.Equal(modelNew.Country, result.Country);
            Assert.Equal(modelNew.TaxNumber, result.TaxNumber);
            Assert.Equal(modelNew.ServiceDescription, result.ServiceDescription);
        }
        [Fact]
        public async Task Update_Contact_Async_Should_Return_True_On_Successful_Update()
        {
            // Arrange
            var contactGuid = Guid.NewGuid();
            var modelNew = new ContactRepoModel
            {
                ContactGUID = contactGuid,
                Name = "New Name",
                Email = "newemail@example.com",
                Role = "New Role",
                PhoneNumber = "1234567890",
                Department = "New Department"
            };

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db").Options;

            using var dbContext = new PartnerManagementDBContext(options);

            // Add a test contact
            var contact = new Contact { ContactGUID = contactGuid };
            dbContext.Contact.Add(contact);
            await dbContext.SaveChangesAsync();

            var partnerService = new PartnerRepository(dbContext);

            // Act
            var result = await partnerService.Update_Contact_Async(modelNew);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(modelNew.Name, result.Name);
            Assert.Equal(modelNew.Email, result.Email);
            Assert.Equal(modelNew.Role, result.Role);
            Assert.Equal(modelNew.PhoneNumber, result.PhoneNumber);
            Assert.Equal(modelNew.Department, result.Department);
        }
        [Fact]
        public async Task Delete_Partner_Async_Should_Return_True_When_Partner_Is_Deleted()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new PartnerManagementDBContext(options);

            // Add a test partner
            var partner = new Partner
            {
                PartnerGUID = partnerGuid,
                CreatedBy = Guid.NewGuid().ToString()
            };

            dbContext.Partner.Add(partner);
            await dbContext.SaveChangesAsync();

            var partnerService = new PartnerRepository(dbContext);

            // Act
            var result = await partnerService.Delete_Partner_Async(partnerGuid);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Delete_Contact_Async_Should_Return_True_When_Contact_Is_Deleted()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new PartnerManagementDBContext(options);

            // Add a test contact
            var contact = new Contact { ContactGUID = contactGuid, PartnerGUID = partnerGuid };
            dbContext.Contact.Add(contact);
            await dbContext.SaveChangesAsync();

            var partnerService = new PartnerRepository(dbContext);

            // Act
            var result = await partnerService.Delete_Contact_Async(partnerGuid, contactGuid);

            // Assert
            Assert.True(result);

            // Verify contact is deleted
            var deletedContact = await dbContext.Contact.FirstOrDefaultAsync(a => a.ContactGUID == contactGuid && a.PartnerGUID == partnerGuid);
            Assert.NotNull(deletedContact);
            Assert.True(deletedContact.IsDeleted);
        }

        [Fact]
        public async Task Delete_All_Contacts_By_PartnerGuid_Async_Should_Return_True_When_Contacts_Are_Deleted()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new PartnerManagementDBContext(options);

            // Add test contacts
            var contacts = new List<Contact>
    {
        new Contact { ContactGUID = Guid.NewGuid(), PartnerGUID = partnerGuid },
        new Contact { ContactGUID = Guid.NewGuid(), PartnerGUID = partnerGuid },
        new Contact { ContactGUID = Guid.NewGuid(), PartnerGUID = partnerGuid }
    };
            dbContext.Contact.AddRange(contacts);
            await dbContext.SaveChangesAsync();

            var partnerService = new PartnerRepository(dbContext);

            // Act
            var result = await partnerService.Delete_All_Contacts_By_PartnerGuid_Async(partnerGuid);

            // Assert
            Assert.True(result);

            // Verify contacts are deleted
            var deletedContacts = await dbContext.Contact.Where(a => a.PartnerGUID == Guid.NewGuid() && a.IsDeleted).ToListAsync();
            Assert.Empty(deletedContacts);
        }
        [Fact]
        public async Task Get_All_Partners_History_Async_Should_Return_Partner_History()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var num_page = 1;
            var pageSize = 20;
            var expectedActions = new List<string> { "Create", "Update" };
            var expectedTotalCount = 2;

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using (var dbContext = new PartnerManagementDBContext(options))
            {
                // Add test partner history
                var partnerHistory = new List<PartnerHistory>
        {
            new PartnerHistory { PartnerGUID = partnerGuid, Action = "Create" },
            new PartnerHistory { PartnerGUID = partnerGuid, Action = "Update" },
        };
                dbContext.PartnerHistory.AddRange(partnerHistory);
                await dbContext.SaveChangesAsync();

                var partnerService = new PartnerRepository(dbContext);

                // Act
                var (result, count) = await partnerService.Get_All_Partners_History_Async(partnerGuid, num_page, pageSize);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.Equal(expectedTotalCount, count);

                foreach (var action in expectedActions)
                {
                    Assert.Contains(result, r => r.Action == action);
                }
            }
        }

        [Fact]
        public async Task Get_All_Partners_History_With_Bad_Return()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            using var dbContext = new PartnerManagementDBContext(options);
            var pagination = new PartnerApiPagination()
            {
                PageSize = 20,
                Start_Page_Number = 1
            };
            var partnerGuid = Guid.NewGuid();

            // No test data added

            // Act
            var repo = new PartnerRepository(dbContext);
            var (result, count) = await repo.Get_All_Partners_History_Async(partnerGuid, pagination.Start_Page_Number, pagination.PageSize);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task Get_All_Contacts_History_Async_Should_Return_Contact_History()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            var num_page = 1;
            var pageSize = 20;
            var expectedActions = new List<string> { "Create", "Update" };
            var expectedTotalCount = 2;

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using (var dbContext = new PartnerManagementDBContext(options))
            {
                // Add test contact history
                var contactHistory = new List<ContactHistory>
        {
            new ContactHistory { PartnerGUID = partnerGuid, ContactGUID = contactGuid, Action = "Create" },
            new ContactHistory { PartnerGUID = partnerGuid, ContactGUID = contactGuid, Action = "Update" },
        };
                dbContext.ContactHistory.AddRange(contactHistory);
                await dbContext.SaveChangesAsync();

                var partnerService = new PartnerRepository(dbContext);

                // Act
                var (result, count) = await partnerService.Get_All_Contacts_History_Async(partnerGuid, contactGuid, num_page, pageSize);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count());
                Assert.Equal(expectedTotalCount, count);

                foreach (var action in expectedActions)
                {
                    Assert.Contains(result, r => r.Action == action);
                }
            }
        }

        [Fact]
        public async Task Get_All_Contacts_History_Async_Should_Return_Bad_Request()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();
            var num_page = 1;
            var pageSize = 20;

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using (var dbContext = new PartnerManagementDBContext(options))
            {
                // No test contact history added

                var partnerService = new PartnerRepository(dbContext);

                // Act
                var (result, count) = await partnerService.Get_All_Contacts_History_Async(partnerGuid, contactGuid, num_page, pageSize);

                // Assert
                Assert.NotNull(result);
                Assert.Empty(result);
            }
        }
        [Fact]
        public async Task Delete_Partner_Async_Should_Return_False_When_Partner_Not_Found()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new PartnerManagementDBContext(options);

            var partnerService = new PartnerRepository(dbContext);

            // Act
            var result = await partnerService.Delete_Partner_Async(partnerGuid);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Delete_Contact_Async_Should_Return_False_When_Contact_Not_Found()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();
            var contactGuid = Guid.NewGuid();

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new PartnerManagementDBContext(options);

            var partnerService = new PartnerRepository(dbContext);

            // Act
            var result = await partnerService.Delete_Contact_Async(partnerGuid, contactGuid);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Delete_All_Contacts_By_PartnerGuid_Async_Should_Return_False_When_No_Contacts_Found()
        {
            // Arrange
            var partnerGuid = Guid.NewGuid();

            // Set up an in-memory database context
            var options = new DbContextOptionsBuilder<PartnerManagementDBContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new PartnerManagementDBContext(options);

            var partnerService = new PartnerRepository(dbContext);

            // Act
            var result = await partnerService.Delete_All_Contacts_By_PartnerGuid_Async(partnerGuid);

            // Assert
            Assert.False(result);
        }
    }
}
