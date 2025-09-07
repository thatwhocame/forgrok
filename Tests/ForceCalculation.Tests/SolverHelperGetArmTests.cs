using ForceCalculationLib.Solvers;
using GeometryLib;

namespace ForceCalculation.Tests;

public class SolverHelperGetArmTests
{
    public static IEnumerable<TestCaseData> ArmTestCases
    {
        get
        {
            return new List<TestCaseData> {
                new TestCaseData(StraightLine.FromPositions(new Vector2F(0, 0), new Vector2F(10, 10)), new Vector2F(0, 10), 5 * MathF.Sqrt(2)),
                new TestCaseData(StraightLine.FromPositions(new Vector2F(0, 0), new Vector2F(-10, -10)), new Vector2F(0, 10), -5 * MathF.Sqrt(2))
            };
        }
    }



    [TestCaseSource(nameof(ArmTestCases))]
    public void GetArmTest(StraightLine baseLine, Vector2F position, float expected)
    {
        Assert.That(expected, Is.EqualTo(SolverHelper.GetArmOld(baseLine, position).Position));
    }
}
