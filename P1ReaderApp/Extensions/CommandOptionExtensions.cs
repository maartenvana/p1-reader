using Microsoft.Extensions.CommandLineUtils;
using P1ReaderApp.Exceptions;

namespace P1ReaderApp.Extensions
{
    public static class CommandOptionExtensions
    {
        public static int GetRequiredIntValue(this CommandOption baudRateOption)
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

        public static string GetRequiredStringValue(this CommandOption portOption)
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
    }
}