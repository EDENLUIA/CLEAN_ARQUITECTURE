using Audit.Core;
using Audit.Core.Common;
using Audit.Core.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;


namespace Audit.Application.permission.cmd
{
    public class CreatePermissionCommand : Permission, IRequest<ApiResponse<string>>
    {

        public class CreatePermissionCommandHandler : IRequestHandler<CreatePermissionCommand, ApiResponse<string>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<CreatePermissionCommand> _logger;
            private readonly IProducerRepository _producerRepository;
            private readonly IPermissionElasticsearchRepository _elasticRepo;

            public CreatePermissionCommandHandler(IUnitOfWork unitOfWork,
                                                  ILogger<CreatePermissionCommand> logger,
                                                  IProducerRepository producerRepository,
                                                  IPermissionElasticsearchRepository elasticRepo)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _producerRepository = producerRepository;
                _elasticRepo = elasticRepo;
            }

            public async Task<ApiResponse<string>> Handle(CreatePermissionCommand request, CancellationToken cancellationToken)
            {
                var apiResponse = new ApiResponse<string>();

                try
                {
                    await _unitOfWork.Permissions.Add(request);
                    var data = _unitOfWork.Save();

                  
                    var elasticResult = await _elasticRepo.IndexAsync(request);

                    if (!elasticResult)
                    {
                        _logger.LogWarning("Documento no se indexó en Elasticsearch.");                        
                    }

                    var kafkaResult = await _producerRepository.SendAsync("demo1", new OperationEvent(Guid.NewGuid(),"CREATE PERMISSION"));

                    if (!kafkaResult)
                    {
                        _logger.LogWarning("Peticion no se envío a Kafka");
                    }

                    apiResponse.Success = true;
                    apiResponse.Message = "Successful Operation";
                    apiResponse.Result = data.ToString();

                }
                catch (Exception ex)
                {                   
                    apiResponse.Success = false;
                    apiResponse.Message = ex.Message;
                    _logger.LogError(ex.Message.ToString());
                }
               
                return apiResponse;

            }
        }
    }
}
