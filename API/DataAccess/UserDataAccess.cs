using MeetingSchedule.DBContext;
using MeetingSchedule.Models.Entities;
using System.Linq;

namespace MeetingSchedule.DataAccess
{
    public interface IUserDataAccess
    {
        User Get(string username);
        User Get(string username, string password);
    }

    public class UserDataAccess : IUserDataAccess
    {
        private readonly MeetingScheduleContext _context;

        public UserDataAccess(MeetingScheduleContext context)
        {
            _context = context;
        }

        public User Get(string username)
        {
            var user = _context.Users.First((user) => user.UserName == username);
            return user;
        }

        public User Get(string username, string password)
        {
            var user = _context.Users.Where((user) => user.UserName == username && user.Password == password).FirstOrDefault();
            return user;
        }
    }
}
