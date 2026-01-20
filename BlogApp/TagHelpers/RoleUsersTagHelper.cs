using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using BlogApp.Entity; 

namespace BlogApp.TagHelpers
{
    [HtmlTargetElement("td", Attributes = "asp-role-users")]
    public class RoleUsersTagHelper : TagHelper
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<AppRole> _roleManager;

        public RoleUsersTagHelper(UserManager<User> userManager, RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HtmlAttributeName("asp-role-users")]
        public string RoleId { get; set; } = null!;

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            var userNames = new List<string>();
            var role = await _roleManager.FindByIdAsync(RoleId);

            if (role != null && role.Name != null)
            {
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

                foreach (var user in usersInRole)
                {
                    userNames.Add(user.Name ?? "");
                }
            }

            output.Content.SetHtmlContent(userNames.Count == 0 ? "Kullanıcı yok" : setHtml(userNames));
        }

        private string setHtml(List<string> userNames)
        {
            var html = "<ul>";
            foreach (var item in userNames)
            {
                html += "<li>" + item + "</li>";
            }
            html += "</ul>";
            return html;
        }
    }
}