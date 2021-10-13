using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EleosWebApi.Models
{
    public class ZoomRecording
    {
        [Key]
        public string RecordingUUID { get; set; }
        public string HashID { get; set; }
        public string RecordingData { get; set; }

        public ZoomRecording(string RecordingUUID, string HashID, string RecordingData)
        {
            this.RecordingUUID = RecordingUUID;
            this.HashID = HashID;
            this.RecordingData = RecordingData;
        }
        public ZoomRecording()
        {

        }
    }
}
