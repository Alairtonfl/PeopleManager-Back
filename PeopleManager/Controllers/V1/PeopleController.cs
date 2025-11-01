using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManager.API.Models;
using PeopleManager.Application.DTOs.Requests;
using PeopleManager.Application.DTOs.Responses;
using PeopleManager.Application.Interfaces.Services;

namespace PeopleManager.API.Controllers.V1
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [ApiVersion("1.0")]
    [ApiExplorerSettings(GroupName = "v1")]
    [Authorize]
    public class PeopleController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPeopleService _peopleService;
        public PeopleController(IMapper mapper, IPeopleService peopleService)
        {
            _mapper = mapper;
            _peopleService = peopleService;
        }

        [HttpPost("create")]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreatePeopleRequestJson request, CancellationToken cancellationToken)
        {
            CreatePeopleRequestDto dto = _mapper.Map<CreatePeopleRequestDto>(request);

            PeopleResponseDto result = await _peopleService.CreateAsync(dto, cancellationToken);

            return Ok(new ApiResponse<PeopleResponseDto>
            {
                Data = result,
                Success = true,
            });
        }
    }
}
