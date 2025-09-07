using GeometryLib;

namespace ForceCalculationLib.SupportsBuilders
{
    public class TwoLineSupportsBuilder : ISupportsBuilder
    {
        public Supports Build(int count, int seed)
        {
            Random random = new Random(seed);

            float width = ((float)random.NextDouble() + 0.1f) * 2f;
            float height = ((float)random.NextDouble() + 0.1f) * 2f;
            bool rotate = random.Next(2) == 1;
            int mirrorSign = random.Next(2) == 1 ? -1 : 1;

            bool oddSupportNumber = (count & 0x1) == 1;
            int supportPairsNumber = count / 2;

            float spacing = MathF.Min(height / (supportPairsNumber - 1), height);
            Vector2F[] supportPositions = new Vector2F[count];
            for(int i = 0; i < supportPairsNumber; i++)
            {
                Vector2F position1 = new Vector2F(-width / 2f,
                    mirrorSign * (spacing * i - height / 2f));
                Vector2F position2 = new Vector2F(width / 2f,
                    mirrorSign * (spacing * i - height / 2f));

                supportPositions[i * 2] = position1;
                supportPositions[i * 2 + 1] = position2;
            }

            if(oddSupportNumber)
                supportPositions[^1] = new Vector2F(0, mirrorSign * (height / 2f));

            if(rotate)
            {
                for(int i = 0; i < supportPositions.Length; i++)
                    (supportPositions[i].X, supportPositions[i].Y) =
                        (supportPositions[i].Y, supportPositions[i].X);
            }

            float widthShift = width * (((float)random.NextDouble() - 0.5f) * 0.6f);
            float heightShift = height * (((float)random.NextDouble() - 0.5f) * 0.6f);
            Vector2F shift = rotate ? new Vector2F(heightShift, widthShift) : new Vector2F(widthShift, heightShift);
            for(int index = 0; index < supportPositions.Length; index++)
                supportPositions[index] += shift;

            return Supports.FromPositions(supportPositions);
        }
    }
}