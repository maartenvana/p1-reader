using Microsoft.Extensions.CommandLineUtils;

namespace P1ReaderApp.Extensions
{
    public static class CommandLineApplicationExtensions
    {
        public static CommandOption CreateBaudRateOption(this CommandLineApplication app) =>
            app.Option("--baudrate", "Baudrate to read serial port with (Example: 115200 or 9600)", CommandOptionType.SingleValue);

        public static CommandOption CreateDataBitsOption(this CommandLineApplication app) =>
            app.Option("--databits", "Databits to read serial port with (Example: 7 or 8)", CommandOptionType.SingleValue);

        public static CommandOption CreateInfluxDatabaseOption(this CommandLineApplication app) =>
            app.Option("--influxdatabase", "InfluxDB database name", CommandOptionType.SingleValue);

        public static CommandOption CreateInfluxHostOption(this CommandLineApplication app) =>
                                    app.Option("--influxhost", "InfluxDB server host", CommandOptionType.SingleValue);

        public static CommandOption CreateInfluxPasswordOption(this CommandLineApplication app) =>
            app.Option("--influxpassword", "InfluxDB database password", CommandOptionType.SingleValue);

        public static CommandOption CreateInfluxUserNameOption(this CommandLineApplication app) =>
                            app.Option("--influxusername", "InfluxDB database username", CommandOptionType.SingleValue);

        public static CommandOption CreateParityOption(this CommandLineApplication app) =>
            app.Option("--parity", "Databits to read serial port with (Example: 1 (Even) or 0 (None))", CommandOptionType.SingleValue);

        public static CommandOption CreatePortOption(this CommandLineApplication app) =>
            app.Option("--port", "Serial port to read from (Example: \"/dev/ttyUSB0\")", CommandOptionType.SingleValue);

        public static CommandOption CreateStopBitsOption(this CommandLineApplication app) =>
            app.Option("--stopbits", "Stopbits to read serial port with (Example: 1)", CommandOptionType.SingleValue);
    }
}