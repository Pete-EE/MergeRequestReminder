using System;

namespace MergeRequestReminder.CustomException;

public class ConfigValueException : Exception
{
    public ConfigValueException()
    {
    }

    public ConfigValueException(string message)
        : base(message)
    {
    }

    public ConfigValueException(string message, Exception inner)
        : base(message, inner)
    {
    }
}