namespace ForceCalculation.Tests;
public static class FloatComparer
{
	public static bool IsApproximatelyEqual(float lhs, float rhs, float maxRelativeError = 0.01f)
	{
		lhs = Math.Abs(lhs);
		rhs = Math.Abs(rhs);
		return lhs - rhs < Math.Max(lhs, rhs) * maxRelativeError;
	}
}
