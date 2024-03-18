using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Post.Common.Events;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Infrastructure.Handlers
{
	public class EventHandler: IEventHandler
	{
		private readonly IPostRepository _postRepository;
		private readonly ICommentRepository _commentRepository;

		public EventHandler(ICommentRepository commentRepository, IPostRepository postRepository)
		{
			_commentRepository = commentRepository;
			_postRepository = postRepository;
		}

		public Task On(PostCreatedEvent @event)
		{
			var post = new PostEntity
			{
				Id = @event.Id,
				Author = @event.Author,
				DatePosted = @event.DatePosted,
				Message = @event.Message,
			};
			return _postRepository.CreateAsync(post);
		}

		public async Task On(MessageUpdatedEvent @event)
		{
			var post = await _postRepository.GetByIdAsync(@event.Id);
			post.Message= @event.Message;

			await _postRepository.UpdateAsync(post);
		}

		public async Task On(PostLikedEvent @event)
		{
			var post = await _postRepository.GetByIdAsync(@event.Id);

			post.Likes++;
			await _postRepository.UpdateAsync(post);
		}

		public async Task On(CommentAddedEvent @event)
		{
			var comment = new CommentEntity
			{
				PostId = @event.Id,
				Id = @event.CommentId,
				CommentDate = @event.CommentDate,
				Comment = @event.Comment,
				UserName = @event.UserName,
				Edited = false
			};

		
			await _commentRepository.CreateAsync(comment);
		}

		public async Task On(CommandUpdatedEvent @event)
		{
			var comment = await _commentRepository.GetByIdAsync(@event.Id);
			comment.Comment=@event.Comment;
			comment.Edited = true;
			comment.CommentDate = @event.EditTime;

			await _commentRepository.UpdateAsync(comment);
		}

		public async Task On(PostRemovedEvent @event)
		{
			await _postRepository.DeleteAsync(@event.Id);
		}

		public async Task On(CommentRemovedEvent @event)
		{
			await _commentRepository.DeleteAsync(@event.Id);
		}
	}
}
