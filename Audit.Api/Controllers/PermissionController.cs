using Audit.Application.permission.query;
using Audit.Application.permission.cmd;
using Audit.Core;
using Audit.Core.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Audit.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PermissionController : ControllerBase
    {

       
        private readonly IMediator _mediator;
        private readonly ILogger<PermissionController> _logger;

        public PermissionController( IMediator mediator, ILogger<PermissionController> logger)
        {
            
            _mediator = mediator;
            _logger = logger;
        }

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ApiResponse<List<Permission>>> Get()
        {

            _logger.LogWarning("Get All Permissions");

            return await _mediator.Send(new GetAllPermissionQuery());
        }  

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ApiResponse<Permission>), StatusCodes.Status200OK)]
        public async Task<ApiResponse<Permission>> GetById(int id)
        {

            _logger.LogWarning($"Get Permission with id : {id}");

            var RequestQuery = new GetPermissionByIdQuery
            {
                Id = id
            };
           
            return await _mediator.Send(RequestQuery);
        }

        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<string>>> Create([FromBody] CreatePermissionCommand request)
        {
            _logger.LogWarning($"Grabando Permision : {request.EmployeeSurname}");
            var result = await _mediator.Send(request);
            return Ok(result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        public async Task<ActionResult<ApiResponse<string>>> Update([FromBody] UpadtePermissionCommand request)
        {
            _logger.LogWarning($"Actualizando Permision with id : {request.EmployeeSurname}");
            var result = await _mediator.Send(request);
            return Ok(result);
        }

    }

    

 }
