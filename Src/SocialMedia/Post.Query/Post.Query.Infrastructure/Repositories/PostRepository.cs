using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;
using Post.Query.Infrastructure.DataAccess;
using SharpCompress.Common;

namespace Post.Query.Infrastructure.Repositories
{
	public class PostRepository: IPostRepository
	{
		private readonly ReadDbContextFactory _contextFactory;

		public PostRepository(ReadDbContextFactory contextFactory)
		{
			_contextFactory = contextFactory;
		}

		public async Task CreateAsync(PostEntity entity)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
		    context.Posts.Add(entity);
			_=await context.SaveChangesAsync();
		}

		public async Task UpdateAsync(PostEntity entity)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			context.Posts.Update(entity);
			_ = await context.SaveChangesAsync();
		}

		public async Task DeleteAsync(Guid postId)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			var post = await GetByIdAsync(postId);
			context.Posts.Remove(post);
			await context.SaveChangesAsync();
		}

		public async Task<PostEntity> GetByIdAsync(Guid postId)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			return await context.Posts
				.Include(p => p.Comments)
				.FirstOrDefaultAsync(x=> x.Id==postId);
		}

		public async Task<List<PostEntity>> ListAllAsync()
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			return await context.Posts.AsNoTracking()
				.Include(p => p.Comments)
				.ToListAsync();
		}

		public async Task<List<PostEntity>> ListByAuthorAsync(string author)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			return await context.Posts.AsNoTracking()
				.Include(p => p.Comments)
				.Where(x=> x.Author== author)
				.ToListAsync();
		}

		public async Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes)
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			return await context.Posts.AsNoTracking()
				.Include(p => p.Comments)
				.Where(x => x.Likes >= numberOfLikes)
				.ToListAsync();
		}

		public async Task<List<PostEntity>> ListWithCommentsAsync()
		{
			using ReadDbContext context = _contextFactory.CreateDbContext();
			return await context.Posts.AsNoTracking()
				.Include(p => p.Comments)
				.Where(x=>x.Comments!=null && x.Comments.Any() )
				.ToListAsync();
		}
	}
}
