using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    [Authorize(Roles = "admin, moderator")]
    public class Admin:Controller
    {
        private UserManager<User>_UserManager;
        private RoleManager<AppRole> _roleManager;
        public Admin(UserManager<User>UserManager,RoleManager<AppRole> roleManager)
        {
            _UserManager=UserManager;
            _roleManager=roleManager;
        }
        public IActionResult Index()
        {
            
            return View(_UserManager.Users);
        }
        public async Task<IActionResult> Edit( int id)
        {
            if(id==0)
            {
                return RedirectToAction("Index");
            }
            var user=await _UserManager.FindByIdAsync(id.ToString());
            if(user !=null)
            {
                
                ViewBag.Roles=await _roleManager.Roles.Select(i=>i.Name).ToListAsync();
                return View(
                    new EditViewModels
                    {
                        UserId=id.ToString(),
                        Name=user.Name,
                        Email=user.Email,
                        SelectedRoles=await _UserManager.GetRolesAsync(user)
                    }
                );
            }
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditViewModels model)
        {
            if(id.ToString() != model.UserId)
            {
                return NotFound();
            }
            if(ModelState.IsValid)
            {
                var user=await _UserManager.FindByIdAsync(id.ToString());
                if(user != null )
                {
                    bool isSuperAdmin = User.IsInRole("admin");
                    if (!isSuperAdmin && model.SelectedRoles != null)
                    {
                        
                        if (model.SelectedRoles.Contains("admin") || model.SelectedRoles.Contains("moderator"))
                        {
                            ModelState.AddModelError("", "Yetkiniz yetersiz: Admin veya Moderatör yetkisi veremezsiniz.");
                            ViewBag.Roles = await _roleManager.Roles.Select(i => i.Name).ToListAsync();
                            return View(model);
                        }
                    }
                    user.Email=model.Email;
                    user.Name=model.Name;
                    var result=await _UserManager.UpdateAsync(user);
                    if(result.Succeeded && !string.IsNullOrEmpty(model.Password))
                    {
                        await _UserManager.RemovePasswordAsync(user);
                        await _UserManager.AddPasswordAsync(user,model.Password);
                    }
                     if(result.Succeeded)
                    {
                        await _UserManager.RemoveFromRolesAsync(user, await _UserManager.GetRolesAsync(user));
                        if(model.SelectedRoles!=null)
                        {
                            await _UserManager.AddToRolesAsync(user, model.SelectedRoles);
                        }
                        return RedirectToAction("Index");
                    }
                    foreach (IdentityError err in result.Errors)
                    {
                        ModelState.AddModelError("",err.Description);
                    }
                    
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var user=await _UserManager.FindByIdAsync(id.ToString());
            if(user !=null)
            {
                await _UserManager.DeleteAsync(user);
            }
            return RedirectToAction("Index");
        }
    }
    
}