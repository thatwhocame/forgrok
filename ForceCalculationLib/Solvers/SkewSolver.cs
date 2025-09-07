using GeometryLib;

namespace ForceCalculationLib.Solvers;
public static class SkewSolver
{
	/// <summary>
	/// Проверяет положительность всех сил роботов и наличие перекоса в текущей конфигурации платформы.
	/// Перед передачей платформы в этот метод, её роботам должны быть присвоены силы давления.
	/// </summary>
	public static bool IsPlatformValid(Platform platform)
	{
		if(platform.Robots.Any(r => r.PushingForce < 0))
			return false;

		var cases = SolverHelper.CreateSolvingCases(platform.Supports);

		foreach(var supportPair in cases)
		{
			if(!IsCaseValid(platform, supportPair.First, supportPair.Second))
				return false;
		}

		return true;
	}



	public static bool IsCaseValid(Platform platform, Support firstSupport, Support secondSupport)
	{

		Diagram diagram = Diagram.FromPlatform(platform, firstSupport, secondSupport);
		float reaction = DiagramSolver.CalculateSupportReaction(diagram.Supports[0], diagram.Supports[1], diagram.GetAllForces());

		return reaction >= -0.001f;
	}
}
