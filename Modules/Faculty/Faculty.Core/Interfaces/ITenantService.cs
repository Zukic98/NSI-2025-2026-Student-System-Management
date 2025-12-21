namespace Faculty.Core.Interfaces;

/// <summary>
/// Service for resolving the current tenant ID from the execution context.
/// </summary>
public interface ITenantService
{
    Guid GetCurrentFacultyId();
}
