using MeetingSchedule.DBContext;
using MeetingSchedule.Models.DTOs.Meetings;
using MeetingSchedule.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MeetingSchedule.DataAccess
{
    public interface IMeetingDataAccess
    {
        IEnumerable<Meeting> Get();
        Meeting Get(int id);
        IEnumerable<Meeting> GetByRoomId(int roomId);
        IEnumerable<Meeting> GetByMonth(DateTime date);
        Meeting Patch(int id, PatchMeeting meetingData);
        void Delete(int id);
        Meeting Add(NewMeeting newMeeting);
        IEnumerable<Meeting> GetByRoomIdAndTimeConflict(int roomId, DateTime start, DateTime end);
        IEnumerable<Meeting> GetByTimeConflict(DateTime start, DateTime end);
        IEnumerable<Meeting> GetByRoomIdAndTimeConflict(int meetingId, int roomId, DateTime start, DateTime end);
        IEnumerable<Meeting> GetByTimeConflict(int meetingId, DateTime start, DateTime end);
    }

    public class MeetingDataAccess : IMeetingDataAccess
    {
        private readonly MeetingScheduleContext _context;

        public MeetingDataAccess(MeetingScheduleContext context)
        {
            _context = context;
        }

        public Meeting Add(NewMeeting newMeeting)
        {
            var meeting = new Meeting()
            {
                Name = newMeeting.Name,
                Description = newMeeting.Description,
                RoomId = newMeeting.RoomId,
                Start = newMeeting.Start,
                End = newMeeting.End
            };

            _context.Meetings.Add(meeting);
            _context.SaveChanges();
            _context.Entry(meeting).Reference(meeting => meeting.Room).Load();

            return meeting;
        }

        public void Delete(int id)
        {
            var meeting = Get(id);
            _context.Remove(meeting);
            _context.SaveChanges();
        }

        public Meeting Get(int id)
        {
            var meeting = _context.Meetings.Where(meeting => meeting.Id == id).ToList();
            return meeting.FirstOrDefault();
        }

        public IEnumerable<Meeting> Get()
        {
            var meetings = _context.Meetings.ToList();
            return meetings;
        }

        public IEnumerable<Meeting> GetByMonth(DateTime date)
        {
            var meetings = _context.Meetings
                .Where((meeting) =>
                    meeting.Start.Date.Year == date.Date.Year
                    && meeting.Start.Date.Month == date.Date.Month
                ).ToList();
            return meetings;
        }

        public IEnumerable<Meeting> GetByRoomId(int roomId)
        {
            var meetings = _context.Meetings.Where((meeting) => meeting.RoomId == roomId).ToList();
            return meetings;
        }

        public IEnumerable<Meeting> GetByRoomIdAndTimeConflict(int roomId, DateTime start, DateTime end)
        {
            var meetings = _context.Meetings
                .Where(
                (meeting) =>
                    meeting.RoomId == roomId &&
                    (
                        (meeting.Start >= start &&
                        meeting.Start < end) ||
                        (meeting.End > start &&
                        meeting.End <= end) ||
                        (meeting.Start < start &&
                        meeting.End > end)
                    )
                )
                .ToList();
            return meetings;
        }

        public IEnumerable<Meeting> GetByRoomIdAndTimeConflict(int meetingId, int roomId, DateTime start, DateTime end)
        {
            var meetings = _context.Meetings
                .Where(
                (meeting) =>
                    meeting.Id != meetingId &&
                    meeting.RoomId == roomId &&
                    (
                        (meeting.Start >= start &&
                        meeting.Start < end) ||
                        (meeting.End > start &&
                        meeting.End <= end) ||
                        (meeting.Start < start &&
                        meeting.End > end)
                    )
                )
                .ToList();
            return meetings;
        }

        public IEnumerable<Meeting> GetByTimeConflict(DateTime start, DateTime end)
        {
            var meetings = _context.Meetings
            .Where(
            (meeting) =>
                    (meeting.Start >= start &&
                    meeting.Start < end) ||
                    (meeting.End > start &&
                    meeting.End <= end) ||
                    (meeting.Start < start &&
                    meeting.End > end)
                )
                .ToList();
            return meetings;
        }

        public IEnumerable<Meeting> GetByTimeConflict(int meetingId, DateTime start, DateTime end)
        {
            var meetings = _context.Meetings
            .Where(
            (meeting) =>
                    meeting.Id != meetingId &&
                    (
                        (meeting.Start >= start &&
                    meeting.Start < end) ||
                    (meeting.End > start &&
                    meeting.End <= end) ||
                    (meeting.Start < start &&
                    meeting.End > end)
                    )
                )
                .ToList();
            return meetings;
        }

        public Meeting Patch(int id, PatchMeeting meetingData)
        {
            var meeting = Get(id);
            if (meeting is null)
                return null;

            if (meetingData.Name != null)
                meeting.Name = meetingData.Name;

            if (meetingData.RoomId != 0)
                meeting.RoomId = meetingData.RoomId;

            if (meetingData.Description != null)
                meeting.Description = meetingData.Description;

            if (meetingData.Start != null)
                meeting.Start = meetingData.Start;

            if (meetingData.End != null)
                meeting.End = meetingData.End;

            _context.SaveChanges();

            return Get(id);
        }
    }
}
