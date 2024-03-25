
using System.Text.Json;
using CQRS.Core.Domain;
using CQRS.Core.Events;
using CQRS.Core.Exception;
using CQRS.Core.Infrastructure;
using CQRS.Core.Producers;
using Post.Common.Converters;

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

		public async Task SaveEventAsync(string aggregateId, IEnumerable<BaseEvent> events, int expectedVersion)
		{
			var options = new JsonSerializerOptions()
			{
				Converters =
				{
					new EventJsonConverter()
				},
				WriteIndented = true
			};
 
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
					AggregateIdentifier = aggregateId.ToString(),
					AggregateType = nameof(Post),
					Version =version,
					EventType = eventType,
					EventData = JsonSerializer.Serialize(@event, options)
			};
				await _eventStoreRepository.SaveAsync(eventModel);

				var topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");

				await _eventProducer.ProduceAsync(topic, @event);
			}
		}

		public async Task<List<BaseEvent>> GetEventsAsync(string aggregateId)
		{
			var eventStream = await _eventStoreRepository.FindByAggregateId(aggregateId);
			if (eventStream == null || !eventStream.Any())
				throw new AggregateNotFoundException("Incorrect post Id provided");

			var options = new JsonSerializerOptions()
			{
				Converters =
				{
					new EventJsonConverter()
				}
			};


			return eventStream.OrderBy(x => x.Version)
				.Select(x=> JsonSerializer.Deserialize<BaseEvent>(x.EventData, options: options)).ToList();
		}

		public async Task<List<string>> GetAggregateIdsAsync()
		{
			var eventStream = await _eventStoreRepository.FindAllAsync();
			if (eventStream == null || !eventStream.Any())
			{
				throw new ArgumentNullException(nameof(eventStream), "Cloud not retrieve event stream form the event");
			}

			return eventStream.Select(e => e.AggregateIdentifier).Distinct().ToList();
		}
	}
}
