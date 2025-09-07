using GeometryLib;

namespace ForceCalculationLib.RobotsBuilders
{
    public class CircleRobotsBuilder : IRobotsBuilder
    {
        public Robot[] Build(Supports supports, int count, int seed)
        {
            if(count == 0)
                return Array.Empty<Robot>();

            Random random = new Random(seed);

            RectangleF bounds = supports.GetBounds();
            float relativeOffset = -((float)random.NextDouble() + 0.5f) * 0.35f;
            bounds = bounds.Resize(1 + relativeOffset);

            float radius = MathF.Max(bounds.Width, bounds.Height);
            Circle circle = new Circle(bounds.Center, radius);

            if(count == 1)
                return [new Robot(circle.Lerp(0))];

            Robot[] robots = new Robot[count];
            for(int i = 0; i < count; i++)
            {
                float t = (float)i / (count);
                Vector2F position = circle.Lerp(t);
                robots[i] = new Robot(position);
            }

            return robots;
        }
    }
}
