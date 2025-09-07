using GeometryLib;

namespace ForceCalculationLib.RobotsBuilders
{
    public class TwoLineRobotsBuilder : IRobotsBuilder
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
            int[] indexes = new int[boundPoints.Length];
            indexes[0] = random.Next(4);
            for(int i = 1; i < indexes.Length; i++)
            {
                indexes[i] = indexes[i - 1] + 1;
                if(indexes[i] >= indexes.Length)
                    indexes[i] = 0;
            }

            LineSegment[] segments = new LineSegment[2];
            for(int i = 0; i < segments.Length; i++)
            {
                Vector2F firstPoint = boundPoints[indexes[i * 2]];
                Vector2F secondPoint = boundPoints[indexes[i * 2 + 1]];
                segments[i] = new LineSegment(firstPoint, secondPoint);
            }

            Robot[] allRobots = new Robot[count];
            int countHalf = count / 2;
            Span<Robot> robotsLine0 = new Span<Robot>(allRobots, 0, countHalf);
            Span<Robot> robotsLine1 = new Span<Robot>(allRobots, countHalf, count - countHalf); 
            for(int i = 0; i < segments.Length; i++)
            {
                var robotsLine = i == 0 ? robotsLine0 : robotsLine1;
                var lineSegment = segments[i];

                if(robotsLine.Length == 0)
                    continue;

                if(robotsLine.Length == 1)
                {
                    robotsLine[0] = new Robot(lineSegment.Center);
                    continue;
                }

                for(int j = 0; j < robotsLine.Length; j++)
                {
                    Vector2F position = lineSegment.Lerp((float)j / (robotsLine.Length - 1));
                    robotsLine[j] = new Robot(position);
                }
            }

            return allRobots;
        }
    }
}
