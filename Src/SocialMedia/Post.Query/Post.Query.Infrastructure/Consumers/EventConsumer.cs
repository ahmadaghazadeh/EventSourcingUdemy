﻿
using System.Text.Json;
using Confluent.Kafka;
using CQRS.Core.Consumers;
using CQRS.Core.Events;
using Microsoft.Extensions.Options;
using Post.Common.Converters;
using Post.Query.Infrastructure.Handlers;

namespace Post.Query.Infrastructure.Consumers
{
	public class EventConsumer: IEventConsumer
	{
		private readonly ConsumerConfig _config;
		private readonly IEventHandler _eventHandler;

		public EventConsumer(IOptions<ConsumerConfig> config, IEventHandler eventHandler)
		{
			_eventHandler = eventHandler;
			_config = config.Value;
		}

		public void Consume(string topic)
		{
			using var consuemr=new ConsumerBuilder<string,string>(_config)
				.SetKeyDeserializer(Deserializers.Utf8)
				.SetValueDeserializer(Deserializers.Utf8)
				.Build();

			consuemr.Subscribe(topic);

			while (true)
			{
				var consumerResult=consuemr.Consume();

				if (consumerResult?.Message == null) continue;

				var options = new JsonSerializerOptions()
				{
					Converters =
					{
						new EventJsonConverter()
					}
				};

				var @event = JsonSerializer.Deserialize<BaseEvent>(consumerResult.Message.Value,options: options);

				var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { @event.GetType() });

				if (handlerMethod == null)
				{
					throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method!");
				}

				handlerMethod.Invoke(_eventHandler, new object?[] { @event });

				consuemr.Commit(consumerResult); // Use Mediatr  

			}
		}
	}
}
