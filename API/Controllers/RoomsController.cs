using MeetingSchedule.Models.DTOs.Room;
using MeetingSchedule.Models.Entities;
using MeetingSchedule.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MeetingSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        public IEnumerable<Models.ViewModels.Room> Get()
        {
            var rooms = new List<Models.ViewModels.Room>();
            var rawRooms = _roomService.Get();
            foreach (var rawRoom in rawRooms)
            {
                rooms.Add(rawRoom.ToViewModel());
            }

            return rooms;
        }

        [HttpGet("{id}")]
        public ActionResult<dynamic> Get(int id)
        {
            Room room = _roomService.Get(id);

            if (room is null)
                return NotFound(new { message = "O recurso solicitado não foi encontrado" });

            return room.ToViewModel();
        }

        [HttpPost]
        public ActionResult<dynamic> Post([FromBody] NewRoom newRoom)
        {
            return new { Room = _roomService.Add(newRoom).ToViewModel() };
        }
    }
}
