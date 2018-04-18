using System.Collections.Generic;
using CNBlogs.Interface;
using CNBlogs.PresentModel;
using Microsoft.AspNetCore.Mvc;

namespace CNBlogs.Controllers
{
    /// <summary>
    /// Test Api
    /// </summary>
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        /// <summary>
        /// this is get request without params
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public IEnumerable<UserInfo> Get()
        {
            return this._userRepository.GetAllUsers();
        }


        /// <summary>
        /// this is get request with one params
        /// </summary>
        /// <param name="id">request value</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public UserInfo Get(int id)
        {
            return this._userRepository.GetUserByUserId(id);
        }

        /// <summary>
        /// this is a post request
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public bool Post([FromBody]UserInfo value)
        {
            if (value != null)
            {
                return this._userRepository.AddUser(value);
            }
            return false;
        }

        /// <summary>
        /// this is a put request
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="value">value</param>
        [HttpPut("{id}")]
        public bool Put(int id, [FromBody]UserInfo value)
        {
            if (value == null)
            {
                return false;
            }
            return this._userRepository.UpdateUser(id, value);
        }

        /// <summary>
        /// this is a delete request
        /// </summary>
        /// <param name="id">id</param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
