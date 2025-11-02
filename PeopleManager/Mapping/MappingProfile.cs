using AutoMapper;
using PeopleManager.API.Models;
using PeopleManager.Application.DTOs.Requests;

namespace PeopleManager.API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreatePeopleRequestJson, CreatePeopleRequestDto>();
            CreateMap<UpdatePeopleRequestJson, UpdatePeopleRequestDto>();
        }
    }
}
