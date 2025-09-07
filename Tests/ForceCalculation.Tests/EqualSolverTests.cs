using ForceCalculationLib;
using ForceCalculationLib.Solvers;
using GeometryLib;

namespace ForceCalculation.Tests;
public class EqualSolverTests
{
    public EqualMaxSolver EqualSolver = new();

    public static IEnumerable<TestCaseData> TestPlatforms
    {
        get
        {
            return new List<TestCaseData>() {
                new TestCaseData(new Platform(
                    [new Vector2F(1, 1),  new Vector2F(1, -1),  new Vector2F(-1, -1),  new Vector2F(-1, 1)], 
                    [new Vector2F(1.5f, 0), new Vector2F(-1.5f, 0), new Vector2F(0, 1.5f), new Vector2F(0, -1.5f)], 100), 
                    10, 
                    80),

                new TestCaseData(new Platform(
                    [new Vector2F(1, 1),  new Vector2F(1, -1),  new Vector2F(-1, -1),  new Vector2F(-1, 1)],
                    [new Vector2F(1.5f, 0), new Vector2F(-1.5f, 0), new Vector2F(0, 1.5f), new Vector2F(0, -1.5f)], 100),
                    10,
                    80),
            };
        }
    }
     
    public static IEnumerable<TestCaseData> CalculateMaxForceSuccessCases
    {
        get
        {
            return new List<TestCaseData>() {
                new TestCaseData(
                    new ArmOld(-2),
                    new ArmOld(6),
                    new ForceOld(2, 100),
                    new ArmOld[] { new ArmOld(-1), new ArmOld(4), new ArmOld(5) },
                    10.0f,
                    SimpleCalculateMaxForce(8, [1, 6, 7], 10, -100, 4)),

                new TestCaseData(
                    new ArmOld(6),
                    new ArmOld(-2),
                    new ForceOld(2, 100),
                    new ArmOld[] { new ArmOld(-1), new ArmOld(4), new ArmOld(5) },
                    10.0f,
                    SimpleCalculateMaxForce(-8, [-1, -2, -7], 10, -100, -4))
            };
        }
    }

    public static IEnumerable<TestCaseData> CalculateMaxForceFailCases
    {
        get
        {
            return new List<TestCaseData>() {
                new TestCaseData(
                    new ArmOld(2),
                    new ArmOld(8),
                    new ForceOld(9, 100),
                    new ArmOld[] { new ArmOld(1), new ArmOld(5), new ArmOld(5), new ArmOld(7) },
                    10.0f,
                    SimpleCalculateMaxForce(6, [-1, 3, 3, 5], 10, -100, 7)),

                new TestCaseData(
                    new ArmOld(8),
                    new ArmOld(2),
                    new ForceOld(9, 100),
                    new ArmOld[] { new ArmOld(1), new ArmOld(5), new ArmOld(5), new ArmOld(7) },
                    10.0f,
                    SimpleCalculateMaxForce(-6, [-1, -3, -3, -7], 10, -100, 1))
            };
        }
    }



    [TestCaseSource(nameof(TestPlatforms))]
    public void SolveTest(Platform platform, float minN, float pushForce)
    {
        Assert.That(pushForce, Is.EqualTo(EqualSolver.Solve(platform)));
    }

    [TestCaseSource(nameof(CalculateMaxForceSuccessCases))]
    [TestCaseSource(nameof(CalculateMaxForceFailCases))]
    public void CalculateMaxRobotForceSuccessTest(ArmOld baseSupport, ArmOld targetSupport, ForceOld gravity, ArmOld[] robots, float minN, float expectedForce)
    {
        float result = EqualSolver.CalculateMaxRobotForce(baseSupport, targetSupport, gravity, robots, minN);
        Assert.That(result, Is.EqualTo(expectedForce), $"Получено {result}, но ожидалось {expectedForce}");
    }



    public static float SimpleCalculateMaxForce(float targetArm, float[] robotArms, float minN, float gravity, float gravityArm)
    {
        return -(gravity * gravityArm + minN * targetArm) / robotArms.Aggregate((a, r) => a + r);
    }
}
