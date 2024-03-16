using CQRS.Core.Events;

namespace Post.Command.Api.Events
{
	public class CommandUpdatedEvent:BaseEvent
	{
		public CommandUpdatedEvent() : base(nameof(CommandUpdatedEvent))
		{
		}

		public Guid CommentId { get; set; }
		public string Comment { get; set; }

		public string UserName { get; set; }

		public DateTime EditTime { get; set; }
	}
}
