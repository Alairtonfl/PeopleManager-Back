using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManager.API.Models;
using PeopleManager.Application.DTOs.Requests;

namespace PeopleManager.API.Controllers.V1
{
    [Route("api/[controller]")]
    public class PeopleController : ControllerBase
    {
        private readonly IMapper _mapper;
        public PeopleController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreatePeopleRequestJson request, CancellationToken cancellationToken)
        {
            CreatePeopleRequestDto dto = _mapper.Map<CreatePeopleRequestDto>(request);

            return Ok(new[] { "Alice", "Bob", "Charlie" });
        }
    }
}
