using GeometryLib;

namespace ForceCalculationLib.SupportsBuilders
{
    public class AllRandomSupportsBuilder : ISupportsBuilder
    {
        public Supports Build(int count, int seed)
        {
            Random random = new Random(seed);
            Vector2F[] points = new Vector2F[count];
            for(int i = 0; i < count; i++)
            {
                Vector2F point = new Vector2F();
                point.X = (float)random.NextDouble() * 2 - 1;
                point.Y = (float)random.NextDouble() * 2 - 1;
                points[i] = point;
            }

            return Supports.FromPositions(points);
        }
    }
}
