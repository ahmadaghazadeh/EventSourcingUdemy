using CQRS.Core.Queries;

namespace Post.Query.Api.Queries
{
    public class FindPostByIdQuery : BaseQuery
    {
        public string Id { get; set; }
    }
}