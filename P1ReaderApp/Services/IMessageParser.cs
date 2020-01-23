using P1ReaderApp.Model;
using System.Threading.Tasks;

namespace P1ReaderApp.Services
{
    public interface IMessageParser
    {
        Task<P1Measurements> ParseSerialMessages(P1MessageCollection messageCollection);
    }
}