using System;
using System.Threading;
using System.Threading.Tasks;

namespace P1ReaderApp.Services
{
    public interface IMessageBuffer<T>
    {
        Task QueueMessage(T message, CancellationToken cancellationToken);

        void RegisterMessageHandler(Func<T, Task> action);
    }
}