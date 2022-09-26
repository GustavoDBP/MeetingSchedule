using MeetingSchedule.DataAccess;
using MeetingSchedule.Models.Entities;

namespace MeetingSchedule.Services
{
    public interface IUserService
    {
        User Get(string username);
        User Get(string username, string password);
    }

    public class UserService : IUserService
    {
        private readonly IUserDataAccess _userDataAccess;

        public UserService(IUserDataAccess userDataAccess)
        {
            _userDataAccess = userDataAccess;
        }

        public User Get(string username)
        {
            return _userDataAccess.Get(username);
        }

        public User Get(string username, string password)
        {
            return _userDataAccess.Get(username, password);
        }
    }
}
