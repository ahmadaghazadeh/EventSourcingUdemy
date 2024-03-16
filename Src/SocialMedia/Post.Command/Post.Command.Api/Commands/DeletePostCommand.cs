using CQRS.Core.Commands;

namespace Post.Command.Api.Commands
{
	public class DeletePostCommand:BaseCommand
	{
		public string UserName { get; set; }

		public string Comment { get; set; }

		public string Username { get; set; }
	}
}
