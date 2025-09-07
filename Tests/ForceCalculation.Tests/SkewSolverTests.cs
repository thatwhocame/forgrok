using ForceCalculationLib;
using ForceCalculationLib.Solvers;
using GeometryLib;

namespace ForceCalculation.Tests;
public class SkewSolverTests
{
	public Platform InvalidPlatform
	{
		get
		{
			Vector2F[] supportPositions = [
				new Vector2F(-0.5f, -2f),
			new Vector2F(-2f, -1.5f),
			new Vector2F(-1.5f, -0.5f)
				];

			Vector2F[] robotPositions = [
				new Vector2F(-2f, -2f),
			new Vector2F(-0.5f, -1f)
				];

			Platform platform = new Platform(supportPositions, robotPositions, -100f);
			foreach(Robot robot in platform.Robots)
				robot.PushingForce = 20f;

			return platform;
		}
	}

	public Platform ValidPlatform
	{
		get
		{
			Vector2F[] supportPositions = [
				new Vector2F(2f, 3f),
				new Vector2F(1f, -2f),
				new Vector2F(-2f, 0f)
				];

			Vector2F[] robotPositions = [
				new Vector2F(2f, 0f),
				new Vector2F(-1f, 2f),
				new Vector2F(-1f, -2f)
				];

			Platform platform = new Platform(supportPositions, robotPositions, -100f);
			foreach(Robot robot in platform.Robots)
				robot.PushingForce = 20f;

			return platform;
		}
	}

	[Test]
	public void IsPlatformValid_Invalid_Test()
	{
		bool result = SkewSolver.IsPlatformValid(InvalidPlatform);
		Assert.That(result, Is.False);
	}

	[Test]
	public void IsPlatformValid_Valid_Test()
	{
		bool result = SkewSolver.IsPlatformValid(ValidPlatform);
		Assert.That(result, Is.True);
	}

	[Test]
	public void IsCaseValid_Invalid_Test()
	{
		Platform platform = InvalidPlatform;
		bool result = SkewSolver.IsCaseValid(platform, platform.Supports.OuterSupports[0], platform.Supports.OuterSupports[1]);
		Assert.That(result, Is.False);
	}
}
