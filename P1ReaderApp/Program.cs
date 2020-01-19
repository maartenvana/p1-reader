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

        private static int GetBaudRateValue(CommandOption baudRateOption)
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

        private static string GetPortValue(CommandOption portOption)
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

        private static void Main(string[] args)
        {
            IMessageBuffer<List<string>> serialMessageBuffer = new MessageBuffer<List<string>>();
            IMessageBuffer<P1Measurements> measurementsBuffer = new MessageBuffer<P1Measurements>();
            IMessageParser messageParser = new MessageParser(measurementsBuffer);

            serialMessageBuffer.RegisterMessageHandler(messageParser.ParseSerialMessages);

            var commandLineApplication = new CommandLineApplication(throwOnUnexpectedArg: false);

            var portOption = commandLineApplication.Option("-p | --port", "Serial port to read from (Example: \"/dev/ttyUSB0\")", CommandOptionType.SingleValue);
            var baudRateOption = commandLineApplication.Option("-b | --baudrate", "Baudrate to read serial port with (Example: 115200 or 9600)", CommandOptionType.SingleValue);

            commandLineApplication.HelpOption("-? | -h | --help");

            commandLineApplication.Command("debug", (target) =>
            {
                target.Description = "Show debug information";

                target.OnExecute(() =>
                {
                    try
                    {
                        CreateStatusLogger();

                        var port = GetPortValue(portOption);
                        var baudRate = GetBaudRateValue(baudRateOption);

                        var statusPrintService = new ConsoleStatusPrintService();
                        measurementsBuffer.RegisterMessageHandler(statusPrintService.PrintP1Measurement);

                        var serialPortReader = new SerialPortReader(port, baudRate, serialMessageBuffer);
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
            });

            commandLineApplication.Command("influxdb", (target) =>
            {
                target.Description = "Write to influxdb";

                target.OnExecute(() =>
                {
                    try
                    {
                        CreateDaemonLogger();

                        var port = GetPortValue(portOption);
                        var baudRate = GetBaudRateValue(baudRateOption);

                        IStorage storage = new InfluxDbStorage();
                        measurementsBuffer.RegisterMessageHandler(storage.SaveP1Measurement);

                        var serialPortReader = new SerialPortReader(port, baudRate, serialMessageBuffer);
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
            });

            commandLineApplication.OnExecute(() =>
            {
                commandLineApplication.ShowHelp();
            });

            commandLineApplication.Execute(args);
        }
    }
}