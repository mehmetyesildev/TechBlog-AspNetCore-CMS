using BlogApp.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "admin")]
    public class RolesController:Controller
    {
        private readonly RoleManager<AppRole>_roleManager;

        private readonly UserManager<User> _userManager;
        public RolesController(RoleManager<AppRole> roleManager, UserManager<User> userManager)
        {
            _roleManager=roleManager;
            _userManager=userManager;
        }
        public IActionResult Index()
        {
            return View(_roleManager.Roles);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(AppRole model)
        {
            if(ModelState.IsValid)
            {
                var result=await _roleManager.CreateAsync(model);
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult>Edit(int id)
        {
            var role=await _roleManager.FindByIdAsync(id.ToString());
            if(role !=null && role.Name != null)
            {
                ViewBag.Users=await _userManager.GetUsersInRoleAsync(role.Name);
                return View(role);
            }
            return RedirectToAction("İndex");
        }
         [HttpPost]
        public async Task<IActionResult> Edit(AppRole model)
        {
            if(ModelState.IsValid)
            {
                var role=await _roleManager.FindByIdAsync(model.Id.ToString());
                if(role !=null)
                {
                    role.Name=model.Name;
                    var result=await _roleManager.UpdateAsync(role);
                    if(result.Succeeded)
                    {
                        return RedirectToAction("Index");
                        
                    }
                    foreach(var err in result.Errors)
                    {
                        ModelState.AddModelError("",err.Description);
                    }
                      if (role.Name != null)
                    {
                        ViewBag.Users = await _userManager.GetUsersInRoleAsync(role.Name);
                    }   

                }             
            }
            return View(model);
    }   }
}