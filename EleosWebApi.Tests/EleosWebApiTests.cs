using EleosWebApi.Data;
using EleosWebApi.Models;
using EleosWebApi.Utils;
using EleosWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.IO;
using System.Linq;

namespace EleosWebApi.Tests
{
    public class EleosWebApiTests
    {
        private IRecordingsUtils recordingsUtils;
        private IRecordingsRepository recordingsRepository;
        private ZoomRecordingDbContext recordingsContext;

        private string projectPath;
        private string downloadsPath;
        private string jsonTestFilePath;

        [SetUp]
        public void Setup()
        {
            Logger<RecordingsUtils> utilsLogger = new Logger<RecordingsUtils>(new LoggerFactory());
            Logger<RecordingsRepository> recordingLogger = new Logger<RecordingsRepository>(new LoggerFactory());

            var options = new DbContextOptionsBuilder<ZoomRecordingDbContext>()
           .UseInMemoryDatabase(databaseName: "MovieListDatabase")
           .Options;

            recordingsContext = new ZoomRecordingDbContext(options);         
            recordingsUtils = new RecordingsUtils(utilsLogger);
            recordingsRepository = new RecordingsRepository(recordingsContext, recordingLogger, recordingsUtils);

            projectPath = Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName;
            downloadsPath = $@"{projectPath}\Downloads\";
            jsonTestFilePath = $@"{projectPath}\JsonTestFiles\";
        }

        [Test]
        public void TestComputeSha256Hash()
        {
            var uuidHashValue = recordingsUtils.ComputeSha256Hash("MfXLNXTVSxmR+G+AkuLSxQ==");
            Assert.IsTrue(uuidHashValue == "14071c4b902f2243a03e4c886f3f0a7b90bb1cbbd197726a7d9f90a7b7ea5be3");
        }

        [Test]
        public void TestDownloadCompletedRecordingJsonTestFileValid()
        {
            if (Directory.Exists(downloadsPath))
                Directory.Delete(downloadsPath, true);

            Directory.CreateDirectory(downloadsPath);

            CompletedZoomRecording recording = GetRecordingObjectFromTestFile();
            recordingsUtils.DownloadCompletedRecordingFiles(recording, downloadsPath);

            var directories = Directory.GetDirectories($@"{downloadsPath}", "").Select(Path.GetFileName).ToList();
            var filesPath = $@"{downloadsPath}{directories[0]}";
            var files = Directory.GetFiles(filesPath).Select(i => Path.GetFileName(i)).ToList();

            Assert.IsTrue(directories[0] == "MfXLNXTVSxmR+G+AkuLSxQ==");
            Assert.IsTrue(files.Any(i => i == "record_51df55e2-4f7d-41ff-bb8d-036b63829e32.M4A"));
            Assert.IsTrue(files.Any(i => i == "record_c8963dbd-d495-448d-86d9-a4feb1d9f054.MP4"));
        }

        [Test]
        public void TestSaveRecordingValid()
        {
            var completedRecording = GetRecordingObjectFromTestFile();
            recordingsRepository.SaveRecording(completedRecording);
            Assert.IsTrue(recordingsContext.ZoomRecordings.FirstOrDefault(i => i.RecordingUUID == "MfXLNXTVSxmR+G+AkuLSxQ==") != null);
        }

        private CompletedZoomRecording GetRecordingObjectFromTestFile()
        {
            CompletedZoomRecording recording = new CompletedZoomRecording();
            using (StreamReader r = new StreamReader($@"{jsonTestFilePath}\jsonWebhookTestFile.json"))
            {
                recording = JsonConvert.DeserializeObject<CompletedZoomRecording>(r.ReadToEnd());
            }
            return recording;
        }
    }
}