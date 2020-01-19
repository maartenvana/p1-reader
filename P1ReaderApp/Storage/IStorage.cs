using P1ReaderApp.Model;
using System.Threading.Tasks;

namespace P1ReaderApp.Storage
{
    public interface IStorage
    {
        Task SaveP1Measurement(P1Measurements p1Measurements);
    }
}