using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading.Tasks;
using EFCoreSample.Monitoring.Domain;
using EFCoreSample.Monitoring.Requests;
using EFCoreSample.Monitoring.Services;
using Microsoft.AspNetCore.Mvc;

namespace EFCoreSample.Monitoring.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReadingController : ControllerBase
    {
        private readonly IReadingService _services;
        private readonly IMonitoringConverter _converter;

        public ReadingController(IReadingService services, IMonitoringConverter converter)
        {
            _services = services;
            _converter = converter;
        }
        
        // [Authorize(Policy = "NUSOnly"), HttpGet]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reading>>> GetAll()
        {
            var result = await _services.GetAll();
            return Ok(result.Select(_converter.ToReadingResponse));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reading>> GetById(Guid id)
        {
            var result = await _services.GetById(id);
            if (result == null) return NotFound();
            return Ok(_converter.ToReadingResponse(result));
        }
        
        // Start the serial port readings by giving the COM port and the sensor sequence in the POST body
        [HttpPost("serial/start/{com}")]
        public ActionResult<SerialPort> SerialStart(string com, [FromBody] List<Guid> sequence)
        {
            // Create the serial port with basic settings
            var port = _services.SerialStart(com, sequence);
            return Ok(port);
        }
        
        // Stop the readings
        [HttpGet("serial/stop")]
        public ActionResult<SerialPort> SerialStop()
        {
            // Create the serial port with basic settings
            var port = _services.SerialStop();
            return Ok(port);
        }

        [HttpPost("{sensorId}")]
        public async Task<ActionResult<Reading>> Post(Guid sensorId, [FromBody] ReadingRequest request)
        {
            var reading = _converter.ToReadingValue(request);
            var result = await _services.Create(sensorId, reading);
            if (result == null) return NotFound("Reading does not exist");
            return Ok(_converter.ToReadingResponse(result));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Reading>> Put(Guid id, [FromBody] ReadingRequest request)
        {
            var reading = _converter.ToReadingValue(request);
            var response = await _services.Update(id, reading);

            if (request == null) return NotFound();
            return Ok(_converter.ToReadingResponse(response));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var success = await _services.Delete(id);
            return success ? Ok() : NotFound() as IActionResult;
        }
    }
}