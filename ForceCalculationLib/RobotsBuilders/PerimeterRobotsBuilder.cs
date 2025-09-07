using GeometryLib;

namespace ForceCalculationLib.RobotsBuilders
{
    public class PerimeterRobotsBuilder : IRobotsBuilder
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
                if(indexes[i] >= 4)
                    indexes[i] = 0;
            }

            LineSegment[] segments = new LineSegment[4];
            Vector2F firstPoint = boundPoints[indexes[3]];
            Vector2F secondPoint = boundPoints[indexes[0]];
            segments[3] = new LineSegment(firstPoint, secondPoint);
            for(int i = 0; i < 3; i++)
            {
                firstPoint = boundPoints[indexes[i]];
                secondPoint = boundPoints[indexes[i + 1]];
                segments[i] = new LineSegment(firstPoint, secondPoint);
            }
            if(segments[1].Magnitude > segments[0].Magnitude)
            {
                (segments[0], segments[1]) = (segments[1], segments[0]);
                (segments[2], segments[3]) = (segments[3], segments[2]);
            }

            int[] halfs = [count / 2, count - count / 2];
            int[] counts = new int[4];
            float width = bounds.Width;
            float height = bounds.Height;
            float max = Math.Max(width, height);
            float min = Math.Min(width, height);
            counts[0] = (int)Math.Round(halfs[0] * max / (min + max));
            counts[1] = (int)Math.Round(halfs[0] * min / (min + max));
            counts[2] = (int)Math.Round(halfs[1] * max / (min + max));
            counts[3] = (int)Math.Round(halfs[1] * min / (min + max));

            if(counts[1] > 0)
            {
                counts[1]--;
                counts[0]++;
            }
            if(counts[3] > 0)
            {
                counts[3]--;
                counts[2]++;
            }

            Robot[] allRobots = new Robot[count];
            Span<Robot> lineWithCorners0 = new Span<Robot>(allRobots, 0, counts[0]);
            Span<Robot> lineWithCorners1 = new Span<Robot>(allRobots, counts[0] + counts[1], counts[2]);
            for(int i = 0; i < 2; i++)
            {
                var robotLine = i == 0 ? lineWithCorners0 : lineWithCorners1;
                var segment = i == 0 ? segments[0] : segments[2];

                if(robotLine.Length == 0)
                    continue;

                if(robotLine.Length == 1)
                {
                    robotLine[0] = new Robot(segment.Center);
                    continue;
                }

                for(int j = 0; j < robotLine.Length; j++)
                {
                    Vector2F position = segment.Lerp((float)j / (robotLine.Length - 1));
                    robotLine[j] = new Robot(position);
                }
            }


            Span<Robot> lineWithoutCorners0 = new Span<Robot>(allRobots, counts[0], counts[1]);
            Span<Robot> lineWithoutCorners1 = new Span<Robot>(allRobots, counts[0] + counts[1] + counts[2], counts[3]); for(int i = 0; i < 2; i++)
            {
                var robotLine = i == 0 ? lineWithoutCorners0 : lineWithoutCorners1;
                var segment = i == 0 ? segments[1] : segments[3];

                if(robotLine.Length == 0)
                    continue;

                if(robotLine.Length == 1)
                {
                    robotLine[0] = new Robot(segment.Center);
                    continue;
                }

                for(int j = 0; j < robotLine.Length; j++)
                {
                    Vector2F position = segment.Lerp((float)(j + 1) / (robotLine.Length + 1));
                    robotLine[j] = new Robot(position);
                }
            }

            return allRobots;
        }
    }
}
