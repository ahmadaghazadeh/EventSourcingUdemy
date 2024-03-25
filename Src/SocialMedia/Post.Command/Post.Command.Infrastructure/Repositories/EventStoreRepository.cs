using CQRS.Core.Domain;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
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

		public  Task<List<EventModel>> FindByAggregateId(string aggregateId)
		{
 

			var tess1=  _eventStoreCollection.
				FindSync(x => x.AggregateIdentifier.Equals(aggregateId.ToString()))
				.ToList();
			return Task.FromResult(tess1);
		}

		public async Task<List<EventModel>> FindAllAsync()
		{
			return await _eventStoreCollection.Find(_ => true).ToListAsync().ConfigureAwait(false);
		}
	}
}
