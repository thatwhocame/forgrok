namespace GeometryLib
{
    public struct RectangleF
    {
        /// <summary>Координата X пивота</summary>
        public float X;
        /// <summary>Координата Y пивота</summary>
        public float Y;

        /// <summary>Ширина</summary>
        public float Width;
        /// <summary>Высота</summary>
        public float Height;

        /// <summary>Пивот - относительное положение "центра масс" объекта. 
        /// X и Y задаются в форме от 0 до 1 и означают положение "центра масс"
        /// относительно ширины и высоты
        /// </summary>
        public Vector2F Pivot;



        /// <summary>Координата X левого края</summary>
        public float Left => X - Width * Pivot.X;
        /// <summary>Координата X правого края</summary>
        public float Right => X + Width * (1 - Pivot.X);
        /// <summary>Координата X середины</summary>
        public float CenterX => X - Width * Pivot.X + Width / 2f;

        /// <summary>Координата Y нижней границы</summary>
        public float Bottom => Y - Height * (1 - Pivot.Y);
        /// <summary>Координата Y верхей границы</summary>
        public float Top => Y + Height * Pivot.Y;
        /// <summary>Координата Y середины</summary>
        public float CenterY => Y - Height * Pivot.Y + Height / 2f;

        /// <summary>Левый верхний угол</summary>
        public Vector2F LeftTop => new Vector2F(Left, Top);
        /// <summary>Левый нижний угол</summary>
        public Vector2F LeftBottom => new Vector2F(Left, Bottom);
        /// <summary>Правый верхний угол</summary>
        public Vector2F RightTop => new Vector2F(Right, Top);
        /// <summary>Правый нижний угол</summary>
        public Vector2F RightBottom => new Vector2F(Right, Bottom);

        /// <summary>Середина левой границы</summary>
        public Vector2F LeftCenter => new Vector2F(Left, CenterY);
        /// <summary>Середина верхней границы</summary>
        public Vector2F CenterTop => new Vector2F(CenterX, Top);
        /// <summary>Середина правой границы</summary>
        public Vector2F RightCenter => new Vector2F(Right, CenterY);
        /// <summary>Середина нижней границы</summary>
        public Vector2F CenterBottom => new Vector2F(CenterX, Bottom);

        /// <summary>Середина</summary>
        public Vector2F Center => new Vector2F(CenterX, CenterY);



        /// <summary>
        /// RectangleF на основе левого нижнего угла и размера
        /// </summary>
        /// <param name="position">Абсолютная позиция левого нижнего угла прямоугольника</param>
        /// <param name="size">Размеры прямоугольника</param>
        public RectangleF(Vector2F position, Vector2F size) : this(position, size, new Vector2F(0.5f, 0.5f)) { }

        /// <summary>
        /// RectangleF на основе левого нижнего угла и размера
        /// </summary>
        /// <param name="position">Абсолютная позиция левого нижнего угла прямоугольника</param>
        /// <param name="size">Размеры прямоугольника</param>
        /// <param name="pivot">Пивот прямоугольника</param>
        public RectangleF(Vector2F position, Vector2F size, Vector2F pivot)
        {
            Pivot = pivot;

            X = position.X + size.X * Pivot.X;
            Y = position.Y + size.Y * Pivot.Y;

            Width = size.X;
            Height = size.Y;
        }



        /// <summary>
        /// Изменяет ширину и высоту, умножая их на коэффициент,
        /// и возвращает получившийся прямоугольник
        /// </summary>
        public RectangleF Resize(float sizeFactor)
        {
            RectangleF result = this;
            result.Width *= sizeFactor;
            result.Height *= sizeFactor;
            return result;
        }

        /// <summary>
        /// Изменяет ширину и высоту, умножая их на соответствующие
        /// относительные значения, и возвращает получившийся прямоугольник
        /// </summary>
        public RectangleF Resize(float widthFactor, float heightFactor)
        {
            RectangleF result = this;
            result.Width *= widthFactor;
            result.Height *= heightFactor;
            return result;
        }

        /// <summary>
        /// Возвращает массив точек, представляющих собой 4 угла данной области
        /// </summary>
        public Vector2F[] GetPoints()
        {
            return new Vector2F[] {
                LeftTop,
                LeftBottom,
                RightBottom,
                RightTop
            };
        }

        /// <summary>
        /// Возвращает массив точек, представляющих собой 4 угла данной области 
        /// с заданным отступом по направлению от центра
        /// </summary>
        /// <param name="offset">
        /// Отступ. Положительный - точки будут дальше от центра, чем реальные углы.
        /// Отрицательный - точки будут ближе к центру, чем реальные углы
        /// </param>
        public Vector2F[] GetPoints(float offset)
        {
            return new Vector2F[] {
                new Vector2F(Left - offset, Top - offset),
                new Vector2F(Left - offset, Bottom + offset),
                new Vector2F(Right + offset, Bottom + offset),
                new Vector2F(Right + offset, Top - offset)
            };
        }
    }
}
