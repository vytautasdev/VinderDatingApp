using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly DataContext dataContext;
        private readonly ITokenService tokenService;
        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            this.tokenService = tokenService;
            this.dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDTO>> Register(DTOs.RegisterDTO registerDTO)
        {

            if (await UserExists(registerDTO.Username)) return BadRequest("This username is already taken. Please try again.");

            using var hmac = new HMACSHA512();

            var user = new AppUser
            {
                UserName = registerDTO.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            dataContext.Users.Add(user);
            await dataContext.SaveChangesAsync();

            return new UserDTO
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(DTOs.LoginDTO loginDTO)
        {
            var user = await dataContext.Users.SingleOrDefaultAsync(item => item.UserName == loginDTO.Username);

            if (user == null) return Unauthorized("Invalid username.");

            using var hmac = new HMACSHA512(user.PasswordSalt);

            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid password.");
            }


            return new UserDTO
            {
                Username = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }

        // helper method to check if given username already exists in the db
        private async Task<bool> UserExists(string username)
        {
            return await dataContext.Users.AnyAsync(item => item.UserName == username.ToLower());
        }
    }
}