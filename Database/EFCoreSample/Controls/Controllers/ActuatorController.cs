using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Controls.Domain;
using EFCoreSample.Controls.Requests;
using EFCoreSample.Controls.Responses;
using EFCoreSample.Controls.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreSample.Controls.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ActuatorController : ControllerBase
    {
        private readonly IActuatorService _service;
        private readonly IControlsConverter _converter;

        public ActuatorController(IActuatorService service, IControlsConverter converter)
        {
            _service = service;
            _converter = converter;
        }

        [Authorize(Policy = "AnalystOnly"), HttpGet]
        public async Task<ActionResult<IEnumerable<ActuatorResponse>>> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result.Select(_converter.ToActuatorResponse));
        }
        
        [HttpGet("active")]
        public async Task<ActionResult<IEnumerable<ActuatorResponse>>> GetAllActive()
        {
            var result = await _service.GetAllActive();
            return Ok(result.Select(_converter.ToActuatorResponse));
        }
        
        [HttpGet("latest/{num}")]
        public async Task<ActionResult<IEnumerable<ActuatorResponse>>> GetLatest(int num)
        {
            var result = await _service.GetAllLatest(num);
            return Ok(result.Select(_converter.ToActuatorResponse));
        }

        [HttpGet("range/{start}/{end}")]
        public async Task<ActionResult<IEnumerable<ActuatorResponse>>> GetAllRange(DateTime start, DateTime end)
        {
            var result = await _service.GetAllRange(start, end);
            return Ok(result.Select(_converter.ToActuatorResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ActuatorResponse>> GetById(Guid id)
        {
            var result = await _service.GetById(id);
            if (result == null) return NotFound("No such actuator can be found");
            return Ok(_converter.ToActuatorResponse(result));
        }
        
        [HttpGet("{id}/active")]
        public async Task<ActionResult<ActuatorResponse>> GetActiveById(Guid id)
        {
            var result = await _service.GetActiveById(id);
            if (result == null) return NotFound("No such actuator can be found");
            return Ok(_converter.ToActuatorResponse(result));
        }
        
        [HttpGet("{id}/latest/{num}")]
        public async Task<ActionResult<ActuatorResponse>> GetLatestById(Guid id, int num)
        {
            var result = await _service.GetLatestById(id, num);
            if (result == null) return NotFound();
            return Ok(_converter.ToActuatorResponse(result));
        }

        [HttpGet("{id}/range/{start}/{end}")]
        public async Task<ActionResult<ActuatorResponse>> GetRangeById(Guid id, DateTime start, DateTime end)
        {
            var result = await _service.GetRangeById(id, start, end);
            if (result == null) return NotFound();
            return Ok(_converter.ToActuatorResponse(result));
        }

        [Authorize(Policy = "OperatorOnly"), HttpPost]
        public async Task<ActionResult<ActuatorResponse>> Post([FromBody] ActuatorRequest request)
        {
            return Ok(_converter.ToActuatorResponse(await _service.Create(_converter.ToActuatorValue(request))));
        }


        [Authorize(Policy = "OperatorOnly"), HttpPut("{id}")]
        public async Task<ActionResult<ActuatorResponse>> Put(Guid id, [FromBody] ActuatorRequest request)
        {
            var result = await _service.Update(id, _converter.ToActuatorValue(request));
            if (result == null) return NotFound();
            return Ok(_converter.ToActuatorResponse(result));
        }

        [Authorize(Policy = "OperatorOnly"), HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.Delete(id);
            return success ? Ok() : NotFound() as IActionResult;
        }
    }
}