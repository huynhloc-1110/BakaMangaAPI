using BakaMangaAPI.Models;

namespace BakaMangaAPI.Data;

public partial class SeedData
{
    private Dictionary<string, Group> SeedGroups()
    {
        var uploader1 = _userManager.FindByEmailAsync("LocLe345@example.com").Result;
        var uploader2 = _userManager.FindByEmailAsync("Admin1@example.com").Result;
        var uploader3 = _userManager.FindByEmailAsync("Uploader1@example.com").Result;

        var groupAvatarUrl = _configuration["JWT:ValidIssuer"] + "/img/avatars/";

        Dictionary<string, Group> groups = new()
        {
            ["English Group"] = new()
            {
                Name = "Tengoku Team",
                AvatarPath = groupAvatarUrl + "tengoku.png",
                IsMangaGroup = true,
                Biography = "Please consider visiting our main site: https://lhtranslation.net/\r\nWE NEED MORE JAPANESE TRANSLATOR, PLS CONTACT DISCORD: malsi#3621 OR tagomisugi2@gmail.com",
                Members = new()
                {
                    new() { User = uploader1, GroupRoles = GroupRole.Owner | GroupRole.GroupUploader },
                    new() { User = uploader3, GroupRoles = GroupRole.Moderator },
                }
            },
            ["Japan Group"] = new()
            {
                Name = "Yannu no Mahou",
                AvatarPath = groupAvatarUrl + "yannu.png",
                Biography = "私たちはマンガの日本語翻訳を専門とするファンサブです。",
                IsMangaGroup = true,
                Members = new()
                {
                    new() { User = uploader2, GroupRoles = GroupRole.Owner | GroupRole.GroupUploader },
                }
            },
            ["VietNam Group"] = new()
            {
                Name = "Tiệm đồ ngọt",
                AvatarPath = groupAvatarUrl + "tiemdongot.png",
                Biography = "Tụi mình chuyên dịch thể loại rom-com.",
                IsMangaGroup = true,
                Members = new()
                {
                    new() { User = uploader3, GroupRoles = GroupRole.Owner | GroupRole.GroupUploader },
                }
            },
        };

        _context.Groups.AddRange(groups.Values);
        return groups;
    }
}
