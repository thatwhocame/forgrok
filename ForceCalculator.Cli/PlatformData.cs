using GeometryLib;

namespace ForceCalculator.Cli;
[Serializable]
public class PlatformData(
    float gravity,
    List<Vector2F> innerSupports,
    List<Vector2F> outerSupports,
    List<Vector2F> robots)
{
    public float Gravity { get; set; } = gravity;
    public List<Vector2F> InnerSupports { get; set; } = innerSupports;
    public List<Vector2F> OuterSupports { get; set; } = outerSupports;
    public List<Vector2F> Robots { get; set; } = robots;
}