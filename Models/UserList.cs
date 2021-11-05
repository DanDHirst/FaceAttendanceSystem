using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceAttendance.Models
{
    public class UserList
    {
        public List<User> userList = new List<User>();

        public void addUser(User user)
        {
            userList.Add(user);
        }
    }
}
