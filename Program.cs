using System;
using System.Collections.Specialized;
using System.Threading;
using System.Threading.Tasks;
using MergeRequestReminder.Component.Engine;

namespace MergeRequestReminder;

class Program
{
    static async Task Main(string[] args)
    {
        var cancelSource = new CancellationTokenSource();

        var engine = new Engine();
        engine.ProcessCommands(args);
        await Task.Run(() => engine.StartChecking(), cancelSource.Token);
        var running = true;
        while (running)
        {
            var key = Console.ReadKey();
            if (key.Key == ConsoleKey.Backspace)
            {
                engine.CommandPrompt();
            }
            else
            {
                running = false;
            }
        }

        cancelSource.Cancel();
    }
}