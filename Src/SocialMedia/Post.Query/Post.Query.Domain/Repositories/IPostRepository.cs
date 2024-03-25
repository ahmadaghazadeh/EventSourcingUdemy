using Post.Query.Domain.Entities;

namespace Post.Query.Domain.Repositories
{
	public interface IPostRepository
	{
		Task CreateAsync(PostEntity entity);
		Task UpdateAsync(PostEntity entity);
		Task DeleteAsync(string postId);
		Task<PostEntity> GetByIdAsync(string postId);

		Task<List<PostEntity>> ListAllAsync();
		Task<List<PostEntity>> ListByAuthorAsync(string author);
		Task<List<PostEntity>> ListWithLikesAsync(int numberOfLikes);
		Task<List<PostEntity>> ListWithCommentsAsync();



	}
}
