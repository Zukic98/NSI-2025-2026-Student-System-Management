using Analytics.Application.Interfaces;
using Analytics.Core.Entities;
using Analytics.Core.Interfaces;

namespace Analytics.Application.Services
{
    public class StatsService : IStatsService
    {
        private readonly IStatRepository _statRepository;
        private readonly IEnumerable<IStatsCalculator> _calculators;

        public StatsService(IStatRepository statRepository, IEnumerable<IStatsCalculator> calculators)
        {
            _statRepository = statRepository;
            _calculators = calculators;
        }

        public async Task<string> GetOrUpdateStatAsync(string metricCode, Scope scope, Guid scopeIdentifier, bool forceRefresh = false)
        {
            var calculator = _calculators.FirstOrDefault(c => c.MetricCode == metricCode && c.Scope == scope && c.ScopeIdentifier == scopeIdentifier);

            if (calculator == null)
            {
                throw new Exception($"No calculator defined");
            }

            var stats = await _statRepository.GetStatsByMetricAndScopeAsync(metricCode, scope, scopeIdentifier);
            var stat = stats.FirstOrDefault();

            if (stat != null && !forceRefresh)
            {
                return stat.Value;
            }
            
            var newValue = await calculator.CalculateAsync(metricCode, scope, scopeIdentifier);

            if (stat == null)
            {
                stat = new Stat
                {
                    Id = Guid.NewGuid(),
                    MetricCode = metricCode,
                    Scope = scope,
                    ScopeIdentifier = scopeIdentifier,
                    Value = newValue,
                    RecordedAt = DateTime.UtcNow
                };
                await _statRepository.AddAsync(stat);
            }
            else
            {
                stat.Value = newValue;
                stat.RecordedAt = DateTime.UtcNow;
                await _statRepository.UpdateAsync(stat);
            }

            return newValue;
        }

        private string ComputeStatValue(string metricCode, Scope scope, Guid scopeIdentifier)
        {
            // Placeholder for actual computation logic
            return "ComputedValue";
        }
    }
}