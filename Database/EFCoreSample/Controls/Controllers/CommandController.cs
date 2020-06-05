using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;
using EFCoreSample.Controls.Requests;
using EFCoreSample.Controls.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreSample.Controls.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CommandController : ControllerBase
    {
        private readonly ICommandService _services;
        private readonly IControlsConverter _converter;

        public CommandController(ICommandService services, IControlsConverter converter)
        {
            _services = services;
            _converter = converter;
        }
        
        //[Authorize(Policy = "OperatorOnly"), HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Command>>> GetAll()
        {
            var result = await _services.GetAll();
            return Ok(result.Select(_converter.ToCommandResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Command>> GetById(Guid id)
        {
            var result = await _services.GetById(id);
            if (result == null) return NotFound("Command does not exist");
            return Ok(_converter.ToCommandResponse(result));
        }

        [HttpPost("{actuatorId}")]
        public async Task<ActionResult<Command>> Post(Guid actuatorId, [FromBody] CommandRequest request)
        {
            var command = _converter.ToCommandValue(request);
            var result = await _services.Create(actuatorId, command);
            if (result == null) return NotFound("Command was not added");
            return Ok(_converter.ToCommandResponse(result));
        }

        [HttpPut("{actuatorId}/{id}")]
        public async Task<ActionResult<Command>> Put(Guid id, Guid actuatorId, [FromBody] CommandRequest request)
        {
            var command = _converter.ToCommandValue(request);
            var response = await _services.Update(id, actuatorId, command);

            if (request == null) return NotFound();
            return Ok(_converter.ToCommandResponse(response));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _services.Delete(id);
            return success ? Ok() : NotFound() as IActionResult;
        }
    }
}