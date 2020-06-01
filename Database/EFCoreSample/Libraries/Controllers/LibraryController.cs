using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Libraries.Domain;
using EFCoreSample.Libraries.Requests;
using EFCoreSample.Libraries.Responses;
using EFCoreSample.Libraries.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreSample.Libraries.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LibraryController : ControllerBase
    {
        private readonly ILibraryService _service;

        public LibraryController(ILibraryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Library>>> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result.Select(ToLibraryResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Library>> GetById(Guid id)
        {
            var result = await _service.GetById(id);
            if (result == null) return NotFound();
            return Ok(ToLibraryResponse(result));
        }

        [HttpPost]
        public async Task<ActionResult<Library>> Post([FromBody] LibraryRequest request)
        {
            return Ok(await _service.Create(ToLibraryValue(request)));
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Library>> Put(Guid id, [FromBody] LibraryRequest request)
        {
            var result = await _service.Update(id, ToLibraryValue(request));
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.Delete(id);
            return success ? Ok() : NotFound() as IActionResult;
        }
        
        private LibraryValue ToLibraryValue(LibraryRequest request)
        {
            return new LibraryValue(request.Name, request.Address);
        }

        private LibraryResponse ToLibraryResponse(Library library)
        {
            return new LibraryResponse(library.Id, library.Value.Name, library.Value.Address, library.Books);
        }
    }
}