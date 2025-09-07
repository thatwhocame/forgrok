using GeometryLib;

namespace ForceCalculationLib.RobotsBuilders
{
    public class OneLineRobotsBuilder : IRobotsBuilder
    {
        public Robot[] Build(Supports supports, int count, int seed)
        {
            if(count == 0)
                return Array.Empty<Robot>();

            Random random = new Random(seed);

            RectangleF bounds = supports.GetBounds();
            float relativeOffset = (float)random.NextDouble() * 0.2f;
            bounds = bounds.Resize(1 + relativeOffset);

            Vector2F[] boundPoints = bounds.GetPoints();
            int firstIndex = random.Next(boundPoints.Length);
            int secondIndex = firstIndex + 1;
            if(secondIndex >= boundPoints.Length)
                secondIndex = 0;

            Vector2F firstPoint = boundPoints[firstIndex];
            Vector2F secondPoint = boundPoints[secondIndex];
            LineSegment segment = new LineSegment(firstPoint, secondPoint);

            if(count == 1)
                return [new Robot(segment.Center)];

            Robot[] robots = new Robot[count];
            for(int i = 0; i < robots.Length; i++)
            {
                Vector2F position = segment.Lerp((float)i / (count - 1));
                robots[i] = new Robot(position);
            }

            return robots;
        }
    }
}
