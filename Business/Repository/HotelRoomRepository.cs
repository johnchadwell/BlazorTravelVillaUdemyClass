using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Business.Repository.IRepository;
using DataAccess.Data;
using Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

namespace Business.Repository
{
    public class HotelRoomRepository : IHotelRoomRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;

        public HotelRoomRepository(ApplicationDbContext db, IMapper map)
        {
            _db = db;
            _mapper = map;
        }


        public async Task<HotelRoomDTO> CreateHotelRoom(HotelRoomDTO dto)
        {
            HotelRoom hotelRoom = _mapper.Map<HotelRoomDTO, HotelRoom>(dto);
            hotelRoom.CreatedDate = DateTime.Now;
            hotelRoom.CreatedBy = "";
            var newHotelRoom = await _db.HotelRooms.AddAsync(hotelRoom);
            await _db.SaveChangesAsync();
            return _mapper.Map<HotelRoom, HotelRoomDTO>(newHotelRoom.Entity);
        }

        //public async Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms()
        //{
        //    try
        //    {
        //        IEnumerable<HotelRoomDTO> hotelRoomDTOs = _mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDTO>>(_db.HotelRooms.Include(x=>x.Images));
        //        return hotelRoomDTOs;
        //    } catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public async Task<IEnumerable<HotelRoomDTO>> GetAllHotelRooms(string checkInDateStr, string checkOutDatestr)
        {
            try
            {
                IEnumerable<HotelRoomDTO> hotelRoomDTOs =
                            _mapper.Map<IEnumerable<HotelRoom>, IEnumerable<HotelRoomDTO>>
                            (_db.HotelRooms.Include(x => x.Images));
                if (!string.IsNullOrEmpty(checkInDateStr) && !string.IsNullOrEmpty(checkOutDatestr))
                {
                    foreach (HotelRoomDTO hotelRoom in hotelRoomDTOs)
                    {
                        hotelRoom.IsBooked = await IsRoomBooked(hotelRoom.Id, checkInDateStr, checkOutDatestr);
                    }
                }
                return hotelRoomDTOs;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public async Task<HotelRoomDTO> GetHotelRoom(int roomId)
        //{
        //    try
        //    {
        //        HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom,HotelRoomDTO>(
        //          await _db.HotelRooms.Include(x=>x.Images).FirstOrDefaultAsync(x => x.Id == roomId));
        //        return hotelRoom;
        //    } catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
        public async Task<HotelRoomDTO> GetHotelRoom(int roomId, string checkInDateStr, string checkOutDatestr)
        {
            try
            {
                HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(
                    await _db.HotelRooms.Include(x => x.Images).FirstOrDefaultAsync(x => x.Id == roomId));

                if (!string.IsNullOrEmpty(checkInDateStr) && !string.IsNullOrEmpty(checkOutDatestr))
                {
                    hotelRoom.IsBooked = await IsRoomBooked(roomId, checkInDateStr, checkOutDatestr);
                }

                return hotelRoom;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //public async Task<bool> IsRoomBooked(int RoomId, string checkInDatestr, string checkOutDatestr)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(checkOutDatestr) && !string.IsNullOrEmpty(checkInDatestr))
        //        {
        //            DateTime checkInDate = DateTime.ParseExact(checkInDatestr, "MM/dd/yyyy", null);
        //            DateTime checkOutDate = DateTime.ParseExact(checkOutDatestr, "MM/dd/yyyy", null);

        //            //var existingBooking = await _db.RoomOrderDetails.Where(x => x.RoomId == RoomId && x.IsPaymentSuccessful &&
        //            //   //check if checkin date that user wants does not fall in between any dates for room that is booked
        //            //   ((checkInDate < x.CheckOutDate && checkInDate.Date >= x.CheckInDate)
        //            //   //check if checkout date that user wants does not fall in between any dates for room that is booked
        //            //   || (checkOutDate.Date > x.CheckInDate.Date && checkInDate.Date <= x.CheckInDate.Date)
        //            //   )).FirstOrDefaultAsync();

        //            //if (existingBooking != null)
        //            //{
        //            //    return true;
        //            //}
        //            return false;
        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }


        //}


        public async Task<HotelRoomDTO> IsRoomUnique(string name, int roomid=0)
        {
            try
            {
                if (roomid==0)
                {
                    HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(
                      await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()));
                    return hotelRoom;

                }
                else
                {
                    HotelRoomDTO hotelRoom = _mapper.Map<HotelRoom, HotelRoomDTO>(
                      await _db.HotelRooms.FirstOrDefaultAsync(x => x.Name.ToLower() == name.ToLower()
                      && x.Id != roomid)); 
                    return hotelRoom;

                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<bool> IsRoomBooked(int RoomId, string checkInDatestr, string checkOutDatestr)
        {
            try
            {
                if (!string.IsNullOrEmpty(checkOutDatestr) && !string.IsNullOrEmpty(checkInDatestr))
                {
                    DateTime checkInDate = DateTime.ParseExact(checkInDatestr, "MM/dd/yyyy", null);
                    DateTime checkOutDate = DateTime.ParseExact(checkOutDatestr, "MM/dd/yyyy", null);

                    var existingBooking = await _db.RoomOrderDetails.Where(x => x.RoomId == RoomId && x.IsPaymentSuccessful &&
                       //check if checkin date that user wants does not fall in between any dates for room that is booked
                       ((checkInDate < x.CheckOutDate && checkInDate.Date >= x.CheckInDate)
                       //check if checkout date that user wants does not fall in between any dates for room that is booked
                       || (checkOutDate.Date > x.CheckInDate.Date && checkInDate.Date <= x.CheckInDate.Date)
                       )).FirstOrDefaultAsync();

                    if (existingBooking != null)
                    {
                        return true;
                    }
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        public async Task<bool> IsRoomBooked2(int RoomId, DateTime checkInDate, DateTime checkOutDate)
        {
            var status = false;
            var existingBooking = await _db.RoomOrderDetails.Where(x => x.RoomId == RoomId && x.IsPaymentSuccessful &&
            (checkInDate.Date < x.CheckOutDate.Date && checkInDate.Date > x.CheckInDate.Date
            || checkOutDate.Date > x.CheckInDate.Date && checkInDate.Date < x.CheckInDate.Date
            )).FirstOrDefaultAsync();
            if (existingBooking != null)
            {
                status = true;
            }
            return status;
        }

        public async Task<HotelRoomDTO> UpdateHotelRoom(int roomId, HotelRoomDTO dto)
        {
            try
            {
                if (roomId == dto.Id)
                {
                    HotelRoom roomDetails = await _db.HotelRooms.FindAsync(roomId);
                    HotelRoom room = _mapper.Map<HotelRoomDTO, HotelRoom>(dto, roomDetails);
                    room.UpdatedBy = "";
                    room.UpdatedDate = DateTime.Now;
                    var updatedRoom = _db.HotelRooms.Update(room);
                    await _db.SaveChangesAsync();
                    return _mapper.Map<HotelRoom, HotelRoomDTO>(updatedRoom.Entity);
                } else
                {
                    return null;
                }
            } catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<int> DeleteHotelRoom(int roomId)
        {
            var roomDetails = await _db.HotelRooms.FindAsync(roomId);
            if (roomDetails!=null)
            {
                var allimages = await _db.HotelRoomImages.Where(x => x.RoomId == roomId).ToListAsync();
                //foreach(var img in allimages)
                //{
                //    if (File.Exists(img.RoomImageUrl))
                //    {
                //        File.Delete(img.RoomImageUrl);
                //    }

                //}
                _db.HotelRoomImages.RemoveRange(allimages);
                _db.HotelRooms.Remove(roomDetails);
                return await _db.SaveChangesAsync();
            }
            return 0;
        }


    }
}
