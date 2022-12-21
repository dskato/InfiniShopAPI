using API.Controllers;
using API.Data;
using API.Entities;
using InfiniShopAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfiniShopAPI.Controllers
{
    public class GlobalAutomotiveServicesController : BaseApiController
    {
        private readonly DataContext _dataContext;
        public GlobalAutomotiveServicesController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [AllowAnonymous]
        [HttpGet("GetAllGlobalAutomotiveServices")]
        public async Task<ActionResult<IEnumerable<GlobalAutomotiveServices>>> GetGlobalAutomotiveServices()
        {
            return await _dataContext.GlobalAutomotiveServices.ToListAsync();

        }

        [AllowAnonymous]
        [HttpGet("GetAllGlobalMotorcycleServices")]
        public async Task<ActionResult<IEnumerable<GlobalMotorcycleServices>>> GetGlobalMotorcycleServices()
        {
            return await _dataContext.GlobalMotorcycleServices.ToListAsync();

        }
    }
}
