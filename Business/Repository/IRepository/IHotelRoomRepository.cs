using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IHotelRoomRepository
    {

        public Task<HotelRoomDTO> CreateHotelRoom(HotelRoomDTO dto);
        public Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO dto);
        public Task<HotelRoomDTO> GetHotelRoom(int roomId, string checkInDate = null, string checkOutDate = null);
        public Task<int> DeleteHotelRoom(int roomId);
        public Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms(string checkInDate = null, string checkOutDate = null);
        public Task<HotelRoomDTO> IsRoomUnique(string name, int roomid=0);
        public Task<bool> IsRoomBooked(int RoomId, string checkInDate, string checkOutDate);
        public Task<bool> IsRoomBooked2(int RoomId, DateTime checkInDate, DateTime checkOutDate);


    }
}
