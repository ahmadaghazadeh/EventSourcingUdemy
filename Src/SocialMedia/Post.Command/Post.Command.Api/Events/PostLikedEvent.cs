using CQRS.Core.Events;

namespace Post.Command.Api.Events
{
	public class PostLikedEvent: BaseEvent
	{
		public PostLikedEvent() : base(nameof(PostLikedEvent))
		{

		}

	}
}
