using Microsoft.AspNetCore.Mvc;
using University.Application.DTOs;
using University.Application.Services;

namespace University.API.Controllers
{
    [ApiController]
    [Route("api/university/courses")]
    public class UniversityController : ControllerBase
    {
        private readonly CourseService _service;

        public UniversityController (CourseService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _service.GetAllAsync();
            return Ok(data);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var course = await _service.GetByIdAsync(id);
            return course == null ? NotFound() : Ok(course);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateCourseDto dto)
        {
            var course = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = course.Id }, course);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, UpdateCourseDto dto)
        {
            var course = await _service.UpdateAsync(id, dto);
            return course == null ? NotFound() : Ok(course);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.DeleteAsync(id);
            return !success ? NotFound() : NoContent();
        }
    }
}
