using Microsoft.Extensions.CommandLineUtils;
using P1ReaderApp.Exceptions;
using P1ReaderApp.Extensions;
using P1ReaderApp.Model;
using P1ReaderApp.Services;
using P1ReaderApp.Storage;
using Serilog;
using Serilog.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace P1ReaderApp
{
    internal static class Program
    {
        private static IMessageBuffer<P1Measurements> _measurementsBuffer;
        private static IMessageBuffer<P1MessageCollection> _serialMessageBuffer;

        private static void CreateDaemonLogger(int minLogLevel)
        {
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Is((LogEventLevel)minLogLevel)
                                .WriteTo.Console()
                                .CreateLogger();
        }

        private static void CreateStatusLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static Action<CommandLineApplication> DebugApplication()
        {
            return (target) =>
            {
                target.Description = "Show debug information";
                target.HelpOption("-? | -h | --help");

                var portOption = target.CreatePortOption();
                var baudRateOption = target.CreateBaudRateOption();
                var stopBitsOption = target.CreateStopBitsOption();
                var dataBitsOption = target.CreateDataBitsOption();
                var parityOption = target.CreateParityOption();

                target.OnExecute(async () =>
                {
                    try
                    {
                        CreateStatusLogger();

                        var port = portOption.GetRequiredStringValue();
                        var baudRate = baudRateOption.GetRequiredIntValue();
                        var stopBits = stopBitsOption.GetRequiredIntValue();
                        var dataBits = dataBitsOption.GetRequiredIntValue();
                        var parity = parityOption.GetRequiredIntValue();

                        IStatusPrintService statusPrintService = new ConsoleStatusPrintService();
                        _measurementsBuffer.RegisterMessageHandler(statusPrintService.UpdateP1Measurements);
                        _serialMessageBuffer.RegisterMessageHandler(statusPrintService.UpdateRawData);

                        var serialPortReader = new SerialPortReader(port, baudRate, stopBits, parity, dataBits, _serialMessageBuffer);
                        serialPortReader.StartReading();

                        while (true)
                        {
                            Console.ReadLine();
                        }
                    }
                    catch (ConfigurationValueRequiredException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                    catch (Exception exception)
                    {
                        Log.Fatal(exception, "Unexpected exception during startup");
                    }

                    return await WaitForCancellation();
                });
            };
        }

        private static Action<CommandLineApplication> InfluxDbApplication()
        {
            return (target) =>
            {
                target.Description = "Write to influxdb";
                target.HelpOption("-? | -h | --help");

                var loggingOption = target.CreateLoggingOption();

                var portOption = target.CreatePortOption();
                var baudRateOption = target.CreateBaudRateOption();
                var stopBitsOption = target.CreateStopBitsOption();
                var dataBitsOption = target.CreateDataBitsOption();
                var parityOption = target.CreateParityOption();

                var influxHostOption = target.CreateInfluxHostOption();
                var influxDatabaseOption = target.CreateInfluxDatabaseOption();
                var influxUsernameOption = target.CreateInfluxUserNameOption();
                var influxPasswordOption = target.CreateInfluxPasswordOption();

                target.OnExecute(async () =>
                {
                    try
                    {
                        var loglevel = loggingOption.GetOptionalIntValue(3);
                        CreateDaemonLogger(loglevel);

                        var port = portOption.GetRequiredStringValue();
                        var baudRate = baudRateOption.GetRequiredIntValue();
                        var stopBits = stopBitsOption.GetRequiredIntValue();
                        var dataBits = dataBitsOption.GetRequiredIntValue();
                        var parity = parityOption.GetRequiredIntValue();

                        var influxHost = influxHostOption.GetRequiredStringValue();
                        var influxDatabase = influxDatabaseOption.GetRequiredStringValue();
                        var influxUsername = influxUsernameOption.GetOptionalStringValue(null);
                        var influxPassword = influxPasswordOption.GetOptionalStringValue(null);

                        IStorage storage = new InfluxDbStorage(influxHost, influxDatabase, influxUsername, influxPassword);
                        _measurementsBuffer.RegisterMessageHandler(storage.SaveP1Measurement);

                        var serialPortReader = new SerialPortReader(port, baudRate, stopBits, parity, dataBits, _serialMessageBuffer);
                        serialPortReader.StartReading();

                        Console.ReadLine();
                    }
                    catch (ConfigurationValueRequiredException exception)
                    {
                        Console.WriteLine(exception.Message);
                    }
                    catch (Exception exception)
                    {
                        Log.Fatal(exception, "Unexpected exception during startup");
                    }

                    return await WaitForCancellation();
                });
            };
        }

        private static void Main(string[] args)
        {
            _serialMessageBuffer = new MessageBuffer<P1MessageCollection>();
            _measurementsBuffer = new MessageBuffer<P1Measurements>();
            var messageParser = new MessageParser(_measurementsBuffer);

            _serialMessageBuffer.RegisterMessageHandler(messageParser.ParseSerialMessages);

            var commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);

            commandLineApplication.HelpOption("-? | -h | --help");

            commandLineApplication.Command("debug", DebugApplication());
            commandLineApplication.Command("influxdb", InfluxDbApplication());
            commandLineApplication.Command("debug", DebugApplication());

            commandLineApplication.OnExecute(() =>
            {
                commandLineApplication.ShowHelp();

                return 1;
            });

            commandLineApplication.Execute(args);
        }

        private static async Task<int> WaitForCancellation()
        {
            var cancellationTokenSource = new CancellationTokenSource();
            AppDomain.CurrentDomain.ProcessExit += (s, e) => cancellationTokenSource.Cancel();
            Console.CancelKeyPress += (s, e) => cancellationTokenSource.Cancel();
            return await Task.Delay(-1, cancellationTokenSource.Token).ContinueWith(t => { return 1; });
        }
    }
}