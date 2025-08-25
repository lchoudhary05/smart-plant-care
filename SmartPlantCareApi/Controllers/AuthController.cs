using Microsoft.AspNetCore.Mvc;
using SmartPlantCareApi.Models;
using SmartPlantCareApi.Services;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace SmartPlantCareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;

        public AuthController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userService.CreateAsync(request);
                
                var token = _userService.GenerateJwtToken(user);
                var refreshToken = _userService.GenerateRefreshToken();

                var response = new AuthResponse
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    User = new UserProfile
                    {
                        Id = user.Id!,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        SubscriptionTier = user.SubscriptionTier,
                        MaxPlants = user.MaxPlants,
                        CreatedAt = user.CreatedAt
                    }
                };

                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred during registration" });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var isValidPassword = await _userService.ValidatePasswordAsync(request.Email, request.Password);
                if (!isValidPassword)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                var user = await _userService.GetByEmailAsync(request.Email);
                if (user == null)
                {
                    return Unauthorized(new { message = "Invalid email or password" });
                }

                // Update last login
                await _userService.UpdateLastLoginAsync(user.Id!);

                var token = _userService.GenerateJwtToken(user);
                var refreshToken = _userService.GenerateRefreshToken();

                var response = new AuthResponse
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    User = new UserProfile
                    {
                        Id = user.Id!,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        SubscriptionTier = user.SubscriptionTier,
                        MaxPlants = user.MaxPlants,
                        CreatedAt = user.CreatedAt
                    }
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred during login" });
            }
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var user = await _userService.GetByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var profile = new UserProfile
                {
                    Id = user.Id!,
                    Email = user.Email,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    SubscriptionTier = user.SubscriptionTier,
                    MaxPlants = user.MaxPlants,
                    CreatedAt = user.CreatedAt
                };

                return Ok(profile);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fetching profile" });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            try
            {
                // In a production app, you'd validate the refresh token against a database
                // For now, we'll just generate a new token if the user is authenticated
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var user = await _userService.GetByIdAsync(userId);
                if (user == null)
                {
                    return NotFound(new { message = "User not found" });
                }

                var token = _userService.GenerateJwtToken(user);
                var refreshToken = _userService.GenerateRefreshToken();

                var response = new AuthResponse
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(60),
                    User = new UserProfile
                    {
                        Id = user.Id!,
                        Email = user.Email,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        SubscriptionTier = user.SubscriptionTier,
                        MaxPlants = user.MaxPlants,
                        CreatedAt = user.CreatedAt
                    }
                };

                return Ok(response);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while refreshing token" });
            }
        }
    }

    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; } = string.Empty;
    }
} 