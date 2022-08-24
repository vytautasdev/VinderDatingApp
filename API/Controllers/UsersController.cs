using API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly DataContext dataContext;
        public UsersController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Entities.AppUser>>> GetUsers()
        {
            return await dataContext.Users.ToListAsync();
        }

        // api/users/3
        [HttpGet("{id}")]
        public async Task<ActionResult<Entities.AppUser>> GetUser(int id)
        {
            return await dataContext.Users.FindAsync(id);
        }
    }
}