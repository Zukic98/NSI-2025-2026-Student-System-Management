using Faculty.Application.DTOs;
using Faculty.Core.Entities;
using Faculty.Core.Exceptions;
using Faculty.Core.Interfaces;
using Faculty.Core.Models;
using Faculty.Core.Enums;

namespace Faculty.Application.Services
{
    public class FacultyService : IFacultyService
    {
        private readonly IFacultyRepository _repository;
        
        public FacultyService(IFacultyRepository repository)
        {
            _repository = repository;
        }
        
        public async Task<Faculty.Core.Entities.Faculty> CreateFacultyAsync(Faculty.Core.Entities.Faculty faculty, CancellationToken cancellationToken = default)
        {
            // Check for unique name
            if (!await _repository.IsNameUniqueAsync(faculty.Name, null, cancellationToken))
                throw new ConflictException($"Faculty name '{faculty.Name}' already exists.");

            // Check for unique abbreviation if provided
            if (!string.IsNullOrWhiteSpace(faculty.Abbreviation) && !await _repository.IsAbbreviationUniqueAsync(faculty.Abbreviation, null, cancellationToken))
                throw new ConflictException($"Faculty abbreviation '{faculty.Abbreviation}' already exists.");
            
            var created = await _repository.AddAsync(faculty, cancellationToken);
            return created;
        }
        
        public async Task<PagedResult<Faculty.Core.Entities.Faculty>> GetFacultiesAsync(
            string? nameFilter, 
            FacultyStatus? statusFilter,
            int pageNumber, 
            int pageSize,
            CancellationToken cancellationToken = default)
        {
            var result = await _repository.GetAllAsync(
                nameFilter, 
                statusFilter, 
                pageNumber, 
                pageSize,
                cancellationToken);
            
            return result;
        }
        
        public async Task<Faculty.Core.Entities.Faculty?> GetFacultyByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var faculty = await _repository.GetByIdAsync(id, cancellationToken);
            return faculty;
        }
        
        public async Task<Faculty.Core.Entities.Faculty> UpdateFacultyAsync(Faculty.Core.Entities.Faculty faculty, CancellationToken cancellationToken = default)
        {
            // Check for unique name (excluding current faculty)
            if (!await _repository.IsNameUniqueAsync(faculty.Name, faculty.Id, cancellationToken))
                throw new ConflictException($"Faculty name '{faculty.Name}' already exists.");

            // Check for unique abbreviation if provided (excluding current faculty)
            if (!string.IsNullOrWhiteSpace(faculty.Abbreviation) && 
                !await _repository.IsAbbreviationUniqueAsync(faculty.Abbreviation, faculty.Id, cancellationToken))
                throw new ConflictException($"Faculty abbreviation '{faculty.Abbreviation}' already exists.");

            await _repository.UpdateAsync(faculty, cancellationToken);
            return faculty;
        }
        
        public async Task DeleteFacultyAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var existing = await _repository.GetByIdAsync(id, cancellationToken);
            if (existing == null)
                throw new NotFoundException($"Faculty with ID {id} not found");
            
            // Check for conflicts
            if (await _repository.IsInUseAsync(id, cancellationToken))
                throw new ConflictException("Faculty is in use and cannot be deleted");
            
            // Soft delete: mark as Inactive
            existing.Status = FacultyStatus.Inactive;
            await _repository.UpdateAsync(existing, cancellationToken);
        }
    }
}