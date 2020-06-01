using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Movies.Domain;
using EFCoreSample.Movies.Requests;
using EFCoreSample.Movies.Responses;
using EFCoreSample.Movies.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreSample.Movies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _services;

        public MovieController(IMovieService services)
        {
            _services = services;
        }
        
        [Authorize(Policy = "TeacherOnly"), HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAll()
        {
            var result = await _services.GetAll();
            return Ok(result.Select(ToMovieResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetById(Guid id)
        {
            var result = await _services.GetById(id);
            if (result == null) return NotFound();
            return Ok(ToMovieResponse(result));
        }

        [HttpPost("{rentalId}")]
        public async Task<ActionResult<Movie>> Post(Guid rentalId, [FromBody] MovieRequest request)
        {
            var movie = ToMovieValue(request);
            var result = await _services.Create(rentalId, movie);
            if (result == null) return NotFound("Movie does not exist");
            return Ok(ToMovieResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Movie>> Put(Guid id, [FromBody] MovieRequest request)
        {
            var movie = ToMovieValue(request);
            var response = await _services.Update(id, movie);

            if (request == null) return NotFound();
            return Ok(ToMovieResponse(response));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _services.Delete(id);
            return success ? Ok() : NotFound() as IActionResult;
        }

        private MovieValue ToMovieValue(MovieRequest request)
        {
            return new MovieValue(request.Title, request.DirectorName, request.ReleaseDate);
        }

        private MovieResponse ToMovieResponse(Movie movie)
        {
            return new MovieResponse(movie.Id, movie.Value.Title, movie.Value.DirectorName, movie.Value.ReleaseDate);
        }

    }
}