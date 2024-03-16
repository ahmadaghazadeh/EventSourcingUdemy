using CQRS.Core.Events;

namespace Post.Command.Api.Events
{
	public class PostRemovedEvent :BaseEvent
	{
		public PostRemovedEvent() : base(nameof(PostRemovedEvent))
		{
		}


	}
}
