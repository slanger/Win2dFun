using Microsoft.Graphics.Canvas;
using Windows.Foundation;
using Windows.UI;

namespace Win2dFun
{
	internal struct RectCollider : IDrawable, ICollidable
	{
		private Rect collider;

		public RectCollider(double x, double y, double width, double height)
		{
			this.collider = new Rect(x, y, width, height);
		}

		public Rect GetCollider()
		{
			return this.collider;
		}

		public bool IsColliding(ICollidable other)
		{
			return RectsIntersect(this.collider, other.GetCollider());
		}

		public void Draw(CanvasDrawingSession cds)
		{
			cds.DrawRectangle(this.collider, Colors.Black);
		}

		public static bool RectsIntersect(Rect rectA, Rect rectB)
		{
			/*
			System.Diagnostics.Debug.WriteLine("rectA.Left < rectB.Right: " + (rectA.Left < rectB.Right));
			System.Diagnostics.Debug.WriteLine("rectA.Right > rectB.Left: " + (rectA.Right > rectB.Left));
			System.Diagnostics.Debug.WriteLine("rectA.Top < rectB.Bottom: " + (rectA.Top < rectB.Bottom));
			System.Diagnostics.Debug.WriteLine("rectA.Bottom > rectB.Top: " + (rectA.Bottom > rectB.Top));
			*/

			// TODO Epsilon
			return
				rectA.Left < rectB.Right &&
				rectA.Right > rectB.Left &&
				rectA.Top < rectB.Bottom &&
				rectA.Bottom > rectB.Top;
		}
	}
}
