using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController:Controller
    {
        private readonly IPostRepository _postrepository;
        private readonly ICommentRepository _commentrepository;
        private readonly ITagRepository _tagrepository;
        private readonly IWebHostEnvironment _hostEvironment;
        public PostsController(IPostRepository postrepository, ICommentRepository commentrepository, ITagRepository tagRepository, IWebHostEnvironment hostEvironment)
        {
            _postrepository = postrepository;
            _commentrepository=commentrepository;
            _tagrepository=tagRepository;
            _hostEvironment=hostEvironment;
        }
        public async Task<IActionResult> Index( string tag)
        {
            var claims=User.Claims;
            var posts =_postrepository.Posts;
            if(!string.IsNullOrEmpty(tag))
            {
                posts=posts.Where(x=>x.Tags.Any(t=>t.Url==tag));
            }
            return View(new PostsViewModel(){ Posts=await posts.ToListAsync()});
        }
        public async Task<IActionResult> Details(string? url)
        {
            if (url == null)
            {
                return NotFound();
            }
            return View(await _postrepository
                        .Posts
                        .Include(x=>x.User)
                        .Include(x=>x.Tags)
                        .Include(x=>x.Comments)
                        .ThenInclude(x=>x.User)
                        .FirstOrDefaultAsync(p=>p.Url==url));
        }
        [HttpPost]
        public JsonResult AddComment(int PostId,string Text)
        {
            var UserId=User.FindFirstValue(ClaimTypes.NameIdentifier);
            var UserName=User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData);
            var Entity= new Comment
            {
                Text=Text,
                PublishedOn=DateTime.Now,
                PostId=PostId,
                UserId=int.Parse(UserId ?? ""),
               
            };
            _commentrepository.CreateComment(Entity);
            // //return Redirect("/posts/details/"+Url);
            // return RedirectToRoute("post_details",new { url = Url});
            return Json(new{
                    UserName,
                    Text,
                    Entity.PublishedOn,
                    avatar
                    });
            
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(PostCreateViewModel model ,IFormFile file)
        {
            if(ModelState.IsValid)
            {

                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                string randomFileName="1.jpg";
                if(file != null)
                {
                    var Extension=Path.GetExtension(file.FileName);
                    randomFileName=string.Format("{0}{1}",Guid.NewGuid(),Extension);
                    var path=Path.Combine(_hostEvironment.WebRootPath,"img",randomFileName);
                    using(var stream=new FileStream(path,FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                _postrepository.CreatePost(
                    new Post{Title=model.Title,
                            Description=model.Description,
                            Content=model.Content,
                            Url=model.Url,
                            UserId=int.Parse(userId ?? ""),
                            Publishedon=DateTime.Now,
                            Image=randomFileName,
                            IsActive=false}
                            
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }
        [Authorize]
        public async Task<IActionResult> List()
        {
            var userId=int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "");
            var role=User.FindFirstValue(ClaimTypes.Role);
            IQueryable<Post> posts = _postrepository.Posts.Include(u=>u.User);
            if(string.IsNullOrEmpty(role))
            {
                posts=posts.Where(i=>i.UserId==userId);
            }

            return View(await posts.ToListAsync());
        }
        [Authorize]
        public IActionResult Edit(int? id)
        {
            if(id==null)
            {
                return NotFound();
            }
            var post=_postrepository.Posts
                                    .Include(x=>x.Tags)
                                    .FirstOrDefault(x=>x.PostId==id);
            if(post==null)
            {
                return NotFound();
            }
            ViewBag.Tags=_tagrepository.Tags.ToList();
            return View(new PostCreateViewModel{
                PostId=post.PostId,
                Title=post.Title,
                Description=post.Description,
                Content=post.Content,
                Url=post.Url,
                IsActive=post.IsActive,
                Tags=post.Tags
            });
        }
        [Authorize]
        [HttpPost]
        public IActionResult Edit(PostCreateViewModel model, int[] tagIds)
        {
            if(ModelState.IsValid)
            {
                var EntityToUpdate=new Post
                {
                    PostId=model.PostId,
                    Title=model.Title,
                    Description=model.Description,
                    Content=model.Content,
                    Url=model.Url,
        
                };
                if (User.FindFirstValue(ClaimTypes.Role) == "admin")
                {
                    EntityToUpdate.IsActive=model.IsActive;
                }
                    _postrepository.EditPost(EntityToUpdate,tagIds);
                return RedirectToAction("List");
            }
            ViewBag.Tags = _tagrepository.Tags.ToList();
            return View(model);
        }
        public IActionResult Delete(int? id)
        {
            if (id==null)
            {
                return NotFound();
            }
            var post=_postrepository.Posts.FirstOrDefault(p=>p.PostId==id);
            if(post==null)
            {
                return NotFound();
            }
            if(!string.IsNullOrEmpty(post.Image))
            {
                var path=Path.Combine(_hostEvironment.WebRootPath,"img",post.Image);
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _postrepository.DeletePost(post);
            return RedirectToAction("List");
        }

    }
}