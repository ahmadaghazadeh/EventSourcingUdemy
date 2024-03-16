using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
	public class LikePostCommand: BaseCommand
	{
		public string Comment { get; set; }
		public string UserName { get; set; }

	}
}
