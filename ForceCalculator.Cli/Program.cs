using System.CommandLine;
using System.Text;
using ForceCalculator.Cli.Commands;
using ForceCalculationLib;

namespace ForceCalculator.Cli
{
    class Program
    {
        private static async Task Main(string[] args)
        {
            var rootCommand = new RootCommand("Force Calculator CLI");

            // Определяем общий параметр --max-force
            var maxForceOption = new Option<float>(
                name: "--max-force",
                description: "Maximum force per robot (in Newtons, default: 50.0)",
                getDefaultValue: () => 50.0f);
            rootCommand.AddGlobalOption(maxForceOption);

            var experiment = new ExperimentCommand();
            rootCommand.AddCommand(experiment.Command);
            
            var analyze = new AnalyzeCommand();
            rootCommand.AddCommand(analyze.Command);

            // Передаём аргументы в InvokeAsync
            await rootCommand.InvokeAsync(args);
        }

        public static void WriteProgressBar(int now, int total)
        {
            int length = 50;
            int filled = (int)(length * ((now + 1) / (float)total));
            int empty = length - filled;

            StringBuilder sb = new();
            sb.Append('\r');
            sb.Append("Progress: ");
            sb.Append('\u2593', filled);
            sb.Append('\u2591', empty);
            sb.Append('\t');
            sb.Append(now);
            sb.Append('/');
            sb.Append(total);
            Console.Write(sb.ToString());
        }
    }
}