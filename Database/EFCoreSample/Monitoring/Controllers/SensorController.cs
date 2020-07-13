using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;
using EFCoreSample.Monitoring.Requests;
using EFCoreSample.Monitoring.Responses;
using EFCoreSample.Monitoring.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreSample.Monitoring.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SensorController : ControllerBase
    {
        private readonly ISensorService _service;
        private readonly IMonitoringConverter _converter;

        public SensorController(ISensorService service, IMonitoringConverter converter)
        {
            _service = service;
            _converter = converter;
        }

        [Authorize(Policy = "AnalystOnly"), HttpGet]
        public async Task<ActionResult<IEnumerable<SensorResponse>>> GetAll()
        {
            var result = await _service.GetAll();
            return Ok(result.Select(_converter.ToSensorResponse));
        }

        [HttpGet("latest/{num}")]
        public async Task<ActionResult<IEnumerable<SensorResponse>>> GetLatest(int num)
        {
            var result = await _service.GetAllLatest(num);
            return Ok(result.Select(_converter.ToSensorResponse));
        }

        [HttpGet("range/{start}/{end}")]
        public async Task<ActionResult<IEnumerable<SensorResponse>>> GetAllRange(DateTime start, DateTime end)
        {
            var result = await _service.GetAllRange(start, end);
            return Ok(result.Select(_converter.ToSensorResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SensorResponse>> GetById(Guid id)
        {
            var result = await _service.GetById(id);
            if (result == null) return NotFound();
            return Ok(_converter.ToSensorResponse(result));
        }

        [HttpGet("{id}/latest/{num}")]
        public async Task<ActionResult<SensorResponse>> GetLatestById(Guid id, int num)
        {
            var result = await _service.GetLatestById(id, num);
            if (result == null) return NotFound();
            return Ok(_converter.ToSensorResponse(result));
        }

        [HttpGet("{id}/range/{start}/{end}")]
        public async Task<ActionResult<SensorResponse>> GetRangeById(Guid id, DateTime start, DateTime end)
        {
            var result = await _service.GetRangeById(id, start, end);
            if (result == null) return NotFound();
            return Ok(_converter.ToSensorResponse(result));
        }

        [Authorize(Policy = "OperatorOnly"), HttpPost]
        public async Task<ActionResult<SensorResponse>> Post([FromBody] SensorRequest request)
        {
            return Ok(_converter.ToSensorResponse(await _service.Create(_converter.ToSensorValue(request))));
        }


        [Authorize(Policy = "OperatorOnly"), HttpPut("{id}")]
        public async Task<ActionResult<SensorResponse>> Put(Guid id, [FromBody] SensorRequest request)
        {
            var result = await _service.Update(id, _converter.ToSensorValue(request));
            if (result == null) return NotFound();
            return Ok(_converter.ToSensorResponse(result));
        }

        [Authorize(Policy = "OperatorOnly"), HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _service.Delete(id);
            return success ? Ok() : NotFound() as IActionResult;
        }
    }
}