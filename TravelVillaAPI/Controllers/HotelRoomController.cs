
using Business.Repository.IRepository;
using Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using TravelVillaServer.Model;

namespace TravelVillaAPI.Controllers
{
    [Route("api/[controller]")]
    public class HotelRoomController : Controller
    {
        private readonly IHotelRoomRepository _hotelRepo;
        public HotelRoomController(IHotelRoomRepository repo)
        {
            _hotelRepo = repo;
        }
        //[Authorize(Roles = SD.Role_Admin)]
        [HttpGet]
        public async Task<IActionResult> GetHotelRooms(string checkInDate = null, string checkOutDate = null)
        {
            if (string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMsg = "All parameters need to be supplied"
                });
            }
            if (!DateTime.TryParseExact(checkInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckInDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMsg = "Invalid CheckIn date format. valid format will be MM/dd/yyyy"
                });
            }
            if (!DateTime.TryParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,

                    ErrorMsg = "Invalid CheckOut date format. valid format will be MM/dd/yyyy"
                });
            }
            var allRooms = await _hotelRepo.GetAllHotelRooms(checkInDate, checkOutDate);
            return Ok(allRooms);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> GetHotelRoom(int? Id, string checkInDate = null, string checkOutDate = null)
        {
            if (Id==null)
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "Error",
                    ErrorMsg = "Room Id is Null",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            if (string.IsNullOrEmpty(checkInDate) || string.IsNullOrEmpty(checkOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMsg = "All parameters need to be supplied"
                });
            }
            if (!DateTime.TryParseExact(checkInDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckInDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMsg = "Invalid CheckIn date format. valid format will be MM/dd/yyyy"
                });
            }
            if (!DateTime.TryParseExact(checkOutDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dtCheckOutDate))
            {
                return BadRequest(new ErrorModel()
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMsg = "Invalid CheckOut date format. valid format will be MM/dd/yyyy"
                });
            }

            var roomDetails = await _hotelRepo.GetHotelRoom(Id.Value, checkInDate, checkOutDate);
            if (roomDetails == null)
            {
                return BadRequest(new ErrorModel()
                {
                    Title = "Error",
                    ErrorMsg = "RoomDetails is Null",
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(roomDetails);
        }

    }
}
