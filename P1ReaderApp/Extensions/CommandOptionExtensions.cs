using Microsoft.Extensions.CommandLineUtils;
using P1ReaderApp.Exceptions;

namespace P1ReaderApp.Extensions
{
    public static class CommandOptionExtensions
    {
        public static string GetOptionalStringValue(this CommandOption option, string defaultValue)
        {
            return option.HasValue() ? option.Value() : defaultValue;
        }

        public static int GetRequiredIntValue(this CommandOption option)
        {
            if (!option.HasValue())
            {
                throw new ConfigurationValueRequiredException(option);
            }

            return int.Parse(option.Value());
        }

        public static string GetRequiredStringValue(this CommandOption option)
        {
            if (!option.HasValue())
            {
                throw new ConfigurationValueRequiredException(option);
            }

            return option.Value();
        }
    }
}