using Microsoft.Extensions.CommandLineUtils;
using System;

namespace P1ReaderApp.Exceptions
{
    public class ConfigurationValueRequiredException : Exception
    {
        public ConfigurationValueRequiredException(CommandOption option) :
            base($"Required: -{option.ShortName} is required. Show help with -h")
        {
        }
    }
}