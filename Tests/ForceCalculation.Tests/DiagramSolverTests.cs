using ForceCalculationLib.Solvers;

namespace ForceCalculation.Tests;
public class DiagramSolverTests
{
	public static IEnumerable<TestCaseData> TestDiagramDatas
	{
		get
		{
			return [
				new TestCaseData(
					new Arm(1.55f),
					new Arm(0.45f),
					new Force[] {
						new Force(0, 20),
						new Force(1.45f, 20),
						new Force(2.55f, -100)
					},
					181f),

				new TestCaseData(
					new Arm(0.45f),
					new Arm(1.55f),
					new Force[] {
						new Force(0, 20),
						new Force(1.45f, 20),
						new Force(2.55f, -100)
					},
					-121f)
				];
		}
	}

	[TestCaseSource(nameof(TestDiagramDatas))]
	public void CalculateSupportReaction_Test(Arm supportToCalculate, Arm startingPoint, Force[] forces, float expected)
	{
		float result = DiagramSolver.CalculateSupportReaction(supportToCalculate, startingPoint, forces);

		bool isEqual = FloatComparer.IsApproximatelyEqual(expected, result);

		Assert.That(isEqual, Is.True, $"Ожидалось {expected}, но получено {result}");
	}
}
