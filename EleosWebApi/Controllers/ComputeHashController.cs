using EleosWebApi.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EleosWebApi.Controllers
{
    [Route("api/ComputeHashController")]
    public class ComputeHashController : Controller
    {
        private readonly ILogger<ComputeHashController> logger;
        private readonly IRecordingsUtils utils;

        public ComputeHashController( ILogger<ComputeHashController> logger, IRecordingsUtils utils)
        {
            this.logger = logger;
            this.utils = utils;
        }

        [HttpGet("GetHashIdByRecordingMeetingId")]
        public IActionResult GetHashIdByRecordingMeetingId(string uuid)
        {
            try
            {
                var hashId = utils.ComputeSha256Hash(uuid);
                return Ok(hashId);
            }
            catch(Exception ex)
            {
                logger.LogError($"GetHashIdByRecordingMeetingId error:{ex.Message}");
                return BadRequest(ex);
            }
        }
    }
}
