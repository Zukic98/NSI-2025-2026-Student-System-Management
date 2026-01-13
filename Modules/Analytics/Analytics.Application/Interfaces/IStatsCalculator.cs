using Analytics.Core.Entities;

namespace Analytics.Application.Interfaces
{
    public interface IStatsCalculator
    {
        string MetricCode { get; }
        Scope Scope { get; }
        Guid ScopeIdentifier { get; }
        Task<string> CalculateAsync(string metricCode, Scope scope, Guid scopeIdentifier);
    }
}