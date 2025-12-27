using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Concrete.EfCore
{
    public class EfPostRepository : IPostRepository
    {
        private BlogContext _context;
        public EfPostRepository(BlogContext context)
        {
            _context=context;
        }
        public IQueryable<Post> Posts => _context.Posts;

        public void CreatePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void DeletePost(Post post)
        {
            _context.Posts.Remove(post);
            _context.SaveChanges();
        }

        public void EditPost(Post post)
        {
            var Entity=_context.Posts.FirstOrDefault(x=>x.PostId==post.PostId);
            if (Entity == null)
            {
                // ID bulunamazsa HATA FIRLAT ki sorunu anlayalım
                throw new Exception($"HATA: Veritabanında {post.PostId} ID'li bir kayıt bulunamadı! Gelen ID yanlış.");
            }
            if (Entity!=null)
            {
                Entity.Title=post.Title;
                Entity.Description=post.Description;
                Entity.Content=post.Content;
                Entity.Url=post.Url;
                Entity.IsActive=post.IsActive;
                
                _context.SaveChanges();
            }
        }

        public void EditPost(Post post, int[] tagıds)
        {
            var Entity = _context.Posts.Include(i => i.Tags).FirstOrDefault(x => x.PostId == post.PostId);
            if (Entity == null)
            {
                // ID bulunamazsa HATA FIRLAT ki sorunu anlayalım
                throw new Exception($"HATA: Veritabanında {post.PostId} ID'li bir kayıt bulunamadı! Gelen ID yanlış.");
            }
            if (Entity != null)
            {
                Entity.Title = post.Title;
                Entity.Description = post.Description;
                Entity.Content = post.Content;
                Entity.Url = post.Url;
                Entity.IsActive = post.IsActive;
                Entity.Tags = _context.Tags.Where(tag =>tagıds.Contains(tag.TagId)).ToList();
                _context.SaveChanges();
            }
        }
    }
}
