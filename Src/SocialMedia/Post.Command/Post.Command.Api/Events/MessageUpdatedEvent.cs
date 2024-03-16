using CQRS.Core.Events;

namespace Post.Command.Api.Events
{
	public class MessageUpdatedEvent:BaseEvent
	{
		public MessageUpdatedEvent() : base(nameof(MessageUpdatedEvent))
		{
		}

		public string Message { get; set; }
	}
}
