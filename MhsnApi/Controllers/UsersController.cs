using MhsnApi.Classes;
using MhsnApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MhsnApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserContext _context;
        public UsersController(UserContext context)
        {
            _context = context;
            if (_context.Users.Count() == 0)
            {
                _context.Users.Add(new UserModel { UserName = "mhsn" });
                _context.SaveChanges();
            }
        }

        // GET : /api/v1/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserModel>>> GetAllUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET : /api/v1/users/{id}
        [HttpGet]
        [Route("{id}")]
        public async Task<UserModel> GetUser(int id)
        {
           var user = await _context.Users.FindAsync(id);
            if(user != null)
            {
                return user;
            }
            return new UserModel { Id = 0 , UserName="Not Found :(" };
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<UserModel>> DeleteUser(int id)
        {
            _context.Users.RemoveRange(_context.Users.Where(x => x.Id == id));
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> CreateUser(UserModel userModel)
        {
            _context.Users.Add(userModel);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<UserModel>> UpdateUser(UserModel userModel)
        {
            if(userModel.Id == null || userModel.Id == 0)
            {
                return NotFound();
            }
            _context.Users.Update(userModel);
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost]
        [Route("login")]
        public ActionResult<UserModel> Login(string username, string password)
        {
            
            var user = (from u in _context.Users
                       where u.UserName.Equals(username) && u.password.Equals(password)
                       select u).FirstOrDefault();
            if (user != null)
            {
                var rnd = new RandGenerator();
                string randomString = rnd.RandomString(32, true);
                return Ok(randomString);
            }            
            return StatusCode(401);
        }
    }
}
