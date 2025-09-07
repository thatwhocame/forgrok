using GeometryLib;

namespace ForceCalculationLib.Solvers;

public static class SolverHelper
{
    public static List<(Support First, Support Second)> CreateSolvingCases(Supports supports)
    {
        var cases = new List<(Support First, Support Second)>();
        for(int i = 0; i < supports.OuterSupports.Count() - 1; i++)
            cases.Add((supports.OuterSupports[i], supports.OuterSupports[i + 1]));
        cases.Add((supports.OuterSupports[^1], supports.OuterSupports[0]));
        return cases;
    }

    public static Support GetFarthestSupport(Supports supports, StraightLine line)
    {
        float maxDistance = 0;
        Support? farthestSupport = null;
        foreach(var outerSupport in supports.OuterSupports)
        {
            if(line.HasPoint(outerSupport.Position))
                continue;

            float distance = line.Distance(outerSupport.Position);
            if(distance > maxDistance)
            {
                maxDistance = distance;
                farthestSupport = outerSupport;
            }
        }

        if(farthestSupport == null)
            throw new Exception();

        return farthestSupport;
    }

    public static Arm GetArm(StraightLine baseLine, Vector2F position)
	{
		float projection = baseLine.GetSignedProjection(position);
		return new Arm(projection);
	}

	public static Force GetForce(StraightLine baseLine, Vector2F position, float value)
	{
		float projection = baseLine.GetSignedProjection(position);
		return new Force(projection, value);
	}

	public static ArmOld GetArmOld(StraightLine baseLine, Vector2F position)
    {
        float projection = baseLine.GetSignedProjection(position);
        return new ArmOld(projection);
    }

    public static ForceOld GetForceOld(StraightLine baseLine, Vector2F position, float value)
    {
        float projection = baseLine.GetSignedProjection(position);
        return new ForceOld(projection, value);
    }
}
