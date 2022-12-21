using AutoMapper;
using dotnet6_api.Data.DTOs;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using webapi_dotnet6.Data;
using webapi_dotnet6.Entity;

namespace dotnet6_api.Controllers;

[ApiController]
[Route("[controller]")]
public class CourseController : ControllerBase
{
    private DataContext _context;
    private IMapper _mapper;

    public CourseController(DataContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public IEnumerable<ReadCourseDTO> GetAllCourses([FromQuery] int skip = 0, int take = 50)
    {
        return _mapper.Map<List<ReadCourseDTO>>(_context.Courses.Skip(skip).Take(take));
    }

    [HttpGet("{id}")]
    public IActionResult GetCourseById(int id)
    {
        var course = _context.Courses.FirstOrDefault(course => course.Id == id);
        if (course == null)
            return NotFound();

        var courseView = _mapper.Map<ReadCourseDTO>(course);
        return Ok(courseView);
    }

    /// <summary>
    /// Adiciona um curso ao banco de dados
    /// </summary>
    /// <param name="courseDTO">
    /// Objeto com campos nescessários para criação de um curso 
    /// </param>
    /// <returns>IActionResult</returns>
    /// <response code="201">Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AddCourse([FromBody] CreateCourseDTO courseDTO)
    {
        Course course = _mapper.Map<Course>(courseDTO);
        _context.Courses.Add(course);
        _context.SaveChanges();
        return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, course);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateCourse(int id, [FromBody] UpdateCourseDTO courseDTO)
    {
        var course = _context.Courses.FirstOrDefault(course => course.Id == id);

        if (course == null) return NotFound();
        _mapper.Map(courseDTO, course);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult PatchCourse(int id, [FromBody] JsonPatchDocument<UpdateCourseDTO> patch)
    {
        var course = _context.Courses.FirstOrDefault(course => course.Id == id);

        if (course == null) return NotFound();
        var courseToUpdate = _mapper.Map<UpdateCourseDTO>(course);

        patch.ApplyTo(courseToUpdate, ModelState);

        if (!TryValidateModel(courseToUpdate))
        {
            return ValidationProblem(ModelState);
        }
        _mapper.Map(courseToUpdate, course);
        _context.SaveChanges();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteCourse(int id)
    {
        var course = _context.Courses.FirstOrDefault(course => course.Id == id);
        if (course == null) return NotFound();
        _context.Remove(course);
        _context.SaveChanges();

        return NoContent();
    }
}
