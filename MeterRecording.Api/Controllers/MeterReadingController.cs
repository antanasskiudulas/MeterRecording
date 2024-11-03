using MeterRecording.Application.DTOs;
using MeterRecording.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MeterRecordingApi.Controllers
{
    [ApiController]
    public class MeterReadingController : ControllerBase
    {
        private readonly IMeterProcessingService _meterProcessingService;
        private readonly ILogger<MeterReadingController> _logger;

        public MeterReadingController(
            IMeterProcessingService meterProcessingService,
            ILogger<MeterReadingController> logger)
        {
            _meterProcessingService = meterProcessingService;
            _logger = logger;
        }

        [HttpPost]
        [Route("meter-reading-uploads")]
        public async Task<IActionResult> RecordMeterReadings(IFormFile file, CancellationToken cancellationToken)
        {
            if (file.Length == 0)
            {
                return BadRequest("File is empty");
            }

            ProcessMeterReadingResultsDto results = await _meterProcessingService.ProcessMeterReadingsAsync(file.OpenReadStream(), cancellationToken);

            return Ok(results);
        }
    }
}
