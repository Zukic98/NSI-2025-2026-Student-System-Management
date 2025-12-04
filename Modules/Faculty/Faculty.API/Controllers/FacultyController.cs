using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Faculty.API.Models; // Add this using statement
using Faculty.Application.DTOs;
using Faculty.Core.Entities;
using Faculty.Core.Exceptions;
using Faculty.Core.Interfaces;
using Faculty.Core.Enums;

namespace Faculty.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // Protect all endpoints in this controller
    public class FacultyController : ControllerBase
    {
        private readonly IFacultyService _facultyService;

        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        /*
        previous placeholder get endpoint
        [HttpGet]
        public IActionResult Get() => Ok("Hello from Faculty API!");
        */

        [HttpGet]
        public async Task<IActionResult> GetFaculties(
            [FromQuery] string? name,
            [FromQuery] FacultyStatus? status,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20,
            CancellationToken cancellationToken = default)
        {
            var result = await _facultyService.GetFacultiesAsync(
                nameFilter: name,
                statusFilter: status,
                pageNumber: pageNumber,
                pageSize: pageSize,
                cancellationToken: cancellationToken);

            // Map to DTO
            var dtoResult = new
            {
                Items = result.Items.Select(f => new FacultyDto
                {
                    Id = f.Id,
                    Name = f.Name,
                    Abbreviation = f.Abbreviation,
                    Description = f.Description,
                    Status = f.Status.ToString(),
                    CreatedAt = f.CreatedAt,
                    UpdatedAt = f.UpdatedAt
                }),
                result.TotalCount,
                result.PageNumber,
                result.PageSize
            };

            return Ok(dtoResult);
        }

        /// <summary>
        /// Create a new Faculty (Superadmin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Superadmin")]
        public async Task<IActionResult> CreateFaculty([FromBody] CreateFacultyDto request, CancellationToken cancellationToken)
        {
            if (!ModelState.IsValid)
            {
                // This is optional; your ApiBehaviorOptions already handle this
                return BadRequest(ModelState);
            }

            // Map DTO to entity
            var faculty = new Faculty.Core.Entities.Faculty
            {
                Name = request.Name.Trim(),
                Abbreviation = string.IsNullOrWhiteSpace(request.Abbreviation) ? null : request.Abbreviation.Trim(),
                Description = request.Description?.Trim(),
                Status = request.Status
            };

            try
            {
                // Call service to create
                var createdFaculty = await _facultyService.CreateFacultyAsync(faculty, cancellationToken);

                // Map back to DTO for response
                var response = new FacultyDto
                {
                    Id = createdFaculty.Id,
                    Name = createdFaculty.Name,
                    Abbreviation = createdFaculty.Abbreviation,
                    Description = createdFaculty.Description,
                    Status = createdFaculty.Status.ToString(),
                    CreatedAt = createdFaculty.CreatedAt,
                    UpdatedAt = createdFaculty.UpdatedAt
                };

                // Return 201 Created with Location header
                return CreatedAtAction(nameof(GetFacultyById), new { id = response.Id }, response);
            }
            catch (ConflictException ex)
            {
                // Name or abbreviation not unique
                return Conflict(new { message = ex.Message });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        /// <summary>
        /// Get single faculty by ID (needed for CreatedAtAction)
        /// </summary>
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Superadmin")] // optional, depends if read allowed
        public async Task<IActionResult> GetFacultyById(Guid id, CancellationToken cancellationToken)
        {
            var faculty = await _facultyService.GetFacultyByIdAsync(id, cancellationToken);
            if (faculty == null) return NotFound();

            var response = new FacultyDto
            {
                Id = faculty.Id,
                Name = faculty.Name,
                Abbreviation = faculty.Abbreviation,
                Description = faculty.Description,
                Status = faculty.Status.ToString(),
                CreatedAt = faculty.CreatedAt,
                UpdatedAt = faculty.UpdatedAt
            };

            return Ok(response);
        }

        /*
        previous placeholder post endpoint
        [HttpPost]
        [Authorize(Roles = "Superadmin")]
        public IActionResult CreateFaculty([FromBody] CreateFacultyRequest request)
        {
            // Placeholder for faculty creation logic
            return Ok($"Faculty '{request.FacultyName}' created by Superadmin.");
        }
        */

        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Superadmin")]
        public async Task<IActionResult> UpdateFaculty(
            [FromRoute] Guid id, 
            [FromBody] UpdateFacultyDto request,
            CancellationToken cancellationToken)
        {
            var existingFaculty = await _facultyService.GetFacultyByIdAsync(id, cancellationToken);
            if (existingFaculty == null)
                return NotFound($"Faculty with ID {id} not found");

            // Update fields
            existingFaculty.Name = request.Name;
            existingFaculty.Abbreviation = request.Abbreviation;
            existingFaculty.Description = request.Description;
            existingFaculty.Status = request.Status;

            try
            {
                var updatedFaculty = await _facultyService.UpdateFacultyAsync(existingFaculty, cancellationToken);

                var response = new FacultyDto
                {
                    Id = updatedFaculty.Id,
                    Name = updatedFaculty.Name,
                    Abbreviation = updatedFaculty.Abbreviation,
                    Description = updatedFaculty.Description,
                    Status = updatedFaculty.Status.ToString(),
                    CreatedAt = updatedFaculty.CreatedAt,
                    UpdatedAt = updatedFaculty.UpdatedAt
                };

                return Ok(response);
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message);
            }
        }

        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Superadmin")]
        public async Task<IActionResult> DeleteFaculty([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                await _facultyService.DeleteFacultyAsync(id, cancellationToken);
                return NoContent(); // HTTP 204 on successful soft delete
            }
            catch (NotFoundException ex)
            {
                return NotFound(ex.Message); // HTTP 404 if faculty doesn't exist
            }
            catch (ConflictException ex)
            {
                return Conflict(ex.Message); // HTTP 409 if faculty is in use
            }
        }

        [HttpGet("throw-exception")]
        [Authorize(Roles = "Superadmin")]
        public IActionResult ThrowException()
        {
            throw new InvalidOperationException("This is a test exception to trigger HTTP 500 error handling.");
        }
    }
}
