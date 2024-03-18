
using System.Data;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exception;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;

namespace Post.Command.Infrastructure.Stores
{
	public class EventStore: IEventStore
	{
		private readonly IEventStoreRepository _eventStoreRepository;
		private readonly IEventProducer _eventProducer;

		public EventStore(IEventStoreRepository repository, IEventProducer eventProducer)
		{
			_eventStoreRepository = repository;
			_eventProducer = eventProducer;
		}

		public async Task SaveEventAsync(Guid aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
		{
			var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
			if (expectedVersion != -1 && eventStream[^1].Version != expectedVersion)// eventStream[^1] eventStream[eventStream.length-1]
				throw new ConcurrencyException();
			var version = expectedVersion;
			foreach (var @event in events)
			{
				version++;
				@event.Version=version;
				var eventType = @event.GetType().Name;
				var eventModel = new EventModel
				{
					TimeStamp = DateTime.Now,
					AggregateIdentifier = aggregateId,
					AggregateType = nameof(Post),
					Version =version,
					EventType = eventType,
					EventData = @event
				};
				await _eventStoreRepository.SaveAsync(eventModel);

				var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");

				await _eventProducer.ProduceAsync(topic, @event);
			}
		}

		public async Task<List<BaseEvent>> GetEventsAsync(Guid aggregateId)
		{
			var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
			if (eventStream == null || !eventStream.Any())
				throw new AggregateNotFoundException("Incorrect post Id provided");

			return eventStream.OrderBy(x => x.Version)
				.Select(x=> x.EventData).ToList();
		}
	}
}
