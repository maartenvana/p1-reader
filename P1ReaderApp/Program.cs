using Microsoft.Extensions.CommandLineUtils;
using P1ReaderApp.Exceptions;
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

        private static CommandOption CreateBaudRateOption(CommandLineApplication app) =>
            app.Option("--baudrate", "Baudrate to read serial port with (Example: 115200 or 9600)", CommandOptionType.SingleValue);

        private static void CreateDaemonLogger()
        {
            Log.Logger = new LoggerConfiguration()
                                .MinimumLevel.Information()
                                .WriteTo.Console()
                                .WriteTo.File("/dev/log/p1reader.log")
                                .CreateLogger();
        }

        private static CommandOption CreateDataBitsOption(CommandLineApplication app) =>
            app.Option("--databits", "Databits to read serial port with (Example: 7 or 8)", CommandOptionType.SingleValue);

        private static CommandOption CreateParityOption(CommandLineApplication app) =>
            app.Option("--parity", "Databits to read serial port with (Example: 1 (Even) or 0 (None))", CommandOptionType.SingleValue);

        private static CommandOption CreatePortOption(CommandLineApplication commandLineApplication)
        {
            return commandLineApplication.Option("-p | --port", "Serial port to read from (Example: \"/dev/ttyUSB0\")", CommandOptionType.SingleValue);
        }

        private static void CreateStatusLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.Console()
                .CreateLogger();
        }

        private static CommandOption CreateStopBitsOption(CommandLineApplication app) =>
            app.Option("--stopbits", "Stopbits to read serial port with (Example: 1)", CommandOptionType.SingleValue);

        private static Action<CommandLineApplication> DebugApplication()
        {
            return (target) =>
            {
                target.Description = "Show debug information";
                target.HelpOption("-? | -h | --help");

                var portOption = CreatePortOption(target);
                var baudRateOption = CreateBaudRateOption(target);
                var stopBitsOption = CreateStopBitsOption(target);
                var dataBitsOption = CreateDataBitsOption(target);
                var parityOption = CreateParityOption(target);

                target.OnExecute(() =>
                {
                    try
                    {
                        CreateStatusLogger();

                        var port = GetStringValue(portOption);
                        var baudRate = GetIntValue(baudRateOption);
                        var stopBits = GetIntValue(stopBitsOption);
                        var dataBits = GetIntValue(dataBitsOption);
                        var parity = GetIntValue(parityOption);

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

        private static int GetIntValue(CommandOption baudRateOption)
        {
            if (baudRateOption.HasValue())
            {
                return int.Parse(baudRateOption.Value());
            }
            else
            {
                throw new ConfigurationValueRequiredException(baudRateOption);
            }
        }

        private static string GetStringValue(CommandOption portOption)
        {
            if (portOption.HasValue())
            {
                return portOption.Value();
            }
            else
            {
                throw new ConfigurationValueRequiredException(portOption);
            }
        }

        private static Action<CommandLineApplication> InfluxDbApplication()
        {
            return (target) =>
            {
                target.Description = "Write to influxdb";
                target.HelpOption("-? | -h | --help");

                var portOption = CreatePortOption(target);
                var baudRateOption = CreateBaudRateOption(target);
                var stopBitsOption = CreateStopBitsOption(target);
                var dataBitsOption = CreateDataBitsOption(target);
                var parityOption = CreateParityOption(target);

                target.OnExecute(() =>
                {
                    try
                    {
                        CreateDaemonLogger();

                        var port = GetStringValue(portOption);
                        var baudRate = GetIntValue(baudRateOption);
                        var stopBits = GetIntValue(stopBitsOption);
                        var dataBits = GetIntValue(dataBitsOption);
                        var parity = GetIntValue(parityOption);

                        IStorage storage = new InfluxDbStorage();
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