using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MovieApp.Server.Services;
using System.Threading.Tasks;
using MovieApp.Shared.DTOs;


namespace MovieApp.Server.Controllers
{
    [Authorize]
    [Route("api/movies")]
    public class MoviesController : ControllerBase
    {
        private readonly ILogger<MoviesController> _logger;
        private readonly IMoviesDbService _dbService;

        public MoviesController(ILogger<MoviesController> logger, IMoviesDbService dbService)
        {
            _logger = logger;
            _dbService = dbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetMovies()
        {
            return Ok(await _dbService.GetMovies());
        }
        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovie([FromRoute] int id)
        {
            return Ok(await _dbService.GetMovie(id));
        }
        
        [HttpPost]
        public async Task<IActionResult> AddMovie([FromBody] MovieDto movieDto)
        {
            await _dbService.AddMovie(movieDto);

            using var sw = new StreamWriter("C:\\Users\\Mati_PC\\Desktop\\dane.txt");

            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovie([FromBody] MovieDto movieDto, [FromRoute] int id)
        {
            await _dbService.UpdateMovie(movieDto, id);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovie([FromRoute] int id)
        {
            var res = await _dbService.DeleteMovie(id);

            if (res)
            {
                return Ok();
            }

            return BadRequest("Nie można usunąć, ponieważ if filmu jest używane w innych tabelach");
        }
        
    }
}
