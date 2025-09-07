using GeometryLib;

namespace Geometry.Tests;

public class StraightLineTests
{
    public static IEnumerable<TestCaseData> GetDistanceCases => FormGetDistanceCases();
    public static IEnumerable<TestCaseData> GetDistanceSignRandomCases => FormGetDistanceSignRandomCases();




    [TestCaseSource(nameof(GetDistanceCases))]
    public void GetDistanceTest(StraightLine line, Vector2F point, float expectedDistance)
    {
        Assert.That(line.Distance(point), Is.EqualTo(expectedDistance));
    }

    [TestCaseSource(nameof(GetDistanceSignRandomCases))]
    public void GetDistanceSignRandomTest(StraightLine line, Vector2F point)
    {
        Assert.That(line.Distance(point), Is.Positive);
    }



    private static IEnumerable<TestCaseData> FormGetDistanceCases()
    {
        var cases = new List<TestCaseData>();

        var line = StraightLine.FromPositions(new Vector2F(0, 0), new Vector2F(1, 0));
        cases.Add(new TestCaseData(line, new Vector2F(0, 6), 6f));
        cases.Add(new TestCaseData(line, new Vector2F(0, -192), 192f));
        cases.Add(new TestCaseData(line, new Vector2F(0, 0), 0f));
        cases.Add(new TestCaseData(line, new Vector2F(0, 87), 87f));

        line = StraightLine.FromPositions(new Vector2F(0, 0), new Vector2F(0, -1));
        cases.Add(new TestCaseData(line, new Vector2F(16, 0), 16f));
        cases.Add(new TestCaseData(line, new Vector2F(-76, 0), 76f));
        cases.Add(new TestCaseData(line, new Vector2F(-23498, 0), 23498f));
        cases.Add(new TestCaseData(line, new Vector2F(123, 0), 123f));

        return cases;
    }

    private static IEnumerable<TestCaseData> FormGetDistanceSignRandomCases()
    {
        Random random = new Random(9182374);
        var cases = new List<TestCaseData>();

        while(cases.Count < 100)
        {
            var line = StraightLine.FromPositions(RandomPosition(random), RandomPosition(random));
            cases.Add(new TestCaseData(line, RandomPosition(random)));
        }

        return cases;
    }

    private static Vector2F RandomPosition(Random random)
    {
        float x = (float)random.NextDouble() * random.Next(int.MinValue, int.MaxValue);
        float y = (float)random.NextDouble() * random.Next(int.MinValue, int.MaxValue);
        return new Vector2F(x, y);
    }
}
