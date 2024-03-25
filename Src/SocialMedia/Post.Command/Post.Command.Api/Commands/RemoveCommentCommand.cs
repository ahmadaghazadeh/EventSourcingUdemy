using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
	public class RemoveCommentCommand: BaseCommand
	{
		public string CommentId { get; set; }
		public string UserID { get; set; }
	}
}
