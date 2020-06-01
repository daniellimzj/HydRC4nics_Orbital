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
    public class BookController : ControllerBase
    {
        private readonly IBookService _services;

        public BookController(IBookService services)
        {
            _services = services;
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetAll()
        {
            var result = await _services.GetAll();
            return Ok(result.Select(ToBookResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetById(Guid id)
        {
            var result = await _services.GetById(id);
            if (result == null) return NotFound();
            return Ok(ToBookResponse(result));
        }

        [HttpPost("{libraryId}")]
        public async Task<ActionResult<Library>> Post(Guid libraryId, [FromBody] BookRequest request)
        {
            var book = ToBookValue(request);
            var result = await _services.Create(libraryId, book);
            if (result == null) return NotFound("Library does not exist");
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Library>> Put(Guid id, [FromBody] BookRequest request)
        {
            var book = ToBookValue(request);
            var response = await _services.Update(id, book);

            if (request == null) return NotFound();
            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _services.Delete(id);
            return success ? Ok() : NotFound() as IActionResult;
        }

        private BookValue ToBookValue(BookRequest request)
        {
            return new BookValue(request.Title, request.AuthorName, request.PublishDate);
        }

        private BookResponse ToBookResponse(Book book)
        {
            return new BookResponse(book.Id, book.Value.Title, book.Value.AuthorName, book.Value.PublishDate);
        }

    }
}