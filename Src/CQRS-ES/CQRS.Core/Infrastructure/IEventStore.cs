using CQRS.Core.Events;

namespace CQRS.Core.Infrastructure
{
	public interface IEventStore
	{
		Task SaveEventAsync(string aggregateId, IEnumerable<BaseEvent> events, int expectedVersion);

		Task<List<BaseEvent>> GetEventsAsync(string aggregateId);

		Task<List<string>> GetAggregateIdsAsync();
	}
}
