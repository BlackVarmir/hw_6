using Azure.Data.Tables;
using hw_6.Models;
using hw_6.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace hw_6.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly MovieService _movieService;
        private readonly TableClient _tableClient;

        public MoviesController(MovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Movie>>> GetMovies()
        {
            var movies = await _movieService.GetMoviesAsync();
            return Ok(movies);
        }


        [HttpGet("{partitionKey}/{rowKey}")]
        public async Task<ActionResult<Movie>> GetMovie(string id, string rowKey)
        {
            var movie = await _movieService.GetMovieAsync(id, rowKey);
            if (movie == null)
            {
                return NotFound();
            }
            return Ok(movie);
        }

        [HttpPost]
        public async Task<ActionResult<Movie>> CreateMovie(Movie movie)
        {
            await _movieService.CreateMovieAsync(movie);
            return CreatedAtAction(nameof(GetMovie), new { movie.RowKey }, movie);
        }

        [HttpPut("{partitionKey}")]
        public async Task<ActionResult> UpdateMovie (string partitionKey, string rowKey, Movie movie)
        {
            if (partitionKey != movie.RowKey) { 
                BadRequest();
            }
            await _movieService.UpdateMovieAsync(movie);
            return NoContent();
        }

        [HttpDelete("{partitionKey}/{rowKey}")]
        public async Task<ActionResult> DeleteMovie(string partitionKey, string rowKey)
        {
            var movie = await _movieService.GetMovieAsync(partitionKey, rowKey);
            if (movie == null)
            {
                return NotFound();
            }

            await _movieService.DeleteMovieAsync(partitionKey, rowKey);
            return NoContent();
        }

        [HttpGet("year/{year}")]
        public async Task<ActionResult<List<Movie>>> GetMoviesByYear(int year)
        {
            var movies = await _movieService.GetMoviesByYearAsync(year);
            return Ok(movies);
        }

    }
}
