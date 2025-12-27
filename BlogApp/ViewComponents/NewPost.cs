using BlogApp.Data.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.ViewComponents
{
    public class NewPost:ViewComponent
    {
        private readonly IPostRepository _postrepository;
        public NewPost(IPostRepository postRepository)
        {
            _postrepository=postRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _postrepository
                        .Posts
                        .OrderByDescending(p=>p.Publishedon)
                        .Take(5)
                        .ToListAsync());
        }
    }
}