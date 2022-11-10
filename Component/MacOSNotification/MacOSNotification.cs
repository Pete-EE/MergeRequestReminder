using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MergeRequestReminder.Component.MacOSNotification
{
    public class MacOSNotification
    {
        public static async Task Show(string appString, string title, string message, string sound, CancellationToken token = default)
        {
            var command =
                $" -c \"osascript -e 'display notification \\\"{appString}\\\" with title \\\"{title}\\\" subtitle \\\"{message}\\\" sound name \\\"{sound}\\\"'";
            command = new string(command.Where(c => !char.IsControl(c)).ToArray());
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WindowStyle = ProcessWindowStyle.Normal,
                FileName = "/bin/bash",
                Arguments = command,
                CreateNoWindow = false,
            };
            process.StartInfo = startInfo;
            process.Start();
            await process.WaitForExitAsync(token);
        }
    }
}