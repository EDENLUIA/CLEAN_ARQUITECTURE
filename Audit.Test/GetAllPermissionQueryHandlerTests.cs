using Audit.Application.permission.query;
using Audit.Core;
using Audit.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using Audit.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Test
{
    public class GetAllPermissionQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetAllPermissionQuery>> _loggerMock;
        private readonly Mock<IProducerRepository> _producerRepositoryMock;
        private readonly GetAllPermissionQuery.GetAllPermissionQueryHandler _handler;

        public GetAllPermissionQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetAllPermissionQuery>>();
            _producerRepositoryMock = new Mock<IProducerRepository>();

            _handler = new GetAllPermissionQuery.GetAllPermissionQueryHandler(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _producerRepositoryMock.Object
            );
        }

        [Fact]
        public async Task Handle_ReturnsListOfPermissions_WhenSuccess()
        {
            // Arrange
            var fakePermissions = new List<Permission>
            {
                new Permission { Id = 1, EmployeeForename = "Juan", EmployeeSurname = "Perez" },
                new Permission { Id = 2, EmployeeForename = "Maria", EmployeeSurname = "Lopez" }
            };

            _unitOfWorkMock.Setup(u => u.Permissions.GetAll()).ReturnsAsync(fakePermissions);
            _producerRepositoryMock.Setup(p => p.SendAsync(It.IsAny<string>(), (OperationEvent)It.IsAny<object>()))
                                   .ReturnsAsync(true);

            var query = new GetAllPermissionQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal(2, result.Result.Count);
            Assert.Equal("Juan", result.Result[0].EmployeeForename);
        }
    }
}
