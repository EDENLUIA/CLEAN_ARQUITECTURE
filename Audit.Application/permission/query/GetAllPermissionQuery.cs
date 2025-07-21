using Audit.Application.permission.cmd;
using Audit.Core;
using Audit.Core.Common;
using Audit.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Audit.Application.permission.query
{
    public class GetAllPermissionQuery : IRequest<ApiResponse<List<Permission>>>
    {
        public GetAllPermissionQuery()
        {
        }
        public class GetAllPermissionQueryHandler : IRequestHandler<GetAllPermissionQuery, ApiResponse<List<Permission>>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<GetAllPermissionQuery> _logger;
            private readonly IProducerRepository _producerRepository;
            public GetAllPermissionQueryHandler(IUnitOfWork unitOfWork, ILogger<GetAllPermissionQuery> logger, IProducerRepository producerRepository)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _producerRepository = producerRepository;
            }
            public async Task<ApiResponse<List<Permission>>> Handle(GetAllPermissionQuery request, CancellationToken cancellationToken)
            {
                ApiResponse<List<Permission>> apiResponse = new ApiResponse<List<Permission>>();
                try
                {

                    var _permissions = (List<Permission>)await _unitOfWork.Permissions.GetAll();

                    var kafkaResponse = await _producerRepository.SendAsync("demo1", new OperationEvent(Guid.NewGuid(), "GET ALL PERMISSION"));

                    if (!kafkaResponse)
                    {
                        _logger.LogWarning("Peticion no se envío a Kafka");
                    }

                    apiResponse.Success = true;
                    apiResponse.Message = "Successful Operation";
                    apiResponse.Result = _permissions;
                }
                catch (Exception ex)
                {
                    apiResponse.Success = false;
                    apiResponse.Message = ex.Message;
                }

                return apiResponse;

            }
        }
    }
}
