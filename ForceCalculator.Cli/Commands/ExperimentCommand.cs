using ForceCalculationLib;
using ForceCalculationLib.Solvers;
using GeometryLib;
using Microsoft.SolverFoundation.Services;
using System.CommandLine;
using System.Text.Json;

namespace ForceCalculator.Cli.Commands
{
    public class ExperimentCommand
    {
        private readonly Option<PlatformType> _platformTypeOption =
            new("--platform-type", "Specifies the platform generation pattern.");

        private readonly Option<int> _supportsNumberOption =
            new("--supports-number", () => 6, "Specifies the number of the supports.");

        private readonly Option<RobotsType> _robotsTypeOption =
            new("--robots-type", "Specifies the robots placing pattern.");

        private readonly Option<int> _robotsNumberOption =
            new("--robots-number", () => 6, "Specifies the number of the robots.");

        private readonly Option<int> _countOption =
            new("--count", () => 1, "Number of generated cases.");

        private readonly Option<FileInfo?> _saveToOption =
            new("--save-to", () => null, "The path to save the results to.");

        private readonly Option<float> _maxForceOption =
            new("--max-force", () => 50.0f, "Maximum force per robot (in Newtons, default: 50.0)");

        public Command Command { get; }

        public ExperimentCommand()
        {
            Command = new Command("experiment", "Generate the results for the specified cases.")
            {
                _platformTypeOption,
                _supportsNumberOption,
                _robotsTypeOption,
                _robotsNumberOption,
                _countOption,
                _saveToOption,
                _maxForceOption
            };

            Command.SetHandler((platformType, supportsNumber, robotsType, robotsNumber, count, saveTo, maxForce) =>
            {
                if (saveTo != null && saveTo.Exists)
                {
                    Console.WriteLine($"File {saveTo.FullName} already exists.");
                    return;
                }

                CaseGenerator generator = new(-100, platformType, supportsNumber, robotsType, robotsNumber);

                List<CaseData> cases = new List<CaseData>(count);
                var equalSolver = new EqualMaxSolver();
                var simplexSolver = new SimplexSolver();

                for (int i = 0; i < count; i++)
                {
                    CaseData caseData = generator.Next();
                    var platform = ConvertToPlatform(caseData.Platform, maxForce);
                    float equalSum = equalSolver.Solve(platform);
                    float[] equalForces = new float[platform.Robots.Length];
                    for (int j = 0; j < platform.Robots.Length; j++)
                        equalForces[j] = platform.Robots[j].PushingForce;
                    bool isEqualValid = equalSum > 0;

                    LinearResult simplexResult = simplexSolver.Solve(platform);
                    float[] indivForces = simplexResult.Select(r => (float)r).Take(platform.Robots.Length).ToArray();
                    float indivSum = indivForces.Sum();
                    bool isIndivValid = indivSum > 0 && indivForces.All(f => f >= 0 && f <= platform.MaxForcePerRobot);

                    caseData.IsEqualSolvedValid = isEqualValid;
                    caseData.EqualRobotForceSum = equalSum * platform.Robots.Length;
                    caseData.EqualRobotForces = equalForces.ToList();

                    caseData.IsIndividualSolvedValid = isIndivValid;
                    caseData.IndividualRobotForceSum = indivSum;
                    caseData.IndividualRobotForces = indivForces.ToList();

                    cases.Add(caseData);
                    Program.WriteProgressBar(i + 1, count);
                }

                if (saveTo != null)
                {
                    ExperimentData data = new()
                    {
                        SupportsGenerationType = platformType,
                        SupportsGenerationTypeDescription = platformType.ToString(),
                        SupportsNumber = supportsNumber,
                        RobotsGenerationType = robotsType,
                        RobotsGenerationTypeDescription = robotsType.ToString(),
                        RobotsNumber = robotsNumber,
                        CaseNumber = count,
                        Cases = cases
                    };

                    using var stream = new FileStream(saveTo.FullName, FileMode.Create);
                    JsonSerializer.Serialize(stream, data, new JsonSerializerOptions { WriteIndented = true });

                    Console.WriteLine($"\nExperiment data saved to {saveTo.FullName}.");
                }
            },
            _platformTypeOption, _supportsNumberOption, _robotsTypeOption,
            _robotsNumberOption, _countOption, _saveToOption, _maxForceOption);
        }

        private Platform ConvertToPlatform(PlatformData platformData, float maxForce)
        {
            // Создаём Supports из объединённых InnerSupports и OuterSupports
            var allSupportPositions = platformData.InnerSupports.Concat(platformData.OuterSupports).ToArray();
            var supports = Supports.FromPositions(allSupportPositions); // Предполагаемый статический метод
            // Создаём массив Robot из позиций роботов
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