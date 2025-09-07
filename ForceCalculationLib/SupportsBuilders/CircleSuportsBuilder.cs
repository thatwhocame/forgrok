using GeometryLib;

namespace ForceCalculationLib.SupportsBuilders
{
    public class CircleSuportsBuilder : ISupportsBuilder
    {
        public Supports Build(int count, int seed)
        {
            Random random = new Random(seed);

            float radius = ((float)random.NextDouble() + 0.1f) * 2f;
            float shiftX = 0.5f * radius * ((float)random.NextDouble() * 2 - 1);
            float shiftY = 0.5f * radius * ((float)random.NextDouble() * 2 - 1);

            Circle circle = new Circle(new Vector2F(shiftX, shiftY), radius);

            Vector2F[] supportPositions = new Vector2F[count];
            for(int i = 0; i < count; i++)
            {
                float t = (float)i / (count);
                Vector2F position = circle.Lerp(t);
                supportPositions[i] = position;
            }

            return Supports.FromPositions(supportPositions);
        }
    }
}
