using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using GeometryLib;

namespace ForceCalculator.Extensions
{
    public static class ConvexPolygonExtension
    {
        public static RenderTargetBitmap ToBitmap(this ConvexPolygon polygon, int size)
        {
            var bitmap = new RenderTargetBitmap(new PixelSize(size, size));
            int offset = 20;
            int innerSize = size - 2 * offset;

            float maxAxisValue = 0;
            foreach(var point in polygon.OuterPoints)
            {
                maxAxisValue = Math.Max(maxAxisValue, Math.Abs(point.X));
                maxAxisValue = Math.Max(maxAxisValue, Math.Abs(point.Y));
            }
            Point shift = new Point(maxAxisValue, maxAxisValue);

            float sizeModifier = (innerSize * 0.5f) / maxAxisValue;

            using(var context = bitmap.CreateDrawingContext(true))
            {
                context.FillRectangle(Brushes.White, new Rect(0, 0, size, size));

                Pen innerStrokePen = new Pen(Brushes.Black, 1);
                foreach(var vector in polygon.InnerPoints)
                {
                    Point point = Vector2ToPoint(vector, shift, sizeModifier, offset);
                    context.DrawEllipse(Brushes.Red, innerStrokePen, point, 14, 14);
                }

                Pen pen = new Pen(Brushes.Black, 2, new DashStyle([7, 4], 0));
                for(int i = 0; i < polygon.OuterPoints.Length - 1; i++)
                {
                    Point point1 = Vector2ToPoint(polygon.OuterPoints[i], shift, sizeModifier, offset);
                    Point point2 = Vector2ToPoint(polygon.OuterPoints[i + 1], shift, sizeModifier, offset);
                    context.DrawLine(pen, point1, point2);
                }
                if(polygon.OuterPoints.Length > 1)
                {
                    Point point1 = Vector2ToPoint(polygon.OuterPoints[0], shift, sizeModifier, offset);
                    Point point2 = Vector2ToPoint(polygon.OuterPoints[^1], shift, sizeModifier, offset);
                    context.DrawLine(pen, point1, point2);
                }

                Pen outerStrokePen = new Pen(Brushes.Black, 3);
                foreach(var vector in polygon.OuterPoints)
                {
                    Point point = Vector2ToPoint(vector, shift, sizeModifier, offset);
                    context.DrawEllipse(Brushes.Blue, outerStrokePen, point, 14, 14);
                }

                Pen gravityPen = new Pen(Brushes.Black, 3);
                Point gravityPoint = Vector2ToPoint(new Vector2F(), shift, sizeModifier, offset);
                context.DrawEllipse(null, gravityPen, gravityPoint, 25, 25);

                Point gravityTopLeft = gravityPoint + new Point(-12, 12);
                Point gravityTopRigth = gravityPoint + new Point(12, 12);
                Point gravityBottomLeft = gravityPoint + new Point(-12, -12);
                Point gravityBottomRigth = gravityPoint + new Point(12, -12);

                context.DrawLine(gravityPen, gravityTopLeft, gravityBottomRigth);
                context.DrawLine(gravityPen, gravityTopRigth, gravityBottomLeft);
            }

            return bitmap;
        }

        private static Point Vector2ToPoint(Vector2F vector, Point shift, float sizeModifier, float offset)
        {
            float x = offset + (vector.X + (float)shift.X) * sizeModifier;
            float y = offset + (vector.Y + (float)shift.Y) * sizeModifier;
            return new Point(x, y);
        }
    }
}
