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

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            List<PeopleResponseDto> result = await _peopleService.GetAllAsync(cancellationToken);

            return Ok(new ApiResponse<List<PeopleResponseDto>>
            {
                Data = result,
                Success = true,
            });
        }

        [HttpGet("get-by-cpf/{cpf}")]
        public async Task<IActionResult> GetByCpf([FromRoute] string cpf, CancellationToken cancellationToken)
        {
            PeopleResponseDto result = await _peopleService.GetByCpfAsync(cpf, cancellationToken);

            return Ok(new ApiResponse<PeopleResponseDto>
            {
                Data = result,
                Success = true,
            });
        }
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteById([FromRoute] long id, CancellationToken cancellationToken)
        {
            await _peopleService.DeleteByIdAsync(id, cancellationToken);

            return Ok(new ApiResponse<string>
            {
                Data = "Pessoa deletada com sucesso.",
                Success = true,
            });
        }

        [HttpPut("update/{id}")]
        public async Task<IActionResult> UpdateById([FromRoute] long id, [FromBody] UpdatePeopleRequestJson request, CancellationToken cancellationToken)
        {
            UpdatePeopleRequestDto dto = _mapper.Map<UpdatePeopleRequestDto>(request);
            PeopleResponseDto result = await _peopleService.UpdateByIdAsync(id, dto, cancellationToken);
            return Ok(new ApiResponse<PeopleResponseDto>
            {
                Data = result,
                Success = true,
            });
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<IActionResult> GetById([FromRoute] long id, CancellationToken cancellationToken)
        {
            PeopleResponseDto result = await _peopleService.GetByIdAsync(id, cancellationToken);

            return Ok(new ApiResponse<PeopleResponseDto>
            {
                Data = result,
                Success = true,
            });
        }
    }
}
