using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace P1ReaderApp.Services
{
    public class MessageBuffer<T> : IMessageBuffer<T>
    {
        private readonly BufferBlock<T> _buffer = new BufferBlock<T>();

        private readonly List<Func<T, Task>> _messageHandlers = new List<Func<T, Task>>();

        public MessageBuffer()
        {
            Task.Run(async () =>
            {
                while (true)
                {
                    var message = await _buffer.ReceiveAsync();

                    Parallel.ForEach(_messageHandlers, async (x) =>
                    {
                        await x(message);
                    });
                }
            });
        }

        public async Task QueueMessage(T message, CancellationToken cancellationToken)
        {
            await _buffer.SendAsync(message, cancellationToken);
        }

        public void RegisterMessageHandler(Func<T, Task> action)
        {
            _messageHandlers.Add(action);
        }
    }
}