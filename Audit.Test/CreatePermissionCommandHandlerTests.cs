using Audit.Application.permission.cmd;
using Audit.Core;
using Audit.Core.Common;
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
    public class CreatePermissionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<CreatePermissionCommand>> _loggerMock;
        private readonly Mock<IProducerRepository> _producerRepositoryMock;
        private readonly Mock<IPermissionElasticsearchRepository> _elasticRepo;
        private readonly CreatePermissionCommand.CreatePermissionCommandHandler _handler;
       

        public CreatePermissionCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<CreatePermissionCommand>>();
            _producerRepositoryMock = new Mock<IProducerRepository>();
            _elasticRepo = new Mock<IPermissionElasticsearchRepository>();
            _handler = new CreatePermissionCommand.CreatePermissionCommandHandler(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _producerRepositoryMock.Object,
                _elasticRepo.Object
            );
        }

        [Fact]
        public async Task Handle_ReturnsSuccess_WhenPermissionCreatedAndKafkaSucceeds()
        {
            // Arrange
            var command = new CreatePermissionCommand
            {
                EmployeeForename = "Luis",
                EmployeeSurname = "Pérez",
                PermissionType = 1,
                PermissionDate = DateTime.UtcNow
            };

            _unitOfWorkMock.Setup(u => u.Permissions.Add(It.IsAny<Permission>()))
                           .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.Save())
                           .Returns(1);

            _elasticRepo.Setup(u=> u.IndexAsync(It.IsAny<Permission>()))
                            .ReturnsAsync(true);

            _producerRepositoryMock.Setup(p => p.SendAsync(It.IsAny<string>(), (OperationEvent)It.IsAny<object>()))
                                   .ReturnsAsync(true); // Kafka OK

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Successful Operation", result.Message);
            Assert.Equal("1", result.Result);
        }
    }
}
