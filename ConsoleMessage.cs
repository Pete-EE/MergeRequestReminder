/*using System;

namespace MergeRequestReminder;

public class ConsoleMessage
{

    private static string buffer = string.Empty;
    private enum MessageType
    {
        Error,
        Warning,
        Success,
        Info,
        Default
    }
    public static void Info(string message)
    {
        ShowMessage(message,MessageType.Info);
    }
    public static void Error(string message)
    {
        ShowMessage(message,MessageType.Error);
    }
    public static void Success(string message)
    {
        ShowMessage(message,MessageType.Success);
    }
    public static void Warning(string message)
    {
        ShowMessage(message,MessageType.Warning);
    }
    public static void Default(string message)
    {
        ShowMessage(message,MessageType.Default);
    }
    public static void AlertInfo(string message, int timeoutInSeconds)
    {
        Console.Clear();
        ShowMessage(message, MessageType.Info,true);
        System.Threading.Thread.Sleep(timeoutInSeconds * 1000);
        Console.Clear();
        WriteLine(buffer,true);
    }
    public static void AlertError(string message, int timeoutInSeconds)
    {
        Console.Clear();
        ShowMessage(message, MessageType.Error,true);
        System.Threading.Thread.Sleep(timeoutInSeconds * 1000);
        Console.Clear();
        WriteLine(buffer,true);
    }
    public static void AlertSuccess(string message, int timeoutInSeconds)
    {
        Console.Clear();
        ShowMessage(message, MessageType.Success,true);
        System.Threading.Thread.Sleep(timeoutInSeconds * 1000);
        Console.Clear();
        WriteLine(buffer,true);
    }
    public static void AlertWarning(string message, int timeoutInSeconds)
    {
        Console.Clear();
        ShowMessage(message, MessageType.Warning,true);
        System.Threading.Thread.Sleep(timeoutInSeconds * 1000);
        Console.Clear();
        WriteLine(buffer,true);
    }
    
    public static void AlertDefault(string message, int timeoutInSeconds)
    {
        Console.Clear();
        ShowMessage(message, MessageType.Default,true);
        System.Threading.Thread.Sleep(timeoutInSeconds * 1000);
        Console.Clear();
        WriteLine(buffer,true);
    }
    public static void Clear()
    {
        buffer = string.Empty;
        Console.Clear();
        Console.WriteLine("\x1b[3J");
        Console.Write("\f\u001bc\x1b[3J");
    }
    private static void ShowMessage(string message, MessageType type, bool suppressBuffer = false)
    {
        var printMessage = type switch
        {
            MessageType.Error => $"[=Red] {message} [/]",
            MessageType.Success => $"[=Green] {message} [/]",
            MessageType.Warning => $"[=Yellow] {message} [/]",
            MessageType.Info => $"[=Blue] {message} [/]",
            MessageType.Default => $"[/] {message} [/]",
            _ => message
        };
        WriteLine(printMessage,suppressBuffer);
        WriteLine("\n",suppressBuffer);
    }
    private static void WriteLine(string msg, bool suppressBuffer = false)
    {
        if (!suppressBuffer)
            buffer += msg;
        var parsedString = msg.Split('[', ']');
        ConsoleColor color;
        foreach (var item in parsedString)
        {
            if (item.StartsWith("/"))
                Console.ResetColor();
            else if (item.StartsWith("=") && Enum.TryParse(item[1..], out color))
                Console.ForegroundColor = color;
            else
                Console.Write(item);
        }
    }
}*/