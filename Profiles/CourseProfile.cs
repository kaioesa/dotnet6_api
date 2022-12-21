using AutoMapper;
using dotnet6_api.Data.DTOs;
using webapi_dotnet6.Entity;

namespace dotnet6_api.Profiles
{
    public class CourseProfile : Profile
    {
        public CourseProfile()
        {
            CreateMap<CreateCourseDTO, Course>();
            CreateMap<UpdateCourseDTO, Course>();
            CreateMap<Course, UpdateCourseDTO>();
        }
    }
}
