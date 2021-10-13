using EleosWebApi.Models;
using EleosWebApi.Utils;
using EleosWebsite.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EleosWebApi.Data
{
    public class RecordingsRepository : IRecordingsRepository
    {
        private readonly ZoomRecordingDbContext context;
        private readonly ILogger<RecordingsRepository> logger;
        private readonly IRecordingsUtils utils;
        public RecordingsRepository(ZoomRecordingDbContext context,
            ILogger<RecordingsRepository> logger,  IRecordingsUtils utils)
        {
            this.context = context;
            this.logger = logger;
            this.utils = utils;
        }

        public void SaveRecording(CompletedZoomRecording recording)
        {
            try
            {
                var UUID = recording.payload.@object.uuid;
                var hashID = utils.ComputeSha256Hash(UUID);

                var jsonObject = Newtonsoft.Json.JsonConvert.SerializeObject(recording);
                context.ZoomRecordings.Add(new ZoomRecording(UUID, hashID, jsonObject));
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.LogError($"SaveRecording exception:{ex.Message}");
                throw ex;
            }
        }
        public ZoomRecording GetRecordingByUUID(string uuid)
        {
            try
            {
                return context.ZoomRecordings.FirstOrDefault(i => i.RecordingUUID == uuid);
            }
            catch(Exception ex)
            {
                logger.LogError($"GetRecordingByUUID exception:{ex.Message}");
                throw ex;
            }
        }
    }
}
