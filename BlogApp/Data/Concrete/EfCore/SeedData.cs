
using BlogApp.Entity;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Data.Concrete.EfCore;

public static class SeedData
{
    public static void TestVerileriniDoldur(IApplicationBuilder app)
    {
        var context=app.ApplicationServices.CreateAsyncScope().ServiceProvider.GetService<BlogContext>();
        if(context!=null)
        {
            if(context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            if (!context.Tags.Any())
            {
                context.Tags.AddRange(
                    new Tag { Text = "Web Programlama",Url="web-programlama",Color=TagColors.warning},
                    new Tag { Text = "Backend",Url = "backend", Color = TagColors.danger },
                    new Tag { Text = "Frondend", Url= "frondend", Color = TagColors.secondary},
                    new Tag { Text = "Fullstack",Url= "fullstack", Color = TagColors.secondary },
                    new Tag { Text = "PHP", Url= "PHP", Color = TagColors.primary }
                );
                context.SaveChanges();
            }
            if (!context.Users.Any())
            {
                context.Users.AddRange(
                    new User { UserName = "MehmetYeşil",Name="Mehmet Yeşil",Email="info@mehmetyesil.com", Password="123456", Image="p1.jpg"},
                    new User { UserName = "Ahmetcan", Name = "Ahmet can", Email = "info@ahmetcan.com", Password = "123456", Image = "p2.jpg" }
                );
                context.SaveChanges();
            }
            if (!context.Posts.Any())
            {
                context.Posts.AddRange(
                   new Post
                   {
                       Title = "ASP.NET Core 8.0: Web Geliştirmenin Yeni Çağı",
                       Url = "aspnet-core-8-inceleme",
                       IsActive = true,
                       Image = "1.png", // 
                       Publishedon = DateTime.Now.AddDays(-5),
                       Tags = context.Tags.Take(3).ToList(),
                       UserId = 1,

                       Description = "Microsoft'un en yeni LTS sürümü .NET 8 ile gelen Native AOT, Blazor Unified ve performans devrimi hakkında bilmeniz gereken her şey.",
                       Content = """
                        <p>Yazılım dünyasında beklenen an geldi! Microsoft, uzun vadeli destek (LTS) sunan <strong>.NET 8</strong> sürümünü yayınladı. Bu sürüm, sadece bir güncelleme değil, performans ve verimlilik anlamında devrim niteliğinde özellikler taşıyor.</p>

                        <h3>1. Native AOT (Ahead-of-Time) Derleme</h3>
                        <p>ASP.NET Core 8.0 ile gelen en büyük yeniliklerden biri Native AOT desteğidir. Artık uygulamalarımız doğrudan makine koduna derleniyor. Bu ne anlama geliyor?</p>
                        <ul>
                            <li>Daha hızlı açılış süreleri (Startup time).</li>
                            <li>Daha az bellek (RAM) kullanımı.</li>
                            <li>Docker container boyutlarında ciddi küçülme.</li>
                        </ul>

                        <h3>2. Blazor Unified (Birleşik Blazor)</h3>
                        <p>Eskiden Blazor Server ve Blazor WebAssembly arasında seçim yapmak zorundaydık. .NET 8 ile gelen yeni "Render Modes" sayesinde, artık aynı sayfa içinde hem sunucu taraflı hem de istemci taraflı renderlama yapabiliyoruz. Bu, kullanıcı deneyimini (UX) zirveye taşıyor.</p>

                        <h3>3. C# 12 Özellikleri</h3>
                        <p>Kod yazarken daha az efor harcamamız için <em>Primary Constructors</em> ve <em>Collection Expressions</em> gibi söz dizimi (syntax) iyileştirmeleri de bu paketle birlikte geldi.</p>

                        <div class="alert alert-info">
                            <strong>Sonuç:</strong> Eğer yeni bir projeye başlıyorsanız veya mevcut projenizi modernize etmek istiyorsanız, ASP.NET Core 8.0 kesinlikle doğru tercih.
                        </div>
                        """,

                                    Comments = new List<Comment> {
                        new Comment { Text = "Native AOT desteği cloud maliyetlerini düşürmek için harika olmuş.", PublishedOn = DateTime.Now.AddDays(-4), UserId = 1 },
                        new Comment { Text = "Blazor tarafındaki render mode değişikliği işimizi çok kolaylaştırdı.", PublishedOn = DateTime.Now.AddDays(-2), UserId = 2 }
                    }
                   },
                    new Post
                    {
                        Title = "PHP",
                        Description = "PHP dersleri",
                        Content = "PHP dersleri",
                        Url = "php",
                        IsActive = true,
                        Image = "2.png",
                        Publishedon = DateTime.Now.AddDays(-20),
                        Tags = context.Tags.Take(2).ToList(),
                        UserId = 1
                    },
                    new Post
                    {
                        Title = "Dijango",
                        Description = "Dijango dersleri",
                        Content = "Dijango dersleri",
                        Url = "dijango",
                        IsActive = true,
                        Image = "3.png",
                        Publishedon = DateTime.Now.AddDays(-5),
                        Tags = context.Tags.Take(4).ToList(),
                        UserId = 2
                    }
                );
                context.SaveChanges();
            }
        }
        
    }

}
