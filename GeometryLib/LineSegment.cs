namespace GeometryLib
{
    public struct LineSegment
    {
        public Vector2F A;
        public Vector2F B;

        public Vector2F Center => Lerp(0.5f);

        public float Magnitude => GetMagnitude();



        public LineSegment(Vector2F a, Vector2F b)
        {
            A = a;
            B = b;
        }



        /// <summary>
        /// Возвращает истину, если передаваемая точка совпадает с одним из концов отрезка
        /// </summary>
        public bool HasEndPoint(Vector2F point)
        {
            return point == A || point == B;
        }

        /// <summary>
        /// Возвращает истину, если хотя бы одна передаваемая точка принадлежит отрезку
        /// </summary>
        public bool HasAnyPoint(Vector2F[] points, bool exclusiveBounds = false)
        {
            for(int i = 0; i < points.Length; i++)
            {
                if(HasPoint(points[i], exclusiveBounds))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Возвращает истину, если передаваемая точка принадлежит отрезку
        /// </summary>
        public bool HasPoint(Vector2F point, bool exclusiveBounds = false)
        {
            if(exclusiveBounds && HasEndPoint(point))
                return false;

            Vector2F direction = B - A;
            if(Vector2F.Skew(direction, point - A) != 0)
                return false;

            Vector2F firstHalf = A - point;
            Vector2F secondHalf = B - point;
            return Vector2F.Dot(firstHalf, secondHalf) <= 0;
        }

        public Vector2F Lerp(float t)
        {
            return A + t * (B - A);
        }

        public float GetMagnitude()
        {
            return A.Distance(B);
        }
    }
}
