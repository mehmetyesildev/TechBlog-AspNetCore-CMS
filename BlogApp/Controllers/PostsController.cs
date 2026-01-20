using System.Security.Claims;
using System.Threading.Tasks;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class PostsController : Controller
    {
        private readonly IPostRepository _postrepository;
        private readonly ICommentRepository _commentrepository;
        private readonly ITagRepository _tagrepository;
        private readonly IWebHostEnvironment _hostEvironment;
        
        private readonly UserManager<User> _userManager;

        public PostsController(IPostRepository postrepository, ICommentRepository commentrepository,
                                ITagRepository tagRepository,
                                IWebHostEnvironment hostEvironment,
                                UserManager<User> userManager)
        {
            _postrepository = postrepository;
            _commentrepository = commentrepository;
            _tagrepository = tagRepository;
            _hostEvironment = hostEvironment;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index(string tag)
        {
            var posts =  _postrepository.Posts.AsQueryable();
            if (!string.IsNullOrEmpty(tag))
            {
                posts = posts.Where(x => x.Tags.Any(t => t.Url == tag));
            }
            return View(new PostsViewModel() { Posts = await posts.ToListAsync() });
        }

        public async Task<IActionResult> Details(string? url)
        {
            if (url == null) return NotFound();

            var post = await _postrepository.Posts
                        .Include(x => x.User)
                        .Include(x => x.Tags)
                        .Include(x => x.Comments)
                        .ThenInclude(x => x.User)
                        .FirstOrDefaultAsync(p => p.Url == url);

            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost]
        public JsonResult AddComment(int PostId, string Text)
        {
           
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userName = User.FindFirstValue(ClaimTypes.Name);
            var avatar = User.FindFirstValue(ClaimTypes.UserData) ?? "avatar.jpg";

            if (userId == null) return Json(new { error = "Lütfen giriş yapınız." });

            var entity = new Comment
            {
                Text = Text,
                PublishedOn = DateTime.Now,
                PostId = PostId,
                UserId = int.Parse(userId), 
            };
            
            _commentrepository.CreateComment(entity);

            return Json(new
            {
                UserName = userName,
                Text,
                entity.PublishedOn,
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
        public async Task<IActionResult> Create(PostCreateViewModel model, IFormFile file)
        {
            if (ModelState.IsValid)
            {
               
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

                string randomFileName = "1.jpg";
                if (file != null)
                {
                    var extension = Path.GetExtension(file.FileName);
                    randomFileName = string.Format("{0}{1}", Guid.NewGuid(), extension);
                    var path = Path.Combine(_hostEvironment.WebRootPath, "img", randomFileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                _postrepository.CreatePost(
                    new Post
                    {
                        Title = model.Title,
                        Description = model.Description,
                        Content = model.Content,
                        Url = model.Url,
                        UserId = userId,
                        Publishedon = DateTime.Now,
                        Image = randomFileName,
                        IsActive = User.IsInRole("admin") 
                    }
                );
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [Authorize]
        public async Task<IActionResult> List()
        {
           
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");

            IQueryable<Post> posts = _postrepository.Posts.Include(u => u.User);

           
            if (!User.IsInRole("admin"))
            {
                posts = posts.Where(i => i.UserId == userId);
            }

            return View(await posts.ToListAsync());
        }

        [Authorize]
        public IActionResult Edit(int? id)
        {
            if (id == null) return NotFound();

            var post = _postrepository.Posts.Include(x => x.Tags).FirstOrDefault(x => x.PostId == id);
            if (post == null) return NotFound();

         
            if (!IsUserOwnerOrAdmin(post.UserId))
            {
                return RedirectToAction("Index");
            }

            ViewBag.Tags = _tagrepository.Tags.ToList();
            return View(new PostCreateViewModel
            {
                PostId = post.PostId,
                Title = post.Title,
                Description = post.Description,
                Content = post.Content,
                Url = post.Url,
                IsActive = post.IsActive,
                Tags = post.Tags
            });
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Edit(PostCreateViewModel model, int[] tagIds)
        {
            if (ModelState.IsValid)
            {
                var originalPost = await _postrepository.Posts.FirstOrDefaultAsync(p => p.PostId == model.PostId);
                if (originalPost == null) return NotFound();

                
                if (!IsUserOwnerOrAdmin(originalPost.UserId))
                {
                    return RedirectToAction("Index");
                }

                var entityToUpdate = new Post
                {
                    PostId = model.PostId,
                    Title = model.Title,
                    Description = model.Description,
                    Content = model.Content,
                    Url = model.Url,
                    Image = originalPost.Image, 
                    UserId = originalPost.UserId 
                };

       
                if (User.IsInRole("admin"))
                {
                    entityToUpdate.IsActive = model.IsActive;
                }
                else
                {
                    entityToUpdate.IsActive = originalPost.IsActive;
                }

                _postrepository.EditPost(entityToUpdate, tagIds);
                return RedirectToAction("List");
            }
            ViewBag.Tags = _tagrepository.Tags.ToList();
            return View(model);
        }

        [Authorize]
        public  IActionResult Delete(int? id)
        {
            if (id == null) return NotFound();
            
            var post = _postrepository.Posts.FirstOrDefault(p => p.PostId == id);
            if (post == null) return NotFound();

    
            if (!IsUserOwnerOrAdmin(post.UserId))
            {
                return RedirectToAction("List");
            }

            if (!string.IsNullOrEmpty(post.Image))
            {
                var path = Path.Combine(_hostEvironment.WebRootPath, "img", post.Image);
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _postrepository.DeletePost(post);
            return RedirectToAction("List");
        }

        
        private bool IsUserOwnerOrAdmin(int postUserId)
        {
            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier) ?? "0");
            
           
            return User.IsInRole("admin") || currentUserId == postUserId;
        }
    }
}