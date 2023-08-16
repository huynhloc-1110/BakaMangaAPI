using BakaMangaAPI.Models;

namespace BakaMangaAPI.Data;

public partial class SeedData
{
    private Dictionary<string, Group> SeedGroups()
    {
        var uploader1 = _userManager.FindByEmailAsync("LocLe345@example.com").Result;
        var uploader2 = _userManager.FindByEmailAsync("TriTat567@example.com").Result;
        var uploader3 = _userManager.FindByEmailAsync("HuyNguyen123@example.com").Result;

        Dictionary<string, Group> groups = new()
        {
            ["English Group"] = new()
            {
                Name = "Tengoku Team",
                Biography = "Please consider visiting our main site: https://lhtranslation.net/\r\nWE NEED MORE JAPANESE TRANSLATOR, PLS CONTACT DISCORD: malsi#3621 OR tagomisugi2@gmail.com",
                Members = new()
                {
                    new() { User = uploader1, IsLeader = true },
                    new() { User = uploader3 },
                }
            },
            ["Japan Group"] = new()
            {
                Name = "Yannu no Mahou",
                Biography = "私たちはマンガの日本語翻訳を専門とするファンサブです。",
                Members = new()
                {
                    new() { User = uploader2, IsLeader = true },
                }
            },
            ["VietNam Group"] = new()
            {
                Name = "Tiệm đồ ngọt",
                Biography = "Tụi mình chuyên dịch thể loại rom-com.",
                Members = new()
                {
                    new() { User = uploader3, IsLeader = true },
                }
            },
        };

        _context.Groups.AddRange(groups.Values);
        return groups;
    }
}
