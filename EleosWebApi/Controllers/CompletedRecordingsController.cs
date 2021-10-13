using EleosWebsite.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using EleosWebApi.Utils;
using EleosWebApi.Data;
using Microsoft.Extensions.Logging;

namespace EleosWebApi.Controllers
{
    [Route("api/CompletedRecordingsController")]
    public class CompletedRecordingsController : Controller
    {
        private string downloadsPath = @$"{Environment.CurrentDirectory}\DownloadedRecords";

        private readonly IRecordingsRepository recordsRepo;
        private readonly ILogger<CompletedRecordingsController> logger;
        private readonly IRecordingsUtils utils;

        public CompletedRecordingsController(IRecordingsRepository recordsRepo,
            ILogger<CompletedRecordingsController> logger,
            IRecordingsUtils utils)
        {
            this.recordsRepo = recordsRepo;
            this.logger = logger;
            this.utils = utils;
        }

        [HttpPost("DownloadCompletedRecording")]
        public IActionResult DownloadCompletedRecording([FromBody] CompletedZoomRecording recording)
        {
            try
            {
                if (recording == null)
                {
                    logger.LogError("DownloadCompletedRecording recording data is unvalid");
                    return BadRequest(new Exception("ERROR: Unvalid recording object"));
                }
                //Download Files to local folder
                utils.DownloadCompletedRecordingFiles(recording, downloadsPath);
                //Save Record to DB
                recordsRepo.SaveRecording(recording);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            return Ok();
        }
    }
}
