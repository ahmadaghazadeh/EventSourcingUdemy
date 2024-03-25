using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;

namespace Post.Query.Infrastructure.Repositories
{
	public class CommentRepository : ICommentRepository
	{
		private readonly ReadDbContextFactory _contextFactory;

		public CommentRepository(ReadDbContextFactory contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task CreateAsync(CommentEntity comment)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			context.Comments.Add(comment);

			_ = await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(string commentId)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			var comment = await GetByIdAsync(commentId);

			if (comment == null) return;

			context.Comments.Remove(comment);
			_ = await context.SaveChangesAsync();
		}

		public async Task<CommentEntity> GetByIdAsync(string commentId)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			return await context.Comments.FirstOrDefaultAsync(x => x.Id == commentId);
		}

		public async Task UpdateAsync(CommentEntity comment)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			context.Comments.Update(comment);

			_ = await context.SaveChangesAsync();
		}
	}
}
