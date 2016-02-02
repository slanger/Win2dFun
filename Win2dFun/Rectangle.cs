namespace Win2dFun
{
	internal class Rectangle
	{
		public float X { get; set; }
		public float Y { get; set; }
		public float Width { get; set; }
		public float Height { get; set; }

		public Rectangle(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}

		public static bool RectanglesIntersect(Rectangle rectA, Rectangle rectB)
		{
			// TODO Epsilon
			return
				rectA.X <= rectB.X + rectB.Width &&
				rectA.X + rectA.Width >= rectB.X &&
				rectA.Y <= rectB.Y + rectB.Height &&
				rectA.Y + rectA.Height >= rectB.Y;
		}
	}
}
