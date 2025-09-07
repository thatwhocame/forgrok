using ForceCalculationLib;
using ForceCalculationLib.Solvers;
using GeometryLib;

namespace ForceCalculation.Tests;

public class SolverHelperGetFarthestSupportTests
{
    public static IEnumerable<TestCaseData> GetFathestCases => FormTestCases();



    [TestCaseSource(nameof(GetFathestCases))]
    public void GetFarthestTest(Supports supports, StraightLine line, Support expected)
    {
        var result = SolverHelper.GetFarthestSupport(supports, line);
        Assert.That(result, Is.EqualTo(expected));
    }



    private static IEnumerable<TestCaseData> FormTestCases()
    {
        var testCases = new List<TestCaseData>();

        var supports = new Supports([new Vector2F(0, 0), new Vector2F(2, 2), new Vector2F(1, 4), new Vector2F(-3, 2)], []);
        testCases.Add(FormCase(supports, 0, 1, 3));
        testCases.Add(FormCase(supports, 1, 2, 3));
        testCases.Add(FormCase(supports, 2, 3, 0));
        testCases.Add(FormCase(supports, 3, 0, 2));

        supports = new Supports([new Vector2F(2, 3), new Vector2F(0, 6), new Vector2F(-3, 0)], []);
        testCases.Add(FormCase(supports, 0, 1, 2));
        testCases.Add(FormCase(supports, 1, 2, 0));
        testCases.Add(FormCase(supports, 2, 0, 1));

        return testCases;
    }

    private static TestCaseData FormCase(Supports supports, int linePoint1Index, int linePoint2Index, int farthestIndex)
    {
        var outerSupports = supports.OuterSupports;
        var line = StraightLine.FromPositions(outerSupports[linePoint1Index].Position, outerSupports[linePoint2Index].Position);
        return new TestCaseData(supports, line, outerSupports[farthestIndex]);
    }
}
