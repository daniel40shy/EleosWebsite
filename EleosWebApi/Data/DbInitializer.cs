using EleosWebApi.Models;
using System.Linq;

namespace EleosWebApi.Data
{
    public class DbInitializer
    {
        public static void Initialize(ZoomRecordingDbContext context)
        {
            context.Database.EnsureCreated();

            if (context.ZoomRecordings.Any())
            {
                return;   
            }

            context.ZoomRecordings.Add(new ZoomRecording("UUID","HashID","Data"));
            context.SaveChanges();
        }
    }
}
