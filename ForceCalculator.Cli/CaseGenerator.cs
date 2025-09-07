using ForceCalculationLib;
using ForceCalculationLib.RobotsBuilders;
using ForceCalculationLib.Solvers;
using ForceCalculationLib.SupportsBuilders;
using GeometryLib;

namespace ForceCalculator.Cli;
public class CaseGenerator(
    float gravity,
    PlatformType platformType,
    int supportsNumber,
    RobotsType robotsType,
    int robotsNumber)
{
    private readonly ISupportsBuilder _supportsBuilder = platformType switch
    {
        PlatformType.AllRandom => new AllRandomSupportsBuilder(),
        PlatformType.Circle => new CircleSuportsBuilder(),
        PlatformType.TwoLine => new TwoLineSupportsBuilder(),
        _ => throw new NotSupportedException()
    };

    private readonly IRobotsBuilder _robotsBuilder = robotsType switch
    {
        RobotsType.AllRandom => new AllRandomRobotsBuilder(),
        RobotsType.Circle => new CircleRobotsBuilder(),
        RobotsType.OneLine => new OneLineRobotsBuilder(),
        RobotsType.TwoLine => new TwoLineRobotsBuilder(),
        RobotsType.Perimeter => new PerimeterRobotsBuilder(),
        _ => throw new NotSupportedException()
    };

    private readonly Random _random = new();

    public CaseData Next()
    {
        var supports = _supportsBuilder.Build(supportsNumber, _random.Next());
        var robots = _robotsBuilder.Build(supports, robotsNumber, _random.Next());

        Platform platform = new(supports, robots, gravity);

        List<Vector2F> innerSupportsPositions = platform.Supports.InnerSupports.Select(s => s.Position).ToList();
        List<Vector2F> outerSupportPositions = platform.Supports.OuterSupports.Select(s => s.Position).ToList();
        List<Vector2F> robotPositions = platform.Robots.Select(r => r.Position).ToList();
        PlatformData platformData = new(gravity, innerSupportsPositions, outerSupportPositions, robotPositions);

        EqualMaxSolver equalMaxSolver = new();
        equalMaxSolver.Solve(platform);
        List<float> equalRobotForces = platform.Robots.Select(r => r.PushingForce).ToList();
        float equalRobotForcesSum = equalRobotForces.Sum();
        bool isEqualValid = SkewSolver.IsPlatformValid(platform);

        SimplexSolver simplexSolver = new();
        simplexSolver.Solve(platform);
        List<float> individualRobotForces = platform.Robots.Select(r => r.PushingForce).ToList();
        float individualRobotForcesSum = individualRobotForces.Sum();
        bool isIndividualValid = SkewSolver.IsPlatformValid(platform);

        return new CaseData
        {
            Platform = platformData,
            IsEqualSolvedValid = isEqualValid,
            EqualRobotForceSum = equalRobotForcesSum,
            EqualRobotForces = equalRobotForces,
            IsIndividualSolvedValid = isIndividualValid,
            IndividualRobotForceSum = individualRobotForcesSum,
            IndividualRobotForces = individualRobotForces,
        };
    }
}