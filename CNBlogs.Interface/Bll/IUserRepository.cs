using CNBlogs.PresentModel;
using System.Collections.Generic;

namespace CNBlogs.Interface
{
    public interface IUserRepository
    {
        IEnumerable<UserInfo> GetAllUsers();
        UserInfo GetUserByUserId(int userId);
        bool AddUser(UserInfo user);
        bool UpdateUser(int userId, UserInfo user);
        bool DeleteUser(int userId);
    }
}
