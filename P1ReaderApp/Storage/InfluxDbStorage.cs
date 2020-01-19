using P1ReaderApp.Model;
using Serilog;
using System.Threading.Tasks;

namespace P1ReaderApp.Storage
{
    public class InfluxDbStorage : IStorage
    {
        public Task SaveP1Measurement(P1Measurements p1Measurements)
        {
            Log.Information("Saving P1 measurement ({timestamp}) to InfluxDB", p1Measurements.TimeStamp);

            return Task.CompletedTask;
        }
    }
}