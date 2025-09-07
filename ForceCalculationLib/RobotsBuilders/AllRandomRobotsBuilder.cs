using GeometryLib;

namespace ForceCalculationLib.RobotsBuilders
{
    public class AllRandomRobotsBuilder : IRobotsBuilder
    {
        public Robot[] Build(Supports supports, int count, int seed)
        {
            if(count == 0)
				return [];

            Random random = new(seed);

            RectangleF bounds = supports.GetBounds();
            float relativeOffset = (float)random.NextDouble() * 0.2f;
            bounds = bounds.Resize(1 + relativeOffset);

            float xScale = bounds.Width;
            float yScale = bounds.Height;

            float xOffset = bounds.Left;
            float yOffset = bounds.Bottom;

            Robot[] robots = new Robot[count];
            for(int i = 0; i < count; i++)
            {
                Vector2F position = new Vector2F();
                position.X = (float)random.NextDouble() * xScale + xOffset;
                position.Y = (float)random.NextDouble() * yScale + yOffset;
                robots[i] = new Robot(position);
            }

            return robots;
        }
    }
}
