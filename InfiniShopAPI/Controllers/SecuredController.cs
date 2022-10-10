using API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InfiniShopAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SecuredController : BaseApiController
    {
        [Authorize]
        [HttpGet]
        public void Test()
        {
            return;
        }
    }
}
