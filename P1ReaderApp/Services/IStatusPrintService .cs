using P1ReaderApp.Model;
using System.Threading.Tasks;

namespace P1ReaderApp.Services
{
    public interface IStatusPrintService 
    {
        Task UpdateP1Measurements(P1Measurements measurements);

        Task UpdateRawData(P1MessageCollection message);
    }
}