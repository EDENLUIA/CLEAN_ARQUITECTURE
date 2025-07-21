using Audit.Application.permission.cmd;
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
    public class UpadtePermissionCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<UpadtePermissionCommand>> _loggerMock;
        private readonly Mock<IProducerRepository> _producerRepositoryMock;
        private readonly Mock<IPermissionElasticsearchRepository> _elasticRepo;
        private readonly UpadtePermissionCommand.UpadtePermissionCommandHandler _handler;

        public UpadtePermissionCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpadtePermissionCommand>>();
            _producerRepositoryMock = new Mock<IProducerRepository>();
            _elasticRepo = new Mock<IPermissionElasticsearchRepository>();
            _handler = new UpadtePermissionCommand.UpadtePermissionCommandHandler(
                _unitOfWorkMock.Object,
                _loggerMock.Object,
                _producerRepositoryMock.Object,
                _elasticRepo.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenUpdateIsOk()
        {
            // Arrange
            var command = new UpadtePermissionCommand
            {
                Id = 1,
                EmployeeForename = "Juan",
                EmployeeSurname = "Aliaga",
                PermissionDate = DateTime.UtcNow,
                PermissionType = 1
            };

            // Configuramos mocks
            _unitOfWorkMock.Setup(u => u.Permissions.Update(It.IsAny<Permission>()))
                           .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.Save())
                           .Returns(1);

            _elasticRepo.Setup(u => u.IndexAsync(It.IsAny<Permission>()))
                        .ReturnsAsync(true);

            _producerRepositoryMock.Setup(p => p.SendAsync(It.IsAny<string>(), (OperationEvent)It.IsAny<object>()))
                                   .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.Equal("Successful Operation", result.Message);
            Assert.Equal("1", result.Result); 
        }
    }
}
