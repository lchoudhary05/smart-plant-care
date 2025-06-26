using Microsoft.AspNetCore.Mvc;
using GreenMonitor.Interfaces;
using GreenMonitor.Models;
using GreenMonitor.Services;

namespace GreenMonitor.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<ActionResult> Login(LoginUser loginUser)
        {
            var user = await _userService.Login(loginUser);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }
        [HttpPost]
        public async Task<IActionResult> Register(Users user)
        {
            user.CreatedAt = DateTime.Now;
            var result = await _userService.CheckUser(user);
            if (result == null)
            {
                await _userService.Register(user);
                return Created();
            }
            return Conflict(new { message = "email or username already exists" });


        }
    }
}