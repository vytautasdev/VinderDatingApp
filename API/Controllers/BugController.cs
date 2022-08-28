using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class BugController : BaseApiController
    {
        private readonly DataContext dataContext;
        public BugController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }

        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
            var item = dataContext.Users.Find(-1);

            if (item == null) return NotFound();

            return Ok(item);
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var item = dataContext.Users.Find(-1);

            var itemToReturn = item.ToString();

            return itemToReturn;
        }

        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
            return BadRequest("Bad request");
        }
    }
}