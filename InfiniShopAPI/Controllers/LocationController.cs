using API.Controllers;
using API.Data;
using API.Entities;
using InfiniShopAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InfiniShopAPI.Controllers
{
    public class LocationController : BaseApiController
    {

        private readonly DataContext _dataContext;
        public LocationController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [AllowAnonymous]
        [HttpGet("allcountries")]
        public async Task<ActionResult<IEnumerable<Country>>> GetAllCountries()
        {
            return await _dataContext.Countries.ToListAsync();

        }
        [AllowAnonymous]
        [HttpGet("allprovinces")]
        public async Task<ActionResult<IEnumerable<Province>>> GetAllProvinces()
        {
            return await _dataContext.Provinces.ToListAsync();

        }
        [AllowAnonymous]
        [HttpGet("allcities")]
        public async Task<ActionResult<IEnumerable<City>>> GetAllCities()
        {
            return await _dataContext.Cities.ToListAsync();

        }

        [AllowAnonymous]
        [HttpGet("GetProvinceById/{id}")]
        public async Task<ActionResult<Province>> GetProvinceById(int id)
        {
            var province = await _dataContext.Provinces.FindAsync(id);
            
            if (province == null)
            {
                return NotFound();
            }

            return province;

        }

        [AllowAnonymous]
        [HttpGet("GetCityById/{id}")]
        public async Task<ActionResult<City>> GetCityById(int id)
        {
            var city = await _dataContext.Cities.FindAsync(id);

            if (city == null)
            {
                return NotFound();
            }

            return city;

        }


        //Get province by country id - 
        [AllowAnonymous]
        [HttpGet("GetProvincesByCountryId/{id}")]
        public async Task<ActionResult<IEnumerable<Province>>> GetProvincesByCountryId(int id)
        {
            var provinceList = _dataContext.Provinces.Where(s => s.Country.CountryId == id).ToListAsync();
            if (provinceList == null)
            {
                return NotFound();
            }
            return await provinceList;
        }

        //Get cities by province id 
        [AllowAnonymous]
        [HttpGet("GetCitiesByProvinceId/{id}")]
        public async Task<ActionResult<IEnumerable<City>>> GetCitiesByProvinceId(int id)
        {
            var citiesList = _dataContext.Cities.Where(s => s.Province.ProvinceId == id).ToListAsync();
            if (citiesList == null)
            {
                return NotFound();
            }
            return await citiesList;
        }
    }
}
