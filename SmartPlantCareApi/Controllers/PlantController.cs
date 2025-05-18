using Microsoft.AspNetCore.Mvc;
using SmartPlantCareApi.Models;
using SmartPlantCareApi.Services;

namespace SmartPlantCareApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PlantController : ControllerBase
    {
        private readonly PlantService _plantService;

        public PlantController(PlantService plantService)
        {
            _plantService = plantService;
        }

        [HttpGet]
        public async Task<List<Plant>> Get() =>
            await _plantService.GetAsync();

        [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<Plant>> Get(string id)
        {
            var plant = await _plantService.GetAsync(id);

            if (plant is null)
                return NotFound();

            return plant;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Ping successful");
        }


        [HttpPost]
        public async Task<IActionResult> Post(Plant newPlant)
        {
            await _plantService.CreateAsync(newPlant);
            return CreatedAtAction(nameof(Get), new { id = newPlant.Id }, newPlant);
        }

        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, Plant updatedPlant)
        {
            var plant = await _plantService.GetAsync(id);
            if (plant is null)
                return NotFound();

            updatedPlant.Id = id;
            await _plantService.UpdateAsync(id, updatedPlant);
            return NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var plant = await _plantService.GetAsync(id);
            if (plant is null)
                return NotFound();

            await _plantService.RemoveAsync(id);
            return NoContent();
        }
    }
}
