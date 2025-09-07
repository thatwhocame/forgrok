using GeometryLib;

namespace ForceCalculationLib.Solvers;
/// <summary>
/// Эпюра сил. Хранит абсолютные положения и значения всех сил, а также абсолютные положения опор.
/// </summary>
public class Diagram
{
	public Arm[] Supports;
	public Force GravityForce;
	public Force[] RobotForces;



	public Diagram(Arm[] supports, Force gravityForce, Force[] robotForces)
	{
		if(supports.Length != 2)
			throw new ArgumentException("Число опор должно быть равно 2.");

		Supports = supports;
		GravityForce = gravityForce;
		RobotForces = robotForces;
	}

	public static Diagram FromPlatform(Platform platform, Support firstSupport, Support secondSupport)
	{
		//Построении прямой, соединяющей две соседние опоры
		StraightLine supportLine = StraightLine.FromPositions(firstSupport.Position, secondSupport.Position);

		//Определение дальней от это прямой опоры
		Support farthestSupport = SolverHelper.GetFarthestSupport(platform.Supports, supportLine);

		//Построение прямой сечения, которая является перпендикуляром от к дальней опоры к прямой первоначальных опор
		StraightLine crossSectionLine = StraightLine.FromDirection(farthestSupport.Position, supportLine.Normal);

		//Определение точки пересечения этих двух прямых
		//Это будет точка представляющая положение двух опор на сечении
		Vector2F baseSupportPosition = crossSectionLine.GetIntersection(supportLine);
		//Проецирование получившуюся точку на прямую сечения
		Arm baseSuportArm = SolverHelper.GetArm(crossSectionLine, baseSupportPosition);

		//То же с отдалённой опорой
		Vector2F otherSupportPosition = crossSectionLine.GetProjeсtion(farthestSupport.Position);
		Arm otherSupportArm = SolverHelper.GetArm(crossSectionLine, otherSupportPosition);

		Arm[] arms = [baseSuportArm, otherSupportArm];

		//То же с точкой приложения силы тяжести
		Vector2F gravityPosition = crossSectionLine.GetProjeсtion(new Vector2F());
		Force gravityForce = SolverHelper.GetForce(crossSectionLine, gravityPosition, platform.GravityForce);

		//То же с позициями роботов	
		List<Force> robotForces = [];
		for(int i = 0; i < platform.Robots.Length; i++)
		{
			Robot robot = platform.Robots[i];
			Force robotForce = SolverHelper.GetForce(crossSectionLine, robot.Position, robot.PushingForce);
			robotForces.Add(robotForce);
		}

		return new Diagram(arms, gravityForce, [.. robotForces]);
	}



	public Force[] GetAllForces()
	{
		return [.. RobotForces.Prepend(GravityForce)];
	}
}
