using Business.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TravelVillaAPI.Controllers
{
    [Route("api/[controller]")]
    public class HotelAmenityController : Controller
    {
        private readonly IAmenityRepository _hotelAmenityRepository;

        public HotelAmenityController(IAmenityRepository hotelAmenityRepository)
        {
            _hotelAmenityRepository = hotelAmenityRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetHotelAmenities()
        {


            var allAmenity = await _hotelAmenityRepository.GetAllHotelAmenity();
            return Ok(allAmenity);
        }
    }
}
