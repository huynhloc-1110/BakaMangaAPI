using BakaMangaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BakaMangaAPI.Controllers;

[Route("helper/language")]
[ApiController]
public class LanguageController : ControllerBase
{
    [HttpGet]
    public IActionResult GetLanguages()
    {
        var languages = Enum.GetNames(typeof(Language)).ToList();
        return Ok(languages);
    }
}
