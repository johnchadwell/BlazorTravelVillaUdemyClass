using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TravelVillaClient.Service.IService;
using System.Net.Http;
using Newtonsoft.Json;

namespace TravelVillaClient.Service
{
    public class HotelAmenityService : IHotelAmenityService
    {
        private readonly HttpClient _client;

        public HotelAmenityService(HttpClient client)
        {
            _client = client;
        }



        public async Task<IEnumerable<HotelAmenityDTO>> GetHotelAmenities()
        {
            var response = await _client.GetAsync($"api/hotelamenity");
            var content = await response.Content.ReadAsStringAsync();

            var rooms = JsonConvert.DeserializeObject<IEnumerable<HotelAmenityDTO>>(content);
            return rooms;
        }
    }
}