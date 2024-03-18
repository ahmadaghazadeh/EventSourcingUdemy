﻿namespace Post.Command.Api.Commands
{
	public interface ICommandHandler
	{
		Task HandleAsync(NewPostCommand command);

		Task HandleAsync(EditMessageCommand command);
		Task HandleAsync(EditCommentCommand command);
		Task HandleAsync(LikePostCommand command);
		Task HandleAsync(AddCommentCommand command);

		Task HandleAsync(RemoveCommentCommand command);
		Task HandleAsync(DeletePostCommand command);
	}
}