using EleosWebApi.Models;
using EleosWebsite.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EleosWebApi.Data
{
    public interface IRecordingsRepository
    {
        public void SaveRecording(CompletedZoomRecording recording);
        public ZoomRecording GetRecordingByUUID(string uuid);
    }
}
