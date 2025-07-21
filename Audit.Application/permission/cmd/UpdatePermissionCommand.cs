using Audit.Core.Common;
using Audit.Core.Interfaces;
using Audit.Core;
using MediatR;
using Serilog;
using Microsoft.Extensions.Logging;



namespace Audit.Application.permission.cmd
{
    public class UpadtePermissionCommand : Permission, IRequest<ApiResponse<string>>
    {

        public class UpadtePermissionCommandHandler : IRequestHandler<UpadtePermissionCommand, ApiResponse<string>>
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly ILogger<UpadtePermissionCommand> _logger;
            private readonly IProducerRepository _producerRepository;
            private readonly IPermissionElasticsearchRepository _elasticRepo;

            public UpadtePermissionCommandHandler(IUnitOfWork unitOfWork,
                                                  ILogger<UpadtePermissionCommand> logger, 
                                                  IProducerRepository producerRepository,
                                                  IPermissionElasticsearchRepository elasticRepo)
            {
                _unitOfWork = unitOfWork;
                _logger = logger;
                _producerRepository = producerRepository;
                _elasticRepo = elasticRepo;
            }

            public async Task<ApiResponse<string>> Handle(UpadtePermissionCommand request, CancellationToken cancellationToken)
            {
                var apiResponse = new ApiResponse<string>();

                try
                {
                    await _unitOfWork.Permissions.Update(request);
                    var data = _unitOfWork.Save();

                    var elasticResult = await _elasticRepo.IndexAsync(request);

                    if (!elasticResult)
                    {
                        _logger.LogWarning("Documento no se indexó en Elasticsearch.");
                    }


                    var kafkaResponse = await _producerRepository.SendAsync("demo1", new OperationEvent(Guid.NewGuid(), "UPDATE PERMISSION"));

                    if (!kafkaResponse)
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
                    _logger.LogError(ex.Message);
                }

                return apiResponse;

            }
        }
    }
}
