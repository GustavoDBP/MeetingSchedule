using MeetingSchedule.DataAccess;
using MeetingSchedule.Models.DTOs.Meetings;
using MeetingSchedule.Models.Entities;
using System;
using System.Collections.Generic;

namespace MeetingSchedule.Services
{
    public interface IMeetingService
    {
        IEnumerable<Meeting> Get();
        Meeting Get(int id);
        IEnumerable<Meeting> GetByMonth(DateTime date);
        Meeting Patch(int id, PatchMeeting meetingData);
        void Delete(int id);
        Meeting Add(NewMeeting newMeeting);
    }

    public class MeetingService : IMeetingService
    {
        private readonly IMeetingDataAccess _meetingDataAccess;

        public MeetingService(IMeetingDataAccess meetingDataAccess)
        {
            _meetingDataAccess = meetingDataAccess;
        }

        public IEnumerable<Meeting> Get()
        {
            return _meetingDataAccess.Get();
        }

        public Meeting Get(int id)
        {
            return _meetingDataAccess.Get(id);
        }

        public IEnumerable<Meeting> GetByMonth(DateTime date)
        {
            var meetings = _meetingDataAccess.GetByMonth(date);
            return meetings;
        }

        public Meeting Patch(int id, PatchMeeting meetingData)
        {
            return _meetingDataAccess.Patch(id, meetingData);
        }

        public void Delete(int id)
        {
            _meetingDataAccess.Delete(id);
        }

        public Meeting Add(NewMeeting newMeeting)
        {
            var id = _meetingDataAccess.Add(newMeeting).Id;
            return Get(id);
        }
    }
}
