namespace ForceCalculationLib.Solvers;
public static class DiagramSolver
{
	/// <summary>
	/// Рассчитывает силы реакии опор для предоставленной диаграммы.
	/// </summary>
	public static float[] CalculateSupportReactions(Diagram diagram)
	{
		float firstReaction = CalculateSupportReaction(diagram.Supports[0], diagram.Supports[1], diagram.GetAllForces());
		float secondReaction = CalculateSupportReaction(diagram.Supports[0], diagram.Supports[1], diagram.GetAllForces());
		return [firstReaction, secondReaction];
	}

	

	/// <summary>
	/// Рассчитывает силу реакции опоры с помощью приравнивания суммы моментов к нулю.
	/// </summary>
	/// <param name="supportToCalculate">Опора, для которой необходимо найти силу реакции.</param>
	/// <param name="startingPoint">Опора, относительно которой будут считаться моменты.</param>
	/// <param name="allForces">Все силы.</param>
	public static float CalculateSupportReaction(Arm supportToCalculate, Arm startingPoint, Force[] allForces)
	{
		float shift = -startingPoint.Position;

		supportToCalculate.Position += shift;

		Force[] adjustedForces = new Force[allForces.Length];
		for(int i = 0; i < allForces.Length; i++)
		{
			Force force = allForces[i];
			adjustedForces[i] = new Force(force.Position + shift, force.Value);
		}

		float sumOfMoments = 0;
		foreach(Force force in adjustedForces)
			sumOfMoments += force.Value * force.Position;

		float reaction = -sumOfMoments / supportToCalculate.Position;
		return reaction;
	}
}
