using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Movies.Domain;
using EFCoreSample.Movies.Requests;
using EFCoreSample.Movies.Responses;
using EFCoreSample.Movies.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreSample.Movies.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RentalController : ControllerBase
    {
        private readonly IRentalService _service;

        public RentalController(IRentalService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalResponse>>> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result.Select(ToRentalResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalResponse>> GetById(Guid id)
        {
            var result = await _service.GetById(id);
            if (result == null) return NotFound();
            return Ok(ToRentalResponse(result));
        }

        [HttpPost]
        public async Task<ActionResult<RentalResponse>> Post([FromBody] RentalRequest request)
        {
            return Ok(ToRentalResponse(await _service.Create(ToRentalValue(request))));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<RentalResponse>> Put(Guid id, [FromBody] RentalRequest request)
        {
            var result = await _service.Update(id, ToRentalValue(request));
            if (result == null) return NotFound();
            return Ok(ToRentalResponse(result));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.Delete(id);
            return success ? Ok() : NotFound() as IActionResult;
        }
        
        private MovieRentalValue ToRentalValue(RentalRequest request)
        {
            return new MovieRentalValue(request.Name, request.Address);
        }

        private RentalResponse ToRentalResponse(MovieRental rental)
        {
            return new RentalResponse(rental.Id, rental.Value.Name, rental.Value.Address, rental.Movies);
        }
    }
}