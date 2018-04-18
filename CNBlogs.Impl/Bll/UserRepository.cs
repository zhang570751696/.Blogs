using CNBlogs.Frame;
using CNBlogs.Interface;
using CNBlogs.Model;
using CNBlogs.PresentModel;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CNBlogs.Impl
{
    public class UserRepository : IUserRepository
    {
        private IRepository<User> _userRepository;

        public UserRepository(IRepository<User> userRepository)
        {
            this._userRepository = userRepository;
        }

        public bool AddUser(UserInfo user)
        {
            var existuserinfo = this._userRepository.IsExist(t => t.LoginName == user.LoginName);
            if (existuserinfo)
            {
                return false;
            }
            var userinfo = new User
            {
                LoginName = user.LoginName,
                Name = user.UserName,
                Password = "123456"
            };
            return this._userRepository.Save(userinfo);
        }

        public bool DeleteUser(int userId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<UserInfo> GetAllUsers()
        {
            return this._userRepository.LoadAll(t => true)
                .Select(u => new UserInfo
                {
                    LoginName = u.LoginName,
                    UserId = u.UserId,
                    UserName = u.Name
                }).ToList();

        }

        public UserInfo GetUserByUserId(int userId)
        {
            UserInfo userInfo = null;
            var user = this._userRepository.Get(t => t.UserId == userId);
            if (user != null)
            {
                userInfo = new UserInfo
                {
                    LoginName = user.LoginName,
                    UserId = user.UserId,
                    UserName = user.Name
                };
            }
            return userInfo;
        }

        public bool UpdateUser(int userId, UserInfo user)
        {
            var userinfo = this._userRepository.Get(t => t.UserId == userId);
            if(userinfo!= null)
            {
                userinfo.Name = user.UserName;
                return this._userRepository.Update(userinfo);
            }
            return false;
        }
    }
}
