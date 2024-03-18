
using CQRS.Core.Domain;
using CQRS.Core.Handlers;
using CQRS.Core.Infrastructure;
using Post.Command.Domain.Aggregates;

namespace Post.Command.Infrastructure.Handlers
{
	public class EventSourcingHandler : IEventSourcingHandler<PostAggregate>
	{
		private readonly IEventStore _eventStore;

		public EventSourcingHandler(IEventStore eventStore)
		{
			_eventStore = eventStore;
		}

		public async Task<PostAggregate> GetByIdAsync(Guid aggregateId)
		{
			var postAggregate=new PostAggregate();
			var events = await _eventStore.GetEventsAsync(aggregateId);
			if (!events.Any()) return postAggregate;

			postAggregate.ReplayEvents(events);
			postAggregate.Version= events.Select(x => x.Version).Max();

			return postAggregate;
		}

		public async Task SaveAsync(AggregateRoot aggregate)
		{
			await _eventStore.SaveEventAsync(aggregate.Id, aggregate.GetUnCommittedChanges(), aggregate.Version);
			aggregate.MarkChangesAsCommitted();

		}
	}
}
