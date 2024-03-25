using CQRS.Core.Events;

namespace Post.Common.Events
{
	public class CommentUpdatedEvent:BaseEvent
	{
		public CommentUpdatedEvent() : base(nameof(CommentUpdatedEvent))
		{
		}

		public string CommentId { get; set; }
		public string Comment { get; set; }

		public string UserName { get; set; }

		public DateTime EditTime { get; set; }
	}
}
