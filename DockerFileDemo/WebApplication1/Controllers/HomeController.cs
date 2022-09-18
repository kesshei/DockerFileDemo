using Common;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
[ApiController]
[Route("[controller]")]
public class HomeController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        var info = ConfigManage.GetSetting("conn");
        return info;
    }
}
}