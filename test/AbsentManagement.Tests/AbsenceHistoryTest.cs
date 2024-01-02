using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Controllers;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.API.Models;
using MainHub.Internal.PeopleAndCulture.AbsentManagement.Repository.Models;
using MainHub.Internal.PeopleAndCulture.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MainHub.Internal.PeopleAndCulture
{
    public class AbsenceHistoryTest
    {


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
    }
}
