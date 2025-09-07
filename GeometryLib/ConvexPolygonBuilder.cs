namespace GeometryLib
{
    internal class ConvexPolygonBuilder
    {
        private Vector2F[] _points;



        public ConvexPolygonBuilder(Vector2F[] points)
        {
            _points = points;
        }



        public (Vector2F[] outerPoints, Vector2F[] innerPoints) GetPoints()
        {
            LineSegment[] outerSegments = GetOuterLineSegments();
            Vector2F[] outerPoints = new Vector2F[outerSegments.Length];

            outerPoints[0] = outerSegments[0].A;
            for(int i = 1; i < outerPoints.Length; i++)
            {
                bool pairFound = false;
                for(int j = 0; j < outerSegments.Length && !pairFound; j++)
                {
                    if(outerPoints[i - 1] == outerSegments[j].A && !outerPoints.Contains(outerSegments[j].B))
                    {
                        outerPoints[i] = outerSegments[j].B;
                        pairFound = true;
                    }
                    else if(outerPoints[i - 1] == outerSegments[j].B && !outerPoints.Contains(outerSegments[j].A))
                    {
                        outerPoints[i] = outerSegments[j].A;
                        pairFound = true;
                    }
                }

                if(!pairFound)
                    throw new Exception();
            }

            Vector2F[] innerPoints = new Vector2F[_points.Length - outerPoints.Length];
            int index = 0;
            for(int i = 0; i < _points.Length; i++)
            {
                if(!outerPoints.Contains(_points[i]))
                    innerPoints[index++] = _points[i];
            }

            return (outerPoints, innerPoints);
        }



        //Тут есть проблема, когда три внешние точки лежат на одной прямой.
        //В этом случае создаётся три сегмента треугольником и ломается
        //дальнейшее выполнение программы
        private LineSegment[] GetOuterLineSegments()
        {
            List<LineSegment> outerSegments = new List<LineSegment>();

            for(int i = 0; i < _points.Length; i++)
            {
                for(int j = i + 1; j < _points.Length; j++)
                {
                    LineSegment segment = new LineSegment(_points[i], _points[j]);
                    if(IsOuterSegment(segment))
                        outerSegments.Add(segment);
                }
            }

            return outerSegments.ToArray();
        }

        private bool IsOuterSegment(LineSegment segment)
        {
            StraightLine line = StraightLine.FromLineSegment(segment);

            int sign = 0;
            for(int i = 0; i < _points.Length; i++)
            {
                Vector2F point = _points[i];

                if(segment.HasEndPoint(point))
                    continue;

                double number = line.Solve(point);
                int pointSign = Math.Sign(number);

                if(pointSign == 0)
                {
                    if(segment.HasPoint(point))
                        continue;
                    else 
                        return false;
                }

                if(sign == 0)
                    sign = pointSign;
                else if(sign != pointSign)
                    return false;
            }

            return true;
        }
    }
}
