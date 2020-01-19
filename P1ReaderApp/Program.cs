using Microsoft.Extensions.CommandLineUtils;
using P1ReaderApp.Exceptions;
using P1ReaderApp.Extensions;
using P1ReaderApp.Model;
using P1ReaderApp.Services;
using P1ReaderApp.Storage;
using Serilog;
using System;
using System.Collections.Generic;

namespace P1ReaderApp
{
    internal static class Program
    {
        private static IMessageBuffer<P1Measurements> _measurementsBuffer;
        private static IMessageBuffer<List<string>> _serialMessageBuffer;

        private static void CreateDaemonLogger()
        {
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Information()
                                .WriteTo.Console()
                                .WriteTo.File("/dev/log/p1reader.log")
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

                target.OnExecute(() =>
                {
                    try
                    {
                        CreateStatusLogger();

                        var port = portOption.GetRequiredStringValue();
                        var baudRate = baudRateOption.GetRequiredIntValue();
                        var stopBits = stopBitsOption.GetRequiredIntValue();
                        var dataBits = dataBitsOption.GetRequiredIntValue();
                        var parity = parityOption.GetRequiredIntValue();

                        var statusPrintService = new ConsoleStatusPrintService();
                        _measurementsBuffer.RegisterMessageHandler(statusPrintService.PrintP1Measurement);

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

                    return 1;
                });
            };
        }

        private static Action<CommandLineApplication> InfluxDbApplication()
        {
            return (target) =>
            {
                target.Description = "Write to influxdb";
                target.HelpOption("-? | -h | --help");

                var portOption = target.CreatePortOption();
                var baudRateOption = target.CreateBaudRateOption();
                var stopBitsOption = target.CreateStopBitsOption();
                var dataBitsOption = target.CreateDataBitsOption();
                var parityOption = target.CreateParityOption();

                var influxHostOption = target.CreateInfluxHostOption();
                var influxDatabaseOption = target.CreateInfluxDatabaseOption();
                var influxUsernameOption = target.CreateInfluxUserNameOption();
                var influxPasswordOption = target.CreateInfluxPasswordOption();

                target.OnExecute(() =>
                {
                    try
                    {
                        CreateDaemonLogger();

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

                    return 1;
                });
            };
        }

        private static void Main(string[] args)
        {
            _serialMessageBuffer = new MessageBuffer<List<string>>();
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
    }
}