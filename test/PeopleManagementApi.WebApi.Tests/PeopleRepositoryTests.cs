using Microsoft.EntityFrameworkCore;
using PeopleManagementRepository;
using Shouldly;
using PeopleManagementRepository.Data;
using PeopleManagementRepository.Models;
using PeopleManagementDataBase;
using Moq;
using Microsoft.Graph;

namespace MainHub.Internal.PeopleAndCulture
{
    public class PeopleRepositoryTests
    {
        [Fact]
        public void Constructor_Should_Inject_PeopleManagementDbContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new PeopleManagementDbContext(options);

            // Act
            // Instance of PeopleRepository with the mock context as a parameter
            var repository = new PeopleRepository(dbContext);

            // Assert
            // Verify that the repository instance is not null and that its _dbContext property is set to the mock context object
            repository.ShouldNotBeNull();
            repository.DbContext.ShouldBe(dbContext);
        }

        [Fact]
        public async Task CreateCollaborator_Should_Create_Collaborator_With_Correct_Properties()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;


            using var dbContext = new PeopleManagementDbContext(options);
            var repository = new PeopleRepository(dbContext);

            var collaborators = new List<Collaborator>
            {
                new Collaborator
                {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Dinis",
                LastName = "Godinho",
                Email = "dgodinho@mainhub.pt",
                Employee_Id = 1,
            },
        };
            await dbContext.Person.AddRangeAsync(collaborators);
            await dbContext.SaveChangesAsync();

            var peopleRepoModel = new PeopleRepoModel
            {
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                CreatedBy = "None",
                FirstName = "user",
                Postal = "1500",
                Status = "Active",
                ChangedBy = "None",
                Email = "dgodinho@mainhub.pt"
            };

            // Act
            var createdPerson = await repository.CreateCollaboratorAsync(peopleRepoModel, userId);
            await dbContext.SaveChangesAsync();

            // Assert
            createdPerson.ShouldNotBeNull();
            Assert.Equal(peopleRepoModel.CCNumber, createdPerson.CCNumber);
            Assert.Equal(peopleRepoModel.TaxNumber, createdPerson.TaxNumber);
            Assert.Equal(peopleRepoModel.SSNumber, createdPerson.SSNumber);
            Assert.Equal(peopleRepoModel.Adress, createdPerson.Adress);
            Assert.Equal(peopleRepoModel.CivilState, createdPerson.CivilState);
            Assert.Equal(peopleRepoModel.Country, createdPerson.Country);
            Assert.Equal(peopleRepoModel.CreatedBy, createdPerson.CreatedBy);
            Assert.Equal(peopleRepoModel.FirstName, createdPerson.FirstName);
            Assert.Equal(peopleRepoModel.Postal, createdPerson.Postal);
            Assert.Equal(peopleRepoModel.Status, createdPerson.Status);
            Assert.Equal(peopleRepoModel.ChangedBy, createdPerson.ChangedBy);
            Assert.Equal(peopleRepoModel.Email, createdPerson.Email);
            Assert.Equal(DateTime.Parse("2999-12-31"), createdPerson.ExitDate);
            Assert.Equal(2, createdPerson.Employee_Id);

            var collaboratorHistory = dbContext.PersonHistory.FirstOrDefault(h => h.PeopleGUID == createdPerson.PeopleGUID);
            collaboratorHistory.ShouldNotBeNull();
            Assert.Equal("Create", collaboratorHistory.Action);
            Assert.Equal(DateTime.Now.Date, collaboratorHistory.ActionDate.Date);
            Assert.Equal(userId.ToString(), collaboratorHistory.UserID);
        }

        [Fact]
        public async Task GetOneCollaborator_Returns_Correct_Collaborator()
        {
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            var dbContext = new PeopleManagementDbContext(options);

            var personId = Guid.NewGuid();
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = personId,
                LastName = "Godinho",
                Email = "dgodinho@mainhub.pt",
                Locality = "Lisboa",
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                CreatedBy = "None",
                FirstName = "João",
                Postal = "1500",
                Status = "Active",
                ChangedBy = "None",
            });
            await dbContext.SaveChangesAsync();

            // Act
            var repo = new PeopleRepository(dbContext);
            var result = await repo.GetOneCollaborator(personId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(personId, result.PeopleGUID);
        }

        [Fact]
        public async Task GetAllCollaborators_WithValidParameters_ReturnsPagedCollaboratorsOrderedByName()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new PeopleManagementDbContext(options))
            {
                var repository = new PeopleRepository(dbContext);
                var collaborators = new List<Collaborator>
                {
                    new Collaborator {
                        PeopleGUID = Guid.NewGuid(),
                        FirstName = "Dinis",
                        LastName = "Godinho",
                        Email = "dgodinho@mainhub.pt",
                        Locality = "Lisboa",
                        CCNumber = "123456789",
                        TaxNumber = "123456789",
                        SSNumber = "123456789",
                        Adress = "Rua x",
                        CivilState = "Single",
                        Country = "Portugal",
                        CreatedBy = "None",
                        Postal = "1500",
                        Status = "Active",
                        ChangedBy = "None",
                    },
                    new Collaborator {
                        PeopleGUID = Guid.NewGuid(),
                        FirstName = "João",
                        LastName = "Monteiro",
                        Email = "jmonteiro@mainhub.pt",
                        Locality = "Lisboa",
                        CCNumber = "987654321",
                        TaxNumber = "987654321",
                        SSNumber = "987654321",
                        Adress = "Rua x",
                        CivilState = "Single",
                        Country = "Portugal",
                        CreatedBy = "None",
                        Postal = "1500",
                        Status = "Active",
                        ChangedBy = "None",
                    },
                };

                await dbContext.Person.AddRangeAsync(collaborators);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new PeopleManagementDbContext(options))
            {
                var repository = new PeopleRepository(dbContext);
                int page = 1;
                int pageSize = 2;
                string filter = string.Empty;
                State? list = null;

                // Act
                var result = await repository.GetAllCollaborators(page, pageSize, filter, list);

                // Assert
                result.ShouldNotBeNull();
                result.Count.ShouldBe(2);
                Assert.Equal("Dinis", result[0].FirstName);
                Assert.Equal("João", result[1].FirstName);
            }
        }

        [Fact]
        public async Task GetAllCollaborators_ListIs1_FilterNotNull_ReturnsFilteredCollaborators()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new PeopleManagementDbContext(options);

            var personid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;
            var filter = "d";
            State? list = State.All;
            var collaborators = new List<Collaborator>
            {
                new Collaborator
            {
                PeopleGUID = personid,
                FirstName = "Dinis",
                LastName = "Godinho",
                Email = "dgodinho@mainhub.pt",
            },
            new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Joao",
                LastName = "Monteiro",
                Email = "jmonteiro@mainhub.pt",
            }
        };
            await dbContext.Person.AddRangeAsync(collaborators);
            await dbContext.SaveChangesAsync();


            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.GetAllCollaborators(page, pageSize, filter, list);

            // Assert
            Assert.NotNull(result);
            result.Count.ShouldBe(1);
            Assert.Contains(result, c => c.FirstName == "Dinis");
        }
        [Fact]
        public async Task GetAllCollaborators_ListIs1_FilterNull_ReturnsAllCollaborators()
        {
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var dbContext = new PeopleManagementDbContext(options);

            var personid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;
            string filter = string.Empty;
            State? list = State.All;
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = personid,
                FirstName = "Dinis",
                LastName = "Godinho",
                Status = "Active",
                Email = "dgodinho@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "João",
                LastName = "Monteiro",
                Status = "Inactive",
                Email = "jmonteiro@mainhub.pt",
            });
            await dbContext.SaveChangesAsync();


            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.GetAllCollaborators(page, pageSize, filter, list);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.FirstName == "Dinis");
            Assert.Contains(result, c => c.FirstName == "João");
        }
        [Fact]
        public async Task GetAllCollaborators_ListIs2_FilterNotNull_ReturnsFilteredCollaborators()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new PeopleManagementDbContext(options);

            var personid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;
            var filter = "d";
            State? list = State.Inactive;
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = personid,
                FirstName = "Dinis",
                LastName = "Godinho",
                ExitDate = DateTime.Now.AddDays(-5),
                Status = "Inactive",
                Email = "dgodinho@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Duarte",
                LastName = "Taveira",
                ExitDate = DateTime.Now.AddDays(5),
                Status = "Active",
                Email = "dtaveira@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Diogo",
                LastName = "Lopes",
                ExitDate = DateTime.Now.AddDays(-5),
                Status = "Inactive",
                Email = "dlopes@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Joao",
                LastName = "Monteiro",
                ExitDate = DateTime.Now.AddDays(5),
                Status = "Active",
                Email = "jmonteiro@mainhub.pt",
            });
            await dbContext.SaveChangesAsync();


            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.GetAllCollaborators(page, pageSize, filter, list);

            // Assert
            Assert.NotNull(result);
            result.Count.ShouldBe(2);
            Assert.Contains(result, c => c.FirstName == "Dinis");
            Assert.Contains(result, c => c.FirstName == "Diogo");
        }
        [Fact]
        public async Task GetAllCollaborators_ListIs2_FilterNull_ReturnsAllCollaborators()
        {
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var dbContext = new PeopleManagementDbContext(options);

            // Add test data
            var personid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;
            string filter = string.Empty;
            State? list = State.Inactive;
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = personid,
                FirstName = "Dinis",
                LastName = "Godinho",
                ExitDate = DateTime.Now.AddDays(-5),
                Status = "Inactive",
                Email = "dgodinho@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Duarte",
                LastName = "Taveira",
                ExitDate = DateTime.Now.AddDays(5),
                Status = "Active",
                Email = "dtaveira@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Diogo",
                LastName = "Lopes",
                ExitDate = DateTime.Now.AddDays(-5),
                Status = "Inactive",
                Email = "dlopes@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "João",
                LastName = "Monteiro",
                ExitDate = DateTime.Now.AddDays(-5),
                Status = "Inactive",
                Email = "jmonteiro@mainhub.pt",
            });
            await dbContext.SaveChangesAsync();


            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.GetAllCollaborators(page, pageSize, filter, list);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count);
            Assert.Contains(result, c => c.FirstName == "Dinis");
            Assert.Contains(result, c => c.FirstName == "João");
            Assert.Contains(result, c => c.FirstName == "Diogo");
        }
        [Fact]
        public async Task GetAllCollaborators_ListIs3_FilterNotNull_ReturnsFilteredCollaborators()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var dbContext = new PeopleManagementDbContext(options);

            // Add test data
            var personid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;
            var filter = "d";
            State? list = State.Active;
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = personid,
                FirstName = "Dinis",
                LastName = "Godinho",
                ExitDate = DateTime.Now.AddDays(-5),
                Status = "Inactive",
                Email = "dgodinho@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Duarte",
                LastName = "Taveira",
                ExitDate = DateTime.Now.AddDays(5),
                Status = "Active",
                Email = "dtaveira@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Diogo",
                LastName = "Lopes",
                ExitDate = DateTime.Now.AddDays(-5),
                Status = "Inactive",
                Email = "dlopes@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Joao",
                LastName = "Monteiro",
                ExitDate = DateTime.Now.AddDays(5),
                Status = "Active",
                Email = "jmonteiro@mainhub.pt",
            });
            await dbContext.SaveChangesAsync();


            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.GetAllCollaborators(page, pageSize, filter, list);

            // Assert
            Assert.NotNull(result);
            result.Count.ShouldBe(1);
            Assert.Contains(result, c => c.FirstName == "Duarte");
        }
        [Fact]
        public async Task GetAllCollaborators_ListIs3_FilterNull_ReturnsAllCollaborators()
        {
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
               .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
               .Options;

            var dbContext = new PeopleManagementDbContext(options);

            // Add test data
            var personid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;
            string filter = string.Empty;
            State? list = State.Active;
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = personid,
                FirstName = "Dinis",
                LastName = "Godinho",
                ExitDate = DateTime.Now.AddDays(-5),
                Status = "Inactive",
                Email = "dgodinho@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Duarte",
                LastName = "Taveira",
                ExitDate = DateTime.Now.AddDays(5),
                Status = "Active",
                Email = "dtaveira@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "Diogo",
                LastName = "Lopes",
                ExitDate = DateTime.Now.AddDays(-5),
                Status = "Inactive",
                Email = "dlopes@mainhub.pt",
            });
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = Guid.NewGuid(),
                FirstName = "João",
                LastName = "Monteiro",
                ExitDate = DateTime.Now.AddDays(5),
                Status = "Active",
                Email = "jmonteiro@mainhub.pt",
            });
            await dbContext.SaveChangesAsync();


            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.GetAllCollaborators(page, pageSize, filter, list);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.FirstName == "Duarte");
            Assert.Contains(result, c => c.FirstName == "João");
        }

        [Fact]
        public async Task DeleteCollaboratorAsync_WithValidPersonId_DeletesCollaboratorAndReturnsTrue()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var dbContext = new PeopleManagementDbContext(options);

            var personid = Guid.NewGuid();
            Guid userId = Guid.NewGuid();
            dbContext.Person.Add(new Collaborator
            {
                PeopleGUID = personid,
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                CreatedBy = "None",
                FirstName = "user",
                LastName = "lastName",
                Postal = "1500",
                Status = "Active",
                ChangedBy = "None",
                Email = "uLastName@mainhub.pt",
                Locality = "Lisboa"
            });
            await dbContext.SaveChangesAsync();

            // Act
            var repo = new PeopleRepository(dbContext);
            var result = await repo.DeleteCollaboratorAsync(personid, userId);

            // Assert
            Assert.True(result);
            var deletedCollaborator = dbContext.Person.FirstOrDefault(a => a.PeopleGUID == personid);
            Assert.Null(deletedCollaborator);

            var deletedCollaboratorHistory = dbContext.PersonHistory.FirstOrDefault(h => h.PeopleGUID == personid);
            Assert.NotNull(deletedCollaboratorHistory);
            Assert.Equal("Deleted", deletedCollaboratorHistory.Action);
            Assert.Equal(DateTime.Now.Date, deletedCollaboratorHistory.ActionDate.Date);
        }


        [Fact]
        public async Task DeleteCollaboratorAsync_WithInvalidPersonId_ReturnsFalse()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var dbContext = new PeopleManagementDbContext(options);

            var personid = Guid.NewGuid();

            // Act
            var repo = new PeopleRepository(dbContext);
            var result = await repo.DeleteCollaboratorAsync(personid, userId);

            // Assert
            Assert.False(result);
        }


        [Fact]
        public void UpdateCollaboratorAsync_WithValidParameters_ReturnsTrue()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var dbContext = new PeopleManagementDbContext(options);

            var existingCollaborator = new Collaborator
            {
                PeopleGUID = personId,
                LastName = "Godinho",
                Email = "dgodinho@mainhub.pt",
                Locality = "Lisboa",
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                CreatedBy = "None",
                FirstName = "user",
                Postal = "1500",
                Status = "Active",
                ChangedBy = "",
            };

            dbContext.Person.Add(existingCollaborator);
            dbContext.SaveChanges();

            var updatedCollaborator = new PeopleRepoModel
            {
                FirstName = "Dinis",
                LastName = "Godinho",
                Email = "dgodinho@mainhub.pt",
                Locality = "Lisboa",
                PeopleGUID = personId,
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua y",
                CivilState = "Married",
                Country = "Spain",
                Postal = "2000",
                Status = "Inactive",
            };

            var repository = new PeopleRepository(dbContext);

            // Act
            var result = repository.UpdateCollaboratorAsync(personId, updatedCollaborator, userId).GetAwaiter().GetResult();

            // Assert
            Assert.True(result);

            var updatedCollaboratorFromDb = dbContext.Person.FirstOrDefault(c => c.PeopleGUID == personId);
            Assert.NotNull(updatedCollaboratorFromDb);
            Assert.Equal(updatedCollaborator.Adress, updatedCollaboratorFromDb.Adress);
            Assert.Equal(updatedCollaborator.CivilState, updatedCollaboratorFromDb.CivilState);
            Assert.Equal(updatedCollaborator.Country, updatedCollaboratorFromDb.Country);
            Assert.Equal(updatedCollaborator.Postal, updatedCollaboratorFromDb.Postal);
            Assert.Equal(updatedCollaborator.Status, updatedCollaboratorFromDb.Status);
            Assert.Equal(updatedCollaborator.ChangedBy, updatedCollaboratorFromDb.ChangedBy);

            var collaboratorHistoryFromDb = dbContext.PersonHistory.FirstOrDefault(h => h.PeopleGUID == personId);
            Assert.NotNull(collaboratorHistoryFromDb);
            Assert.Equal("Update", collaboratorHistoryFromDb.Action);
            Assert.Equal(userId.ToString(), collaboratorHistoryFromDb.UserID);
        }

        [Fact]
        public async Task UpdateCollaboratorAsync_WithNonexistentCollaborator_ReturnsFalse()
        {
            // Arrange
            var personId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using var dbContext = new PeopleManagementDbContext(options);

            var updatedCollaborator = new PeopleRepoModel
            {
                FirstName = "Dinis Godinho",
                LastName = "Godinho",
                Email = "dgodinho@mainhub.pt",
                Locality = "Lisboa",
                PeopleGUID = personId,
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                Postal = "1500",
                Status = "Active",
            };

            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.UpdateCollaboratorAsync(personId, updatedCollaborator, userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetCollaboratorHistory_WithValidIdAndPage_ReturnsPagedHistory()
        {
            // Arrange
            var id = Guid.NewGuid();
            var page = 1;
            var pageSize = 2;

            var expectedHistory = new List<CollaboratorHistory>
            {
                new CollaboratorHistory
                {
                FirstName = "Dinis",
                LastName = "Godinho",
                Email = "dgodinho@mainhub.pt",
                Locality = "Lisboa",
                PeopleGUID = id,
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                CreatedBy = "None",
                Postal = "1500",
                Status = "Active",
                ChangedBy = "None",
                UserID = Guid.NewGuid().ToString(),
            },
                new CollaboratorHistory
                {
                FirstName = "João",
                LastName = "Monteiro",
                Email = "jmonteiro@mainhub.pt",
                Locality = "Lisboa",
                PeopleGUID = id,
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                CreatedBy = "None",
                Postal = "1500",
                Status = "Active",
                ChangedBy = "None",
                UserID = Guid.NewGuid().ToString(),
                }
            };

            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                 .UseInMemoryDatabase(databaseName: "TestDatabase")
                 .Options;

            using var dbContext = new PeopleManagementDbContext(options);

            await dbContext.PersonHistory.AddRangeAsync(expectedHistory);
            await dbContext.SaveChangesAsync();

            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.GetCollaboratorHistory(id, page, pageSize);

            // Assert
            result.ShouldNotBeNull();
            Assert.Equal(expectedHistory.Count, result.Count);
        }

        [Fact]
        public async Task GetCollaboratorHistory_WithInvalidId_ReturnsEmptyHistory()
        {
            // Arrange
            var id = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                 .UseInMemoryDatabase(databaseName: "TestDatabase")
                 .Options;

            using var dbContext = new PeopleManagementDbContext(options);

            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.GetCollaboratorHistory(id, page, pageSize);

            // Assert
            result.ShouldNotBeNull();
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetCollaboratorHistory_WithZeroPage_ReturnsPagedHistoryWithPageOne()
        {
            // Arrange
            var id = Guid.NewGuid();
            var page = 0;
            var pageSize = 2;

            var expectedHistory = new List<CollaboratorHistory>
            {
                new CollaboratorHistory
                {
                FirstName = "Dinis",
                LastName = "Godinho",
                Email = "dgodinho@mainhub.pt",
                Locality = "Lisboa",
                PeopleGUID = id,
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                CreatedBy = "None",
                Postal = "1500",
                Status = "Active",
                ChangedBy = "None",
                UserID = Guid.NewGuid().ToString(),
            },
                new CollaboratorHistory
                {
                FirstName = "João",
                LastName = "Monteiro",
                Email = "jmonteiro@mainhub.pt",
                Locality = "Lisboa",
                PeopleGUID = id,
                CCNumber = "123456789",
                TaxNumber = "123456789",
                SSNumber = "123456789",
                Adress = "Rua x",
                CivilState = "Single",
                Country = "Portugal",
                CreatedBy = "None",
                Postal = "1500",
                Status = "Active",
                ChangedBy = "None",
                UserID = Guid.NewGuid().ToString(),
                }
            };

            var options = new DbContextOptionsBuilder<PeopleManagementDbContext>()
                 .UseInMemoryDatabase(databaseName: "TestDatabase")
                 .Options;

            using var dbContext = new PeopleManagementDbContext(options);

            await dbContext.PersonHistory.AddRangeAsync(expectedHistory);
            await dbContext.SaveChangesAsync();

            var repository = new PeopleRepository(dbContext);

            // Act
            var result = await repository.GetCollaboratorHistory(id, page, pageSize);

            // Assert
            result.ShouldNotBeNull();
            Assert.Equal(expectedHistory.Count, result.Count);
        }

    }
}
