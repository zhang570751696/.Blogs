using System.Collections.Generic;
using CNBlogs.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CNBlogs.Controllers
{
    /// <summary>
    /// Test Api
    /// </summary>
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IDateTimeService _dateTimeService;

        public ValuesController(IDateTimeService dateTimeService)
        {
            this._dateTimeService = dateTimeService;
        }

        /// <summary>
        /// this is get request without params
        /// </summary>
        /// <returns></returns>
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var time1 = this._dateTimeService.GetCurrentUtcTime();
            return new string[] { time1 };
        }


        /// <summary>
        /// this is get request with one params
        /// </summary>
        /// <param name="id">request value</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        /// <summary>
        /// this is a post request
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        /// <summary>
        /// this is a put request
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="value">value</param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
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
