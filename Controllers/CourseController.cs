using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using webapi_dotnet6.Entity;

namespace webapi_dotnet6.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CourseController : ControllerBase
    {
        private static List<Course> courses = new List<Course>();
        private int id = 0;

        [HttpGet]
        public IEnumerable<Course> GetCourses()
        {
            return courses;
        }

        [HttpGet("{id}")]
        public IActionResult GetCourseById(int id)
        {
            var result = courses.FirstOrDefault(course => course.Id == id);

            if (result == null)
                return NotFound();
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddCourse([FromBody] Course course)
        {
            course.Id = id++;
            courses.Add(course);
            return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, course);
        }

        [HttpPut("{id}")]
        public IEnumerable<Course> UpdateCourse()
        {
            return courses;
        }

        [HttpDelete("{id}")]
        public IEnumerable<Course> DeleteCourse()
        {
            return courses;
        }
    }
}
