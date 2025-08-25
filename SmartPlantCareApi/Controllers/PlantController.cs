using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SmartPlantCareApi.Models;
using SmartPlantCareApi.Services;
using System.Security.Claims;

namespace SmartPlantCareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class PlantController : ControllerBase
    {
        private readonly PlantService _plantService;

        public PlantController(PlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Plant>>> GetUserPlants()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var plants = await _plantService.GetUserPlantsAsync(userId);
                return Ok(plants);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fetching plants" });
            }
        }

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Plant>> GetUserPlant(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var plant = await _plantService.GetUserPlantAsync(userId, id);
                if (plant == null)
                    return NotFound(new { message = "Plant not found" });

                return Ok(plant);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fetching plant" });
            }
        }

        [HttpGet("needing-water")]
        public async Task<ActionResult<List<Plant>>> GetPlantsNeedingWater()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var plants = await _plantService.GetPlantsNeedingWaterAsync(userId);
                return Ok(plants);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fetching plants needing water" });
            }
        }

        [HttpGet("needing-fertilizer")]
        public async Task<ActionResult<List<Plant>>> GetPlantsNeedingFertilizer()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                var plants = await _plantService.GetPlantsNeedingFertilizerAsync(userId);
                return Ok(plants);
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fetching plants needing fertilizer" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlant([FromBody] PlantCreateRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var plant = await _plantService.CreateAsync(userId, request);
                return CreatedAtAction(nameof(GetUserPlant), new { id = plant.Id }, plant);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while creating plant" });
            }
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> UpdatePlant(string id, [FromBody] PlantUpdateRequest request)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await _plantService.UpdateAsync(userId, id, request);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while updating plant" });
            }
        }

        [HttpPost("{id:length(24)}/water")]
        public async Task<IActionResult> WaterPlant(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                await _plantService.WaterPlantAsync(userId, id);
                return Ok(new { message = "Plant watered successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while watering plant" });
            }
        }

        [HttpPost("{id:length(24)}/fertilize")]
        public async Task<IActionResult> FertilizePlant(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                await _plantService.FertilizePlantAsync(userId, id);
                return Ok(new { message = "Plant fertilized successfully" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while fertilizing plant" });
            }
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> DeletePlant(string id)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Invalid token" });
                }

                await _plantService.RemoveAsync(userId, id);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "An error occurred while deleting plant" });
            }
        }

        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult Ping()
        {
            return Ok("Ping successful");
        }
    }
}
