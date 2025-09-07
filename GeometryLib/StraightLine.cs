namespace GeometryLib
{
    public struct StraightLine
    {
        public float A { get; private set; }
        public float B { get; private set; }
        public float C { get; private set; }

        public Vector2F Normal => new Vector2F(A, B);
        public Vector2F Direction => new Vector2F(-B, A);



        public StraightLine(float a, float b, float c)
        {
            A = a;
            B = b;
            C = c;
        }

        public static StraightLine FromDirection(Vector2F point, Vector2F direction)
        {
            return new StraightLine {
                A = direction.Y,
                B = -direction.X,
                C = direction.X * point.Y - direction.Y * point.X
            };
        }

        public static StraightLine FromNormal(Vector2F point, Vector2F normal)
        {
            return new StraightLine {
                A = normal.X,
                B = normal.Y,
                C = -(normal.X * point.X + normal.Y + point.Y)
            };
        }

        public static StraightLine FromPositions(Vector2F point1, Vector2F point2)
        {
            return new StraightLine {
                A = point2.Y - point1.Y,
                B = point1.X - point2.X,
                C = point1.Y * (point2.X - point1.X) + point1.X * (point1.Y - point2.Y)
            };
        }

        public static StraightLine FromLineSegment(LineSegment lineSegment)
        {
            return new StraightLine {
                A = lineSegment.B.Y - lineSegment.A.Y,
                B = lineSegment.A.X - lineSegment.B.X,
                C = lineSegment.A.Y * (lineSegment.B.X - lineSegment.A.X) + lineSegment.A.X * (lineSegment.A.Y - lineSegment.B.Y)
            };
        }



        /// <summary>
        /// Возвращает истину, если передаваемая точка принадлежит прямой
        /// </summary>
        public bool HasPoint(Vector2F point)
        {
            return Solve(point) == 0;
        }

        /// <summary>
        /// Возвращает истину, если хотя бы одна передаваемая точка принадлежит прямой
        /// </summary>
        public bool HasAnyPoint(params Vector2F[] points)
        {
            for(int i = 0; i < points.Length; i++)
            {
                if(Solve(points[i]) == 0)
                    return true;
            }

            return false;
        }

        public float Solve(Vector2F point)
        {
            return A * point.X + B * point.Y + C;
        }

        public float Solve(float x, float y)
        {
            return A * x + B * y + C;
        }

        public Vector2F GetIntersection(StraightLine other)
        {
            Vector2F intersection = new Vector2F();
            intersection.X = (B * other.C - other.B * C) / (A * other.B - other.A * B);
            intersection.Y = -(A * other.C - other.A * C) / (A * other.B - other.A * B);
            return intersection;
        }

        public Vector2F GetProjeсtion(Vector2F point)
        {
            StraightLine normalLine = FromDirection(point, Normal);
            return GetIntersection(normalLine);
        }

        public float GetSignedProjection(Vector2F point)
        {
            return point.Project(Direction);
        }

        public float Distance(Vector2F point)
        {
            return MathF.Abs((A * point.X + B * point.Y + C) / MathF.Sqrt(A * A + B * B));
        }
    }
}
