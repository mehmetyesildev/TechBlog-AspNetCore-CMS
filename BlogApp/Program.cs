using BlogApp.Concrete.EfCore;
using BlogApp.Data.Abstract;
using BlogApp.Data.Concrete.EfCore;
using BlogApp.Entity;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IEmailSender,SmtpEmailSender>(i=>
    new SmtpEmailSender(
        builder.Configuration["EmailSender:Host"],
        builder.Configuration.GetValue<int>("EmailSender:Port"),
        builder.Configuration.GetValue<bool>("EmailSender:EnableSSl"),
        builder.Configuration["EmailSender:Username"],
        builder.Configuration["EmailSender:Password"],
        builder.Configuration["EmailSender:Sender"]
    ));
builder.Services.AddControllersWithViews();


builder.Services.AddDbContext<BlogContext>(Options =>
{
    var config = builder.Configuration;
    var Connetionstring = config.GetConnectionString("sql_connetion");
    Options.UseSqlite(Connetionstring);
});

builder.Services.AddScoped<IPostRepository, EfPostRepository>();
builder.Services.AddScoped<ITagRepository,EfTagRepository>();
builder.Services.AddScoped<ICommentRepository, EfCommentRepository>();


builder.Services.AddIdentity<User,AppRole>().
    AddEntityFrameworkStores<BlogContext>().
    AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(Options=>
{
    Options.Password.RequiredLength=6;
    Options.Password.RequireNonAlphanumeric=false;
    Options.Password.RequireLowercase=false;
    Options.Password.RequireUppercase=false;
    Options.Password.RequireDigit=false;
    Options.User.RequireUniqueEmail=true;
    Options.User.RequireUniqueEmail=true;
    Options.User.AllowedUserNameCharacters= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789 -._@+";
    Options.Lockout.DefaultLockoutTimeSpan=TimeSpan.FromMinutes(5);
    Options.Lockout.MaxFailedAccessAttempts=5;
    Options.SignIn.RequireConfirmedEmail=false;
});
builder.Services.ConfigureApplicationCookie(Options=>
{
    Options.LoginPath = "/Users/Login";
    Options.AccessDeniedPath="/Users/AccessDenied";
    Options.SlidingExpiration=true;
    Options.ExpireTimeSpan=TimeSpan.FromDays(30);
});
var app = builder.Build();

app.UseRouting();
app.UseStaticFiles();
app.UseAuthentication();//tanıma
app.UseAuthorization();//yetkilendirme

SeedData.TestVerileriniDoldur(app).Wait();

app.MapControllerRoute(
    name:"post_details",
    pattern: "posts/details/{url}",
    defaults: new {Controller="Posts",Action="Details"}
);
app.MapControllerRoute(
    name: "posts_by_tag",
    pattern: "posts/tag/{tag}",
    defaults: new { Controller = "Posts", Action = "Index" }
);
app.MapControllerRoute(
    name: "user_profile",
    pattern: "profile/{username}",
    defaults: new { Controller = "Users", Action = "Profile" }
);
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Posts}/{action=Index}/{id?}"
);

app.Run();
