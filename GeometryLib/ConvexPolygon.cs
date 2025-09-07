namespace GeometryLib
{
    public class ConvexPolygon
    {
        public Vector2F[] OuterPoints;
        public Vector2F[] InnerPoints;



        public ConvexPolygon(Vector2F[] points)
        {
            ConvexPolygonBuilder builder = new (points);
            (OuterPoints, InnerPoints) = builder.GetPoints();
        }
    }
}
