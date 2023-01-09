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

    /// <summary>
    /// Obtem todos os cursos do banco de dados
    /// </summary>
    /// <param name="skip">Define a quantidade de items a serem pulados na requisição</param>
    /// <param name="take">Define a quantidade de items a serem guardados dentro da requisição</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso os dados sejam obtidos com sucesso</response>
    [HttpGet]
    public IEnumerable<ReadCourseDTO> GetAllCourses([FromQuery] int skip = 0, int take = 50)
    {
        return _mapper.Map<List<ReadCourseDTO>>(_context.Courses.Skip(skip).Take(take));
    }

    /// <summary>
    /// Obtem cursos especificos do banco de dados
    /// </summary>
    /// <param name="id">Id referente ao curso que será obtido</param>
    /// <returns>IActionResult</returns>
    /// <response code="200">Caso os dados sejam obtidos com sucesso</response>
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

    /// <summary>
    /// Atualiza um curso existente no banco de dados
    /// </summary>
    /// <param name="id">Id referente ao curso que será alterado</param>
    /// <param name="courseDTO">Objeto com campos para edição de um curso</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização seja feita com sucesso</response>
    [HttpPut("{id}")]
    public IActionResult UpdateCourse(int id, [FromBody] UpdateCourseDTO courseDTO)
    {
        var course = _context.Courses.FirstOrDefault(course => course.Id == id);

        if (course == null) return NotFound();
        _mapper.Map(courseDTO, course);
        _context.SaveChanges();

        return NoContent();
    }

    /// <summary>
    /// Atualiza parcialmente um curso existente no banco de dados
    /// </summary>
    /// <param name="id">Id referente ao curso que será alterado</param>
    /// <param name="patch">Objeto com campos para a edição parcial ou total de um curso</param>
    /// <returns>IActionResult</returns>
    /// <response code="204">Caso a atualização seja feita com sucesso</response>
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

/// <summary>
/// Deleta um curso existente no banco de dados
/// </summary>
/// <param name="id">Id referente ao curso que será alterado</param>
/// <returns>IActionResult</returns>
/// <response code="204">Caso a deleção seja feita com sucesso</response>
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
