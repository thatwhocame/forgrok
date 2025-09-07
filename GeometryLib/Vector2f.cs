using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace GeometryLib
{
    [Serializable]
    public struct Vector2F
    {
        public float X { get; set; }
        public float Y { get; set; }


        [JsonIgnore]
        public float Magnitude => GetMagnitude();
        [JsonIgnore]
        public Vector2F Normalized => GetNormalized();



        public Vector2F()
        {
            X = 0;
            Y = 0;
        }

        public Vector2F(float x, float y)
        {
            X = x;
            Y = y;
        }



        public static Vector2F operator +(Vector2F lhs, Vector2F rhs)
        {
            return new Vector2F(lhs.X + rhs.X, lhs.Y + rhs.Y);
        }

        public static Vector2F operator -(Vector2F lhs, Vector2F rhs)
        {
            return new Vector2F(lhs.X - rhs.X, lhs.Y - rhs.Y);
        }

        public static Vector2F operator *(Vector2F v, float t)
        {
            return new Vector2F(v.X * t, v.Y * t);
        }

        public static Vector2F operator *(float t, Vector2F v)
        {
            return new Vector2F(v.X * t, v.Y * t);
        }

        public static Vector2F operator /(Vector2F v, float t)
        {
            return new Vector2F(v.X / t, v.Y / t);
        }


        public static bool operator ==(Vector2F lhs, Vector2F rhs)
        {
            return lhs.X == rhs.X && lhs.Y == rhs.Y;
        }

        public static bool operator !=(Vector2F lhs, Vector2F rhs)
        {
            return lhs.X != rhs.X || lhs.Y != rhs.Y;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals([NotNullWhen(true)] object? obj)
        {
            return base.Equals(obj);
        }



        public static float Dot(Vector2F lhs, Vector2F rhs)
        {
            return (lhs.X * rhs.X + lhs.Y * rhs.Y) / (float)Math.Sqrt((lhs.X * lhs.X + lhs.Y * lhs.Y) * (rhs.X * rhs.X + rhs.Y * rhs.Y));
        }

        public static float Skew(Vector2F lhs, Vector2F rhs)
        {
            return lhs.X * rhs.Y - rhs.X * lhs.Y;
        }



        public float Project(Vector2F other)
        {
            return (X * other.X + Y * other.Y) / (float)Math.Sqrt(other.X * other.X + other.Y * other.Y);
        }

        public float Distance(Vector2F other)
        {
            return (other - this).Magnitude;
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }



        private float GetMagnitude()
        {
            return (float)Math.Sqrt(X * X + Y * Y);
        }

        private Vector2F GetNormalized()
        {
            return this / (float)Math.Sqrt(X * X + Y * Y);
        }
    }
}
