﻿using CQRS.Core.Events;

namespace Post.Command.Api.Events
{
	public class CommentRemovedEvent : BaseEvent
	{
		public CommentRemovedEvent() : base(nameof(CommentRemovedEvent))
		{
		}

		public Guid CommentId { get; set; }
	}
}