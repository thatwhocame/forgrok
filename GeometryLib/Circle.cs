namespace GeometryLib
{
    public class Circle
    {
        public Vector2F Center;
        public float Radius;



        public Circle(Vector2F center, float radius)
        {
            Center = center;
            Radius = radius;
        }



        public Vector2F Lerp(float t)
        {
            float angle = t * MathF.PI * 2f;
            float x = Center.X + MathF.Cos(angle) * Radius;
            float y = Center.Y + MathF.Sin(angle) * Radius;
            return new Vector2F(x, y);
        }
    }
}
