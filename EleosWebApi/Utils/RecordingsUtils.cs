using EleosWebsite.Models;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EleosWebApi.Utils
{
    public interface IRecordingsUtils
    {
        public void DownloadCompletedRecordingFiles(CompletedZoomRecording recording, string path);
        public string ComputeSha256Hash(string recordingUUID);
    }

    public class RecordingsUtils : IRecordingsUtils
    {
        private readonly ILogger<RecordingsUtils> logger;
        public RecordingsUtils(ILogger<RecordingsUtils> logger)
        {
            this.logger = logger;
        }

        public void DownloadCompletedRecordingFiles(CompletedZoomRecording recording, string path)
        {
            try
            {
                var files = recording.payload.@object.recording_files;
                var fileFolder = Directory.CreateDirectory($"{path}\\{recording.payload.@object.uuid}");

                foreach (var file in files)
                {
                    if (file.download_url == null || file.download_url == "")
                        continue;

                    string fileName = Path.Combine($"{fileFolder}", $"record_{file.id}.{file.file_extension}");
                    using (var client = new WebClient())
                    {
                        client.DownloadFileAsync(new Uri(file.download_url), fileName);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"DownloadCompletedRecordingFiles error:{ex.Message}");
                throw ex;
            }
        }
        public string ComputeSha256Hash(string recordingUUID)
        {
            try
            {
                // Create a SHA256   
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    // ComputeHash - returns byte array  
                    byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(recordingUUID));

                    // Convert byte array to a string   
                    StringBuilder builder = new StringBuilder();
                    for (int i = 0; i < bytes.Length; i++)
                    {
                        builder.Append(bytes[i].ToString("x2"));
                    }
                    return builder.ToString();
                }
            }
            catch (Exception ex)
            {
                logger.LogError($"ComputeSha256Hash error:{ex.Message}");
                throw ex;
            }
        }
    }
}
