using BlogApp.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore;

public static class SeedData
{
 
    private const string adminUser = "admin";
    private const string adminPassword = "Admin123";
    private const string memberUser = "ahmetcan";
    private const string memberPassword = "User123"; 

    public static async Task TestVerileriniDoldur(IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateAsyncScope();
        
        var context = scope.ServiceProvider.GetService<BlogContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<AppRole>>();

        if (context != null)
        {
            if (context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }

            if (!await roleManager.RoleExistsAsync("admin"))
            {
                await roleManager.CreateAsync(new AppRole { Name = "admin" });
                await roleManager.CreateAsync(new AppRole { Name = "Member" });
            }

            if (!context.Tags.Any())
            {
                context.Tags.AddRange(
                    new Tag { Text = "Web Programlama", Url = "web-programlama", Color = TagColors.warning },
                    new Tag { Text = "Backend", Url = "backend", Color = TagColors.danger },
                    new Tag { Text = "Frontend", Url = "frontend", Color = TagColors.secondary }, 
                    new Tag { Text = "Fullstack", Url = "fullstack", Color = TagColors.secondary },
                    new Tag { Text = "PHP", Url = "php", Color = TagColors.primary }
                );
                await context.SaveChangesAsync();
            }

           
            if (!context.Users.Any())
            {
                var admin = new User 
                { 
                    UserName = adminUser, 
                    Name = "Mehmet Yeşil", 
                    Email = "info@mehmetyesil.com", 
                    Image = "p1.jpg",
                    EmailConfirmed = true 
                };

                var member = new User 
                { 
                    UserName = memberUser, 
                    Name = "Ahmet Can", 
                    Email = "info@ahmetcan.com", 
                    Image = "p2.jpg",
                    EmailConfirmed = true 
                };

                await userManager.CreateAsync(admin, adminPassword);
                await userManager.CreateAsync(member, memberPassword);

              
                await userManager.AddToRoleAsync(admin, "admin");
                await userManager.AddToRoleAsync(member, "Member");
            }

        
            if (!context.Posts.Any())
            {
                // Kullanıcıların gerçek ID'lerini veritabanından buluyoruz (Garanti Yöntem)
                var adminUserEntity = await userManager.FindByNameAsync(adminUser);
                var memberUserEntity = await userManager.FindByNameAsync(memberUser);

                context.Posts.AddRange(
                    new Post
                    {
                        Title = "ASP.NET Core 8.0: Web Geliştirmenin Yeni Çağı",
                        Url = "aspnet-core-8-inceleme",
                        IsActive = true,
                        Image = "1.png",
                        Publishedon = DateTime.Now.AddDays(-5),
                        Tags = context.Tags.Take(3).ToList(),
                        UserId = adminUserEntity!.Id, // Admin'in ID'si

                        Description = "Microsoft'un en yeni LTS sürümü .NET 8 ile gelen yenilikler.",
                        Content = @"<p>Yazılım dünyasında beklenen an geldi! .NET 8 sürümünü yayınladı...</p>",
                        Comments = new List<Comment> {
                            new Comment { Text = "Harika içerik.", PublishedOn = DateTime.Now.AddDays(-4), UserId = memberUserEntity!.Id },
                            new Comment { Text = "Teşekkürler.", PublishedOn = DateTime.Now.AddDays(-2), UserId = adminUserEntity.Id }
                        }
                    },
                    new Post
                    {
                        Title = "PHP Dersleri",
                        Url = "php",
                        Description = "PHP dersleri",
                        Content = "PHP dersleri",
                        IsActive = true,
                        Image = "2.png",
                        Publishedon = DateTime.Now.AddDays(-20),
                        Tags = context.Tags.Take(2).ToList(),
                        UserId = adminUserEntity.Id 
                    },
                    new Post
                    {
                        Title = "Django Dersleri",
                        Url = "django",
                        Description = "Django dersleri",
                        Content = "Django dersleri",
                        IsActive = true,
                        Image = "3.png",
                        Publishedon = DateTime.Now.AddDays(-5),
                        Tags = context.Tags.Take(4).ToList(),
                        UserId = memberUserEntity.Id 
                    }
                );
                await context.SaveChangesAsync();
            }
        }
    }
}