using ForceCalculationLib;
using GeometryLib;
using System.CommandLine;
using System.Text.Json;

namespace ForceCalculator.Cli.Commands
{
    public class AnalyzeCommand
    {
        private readonly Option<FileInfo> _fromFileOption =
            new("--from-file", "The path to experiment file.");

        private readonly Option<float> _maxForceOption =
            new("--max-force", () => 50.0f, "Maximum force per robot (in Newtons, default: 50.0)");

        public Command Command { get; }

        public AnalyzeCommand()
        {
            Command = new Command("analyze", "Analyzes the results of the experiment.")
            {
                _fromFileOption,
                _maxForceOption
            };

            Command.SetHandler((file, maxForce) =>
            {
                if (!file.Exists)
                {
                    Console.WriteLine($"File {file.FullName} does not exist.");
                    return;
                }

                using var stream = new FileStream(file.FullName, FileMode.Open);
                ExperimentData? data = JsonSerializer.Deserialize<ExperimentData>(stream);

                if (data == null)
                {
                    Console.WriteLine($"File {file.FullName} could not be deserialized.");
                    return;
                }

                Console.WriteLine($"File {file.FullName} deserialized successfully.");

                int total = data.Cases.Count();
                Console.WriteLine();
                Console.WriteLine($"Total cases: {total}");
                Console.WriteLine($"Supports generation pattern: {data.SupportsGenerationType}");
                Console.WriteLine($"Robots generation pattern: {data.RobotsGenerationType}");
                Console.WriteLine($"Max force per robot: {maxForce:F2} N");

                int validEqual = 0;
                int validIndividual = 0;

                List<float> equalValidForces = [];
                List<float> individualOnEqualValidForces = [];
                List<float> individualValidForces = [];

                List<float> improvementInPercent = [];

                foreach (CaseData dataCase in data.Cases)
                {
                    var platform = ConvertToPlatform(dataCase.Platform, maxForce);
                    // Проверка, что силы не превышают MaxForcePerRobot
                    bool forceWithinLimit = dataCase.EqualRobotForces.All(f => f <= platform.MaxForcePerRobot) &&
                                          dataCase.IndividualRobotForces.All(f => f <= platform.MaxForcePerRobot);

                    if (dataCase.IsEqualSolvedValid && forceWithinLimit)
                    {
                        validEqual++;
                        equalValidForces.Add(dataCase.EqualRobotForceSum);
                    }

                    if (dataCase.IsIndividualSolvedValid && forceWithinLimit)
                    {
                        validIndividual++;
                        individualValidForces.Add(dataCase.IndividualRobotForceSum);

                        if (dataCase.IsEqualSolvedValid)
                        {
                            individualOnEqualValidForces.Add(dataCase.IndividualRobotForceSum);
                            float improvement = (dataCase.IndividualRobotForceSum - dataCase.EqualRobotForceSum) / dataCase.EqualRobotForceSum * 100;
                            improvementInPercent.Add(improvement);
                        }
                    }
                }

                if (equalValidForces.Count == 0) equalValidForces.Add(0);
                if (individualValidForces.Count == 0) individualValidForces.Add(0);
                if (individualOnEqualValidForces.Count == 0) individualOnEqualValidForces.Add(0);

                float averageEqualForce = equalValidForces.Average();
                float averageIndividual = individualValidForces.Average();
                float averageIndividualOnEqualForce = individualOnEqualValidForces.Average();

                Console.WriteLine("------------------");
                Console.WriteLine($"Valid equal solver cases: {validEqual} ({validEqual / (float)total * 100:F2}%)");
                Console.WriteLine($"Average equal solver force: {averageEqualForce:F2} N");
                Console.WriteLine("------------------");
                Console.WriteLine($"Valid individual solver cases: {validIndividual} ({validIndividual / (float)total * 100:F2}%)");
                Console.WriteLine($"Average individual solver force: {averageIndividual:F2} N");
                Console.WriteLine("------------------");
                Console.WriteLine($"Average individual solver force (on equal also valid): {averageIndividualOnEqualForce:F2} N");
                Console.WriteLine($"Individual valid when equal not valid: {(validIndividual - validEqual) / (float)(total - validEqual) * 100:F2}%");
                Console.WriteLine($"Individual force gain: {(averageIndividual - averageEqualForce) / averageIndividual * 100:F2}%");
                Console.WriteLine("------------------");

                WriteSplitInfo(2f, 0.1f, improvementInPercent.ToArray());
                Console.WriteLine("Data analyze finished.");
            }, _fromFileOption, _maxForceOption);
        }

        private static void WriteSplitInfo(float to, float step, float[] data)
        {
            float divider = data.Length / 100f;
            float min = float.MinValue;
            for (float max = step; Math.Abs(max - to) < 0.01f || max < to; max += step)
            {
                float casePercentage = data.Count(f => f > min && f <= max) / divider;
                Console.WriteLine($"Improve {max * 100:F0}%: \t{casePercentage:F2}\t%");
                min = max;
            }

            float t = data.Count(f => f > min) / divider;
            Console.WriteLine($"Improve >{min * 100:F0}%: \t{t:F2}\t%");
        }

        private static Platform ConvertToPlatform(PlatformData platformData, float maxForce)
        {
            var allSupportPositions = platformData.InnerSupports.Concat(platformData.OuterSupports).ToArray();
            var supports = Supports.FromPositions(allSupportPositions); // Предполагаемый статический метод
            var robots = platformData.Robots.Select(pos => new Robot(pos)).ToArray();
            return new Platform(supports, robots, platformData.Gravity, maxForce);
        }
    }

    public class ExperimentData
    {
        public required PlatformType SupportsGenerationType { get; set; }
        public required string SupportsGenerationTypeDescription { get; set; }
        public required int SupportsNumber { get; set; }
        public required RobotsType RobotsGenerationType { get; set; }
        public required string RobotsGenerationTypeDescription { get; set; }
        public required int RobotsNumber { get; set; }
        public int CaseNumber { get; set; }
        public required List<CaseData> Cases { get; set; }
    }

    public class CaseData
    {
        public required PlatformData Platform { get; set; }
        public required bool IsEqualSolvedValid { get; set; }
        public required float EqualRobotForceSum { get; set; }
        public required List<float> EqualRobotForces { get; set; }
        public required bool IsIndividualSolvedValid { get; set; }
        public required float IndividualRobotForceSum { get; set; }
        public required List<float> IndividualRobotForces { get; set; }
    }

    public class PlatformData
    {
        public float Gravity { get; set; }
        public List<Vector2F> InnerSupports { get; set; }
        public List<Vector2F> OuterSupports { get; set; }
        public List<Vector2F> Robots { get; set; }
    }

    public enum PlatformType { Random, Circle, Rectangle }
    public enum RobotsType { Random, Line, Circle }
}