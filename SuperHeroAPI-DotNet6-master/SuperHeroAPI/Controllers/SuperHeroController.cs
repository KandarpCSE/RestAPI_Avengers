
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SuperHeroAPI.Middleware;
using SuperHeroAPI.Repositories;
using TinifyAPI;

namespace SuperHeroAPI.Controllers.V1
{
    [ApiController]

    [Route("api/v1/[controller]")]
    public class SuperHeroController : ControllerBase
    {
        private readonly ISuperHeroRepo superHeroRepo;
        private readonly ILoggerFactory loggerFactory;
        private readonly ILogger<Request_Response_Middleware> _logger;

        public SuperHeroController(ISuperHeroRepo superHeroRepo, ILoggerFactory loggerFactory)
        {
            this.superHeroRepo = superHeroRepo;
            _logger = loggerFactory
                     .CreateLogger<Request_Response_Middleware>();
        }


        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetHeros()
        {
            var OurHeros = await superHeroRepo.GetAllHero();
            if( OurHeros.Count == 0 )
            {
                _logger.LogInformation("Heros Count was 0");
                return Ok("We need to Get our Heros Back");
            }
            return Ok(OurHeros);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetHero(int id)
        {
            var hero = await superHeroRepo.GetHero(id);
            if (hero == null)
            {
                _logger.LogInformation($"hero of {id} is not found");
                return BadRequest("Ohhh Hero was not Found.");
            }
            return Ok(hero);
        }


        [HttpGet("HeroImage/{id}")]
        public async Task<ActionResult<SuperHero>> GetImage(int id)
        {
            var hero = await superHeroRepo.GetHero(id);
            if (hero == null)
            {

                _logger.LogInformation($"hero of {id} is not found");

                return BadRequest("Ohhh Hero was not Found.");
            }
            try
            {
                Byte[] b = System.IO.File.ReadAllBytes(hero.ImageURl);
                return File(b, "image/jpeg");
            }
            catch (Exception)
            {

                _logger.LogInformation($"hero image of {id} is not found");
                return BadRequest("No Image");
            }
        }


        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            try
            {
                hero.ImageURl = string.Empty;
                var  newHero = await superHeroRepo.AddNewHero(hero);
                return Ok(newHero);
            }
            catch (Exception ex)
            {

                _logger.LogInformation($"{hero} is not valid");
                return BadRequest("Something Went Wrong" + ex);
            }
        }


        [HttpPost("Uploade Photo/{id}")]
        public async Task<IActionResult> AddHeroPhoto(IFormFile file,int id)
        {
            if (file.Length > 0)
            {
                try
                {

                    await superHeroRepo.UploadHeroImage(file, id);
                }
                catch(Exception ex)
                {

                    _logger.LogInformation($"hero of {id} and {file.FileName} is not valid");
                    return BadRequest("May be Error Come :" + ex.Message);
                }
                return Ok("Uploaded Successfully" + " " + file.FileName);
            }
            else
            {
                return BadRequest("Please Upload one image");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero updateHero,int id)
        {
            var updatedHero = await superHeroRepo.UpdateOurHero(updateHero,id);
            if(updatedHero == null)
            {
                return NotFound("Following Hero was not found"+id);
            }
            return Ok(superHeroRepo.GetHero(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {
            var response = await superHeroRepo.DeleteHero(id);
            if (response == null)
            {

                _logger.LogInformation($"hero of {id} is not found");
                return BadRequest("Hero not found.");

            }
            return Ok("Successfully deleted" + id);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateHero(int id,[FromBody] JsonPatchDocument<SuperHero> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Null values");

            var hero = await superHeroRepo.GetHero(id);
            if (hero == null)
            {
                return NotFound("Hero Not Found");
            }
            patchDoc.ApplyTo(hero, ModelState);
            TryValidateModel(hero);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var updatedHero = superHeroRepo.UpdateOurHero(hero, id);
            return Ok(updatedHero);
        }

    }
}


namespace SuperHeroAPI.Controllers.V2
{
    [ApiController]

    [Route("api/v2/[controller]")]
    public class SuperHeroController : ControllerBase
    {
        private readonly ISuperHeroRepo superHeroRepo;

        public SuperHeroController(ISuperHeroRepo superHeroRepo)
        {
            this.superHeroRepo = superHeroRepo;
        }


        [HttpGet]
        public async Task<ActionResult<List<SuperHero>>> GetHeros()
        {
            var OurHeros = await superHeroRepo.GetAllHero();
            if (OurHeros.Count == 0)
            {
                return Ok("We need to Get our Heros Back");
            }
            return Ok(OurHeros);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<SuperHero>> GetHero(int id)
        {
            var hero = await superHeroRepo.GetHero(id);
            if (hero == null)
            {
                return BadRequest("Ohhh Hero was not Found.");
            }
            return Ok(hero);
        }


        [HttpGet("HeroImage/{id}")]
        public async Task<ActionResult<SuperHero>> GetImage(int id)
        {
            var hero = await superHeroRepo.GetHero(id);
            if (hero == null)
                return BadRequest("Ohhh Hero was not Found.");
            try
            {
                Byte[] b = System.IO.File.ReadAllBytes(hero.ImageURl);
                return File(b, "image/jpeg");
            }
            catch (Exception)
            {
                return BadRequest("No Image");
            }
        }


        [HttpPost]
        public async Task<ActionResult<List<SuperHero>>> AddHero(SuperHero hero)
        {
            try
            {
                hero.ImageURl = string.Empty;
                var newHero = await superHeroRepo.AddNewHero(hero);
                return Ok(newHero);
            }
            catch (Exception ex)
            {
                return BadRequest("Something Went Wrong" + ex);
            }
        }


        [HttpPost("Uploade Photo/{id}")]
        public async Task<IActionResult> AddHeroPhoto(IFormFile file, int id)
        {
            if (file.Length > 0)
            {
                try
                {
                    await superHeroRepo.UploadHeroImage(file, id);
                }
                catch (Exception ex)
                {
                    return BadRequest("May be Error Come :" + ex.Message);
                }
                return Ok("Uploaded Successfully" + " " + file.FileName);
            }
            else
            {
                return BadRequest("Please Upload one image");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<List<SuperHero>>> UpdateHero(SuperHero updateHero, int id)
        {
            var updatedHero = await superHeroRepo.UpdateOurHero(updateHero, id);
            if (updatedHero == null)
            {
                return NotFound("Following Hero was not found" + id);
            }
            return Ok(superHeroRepo.GetHero(id));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<List<SuperHero>>> Delete(int id)
        {
            var response = await superHeroRepo.DeleteHero(id);
            if (response == null)
                return BadRequest("Hero not found.");
            return Ok("Successfully deleted" + id);
        }


        [HttpPatch("{id}")]
        public async Task<IActionResult> PartiallyUpdateHero(int id, [FromBody] JsonPatchDocument<SuperHero> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest("Null values");

            var hero = await superHeroRepo.GetHero(id);
            if (hero == null)
            {
                return NotFound("Hero Not Found");
            }
            patchDoc.ApplyTo(hero, ModelState);
            TryValidateModel(hero);
            if (!ModelState.IsValid) { return BadRequest(ModelState); }
            var updatedHero = superHeroRepo.UpdateOurHero(hero, id);
            return Ok(updatedHero);
        }

    }
}
