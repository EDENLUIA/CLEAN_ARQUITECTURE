using Audit.Core.Common;
using Audit.Core.Interfaces;
using Audit.Core;
using MediatR;
using Serilog;
using Microsoft.Extensions.Logging;

namespace Audit.Application.permission.query
{
    public class GetPermissionByIdQuery : Permission, IRequest<ApiResponse<Permission>>
    {
        public GetPermissionByIdQuery()
        {
        }
        public class GetPermissionByIdQueryHandler : IRequestHandler<GetPermissionByIdQuery, ApiResponse<Permission>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<GetPermissionByIdQuery> _logger;
            private readonly IProducerRepository _producerRepository;
            public GetPermissionByIdQueryHandler(IUnitOfWork unitOfWork, ILogger<GetPermissionByIdQuery> logger, IProducerRepository producerRepository)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _producerRepository = producerRepository;
            }
            public async Task<ApiResponse<Permission>> Handle(GetPermissionByIdQuery request, CancellationToken cancellationToken)
            {
                ApiResponse<Permission> apiResponse = new ApiResponse<Permission>();
                try
                {
                    var _permissions = (Permission)await _unitOfWork.Permissions.GetById(request.Id);

                    var kafkaResponse = await _producerRepository.SendAsync("demo1", new OperationEvent(Guid.NewGuid(), $"GET PERMISSION {request.Id}"));

                    if (!kafkaResponse)
                    {
                        _logger.LogWarning("Peticion no se envío a Kafka");
                    }

                    apiResponse.Success = _permissions == null ? false : true;
                    apiResponse.Message = _permissions == null ? "Not Found Permission" : "OK";
                    apiResponse.Result = _permissions == null ? new Permission() : _permissions;
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
