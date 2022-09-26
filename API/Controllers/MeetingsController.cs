using Castle.Core.Internal;
using MeetingSchedule.Models.DTOs.Meetings;
using MeetingSchedule.Models.ViewModels;
using MeetingSchedule.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;

namespace MeetingSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingsController : ControllerBase
    {
        private readonly IMeetingService _meetingService;
        private readonly IRoomService _roomService;

        public MeetingsController(IMeetingService meetingService, IRoomService roomService)
        {
            _meetingService = meetingService;
            _roomService = roomService;
        }

        [HttpGet("{id:int}")]
        public ActionResult<Meeting> Get(int id)
        {
            var rawMeeting = _meetingService.Get(id);
            if (rawMeeting is null)
                return NotFound();

            return rawMeeting.ToViewModel();
        }

        [HttpGet("calendar-view/{date:datetime}")]
        public IEnumerable<Meeting> Get(DateTime date)
        {
            var rawMeetings = _meetingService.GetByMonth(date); ;
            var meetings = new List<Meeting>();
            foreach (var meeting in rawMeetings)
            {
                meetings.Add(meeting.ToViewModel());
            }

            return meetings;
        }

        [HttpPatch("{id:int}")]
        public ActionResult<dynamic> Patch(int id, [FromBody] PatchMeeting meetingData)
        {
            Models.ViewModels.Conflicts conflicts = null;
            if (meetingData.RoomId != 0)
                conflicts = _roomService.GetConflictsByRoomIdAndTime(id, meetingData.RoomId, meetingData.Start, meetingData.End);
            else
            {
                var room = _roomService.GetFirstAvailableRoom(id, meetingData.Start, meetingData.End);
                if (room is null)
                    conflicts = _roomService.GetConflictsByTime(id, meetingData.Start, meetingData.End);
                else
                    meetingData.RoomId = room.Id;
            }

            if (conflicts is null || conflicts.MeetingConflicts.IsNullOrEmpty())
            {
                return _meetingService.Patch(id, meetingData).ToViewModel();
            }

            return Conflict(new { conflicts.MeetingConflicts });
        }
        [HttpDelete("{id:int}")]
        public void Delete(int id)
        {
            _meetingService.Delete(id);
        }

        [HttpPost]
        public ActionResult<dynamic> Post([FromBody] NewMeeting newMeeting)
        {
            Models.ViewModels.Conflicts conflicts = null;
            if (newMeeting.RoomId != 0)
                conflicts = _roomService.GetConflictsByRoomIdAndTime(newMeeting.RoomId, newMeeting.Start, newMeeting.End);
            else
            {
                var room = _roomService.GetFirstAvailableRoom(newMeeting.Start, newMeeting.End);
                if (room is null)
                    conflicts = _roomService.GetConflictsByTime(newMeeting.Start, newMeeting.End);
                else
                    newMeeting.RoomId = room.Id;
            }

            if (conflicts is null || conflicts.MeetingConflicts.IsNullOrEmpty())
                return new { Meeting = _meetingService.Add(newMeeting).ToViewModel() };

            return Conflict(new { conflicts.MeetingConflicts });
        }
    }
}