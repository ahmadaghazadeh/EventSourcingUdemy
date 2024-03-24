using Post.Query.Domain.Entities;
using Post.Query.Domain.Repositories;

namespace Post.Query.Api.Queries
{
	public class QueryHandler : IQueryHandler
	{
		private readonly IPostRepository _repository;

		public QueryHandler(IPostRepository repository)
		{
			_repository = repository;
		}

		public async Task<List<PostEntity>> HandleAsync(FindAllPostsQuery query)
		{
			return await _repository.ListAllAsync();
		}

		public async Task<List<PostEntity>> HandleAsync(FindPostByIdQuery query)
		{
			var post= await _repository.GetByIdAsync(query.Id);
			return new List<PostEntity> { post };
		}

		public async Task<List<PostEntity>> HandleAsync(FindPostsByAuthorQuery query)
		{
			return await _repository.ListByAuthorAsync(query.Author);
		}

		public async Task<List<PostEntity>> HandleAsync(FindPostsWithCommentsQuery query)
		{
			return await _repository.ListWithCommentsAsync();
		}

		public async Task<List<PostEntity>> HandleAsync(FindPostsWithLikesQuery query)
		{
			return await _repository.ListWithLikesAsync(query.NumberOfLikes);
		}
	}
}
