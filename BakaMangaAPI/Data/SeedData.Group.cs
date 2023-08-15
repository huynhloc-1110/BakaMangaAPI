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
                Name = "English Group",
                Biography = "El Rincón de Atena Asamiya:\r\n\r\nSitio dedicado a Athena Asamiya, hecho por fans para fans, con la intención de compartir información referente al personaje y a los juegos en donde aparece, principalmente King of Fighters",
                Members = new()
                {
                    new() { User = uploader1, IsLeader = true },
                    new() { User = uploader3 },
                }
            },
            ["Japan Group"] = new()
            {
                Name = "Japan Group",
                Biography = "Matkojebanie czy to JoJo po polsku?",
                Members = new()
                {
                    new() { User = uploader2, IsLeader = true },
                }
            },
            ["VietNam Group"] = new()
            {
                Name = "VietNam Group",
                Biography = "Dedicado a traducir de inglés a español mangas que me parezcan interesantes y que no estén siendo traducidos o que hayan sido abandonados, especialmente los de la franquicia de Girls und Panzer, con la mayor pulcritud lingüística que me sea posible (de ahí el nombre).",
            },
        };

        _context.Groups.AddRange(groups.Values);
        return groups;
    }
}
