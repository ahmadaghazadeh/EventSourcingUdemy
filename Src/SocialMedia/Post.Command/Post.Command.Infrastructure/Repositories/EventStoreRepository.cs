using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Post.Command.Infrastructure.Config;

namespace Post.Command.Infrastructure.Repositories
{
    public class EventStoreRepository: IEventStoreRepository
	{
		private readonly IMongoCollection<EventModel> _eventStoreCollection;

		public EventStoreRepository(IOptions<MongoDbConfig> config)
		{
			var mongoClinet = new MongoClient(config.Value.ConnectionString);
			var mongoDataBase = mongoClinet.GetDatabase(config.Value.DatabaseName);

			_eventStoreCollection = mongoDataBase.GetCollection<EventModel>(config.Value.Collection);

		}
		public async Task SaveAsync(EventModel @event)
		{
			await _eventStoreCollection.InsertOneAsync(@event).ConfigureAwait(false);
		}

		public async Task<List<EventModel>> FindByAggregateId(Guid aggregateId)
		{
			return await _eventStoreCollection.Find(x => x.AggregateIdentifier == aggregateId)
				.ToListAsync().ConfigureAwait(false);
		}
	}
}
