using BlogApp.Entity;

namespace BlogApp.Data.Abstract
{
    public interface IPostRepository
    {
        IQueryable<Post>Posts{get;}
        void CreatePost(Post post);
        void EditPost(Post post, int[]tagıds);
        void DeletePost(Post post);

    }
}