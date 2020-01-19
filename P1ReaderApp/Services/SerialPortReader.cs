using Serilog;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace P1ReaderApp.Services
{
    public class SerialPortReader : IDisposable
    {
        private readonly IMessageBuffer<List<string>> _messageBuffer;
        private readonly SerialPort _serialPort;

        private CancellationTokenSource _cancellationTokenSource;
        private bool _disposedValue = false;

        public SerialPortReader(string portName, int baudrate, IMessageBuffer<List<string>> messageBufferService)
        {
            _serialPort = new SerialPort(portName, baudrate)
            {
                ReadTimeout = 20_000,
                Parity = Parity.Even,
                DataBits = 7,
                StopBits = StopBits.One
            };

            _messageBuffer = messageBufferService;
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void StartReading()
        {
            if (!_serialPort.IsOpen)
            {
                _cancellationTokenSource = new CancellationTokenSource();

                try
                {
                    Log.Information("Opening serial port");

                    _serialPort.Open();

                    Log.Information("Starting read task");

                    CreateReadTask(_cancellationTokenSource.Token);
                }
                catch (Exception exception)
                {
                    Log.Fatal(exception, "Error during serial port read");
                }
            }
            else
            {
                Log.Error("Cannot read serial port: already opened");
            }
        }

        public void StopReading()
        {
            if (!_cancellationTokenSource.IsCancellationRequested)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _cancellationTokenSource.Cancel();
                    _serialPort.Close();
                }

                _disposedValue = true;
            }
        }

        private void CreateReadTask(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Read(cancellationToken);
                    }
                }
                catch (Exception exception)
                {
                    Log.Error(exception, "Error during read");
                }
            }, cancellationToken);
        }

        private async Task Read(CancellationToken cancellationToken)
        {
            try
            {
                var messages = new List<string>();

                while (true)
                {
                    var message = _serialPort.ReadLine();

                    messages.Add(message);

                    if (message.StartsWith("!"))
                    {
                        break;
                    }
                }

                await _messageBuffer.QueueMessage(messages, cancellationToken);

                Log.Debug("{messages}", messages);
            }
            catch (TimeoutException)
            {
                // Do nothing
            }
            catch (Exception exception)
            {
                Log.Error(exception, "Unexpected exception during serial read");
            }
        }
    }
}