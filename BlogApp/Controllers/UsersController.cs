using System.Security.Claims;
using BlogApp.Data.Abstract;
using BlogApp.Entity;
using BlogApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Controllers
{
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _singInManager;
        private readonly IEmailSender _emailSender;
        public UsersController(UserManager<User> userManager,SignInManager<User>singInManager,IEmailSender emailSender)
        {
            _userManager=userManager;
            _singInManager=singInManager;
            _emailSender=emailSender;
        }
        public IActionResult Login()
        {
            if(User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Index","Posts");
            }
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                var user=new User
                {
                    UserName=model.UserName,
                    Name=model.Name,
                    Email=model.Email,
                    Image="avatar.jpg"
                };
                var result=await _userManager.CreateAsync(user,model.Password);
                if(result.Succeeded)
                {
                    var token=await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    var url=Url.Action("ConfirmEmail","Users",new{Id=user.Id,token});
                    await _emailSender.SendEmailAsync(user.Email!,"Hesap Onayı",$"Lütfen email hesabınızı onaylamak için linke tıklayınız<a href='http://localhost:5160{url}'>tıklayınız</a>");
                    TempData["message"] = "Email hesabınızdaki onay mailine tıklayınız";
                    return RedirectToAction("Login");
                }
                foreach (var err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);
        }
        public async Task<IActionResult>ConfirmEmail(int Id,string token)
        {
            if(Id==0||token==null)
            {
                 TempData["message"] = "Gecersiz token bilgisi";
                 return RedirectToAction("Login");
            }
            var user= await _userManager.FindByIdAsync(Id.ToString());
            if(user!=null)
            {
                var result=await _userManager.ConfirmEmailAsync(user,token);
                if(result.Succeeded)
                {
                    TempData["message"] = "Hesabınız Onaylandı";
                    return RedirectToAction("Login");
                }
            }
            TempData["message"] = "Kulanıcı bulunamadı";
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _singInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult>ForgotPassword(string Email)
        {
            if(string.IsNullOrEmpty(Email))
            {
                TempData["message"]="Eposta adresi giriniz";
                return View();
            }
            var user=await _userManager.FindByEmailAsync(Email);
            if(user==null)
            {
                TempData["message"] = "Eposta adresi ile eşlesen bir kayıt yok";
                return View();
            }
            var token=await _userManager.GeneratePasswordResetTokenAsync(user);
            var url=Url.Action("ResetPassword","Users",new{user.Id,token});
            await _emailSender.SendEmailAsync(Email,"Porola Sıfırlama",$"Parolayı yenilemk için linke tıklayınız <a href='http://localhost:5160{url}'>tıklayınız</a>");
            TempData["message"]="Eposta adresinize gönderilen link ile sifrenizi sıfırlayabilirsiniz";
            return View();
        }
        public IActionResult ResetPassword(int Id, string token)
        {
             if(Id==0||token==null)
            {
                 TempData["message"] = "Gecersiz token bilgisi";
                 return RedirectToAction("Login");
            }
            var model=new ResetPasswordModels{Token=token};
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult>ResetPassword(ResetPasswordModels model)
        {
            if(ModelState.IsValid)
            {
                var users=await _userManager.FindByEmailAsync(model.Email);
                if(users==null)
                {
                    TempData["message"] = "Eposta adresi ile eşlesen bir kayıt yok";
                    return RedirectToAction("Login");
                }
                var result=await _userManager.ResetPasswordAsync(users,model.Token,model.Password);
                if(result.Succeeded)
                {
                    TempData["message"] ="Sifreniz değiştirildi";
                    return RedirectToAction("Login");
                }
                foreach (IdentityError err in result.Errors)
                {
                    ModelState.AddModelError("",err.Description);
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if(ModelState.IsValid)
            {
                var User=await _userManager.FindByEmailAsync(model.Email!);
                if(User!=null)
                {
                   await _singInManager.SignOutAsync();
                   if(!await _userManager.IsEmailConfirmedAsync(User))
                    {
                        ModelState.AddModelError("","Hesapınızı Onaylayınız");
                        return View(model);
                    } 
                   var result=await _singInManager.PasswordSignInAsync(User.UserName!,model.Password!,false,true);
                   if(result.Succeeded)
                    {
                        await _userManager.ResetAccessFailedCountAsync(User);
                        await _userManager.SetLockoutEndDateAsync(User,null);
                        return RedirectToAction("Index","Posts");
                    }
                    else if(result.IsLockedOut)
                    {
                        var lockoutDate=await _userManager.GetLockoutEndDateAsync(User);
                        var timeleft=lockoutDate.Value-DateTime.UtcNow;
                        ModelState.AddModelError("",$"Hesabınız kitlendi, Lütfen {timeleft.Minutes}dakika sonra deneyiniz" );
                    }else
                    {
                        ModelState.AddModelError("", "Parolanız hatalı");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Bu email adresi ile hesap bulunamadı");
                }
            }
            return View(model);
        }
        public IActionResult Profile(string username)
        {
            if(string.IsNullOrEmpty(username))
            {
                return NotFound();
            }
            var user=_userManager
                    .Users
                    .Include(u=>u.Posts)
                    .Include(x=>x.Comments)
                    .ThenInclude(x=>x.Post)
                    .FirstOrDefault(x=>x.UserName== username);
            if(user==null)
            {
                return NotFound();
            }
            return View(user);
        }


    } 
}