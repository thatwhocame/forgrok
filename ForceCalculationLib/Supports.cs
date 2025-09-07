using GeometryLib;

namespace ForceCalculationLib
{
    public class Supports
    {
        public Support[] OuterSupports;
        public Support[] InnerSupports;



        public Supports(Support[] outerSupports, Support[] innerSupport)
        {
            OuterSupports = outerSupports;
            InnerSupports = innerSupport;
        }

        public Supports(Vector2F[] outerSupports, Vector2F[] innerSupports)
        {
            OuterSupports = outerSupports.Select((p, i) => new Support(p)).ToArray();
            InnerSupports = innerSupports.Select((p, i) => new Support(p)).ToArray();
        }



        public static Supports FromPositions(Vector2F[] positions)
        {
            ConvexPolygon polygon = new ConvexPolygon(positions);
            return FromPolygon(polygon);
        }

        public static Supports FromPolygon(ConvexPolygon polygon)
        {
            Support[] outerSupports = new Support[polygon.OuterPoints.Length];
            for(int i = 0; i < outerSupports.Length; i++)
                outerSupports[i] = new Support(polygon.OuterPoints[i]);

            Support[] innerSupports = new Support[polygon.InnerPoints.Length];
            for(int i = 0; i < innerSupports.Length; i++)
                innerSupports[i] = new Support(polygon.InnerPoints[i]);

            return new Supports(outerSupports, innerSupports);
        }



        public RectangleF GetBounds()
        {
            float minX = float.PositiveInfinity;
            float maxX = float.NegativeInfinity;
            float minY = float.PositiveInfinity;
            float maxY = float.NegativeInfinity;

            foreach(var support in OuterSupports)
            {
                if(support.Position.X > maxX)
                    maxX = support.Position.X;
                if(support.Position.X < minX)
                    minX = support.Position.X;

                if(support.Position.Y > maxY)
                    maxY = support.Position.Y;
                if(support.Position.Y < minY)
                    minY = support.Position.Y;
            }

            Vector2F position = new Vector2F(minX, minY);
            Vector2F size = new Vector2F(maxX - minX, maxY - minY);
            return new RectangleF(position, size);
        }
    }
}
