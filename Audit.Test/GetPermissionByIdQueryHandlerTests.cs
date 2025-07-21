using Audit.Application.permission.query;
using Audit.Core.Common;
using Audit.Core;
using Audit.Core.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Audit.Test
{
    public  class GetPermissionByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<GetPermissionByIdQuery>> _loggerMock;
        private readonly Mock<IProducerRepository> _producerRepositoryMock;
        private readonly GetPermissionByIdQuery.GetPermissionByIdQueryHandler _handler;

        public GetPermissionByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<GetPermissionByIdQuery>>();
            _producerRepositoryMock = new Mock<IProducerRepository>();

            _handler = new GetPermissionByIdQuery.GetPermissionByIdQueryHandler(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _producerRepositoryMock.Object
            );
        }
        [Fact]        
        public async Task Handle_ReturnsListOfPermissions_WhenSuccess()
        {
            // Arrange
            var fakePermission = new Permission
            {
                Id = 1,
                EmployeeForename = "Juan",
                EmployeeSurname = "Perez"
            };

            _unitOfWorkMock.Setup(u => u.Permissions.GetById(1))
                            .ReturnsAsync(fakePermission);
            _producerRepositoryMock.Setup(p => p.SendAsync(It.IsAny<string>(), (OperationEvent)It.IsAny<object>()))
                                   .ReturnsAsync(true);

            var query = new GetPermissionByIdQuery { Id = 1 };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Result);
            Assert.Equal("Juan", result.Result.EmployeeForename);
        }


    }
}
