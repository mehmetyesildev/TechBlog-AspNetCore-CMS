using BlogApp.Data.Abstract;
using BlogApp.Entity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace BlogApp.ViewComponents
{
    public class TagsMenu:ViewComponent
    {
        private ITagRepository _tagrepository;
        public TagsMenu(ITagRepository tagRepository)
        {
            _tagrepository=tagRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _tagrepository.Tags.ToListAsync());
        }
    }
}