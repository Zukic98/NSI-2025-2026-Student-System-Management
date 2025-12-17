using Support.Application.DTOs;

namespace Support.Application.Services
{
    public interface IIssueService
    {
        // Issue methods
        Task<IssueDto> CreateIssueAsync(CreateIssueDto createIssueDto, CancellationToken cancellationToken = default);
        Task<IssueDto?> GetIssueByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<IssueDto>> GetAllIssuesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<IssueDto>> GetIssuesByUserIdAsync(string userId, CancellationToken cancellationToken = default);
        Task<IssueDto?> UpdateIssueAsync(int id, UpdateIssueDto updateIssueDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteIssueAsync(int id, CancellationToken cancellationToken = default);

        // Category methods
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryDto createCategoryDto, CancellationToken cancellationToken = default);
        Task<CategoryDto?> GetCategoryByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync(CancellationToken cancellationToken = default);
        Task<CategoryDto?> UpdateCategoryAsync(int id, UpdateCategoryDto updateCategoryDto, CancellationToken cancellationToken = default);
        Task<bool> DeleteCategoryAsync(int id, CancellationToken cancellationToken = default);
    }
}
