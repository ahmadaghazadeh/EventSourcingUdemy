using CQRS.Core.Events;

namespace Post.Common.Events
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
