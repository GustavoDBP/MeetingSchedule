using MeetingSchedule.DBContext;
using MeetingSchedule.Models.DTOs.Room;
using MeetingSchedule.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingSchedule.DataAccess
{
    public interface IRoomDataAccess
    {
        Room Add(NewRoom newRoom);
        IEnumerable<Room> Get();
        Room Get(int id);
        Room GetFirstAvailableRoom(DateTime start, DateTime end);
        Room GetFirstAvailableRoom(int meetingId, DateTime start, DateTime end);
    }

    public class RoomDataAccess : IRoomDataAccess
    {
        private readonly MeetingScheduleContext _context;

        public RoomDataAccess(MeetingScheduleContext context)
        {
            _context = context;
        }

        public Room Add(NewRoom newRoom)
        {
            var room = new Room()
            {
                Name = newRoom.name
            };

            _context.Rooms.Add(room);
            _context.SaveChanges();

            return room;
        }

        public IEnumerable<Room> Get()
        {
            var room = _context.Rooms.ToList();
            return room;
        }

        public Room Get(int id)
        {
            var room = _context.Rooms.Where((room) => room.Id == id).FirstOrDefault();
            return room;
        }

        public Room GetFirstAvailableRoom(DateTime start, DateTime end)
        {
            var room = _context.Rooms.FirstOrDefault(
                (room) => room.Meetings.FirstOrDefault((meeting) =>
                    (meeting.Start >= start &&
                    meeting.Start < end) ||
                    (meeting.End > start &&
                    meeting.End <= end) ||
                    (meeting.Start < start &&
                    meeting.End > end)
                ) == null
            );

            return room;
        }

        public Room GetFirstAvailableRoom(int meetingId, DateTime start, DateTime end)
        {
            var room = _context.Rooms.FirstOrDefault(
                (room) => room.Meetings.FirstOrDefault((meeting) =>
                    meeting.Id != meetingId &&
                    (
                        (meeting.Start >= start &&
                        meeting.Start <= end) ||
                        (meeting.End >= start &&
                        meeting.End <= end) ||
                        (meeting.Start < start &&
                        meeting.End > end)
                    )
                ) == null
            );

            return room;
        }
    }
}
