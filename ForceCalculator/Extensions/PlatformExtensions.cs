using System;
using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using ForceCalculationLib;
using GeometryLib;

namespace ForceCalculator.Extensions
{
    public static class PlatformExtensions
    {
        public static RenderTargetBitmap ToBitmap(this Platform platform, int size)
        {
            var bitmap = new RenderTargetBitmap(new PixelSize(size, size));
            int offset = 20;
            int innerSize = size - 2 * offset;

            float maxAxisValue = 0;
            foreach(var outerSupport in platform.Supports.OuterSupports)
            {
                Vector2F point = outerSupport.Position;
                maxAxisValue = Math.Max(maxAxisValue, Math.Abs(point.X));
                maxAxisValue = Math.Max(maxAxisValue, Math.Abs(point.Y));
            }
            foreach(var robot in platform.Robots)
            {
                Vector2F point = robot.Position;
                maxAxisValue = Math.Max(maxAxisValue, Math.Abs(point.X));
                maxAxisValue = Math.Max(maxAxisValue, Math.Abs(point.Y));
            }

            Point shift = new Point(maxAxisValue, maxAxisValue);

            float sizeModifier = (innerSize * 0.5f) / maxAxisValue;

            using(var context = bitmap.CreateDrawingContext(true))
            {
                context.FillRectangle(Brushes.White, new Rect(0, 0, size, size));

                Pen innerStrokePen = new Pen(Brushes.Black, 1);
                foreach(var innerSupport in platform.Supports.InnerSupports)
                {
                    Point point = Vector2ToPoint(innerSupport.Position, shift, sizeModifier, offset);
                    context.DrawEllipse(Brushes.Red, innerStrokePen, point, 14, 14);
                }

                Pen pen = new Pen(Brushes.Black, 2, new DashStyle([7, 4], 0));
                for(int i = 0; i < platform.Supports.OuterSupports.Length - 1; i++)
                {
                    Support support1 = platform.Supports.OuterSupports[i];
                    Point point1 = Vector2ToPoint(support1.Position, shift, sizeModifier, offset);
                    Support support2 = platform.Supports.OuterSupports[i + 1];
                    Point point2 = Vector2ToPoint(support2.Position, shift, sizeModifier, offset);
                    context.DrawLine(pen, point1, point2);
                }
                if(platform.Supports.OuterSupports.Length > 1)
                {
                    Support support1 = platform.Supports.OuterSupports[0];
                    Point point1 = Vector2ToPoint(support1.Position, shift, sizeModifier, offset);
                    Support support2 = platform.Supports.OuterSupports[^1];
                    Point point2 = Vector2ToPoint(support2.Position, shift, sizeModifier, offset);
                    context.DrawLine(pen, point1, point2);
                }

                Pen outerStrokePen = new Pen(Brushes.Black, 3);
                foreach(var outerSupport in platform.Supports.OuterSupports)
                {
                    Point point = Vector2ToPoint(outerSupport.Position, shift, sizeModifier, offset);
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



                foreach(var robot in platform.Robots)
                {
                    Point center = Vector2ToPoint(robot.Position, shift, sizeModifier, offset);
                    Point robotSize = new Point(12, 12);
                    Rect rect = new Rect(center - robotSize, center + robotSize);
                    context.DrawRectangle(Brushes.Green, outerStrokePen, rect);
                }
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
