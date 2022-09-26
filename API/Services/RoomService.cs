using MeetingSchedule.DataAccess;
using MeetingSchedule.Models.DTOs.Room;
using MeetingSchedule.Models.Entities;
using System;
using System.Collections.Generic;

namespace MeetingSchedule.Services
{
    public interface IRoomService
    {
        Room Add(NewRoom newRoom);
        IEnumerable<Room> Get();
        Room Get(int id);
        Room GetFirstAvailableRoom(DateTime start, DateTime end);
        Room GetFirstAvailableRoom(int meetingId, DateTime start, DateTime end);
        Models.ViewModels.Conflicts GetConflictsByRoomIdAndTime(int roomId, DateTime start, DateTime end);
        Models.ViewModels.Conflicts GetConflictsByRoomIdAndTime(int meetingId, int roomId, DateTime start, DateTime end);
        Models.ViewModels.Conflicts GetConflictsByTime(DateTime start, DateTime end);
        Models.ViewModels.Conflicts GetConflictsByTime(int meetingId, DateTime start, DateTime end);
    }

    public class RoomService : IRoomService
    {
        private readonly IRoomDataAccess _roomDataAccess;
        private readonly IMeetingDataAccess _meetingDataAccess;

        public RoomService(IRoomDataAccess roomDataAccess, IMeetingDataAccess meetingDataAccess)
        {
            _roomDataAccess = roomDataAccess;
            _meetingDataAccess = meetingDataAccess;
        }

        public Room Add(NewRoom newRoom)
        {
            return _roomDataAccess.Add(newRoom);
        }

        public IEnumerable<Room> Get()
        {
            return _roomDataAccess.Get();
        }

        public Room Get(int id)
        {
            return _roomDataAccess.Get(id);
        }

        public Models.ViewModels.Conflicts GetConflictsByRoomIdAndTime(int roomId, DateTime start, DateTime end)
        {
            var conflictedMeetigs = _meetingDataAccess.GetByRoomIdAndTimeConflict(roomId, start, end);
            var conflicts = new Models.ViewModels.Conflicts();
            conflicts.MeetingConflicts = new List<Models.ViewModels.Meeting>();
            foreach(var conflictedMeeting in conflictedMeetigs)
            {
                conflicts.MeetingConflicts.Add(conflictedMeeting.ToViewModel());
            }

            return conflicts;
        }

        public Models.ViewModels.Conflicts GetConflictsByRoomIdAndTime(int meetingId, int roomId, DateTime start, DateTime end)
        {
            var conflictedMeetigs = _meetingDataAccess.GetByRoomIdAndTimeConflict(meetingId, roomId, start, end);
            var conflicts = new Models.ViewModels.Conflicts();
            conflicts.MeetingConflicts = new List<Models.ViewModels.Meeting>();
            foreach (var conflictedMeeting in conflictedMeetigs)
            {
                conflicts.MeetingConflicts.Add(conflictedMeeting.ToViewModel());
            }

            return conflicts;
        }

        public Models.ViewModels.Conflicts GetConflictsByTime(DateTime start, DateTime end)
        {
            var conflictedMeetigs = _meetingDataAccess.GetByTimeConflict(start, end);
            var conflicts = new Models.ViewModels.Conflicts();
            conflicts.MeetingConflicts = new List<Models.ViewModels.Meeting>();
            foreach (var conflictedMeeting in conflictedMeetigs)
            {
                conflicts.MeetingConflicts.Add(conflictedMeeting.ToViewModel());
            }

            return conflicts;
        }

        public Models.ViewModels.Conflicts GetConflictsByTime(int meetingId, DateTime start, DateTime end)
        {
            var conflictedMeetigs = _meetingDataAccess.GetByTimeConflict(meetingId, start, end);
            var conflicts = new Models.ViewModels.Conflicts();
            conflicts.MeetingConflicts = new List<Models.ViewModels.Meeting>();
            foreach (var conflictedMeeting in conflictedMeetigs)
            {
                conflicts.MeetingConflicts.Add(conflictedMeeting.ToViewModel());
            }

            return conflicts;
        }

        public Room GetFirstAvailableRoom(DateTime start, DateTime end)
        {
            var room = _roomDataAccess.GetFirstAvailableRoom(start, end);
            return room;
        }

        public Room GetFirstAvailableRoom(int meetingId, DateTime start, DateTime end)
        {
            var room = _roomDataAccess.GetFirstAvailableRoom(meetingId, start, end);
            return room;
        }
    }
}
