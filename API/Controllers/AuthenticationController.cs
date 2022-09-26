using MeetingSchedule.Models.DTOs;
using MeetingSchedule.Models.Entities;
using MeetingSchedule.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MeetingSchedule.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IUserService _userService;

        public AuthenticationController(IAuthenticationService authenticationService, IUserService userService)
        {
            _authenticationService = authenticationService;
            _userService = userService;
        }

        [HttpPost]
        [Route("authenticate")]
        public ActionResult<dynamic> Authenticate([FromBody] UserCredentials credentials)
        {
            var user = _userService.Get(credentials.UserName, credentials.Password);
            if (user == null)
                return Unauthorized(new { message = "Usuário ou senha inválidos" });

            var token = _authenticationService.GenerateToken(user);

            return new
            {
                token
            };
        }
    }
}
