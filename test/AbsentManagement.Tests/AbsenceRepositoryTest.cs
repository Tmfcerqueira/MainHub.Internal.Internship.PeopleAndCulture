using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Controllers;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Database.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Extensions;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Moq;
using PeopleManagementDataBase;
using Shouldly;
using Xunit;

namespace MainHub.Internal.PeopleAndCulture.Tests.AbsentManagementTests
{
    public class AbsenceRepositoryTests
    {
        //creates

        [Fact]
        public void Constructor_Should_Inject_AbsenceManagementDbContext()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            // Act
            // Instance of AbsenceRepository with the mock context as a parameter
            var repository = new AbsenceRepository(dbContext);

            // Assert
            // Verify that the repository instance is not null and that its _dbContext property is set to the mock context object
            repository.ShouldNotBeNull();
            repository.SkillHubDb.ShouldBe(dbContext);
        }


        [Fact]
        public async Task Create_Should_Create_Absence_With_Correct_Properties()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);
            var repository = new AbsenceRepository(dbContext);

            var absenceRepoModel = new AbsenceRepoModel
            {
                AbsenceGuid = Guid.Empty,
                PersonGuid = Guid.Empty,
                AbsenceTypeGuid = Guid.Empty,
                ApprovedBy = "None",
                ApprovalStatus = ApprovalStatus.Draft,
                ApprovalDate = new DateTime(2999, 12, 31, 23, 59, 59),
                SubmissionDate = new DateTime(2999, 12, 31, 23, 59, 59)
            };

            // Act
            var createdAbsence = await repository.CreateAbsence(absenceRepoModel, It.IsAny<Guid>());

            // Assert
            createdAbsence.ShouldNotBeNull();
        }





        [Fact]
        public async Task CreateTypeAbsence_Should_Create_AbsenceType_With_Correct_Properties()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);
            var repository = new AbsenceRepository(dbContext);

            var absenceTypeRepoModel = new AbsenceTypeRepoModel
            {
                TypeGuid = Guid.Empty,
                Type = "Has no Type"
            };

            // Act
            var createdAbsenceType = await repository.CreateAbsenceType(absenceTypeRepoModel, It.IsAny<Guid>());

            // Assert
            createdAbsenceType.ShouldNotBeNull();
        }



        //gets
        [Fact]
        public async Task GetAbsencesByPerson_ReturnsExpectedAbsences()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            // Add test data
            var personId = Guid.NewGuid();
            var year = DateTime.Now.Year;
            var expectedAbsences = new List<Absence>
            {
                new Absence { PersonGuid = personId, AbsenceStart = new DateTime(year, 1, 1), ApprovalStatus = ApprovalStatus.Approved },
                new Absence { PersonGuid = personId, AbsenceStart = new DateTime(year, 2, 1), ApprovalStatus = ApprovalStatus.Draft },
                new Absence { PersonGuid = personId, AbsenceStart = new DateTime(year, 3, 1), ApprovalStatus = ApprovalStatus.Rejected }
            };
            dbContext.Absence.AddRange(expectedAbsences);
            await dbContext.SaveChangesAsync();

            var repo = new AbsenceRepository(dbContext);

            // Act 1: Test when status is ApprovalStatus.All
            var result1 = await repo.GetAbsencesByPerson(personId, year, 1, 1000, ApprovalStatus.All, new DateTime(), new DateTime());

            // Assert 1: Ensure all expected absences are returned
            Assert.NotNull(result1);
            Assert.Equal(expectedAbsences.Count, result1.absences.Count);
            foreach (var expectedAbsence in expectedAbsences)
            {
                var matchingAbsence = result1.absences.FirstOrDefault(a =>
                    a.PersonGuid == expectedAbsence.PersonGuid &&
                    a.AbsenceStart == expectedAbsence.AbsenceStart &&
                    a.ApprovalStatus == expectedAbsence.ApprovalStatus);

                Assert.NotNull(matchingAbsence);
            }

            // Act 2: Test when status is ApprovalStatus.Approved
            var result2 = await repo.GetAbsencesByPerson(personId, year, 1, 1000, ApprovalStatus.Approved, new DateTime(), new DateTime());

            // Assert 2: Ensure only approved absences are returned
            var expectedApprovedAbsences = expectedAbsences.Where(a => a.ApprovalStatus == ApprovalStatus.Approved).ToList();
            Assert.NotNull(result2);
            Assert.Equal(expectedApprovedAbsences.Count, result2.absences.Count);
            foreach (var expectedAbsence in expectedApprovedAbsences)
            {
                var matchingAbsence = result2.absences.FirstOrDefault(a =>
                    a.PersonGuid == expectedAbsence.PersonGuid &&
                    a.AbsenceStart == expectedAbsence.AbsenceStart &&
                    a.ApprovalStatus == expectedAbsence.ApprovalStatus);

                Assert.NotNull(matchingAbsence);
            }

            // Act 3: Test when status is ApprovalStatus.Draft
            var result3 = await repo.GetAbsencesByPerson(personId, year, 1, 1000, ApprovalStatus.Draft, new DateTime(), new DateTime());

            // Assert 3: Ensure only draft absences are returned
            var expectedDraftAbsences = expectedAbsences.Where(a => a.ApprovalStatus == ApprovalStatus.Draft).ToList();
            Assert.NotNull(result3);
            Assert.Equal(expectedDraftAbsences.Count, result3.absences.Count);
            foreach (var expectedAbsence in expectedDraftAbsences)
            {
                var matchingAbsence = result3.absences.Select(a =>
                    a.PersonGuid == expectedAbsence.PersonGuid &&
                    a.AbsenceStart == expectedAbsence.AbsenceStart &&
                a.ApprovalStatus == expectedAbsence.ApprovalStatus);

                Assert.NotNull(matchingAbsence);
            }

            // Act 4: Test when status is ApprovalStatus.Rejected
            var result4 = await repo.GetAbsencesByPerson(personId, year, 1, 1000, ApprovalStatus.Rejected, new DateTime(), new DateTime());

            // Assert 4: Ensure only rejected absences are returned
            var expectedRejectedAbsences = expectedAbsences.Where(a => a.ApprovalStatus == ApprovalStatus.Rejected).ToList();
            Assert.NotNull(result4);
            Assert.Equal(expectedRejectedAbsences.Count, result4.absences.Count);
            foreach (var expectedAbsence in expectedRejectedAbsences)
            {
                var matchingAbsence = result4.absences.Select(a =>
                    a.PersonGuid == expectedAbsence.PersonGuid &&
                    a.AbsenceStart == expectedAbsence.AbsenceStart &&
                    a.ApprovalStatus == expectedAbsence.ApprovalStatus);

                Assert.NotNull(matchingAbsence);
            }
        }



        [Fact]
        public async Task GetAbsenceByPerson_ReturnsExpectedAbsence()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            // Add test data
            var personId = Guid.NewGuid();
            var absenceId = Guid.NewGuid();
            dbContext.Absence.Add(new Absence { PersonGuid = personId, AbsenceGuid = absenceId });
            await dbContext.SaveChangesAsync();

            // Act
            var repo = new AbsenceRepository(dbContext);
            var result = await repo.GetAbsenceByPerson(personId, absenceId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(absenceId, result.AbsenceGuid);
            Assert.Equal(personId, result.PersonGuid);
        }

        [Fact]
        public async Task GetAbsenceByPerson_Returns_Null_When_Absence_Null()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            // Add test data
            var personId = Guid.NewGuid();
            var absenceId = Guid.NewGuid();

            // Act
            var repo = new AbsenceRepository(dbContext);
            var result = await repo.GetAbsenceByPerson(personId, absenceId);

            // Assert
            Assert.Null(result);
        }


        [Fact]
        public async Task GetAllAbsences_ReturnsAllAbsences()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            // Add test data
            var person1 = new Collaborator();
            var person2 = new Collaborator();
            dbContext.Person.AddRange(person1, person2);
            await dbContext.SaveChangesAsync();

            var absence1 = new Absence { PersonGuid = person1.PeopleGUID, AbsenceGuid = Guid.NewGuid() };
            var absence2 = new Absence { PersonGuid = person2.PeopleGUID, AbsenceGuid = Guid.NewGuid() };
            dbContext.Absence.AddRange(absence1, absence2);
            await dbContext.SaveChangesAsync();

            var repo = new AbsenceRepository(dbContext);

            // Act
            var result = await repo.GetAllAbsences(1, 1000, Common.ApprovalStatus.All);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.totalCount);
        }



        [Fact]
        public async Task GetAllAbsenceTypes_ReturnsAllAbsenceTypes()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            // Add test data
            var absenceType1 = new AbsenceType { TypeGuid = Guid.NewGuid(), Type = "Vacation" };
            var absenceType2 = new AbsenceType { TypeGuid = Guid.NewGuid(), Type = "Sick" };
            dbContext.AbsenceType.AddRange(absenceType1, absenceType2);
            await dbContext.SaveChangesAsync();

            // Act
            var repo = new AbsenceRepository(dbContext);
            var result = await repo.GetAllAbsenceTypes();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Contains(result, r => r.TypeGuid == absenceType1.TypeGuid && r.Type == absenceType1.Type);
            Assert.Contains(result, r => r.TypeGuid == absenceType2.TypeGuid && r.Type == absenceType2.Type);
        }


        [Fact]
        public async Task DeleteAbsence_ReturnsTrue_When_AbsenceExists()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            // Add test data
            var absenceId = Guid.NewGuid();
            dbContext.Absence.Add(new Absence { AbsenceGuid = absenceId });
            await dbContext.SaveChangesAsync();

            // Act
            var repo = new AbsenceRepository(dbContext);
            var result = await repo.DeleteAbsence(absenceId, It.IsAny<Guid>());

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAbsence_ReturnsFalse_When_AbsenceDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            // Act
            var repo = new AbsenceRepository(dbContext);
            var result = await repo.DeleteAbsence(Guid.NewGuid(), It.IsAny<Guid>());

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task UpdateAbsence_ReturnsTrue_When_AbsenceExistsAndIsUpdated()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
            .UseInMemoryDatabase(databaseName: "test_db")
            .Options;
            using var dbContext = new AbsenceManagementDbContext(options);

            // Add test data
            var absenceId = Guid.NewGuid();
            var absenceType = new AbsenceType { TypeGuid = Guid.NewGuid(), Type = "Vacation" };
            dbContext.AbsenceType.Add(absenceType);
            await dbContext.SaveChangesAsync();
            dbContext.Absence.Add(new Absence
            {
                AbsenceGuid = absenceId,
                AbsenceStart = DateTime.Today.AddDays(1),
                AbsenceEnd = DateTime.Today.AddDays(2),
                AbsenceTypeGuid = absenceType.TypeGuid,
                Schedule = "Schedule",
                SubmissionDate = DateTime.Today.AddDays(-1)
            });
            await dbContext.SaveChangesAsync();

            var updatedAbsence = new AbsenceRepoModel
            {
                AbsenceGuid = absenceId,
                AbsenceStart = DateTime.Today.AddDays(3),
                AbsenceEnd = DateTime.Today.AddDays(4),
                AbsenceTypeGuid = absenceType.TypeGuid,
                Schedule = "Updated schedule"
            };

            // Act
            var repo = new AbsenceRepository(dbContext);
            var result = await repo.UpdateAbsence(updatedAbsence, It.IsAny<Guid>());

            // Assert
            Assert.True(result);

            var updatedAbsenceEntity = await dbContext.Absence.FirstOrDefaultAsync(a => a.AbsenceGuid == absenceId);
            Assert.NotNull(updatedAbsenceEntity);
            Assert.Equal(updatedAbsence.AbsenceStart, updatedAbsenceEntity.AbsenceStart);
            Assert.Equal(updatedAbsence.AbsenceEnd, updatedAbsenceEntity.AbsenceEnd);
            Assert.Equal(updatedAbsence.AbsenceTypeGuid, updatedAbsenceEntity.AbsenceTypeGuid);
            Assert.Equal(updatedAbsence.Schedule, updatedAbsenceEntity.Schedule);
            Assert.NotEqual(updatedAbsence.SubmissionDate, updatedAbsenceEntity.SubmissionDate); // Check that SubmissionDate is updated
        }

        [Fact]
        public async Task UpdateAbsence_ReturnsFalse_When_AbsenceDoesNotExist()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using var dbContext = new AbsenceManagementDbContext(options);

            // Add test data
            var absenceType = new AbsenceType { TypeGuid = Guid.NewGuid(), Type = "Vacation" };
            dbContext.AbsenceType.Add(absenceType);
            await dbContext.SaveChangesAsync();

            var updatedAbsence = new AbsenceRepoModel
            {
                AbsenceGuid = Guid.NewGuid(),
                AbsenceStart = DateTime.Today.AddDays(3),
                AbsenceEnd = DateTime.Today.AddDays(4),
                AbsenceTypeGuid = absenceType.TypeGuid,
                Schedule = "Updated schedule"
            };

            // Act
            var repo = new AbsenceRepository(dbContext);
            var result = await repo.UpdateAbsence(updatedAbsence, It.IsAny<Guid>());

            // Assert
            Assert.False(result);
        }

        //admin

        [Fact]
        public async Task GetAllAbsences_ReturnsListOfAbsenceRepoModel()
        {
            // Arrange
            var page = 1;
            var pageSize = 10;
            var status = Common.ApprovalStatus.Approved;

            var absences = new List<Absence>
            {
                new Absence { IsDeleted = false, ApprovalStatus = status, AbsenceGuid = new Guid()},
                new Absence { IsDeleted = false, ApprovalStatus = status, AbsenceGuid =  new Guid() },
                new Absence { IsDeleted = false, ApprovalStatus = status, AbsenceGuid =  new Guid() }
            };

            var expectedAbsenceRepoModels = absences.Select(a => a.ToAbsenceRepoModel()).ToList();

            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using (var dbContext = new AbsenceManagementDbContext(options))
            {
                dbContext.Absence.AddRange(absences);
                await dbContext.SaveChangesAsync();
            }

            using (var dbContext = new AbsenceManagementDbContext(options))
            {
                var repository = new AbsenceRepository(dbContext);

                // Act
                var result = await repository.GetAllAbsences(page, pageSize, status);

                // Assert
                Assert.NotNull(result);
            }
        }




        [Fact]
        public async Task GetAbsencesHistory_ReturnsCorrectAbsences()
        {
            // Arrange
            var id = Guid.NewGuid();
            var page = 1;
            var pageSize = 10;

            var options = new DbContextOptionsBuilder<AbsenceManagementDbContext>()
                .UseInMemoryDatabase(databaseName: "test_db")
                .Options;

            using (var dbContext = new AbsenceManagementDbContext(options))
            {
                // Add test data to the in-memory database
                var absenceHistories = new List<AbsenceHistory>
            {
                new AbsenceHistory
                {
                    AbsenceGuid = id,
                    PersonGuid = Guid.NewGuid(),
                    // Add other properties as needed
                },
                // Add more test data as needed
            };
                await dbContext.AbsenceHistory.AddRangeAsync(absenceHistories);
                await dbContext.SaveChangesAsync();

                var repository = new AbsenceRepository(dbContext);

                // Act
                var result = await repository.GetAbsencesHistory(id, page, pageSize);

                // Assert
                Assert.NotNull(result);
                Assert.IsType<List<AbsenceHistoryRepoModel>>(result.absences);
                // Perform additional assertions as needed based on the expected behavior of the method
            }
        }



    }
}
