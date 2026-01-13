using Analytics.Core.Entities;

namespace Analytics.Application.Interfaces
{
    public interface IStatsService
    {
        Task<string> GetOrUpdateStatAsync(string metricCode, Scope scope, Guid scopeIdentifier, bool forceRefresh = false);
    }
}