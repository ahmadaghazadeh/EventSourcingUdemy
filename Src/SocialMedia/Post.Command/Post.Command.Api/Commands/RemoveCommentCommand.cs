using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
	public class RemoveCommentCommand: BaseCommand
	{
		public Guid CommentId { get; set; }
		public string UserID { get; set; }
	}
}
